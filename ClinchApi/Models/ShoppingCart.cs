namespace ClinchApi.Models;

public class ShoppingCart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<int>? ShoppingCartItemIds { get; set; }
    public List<ShoppingCartItem>? ShoppingCartItems { get; set; }
    public DateTime CreatedAt { get; set; }
}
