using ClinchApi.Entities;

namespace ClinchApi.Extensions;

public static class ShoppingCartCreator
{
    public static ShoppingCart CreateShoppingCart(int userId)
    {
        return new()
        {
            UserId = userId,
            ShoppingCartItems = []
        };
    }
}
