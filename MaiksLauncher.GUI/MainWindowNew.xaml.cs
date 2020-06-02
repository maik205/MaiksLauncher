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

        private CMLauncher launcher;
        private string SelectedVersion;
        private int CurrentGrid = 0;

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
                Application.Current.Dispatcher.Invoke((Action)delegate {
                    LaunchButton.IsEnabled = false;
                });
                string selectedver = "";
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    selectedver = SelectedVersion;
                }));
                var launchOptions = new MLaunchOption
                {
                    MaximumRamMb = 2048,
                    Session = MainSession,
                };
                var process = launcher.CreateProcess(selectedver, launchOptions);
                process.Start();
                Application.Current.Dispatcher.Invoke((Action)delegate {
                    LaunchButton.IsEnabled = true;
                });
            }));
            th.Start();
           
        }
        private void windowActive(object sender, RoutedEventArgs e)
        {
            username.Text = " " + Username;
            var th = new Thread(new ThreadStart(delegate
            {
                var McPath = Minecraft.GetOSDefaultPath();
                launcher = new CMLauncher(McPath);

                // you must write this because of cmllib.core bug. it will be fixed soon
                launcher.ProgressChanged += Launcher_ProgressChanged;
                launcher.FileChanged += Launcher_FileChanged;

                launcher.UpdateProfiles(); // this code will block ui, so it should run in thread

                Dispatcher.BeginInvoke(new Action(delegate // call UI Thread
                {
                    foreach (var item in launcher.Profiles)
                    {
                        if (item.MType == MProfileType.Release || item.MType == MProfileType.Custom)
                            versionList.Items.Add(item.Name);
                    }
                }));

            }));
            th.Start();
        }

        private void Launcher_FileChanged(DownloadFileChangedEventArgs e)
        {
            Console.WriteLine("[{0}] {1} - {2}/{3}", e.FileKind.ToString(), e.FileName, e.ProgressedFileCount, e.TotalFileCount);
        }

        private void Launcher_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            Console.WriteLine("{0}%", e.ProgressPercentage);
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
            PlayerInfo.Visibility = Visibility.Visible;
            PlayerInfoButton.IsEnabled = false;
            if (CurrentGrid == 0)
            {  Home.Visibility = Visibility.Hidden; ; HomeButton.IsEnabled = true;  }
            else if (CurrentGrid == 2)
            { Information.Visibility = Visibility.Hidden; ; InformationButton.IsEnabled = true; }
            else if (CurrentGrid == 3)
            { Status.Visibility = Visibility.Hidden; ; ServerStatusButton.IsEnabled = true;}
            else if (CurrentGrid == 4)
            { Settings.Visibility = Visibility.Hidden; ; SettingsButton.IsEnabled = true;}
            CurrentGrid = 1;

        }

        private void InfoClick(object sender, RoutedEventArgs e)
        {
            Information.Visibility = Visibility.Visible;
            InformationButton.IsEnabled = false;
            if (CurrentGrid == 0)
            { Home.Visibility = Visibility.Hidden; ; HomeButton.IsEnabled = true;}
            else if (CurrentGrid == 1)
            { PlayerInfo.Visibility = Visibility.Hidden; ; PlayerInfoButton.IsEnabled = true; }
            else if (CurrentGrid == 3)
            { Status.Visibility = Visibility.Hidden; ; ServerStatusButton.IsEnabled = true;  }
            else if (CurrentGrid == 4)
            { Settings.Visibility = Visibility.Hidden; ; SettingsButton.IsEnabled = true;}
            CurrentGrid = 2;
        }

        private void ServerStatusClick(object sender, RoutedEventArgs e)
        {
            Status.Visibility = Visibility.Visible;
            ServerStatusButton.IsEnabled = false;
            if (CurrentGrid == 3)
            { Home.Visibility = Visibility.Hidden;  HomeButton.IsEnabled = true; }
            else if (CurrentGrid == 1)
            { PlayerInfo.Visibility = Visibility.Hidden; ; PlayerInfoButton.IsEnabled = true; }
            else if (CurrentGrid == 2)
            { Information.Visibility = Visibility.Hidden; ; InformationButton.IsEnabled = true; }
            else if (CurrentGrid == 4)
            { Settings.Visibility = Visibility.Hidden; ; SettingsButton.IsEnabled = true; }
            CurrentGrid = 3;
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            Settings.Visibility = Visibility.Visible;
            SettingsButton.IsEnabled = false;
            if (CurrentGrid == 0)
            { Home.Visibility = Visibility.Hidden; ; HomeButton.IsEnabled = true;  }
            else if (CurrentGrid == 1)
            { PlayerInfo.Visibility = Visibility.Hidden; ; PlayerInfoButton.IsEnabled = true; }
            else if (CurrentGrid == 2)
            { Information.Visibility = Visibility.Hidden; ; InformationButton.IsEnabled = true; }
            else if (CurrentGrid == 3)
            { Status.Visibility = Visibility.Hidden; ; ServerStatusButton.IsEnabled = true;}
            CurrentGrid = 4;
        }
        private void HomeClick(object sender, RoutedEventArgs e)
        {
            Home.Visibility = Visibility.Visible;
            HomeButton.IsEnabled = false;
            if (CurrentGrid == 1)
            { PlayerInfo.Visibility = Visibility.Hidden; ; PlayerInfoButton.IsEnabled = true; }
            else if (CurrentGrid == 2)
            { Information.Visibility = Visibility.Hidden; ; InformationButton.IsEnabled = true; }
            else if (CurrentGrid == 3)
            { Status.Visibility = Visibility.Hidden; ; ServerStatusButton.IsEnabled = true; }
            else if (CurrentGrid == 4)
            { Settings.Visibility = Visibility.Hidden; ; SettingsButton.IsEnabled = true; }
            CurrentGrid = 0;
        }
        private void SignOutClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
