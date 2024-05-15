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

    //EF Core is smart enough to create the join table on its own
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<Product>()
    //        .HasMany(e => e.Categories)
    //        .WithMany(e => e.Products)
    //        .UsingEntity<ProductCategory>();
    //}

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<ShoppingCartItem>()
    //        .HasKey(sci => sci.Id);

    //    modelBuilder.Entity<ShoppingCartItem>()
    //        .HasOne(sci => sci.Product)
    //        .WithMany()
    //        .HasForeignKey(sci => sci.ProductId);

    //    modelBuilder.Entity<ShoppingCart>()
    //        .HasMany(sc => sc.ShoppingCartItems)
    //        .WithOne()
    //        .HasForeignKey(sci => sci.ShoppingCartId)
    //        .OnDelete(DeleteBehavior.Cascade); // Optional: Cascade delete if a cart is deleted
    //}

}
