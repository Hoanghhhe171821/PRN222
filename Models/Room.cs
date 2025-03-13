using System;
using System.Collections.Generic;

namespace AssignmentPRN222.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string RoomName { get; set; } = null!;

    public int CinemaId { get; set; }

    public int TotalRow { get; set; }

    public int TotalColum { get; set; }

    public bool IsDelete { get; set; }

    public virtual Cinema Cinema { get; set; } = null!;

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
