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

        ////NOTE: Any item that is out of stock should have the AddToCart endpoint disabled
        //if (productToAdd.Quantity == 0)
        //{
        //    throw new InvalidOperationException("Product out of stock");
        //}

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

        ////Check if item already exists in the shoppingCart
        ////(NOTE: any item that is already in the cart should not have the AddToCart endpoint available)
        //var existingItem = shoppingCart.ShoppingCartItems.FirstOrDefault(item => item.ProductId == productId);

        //if (existingItem is null)
        //{
        //    shoppingCart.ShoppingCartItems.Add(new ShoppingCartItem()
        //    {
        //        ShoppingCartId = shoppingCart.Id,
        //        ProductId = productId,
        //        Product = productToAdd,
        //        Quantity = 1,
        //        UnitPrice = productToAdd.Price
        //    });
        //}
        //else
        //{
        //    //NOTE: the AddToCart endpoint should NOT be available
        //    //to products that are already in the cart!!
        //    existingItem.Quantity = existingItem.Quantity + 1;
        //}

        //Create the item to add to the cart
        var itemToAdd = new ShoppingCartItem()
        {
            ShoppingCartId = shoppingCart.Id,
            ProductId = productId,
            Product = productToAdd,
            Quantity = 1,
            UnitPrice = productToAdd.Price
        };

        //Add the item to the cart
        shoppingCart.ShoppingCartItems.Add(itemToAdd);

        await _context.SaveChangesAsync();

        return shoppingCart;
    }

    //Increase quantity (Some products might have max purchase limit)
    public async Task<ShoppingCart> IncreaseQuantity(int userId, int productId)
    {
        using(var transaction = _context.Database.BeginTransaction())
        {
            //Get the shopping cart based on the userId
            var shoppingCart = _context.ShoppingCarts
                .SingleOrDefault(c => c.UserId == userId);

            //Get the item to update from the cart
            var itemToUpdate = shoppingCart.ShoppingCartItems.FirstOrDefault(item => item.ProductId == productId);

            //Check the quantity of the product available
            var product = await _context.Products
                .SingleOrDefaultAsync(p => p.Id == productId);
            if (product.Quantity == 0)
            {
                throw new InvalidOperationException("Product out of stock");
            }

            //Increase the quantity of the item by one
            itemToUpdate.Quantity++;

            ////Decrease the quantity of the product by one
            ////NOTE: it's not good to decrease the quantity of a product
            ////just because somebody added it to their cart
            ////because they might not buy it at all
            //product.Quantity--;

            await _context.SaveChangesAsync();
            transaction.Commit();

            return shoppingCart;
        }
    }

    //Decrease quantity (remove from cart if quantity = 0)
    public async Task<ShoppingCart> DecreaseQuantity(int userId, int productId)
    {
        //Get the shopping cart based on the userId
        var shoppingCart = _context.ShoppingCarts
            .SingleOrDefault(c => c.UserId == userId);

        //Get the item to update from the cart
        var itemToUpdate = shoppingCart.ShoppingCartItems.FirstOrDefault(item => item.ProductId == productId);

        if (itemToUpdate.Quantity == 1)
        {
            shoppingCart.ShoppingCartItems.Remove(itemToUpdate);
        }
        else
        {
            itemToUpdate.Quantity--;
        }
        
        await _context.SaveChangesAsync();

        return shoppingCart;
    }

    //Remove from cart
    public async Task<ShoppingCart> RemoveFromCart(int userId, int productId)
    {
        //Get the shopping cart based on the userId
        var shoppingCart = _context.ShoppingCarts
            .SingleOrDefault(c => c.UserId == userId);

        //Get the item to remove from the shopping cart
        var itemToRemove = shoppingCart.ShoppingCartItems.FirstOrDefault(item => item.ProductId == productId);

        //Remove the item
        shoppingCart.ShoppingCartItems.Remove(itemToRemove);
        
        await _context.SaveChangesAsync();

        return shoppingCart;
    }

    //Clear cart
    public async Task<ShoppingCart> ClearCart(int userId)
    {
        //Get the shopping cart based on the userId
        var shoppingCart = _context.ShoppingCarts
            .SingleOrDefault(c => c.UserId == userId);

        //Get all the items in the cart
        var cartItems = shoppingCart.ShoppingCartItems;

        //Remove all the items
        foreach (var item in cartItems)
        {
            shoppingCart.ShoppingCartItems.Remove(item);
        }

        await _context.SaveChangesAsync();

        return shoppingCart;
    }
}
