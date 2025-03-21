using System;
using System.Collections.Generic;

namespace AssignmentPRN222.Models;

public partial class SeatsBooking
{
    public int Id { get; set; }

    public int SeatId { get; set; }

    public int ShowTimeId { get; set; }

    public bool IsBooked { get; set; }

    public int? OrderId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Seat Seat { get; set; } = null!;

    public virtual ShowTime ShowTime { get; set; } = null!;
}
public class SeatBookingViewModel
{
    public ShowTime ShowTime { get; set; }
    public List<Seat> Seats { get; set; }
    public List<int> BookedSeatIds { get; set; }
}
