using System;
using System.Collections.Generic;

namespace AssignmentPRN222.Models;

public partial class ProVince
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Cinema> Cinemas { get; set; } = new List<Cinema>();
}
