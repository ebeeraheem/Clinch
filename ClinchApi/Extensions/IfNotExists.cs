using ClinchApi.Data;

namespace ClinchApi.Extensions;

public static class IfNotExists
{
    public static void CreateDb(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services
            .GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        DbInitializer.Initialize(context);
    }
}
