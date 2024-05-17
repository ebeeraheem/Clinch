using ClinchApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinchApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : 
        base(options)
    {        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }

    
    //Specify the decimal precision for all decimal properties to avoid silent truncating
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var decimalProps = modelBuilder.Model
        .GetEntityTypes()
        .SelectMany(t => t.GetProperties())
        .Where(p => (Nullable.GetUnderlyingType(p.ClrType) ?? 
            p.ClrType) == typeof(decimal));

        foreach (var property in decimalProps)
        {
            property.SetPrecision(18);
            property.SetScale(2);
        }
    }
}
