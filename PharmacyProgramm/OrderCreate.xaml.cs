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
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;

namespace PharmacyProgramm
{
    /// <summary>
    /// Логика взаимодействия для OrderCreate.xaml
    /// </summary>
    public partial class OrderCreate : Window
    {
        string connectionString = SqkConnectionString.GetConnectionSqlServer();
        public OrderCreate()
        {
            InitializeComponent();
            EmpName();
            PrepName();
        }
        private void rus (object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Я]");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void noEng (object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z]");
            e.Handled = !regex.IsMatch(e.Text);
        }
        private void number (object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void MyDatePicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        public void EmpName()
        {
            combEmp.Items.Clear();
            string query = "SELECT E_Surname FROM Employee where JobTitleID = 2";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string postdName = reader.GetString(0);
                        combEmp.Items.Add(postdName);
                    }
                }
            }
        }
        public void PrepName()
        {
            combPrep.Items.Clear();
            string query = "SELECT P_Title FROM Preparation";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string postdName = reader.GetString(0);
                        combPrep.Items.Add(postdName);
                    }
                }
            }
        }

        private void btnCalc_Click(object sender, RoutedEventArgs e)
        {
            if (combPrep.SelectedIndex == -1 || txtCount.Text == "")
            {
                MessageBox.Show("Выберите препарат и его количество!");
            }
            else
            {
                string prepName = Convert.ToString(combPrep.SelectedItem);
                string query = "SELECT P_Price FROM Preparation where P_Title = '" + prepName + "'";

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    int prepPrice = Convert.ToInt32(command.ExecuteScalar());
                    int prepCount = Convert.ToInt32(txtCount.Text);
                    int calc = prepCount * prepPrice;
                    txtPrice.Text = Convert.ToString(calc);

                    connection.Close();
                }
            }
        }
        private void combPrep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string query = "SELECT P_Info FROM Preparation where P_Title = '" + combPrep.SelectedItem + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                txtInfo.Text = Convert.ToString(command.ExecuteScalar());
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Sur.Text == "" || Name.Text == "" || FIO.Text == "" || phone.Text == "" || txtAdres.Text == "" || txtPrice.Text == "" || txtCount.Text == "" || txtInfo.Text == "" || combEmp.SelectedIndex == -1 || combPrep.SelectedIndex == -1 )
            {
                MessageBox.Show("Вы не заполнили все данные");
            }
            else
            {
                SqlConnection saveZak = new SqlConnection(connectionString);
                try
                {
                    saveZak.Open();
                    DateTime dateNow = DateTime.Now;

                    string saveQuery1 = "SELECT PreparationID FROM Preparation WHERE P_Title ='" + combPrep.SelectedItem + "'";
                    SqlCommand save1 = new SqlCommand(saveQuery1, saveZak);
                    string savePrep = Convert.ToString(save1.ExecuteScalar());

                    string saveQuery2 = "SELECT EmployeeID FROM Employee WHERE E_Surname ='" + combEmp.SelectedItem + "'";
                    SqlCommand save2 = new SqlCommand(saveQuery2, saveZak);
                    string saveEmp = Convert.ToString(save2.ExecuteScalar());

                    string saveQuery3 = "Insert into Client (C_SURNAME,C_NAME, C_Patronymic, C_Address, C_Phone) values ('" + Sur.Text + "','" + Name.Text + "','" + FIO.Text + "','" + txtAdres.Text + "','" + phone.Text + "')";
                    SqlCommand save3 = new SqlCommand(saveQuery3, saveZak); save3.ExecuteNonQuery();

                    string saveQuery31 = "select Top 1 ClientID from Client order by ClientID desc";
                    SqlCommand save31 = new SqlCommand(saveQuery31, saveZak);
                    int klientID = Convert.ToInt32(save31.ExecuteScalar());

                    string costQuery = "SELECT P_Quantity FROM Preparation WHERE P_Title ='" + combPrep.SelectedItem + "'";
                    SqlCommand cost = new SqlCommand(costQuery, saveZak);
                    int costPrep = Convert.ToInt32(cost.ExecuteScalar());
                    int selectCost = Convert.ToInt32(txtCount.Text);

                    int calcCost = costPrep - selectCost;

                    string saveCalc = "Update Preparation set P_Quantity = '" + calcCost + "' where P_Title = '" + combPrep.SelectedItem + "'";
                    SqlCommand sc = new SqlCommand(saveCalc, saveZak); sc.ExecuteNonQuery();

                    if (dateOrder.Text != "")
                    {
                        string savezakaz = "Insert into [Order] (EmployeeID,ClientID,PreparationID,TotalCount,TotalPrice,DateOrder) values ('" + saveEmp + "','" + klientID + "','" + savePrep + "','" + txtCount.Text + "','" + txtPrice.Text + "','" + dateOrder.Text + "')";
                        SqlCommand sz = new SqlCommand(savezakaz, saveZak); sz.ExecuteNonQuery();
                    }
                    else
                    {
                        string savezakaz = "Insert into [Order] (EmployeeID,ClientID,PreparationID,TotalCount,TotalPrice,DateOrder) values ('" + saveEmp + "','" + klientID + "','" + savePrep + "','" + txtCount.Text + "','" + txtPrice.Text + "','" + dateNow + "')";
                        SqlCommand sz = new SqlCommand(savezakaz, saveZak); sz.ExecuteNonQuery();
                    }
                    saveZak.Close();

                    combPrep.SelectedIndex = -1;
                    combEmp.SelectedIndex = -1;
                    Sur.Text = "";
                    Name.Text = "";
                    FIO.Text = "";
                    phone.Text = "";
                    txtAdres.Text = "";
                    txtPrice.Text = "";
                    txtCount.Text = "";
                    txtInfo.Text = "";
                    dateOrder.Text = "";

                    MessageBox.Show("Данные занесены в БД");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
                finally
                {
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            PharmacistWindow pw = new PharmacistWindow();
            pw.Show();
            this.Close();
        }
    }
}
