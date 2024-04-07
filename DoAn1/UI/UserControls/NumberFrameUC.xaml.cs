﻿using System;
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
    /// Interaction logic for NumberFrameUC.xaml
    /// </summary>
    public partial class NumberFrameUC : UserControl, INotifyPropertyChanged
    {
        public string UC_Title { get; set; } = "Title";
        public double UC_Number
        {
            get { return (double)GetValue(UC_NumberProperty); }
            set { SetValue(UC_NumberProperty, value); }
        }

        public static readonly DependencyProperty UC_NumberProperty =
            DependencyProperty.Register("UC_Number", typeof(double),
                typeof(NumberFrameUC), new FrameworkPropertyMetadata(0.0));

        public string UC_Foreground { get; set; } = DoAn1.Properties.Settings.Default.ColorPalatte_Background;
        public string UC_BorderBrush { get; set; } = DoAn1.Properties.Settings.Default.ColorPalatte_SubBackground;

        public static readonly DependencyProperty UC_BackgroundProperty =
            DependencyProperty.Register("UC_Background", typeof(String),
                typeof(NumberFrameUC), new FrameworkPropertyMetadata(DoAn1.Properties.Settings.Default.ColorPalatte_DarkerSubBackground));

        public String UC_Background
        {
            get { return GetValue(UC_BackgroundProperty).ToString(); }
            set { SetValue(UC_BackgroundProperty, value); }
        }

        public NumberFrameUC()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
