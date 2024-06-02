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
    /// Логика взаимодействия для EditPreparation.xaml
    /// </summary>
    public partial class EditPreparation : Window
    {
        string connectionString = SqkConnectionString.GetConnectionSqlServer();
        public EditPreparation()
        {
            InitializeComponent();
            PrepId();
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
        public void PrepId()
        {
            combID.Items.Clear();
            string query = "SELECT PreparationId FROM Preparation";

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
            int PrepId = Convert.ToInt32(combID.SelectedItem);

            string query = "select P_Title from Preparation where PreparationID = '" + PrepId + "'";
            string query1 = "select P_Info from Preparation where PreparationID = '" + PrepId + "'";
            string query2 = "select P_Price from Preparation where PreparationID = '" + PrepId + "'";
            string query3 = "select P_Quantity from Preparation where PreparationID = '" + PrepId + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                SqlCommand Sqlcmd = new SqlCommand(query, connection);
                Name.Text = Convert.ToString(Sqlcmd.ExecuteScalar());
                SqlCommand Sqlcmd1 = new SqlCommand(query1, connection);
                Info.Text = Convert.ToString(Sqlcmd1.ExecuteScalar());
                SqlCommand Sqlcmd2 = new SqlCommand(query2, connection);
                Price.Text = Convert.ToString(Sqlcmd2.ExecuteScalar());
                SqlCommand Sqlcmd3 = new SqlCommand(query3, connection);
                Count.Text = Convert.ToString(Sqlcmd3.ExecuteScalar());
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
            PrepId();
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
            PrepId();
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow pw = new AdminWindow();
            pw.Show();
            this.Close();
        }
        private void btnSaveEdit_Click(object sender, RoutedEventArgs e)
        {
            if (combID.SelectedIndex == -1)
            {
                MessageBox.Show("Вы не выбрали код товара!");
            }
            else
            {
                if (Name.Text == "" || Count.Text == "" || Price.Text == "" || Info.Text == "")
                {
                    MessageBox.Show("Вы не заполнили все данные");
                }
                else
                {
                    SqlConnection saveEmp = new SqlConnection(connectionString);

                    try
                    {
                        saveEmp.Open();

                        string updatePrep = "Update Preparation set P_Title = '" + Name.Text + "', P_Info = '" + Info.Text + "', P_Price = '" + Price.Text +
                            "', P_Quantity = '" + Count.Text + "' where PreparationID =" + combID.SelectedItem;
                        SqlCommand up = new SqlCommand(updatePrep, saveEmp); up.ExecuteNonQuery();

                        saveEmp.Close();

                        combID.SelectedIndex = -1;
                        Name.Text = "";
                        Info.Text = "";
                        Price.Text = "";
                        Count.Text = "";
                        PrepId();

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
                MessageBox.Show("Вы не выбрали код товара!");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены что хотите удалить данный товар?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    string query = "DELETE FROM Preparation WHERE PreparationID = @PreparationId";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PreparationId", Convert.ToString(combID.SelectedItem));

                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            MessageBox.Show("Товар удален");
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("SQL Error: " + ex.Message);
                        }
                    }
                    combID.SelectedIndex = -1;
                    Name.Text = "";
                    Info.Text = "";
                    Price.Text = "";
                    Count.Text = "";
                    PrepId();
                }
                else if (result == MessageBoxResult.No)
                {
                }
            }
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "" || Count.Text == "" || Price.Text == "" || Info.Text == "")
            {
                MessageBox.Show("Вы не заполнили все данные");
            }
            else
            {
                SqlConnection saveEmp = new SqlConnection(connectionString);
                try
                {
                    saveEmp.Open();

                    string saveEmployee = "Insert into Preparation (P_Title,P_Info,P_Price,P_Quantity) values ('" + Name.Text +
                        "','" + Info.Text + "','" + Price.Text + "','" + Count.Text + "')";
                    SqlCommand se = new SqlCommand(saveEmployee, saveEmp); se.ExecuteNonQuery();

                    saveEmp.Close();

                    combID.SelectedIndex = -1;
                    Name.Text = "";
                    Info.Text = "";
                    Price.Text = "";
                    Count.Text = "";
                    PrepId();
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
    }
}
