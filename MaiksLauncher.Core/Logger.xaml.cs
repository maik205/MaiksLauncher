using System;
using System.Windows;

namespace MaiksLauncher.Core
{
    /// <summary>
    /// Interaction logic for Logger.xaml
    /// </summary>
    public partial class Logger : Window
    {
        public static string log;

        public Logger()
        {
            InitializeComponent();
            Log.Text += Environment.NewLine;
        }
    }
}