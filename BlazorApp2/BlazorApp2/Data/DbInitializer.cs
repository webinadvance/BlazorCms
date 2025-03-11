using BlazorApp2.Data.Models;
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

        // Seed data if no bookings exist
        if (!await context.Bookings.AnyAsync())
        {
            // Add a customer if none exists
            if (!await context.Customers.AnyAsync())
            {
                var customer = new Customer { Name = "John Doe", Email = "john@example.com", Phone = "555-123-4567" };
                context.Customers.Add(customer);
                await context.SaveChangesAsync();
            }
            var customerId = await context.Customers.Select(c => c.Id)
                .FirstAsync();
            var resources = await context.Resources.ToListAsync();
            if (resources.Any())
            {
                // Add sample bookings
                var bookings = new List<Booking>
                {
                    new()
                    {
                        StartTime = DateTime.Now.AddDays(1),
                        EndTime = DateTime.Now.AddDays(1)
                            .AddHours(2),
                        Status = BookingStatus.Confirmed,
                        Notes = "Sample booking 1",
                        CustomerId = customerId,
                        ResourceId = resources[0].Id,
                        TotalPrice = resources[0].HourlyRate.HasValue ? resources[0].HourlyRate.Value * 2 : 0
                    },
                    new()
                    {
                        StartTime = DateTime.Now.AddDays(2),
                        EndTime = DateTime.Now.AddDays(2)
                            .AddHours(3),
                        Status = BookingStatus.Pending,
                        Notes = "Sample booking 2",
                        CustomerId = customerId,
                        ResourceId = resources.Count > 1 ? resources[1].Id : resources[0].Id,
                        TotalPrice = resources[0].HourlyRate.HasValue ? resources[0].HourlyRate.Value * 3 : 0
                    }
                };
                context.Bookings.AddRange(bookings);
                await context.SaveChangesAsync();
            }
        }
    }
}