using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DoAn1;

public partial class Book : INotifyPropertyChanged
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public double? Price { get; set; }

    public int? NumOfPage { get; set; }

    public string? PublishingCompany { get; set; }

    public string? Author { get; set; }

    public string? Cover { get; set; }

    public double? CostPrice { get; set; }

    public string? Description { get; set; }

    public int? CategoryId { get; set; }
    public string CategoryName
    {
        get
        {
            return CategoryDAO.Instance.FindById((int)CategoryId).Name;
        }
    }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderBook> OrderBooks { get; set; } = new List<OrderBook>();

    public event PropertyChangedEventHandler? PropertyChanged;
}
