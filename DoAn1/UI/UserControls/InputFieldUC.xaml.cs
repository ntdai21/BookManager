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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DoAn1.UI.UserControls
{
    /// <summary>
    /// Interaction logic for InputFieldUC.xaml
    /// </summary>

    public partial class InputFieldUC : UserControl, INotifyPropertyChanged
    {
        public string UC_IsReadOnly { get; set; } = "False";
        public string UC_Title {  get; set; }
        public int UC_TitleWidth { get; set; } = 70;

        public static readonly DependencyProperty UC_IsEnabledProperty =
            DependencyProperty.Register("UC_IsEnabled", typeof(bool),
                typeof(InputFieldUC), new FrameworkPropertyMetadata(true));

        public bool UC_IsEnabled
        {
            get { return (bool)GetValue(UC_IsEnabledProperty); }
            set { SetValue(UC_IsEnabledProperty, value); }
        }

        public static readonly DependencyProperty UC_TextInputProperty =
            DependencyProperty.Register("UC_TextInput", typeof(String),
                typeof(InputFieldUC), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public String UC_TextInput
        {
            get { return GetValue(UC_TextInputProperty).ToString(); }
            set { SetValue(UC_TextInputProperty, value); }
        }

        public bool FiringValidationsAsDefault { get; set; } = true;

        public InputFieldUC()
        {
            InitializeComponent();

        }

        public bool HasError
        {
            get => Validation.GetHasError(textBox);
        }

        private void textBox_Loaded(object sender, RoutedEventArgs e)
        {
            BindingExpression mainTxtBxBinding = BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty);
            BindingExpression textBinding = BindingOperations.GetBindingExpression(this, UC_TextInputProperty);

            if (textBinding != null && mainTxtBxBinding != null && textBinding.ParentBinding != null && textBinding.ParentBinding.ValidationRules.Count > 0 && mainTxtBxBinding.ParentBinding.ValidationRules.Count < 1)
            {
                foreach (ValidationRule vRule in textBinding.ParentBinding.ValidationRules)
                    mainTxtBxBinding.ParentBinding.ValidationRules.Add(vRule);
            }

            if (FiringValidationsAsDefault) textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }
    }
}
