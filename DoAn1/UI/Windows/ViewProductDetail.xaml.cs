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
    /// Interaction logic for ViewProductDetail.xaml
    /// </summary>
    public partial class ViewProductDetail : Window
    {
        Book _book;
        public ViewProductDetail()
        {
            InitializeComponent();
        }
        public ViewProductDetail(Book book)
        {
            InitializeComponent();
            _book= book;
            this.DataContext = _book;
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new UpdateProduct(_book);
            if(screen.ShowDialog()==true)
            {
                //Handle event success?
            }


        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult choice = MessageBox.Show("Do you really want to delete this item?", "Delete item",
                MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if(choice==MessageBoxResult.OK)
            {
                BookDAO.Instance.DeleteBookById(_book.Id);
                MessageBox.Show("Delete successfully");
                this.Close();
            }
        }
    }
}
