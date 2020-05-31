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
            var th = new Thread(new ThreadStart(delegate
            {
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

            //// weird behaviour but fixed
            //char[] versionSelectedChar = SelectedVersion.ToCharArray();
            //if (versionSelectedChar[0] == ' ') { versionSelectedChar = versionSelectedChar.Skip(1).ToArray(); }
            //string fixedVersionSlected = new string(versionSelectedChar);
            //SelectedVersion = fixedVersionSlected;
        }
    }
}
