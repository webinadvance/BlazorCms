using System;
namespace ClassLibrary1.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public string? Notes { get; set; }
        public decimal TotalPrice { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int ResourceId { get; set; }
        public Resource? Resource { get; set; }
        public int? BookingRecurrenceId { get; set; }
        public BookingRecurrence? BookingRecurrence { get; set; }
    }
}