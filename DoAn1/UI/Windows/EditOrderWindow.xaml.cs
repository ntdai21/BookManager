﻿using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Auth;
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
using System.ComponentModel;
using DoAn1.Models;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace DoAn1.UI.Windows
{
    /// <summary>
    /// Interaction logic for EditOrderWindow.xaml
    /// </summary>
    public partial class EditOrderWindow : Window, INotifyPropertyChanged
    {

        public Order NewOrder { get; set; } = null;
        BindingList<OrderBook> _orderBooks = null;
        public double GrossPrice { get; set; } = 0;
        public double Discount { get; set; } = 0;
        public double TotalPrice { get; set; } = 0;


        public event PropertyChangedEventHandler? PropertyChanged;



        public EditOrderWindow(Order order)
        {
            InitializeComponent();
            NewOrder = (Order) order.Clone();
            _orderBooks = new BindingList<OrderBook>((IList<OrderBook>)NewOrder.OrderBooks);
            booksDataGrid.ItemsSource = _orderBooks;
            MyShopContext db = new MyShopContext();
            DiscountComboBox.ItemsSource = new BindingList<Discount>();
            this.DataContext = this;

        }

        private void calculateTotalPrice()
        {
            GrossPrice = 0;
            foreach (OrderBook orderBook in NewOrder.OrderBooks)
            {
                GrossPrice += (double)(orderBook.Book.Price * (double)orderBook.NumOfBook);
            }
            if (NewOrder.DiscountId == null)
            {
                Discount = 0;
            }
            else
            {
                Discount = (double)(GrossPrice * (NewOrder.Discount.DiscountPercent / 100));
                if (Discount > NewOrder.Discount.MaxDiscount)
                {
                    Discount = (double)NewOrder.Discount.MaxDiscount;
                }
            }
            TotalPrice = GrossPrice - Discount;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            NewOrder.CustomerName = CustomerNameInputFieldUC.UC_TextInput;
            NewOrder.ShippingAddress = ShippingAddressInputFieldUC.UC_TextInput;

            if (NewOrder.CustomerName.Trim() == "")
            {
                MessageBox.Show("You have to insert your customer's name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (NewOrder.ShippingAddress.Trim() == "")
            {
                MessageBox.Show("You have to insert shipping address", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (NewOrder.OrderBooks.Count() == 0)
            {
                MessageBox.Show("You have to insert products to your order", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NewOrder.TotalPrice = TotalPrice;
            NewOrder.CreatedAt = DateTime.Now;

            NewOrder.Discount = null;
            foreach (OrderBook orderBook in NewOrder.OrderBooks)
            {
                orderBook.Book = null;
            }
            OrderDAO.Instance.AddOrder(NewOrder);

            MessageBox.Show("Added a order successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var tb = (Button)e.OriginalSource;
            var dataCxtx = tb.DataContext;

            OrderBook orderBook = NewOrder.OrderBooks.SingleOrDefault(ob => ob.BookId == ((OrderBook)dataCxtx).BookId);
            NewOrder.OrderBooks.Remove(orderBook);
            _orderBooks.Remove(orderBook);
            calculateTotalPrice();
            MessageBox.Show("Deleted the selected book successfully!");
        }
        private void TextOnlyButtonUC_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddBookToOrder(NewOrder, _orderBooks);
            if (screen.ShowDialog() == true)
            {
            }

            calculateTotalPrice();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            var tb = (Button)e.OriginalSource;
            var dataCxtx = tb.DataContext;

            OrderBook orderBook = (OrderBook)dataCxtx;
            var screen = new BookQuantityFormWindow(orderBook.Book, (int)orderBook.NumOfBook);
            if (screen.ShowDialog() == true)
            {
                orderBook.NumOfBook = screen.BookQuantity;
                calculateTotalPrice();
                MessageBox.Show($"Updated the selected book successfully! {NewOrder.OrderBooks}");
            }
        }

        private void DiscountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Discount discount = (Discount)DiscountComboBox.SelectedItem;
            NewOrder.DiscountId = discount.Id;
            NewOrder.Discount = discount;
            calculateTotalPrice();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            NewOrder.CustomerName = CustomerNameInputFieldUC.UC_TextInput;
            NewOrder.ShippingAddress = ShippingAddressInputFieldUC.UC_TextInput;

            if (NewOrder.CustomerName != "")
            {
                MessageBox.Show("You have to insert your customer's name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (NewOrder.ShippingAddress != "")
            {
                MessageBox.Show("You have to insert shipping address", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (NewOrder.OrderBooks.Count() == 0)
            {
                MessageBox.Show("You have to insert products to your order", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            OrderDAO.Instance.AddOrder(NewOrder);
            MessageBox.Show("Added a order successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

