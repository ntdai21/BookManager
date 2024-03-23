using System;
using System.Collections.Generic;

namespace DoAn1;

public partial class Discount
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public double? DiscountPercent { get; set; }

    public double? MaxDiscount { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
