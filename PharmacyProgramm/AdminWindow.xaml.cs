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
using System.Windows.Shapes;

namespace PharmacyProgramm
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mn = new MainWindow();
            mn.Show();
            this.Close();
        }

        private void btnWatchOrder_Click(object sender, RoutedEventArgs e)
        {
            WatchOrder wo = new WatchOrder();
            wo.Show();
        }

        private void btnWatchEmp_Click(object sender, RoutedEventArgs e)
        {
            WatchEmployee we = new WatchEmployee();
            we.Show();
        }

        private void btnWatchPrep_Click(object sender, RoutedEventArgs e)
        {
            WatchStuff ws = new WatchStuff();
            ws.Show();
        }

        private void btnEditOrder_Click(object sender, RoutedEventArgs e)
        {
            EditOrder eo = new EditOrder();
            eo.Show();
            this.Close();
        }

        private void btnEditEmp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditPrep_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
