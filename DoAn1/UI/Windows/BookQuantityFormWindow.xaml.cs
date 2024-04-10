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
            int number;

            bool success = int.TryParse(quantityInputField.UC_TextInput, out number);
            if (success)
            {
                BookQuantity = number;
                if (BookQuantity <= 0)
                {
                    MessageBox.Show("Quantity must be above 0", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (BookQuantity > _book.Quantity)
                {
                    MessageBox.Show($"The quantity of the {_book.Name} book is {_book.Quantity} ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    DialogResult = true;
                }
            }
            else
            {
                MessageBox.Show("Your input is not valid", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

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
