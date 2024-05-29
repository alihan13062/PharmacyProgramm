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
using System.Data.SqlClient;
using System.Windows.Navigation;
using System.Diagnostics;
using System.Data;
using DataTable = System.Data.DataTable;

namespace PharmacyProgramm
{
    /// <summary>
    /// Логика взаимодействия для WatchStuff.xaml
    /// </summary>
    public partial class WatchStuff : Window
    {
        private DataTable ordersTable;
        private DataView ordersView;
        public WatchStuff()
        {
            InitializeComponent();
            tablePrep();
        }
        static DataTable ExecuteSql(string sql)
        {
            DataTable DT = new DataTable();
            string connectionString = SqkConnectionString.GetConnectionSqlServer();
            SqlConnection sqlcon = new SqlConnection(connectionString);
            using (sqlcon)
            {
                sqlcon.Open();
                SqlCommand sqlcd = new SqlCommand(sql, sqlcon);
                SqlDataReader sqlread = sqlcd.ExecuteReader();
                using (sqlread)
                {
                    DT.Load(sqlread);
                }
            }
            return DT;
        }
        public void tablePrep()
        {
            ordersTable = ExecuteSql("SELECT * FROM Preparation");
            ordersView = new DataView(ordersTable);
            listOrder.ItemsSource = ordersView;
        }
    }
}
