namespace BlazorApp2.Data.Models;
public class Resource
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;
    public decimal? HourlyRate { get; set; }
    public decimal? DailyRate { get; set; }
    public List<Booking> Bookings { get; set; } = new();
}