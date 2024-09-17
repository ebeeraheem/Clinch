using ClinchApi.Data;
using ClinchApi.Entities;
using ClinchApi.Extensions;
using ClinchApi.StartupConfigs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if (builder.Environment.IsProduction())
{
    // For production
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ClinchMonsterDb"))
        .LogTo(Console.WriteLine, LogLevel.Information));
}
else
{
    // For development or testing
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
        .LogTo(Console.WriteLine, LogLevel.Information));
}

builder.Services.AddControllers();
builder.AddCustomServices();
builder.AddAuthServices();
builder.AddSwaggerConfig();

var app = builder.Build();

// Seed the database before running the app
app.CreateDb();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<ApplicationUser>();

app.MapControllers();

app.Run();
