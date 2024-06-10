using ClinchApi.Data;

namespace ClinchApi.Extensions;

public static class IfNotExists
{
    public static async void CreateDb(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services
            .GetRequiredService<ApplicationDbContext>();

        //// Ensure database is deleted and created anew
        //context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Seed the database with initial data, including roles and admin user
        await DbInitializer.Initialize(context, services);
    }
}
