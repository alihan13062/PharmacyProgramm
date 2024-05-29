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
    /// Логика взаимодействия для PharmacistWindow.xaml
    /// </summary>
    public partial class PharmacistWindow : Window
    {
        public PharmacistWindow()
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

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderCreate oc = new OrderCreate();
            oc.Show();
            this.Close();
        }
    }
}
