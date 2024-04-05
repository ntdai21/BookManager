using DoAn1.BUS;
using DoAn1.DAO;
using DoAn1.Models;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
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
    /// Interaction logic for CouponManagement.xaml
    /// </summary>
    public partial class CouponManagement : Window
    {
        int currentPage = 1;
        int totalPages = 0;
        int rowsPerPage = DoAn1.Properties.Settings.Default.ItemsPerPage; 
        int totalItems = 0;
        string keyword = "";
        string sortBy = "Latest";
        BindingList<Discount> _discounts = new();

        public CouponManagement()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DoAn1.Properties.Settings.Default.LastWindow = "Coupon Management";
            DoAn1.Properties.Settings.Default.Save();
            sortCombobox.SelectedItem = 0;
            discountDataGrid.ItemsSource = _discounts;
            LoadAll();
        }

        private void NextPageBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PreviousPageBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {

        }

        public void LoadAll()
        {
            (totalPages, totalItems) = DiscountBUS.Instance.LoadDiscounts(_discounts, currentPage, rowsPerPage, keyword, sortBy);
            showingItemsText.Text = _discounts.Count.ToString();
            totalItemsText.Text = totalItems.ToString();
            currentPageText.Text = currentPage.ToString();
            totalPagesText.Text = totalPages.ToString();
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            var discount = (Discount)discountDataGrid.SelectedItem;

            var confirm = MessageBox.Show($"Are you sure to delete discount \'{discount.Code}\'", "", MessageBoxButton.YesNo);

            if (confirm == MessageBoxResult.Yes)
            {
                DiscountDAO.Instance.DeleteDiscountById(discount.Id);
                LoadAll();
            }


        }
    }
}
