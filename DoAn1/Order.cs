using System;
using System.Collections.Generic;

namespace DoAn1;

public partial class Order
{
    public int Id { get; set; }

    public string? CustomerName { get; set; }

    public double? TotalPrice { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? DiscountId { get; set; }

    public virtual Discount? Discount { get; set; }

    public virtual ICollection<OrderBook> OrderBooks { get; set; } = new List<OrderBook>();
}
