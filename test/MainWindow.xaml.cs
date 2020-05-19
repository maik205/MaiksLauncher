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
using CefSharp.Wpf;
using System.IO;
using Newtonsoft.Json;
using CmlLib;
using CmlLib.Core;
using MaiksLauncher;

namespace test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string versionSelected;
        public static string finalForgeVersion;
        public static bool ifShowVersions = false;
        

        public MainWindow()
        {
            InitializeComponent();
            // Get the versions for The ComboBox
            var McPath = Minecraft.GetOSDefaultPath();
            var Launcher = new CMLauncher(McPath);         
            foreach (var item in Launcher.UpdateProfiles())
            {
                LauncherProfiles.Items.Add(item); 
            }
        }
        public static string loginPass;
        public static string loginEmail;
        
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
        }
        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            loginPass = Password.Password;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            loginPass = Password.Password;
            var p = new Core();
            
            var session = p.Login();
            
            if (session != null)
            {
                Core.LoggerUpdate("[Auth] Login successful");
                p.start(session);
                LoginSuccess.Content = "Login Successful!";
                LoginUnsuccess.Content = "";
            }
            else
            {
                Core.LoggerUpdate("[Auth] Login unsuccessful");

                LoginUnsuccess.Content = "Login Unsuccessful! Please check your email/password";
                LoginSuccess.Content = "";
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            loginEmail = Email.Text;
        }
        public static Logger LoggerMain() { return null; }

        private void OpenLog_Click(object sender, RoutedEventArgs e)
        {
            Core.openLogger();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            versionSelected = LauncherProfiles.SelectedItem.ToString();
            
                // weird behaviour but fixed
                char[] versionSelectedChar = versionSelected.ToCharArray();
                if (versionSelectedChar[0] == ' ') { versionSelectedChar = versionSelectedChar.Skip(1).ToArray(); }
                string fixedVersionSlected = new string(versionSelectedChar);
                versionSelected = fixedVersionSlected;
            
            
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            ifShowVersions = true;
        }

        private void ifShowVersion_Unchecked(object sender, RoutedEventArgs e)
        {
            ifShowVersions = false;
        }
    }
}
