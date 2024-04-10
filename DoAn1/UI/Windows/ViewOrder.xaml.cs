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
using System.Collections.ObjectModel;
using DoAn1.BUS;
namespace DoAn1.UI.Windows
{
    /// <summary>
    /// Interaction logic for ViewOrder.xaml
    /// </summary>
    public partial class ViewOrder : Window
    {
        int currentPage = 1;
        int totalPages = 0;
        int rowsPerPage = DoAn1.Properties.Settings.Default.ItemsPerPage;
        int totalItems = 0;
        string keyword = "";
        string sortBy = "Latest";
        public string dateCreated { get; set; }
        bool isLoaded = false;
        ObservableCollection<Order> _orders = new ObservableCollection<Order>();
        public ViewOrder()
        {
            InitializeComponent();
            orderDataGrid.ItemsSource = _orders;
            sortCombobox.SelectedItem = 0;
            loadAll();
        }
        
        public async void loadAll()
        {
            if(!isLoaded) { return; }
            (totalPages, totalItems) = OrderBUS.Instance.AddDataToTable(_orders, currentPage, rowsPerPage, keyword, sortBy, dateCreated);
            showingItemsText.Text = _orders.Count.ToString();
            totalItemsText.Text = totalItems.ToString();
            currentPageText.Text = currentPage.ToString();
            totalPagesText.Text = totalPages.ToString();
        }



        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new CreateOrderWindow();
            if (screen.ShowDialog() == true)
            {

            }
        }

        private void deleteOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = (Order)orderDataGrid.SelectedItem;
            OrderBUS.Instance.HandleDeleteOrder(selectedOrder, _orders);
            loadAll();
        }

        private void editOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = (Order)orderDataGrid.SelectedItem;
            var screen = new EditOrderWindow(selectedOrder);
            if (screen.ShowDialog() == true)
            {

            }
            loadAll();
        }

        private void PreviousPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage == 1) return;
            currentPage--;
            loadAll();
        }

        private void NextPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage == totalPages) return;
            currentPage++;
            loadAll();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtSearch.Text != "")
            {
                SearchTextBlock.Text = "";
            }
            else SearchTextBlock.Text = "Search here...";
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            keyword = txtSearch.Text;
            loadAll();
            if (totalItems == 0) { 
                MessageBox.Show("No matching results were found!");
                keyword = "";
                loadAll();
            }
            else MessageBox.Show("Searching successfully");
        }

        private void detailOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = (Order)orderDataGrid.SelectedItem;
            var screen = new OrderDetailWindow(selectedOrder);
            if (screen.ShowDialog() == true)
            {

            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)sortCombobox.SelectedItem;
            sortBy = selectedItem.Content.ToString();
            loadAll();
        }

        private void ViewOrder_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
            loadAll();

            DoAn1.Properties.Settings.Default.LastWindow = "Invoice Management";
            DoAn1.Properties.Settings.Default.Save();
        }

        private void TextOnlyButtonUC_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void dateCreated_Click(object sender, RoutedEventArgs e)
        {
            CalenderWindow calenderWindow = new CalenderWindow(dateCreated);
            if (calenderWindow.ShowDialog() == true)
            {
                if (dateCreated != "")
                {
                    dateButton.UC_Text = dateCreated;
                }
                else
                {
                    dateButton.UC_Text = "Date Created";
                }
                loadAll();
            }
        }
        public void UpdateDateCreated(string newDate)
        {
            dateCreated = newDate;
        }

    }
}
