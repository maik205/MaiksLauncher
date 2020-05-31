﻿using System;
using System.Collections.Generic;
using System.Printing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CmlLib.Core;
using MaiksLauncher.Core;

namespace MaiksLauncher
{
    /// <summary>
    /// Interaction logic for LoginNew.xaml
    /// </summary>
    public partial class LoginNew : Window
    {
        public LoginNew()
        {
            InitializeComponent();
        }

        private MSession Session;
        private bool isEncrypted = false;
        MainWindowNew mw = new MainWindowNew();

        private void SignIn(object sender, RoutedEventArgs e)
        {
            Progress.Value = 0;
            Progress.Opacity = 100;
            var th = new Thread(new ThreadStart(delegate
            {
                var login = new MLogin();
                string email = "";
                string password = "";
                this.Dispatcher.Invoke(() =>
                {
                    email = Email.Text;
                    password = userPassword.Password;
                });
                var result = login.Authenticate(email,password);
                if (result.Result == MLoginResult.Success)
                {
                    Session = result;
                    Dispatcher.BeginInvoke(new Action(delegate
                    {
                        MainWindowNew.accessToken = Session.AccessToken;
                        MainWindowNew.Username = Session.Username;
                        MainWindowNew.userUUID = Session.UUID;
                        LoginStatus.Foreground = Brushes.LightGreen;
                        LoginStatus.Text = "Successful login! Transfering you to the main window...";
                        this.Hide();
                        MainWindowNew mw = new MainWindowNew();
                       
                        mw.Show();
                    }));
                }
                else
                {
                    Dispatcher.BeginInvoke(new Action(delegate
                    {
                        LoginStatus.Foreground = Brushes.Red;
                        LoginStatus.Text = "Please check your email/password";
                        Progress.Opacity = 0;
                    }));
                }
            }));

                if (ifOffline.IsChecked != false)
                {
                    MainWindowNew.Username = Email.Text;
                    MainWindowNew mw = new MainWindowNew();
                    mw.Show();
                }
                else
                {
                    th.Start();
                }

        }

       
        // Checkboxes
        private void OfflineChecked(object sender, RoutedEventArgs e)
        {
            userPassword.IsEnabled = false;
        }

        private void OfflineUnchecked(object sender, RoutedEventArgs e)
        {
            userPassword.IsEnabled = true;
        }
        private void tryautologin()
        {
            var login = new MLogin();
            var session = login.TryAutoLogin();
            if (session.Result == MLoginResult.Success)
            {
                MainWindowNew mw = new MainWindowNew();
                MainWindowNew.MainSession = session;
                MainWindowNew.Username = session.Username;
                MainWindowNew.userUUID = session.UUID;
                this.Close();

                mw.Show();

            }
            else
            {
                LoginCover.Opacity = 0;
                LoginCover.IsEnabled = false;
            }
        }
        private void LoginActive(object sender, EventArgs e)
        {
            tryautologin();

        }


    }
}