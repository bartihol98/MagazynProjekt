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
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;

namespace WirtualnyMagazyn.Views
{
    /// <summary>
    /// Interaction logic for Awaiting.xaml
    /// </summary>
    public partial class Awaiting : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// polaczenie
        /// </summary>
        private static string conn = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True";
        /// <summary>
        /// posluzenie sie eventem propertychanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// lista ktora sie poslugujemy pozniej
        /// </summary>
        List<int> Lista = new List<int>();
        public Awaiting()
        {
            InitializeComponent();
            PopulateCombobox();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Displaydata();
        }
        /// <summary>
        /// zapelnienie comboboxa wartosciami od 1 do 100
        /// </summary>
        private void PopulateCombobox()
        {
            Lista.Clear();
            GenerateNumbersForList();
            Combobox_Addbar.ItemsSource = Lista;
        }
        /// <summary>
        /// wygenerowanie wartosci dla funkcji PopulateCombobox()
        /// </summary>
        private void GenerateNumbersForList(int n = 100)
        {
            for (int i = 1; i <= n; i++)
            {
                Lista.Add(i);
            }
        }
        /// <summary>
        /// polaczenie z baza i zapelnienie tabeli
        /// </summary>
        void Displaydata()
        {
            using (SqlConnection myCon = new SqlConnection(conn))
            using (SqlCommand comm = new SqlCommand("SELECT Id, nazwa, ilosc, CONVERT(varchar, data, 101)as data FROM Awaiting ORDER BY Id ASC", myCon))
            {

                SqlDataAdapter sda = new SqlDataAdapter(comm);
                DataTable dt = new DataTable("Awaiting");
                sda.Fill(dt);
                AwaitingDataGrid.ItemsSource = dt.DefaultView;
            }
        }
        /// <summary>
        /// wrzucenie danych do bazy danych
        /// </summary>
        private void Create_Task_Query(string nazwa,int ilosc, string date)
        {
            SqlCommand command = new SqlCommand();

            using (SqlConnection myCon = new SqlConnection(conn))
            using (myCon)
            {

                
                command.CommandText = "INSERT INTO Awaiting ( nazwa, ilosc, data) VALUES (@nazwa, @ilosc, @data)";
                command.Parameters.AddWithValue("@nazwa", nazwa);
                command.Parameters.AddWithValue("@ilosc", ilosc);
                command.Parameters.AddWithValue("@data", date);

                using(command)
                {
                    myCon.Open();
                    command.Connection = myCon;
                    command.ExecuteNonQuery();
                    Displaydata();
                    raiseEventThatPropertyChanged("DataContext");
                    string HistoryName = "Dodano transport "+ nazwa+" sztuk: "+ilosc +" na "+date;

                    HistoryInsert(HistoryName, " W AWAITNG ");
                }

            }
        }
        /// <summary>
        /// funkcja pomagajaca nam skontaktowac sie z delegatem(eventem) propertychanged co pozwala na zresetowanie datacontext
        /// </summary>
        public void raiseEventThatPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// funkcja zwracajaca date
        /// </summary>
        public static string GetDate()
        {
            DateTime now = DateTime.Now;
            var czas = now.ToShortDateString();
            string[] data = czas.Split('.', '/', '-');
            string datax = data[2] + data[1] + data[0];
            return datax;
        }
        /// <summary>
        /// funkcja ktora dodaje dane do tabeli history
        /// </summary>
        private void HistoryInsert(string nazwa, string rodzaj)
        {
            string datax = GetDate();
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
        /// funkcja przy przycisku addaccept
        /// </summary>
        private void addaccept_Click(object sender, RoutedEventArgs e)
        {
            string nazwa = NameofProduct.Text;
            int ilosc = Convert.ToInt32(Combobox_Addbar.SelectedItem);
            var SavedDate = Date_Task.SelectedDate.Value.Date;
            string datax = SavedDate.ToShortDateString();
            string[] task_date = datax.Split('.', '/', '-'); // [0] = dzien, [1] miesiac [2] rok TASKA
            datax = task_date[2] + task_date[1] + task_date[0];
            if(nazwa.Length > 1)
            {
                if(ilosc > 0)
                {
                    if(datax.Length > 0)
                    {
                        try
                        {
                            Create_Task_Query(nazwa, ilosc, datax);
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
