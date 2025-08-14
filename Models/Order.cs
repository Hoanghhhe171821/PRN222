using System;
using System.Collections.Generic;

namespace AssignmentPRN222.Models;

public partial class Order
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public int PriceFrist { get; set; }

    public virtual ICollection<SeatsBooking> SeatsBookings { get; set; } = new List<SeatsBooking>();

    public string Status { get; set; } 

    // Phương thức thanh toán
    public string PaymentMethod { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
