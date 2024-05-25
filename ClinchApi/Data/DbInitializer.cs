using ClinchApi.Entities;
using Microsoft.AspNetCore.Identity;

namespace ClinchApi.Data;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context, IServiceProvider serviceProvider)
    {
            // Add address if no address is found in the db
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

            // Add categories if no category is found in the db
        if (!context.Categories.Any())
        {
            var uncategorized = new Category { Name = "Uncategorized" };
            var phonesCategory = new Category { Name = "Phones" };
            var laptopsCategory = new Category { Name = "Laptops" };
            var gadgetsCategory = new Category { Name = "Gadgets" };
            var foodCategory = new Category { Name = "Food" };
            var fashionCategory = new Category { Name = "Fashion" };
            var watchesCategory = new Category { Name = "Watches" };
            var electronicsCategory = new Category { Name = "Electronics" };

            await context.Categories.AddRangeAsync(
                uncategorized,
                phonesCategory,
                laptopsCategory,
                gadgetsCategory,
                foodCategory,
                fashionCategory,
                watchesCategory,
                electronicsCategory);

            context.SaveChanges();
        }

            // Add products if no product is found in the db
        if (!context.Products.Any())
        {
            var products = new Product[]
            {
                new()
            {
                Name = "Vivo Z10",
                Description = "A vivo phone",
                Price = 13500,
                Quantity = 15,
                CategoryId = [4, 2],
                ImageUrl = new Uri("https://your-image-url.com/product1.jpg"),
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
                new()
            {
                Name = "Samsung Galaxy S10+",
                Description = "A samsung galaxy",
                Price = 155000,
                Quantity = 4,
                CategoryId = [4, 2],
                ImageUrl = new Uri("https://your-image-url.com/product2.jpg"),
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
                new()
            {
                Name = "Phillips Iron",
                Description = "Iron your clothes",
                Price = 23000,
                Quantity = 8,
                CategoryId = [8],
                ImageUrl = new Uri("https://your-image-url.com/product3.jpg"),
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
                new()
            {
                Name = "Classic Rolex",
                Description = "Tell time easily",
                Price = 1350000,
                Quantity = 3,
                CategoryId = [6, 7],
                ImageUrl = new Uri("https://your-image-url.com/product4.jpg"),
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
                new()
            {
                Name = "RM Jersey",
                Description = "High quality jersey",
                Price = 7800,
                Quantity = 35,
                CategoryId = [6],
                ImageUrl = new Uri("https://your-image-url.com/product5.jpg"),
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
                new()
            {
                Name = "Lenovo Predator",
                Description = "A gaming laptop",
                Price = 260000,
                Quantity = 6,
                CategoryId = [3, 4],
                ImageUrl = new Uri("https://your-image-url.com/product6.jpg"),
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
                new()
            {
                Name = "BUA Rice",
                Description = "All natural rice",
                Price = 93500,
                Quantity = 100,
                CategoryId = [5],
                ImageUrl = new Uri("https://your-image-url.com/product7.jpg"),
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
                new()
            {
                Name = "SEGA Game controller",
                Description = "Game controller",
                Price = 2500,
                Quantity = 20,
                CategoryId = [4],
                ImageUrl = new Uri("https://your-image-url.com/product8.jpg"),
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
                new()
            {
                Name = "HP Yoga",
                Description = "Slim laptop",
                Price = 240000,
                Quantity = 8,
                CategoryId = [4, 3],
                ImageUrl = new Uri("https://your-image-url.com/product9.jpg"),
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            },
                new()
                {
                    Name = "A4 Paper",
                    Description = "Save the jungle",
                    Price = 3500,
                    Quantity = 40,
                    CategoryId = [],
                    ImageUrl = new Uri("https://your-image-url.com/product19.jpg"),
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                }
            };

            //For each of the products, fetch the respective categories from the db
            //and add it to the product
            foreach (var product in products)
            {
                product.Categories = [];

                //If product does not belong to any category, add it to uncategorized
                if (product.CategoryId.Count == 0)
                {
                    product.CategoryId.Add(1);
                }

                foreach (var id in product.CategoryId)
                {
                    var category = context.Categories.Find(id);
                    product.Categories.Add(category!);
                }
            }

            await context.Products.AddRangeAsync(products);
            context.SaveChanges();
        }

        // 
        using (var scope = serviceProvider.CreateScope())
        {
            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole<int>>>();

            // Seed roles
            string[] roleNames = { "Admin", "Store Owner", "Store Manager", "Customer" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(roleName));
                }
            }

            // Seed default admin user
            var adminUserEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
            if (adminUser is null)
            {
                var user = new ApplicationUser
                {
                    UserGuid = Guid.NewGuid(),
                    UserName = adminUserEmail,
                    Email = adminUserEmail,
                    FirstName = "Ibrahim",
                    LastName = "Suleiman",
                    PhoneNumber = "08143660104",
                    Gender = Gender.Male,
                    DateOfBirth = new DateOnly(1999, 1, 4),
                    UserAddressId = 1
                };
                var result = await userManager.CreateAsync(user, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
