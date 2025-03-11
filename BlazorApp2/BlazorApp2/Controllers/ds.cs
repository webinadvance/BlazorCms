using BlazorApp2.Data.Services;
using ClassLibrary1.Models;
using Microsoft.AspNetCore.Mvc;
namespace BlazorApp2.Controllers;
[ApiController]
[Route("api")]
public class ApiController : ControllerBase
{
    private readonly BookingService _bookingService;
    public ApiController(BookingService bookingService)
    {
        _bookingService = bookingService;
    }
    [HttpGet("resources")]
    public async Task<IActionResult> GetResources()
    {
        return Ok(await _bookingService.GetResourcesAsync());
    }
    [HttpGet("resources/{id}")]
    public async Task<IActionResult> GetResource(int id)
    {
        var resource = await _bookingService.GetResourceByIdAsync(id);
        return resource != null ? Ok(resource) : NotFound();
    }
    [HttpGet("customers")]
    public async Task<IActionResult> GetCustomers()
    {
        return Ok(await _bookingService.GetCustomersAsync());
    }
    [HttpGet("customers/{id}")]
    public async Task<IActionResult> GetCustomer(int id)
    {
        var customer = await _bookingService.GetCustomerByIdAsync(id);
        return customer != null ? Ok(customer) : NotFound();
    }
    [HttpPost("customers")]
    public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
    {
        var result = await _bookingService.AddCustomerAsync(customer);
        return CreatedAtAction(nameof(GetCustomer), new { id = result.Id }, result);
    }
    [HttpGet("bookings")]
    public async Task<IActionResult> GetBookings()
    {
        return Ok(await _bookingService.GetBookingsAsync());
    }
    [HttpGet("bookings/{id}")]
    public async Task<IActionResult> GetBooking(int id)
    {
        var booking = await _bookingService.GetBookingByIdAsync(id);
        return booking != null ? Ok(booking) : NotFound();
    }
    [HttpPost("bookings")]
    public async Task<IActionResult> CreateBooking([FromBody] Booking booking)
    {
        try
        {
            var result = await _bookingService.CreateBookingAsync(booking);
            return CreatedAtAction(nameof(GetBooking), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
    [HttpPut("bookings/{id}/status")]
    public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] BookingStatus status)
    {
        var booking = await _bookingService.UpdateBookingStatusAsync(id, status);
        return booking != null ? Ok(booking) : NotFound();
    }
    [HttpPost("bookings/recurrence")]
    public async Task<IActionResult> CreateRecurringBooking([FromBody] BookingRecurrence recurrence)
    {
        try
        {
            var result = await _bookingService.CreateRecurringBookingAsync(recurrence);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}