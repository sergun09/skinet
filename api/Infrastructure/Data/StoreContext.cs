using Core.Entities;
using Infrastructure.Config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class StoreContext : IdentityDbContext<AppUser>
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Adress> Adresses { get; set; }

    public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
    public StoreContext(DbContextOptions options) : base(options)
    { 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
    }
}
