using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly StoreContext _context;

    public ProductRepository(StoreContext context)
    {
        _context = context;
    }

    public void AddProduct(Product product)
    {
        _context.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        _context.Products.Remove(product);
    }

    public bool DoesProductExist(int id)
    {
        return _context.Products.Any(x => x.Id == id);
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        return await _context.Products.Select(x => x.Brand)
            .Distinct()
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(brand)) query = query.Where(x => x.Brand == brand);
        if (!string.IsNullOrWhiteSpace(type)) query = query.Where(x => x.Type == type);
        if (!string.IsNullOrWhiteSpace(sort)) 
        {
            query = sort switch
            {
                "priceAsc" => query.OrderBy(p => p.Price),
                "priceDesc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            }; ;
        }

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
        return await _context.Products.Select(x => x.Type)
            .Distinct()
            .ToListAsync();
    }

    public void UpdateProduct(Product product)
    {
        _context.Products.Update(product);
    }
}
