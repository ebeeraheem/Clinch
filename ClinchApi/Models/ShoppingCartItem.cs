using System.Text.Json.Serialization;

namespace ClinchApi.Models;

public class ShoppingCartItem
{
    public int Id { get; set; }
    public int ShoppingCartId { get; set; }

    [JsonIgnore]
    public virtual ShoppingCart ShoppingCart { get; set; }
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}