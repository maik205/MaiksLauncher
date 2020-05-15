using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CmlLib;
using CmlLib.Core;
using MaiksLauncher;

namespace test
{
    class Core
    {
        public static string ErrorLog;
        static string accessToken;
        
        
        public static string GetAccessToken()
        {
           
            var p = new Core();
            var session = p.Login();
            var log = new Logger();
            log.Update("[Auth] Started getting access token");
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
                    log.Update("[Auth]" + "Unsuccessful Login");
                    return null;
                }
                else { log.Update("[Auth] Successful Login"); }
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
            log.Update("[Game] Initialized in " + launcher.Minecraft.path);
            launcher.UpdateProfiles();
            log.UpdateNoNewLine("Versions available: ");
            foreach (var item in launcher.Profiles)
            {
                log.UpdateNoNewLine(item.Name + " ");
            }
            var launchOptions = new MLaunchOption
            {
                MaximumRamMb = 1024,
                Session = session,
            };
            log.Update("[STUBBED] Input version: ");
            var process = launcher.CreateProcess("1.8.9", launchOptions);

            log.Update("[Game] Launch Arguments:" + process.StartInfo.Arguments);
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
    }
        
}

