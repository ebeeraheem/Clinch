namespace ClinchApi.Models;

public class ShoppingCart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<ShoppingCartItem> ShoppingCartItems { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
