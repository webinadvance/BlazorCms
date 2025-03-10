namespace BlazorApp2.Data.Models;

public class BookingRecurrence
{
    public int Id { get; set; }
    public RecurrenceType RecurrenceType { get; set; }
    public int Interval { get; set; } = 1; 
    public int? OccurrenceCount { get; set; }
    public DateTime? EndDate { get; set; }
    public string? DaysOfWeek { get; set; }
    public int? DayOfMonth { get; set; }
    public int? MonthOfYear { get; set; }
    public DateTime StartDate { get; set; }
    public TimeSpan Duration { get; set; }
    public int ResourceId { get; set; }
    public Resource? Resource { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public List<Booking> Bookings { get; set; } = new();
}
