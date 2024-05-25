﻿using System;
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

namespace PharmacyProgramm
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connectionString = SqkConnectionString.GetConnectionSqlServer();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT COUNT(1) FROM Employee WHERE Logins = @log AND Passwords = @pas";

            try
            {
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlcmd = new SqlCommand(query, sqlcon))
                    {
                        sqlcmd.CommandType = CommandType.Text;
                        sqlcmd.Parameters.AddWithValue("@log", txtLog.Text);
                        sqlcmd.Parameters.AddWithValue("@pas", password.Password);

                        sqlcon.Open();
                        int count = Convert.ToInt32(sqlcmd.ExecuteScalar());

                        if (count == 1)
                        {
                            string postQuery = "SELECT JobTitleID FROM Employee WHERE Logins = @log";
                            SqlCommand postCmd = new SqlCommand(postQuery, sqlcon);
                            postCmd.Parameters.AddWithValue("@log", txtLog.Text);
                            int jobId = Convert.ToInt32(postCmd.ExecuteScalar());

                            if (jobId == 1)
                            {
                                AdminWindow admin = new AdminWindow();
                                admin.Show();
                            }
                            else if (jobId == 2)
                            {
                                PharmacistWindow pharm = new PharmacistWindow();
                                pharm.Show();
                            }
                            else
                            {
                                MessageBox.Show("Должность не определена.");
                            }

                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Не правильный ввод данных");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
