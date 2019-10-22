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

namespace WirtualnyMagazyn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// zmienna login w ktorej zapisywany jest login zalogowanego uzytkownika przy pomyslnym logowaniu
        /// </summary>
        private string login = "";
        private int userLevel = 0;
        public int Userlevel
        {
            get { return userLevel; }
            set {
                    userLevel = value;
                }
        }
        public string Login
        {
            get
            {
                return login;
            }
            set
            {

                if ((value.Length > 3) && (value is string))
                {
                    login = value;
                }
                else if (value == "")
                    login = value;
            }
        }
        /// <summary>
        /// Zapisanie nazwy zalogowanego uzytkownika u gory aplikacji
        /// </summary>
        public void LoginNameTopBar()
        {
            if(!(login == ""))
               UserNameTopBar.Text = "Witaj : " + Login;
            if (login == "")
                UserNameTopBar.Text = "";
        }

        /// <summary>
        /// Rozpoczecie inicjalizacji glownego programu i od razu zmienienie datacontext na usercontrol z panelem logowania
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new LoginPanelModel();
            
        }
        /// <summary>
        /// custom nav panel z mozliwoscia przesuwania okienka
        /// </summary>
        public void TopNavPanel(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        /// <summary>
        /// przycisk do minimalizowania okienka
        /// </summary>
        private void MinimizeProgram_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        /// <summary>
        /// przycisk do wylaczenia aplikacji
        /// </summary>
        private void ShutdownProgram_Button(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        /// <summary>
        /// przycisk wylogywuwajacy zmieniajacy zmienna przechowujaca nazwe uzytkownika na poczatku klasy MainWindow oraz cofniecie datacontext na panel logowania.
        /// </summary>
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            login = "";
            this.DataContext = new LoginPanelModel();
            LoginNameTopBar();
        }
    }
}
