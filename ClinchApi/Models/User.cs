namespace ClinchApi.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    
    // Navigation properties for addresses
    public int UserAddressId { get; set; }
    public virtual Address UserAddress { get; set; }

    public int BillingAddressId { get; set; }
    public virtual Address BillingAddress { get; set; }

    public int ShippingAddressId { get; set; }
    public virtual Address ShippingAddress { get; set; }

    public UserRole UserRole { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
}
public enum UserRole
{
    Customer,
    Admin
}
