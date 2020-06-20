using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Net.NetworkInformation;
using System.Windows.Navigation;
using CmlLib;
using MojangSharpCore;
using MojangSharpCore.Endpoints;
using MojangSharpCore.Responses;
using System.Security.Cryptography;
using System.Windows.Automation.Provider;
using MaiksLauncher.Core;

namespace MaiksLauncher
{
    /// <summary>
    /// Interaction logic for MainWindowNew.xaml
    /// </summary>
    public partial class MainWindowNew : Window
    {
        public MSession MainSession;

        public MainWindowNew(MSession session)
        {
            MainSession = session;
            InitializeComponent();
            MouseDown += MainWindow_MouseDown;
        }

        

        private CMLauncher launcher;
        //private string SelectedVersion;
        private int CurrentGrid = 0;

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }


        private void PowerButton_Click(object sender, RoutedEventArgs e)
        {
            saveInfo();
            foreach (var process in Process.GetProcessesByName("MaiksLauncher.GUI"))
            {
                process.Kill();
            }
            Close();
        }

        private void LaunchClick(object sender, RoutedEventArgs e)
        {
            LaunchProgress.Opacity = 100;
            var th = new Thread(new ThreadStart(delegate
            {
                var selectedver = "";

                Application.Current.Dispatcher.Invoke((Action)delegate {
                    LaunchButton.IsEnabled = false;
                    selectedver = versionList.Text;
                });
                var ThisThreadOptions = new MLaunchOption();
                Application.Current.Dispatcher.Invoke(delegate
                {
                    int screenHeight = 0;
                    int screenWidth = 0;
                    int MaxRamMB = Convert.ToInt32(MaxMemSlider.Value);
                    try
                    {
                        screenHeight = Convert.ToInt32(ScreenHeightBox.Text);
                        screenWidth = Convert.ToInt32(ScreenWidthBox.Text);
                    }
                    catch
                    {
                        screenHeight = 0;
                        screenWidth = 0;
                    }

                    var launchOptions = new MLaunchOption
                    {
                        JavaPath = JavaPathBox.Text,
                        ServerIp = ServerIPBox.Text,
                        ScreenHeight = screenHeight,
                        ScreenWidth = screenWidth,
                        MaximumRamMb = MaxRamMB,
                        Session = MainSession,
                        JVMArguments = CustomArgsBox.Text.Split(" ")
                    };
                    ThisThreadOptions = launchOptions;
                });

                var process = launcher.CreateProcess(selectedver, ThisThreadOptions);
                process.Start();

                Application.Current.Dispatcher.Invoke((Action)delegate {
                    LaunchButton.IsEnabled = true;
                    LaunchProgress.Visibility = Visibility.Hidden;
                    LaunchLog.Visibility = Visibility.Hidden;

                });
            }));
            th.Start();
        }
        private void windowActive(object sender, RoutedEventArgs e)
        {
            
            LaunchProgress.Opacity = 0;
            username.Text = " " + MainSession.Username;
            string config = ReadWrite.ReadConfig(1);
            var th = new Thread(new ThreadStart(delegate
            {
                
                Application.Current.Dispatcher.Invoke(delegate
                {
                    reloadVersions();
                    loadInfo();
                });
                getStatus();
                
            }));
            th.Start();
            string nameMcURL = "https://namemc.com/profile/" + MainSession.Username;
            nameMCLink.NavigateUri= new Uri(nameMcURL);
            
        }
        
