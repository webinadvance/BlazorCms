using BlazorApp2.Data.Models;
using Microsoft.EntityFrameworkCore;
namespace BlazorApp2.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Customer)
            .WithMany(c => c.Bookings)
            .HasForeignKey(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Resource)
            .WithMany(r => r.Bookings)
            .HasForeignKey(b => b.ResourceId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Resource>()
            .HasData(
                new Resource
                {
                    Id = 1,
                    Name = "Conference Room A",
                    Description = "Large conference room",
                    Category = "Room",
                    IsAvailable = true,
                    HourlyRate = 50,
                    DailyRate = 300
                }, new Resource
                {
                    Id = 2,
                    Name = "Projector",
                    Description = "HD Projector",
                    Category = "Equipment",
                    IsAvailable = true,
                    HourlyRate = 15
                });
    }
}