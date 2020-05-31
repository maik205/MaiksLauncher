using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CmlLib.Core;
using System.Linq;
using CmlLib;
using MojangSharpCore;
using MojangSharpCore.Endpoints;
using MojangSharpCore.Responses;

namespace MaiksLauncher
{
    /// <summary>
    /// Interaction logic for MainWindowNew.xaml
    /// </summary>
    public partial class MainWindowNew : Window
    {
        public MainWindowNew()
        {
            InitializeComponent();
            MouseDown += MainWindow_MouseDown;
        }

        public static string accessToken;
        public static string Username;
        public static MSession MainSession;
        public static string userUUID;
        private string SelectedVersion;
        private int CurrentGrid;

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }


        private void PowerButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("MaiksLauncher.GUI"))
            {
                process.Kill();
            }
            Close();
        }

        private void LaunchClick(object sender, RoutedEventArgs e)
        {
            var session = new MSession (Username, accessToken, userUUID);
            var th = new Thread(new ThreadStart(delegate
            {
                CMLauncher launcher = new CMLauncher(Minecraft.GetOSDefaultPath());
                string selectedver = "";
                MSession sessionLaunch;
                Dispatcher.BeginInvoke(new Action( delegate
                {
                    selectedver = SelectedVersion;
                    sessionLaunch = session;
                }));
                var launchOptions = new MLaunchOption
                {
                    MaximumRamMb = 2048,
                    Session = session,
                };
                var process = launcher.CreateProcess(selectedver, launchOptions);
                process.Start();
            }));
            th.Start();
            
        }
        private void windowActive(object sender, RoutedEventArgs e)
        {
            username.Text = " " + Username;
            var th = new Thread(new ThreadStart(delegate
            {
                var McPath = Minecraft.GetOSDefaultPath();
                var Launcher = new CMLauncher(McPath);
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    foreach (var item in Launcher.UpdateProfiles())
                    {
                        // Cant figure this out for now, will look later
                        versionList.Items.Add(item);

                    }
                }));

            }));
            th.Start();
        }

        private void versionChange(object sender, SelectionChangedEventArgs e)
        {
            SelectedVersion = versionList.SelectedItem.ToString();

            // weird behaviour but fixed
            char[] versionSelectedChar = SelectedVersion.ToCharArray();
            if (versionSelectedChar[0] == ' ') { versionSelectedChar = versionSelectedChar.Skip(1).ToArray(); }
            string fixedVersionSlected = new string(versionSelectedChar);
            SelectedVersion = fixedVersionSlected;
        }

        private void PlayerInfoClick(object sender, RoutedEventArgs e)
        {
            PlayerInfo.Opacity = 100;
            PlayerInfoButton.IsEnabled = false;
            if (CurrentGrid == 0)
            {  Home.Opacity = 0; HomeButton.IsEnabled = true;}
            else if (CurrentGrid == 2)
            { Information.Opacity = 0; InformationButton.IsEnabled = true; }
            else if (CurrentGrid == 3)
            { Status.Opacity = 0; ServerStatusButton.IsEnabled = true; }
            else if (CurrentGrid == 4)
            { Settings.Opacity = 0; SettingsButton.IsEnabled = true; }
            CurrentGrid = 1;

        }

        private void InfoClick(object sender, RoutedEventArgs e)
        {
            Information.Opacity = 100;
            InformationButton.IsEnabled = false;
            if (CurrentGrid == 0)
            { Home.Opacity = 0; HomeButton.IsEnabled = true; }
            else if (CurrentGrid == 1)
            { PlayerInfo.Opacity = 0; PlayerInfoButton.IsEnabled = true; }
            else if (CurrentGrid == 3)
            { Status.Opacity = 0; ServerStatusButton.IsEnabled = true; }
            else if (CurrentGrid == 4)
            { Settings.Opacity = 0; SettingsButton.IsEnabled = true; }
            CurrentGrid = 2;
        }

        private void ServerStatusClick(object sender, RoutedEventArgs e)
        {
            Status.Opacity = 100;
            ServerStatusButton.IsEnabled = false;
            if (CurrentGrid == 3)
            { Home.Opacity = 0; HomeButton.IsEnabled = true; }
            else if (CurrentGrid == 1)
            { PlayerInfo.Opacity = 0; PlayerInfoButton.IsEnabled = true; }
            else if (CurrentGrid == 2)
            { Information.Opacity = 0; InformationButton.IsEnabled = true; }
            else if (CurrentGrid == 4)
            { Settings.Opacity = 0; SettingsButton.IsEnabled = true; }
            CurrentGrid = 1;
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            Settings.Opacity = 100;
            SettingsButton.IsEnabled = false;
            if (CurrentGrid == 0)
            { Home.Opacity = 0; HomeButton.IsEnabled = true; }
            else if (CurrentGrid == 1)
            { PlayerInfo.Opacity = 0; PlayerInfoButton.IsEnabled = true; }
            else if (CurrentGrid == 2)
            { Information.Opacity = 0; InformationButton.IsEnabled = true; }
            else if (CurrentGrid == 3)
            { Status.Opacity = 0; ServerStatusButton.IsEnabled = true; }
            CurrentGrid = 1;
        }

        private void HomeClick(object sender, RoutedEventArgs e)
        {
            PlayerInfo.Opacity = 100;
            PlayerInfoButton.IsEnabled = false;
            if (CurrentGrid == 1)
            { PlayerInfo.Opacity = 0; PlayerInfoButton.IsEnabled = true; }
            else if (CurrentGrid == 2)
            { Information.Opacity = 0; InformationButton.IsEnabled = true; }
            else if (CurrentGrid == 3)
            { Status.Opacity = 0; ServerStatusButton.IsEnabled = true; }
            else if (CurrentGrid == 4)
            { Settings.Opacity = 0; SettingsButton.IsEnabled = true; }
            CurrentGrid = 1;
        }

        private void InvalidateSessionClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