        private void Launcher_FileChanged(DownloadFileChangedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                LaunchLog.Content = string.Format("[{0}] {1} - {2}/{3}", e.FileKind.ToString(), e.FileName, e.ProgressedFileCount, e.TotalFileCount);
                if (e.FileKind == MFile.Resource)
                {
                    LaunchProgress.Maximum = e.TotalFileCount;
                    LaunchProgress.Value = e.ProgressedFileCount;
                }
            }));
        }

        private void Launcher_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => {
                LaunchProgress.Maximum = 100;
                LaunchProgress.Value = e.ProgressPercentage;
            }));
        }

        private void PlayerInfoClick(object sender, RoutedEventArgs e)
        {
            playerinfoConfirm();
        }

        private void InfoClick(object sender, RoutedEventArgs e)
        {
            Information.Visibility = Visibility.Visible;
            InformationButton.IsEnabled = false;
            if (CurrentGrid == 0)
            { Home.Visibility = Visibility.Hidden; ; HomeButton.IsEnabled = true; }
            else if (CurrentGrid == 1)
            { PlayerInfo.Visibility = Visibility.Hidden; ; PlayerInfoButton.IsEnabled = true; }
            else if (CurrentGrid == 3)
            { Status.Visibility = Visibility.Hidden; ; ServerStatusButton.IsEnabled = true; }
            else if (CurrentGrid == 4)
            { Settings.Visibility = Visibility.Hidden; ; SettingsButton.IsEnabled = true; }
            CurrentGrid = 2;
        }

        private void ServerStatusClick(object sender, RoutedEventArgs e)
        {
            Status.Visibility = Visibility.Visible;
            ServerStatusButton.IsEnabled = false;
            if (CurrentGrid == 0)
            { Home.Visibility = Visibility.Hidden; HomeButton.IsEnabled = true; }
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
            { Home.Visibility = Visibility.Hidden; ; HomeButton.IsEnabled = true; }
            else if (CurrentGrid == 1)
            { PlayerInfo.Visibility = Visibility.Hidden; ; PlayerInfoButton.IsEnabled = true; }
            else if (CurrentGrid == 2)
            { Information.Visibility = Visibility.Hidden; ; InformationButton.IsEnabled = true; }
            else if (CurrentGrid == 3)
            { Status.Visibility = Visibility.Hidden; ; ServerStatusButton.IsEnabled = true; }
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
            MLogin mLogin = new MLogin();
            mLogin.Invalidate();
            LoginNew newLogin = new LoginNew();
            this.Close();
            newLogin.Show();
        }

        private void getStatus()
        {
            ApiStatusResponse status = new ApiStatus().PerformRequestAsync().Result;
            Ping ping = new Ping();
            var HypixelPingResult = ping.Send("mc.hypixel.net");
            var MineplexPingResult = ping.Send("us.mineplex.com");
            var MinemenPingResult = ping.Send("minemen.club");
            var WynncraftPingResult = ping.Send("play.wynncraft.com");
            Application.Current.Dispatcher.Invoke(delegate
            {
                if (HypixelPingResult.Status == IPStatus.Success)
                { HypixelPing.Text = HypixelPingResult.RoundtripTime.ToString(); HypixelStatus.Fill = Brushes.LawnGreen; }
                else
                { HypixelPing.Text = "Timed out"; HypixelStatus.Fill = Brushes.Red; }

                if (MinemenPingResult.Status == IPStatus.Success)
                { MinemenPing.Text = MinemenPingResult.RoundtripTime.ToString(); MinemenStatus.Fill = Brushes.LawnGreen; }
                else
                {  HypixelPing.Text = "Timed out"; HypixelStatus.Fill = Brushes.Red; }
                if (MineplexPingResult.Status == IPStatus.Success)
                { MineplexPing.Text = MineplexPingResult.RoundtripTime.ToString(); MineplexStatus.Fill = Brushes.LawnGreen; }
                else
                { HypixelPing.Text = "Timed out"; HypixelStatus.Fill = Brushes.Red; }
                if (WynncraftPingResult.Status == IPStatus.Success)
                { WynncraftPing.Text = WynncraftPingResult.RoundtripTime.ToString(); WynncraftStatus.Fill = Brushes.LawnGreen; }
                else
                { HypixelPing.Text = "Timed out"; HypixelStatus.Fill = Brushes.Red; }
            });
            
            if (status.IsSuccess)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    if (status.Mojang == ApiStatusResponse.Status.Available)
                    { MojangStatus.Fill = Brushes.LawnGreen; }
                    else if (status.Mojang == ApiStatusResponse.Status.SomeIssues)
                    { MojangStatus.Fill = Brushes.Yellow; }
                    else
                    { MojangStatus.Fill = Brushes.Red; }

                    if (status.Minecraft == ApiStatusResponse.Status.Available)
                    { MinecraftNetStatus.Fill = Brushes.LawnGreen; }
                    else if (status.Minecraft == ApiStatusResponse.Status.SomeIssues)
                    { MinecraftNetStatus.Fill = Brushes.Yellow; }
                    else
                    { MinecraftNetStatus.Fill = Brushes.Red; }

                    if (status.MojangAccounts == ApiStatusResponse.Status.Available)
                    { MojangAccStatus.Fill = Brushes.LawnGreen; }
                    else if (status.MojangAccounts == ApiStatusResponse.Status.SomeIssues)
                    { MojangAccStatus.Fill = Brushes.Yellow; }
                    else
                    { MojangAccStatus.Fill = Brushes.Red; }

                    if (status.MojangApi == ApiStatusResponse.Status.Available)
                    { MojangApiStatus.Fill = Brushes.LawnGreen; }
                    else if (status.MojangApi == ApiStatusResponse.Status.SomeIssues)
                    { MojangApiStatus.Fill = Brushes.Yellow; }
                    else
                    { MojangApiStatus.Fill = Brushes.Red; }

                    if (status.MojangAutenticationServers == ApiStatusResponse.Status.Available)
                    { MojangAuthServersStatus.Fill = Brushes.LawnGreen; }
                    else if (status.MojangAutenticationServers == ApiStatusResponse.Status.SomeIssues)
                    { MojangAuthServersStatus.Fill = Brushes.Yellow; }
                    else
                    { MojangAuthServersStatus.Fill = Brushes.Red; }

                    if (status.MojangAuthenticationService == ApiStatusResponse.Status.Available)
                    { MojangAuthSvcStatus.Fill = Brushes.LawnGreen; }
                    else if (status.MojangAuthenticationService == ApiStatusResponse.Status.SomeIssues)
                    { MojangAuthSvcStatus.Fill = Brushes.Yellow; }
                    else
                    { MojangAuthSvcStatus.Fill = Brushes.Red; }

                    if (status.MojangSessionsServer == ApiStatusResponse.Status.Available)
                    { MojangSessionServerStatus.Fill = Brushes.LawnGreen; }
                    else if (status.MojangSessionsServer == ApiStatusResponse.Status.SomeIssues)
                    { MojangSessionServerStatus.Fill = Brushes.Yellow; }
                    else
                    { MojangSessionServerStatus.Fill = Brushes.Red; }

                    if (status.Sessions == ApiStatusResponse.Status.Available)
                    { SessionStatus.Fill = Brushes.LawnGreen; }
                    else if (status.Sessions == ApiStatusResponse.Status.SomeIssues)
                    { SessionStatus.Fill = Brushes.Yellow; }
                    else
                    { SessionStatus.Fill = Brushes.Red; }

                    if (status.Skins == ApiStatusResponse.Status.Available)
                    { SkinStatus.Fill = Brushes.LawnGreen; }
                    else if (status.Skins == ApiStatusResponse.Status.SomeIssues)
                    { SkinStatus.Fill = Brushes.Yellow; }
                    else
                    { SkinStatus.Fill = Brushes.Red; }

                    if (status.Textures == ApiStatusResponse.Status.Available)
                    { TextureServerStatus.Fill = Brushes.LawnGreen; }
                    else if (status.Textures == ApiStatusResponse.Status.SomeIssues)
                    { TextureServerStatus.Fill = Brushes.Yellow; }
                    else
                    { TextureServerStatus.Fill = Brushes.Red; }
                });
               
            }
            else
            {
                Application.Current.Dispatcher.Invoke(delegate {
                    Error.Content = "Please check your internet connection";
                    Error.Foreground = Brushes.Red;
                });
            }
        }

        public static bool ifOfflineMode;

        private void setUserInfo(MSession session)
        {
            if (ifOfflineMode != false)
            {
                PlayerInfoName.Text = session.Username;
                PlayerInfoAToken.Text = "(Offline)";
                PlayerInfoClientToken.Text = "(Offline)";
                PlayerInfoUUID.Text = "(Offline)";
                
            }
            else
            {
                PlayerInfoName.Text = session.Username;
                PlayerInfoAToken.Text = session.AccessToken;
                PlayerInfoClientToken.Text = session.ClientToken;
                PlayerInfoUUID.Text = session.UUID;
            }
            
        }
        string BaseAnswer;
        int randomTest;
        private void playerinfoConfirm()
        {
            if (CurrentGrid == 0)
            { Home.Visibility = Visibility.Hidden; ; HomeButton.IsEnabled = true; }
            else if (CurrentGrid == 2)
            { Information.Visibility = Visibility.Hidden; ; InformationButton.IsEnabled = true; }
            else if (CurrentGrid == 3)
            { Status.Visibility = Visibility.Hidden; ; ServerStatusButton.IsEnabled = true; }
            else if (CurrentGrid == 4)
            { Settings.Visibility = Visibility.Hidden; ; SettingsButton.IsEnabled = true; }
            Confirm.Visibility = Visibility.Visible;
            Random rd = new Random();
            
            int i = 0;
            while (i <= 21)
            {
                randomTest = rd.Next(1, 3);
                i++;
            }
            
            if (randomTest == 1) { BaseAnswer = "love"; humanTestImg.Source = new BitmapImage(new Uri(@"\test1.png", UriKind.Relative)); }
            if (randomTest == 2) { BaseAnswer = "mine"; humanTestImg.Source = new BitmapImage(new Uri(@"\test2.png", UriKind.Relative)); }
            
        }

        private void humanTestSubmit(object sender, RoutedEventArgs e)
        {
            if (humanTestAnswer.Text.Equals(BaseAnswer)) {
                Confirm.Visibility = Visibility.Hidden;
                PlayerInfo.Visibility = Visibility.Visible;
                PlayerInfoButton.IsEnabled = false;
                CurrentGrid = 1;
            }
            else
            {
                foreach(var process in Process.GetProcessesByName("MaiksLauncher.GUI"))
                {
                    process.Kill();
                }
                this.Close();
            }
            
        }

        private void saveInfo()
        {
            ReadWrite.WriteConfigByLine(MaxMemSlider.Value.ToString(), 1);
            if (!string.IsNullOrEmpty(CustomArgsBox.Text))
            {
                ReadWrite.WriteConfigByLine(CustomArgsBox.Text,4);
            }

            if (!string.IsNullOrEmpty(JavaPathBox.Text))
            {
                ReadWrite.WriteConfigByLine(JavaPathBox.Text, 3);
            }

            if (!string.IsNullOrEmpty(ServerIPBox.Text))
            {
                ReadWrite.WriteConfigByLine(ServerIPBox.Text, 9);
            }

            if (!string.IsNullOrEmpty(ScreenHeightBox.Text))
            {
                ReadWrite.WriteConfigByLine(ScreenHeightBox.Text, 11);
            }

            if (!string.IsNullOrEmpty(ScreenWidthBox.Text))
            {
                ReadWrite.WriteConfigByLine(ScreenWidthBox.Text,10);
            }
        }

        private void loadInfo()
        {
            string MaxMem = ReadWrite.ReadConfig(2);
            int maxMem = Convert.ToInt32(MaxMem);
            MaxMemSlider.Value = maxMem;
            foreach (string ver in ReadWrite.LoadVersionList())
            {
                versionList.Items.Add(ver);
            }
            CustomArgsBox.Text = ReadWrite.ReadConfig(4);
            int verIndex = versionList.Items.IndexOf(ReadWrite.ReadConfig(2));
            versionList.SelectedIndex = verIndex;
            ScreenWidthBox.Text = ReadWrite.ReadConfig(19);
            ScreenHeightBox.Text = ReadWrite.ReadConfig(11);
            JavaPathBox.Text = ReadWrite.ReadConfig(3);
        }

        private void reloadVersions()
        {
            
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\MaiksLauncher\";
            File.WriteAllText(path + @"VersionList.mvl", String.Empty);
            var McPath = Minecraft.GetOSDefaultPath();
            launcher = new CMLauncher(McPath);
            // you must write this because of cmllib.core bug. it will be fixed soon
            launcher.ProgressChanged += Launcher_ProgressChanged;
            launcher.FileChanged += Launcher_FileChanged;
            launcher.UpdateProfiles(); 
            string[] vers = new string[launcher.Profiles.Length];// this code will block ui, so it should run in thread
            int index = 0;
            foreach (var profile in launcher.Profiles)
            {
                vers[index] = profile.Name;
            }

            StreamWriter sw = new StreamWriter(path + @"VersionList.mvl");
            foreach (string VARIABLE in vers)
            {
                sw.WriteLine(VARIABLE);
            }
            sw.Close();
            Application.Current.Dispatcher.Invoke(delegate
            {
                versionList.Items.Clear();
                foreach (string ver in vers)
                {
                    versionList.Items.Add(ver);
                }
            });
        }

        private void testSubmit(object sender, RoutedEventArgs e)
        {
            string test = ReadWrite.ReadConfig(Convert.ToInt32(configLine.Text));
            configReaded.Content = test;
            configLength.Content = test.Length;
        }
    }


    }
    

