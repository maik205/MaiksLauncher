using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CefSharp.Wpf;
using MojangSharp;
using MojangSharp.Api;
using MojangSharp.Endpoints;
using MojangSharp.Responses;
using System.IO;
using Newtonsoft.Json;
using static MojangSharp.Endpoints.Statistics;
using static MojangSharp.Responses.ChallengesResponse;
using static MojangSharp.Responses.NameHistoryResponse;


namespace test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }
        private string loginPass;
        private string loginEmail;
        
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
        }
        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            loginPass = Password.Password;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AuthenticateResponse auth = new Authenticate(new Credentials() { Username = loginEmail, Password = loginPass }).PerformRequestAsync().Result;
            if (auth.IsSuccess == true)
            {
                System.IO.File.WriteAllText(@"C:\Users\meik\TestFolder\WriteText.txt", auth.AccessToken);
                Response v = new Validate(auth.AccessToken).PerformRequestAsync().Result;
                if (v.IsSuccess == true)
                    AccessValidator.Content ="Validated!";
                else
                    AccessValidator.Content ="Not validated.";
            }
            else
            {
                PasswordCheck.Content = auth.Error.Exception == null ? auth.Error.ErrorMessage : auth.Error.Exception.Message;
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            loginEmail = Email.Text;
            TestEmail.Content = loginEmail;
        }
    }
}
