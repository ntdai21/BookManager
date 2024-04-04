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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DoAn1.UI.UserControls
{
    /// <summary>
    /// Interaction logic for TextFieldUC.xaml
    /// </summary>
    public partial class TextFieldUC : UserControl, INotifyPropertyChanged
    {
        public TextFieldUC()
        {
            InitializeComponent();
        }
        public string UC_Title { get; set; }
        public int UC_TitleWidth { get; set; } = 70;
        public int UC_TitleSize { get; set; } = 14;

        public int UC_TextInputSize { get; set; } = 16;

        public string UC_InputTextFontWeight { get; set; }



        public string UC_TextInput
        {
            get { return (string)GetValue(UC_TextInputProperty); }
            set { SetValue(UC_TextInputProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UC_TextInput.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UC_TextInputProperty =
            DependencyProperty.Register("UC_TextInput", typeof(string), typeof(TextFieldUC), new PropertyMetadata("Test"));



    }
}
