using ClinchApi.Data;
using ClinchApi.Entities;
using ClinchApi.Extensions;
using ClinchApi.StartupConfigs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// For production or non-testing environment
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
    .LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddControllers();
builder.AddCustomServices();
builder.AddAuthServices();
builder.AddSwaggerConfig();

var app = builder.Build();

// Seed the database before running the app
app.CreateDb();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<ApplicationUser>();

app.MapControllers();

app.Run();
