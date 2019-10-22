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
namespace WirtualnyMagazyn.Views
{
    /// <summary>
    /// Interaction logic for History.xaml
    /// </summary>
    public partial class History : UserControl
    {
        /// <summary>
        /// polaczenie
        /// </summary>
        private static string conn = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True";
        public History()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Displaydata();
        }
        /// <summary>
        /// wywolanie danych z tablicy history
        /// </summary>
        void Displaydata()
        {
            using (SqlConnection myCon = new SqlConnection(conn))
            using (SqlCommand comm = new SqlCommand("SELECT Id, nazwa, rodzaj, CONVERT(varchar, data, 101)as data FROM History ORDER BY Id ASC", myCon))
            {
                SqlDataAdapter sda = new SqlDataAdapter(comm);
                DataTable dt = new DataTable("Awaiting");
                sda.Fill(dt);
                AwaitingDataGrid.ItemsSource = dt.DefaultView;
            }
        }
    }
}
