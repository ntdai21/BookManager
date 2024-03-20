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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DoAn1.UI.UserControls
{
    /// <summary>
    /// Interaction logic for IconButtonUC.xaml
    /// </summary>
    public partial class IconButtonUC : UserControl
    {
        public string UC_Kind { get; set; }
        public string UC_Foreground { get; set; } = "DimGray";
        public string UC_Title { get; set; }

        public IconButtonUC()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
