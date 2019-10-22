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
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace WirtualnyMagazyn.Views
{
    /// <summary>
    /// Interaction logic for Sent.xaml
    /// </summary>
    public partial class Sent : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// polaczenie
        /// </summary>
        private static string conn = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True";
        /// <summary>
        /// propertychanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// lista
        /// </summary>
        List<int> Lista = new List<int>();
        public Sent()
        {
            InitializeComponent();
            PopulateCombobox();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Displaydata();
        }
        /// <summary>
        /// zapelnienie comboboxa
        /// </summary>
        private void PopulateCombobox()
        {
            Lista.Clear();
            GenerateNumbersForList();
            Combobox_Addbar.ItemsSource = Lista;
        }
        /// <summary>
        /// wygenerowanie wartosci dla fukncji populate combobox()
        /// </summary>
        private void GenerateNumbersForList(int n = 100)
        {
            for (int i = 1; i <= n; i++)
            {
                Lista.Add(i);
            }
        }
        /// <summary>
        /// wyciagniecie danych z tablicy
        /// </summary>
        void Displaydata()
        {
            using (SqlConnection myCon = new SqlConnection(conn))
            using (SqlCommand comm = new SqlCommand("SELECT Id, nazwa, ilosc, klient FROM Sent ORDER BY Id ASC", myCon))
            {

                SqlDataAdapter sda = new SqlDataAdapter(comm);
                DataTable dt = new DataTable("Awaiting");
                sda.Fill(dt);
                AwaitingDataGrid.ItemsSource = dt.DefaultView;
            }
        }
        /// <summary>
        /// wrzucenie danych do tablciy history
        /// </summary>
        private void HistoryInsert(string nazwa, string rodzaj)
        {
            DateTime now = DateTime.Now;
            var czas = now.ToShortDateString();
            string[] data = czas.Split('.', '/', '-');
            string datax = data[2] + data[1] + data[0];
            using (SqlConnection myCon = new SqlConnection(conn))
            using (myCon)
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "INSERT INTO History (nazwa, rodzaj, data) VALUES (@nazwa, @rodzaj, @data)";
                command.Parameters.AddWithValue("@nazwa", nazwa);
                command.Parameters.AddWithValue("@rodzaj", rodzaj);
                command.Parameters.AddWithValue("@data", datax);
                using (command)
                {
                    myCon.Open();
                    command.Connection = myCon;
                    command.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// polaczenie z baza i wrzucenie danych
        /// </summary>
        private void Create_Task_Query(string nazwa, int ilosc, string klient)
        {
            SqlCommand command = new SqlCommand();

            using (SqlConnection myCon = new SqlConnection(conn))
            using (myCon)
            {


                command.CommandText = "INSERT INTO Sent ( nazwa, ilosc, klient) VALUES (@nazwa, @ilosc, @klient)";
                command.Parameters.AddWithValue("@nazwa", nazwa);
                command.Parameters.AddWithValue("@ilosc", ilosc);
                command.Parameters.AddWithValue("@klient", klient);

                using (command)
                {
                    myCon.Open();
                    command.Connection = myCon;
                    command.ExecuteNonQuery();
                    Displaydata();
                    raiseEventThatPropertyChanged("DataContext");
                    string HistoryName = "Dodano transport  " + nazwa + " sztuk: " + ilosc + " do : "+klient ;

                    HistoryInsert(HistoryName, " W SENT ");
                }

            }
        }
        /// <summary>
        /// event propertychanged pomagajacy nam zresetowac datacontext
        /// </summary>
        public void raiseEventThatPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// przycisk do wrzucania danych
        /// </summary>
        private void addaccept_Click(object sender, RoutedEventArgs e)
        {
            string nazwa = NameofProduct.Text;
            int ilosc = Convert.ToInt32(Combobox_Addbar.SelectedItem);
            string klient = NameofClient.Text;
            if (nazwa.Length > 1)
            {
                if (ilosc > 0)
                {
                    if (klient.Length > 0)
                    {
                        try
                        {
                            Create_Task_Query(nazwa, ilosc, klient);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Wystapil blad! :" + ex);
                        }

                    }
                }
            }
        }
    }
}
