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

namespace WirtualnyMagazyn.Views
{
    /// <summary>
    /// Interaction logic for MainPanel.xaml
    /// </summary>
    /// 
    public partial class MainPanel : UserControl
    {


        /// <summary>
        /// zmiane datacontextow oraz kosmetyka przyciskow (border)
        /// </summary>


        public MainPanel()
        {
            ((MainWindow)App.Current.MainWindow).LoginNameTopBar();
            InitializeComponent();
            hello1.DataContext = new InStockModel();
            ZeroThickness();
            INSTOCK.BorderThickness = new Thickness(0, 0, 10, 0);
        }
        
         
        private void ZeroThickness()
        {
            INSTOCK.BorderThickness = new Thickness(0, 0, 0, 0);
            AWAITING.BorderThickness = new Thickness(0, 0, 0, 0);
            SENT.BorderThickness = new Thickness(0, 0, 0, 0);
            HISTORY.BorderThickness = new Thickness(0, 0, 0, 0);

        }

        private void INSTOCK_Click(object sender, RoutedEventArgs e)
        {
            hello1.DataContext = new InStockModel();
            ZeroThickness();
            INSTOCK.BorderThickness = new Thickness(0, 0, 10, 0);
        }

        private void AWAITING_Click(object sender, RoutedEventArgs e)
        {
            hello1.DataContext = new AwaitingModel();
            ZeroThickness();
            AWAITING.BorderThickness = new Thickness(0, 0, 10, 0);
        }

        private void SENT_Click(object sender, RoutedEventArgs e)
        {
            hello1.DataContext = new SentModel();
            ZeroThickness();
            SENT.BorderThickness = new Thickness(0, 0, 10, 0);
        }

        private void HISTORY_Click(object sender, RoutedEventArgs e)
        {
            hello1.DataContext = new HistoryModel();
            ZeroThickness();
            HISTORY.BorderThickness = new Thickness(0, 0, 10, 0);
        }
    }
}
