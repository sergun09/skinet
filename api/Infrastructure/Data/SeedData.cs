﻿using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace Infrastructure.Data;

public class SeedData
{
    public static async Task SeedAsync(StoreContext context, UserManager<AppUser> userManager) 
    {

        if(!userManager.Users.Any(x => x.UserName == "admin@test.com"))
        {
            var user = new AppUser()
            {
                UserName = "admin@test.com",
                Email = "admin@test.com",
            };

            await userManager.CreateAsync(user, "Password123!");
            await userManager.AddToRoleAsync(user, "Admin");
            await context.SaveChangesAsync();
        }


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
