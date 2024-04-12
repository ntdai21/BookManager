using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn1
{
    public partial class Book : INotifyPropertyChanged
    {
        public int Quantity { get; set; }

        public static explicit operator Book(OrderBook? v)
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
