using DoAn1.BUS;
using DoAn1.DAO;
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
    /// Interaction logic for AddDiscountWindow.xaml
    /// </summary>
    public partial class AddDiscountWindow : Window
    {
        public Discount discount;

        public AddDiscountWindow()
        {
            InitializeComponent();
            discount = new Discount();
            DataContext = discount;
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            if (editDiscount.HasError)
            {
                MessageBox.Show("Invalid data!");
            }
            else if (DiscountBUS.ValidateDiscount(discount) == DiscountBUS.DiscountValidationResult.CodeAlreadyExists)
            {
                MessageBox.Show("Code already exists!");
            }
            else
            {
                DiscountDAO.Instance.AddDiscount(discount);
                DialogResult = true;
            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
