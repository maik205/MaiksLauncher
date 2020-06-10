using CmlLib.Core;
using MaiksLauncher;
using System;
using System.ComponentModel;


namespace MaiksLauncher.Core
{
    public class MainCore
    {
        // initialize the logger
        public static Logger MainLoggerWindow = new Logger();

        // strings
        private static string accessToken;

        // settings
        public static int maxRamMB;
        public static string offlineUsername;
        public static string GetAccessToken(string password, string email)
        {
            var p = new MaiksLauncher.Core.MainCore();
            var session = p.Login(email, password, true);
            LoggerUpdate("[Auth] Started getting access token");
            return accessToken;
        }

        public static void Launch(string version, string email, string password)
        {
            var p = new Core.MainCore();
            var session = p.Login(email, password, true);
            p.start(session, version);
        }

        public MSession Login(string loginEmail, string LoginPass, bool ifPremium)
        {
            var session = new MSession();

            if (ifPremium)
            {
                var login = new MLogin();
                session = login.TryAutoLogin();
                session = login.Authenticate(loginEmail, LoginPass);
                if (session.Result != MLoginResult.Success)
                {
                    LoggerUpdate("[Auth]" + "Unsuccessful Login");
                    return null;
                }
                else { LoggerUpdate("[Auth] Successful Login"); }
                accessToken = session.AccessToken;
            }
            else
            {
                session = MSession.GetOfflineSession(offlineUsername);
            }
            return session;
        }

        public void start(MSession session, string version)
        {
            var McPath = Minecraft.GetOSDefaultPath();
            var launcher = new CmlLib.CMLauncher(McPath);
            // ignore 2 errors
            var launchOptions = new MLaunchOption
            {
                MaximumRamMb = 2048,
                Session = session,
            };
            var process = launcher.CreateProcess(version, launchOptions);
            process.Start();
        }

        private int nextline = -1;

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


    }
}