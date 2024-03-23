using System;
using System.Collections.Generic;

namespace DoAn1;

public partial class OrderBook
{
    public int OrderId { get; set; }

    public int BookId { get; set; }

    public int? NumOfBook { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
