using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos;
using WebApi.RequestHelpers;

namespace Api.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IGenericRepository<Product> _productRepository;

    public ProductsController(IGenericRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }


    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery]ProductSpecParams specParams) 
    {
        var spec = new ProductSpecification(specParams);

        var products = await _productRepository.ListAsync(spec);

        var count = await _productRepository.CountAsync(spec);

        var pagination = new Pagination<Product>
        {
            PageIndex = specParams.PageIndex,
            PageSize = specParams.PageSize,
            Count = count,
            Data = products
        };

        return Ok(pagination);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();
        var brands = await _productRepository.ListAsync(spec);
        return Ok(brands);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();
        var types = await _productRepository.ListAsync(spec);
        return Ok(types);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        Product? product = await _productRepository.GetByIdAsync(id);

        if (product == null) return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        _productRepository.Add(product);

        if(!await _productRepository.SaveAllAsync())
            return BadRequest("Problème lors de la création d'un produit");

        return CreatedAtAction(nameof(GetProduct), new { product.Id }, product);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct([FromBody] Product product, int id)
    {
        if (!_productRepository.Exists(id))
            return NotFound();

        _productRepository.Update(product);

        if (!await _productRepository.SaveAllAsync())
            return BadRequest("Problème lors de la mise à jour d'un produit");

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null) return NotFound();

        _productRepository.Delete(product);

        if(!await _productRepository.SaveAllAsync()) return BadRequest("Problème lors de la mise à jour d'un produit");

        return Ok();
    }
}
