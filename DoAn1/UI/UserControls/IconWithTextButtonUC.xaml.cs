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
    /// Interaction logic for IconWithTextButtonUC.xaml
    /// </summary>
    public partial class IconWithTextButtonUC : UserControl
    {
        public string UC_Kind { get; set; } = "";
        public string UC_Foreground { get; set; } = "#593122";
        public string UC_Text { get; set; }
        public int UC_IconSize { get; set; } = DoAn1.Properties.Settings.Default.ButtonSize;
        public int UC_FontSize { get; set; } = 12;
        public string UC_Background { get; set; } = "#01FFFFFF";

        public event RoutedEventHandler Click
        {
            add { button.AddHandler(ButtonBase.ClickEvent, value); }
            remove { button.RemoveHandler(ButtonBase.ClickEvent, value); }
        }

        public IconWithTextButtonUC()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
