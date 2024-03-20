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
    /// Interaction logic for DashboardWindow.xaml
    /// </summary>
    public partial class DashboardWindow : Window
    {

        public DashboardWindow()
        {
            InitializeComponent();
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Success");
        }

        private void switchMenuMode(object sender, RoutedEventArgs e)
        {
            var size = menuPanel.Width;
            if (size > 20)
            {
                menuPanel.Width = 20;
            } else
            {
                menuPanel.Width = 200;
            }
        }
    }
}
