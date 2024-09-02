using Infrastructure.Data;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Core.Entities;

namespace WebApi.Extensions
{
    public  static class WebApplicationExtensions
    {
        public async static Task MigrateAsync(this WebApplication webApplication)
        {
            try
            {
                using var scope = webApplication.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                await context.Database.MigrateAsync();
                await SeedData.SeedAsync(context, userManager);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
