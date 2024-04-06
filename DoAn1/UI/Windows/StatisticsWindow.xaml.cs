using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2016.Drawing.Command;
using DocumentFormat.OpenXml.Spreadsheet;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MessageBox = System.Windows.Forms.MessageBox;

namespace DoAn1.UI.Windows
{
    /// <summary>
    /// Interaction logic for StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        bool loaded = false;
        int filter = 0;
        int filterBook = 0;
        List<string> Labels = new List<string>();
        List<string> RankLabels = new List<string>();
        bool started = false;

        public StatisticsWindow()
        {
            InitializeComponent();
        }

        private async void StatisticsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DoAn1.Properties.Settings.Default.LastWindow = "Statistical Reporting";
            DoAn1.Properties.Settings.Default.Save();

            loaded = true;
            int revenue, profit;
            (revenue, profit) = OrderDAO.Instance.CalculateOverallRevenueAndProfit();
            totalRevenue.textBox.Text = revenue.ToString("#,##0") + " VNĐ";
            totalProfit.textBox.Text = profit.ToString("#,##0") + " VNĐ";
            DateTime currentDate = DateTime.Now;
            DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

            // Đợi 1 giây
            await Task.Delay(1000);
            datePickerRevenueProfitMonth.SelectedDate = firstDayOfMonth;
            datePickerBookMonth.SelectedDate = firstDayOfMonth;
        }
        private void ShowMonthlyRevenueAndProfit(int year, int month)
        {
            Labels.Clear();
            decimal[] monthlyRevenue = new decimal[DateTime.DaysInMonth(year, month) + 1];
            decimal[] monthlyProfit = new decimal[DateTime.DaysInMonth(year, month) + 1];
            for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
            {
                DateTime date = new DateTime(year, month, day);
                (monthlyRevenue[day], monthlyProfit[day]) = OrderDAO.Instance.CalculateDailyRevenueAndProfit(date);
                Labels.Add(day.ToString());
            }

            RevenueProfitChart.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = $"Revenue {month}/{year}",
                    Values = new ChartValues<decimal>(monthlyRevenue),
                    StrokeDashArray = new DoubleCollection{2},
                    Stroke = Brushes.Red,
                    StrokeThickness = 1,
                    Fill = Brushes.Red,
                },
                new ColumnSeries
                {
                    Title = $"Profit {month}/{year}",
                    Values = new ChartValues<decimal>(monthlyProfit),
                    StrokeDashArray = new DoubleCollection{2},
                    Stroke = Brushes.Blue,
                    StrokeThickness = 1,
                    Fill = Brushes.Blue,
                    ColumnPadding = 3,
                }
            };
            titleRevenueProfitChart.Text = $"Revenue And Profit Of {month}/{year}";
        }

        private void ShowYearlyRevenueAndProfit(int year)
        {
            Labels.Clear();
            decimal[] yearlyRevenue = new decimal[13];
            decimal[] yearlyProfit = new decimal[13];
            for (int month = 1; month <= 12; month++)
            {
                (yearlyRevenue[month], yearlyProfit[month]) = OrderDAO.Instance.CalculateMonthlyRevenueAndProfit(year, month);
                Labels.Add(month.ToString());
            }

            // Update chart
            RevenueProfitChart.Series = new SeriesCollection {
                new ColumnSeries
                {
                    Title = $"Revenue {year}",
                    Values = new ChartValues<decimal>(yearlyRevenue),
                    StrokeDashArray = new DoubleCollection{2},
                    Fill = Brushes.Red,
                },
                new ColumnSeries
                {
                    Title = $"Profit {year}",
                    Values = new ChartValues<decimal>(yearlyProfit),
                    StrokeDashArray = new DoubleCollection{2},
                    Fill = Brushes.Blue,
                    ColumnPadding = 3,
                }
            };
            titleRevenueProfitChart.Text = $"Revenue And Profit Of {year}";

        }

        private void ShowTopBookSoldInMonth(int month, int year)
        {
            RankLabels.Clear();
            BookChart.Series.Clear();
            List<Tuple<Book, int>> topSellingBooks = OrderDAO.Instance.GetTopSellingBooksInMonth(month, year);

            foreach (var tuple in topSellingBooks)
            {
                Book book = tuple.Item1;
                int quantitySold = tuple.Item2;
                RankLabels.Add(topSellingBooks.IndexOf(tuple).ToString());
                BookChart.Series.Add(new RowSeries
                {
                    Title = book.Name,
                    Values = new ChartValues<decimal> { quantitySold },
                    RowPadding = 7
                });
            }
            titleBookChart.Text = $"Top 7 Bestselling Books Of {month}/{year}";
        }
        private void ShowTopBookSoldInYear(int year)
        {
            RankLabels.Clear();
            BookChart.Series.Clear();
            List<Tuple<Book, int>> topSellingBooks = OrderDAO.Instance.GetTopSellingBooksInYear(year);

            foreach (var tuple in topSellingBooks)
            {
                Book book = tuple.Item1;
                int quantitySold = tuple.Item2;
                RankLabels.Add(topSellingBooks.IndexOf(tuple).ToString());
                BookChart.Series.Add(new RowSeries
                {
                    Title = book.Name,
                    Values = new ChartValues<decimal> { quantitySold },
                    RowPadding = 7
                });
            }
            titleBookChart.Text = $"Top 7 Bestselling Books Of {year}";
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


        private void configBtn_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationWindow configurationWindow = new ConfigurationWindow();
            configurationWindow.Owner = this;
            configurationWindow.ShowDialog();
        }

        void UpdateDatePickerVisibility(int filterIndex)
        {
            datePickerRevenueProfitMonth.IsEnabled = filterIndex == 0 && loaded;
            datePickerRevenueProfitMonth.Visibility = filter == 0 ? Visibility.Visible : Visibility.Collapsed;
            datePickerRevenueProfitYear.IsEnabled = filterIndex == 1 && loaded;
            datePickerRevenueProfitYear.Visibility = filter == 1 ? Visibility.Visible : Visibility.Collapsed;
            Canvas.SetZIndex(datePickerRevenueProfitMonth, filterIndex == 0 ? 1 : 0);
            Canvas.SetZIndex(datePickerRevenueProfitYear, filterIndex == 1 ? 1 : 0);
        }

        private void ComboBoxTypeDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (loaded == true)
            {
                filter = sortCombobox.SelectedIndex;
                UpdateDatePickerVisibility(filter);
            }
        }

        private void datePickerRevenueProfit_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selecteDate = new DateTime();
            int month = 0, year = 0;
            if (loaded == true && (datePickerRevenueProfitMonth.SelectedDate != null || datePickerRevenueProfitYear.SelectedDate != null))
            {
                if (filter == 0)
                {
                    selecteDate = (DateTime)datePickerRevenueProfitMonth.SelectedDate;
                    month = selecteDate.Month;
                    year = selecteDate.Year;
                    ShowMonthlyRevenueAndProfit(year, month);
                }
                else if (filter == 1)
                {
                    selecteDate = (DateTime)datePickerRevenueProfitYear.SelectedDate;
                    year = selecteDate.Year;
                    ShowYearlyRevenueAndProfit(year);
                }
            }
        }
        private void DatePicker_Opened(object sender, RoutedEventArgs e)
        {
            DatePicker datepicker = (DatePicker)sender;
            Popup popup = (Popup)datepicker.Template.FindName("PART_Popup", datepicker);
            Calendar cal = (Calendar)popup.Child;
            cal.DisplayModeChanged += Calender_DisplayModeChanged;
            cal.DisplayMode = CalendarMode.Decade;
        }

        private void Calender_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            Calendar calendar = (Calendar)sender;
            if (calendar.DisplayMode == CalendarMode.Month)
            {
                calendar.SelectedDate = calendar.DisplayDate;
                datePickerRevenueProfitMonth.IsDropDownOpen = false;
            }
        }

        private void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            datePickerRevenueProfitMonth.SelectedDate = firstDayOfMonth;
        }

        private void DatePickerBook_Opened(object sender, RoutedEventArgs e)
        {
            DatePicker datepicker = (DatePicker)sender;
            Popup popup = (Popup)datepicker.Template.FindName("PART_Popup", datepicker);
            Calendar cal = (Calendar)popup.Child;
            cal.DisplayModeChanged += CalenderBook_DisplayModeChanged;
            cal.DisplayMode = CalendarMode.Decade;
        }
        private void CalenderBook_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            Calendar calendar = (Calendar)sender;
            if (calendar.DisplayMode == CalendarMode.Month)
            {
                calendar.SelectedDate = calendar.DisplayDate;
                datePickerBookMonth.IsDropDownOpen = false;
            }
        }

        private void datePickerBook_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selecteDate = new DateTime();
            int month = 0, year = 0;
            if (loaded == true && (datePickerBookMonth.SelectedDate != null || datePickerBookYear.SelectedDate != null))
            {
                if (filterBook == 0)
                {
                    selecteDate = (DateTime)datePickerBookMonth.SelectedDate;
                    month = selecteDate.Month;
                    year = selecteDate.Year;
                    ShowTopBookSoldInMonth(month, year);
                }
                else if (filterBook == 1)
                {
                    selecteDate = (DateTime)datePickerBookYear.SelectedDate;
                    year = selecteDate.Year;
                    ShowTopBookSoldInYear(year);
                }
            }
        }

        void UpdateDatePickerBookVisibility(int filterIndex)
        {
            datePickerBookMonth.IsEnabled = filterIndex == 0 && loaded;
            datePickerBookMonth.Visibility = filterIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
            datePickerBookYear.IsEnabled = filterIndex == 1 && loaded;
            datePickerBookYear.Visibility = filterIndex == 1 ? Visibility.Visible : Visibility.Collapsed;
            Canvas.SetZIndex(datePickerBookMonth, filterIndex == 0 ? 1 : 0);
            Canvas.SetZIndex(datePickerBookYear, filterIndex == 1 ? 1 : 0);
        }


        private void ComboBoxBookTypeDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (loaded == true)
            {
                filterBook = sortBookCombobox.SelectedIndex;
                UpdateDatePickerBookVisibility(filterBook);
            }
        }
    }
}


