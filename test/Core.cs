using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CmlLib;
using CmlLib.Core;
using MaiksLauncher;
using System.ComponentModel;

namespace test
{
    class Core
    {
        // initialize the logger
        public static Logger MainLoggerWindow = new Logger();
        // strings
        static string accessToken;
        // settings
        public static int maxRamMB;

        public static void openLogger()
        {
            MainLoggerWindow.Show();
        }
        private void LoggerWindow_Closing(object sender, CancelEventArgs e)
        {
            MainLoggerWindow.LoggerWindow.Hide();
        }
        public static string GetAccessToken()
        {
            
           
            var p = new Core();
            var session = p.Login();
            var log = new Logger();
            LoggerUpdate("[Auth] Started getting access token");
            return accessToken;
            
        }
        public static void Launch()
        {
            var p = new Core();
            var session = p.Login();
            p.start(session);
        }
        public MSession Login()
        {
            bool ifPremium = true;
            var session =  new MSession();

            if (ifPremium)
            {
                var login = new MLogin();
                var log = new Logger();
                session = login.TryAutoLogin();
                session = login.Authenticate(MainWindow.loginEmail, MainWindow.loginPass);
                if (session.Result != MLoginResult.Success)
                {
                    LoggerUpdate("[Auth]" + "Unsuccessful Login");
                    return null;
                }
                else { LoggerUpdate("[Auth] Successful Login"); }
                accessToken = session.AccessToken;
            }
            else
            { // USERNAME NEEDED
                
                session = MSession.GetOfflineSession("username");
            }
            return session;
        }
        public void start(MSession session)
        {
            var log = new Logger();
            var McPath = Minecraft.GetOSDefaultPath();
            var launcher = new CmlLib.CMLauncher(McPath);
            // ignore 2 errors
            launcher.ProgressChanged += Downloader_ChangeProgress;
            launcher.FileChanged += Downloader_ChangeFile;
            LoggerUpdate("[Game] Initialized in " + launcher.Minecraft.path);
            launcher.UpdateProfiles();
            if (MainWindow.ifShowVersions = true)
            {
                LoggerUpdateNoNewLine("Versions available: ");

                foreach (var item in launcher.Profiles)
                {
                    LoggerUpdateNoNewLine(item.Name + " ");
                }
            }
            var launchOptions = new MLaunchOption
            {
                MaximumRamMb = 2048,
                Session = session,
            };
            LoggerUpdate("[STUBBED] Input version: ");
            var process = launcher.CreateProcess(MainWindow.versionSelected , launchOptions);

            LoggerUpdate("[Game] Launch Arguments: " + process.StartInfo.Arguments);
            process.Start();
        }
        int nextline = -1;
        private void Downloader_ChangeProgress(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (nextline < 0)
                return;

            
        }
        private void Downloader_ChangeFile(DownloadFileChangedEventArgs e)
        {
            if (e.FileKind == MFile.Resource)
            {
                

                if (e.ProgressedFileCount < e.TotalFileCount)
                { }    
            }
            else
            {
                
            }
        }
        public static void LoggerUpdate(string UpdateText)
        {
            MainLoggerWindow.Log.Text += UpdateText + Environment.NewLine;
        }
        public static void LoggerUpdateNoNewLine(string UpdateText)
        {
            MainLoggerWindow.Log.Text += UpdateText;
        }
        public static void LoggerUpdate(string UpdateText, string Color)
        {
            if (Color == "red")
            {
                
            }
        }
        public static void LoggerUpdateNoNewLine(string UpdateText, string Color)
        {
            MainLoggerWindow.Log.Text += UpdateText;
        }
    }
        
}

