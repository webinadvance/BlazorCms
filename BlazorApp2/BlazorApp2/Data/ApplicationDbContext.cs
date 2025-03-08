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
    public DbSet<ResourceType> ResourceTypes { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure relationships
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
            .HasOne(r => r.ResourceType)
            .WithMany(rt => rt.Resources)
            .HasForeignKey(r => r.ResourceTypeId)
            .OnDelete(DeleteBehavior.SetNull);

        // Seed data
        modelBuilder.Entity<ResourceType>()
            .HasData(new ResourceType { Id = 1, Name = "Room", Description = "Meeting or event rooms" },
                new ResourceType { Id = 2, Name = "Equipment", Description = "Rentable equipment" },
                new ResourceType { Id = 3, Name = "Service", Description = "Bookable services" });
        modelBuilder.Entity<Resource>()
            .HasData(
                new Resource
                {
                    Id = 1,
                    Name = "Conference Room A",
                    Description = "Large conference room",
                    IsAvailable = true,
                    HourlyRate = 50,
                    DailyRate = 300,
                    ResourceTypeId = 1
                },
                new Resource
                {
                    Id = 2,
                    Name = "Meeting Room B",
                    Description = "Small meeting room",
                    IsAvailable = true,
                    HourlyRate = 30,
                    DailyRate = 180,
                    ResourceTypeId = 1
                }, new Resource
                {
                    Id = 3,
                    Name = "Projector",
                    Description = "HD Projector",
                    IsAvailable = true,
                    HourlyRate = 15,
                    DailyRate = 60,
                    ResourceTypeId = 2
                });
    }
}