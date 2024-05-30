namespace ClinchApi.Entities;

public class ShoppingCart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<ShoppingCartItem> ShoppingCartItems { get; set; } = [];
}
