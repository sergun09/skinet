using Core.Entities;

namespace Core.Interfaces;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort);
    Task<IReadOnlyList<string>> GetTypesAsync();
    Task<IReadOnlyList<string>> GetBrandsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    bool DoesProductExist(int id);
    Task<bool> SaveChangesAsync();
}
