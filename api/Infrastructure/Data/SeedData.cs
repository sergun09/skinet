using Core.Entities;
using System.Text.Json;

namespace Infrastructure.Data;

public class SeedData
{
    public static async Task SeedAsync(StoreContext context) 
    {
        if(!context.Products.Any()) 
        {
            var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);

            if (products is null) return;

            context.Products.AddRange(products);

            await context.SaveChangesAsync();
        }

        if (!context.DeliveryMethods.Any())
        {
            var deliveryData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/delivery.json");
            var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);

            if (deliveryMethods is null) return;

            context.DeliveryMethods.AddRange(deliveryMethods);

            await context.SaveChangesAsync();
        }
    }
}
