using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> Users => Set<User>();
    public DbSet<ShoppingListItem> ShoppingListItems => Set<ShoppingListItem>();

    public static async Task SeedDataAsync(AppDbContext context)
    {
        if (context.Products.Any())
            return;

        var products = new List<Product>
    {
        new Product { Name = "Bread", Weight = 1, Quantity = 11 },
        new Product { Name = "Pinnaple", Weight = 3, Quantity = 45 },
        new Product { Name = "Sugar", Weight = 5, Quantity = 12 },
        new Product { Name = "Butter", Weight = 0.5, Quantity = 1002 },
        new Product { Name = "Ice cream", Weight = 0.3, Quantity = 309 },
        new Product { Name = "Wooden log", Weight = 22, Quantity = 33 },
        new Product { Name = "Cookie", Weight = 0.2, Quantity = 100 },
        new Product { Name = "Orange juice", Weight = 2, Quantity = 130 },
        new Product { Name = "Rice", Weight = 1.6, Quantity = 44 },
        new Product { Name = "Sparkling water", Weight = 5, Quantity = 222 }
    };

        context.Products.AddRange(products);
        await context.SaveChangesAsync();
    }
}