using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    /// Interaction logic for CalenderWindow.xaml
    /// </summary>
    public partial class CalenderWindow : Window, INotifyPropertyChanged
    {
        string Date;
        public CalenderWindow(string date)
        {
            InitializeComponent();
            Date = date;
        }
        private void mycalender_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dateSelected != null)
            {
                dateSelected.UC_TextInput = GetDateOnly(mycalender.SelectedDate.ToString());
            }
        }


    private void mycalender_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dateSelected.UC_TextInput == "")
            {
                dateSelected.UC_TextInput = GetDateOnly(mycalender.SelectedDate.ToString());
                return;
            }
            if (dateSelected.UC_TextInput == GetDateOnly(mycalender.SelectedDate.ToString()))
            {
                dateSelected.UC_TextInput = "";
            }
        }

        private void mycalender_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (dateSelected != null)
            {
                dateSelected.UC_TextInput = GetDateOnly(mycalender.SelectedDate.ToString());
            }
        }
        static string GetDateOnly(string dateTimeString)
        {
            string[] parts = dateTimeString.Split(' ');

            return ConvertDateFormat(parts[0]);
        }
        static string ConvertDateFormat(string dateTimeString)
        {
            string[] parts = dateTimeString.Split('/');

            string formattedDate = parts[1] + "/" + parts[0] + "/" + parts[2];

            return formattedDate;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            Date = dateSelected.UC_TextInput;

            // Lấy tham chiếu tới OrderWindow
            ViewOrder orderWindow = Application.Current.Windows.OfType<ViewOrder>().FirstOrDefault();
            if (orderWindow != null)
            {
                // Gọi phương thức cập nhật dateCreated trong OrderWindow
                orderWindow.UpdateDateCreated(Date);
            }

            DialogResult = true;
        }

    }
}
