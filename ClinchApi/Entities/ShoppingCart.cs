namespace ClinchApi.Entities;

public class ShoppingCart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<int> ShoppingCartItemIds { get; set; } = new();
    public List<ShoppingCartItem> ShoppingCartItems { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
