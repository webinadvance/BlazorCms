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
    public async Task<List<Resource>> GetResourcesAsync()
    {
        return await _context.Resources.ToListAsync();
    }
    public async Task<Resource?> GetResourceByIdAsync(int id)
    {
        return await _context.Resources.FirstOrDefaultAsync(r => r.Id == id);
    }
    public async Task<List<Resource>> GetResourcesByCategoryAsync(string category)
    {
        return await _context.Resources.Where(r => r.Category == category)
            .ToListAsync();
    }
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
        var conflictingBookings = await _context.Bookings.Where(b =>
                b.ResourceId == booking.ResourceId && b.Status != BookingStatus.Cancelled &&
                ((b.StartTime <= booking.StartTime && b.EndTime > booking.StartTime) ||
                 (b.StartTime < booking.EndTime && b.EndTime >= booking.EndTime) ||
                 (b.StartTime >= booking.StartTime && b.EndTime <= booking.EndTime)))
            .AnyAsync();
        if (conflictingBookings)
            throw new InvalidOperationException("The resource is not available during the selected time period.");
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
    
    public async Task<BookingRecurrence> CreateRecurringBookingAsync(BookingRecurrence recurrence)
    {
        _context.BookingRecurrences.Add(recurrence);
        await _context.SaveChangesAsync();
        
        var bookingDates = GenerateRecurringBookingDates(recurrence);
        var resource = await _context.Resources.FindAsync(recurrence.ResourceId);
        
        foreach (var date in bookingDates)
        {
            var booking = new Booking
            {
                StartTime = date,
                EndTime = date.Add(recurrence.Duration),
                ResourceId = recurrence.ResourceId,
                CustomerId = recurrence.CustomerId,
                Status = BookingStatus.Confirmed,
                BookingRecurrenceId = recurrence.Id
            };
            
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
        }
        
        await _context.SaveChangesAsync();
        return recurrence;
    }
    
    private List<DateTime> GenerateRecurringBookingDates(BookingRecurrence recurrence)
    {
        var dates = new List<DateTime>();
        var currentDate = recurrence.StartDate;
        
        // Determine end condition
        DateTime? endDateTime = null;
        if (recurrence.EndDate.HasValue)
        {
            endDateTime = recurrence.EndDate.Value;
        }
        else if (recurrence.OccurrenceCount.HasValue)
        {
            // For count-based recurrence, we'll calculate as we go
        }
        else
        {
            // Default to 1 year max if no end condition specified
            endDateTime = recurrence.StartDate.AddYears(1);
        }
        
        int occurrenceCounter = 0;
        
        while ((!endDateTime.HasValue || currentDate <= endDateTime) && 
               (!recurrence.OccurrenceCount.HasValue || occurrenceCounter < recurrence.OccurrenceCount.Value))
        {
            switch (recurrence.RecurrenceType)
            {
                case RecurrenceType.Daily:
                    dates.Add(currentDate);
                    currentDate = currentDate.AddDays(recurrence.Interval);
                    break;
                case RecurrenceType.Weekly:
                    if (string.IsNullOrEmpty(recurrence.DaysOfWeek))
                    {
                        dates.Add(currentDate);
                        currentDate = currentDate.AddDays(7 * recurrence.Interval);
                    }
                    else
                    {
                        var daysOfWeek = recurrence.DaysOfWeek.Split(',').Select(int.Parse).ToList();
                        if (daysOfWeek.Contains((int)currentDate.DayOfWeek))
                        {
                            dates.Add(currentDate);
                        }
                        currentDate = currentDate.AddDays(1);
                    }
                    break;
                // Implement other recurrence types (Monthly, Yearly, Custom)
            }
            
            occurrenceCounter++;
        }
        
        return dates;
    }
}