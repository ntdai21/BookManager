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
    /// Interaction logic for TextOnlyOutlineButtonUC.xaml
    /// </summary>
    public partial class TextOnlyOutlineButtonUC : UserControl
    {
        public string UC_Foreground { get; set; } = "#593122";
        public string UC_Text { get; set; }

        public event RoutedEventHandler Click
        {
            add { button.AddHandler(ButtonBase.ClickEvent, value); }
            remove { button.RemoveHandler(ButtonBase.ClickEvent, value); }
        }

        public TextOnlyOutlineButtonUC()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
