﻿using System.Collections.Generic;
namespace ClassLibrary1.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}