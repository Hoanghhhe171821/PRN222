using System;
using System.Collections.Generic;

namespace AssignmentPRN222.Models;

public partial class ShowTime
{
    public int Id { get; set; }

    public int MovieId { get; set; }

    public int CinemaId { get; set; }

    public int RoomId { get; set; }

    public DateOnly DateShowTime { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int PriceA { get; set; }

    public int PriceB { get; set; }

    public bool IsBooked { get; set; }

    public virtual Movie Movie { get; set; } = null!;

    public virtual ICollection<SeatsBooking> SeatsBookings { get; set; } = new List<SeatsBooking>();
}
