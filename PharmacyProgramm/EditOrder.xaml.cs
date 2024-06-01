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
    /// Логика взаимодействия для EditOrder.xaml
    /// </summary>
    public partial class EditOrder : Window
    {
        string connectionString = SqkConnectionString.GetConnectionSqlServer();
        public EditOrder()
        {
            InitializeComponent();
            EmpName();
            PrepName();
            OrderId();
        }
        private void rus(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Я]");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void noEng(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z]");
            e.Handled = !regex.IsMatch(e.Text);
        }
        private void number(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void MyDatePicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow pw = new AdminWindow();
            pw.Show();
            this.Close();
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
        public void OrderId()
        {
            combID.Items.Clear();
            string query = "SELECT OrderId FROM [Order]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int postdName = reader.GetInt32(0);
                        combID.Items.Add(postdName);
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

        private void combID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int orderId = Convert.ToInt32(combID.SelectedItem);

            string query = "select C_SURNAME from [Order] inner join Client on [Order].ClientID = Client.ClientID where OrderID = '" + orderId + "'";
            string query1 = "select C_NAME from [Order] inner join Client on [Order].ClientID = Client.ClientID where OrderID = '" + orderId + "'";
            string query2 = "select C_Patronymic from [Order] inner join Client on [Order].ClientID = Client.ClientID where OrderID = '" + orderId + "'";
            string query3 = "select C_PHONE from [Order] inner join Client on [Order].ClientID = Client.ClientID where OrderID = '" + orderId + "'";
            string query31 = "select C_Address from [Order] inner join Client on [Order].ClientID = Client.ClientID where OrderID = '" + orderId + "'";
            string query4 = "select E_SURNAME from [Order] inner join Employee on [Order].EmployeeID = Employee.EmployeeID where OrderID = '" + orderId + "'";
            string query5 = "select P_Title from [Order] inner join Preparation on [Order].PreparationID = Preparation.PreparationID where OrderID = '" + orderId + "'";
            string query51 = "select TotalCount from [Order] where OrderID = '" + orderId + "'";
            string query52 = "select P_Info from [Order] inner join Preparation on [Order].PreparationID = Preparation.PreparationID where OrderID = '" + orderId + "'";
            string query6 = "select DateOrder from [Order] where OrderID = '" + orderId + "'";
            string query7 = "select TotalPrice from [Order] where OrderID = '" + orderId + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                SqlCommand Sqlcmd = new SqlCommand(query, connection);
                Sur.Text = Convert.ToString(Sqlcmd.ExecuteScalar());
                SqlCommand Sqlcmd1 = new SqlCommand(query1, connection);
                Name.Text = Convert.ToString(Sqlcmd1.ExecuteScalar());
                SqlCommand Sqlcmd2 = new SqlCommand(query2, connection);
                FIO.Text = Convert.ToString(Sqlcmd2.ExecuteScalar());
                SqlCommand Sqlcmd3 = new SqlCommand(query3, connection);
                phone.Text = Convert.ToString(Sqlcmd3.ExecuteScalar());
                SqlCommand Sqlcmd31 = new SqlCommand(query31, connection);
                txtAdres.Text = Convert.ToString(Sqlcmd31.ExecuteScalar());

                SqlCommand Sqlcmd4 = new SqlCommand(query4, connection);
                combEmp.SelectedItem = Convert.ToString(Sqlcmd4.ExecuteScalar());

                SqlCommand Sqlcmd5 = new SqlCommand(query5, connection);
                combPrep.SelectedItem = Convert.ToString(Sqlcmd5.ExecuteScalar());

                SqlCommand Sqlcmd51 = new SqlCommand(query51, connection);
                txtCount.Text = Convert.ToString(Sqlcmd51.ExecuteScalar());
                SqlCommand Sqlcmd52 = new SqlCommand(query52, connection);
                txtInfo.Text = Convert.ToString(Sqlcmd52.ExecuteScalar());

                SqlCommand Sqlcmd6 = new SqlCommand(query6, connection);
                dateOrder.Text = Convert.ToString(Sqlcmd6.ExecuteScalar());
                SqlCommand Sqlcmd7 = new SqlCommand(query7, connection);
                txtPrice.Text = Convert.ToString(Sqlcmd7.ExecuteScalar());

                connection.Close();
            }
        }
        public void CheckOrderId()
        {
            if (combID.SelectedIndex == -1)
            {
                Sur.Text = "";
                Name.Text = "";
                FIO.Text = "";
                phone.Text = "";
                txtAdres.Text = "";
                txtCount.Text = "";
                txtInfo.Text = "";
                txtPrice.Text = "";
                dateOrder.Text = null;
                combEmp.SelectedIndex = -1;
                combPrep.SelectedIndex = -1;
            }
        }
        private void btnSaveEdit_Click(object sender, RoutedEventArgs e)
        {
            if (combID.SelectedIndex == -1)
            {
                MessageBox.Show("Вы не выбрали код заказа!");
            }
            else
            {
                if (Sur.Text == "" || Name.Text == "" || FIO.Text == "" || phone.Text == "" || txtAdres.Text == "" || txtPrice.Text == "" || txtCount.Text == "" || txtInfo.Text == "" || combEmp.SelectedIndex == -1 || combPrep.SelectedIndex == -1 || dateOrder.Text == "")
                {
                    MessageBox.Show("Вы не заполнили все данные");
                }
                else
                {
                    SqlConnection saveZak = new SqlConnection(connectionString);

                    try
                    {
                        saveZak.Open();
                        string saveQuery2 = "SELECT PreparationID FROM Preparation WHERE P_Title ='" + combPrep.SelectedItem + "'";
                        SqlCommand save2 = new SqlCommand(saveQuery2, saveZak);
                        string savePrep = Convert.ToString(save2.ExecuteScalar());

                        string saveQuery3 = "SELECT Client.ClientID FROM [Order] inner join Client on [Order].ClientID = Client.ClientID where OrderID ='" + combID.SelectedItem + "'";
                        SqlCommand save3 = new SqlCommand(saveQuery3, saveZak);
                        string saveKlient = Convert.ToString(save3.ExecuteScalar());

                        string saveQuery4 = "SELECT Employee.EmployeeID FROM Employee where E_Surname = '" + combEmp.SelectedItem + "'";
                        SqlCommand save4 = new SqlCommand(saveQuery4, saveZak);
                        string saveSotrud = Convert.ToString(save4.ExecuteScalar());

                        string savezakaz = "Update [Order] set PreparationID = '" + savePrep + "', ClientID = '" + saveKlient + "', EmployeeID = '" + saveSotrud + "', DateOrder = '" +
                            dateOrder.Text + "', TotalCount = '" + txtCount.Text + "', TotalPrice = '" + txtPrice.Text + "' where OrderID =" + combID.SelectedItem;
                        SqlCommand sz = new SqlCommand(savezakaz, saveZak); sz.ExecuteNonQuery();

                        string saveklient2 = "Update Client set C_SURNAME = '" + Sur.Text + "', C_NAME = '" + Name.Text + "', C_Patronymic = '" + FIO.Text + "', " +
                            "C_Address = '" + txtAdres.Text + "', C_PHONE = '" + phone.Text + "' where ClientID =" + saveKlient;
                        SqlCommand sk = new SqlCommand(saveklient2, saveZak); sk.ExecuteNonQuery();
                        saveZak.Close();

                        combID.SelectedIndex = -1;
                        CheckOrderId();
                        MessageBox.Show("Данные обновлены");
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
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (combID.SelectedIndex == -1)
            {
                MessageBox.Show("Вы не выбрали код заказа!");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены что хотите удалить данный заказ?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    string query = "DELETE FROM [Order] WHERE OrderID = @OrderId";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderId", Convert.ToString(combID.SelectedItem));

                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            CheckOrderId();
                            MessageBox.Show("Заказ удален");
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("SQL Error: " + ex.Message);
                        }
                    }
                    CheckOrderId();
                    OrderId();
                }
                else if (result == MessageBoxResult.No)
                {
                }
            }
        }
    }
}
