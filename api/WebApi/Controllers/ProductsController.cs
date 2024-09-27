using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.RequestHelpers;

namespace Api.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery]ProductSpecParams specParams) 
    {
        var spec = new ProductSpecification(specParams);

        var products = await _unitOfWork.Repository<Product>().ListAsync(spec);

        var count = await _unitOfWork.Repository<Product>().CountAsync(spec);

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
        var brands = await _unitOfWork.Repository<Product>().ListAsync(spec);
        return Ok(brands);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();
        var types = await _unitOfWork.Repository<Product>().ListAsync(spec);
        return Ok(types);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        Product? product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);

        if (product == null) return NotFound();

        return Ok(product);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromForm] CreateProductDTO productDto)
    {
        if (ModelState.IsValid)
        {
            if(productDto.File is not null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(productDto.File.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\products");


                // Supression de l'image lors d'un update
                if (!string.IsNullOrEmpty(productDto.PictureUrl))
                {
                    var oldImage = Path.Combine(wwwRootPath, productDto.PictureUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImage))
                        System.IO.File.Delete(oldImage);
                }

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    productDto.File.CopyTo(fileStream);
                }

                Product product = new()
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    Brand = productDto.Brand,
                    Type = productDto.Type,
                    QuantityInStock = productDto.QuantityInStock,
                    PictureUrl = @"images\products" + fileName
                };

                _unitOfWork.Repository<Product>().Add(product);

                if (!await _unitOfWork.SaveChanges())
                    return BadRequest("Problème lors de la création d'un produit");

                return CreatedAtAction(nameof(GetProduct), new { product.Id }, product);

            }
            
        }

        return BadRequest("Problème lors de la création du produit");
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct([FromBody] Product product, int id)
    {
        if (!_unitOfWork.Repository<Product>().Exists(id))
            return NotFound();

        _unitOfWork.Repository<Product>().Update(product);

        if (!await _unitOfWork.SaveChanges())
            return BadRequest("Problème lors de la mise à jour d'un produit");

        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);

        if (product is null) return NotFound();

        _unitOfWork.Repository<Product>().Delete(product);

        if(!await _unitOfWork.SaveChanges()) return BadRequest("Problème lors de la suppresion d'un produit");

        return Ok();
    }
}
