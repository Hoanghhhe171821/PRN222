using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssignmentPRN222.Models;

public partial class ShowTime
{
    public int Id { get; set; }
    public int MovieId { get; set; }

    public int CinemaId { get; set; }

    public int RoomId { get; set; }

    public DateOnly DateShowTime { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public int PriceA { get; set; }

    public int PriceB { get; set; }

    public bool IsBooked { get; set; }
    public virtual Movie? Movie { get; set; }
    public virtual ICollection<SeatsBooking> SeatsBookings { get; set; } = new List<SeatsBooking>();

}
