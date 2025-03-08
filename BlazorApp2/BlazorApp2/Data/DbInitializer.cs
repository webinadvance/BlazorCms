using Microsoft.EntityFrameworkCore;
namespace BlazorApp2.Data;
public static class DbInitializer
{
    public static async Task InitializeAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Apply migrations and create database if it doesn't exist
        await context.Database.MigrateAsync();
    }
}