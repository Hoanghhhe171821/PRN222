using System;
using System.Collections.Generic;

namespace AssignmentPRN222.Models;

public partial class Movie
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Actor { get; set; } = null!;

    public string? Images { get; set; } = null!;

    public TimeOnly TimeMovie { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<ShowTime> ShowTimes { get; set; } = new List<ShowTime>();
}
