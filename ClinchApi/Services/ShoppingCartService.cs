using ClinchApi.Data;
using ClinchApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinchApi.Services;

public class ShoppingCartService
{
    private readonly ApplicationDbContext _context;

    public ShoppingCartService(ApplicationDbContext context)
    {
        _context = context;
    }

    //Add to cart
    public async Task<ShoppingCart> AddToCart(int userId, int productId)
    {
        //Get the product to add to the cart
        var productToAdd = await _context.Products.AsNoTracking()
            .Include(p => p.Categories)
            .SingleOrDefaultAsync(p => p.Id == productId);

        if (productToAdd is null)
        {
            throw new InvalidOperationException($"Product with id {productId} not found");
        }

        //Any item that is out of stock should have the AddToCart endpoint disabled
        if (productToAdd.Quantity == 0)
        {
            throw new InvalidOperationException("Product out of stock");
        }

        //Get the shopping cart based on the user ID
        var shoppingCart = _context.ShoppingCarts
            .SingleOrDefault(c => c.UserId == userId);

        if (shoppingCart is null)
        {
            //Create a new cart if it doesn't exist
            shoppingCart = new()
            {
                UserId = userId,
                ShoppingCartItems = new(),
                CreatedAt = DateTime.UtcNow
            };
        }

        //Check if item already exists in the shoppingCart
        //(although any item that is already in the cart should not have the AddToCart endpoint available)
        var existingItem = shoppingCart.ShoppingCartItems.FirstOrDefault(item => item.ProductId == productId);

        if (existingItem is null)
        {
            shoppingCart.ShoppingCartItems.Add(new ShoppingCartItem()
            {
                ShoppingCartId = shoppingCart.Id,
                ProductId = productId,
                Product = productToAdd,
                Quantity = 1,
                UnitPrice = productToAdd.Price
            });
        }
        else
        {
            //I should point out that the AddToCart endpoint should NOT be
            //available to products that are already in the cart!!
            existingItem.Quantity = existingItem.Quantity + 1;
        }

        await _context.SaveChangesAsync();

        return shoppingCart;
    }

    //Increase quantity (Some products might have max purchase limit)
    public async Task<ShoppingCart> IncreaseQuantity(int userId, int productId)
    {
        //This should NEVER be null because having the IncreaseQuantity endpoint means
        //the product already exists in the cart
        var productToUpdate = await _context.Products.AsNoTracking()
            .Include(p => p.Categories)
            .SingleOrDefaultAsync(p => p.Id == productId);

        //This should also NEVER be null for the same reason as above
        //Get the shopping cart based on the userId
        var shoppingCart = _context.ShoppingCarts
            .SingleOrDefault(c => c.UserId == userId);

        var itemToUpdate = shoppingCart.ShoppingCartItems.FirstOrDefault(item => item.ProductId == productId);

        //Increase the quantityof the item by one
        itemToUpdate.Quantity++;

        await _context.SaveChangesAsync();

        return shoppingCart;
    }

    //Decrease quantity (remove from cart if quantity = 0)

    //Remove from cart
}
