using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using test;

namespace MaiksLauncher
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
        }
        
        public void Update(string updateText)
        {
            Log.Text += updateText + Environment.NewLine;
        }
        public void UpdateNoNewLine(string updateText)
        {
            Log.Text += updateText;
        }
    }
}
