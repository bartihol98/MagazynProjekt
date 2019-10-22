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
using WirtualnyMagazyn.ViewModels;
using WirtualnyMagazyn.Views;
using System.Threading;
using System.Data;
using System.Data.SqlClient;

namespace WirtualnyMagazyn.Views
{
    /// <summary>
    /// Interaction logic for LoginPanel.xaml
    /// </summary>
    public partial class LoginPanel : UserControl
    {
        /// <summary>
        /// polaczenie
        /// </summary>
        private static string conn = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True";
        public LoginPanel()
        {
            InitializeComponent();
            ((MainWindow)App.Current.MainWindow).LoginNameTopBar();
        }
        /// <summary>
        /// sprawdzenie czy wpisany login i haslo sa poprawne i wtedy zalogowanie uzytkownika
        /// </summary>
        void UserLogIn(string login, string pwd)
        {
            using (SqlConnection myCon = new SqlConnection(conn))
            using (SqlCommand comm = new SqlCommand("SELECT login, pwd FROM Users WHERE CONVERT(VARCHAR,login) = @login", myCon))
            {
                myCon.Open();
                comm.Parameters.AddWithValue("@login", login);
                SqlDataReader reader = comm.ExecuteReader();
                while(reader.Read())
                {
                    string password = reader["pwd"].ToString();
                    if( password == pwd)
                    {
                        ((MainWindow)App.Current.MainWindow).DataContext = new MainPanelModel();
                    }
                    else
                    {
                        MessageBox.Show("Wrong login or password");
                    }
                }
            }

        }

        /// <summary>
        /// l;ogowanie uzytkownika
        /// </summary>
        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginValue.Text;
            string pwd = PwdValue.Password;
            if ((login.Length > 3) && (login is string))
                {
                if ((pwd.Length > 3) && (pwd is string))
                    {
                    UserLogIn(login, pwd);
                }
                else MessageBox.Show("Wrong password");

            }
            else MessageBox.Show("Wrong login");
            Thread.Sleep(500);
           ((MainWindow)App.Current.MainWindow).Login = LoginValue.Text;

        }
        /// <summary>
        /// wiadomosc do przycisku help
        /// </summary>
        private void help_button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Prosze skontaktuj sie z administratorem.");
        }
    }
}
