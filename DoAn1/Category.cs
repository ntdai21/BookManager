using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DoAn1;

public partial class Category:INotifyPropertyChanged
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    public event PropertyChangedEventHandler? PropertyChanged;
}
