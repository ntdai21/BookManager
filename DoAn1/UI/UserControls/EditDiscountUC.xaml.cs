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
    /// Interaction logic for EditDiscountUC.xaml
    /// </summary>
    /// 
    public partial class EditDiscountUC : UserControl
    {
        public bool ID_Enabled { get; set; } = true;

        public EditDiscountUC()
        {
            InitializeComponent();
        }

        public bool HasError
        {
            get
            {
                return codeTextBox.HasError || discountPercentTextBox.HasError || maxDiscountTextBox.HasError;
            }
        }
    }
}
