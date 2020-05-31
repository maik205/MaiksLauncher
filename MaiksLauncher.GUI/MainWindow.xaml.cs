using CmlLib;
using CmlLib.Core;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MaiksLauncher.Core;

namespace MaiksLauncher.GUI
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
            // Get the versions for the ComboBox
            var McPath = Minecraft.GetOSDefaultPath();
            var Launcher = new CMLauncher(McPath);
            
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
            var p = new MaiksLauncher.Core.MainCore();

            var session = p.Login(loginEmail, loginPass, true);

            if (session != null)
            {
                p.start(session, versionSelected);
                LoginSuccess.Content = "Login Successful!";
                LoginUnsuccess.Content = "";
            }
            else
            {
                LoginUnsuccess.Content = "Login Unsuccessful! Please check your email/password";
                LoginSuccess.Content = "";
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            loginEmail = Email.Text;
        }
        private void OpenLog_Click(object sender, RoutedEventArgs e)
        {
            LoginNew mainWindowNew = new LoginNew();
            mainWindowNew.Show();
            this.Close();

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
        private bool tg = false;
        private void toggle(object sender, RoutedEventArgs e)
        {
            if (tg == false)
            {
                TestG.Visibility = Visibility.Hidden;
                TestG.Opacity = 0;
                tg = true;
            }
            else
            {
                TestG.Visibility = Visibility.Visible;
                TestG.Opacity = 100;
                tg = false;
            }
        }
    }
}