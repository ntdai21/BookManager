using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DoAn1.UI.Windows
{
    /// <summary>
    /// Interaction logic for BookQuantityWindow.xaml
    /// </summary>
    public partial class BookQuantityFormWindow : Window
    {

        Book _book = null;
        public int BookQuantity { get; set; } = 0;
        public BookQuantityFormWindow(Book book, int bookQuantity)
        {
            InitializeComponent();
            _book = book;
            BookQuantity = bookQuantity;
            this.DataContext = _book;
            quantityInputField.DataContext = BookQuantity;
        }

        private void AddToOrder()
        {
            BookQuantity = Int32.Parse(quantityInputField.UC_TextInput);
            DialogResult = true;
        }

        private void AddToOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (quantityInputField.UC_TextInput != null)
            {
                AddToOrder();
            }
        }

        private void quantityInputField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter && quantityInputField.UC_TextInput != null)
            {
                AddToOrder();
            }
        }
    }
}
