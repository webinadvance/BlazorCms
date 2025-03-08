using BlazorApp2.Data.Models;
using Microsoft.EntityFrameworkCore;
namespace BlazorApp2.Data.Services;
public class BookingService
{
    private readonly ApplicationDbContext _context;
    public BookingService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Resources
    public async Task<List<Resource>> GetResourcesAsync()
    {
        return await _context.Resources.Include(r => r.ResourceType)
            .ToListAsync();
    }
    public async Task<Resource?> GetResourceByIdAsync(int id)
    {
        return await _context.Resources.Include(r => r.ResourceType)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    // Resource Types
    public async Task<List<ResourceType>> GetResourceTypesAsync()
    {
        return await _context.ResourceTypes.ToListAsync();
    }

    // Customers
    public async Task<List<Customer>> GetCustomersAsync()
    {
        return await _context.Customers.ToListAsync();
    }
    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        return await _context.Customers.Include(c => c.Bookings)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    public async Task<Customer> AddCustomerAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    // Bookings
    public async Task<List<Booking>> GetBookingsAsync()
    {
        return await _context.Bookings.Include(b => b.Customer)
            .Include(b => b.Resource)
            .ToListAsync();
    }
    public async Task<Booking?> GetBookingByIdAsync(int id)
    {
        return await _context.Bookings.Include(b => b.Customer)
            .Include(b => b.Resource)
            .FirstOrDefaultAsync(b => b.Id == id);
    }
    public async Task<List<Booking>> GetBookingsByResourceIdAsync(int resourceId)
    {
        return await _context.Bookings.Where(b => b.ResourceId == resourceId)
            .Include(b => b.Customer)
            .ToListAsync();
    }
    public async Task<List<Booking>> GetBookingsByCustomerIdAsync(int customerId)
    {
        return await _context.Bookings.Where(b => b.CustomerId == customerId)
            .Include(b => b.Resource)
            .ToListAsync();
    }
    public async Task<Booking> CreateBookingAsync(Booking booking)
    {
        // Check availability
        var conflictingBookings = await _context.Bookings.Where(b =>
                b.ResourceId == booking.ResourceId && b.Status != BookingStatus.Cancelled &&
                ((b.StartTime <= booking.StartTime && b.EndTime > booking.StartTime) ||
                 (b.StartTime < booking.EndTime && b.EndTime >= booking.EndTime) ||
                 (b.StartTime >= booking.StartTime && b.EndTime <= booking.EndTime)))
            .AnyAsync();
        if (conflictingBookings)
            throw new InvalidOperationException("The resource is not available during the selected time period.");

        // Calculate price based on duration and resource rates
        var resource = await _context.Resources.FindAsync(booking.ResourceId);
        if (resource != null)
        {
            var duration = booking.EndTime - booking.StartTime;
            if (duration.TotalHours <= 24 && resource.HourlyRate.HasValue)
            {
                booking.TotalPrice = (decimal)duration.TotalHours * resource.HourlyRate.Value;
            }
            else if (resource.DailyRate.HasValue)
            {
                var days = Math.Ceiling(duration.TotalDays);
                booking.TotalPrice = (decimal)days * resource.DailyRate.Value;
            }
        }
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        return booking;
    }
    public async Task<Booking?> UpdateBookingStatusAsync(int id, BookingStatus status)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null) return null;
        booking.Status = status;
        await _context.SaveChangesAsync();
        return booking;
    }
}