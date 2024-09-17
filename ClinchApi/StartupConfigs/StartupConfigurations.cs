// Ignore Spelling: Auth

using ClinchApi.Data;
using ClinchApi.Entities;
using ClinchApi.Services;
using ClinchApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace ClinchApi.StartupConfigs;

public static class StartupConfigurations
{
    public static void AddCustomServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAddressService, AddressService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
        builder.Services.AddScoped<ICheckoutService, CheckoutService>();
        builder.Services.AddScoped<IPaymentService, FakePaymentService>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        builder.Services.AddScoped<IUserService, UserService>();
    }

    public static void AddSwaggerConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Enter 'Bearer' followed by a space and the JWT token."
            });
            options.OperationFilter<SecurityRequirementsOperationFilter>();

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
               {
                   new OpenApiSecurityScheme
                   {
                       Reference = new OpenApiReference
                       {
                           Type = ReferenceType.SecurityScheme,
                           Id = "Bearer",
                       }
                   },
                   Array.Empty<string>()
               }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
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

        //// Require users of the app to be authenticated
        //builder.Services.AddAuthorizationBuilder()
        //    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
        //        .RequireAuthenticatedUser()
        //        .Build());
    }
}
