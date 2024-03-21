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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DoAn1.UI.UserControls
{
    /// <summary>
    /// Interaction logic for IconOnlyButtonUC.xaml
    /// </summary>
    public partial class IconOnlyButtonUC : UserControl
    {
        public string UC_Kind { get; set; }
        public string UC_Foreground { get; set; } = "#593122";
        public int UC_Height { get; set; } = DoAn1.Properties.Settings.Default.ButtonSize;

        public event RoutedEventHandler Click
        {
            add { button.AddHandler(ButtonBase.ClickEvent, value); }
            remove { button.RemoveHandler(ButtonBase.ClickEvent, value); }
        }

        public IconOnlyButtonUC()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
