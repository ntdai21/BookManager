using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace DoAn1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            FrameworkElement.StyleProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata
            {
                DefaultValue = Application.Current.FindResource(typeof(Window))
            });

            //if (Debugger.IsAttached)
            //    DoAn1.Properties.Settings.Default.Reset();
        }
    }

}
