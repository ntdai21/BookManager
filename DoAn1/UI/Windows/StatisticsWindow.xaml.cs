using LiveCharts;
using LiveCharts.Wpf;
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
    /// Interaction logic for StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        
        public StatisticsWindow()
        {
            InitializeComponent();
        }

        private void StatisticsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ShowYearlyRevenueAndProfit(2024);
        }

        private void ShowMonthlyRevenueAndProfit(int year, int month)
        {
            // Calculate monthly revenue and profit
            decimal[] monthlyData = new decimal[DateTime.DaysInMonth(year, month)];
            for (int day = 1; day <= monthlyData.Length; day++)
            {
                DateTime date = new DateTime(year, month, day);
                monthlyData[day - 1] = OrderDAO.Instance.CalculateDailyRevenueAndProfit(date);
            }

            // Update chart
            RevenueProfitChart.Series = new SeriesCollection
        {
            new ColumnSeries
            {
                Title = "Revenue and Profit",
                Values = new ChartValues<decimal>(monthlyData)
            }
        };
        }

        private void ShowYearlyRevenueAndProfit(int year)
        {
            // Calculate yearly revenue and profit
            decimal[] yearlyData = new decimal[12];
            for (int month = 1; month <= 12; month++)
            {
                yearlyData[month - 1] = OrderDAO.Instance.CalculateMonthlyRevenueAndProfit(year, month);
            }

            // Update chart
            RevenueProfitChart.Series = new SeriesCollection
        {
            new ColumnSeries
            {
                Title = "Revenue and Profit",
                Values = new ChartValues<decimal>(yearlyData)
            }
        };
        }

        private void MonthlyRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            ShowMonthlyRevenueAndProfit(year, month);
        }

        private void YearlyRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            int year = DateTime.Now.Year;
            ShowYearlyRevenueAndProfit(year);
        }

        private void switchMenuMode(object sender, RoutedEventArgs e)
        {
            var size = DoAn1.Properties.Settings.Default.ButtonSize + 2;
            if (menuPanel.Width > size)
            {
                menuPanel.Width = size;
            }
            else
            {
                menuPanel.Width = 200;
            }
        }
        private void configBtn_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationWindow configurationWindow = new ConfigurationWindow();
            configurationWindow.Owner = this;
            configurationWindow.ShowDialog();
        }
    }
}
