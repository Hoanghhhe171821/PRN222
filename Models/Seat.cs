using System;
using System.Collections.Generic;

namespace AssignmentPRN222.Models;

public partial class Seat
{
    public int Id { get; set; }

    public int RowNumber { get; set; }

    public int CloNumber { get; set; }

    public string TypeSeat { get; set; } = null!;

    public int RoomId { get; set; }

    public virtual Room Room { get; set; } = null!;

    public virtual ICollection<SeatsBooking> SeatsBookings { get; set; } = new List<SeatsBooking>();
}
