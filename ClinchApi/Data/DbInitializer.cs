using ClinchApi.Models;
using ClinchApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ClinchApi.Data;

public static class DbInitializer
{
    public static async void Initialize(ApplicationDbContext context)
    {
		if (!context.Addresses.Any())
		{
			var rZaki = new Address()
			{
				StreetAddress = "Layin Mahalli, Rijiyar Zaki",
				City = "Kano",
				State = "Kano State",
				Country = "Nigeria",
				PostalCode = "700001",
				IsBillingAddress = true,
				IsShippingAddress = false
			};
			var dorayi = new Address()
			{
                StreetAddress = "Layin Masallacin Dan Sarari",
                City = "Kano",
                State = "Kano State",
                Country = "Nigeria",
                PostalCode = "700001",
                IsBillingAddress = true,
                IsShippingAddress = true
            };
			var abuja = new Address()
			{
                StreetAddress = "Maitama Street",
                City = "Abuja",
                State = "FCT",
                Country = "Nigeria",
                PostalCode = "700421",
                IsBillingAddress = false,
                IsShippingAddress = false
            };

			context.Addresses.AddRange(rZaki, dorayi, abuja);
        }

		if (!context.Categories.Any())
		{
            var phonesCategory = new Category { Name = "Phones" };
            var laptopsCategory = new Category { Name = "Laptops" };
            var gadgetsCategory = new Category { Name = "Gadgets" };
            var foodCategory = new Category { Name = "Food" };
            var fashionCategory = new Category { Name = "Fashion" };
            var watchesCategory = new Category { Name = "Watches" };
            var electronicsCategory = new Category { Name = "Electronics" };

            await context.Categories.AddRangeAsync(
                phonesCategory, 
                laptopsCategory,
                gadgetsCategory,
                foodCategory, 
                fashionCategory,
                watchesCategory,
                electronicsCategory); 

            context.SaveChanges();
        }

		if (!context.Products.Any())
		{
            var products = new Product[]
            {
                new Product()
            {
                Name = "Vivo Z10",
                Description = "A vivo phone",
                Price = 13500,
                Quantity = 15,
                CategoryId = new List<int> { 3, 1 },
                ImageUrl = new Uri("https://your-image-url.com/product1.jpg"),
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
                new Product()
            {
                Name = "Samsung Galaxy S10+",
                Description = "A samsung galaxy",
                Price = 155000,
                Quantity = 4,
                CategoryId = new List<int> { 3, 1 },
                ImageUrl = new Uri("https://your-image-url.com/product2.jpg"),
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
                new Product()
            {
                Name = "Phillips Iron",
                Description = "Iron your clothes",
                Price = 23000,
                Quantity = 8,
                CategoryId = new List<int> { 7 },
                ImageUrl = new Uri("https://your-image-url.com/product3.jpg"),
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
                new Product()
            {
                Name = "Classic Rolex",
                Description = "Tell time easily",
                Price = 1350000,
                Quantity = 3,
                CategoryId = new List<int> { 5, 6 },
                ImageUrl = new Uri("https://your-image-url.com/product4.jpg"),
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
                new Product()
            {
                Name = "RM Jersey",
                Description = "High quality jersey",
                Price = 7800,
                Quantity = 35,
                CategoryId = new List<int> { 5 },
                ImageUrl = new Uri("https://your-image-url.com/product5.jpg"),
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
                new Product()
            {
                Name = "Lenovo Predator",
                Description = "A gaming laptop",
                Price = 260000,
                Quantity = 6,
                CategoryId = new List<int> { 2, 3 },
                ImageUrl = new Uri("https://your-image-url.com/product6.jpg"),
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
                new Product()
            {
                Name = "BUA Rice",
                Description = "All natural rice",
                Price = 93500,
                Quantity = 100,
                CategoryId = new List<int> { 4 },
                ImageUrl = new Uri("https://your-image-url.com/product7.jpg"),
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
                new Product()
            {
                Name = "SEGA Game controller",
                Description = "Game controller",
                Price = 2500,
                Quantity = 20,
                CategoryId = new List<int> { 3 },
                ImageUrl = new Uri("https://your-image-url.com/product8.jpg"),
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
                new Product()
            {
                Name = "HP Yoga",
                Description = "Slim laptop",
                Price = 240000,
                Quantity = 8,
                CategoryId = new List<int> { 3, 2 },
                ImageUrl = new Uri("https://your-image-url.com/product9.jpg"),
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            }
        };

            //For each of the products, fetch the respective categories from the db
            //and add it to the product
            foreach (var product in products)
            {
                product.Categories = new();
                foreach (var id in product.CategoryId)
                {
                    var category = context.Categories.Find(id);
                    product.Categories.Add(category);
                }
            }

            await context.Products.AddRangeAsync(products);
            context.SaveChanges();
        }
    }
}
