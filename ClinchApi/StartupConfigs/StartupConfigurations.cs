using ClinchApi.Services.Interfaces;
using ClinchApi.Services;
using System.Runtime.CompilerServices;
using Swashbuckle.AspNetCore.Filters;
using ClinchApi.Data;
using ClinchApi.Entities;
using Microsoft.AspNetCore.Identity;

namespace ClinchApi.StartupConfigs;

public static class StartupConfigurations
{
    public static void AddCustomServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAddressService, AddressService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
        builder.Services.AddScoped<ICheckoutService, CheckoutService>();
        builder.Services.AddScoped<PaymentService>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        builder.Services.AddScoped<IUserService, UserService>();
    }

    public static void AddSwaggerConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
            {
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Enter 'Bearer' followed by a space and the JWT token."
            });
            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });
    }

    public static void AddAuthServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentityApiEndpoints<ApplicationUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 8;
        })
        .AddRoles<IdentityRole<int>>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddEndpointsApiExplorer();
    }
}
