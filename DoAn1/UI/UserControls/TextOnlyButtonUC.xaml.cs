using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for TextOnlyButtonUC.xaml
    /// </summary>
    public partial class TextOnlyButtonUC : UserControl, INotifyPropertyChanged
    {
        public string UC_Foreground { get; set; } = DoAn1.Properties.Settings.Default.ColorPalatte_Background;
        public string UC_Text { get; set; }
        public string UC_Background { get; set; } = DoAn1.Properties.Settings.Default.ColorPalatte_ButtonColor;

        public event RoutedEventHandler Click
        {
            add { button.AddHandler(ButtonBase.ClickEvent, value); }
            remove { button.RemoveHandler(ButtonBase.ClickEvent, value); }
        }

        public TextOnlyButtonUC()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
