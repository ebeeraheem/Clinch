using ClinchApi.Entities;

namespace ClinchApi.Services.Interfaces
{
    public interface IShoppingCartService
    {
        Task<ShoppingCart> AddToCart(int userId, int productId);
        Task<ShoppingCart> ClearCart(int userId);
        Task<ShoppingCart> DecreaseQuantity(int userId, int productId);
        Task<List<ShoppingCartItem>> GetCart(int userId);
        Task<ShoppingCart> IncreaseQuantity(int userId, int productId);
        Task<ShoppingCart> RemoveFromCart(int userId, int productId);
    }
}