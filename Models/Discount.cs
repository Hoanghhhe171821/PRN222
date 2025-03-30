using System;
using System.Collections.Generic;

namespace AssignmentPRN222.Models;

public partial class Discount
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int DiscountPrice { get; set; }

    public bool IsDiscounted { get; set; }
    public int Quantity { get; set; }
}
