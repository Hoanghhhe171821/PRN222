using System;
using System.Collections.Generic;

namespace AssignmentPRN222.Models;

public partial class Cinema
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public int ProvinceId { get; set; }

    public bool IsDelete { get; set; }

    public byte[] Version { get; set; } = null!;

    public virtual ProVince Province { get; set; } = null!;

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
