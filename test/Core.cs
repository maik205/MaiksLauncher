using System;
using System.Collections.Generic;
using System.Text;
using CmlLib;
using CmlLib.Core;
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
            return accessToken;
        }
        MSession Login()
        {
            bool ifPremium = true;
            var session =  new MSession();

            if (ifPremium)
            {
                var login = new MLogin();
                session = login.TryAutoLogin();
                session = login.Authenticate(MainWindow.loginEmail, MainWindow.loginPass);
                if (session.Result != MLoginResult.Success)
                {
                    ErrorLog = "Unsuccessful Login";
                    return null;
                }
                accessToken = session.AccessToken;
                

            }
            return session;
        }
        public static void LaunchMC()
        {

        }
        
    }
}
