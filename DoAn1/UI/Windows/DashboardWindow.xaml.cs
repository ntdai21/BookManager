﻿using DoAn1.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for DashboardWindow.xaml
    /// </summary>
    public partial class DashboardWindow : Window, INotifyPropertyChanged
    {
        public float NumOfBooks {  get; set; }
        public float NumOfOrders { get; set; }
        public float NumOfDiscounts { get; set; }

        public float NewOrdersDay { get; set; }
        public float NewOrdersWeek { get; set; }
        public float NewOrdersMonth { get; set; }

        public float RevenueDay { get; set; }
        public float RevenueWeek { get; set; }
        public float RevenueMonth { get; set; }

        public DashboardWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DoAn1.Properties.Settings.Default.LastWindow = "Dashboard";
            DoAn1.Properties.Settings.Default.Save();

            //
            NumOfBooks = BookDAO.Instance.GetBooks().Count;
            NumOfOrders = OrderDAO.Instance.GetOrders().Count;
            NumOfDiscounts = DiscountDAO.Instance.GetDiscounts().Count;

            NewOrdersDay = 99;
            NewOrdersWeek = 99;
            NewOrdersMonth = 99;

            RevenueDay = 99;
            RevenueWeek = 99;
            RevenueMonth = 99;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
