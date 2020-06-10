using System;
using System.Collections.Generic;
using System.Printing;
using System.Text;
using System.Text.RegularExpressions;
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
using MaiksLauncher.GUI;

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
                var result = login.Authenticate(email, password);
                if (result.Result == MLoginResult.Success)
                {
                    Session = result;
                    Dispatcher.BeginInvoke(new Action(delegate
                    {
                        LoginStatus.Foreground = Brushes.LightGreen;
                        LoginStatus.Text = "Successful login! Transfering you to the main window...";
                        this.Hide();

                        MainWindowNew mw = new MainWindowNew(Session);
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
                Regex r = new Regex("^[a-zA-Z0-9_]+$");
                if (r.IsMatch(Email.Text))
                {
                    MainWindowNew mw = new MainWindowNew(MSession.GetOfflineSession(Email.Text));
                    mw.Show();
                    this.Close();
                }
                else
                {
                    LoginStatus.Text = "Invalid characters in your username";
                    Progress.Opacity = 0;
                }
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
                MainWindowNew mw = new MainWindowNew(session);
                this.Close();

                mw.Show();

            }
            else
            {
                LoginCover.Visibility = Visibility.Hidden;
                LoginCover.IsEnabled = false;
            }
        }

        private void LoginActive(object sender, RoutedEventArgs e)
        {

            var th = new Thread(new ThreadStart(delegate
            {
                var login = new MLogin();
                var session = login.TryAutoLogin();
                if (session.Result == MLoginResult.Success)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        {
                            MainWindowNew mw = new MainWindowNew(session);
                            mw.Show();
                            this.Close();
                        }
                    });
                }
                else
                {
                    Dispatcher.BeginInvoke(new Action(delegate
                    {
                        LoginCover.Visibility = Visibility.Hidden;
                        LoginCover.IsEnabled = false;

                    }));
                }
            }));
            th.Start();
        }

        private void OldLookClick(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}
