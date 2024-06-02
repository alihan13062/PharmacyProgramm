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
    /// Логика взаимодействия для EditEmployee.xaml
    /// </summary>
    public partial class EditEmployee : Window
    {
        string connectionString = SqkConnectionString.GetConnectionSqlServer();
        public EditEmployee()
        {
            InitializeComponent();
            EmpJob();
            EmpId();
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
        public void EmpJob()
        {
            combEmp.Items.Clear();
            string query = "SELECT J_Title FROM JobTitle";
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
        public void EmpId()
        {
            combID.Items.Clear();
            string query = "SELECT EmployeeId FROM Employee";

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
        private void combID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int EmpId = Convert.ToInt32(combID.SelectedItem);

            string query = "select E_SURNAME from Employee where EmployeeID = '" + EmpId + "'";
            string query1 = "select E_NAME from Employee where EmployeeID = '" + EmpId + "'";
            string query2 = "select E_Patronymic from Employee where EmployeeID = '" + EmpId + "'";
            string query3 = "select E_PHONE from Employee where EmployeeID = '" + EmpId + "'";
            string query4 = "select Logins from Employee where EmployeeID = '" + EmpId + "'";
            string query5 = "select Passwords from Employee where EmployeeID = '" + EmpId + "'";
            string query6 = "select J_Title from Employee inner join JobTitle on Employee.JobTitleID = JobTitle.JobTitleID where EmployeeID = '" + EmpId + "'";

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
                SqlCommand Sqlcmd4 = new SqlCommand(query4, connection);
                txtLogin.Text = Convert.ToString(Sqlcmd4.ExecuteScalar());
                SqlCommand Sqlcmd5 = new SqlCommand(query5, connection);
                txtPassword.Text = Convert.ToString(Sqlcmd5.ExecuteScalar());

                SqlCommand Sqlcmd6 = new SqlCommand(query6, connection);
                combEmp.SelectedItem = Convert.ToString(Sqlcmd6.ExecuteScalar());
                connection.Close();
            }
        }
        private void btnEditEmp_Click(object sender, RoutedEventArgs e)
        {
            combID.Visibility = Visibility.Visible;
            txtID.Visibility = Visibility.Visible;
            btnDelete.Visibility = Visibility.Visible;
            btnSaveEdit.Visibility = Visibility.Visible;
            btnNoEditEmp.Visibility = Visibility.Visible;
            btnEditEmp.Visibility = Visibility.Hidden;
            btnAddNew.Visibility = Visibility.Hidden;
            btnEditEmp.Visibility = Visibility.Hidden;
            EmpJob();
            EmpId();
        }
        private void btnNoEditEmp_Click(object sender, RoutedEventArgs e)
        {
            combID.Visibility = Visibility.Hidden;
            txtID.Visibility = Visibility.Hidden;
            btnDelete.Visibility = Visibility.Hidden;
            btnSaveEdit.Visibility = Visibility.Hidden;
            btnNoEditEmp.Visibility = Visibility.Hidden;
            btnEditEmp.Visibility = Visibility.Visible;
            btnAddNew.Visibility = Visibility.Visible;
            btnEditEmp.Visibility = Visibility.Visible;
            EmpJob();
            EmpId();
        }
        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            if (Sur.Text == "" || Name.Text == "" || FIO.Text == "" || phone.Text == "" || txtLogin.Text == "" || txtPassword.Text == "" || combEmp.SelectedIndex == -1)
            {
                MessageBox.Show("Вы не заполнили все данные");
            }
            else
            {
                SqlConnection saveEmp = new SqlConnection(connectionString);
                try
                {
                    saveEmp.Open();

                    string saveQuery1 = "SELECT JobTitleID FROM JobTitle WHERE J_Title ='" + combEmp.SelectedItem + "'";
                    SqlCommand save1 = new SqlCommand(saveQuery1, saveEmp);
                    string saveJobID = Convert.ToString(save1.ExecuteScalar());

                    string saveEmployee = "Insert into Employee (E_NAME,E_SURNAME,E_Patronymic,E_PHONE,JobTitleID,Logins,Passwords) values ('" + Name.Text + 
                        "','" + Sur.Text + "','" + FIO.Text + "','" + phone.Text + "','" + saveJobID + "','" + txtLogin.Text + "','" + txtPassword.Text + "')";
                    SqlCommand se = new SqlCommand(saveEmployee, saveEmp); se.ExecuteNonQuery();

                    saveEmp.Close();

                    combEmp.SelectedIndex = -1;
                    combID.SelectedIndex = -1;
                    Sur.Text = "";
                    Name.Text = "";
                    FIO.Text = "";
                    phone.Text = "";
                    txtLogin.Text = "";
                    txtPassword.Text = "";
                    EmpId();
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
        private void btnSaveEdit_Click(object sender, RoutedEventArgs e)
        {
            if (combID.SelectedIndex == -1)
            {
                MessageBox.Show("Вы не выбрали код сотрудника!");
            }
            else
            {
                if (Sur.Text == "" || Name.Text == "" || FIO.Text == "" || phone.Text == "" || txtLogin.Text == "" || txtPassword.Text == "" || combEmp.SelectedIndex == -1)
                {
                    MessageBox.Show("Вы не заполнили все данные");
                }
                else
                {
                    SqlConnection saveEmp = new SqlConnection(connectionString);

                    try
                    {
                        saveEmp.Open();

                        string saveQuery1 = "SELECT JobTitleID FROM JobTitle WHERE J_Title ='" + combEmp.SelectedItem + "'";
                        SqlCommand save1 = new SqlCommand(saveQuery1, saveEmp);
                        string saveJobID = Convert.ToString(save1.ExecuteScalar());

                        string updateEmp = "Update Employee set E_NAME = '" + Name.Text + "', E_SURNAME = '" + Sur.Text + "', E_Patronymic = '" + FIO.Text +
                            "', E_PHONE = '" + phone.Text + "', JobTitleID = '" + saveJobID + "', Logins = '" + txtLogin.Text + "', Passwords = '" +
                            txtPassword.Text + "' where EmployeeID =" + combID.SelectedItem;
                        SqlCommand ue = new SqlCommand(updateEmp, saveEmp); ue.ExecuteNonQuery();

                        saveEmp.Close();

                        combEmp.SelectedIndex = -1;
                        combID.SelectedIndex = -1;
                        Sur.Text = "";
                        Name.Text = "";
                        FIO.Text = "";
                        phone.Text = "";
                        txtLogin.Text = "";
                        txtPassword.Text = "";
                        EmpId();

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
                MessageBox.Show("Вы не выбрали код сотрудника!");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены что хотите удалить данного сотрудника?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    string query = "DELETE FROM Employee WHERE EmployeeID = @EmployeeId";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeId", Convert.ToString(combID.SelectedItem));

                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            MessageBox.Show("Сотрудник удален");
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("SQL Error: " + ex.Message);
                        }
                    }
                    combEmp.SelectedIndex = -1;
                    combID.SelectedIndex = -1;
                    Sur.Text = "";
                    Name.Text = "";
                    FIO.Text = "";
                    phone.Text = "";
                    txtLogin.Text = "";
                    txtPassword.Text = "";
                    EmpId();
                }
                else if (result == MessageBoxResult.No)
                {
                }
            }
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow pw = new AdminWindow();
            pw.Show();
            this.Close();
        }
    }
}
