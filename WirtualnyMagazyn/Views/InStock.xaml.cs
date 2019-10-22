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
    /// Interaction logic for InStock.xaml
    /// </summary>
    public partial class InStock : UserControl , INotifyPropertyChanged
    {
        /// <summary>
        /// polaczenie
        /// </summary>
        private static string conn= @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True";
        /// <summary>
        /// kilka list 
        /// </summary>
        List<int> Lista = new List<int>();
        List<int> ListaEmpty = new List<int>();
        List<string> RemovingSource = new List<string>();

        /// <summary>
        /// posluzeniem sie eventem property changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public InStock()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Displaydata();
            PopulateCombobox();
            PopulateComboxboxRemoverBar();
        }
        /// <summary>
        /// wygenerowanei wartosci dla funkcji populate combobox
        /// </summary>
        private void GenerateNumbersForList(int n = 100)
        {
            for(int i = 1; i<=n;i++)
            {
                Lista.Add(i);
            }
        }
        /// <summary>
        /// wyciagniacie danych z db i wywolanie do datagridu
        /// </summary>
        void Displaydata()
        {
            using (SqlConnection myCon = new SqlConnection(conn))
            using(SqlCommand comm = new SqlCommand("SELECT Id, nazwa, ilosc FROM InStock ORDER BY Id ASC", myCon))
            {
                
                SqlDataAdapter sda = new SqlDataAdapter(comm);
                DataTable dt = new DataTable("InStock");
                sda.Fill(dt);
                InStockDataGrid.ItemsSource = dt.DefaultView;
            }
        }
        /// <summary>
        /// funkcja zapelnia wartosci comboboxa od 1 do 100 przy pomocy funkcji generate numberesfolist
        /// </summary>
        private void PopulateCombobox()
        {
            Lista.Clear();
            GenerateNumbersForList();
            Combobox_Addbar.ItemsSource = Lista;
        }
        /// <summary>
        /// zapelnienie comboboxa danymi z tablicy
        /// </summary>
        private void PopulateComboxboxRemoverBar()
        {
            RemovingSource.Clear();
            using (SqlConnection myCon = new SqlConnection(conn))
            using (myCon)
            {
                SqlCommand sqlCmd = new SqlCommand("SELECT nazwa FROM InStock", myCon);
                myCon.Open();
                SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    RemovingSource.Add(sqlReader["nazwa"].ToString());
                   // removerbar_checkbox.Items.Add(sqlReader["nazwa"].ToString());
                }

                sqlReader.Close();
            }
            removerbar_checkbox.ItemsSource = RemovingSource;
        }
        /// <summary>
        /// wrzucenie danych do history tablicy
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
        /// wrzucenie danych do tablicy
        /// </summary>
        private void InsertValues_Addbar()
        {
            string NameOfProduct = NameofProduct.Text;
            int ProductsNumber = Convert.ToInt32(Combobox_Addbar.SelectedItem);
            if(NameOfProduct is string)
            {
                using (SqlConnection myCon = new SqlConnection(conn))
                using (myCon)
                {
                    SqlCommand command = new SqlCommand();
                    command.CommandText = "INSERT INTO InStock (nazwa, ilosc) VALUES ( '" + NameOfProduct + "' , " + ProductsNumber + " );";
                    using (command)
                    {
                        myCon.Open();
                        command.Connection = myCon;
                        command.ExecuteNonQuery();
                        Displaydata();
                        PopulateComboxboxRemoverBar();
                        raiseEventThatPropertyChanged("DataContext");
                        string HistoryName = "Dodano przedmiot " + NameOfProduct + " ilosc : " + ProductsNumber;

                        HistoryInsert(HistoryName, " Do InStock ");
                    }
                }
            }
        }

        private void AddRemoveBar_Add_Button_Click(object sender, RoutedEventArgs e)
        {
            addremovebar.Visibility = Visibility.Hidden;
            AddBar.Visibility = Visibility.Visible;      
        }

        private void declineaccept_Click(object sender, RoutedEventArgs e)
        {
            AddBar.Visibility = Visibility.Hidden;
            addremovebar.Visibility = Visibility.Visible;
        }

        private void AddRemoveBar_Remove_Button_Click(object sender, RoutedEventArgs e)
        {
            addremovebar.Visibility = Visibility.Hidden;
            removerbar.Visibility = Visibility.Visible;
        }

        private void removebar_button_click(object sender, RoutedEventArgs e)
        {
            removerbar.Visibility = Visibility.Hidden;
            addremovebar.Visibility = Visibility.Visible;
        }

        private void Addaccept_Click(object sender, RoutedEventArgs e)
        {
            InsertValues_Addbar();
        }

        private void removerbar_checkbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //
        }
        /// <summary>
        /// po zaznaczeniu danego przedmiotu np lusterek, drugi combobox zapelniany jest prawdziwa liczba tego przemiotu
        /// </summary>
        private void removerbar_checkbox_DropDownClosed(object sender, EventArgs e)
        {
            Combobox_Removerbar.IsEnabled = false;
            Combobox_Removerbar.ItemsSource = ListaEmpty;
            Lista.Clear();

            int n = 0;
            try
            {


                string Value = (removerbar_checkbox.SelectedValue).ToString();
                using (SqlConnection myCon = new SqlConnection(conn))
                using (SqlCommand comm = new SqlCommand("SELECT ilosc FROM InStock WHERE CONVERT(VARCHAR,nazwa) = @value", myCon))
                {
                    myCon.Open();
                    comm.Parameters.AddWithValue("@value", Value);
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        n = Convert.ToInt32(reader["ilosc"]);
                        GenerateNumbersForList(n);
                        Combobox_Removerbar.IsEnabled = true;
                        Combobox_Removerbar.ItemsSource = Lista;
                    }
                }
            }
            catch(Exception)
            {
                // nothing;
            }
        }
        /// <summary>
        /// funkcja do propertychanged co pozwala na zresetowanie datacontext
        /// </summary>
        public void raiseEventThatPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// usuniecie jakichs przemiotow zaznaczonych w comboboxach
        /// </summary>
        private void RemoverBar_Accept_Button_Click(object sender, RoutedEventArgs e)
        {
            string name = (removerbar_checkbox.SelectedValue).ToString();
            int k = Convert.ToInt32(Combobox_Removerbar.SelectedValue);
            int item = Lista[Lista.Count - 1];
            k = item - k;   
            if (name.Length > 2)
            {
                if ((k > 0) && (k is int))
                {
                    using (SqlConnection myCon = new SqlConnection(conn))
                    using (SqlCommand comm = new SqlCommand("UPDATE InStock SET ilosc = @ilosck WHERE CONVERT(VARCHAR,nazwa) = @value ", myCon))
                    {
                        myCon.Open();
                        comm.Parameters.AddWithValue("@value", name);
                        if (k > 0)
                            comm.Parameters.AddWithValue("@ilosck", k);
                        else
                            comm.Parameters.AddWithValue("@ilosck", DBNull.Value);
                        int rows = comm.ExecuteNonQuery();
                        Displaydata();
                        raiseEventThatPropertyChanged("DataContext");
                        if (rows == 1) MessageBox.Show("Success");
                        string HistoryName = "Usunieto przedmiot " + name + " ilosc : " + k ;

                        HistoryInsert(HistoryName, " Z InStock ");
                        Combobox_Removerbar.IsEnabled = false;

                    }
                }

            }

            
        }
            
    }
}
