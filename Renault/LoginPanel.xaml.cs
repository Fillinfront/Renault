using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
using Renault;

namespace Renault
{
    public partial class LoginPanel : Window
    {
        public LoginPanel()
        {
            InitializeComponent();
        }

        Database database = new Database();

        private void MinusBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void PasswordBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && PasswordInput.Visibility != Visibility.Hidden)
            {
                PasswordInput.Visibility = Visibility.Hidden;
                VisiblePasswordInput.Text = PasswordInput.Password;
                VisiblePasswordInput.Visibility = Visibility.Visible;     
            }
            else
            {
                VisiblePasswordInput.Visibility = Visibility.Hidden;
                PasswordInput.Password = VisiblePasswordInput.Text;
                PasswordInput.Visibility = Visibility.Visible;
            }
        }

        private void PasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            WarningOff();

            if (PasswordInput.Password.Length == 0)
            {
                PasswordMask.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordMask.Visibility = Visibility.Hidden;
            }
        }

        private void VisiblePasswordInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            WarningOff();

            if (VisiblePasswordInput.Text.Length == 0)
            {
                PasswordMask.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordMask.Visibility = Visibility.Hidden;
            }
        }

        private void LoginInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            WarningOff();

            if (LoginInput.Text.Length == 0)
            {
                LoginMask.Visibility = Visibility.Visible;
            }
            else
            {
                LoginMask.Visibility = Visibility.Hidden;
            }
        }

        private void Registration_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RegisterPanel registerPanel = new RegisterPanel();
            this.Close();
            registerPanel.Show();
        }

        public void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            database.openConnection();

            var userLogin = LoginInput.Text;
            var userPassword = PasswordInput.Password;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable datatable = new DataTable();

            if (PasswordInput.Visibility == Visibility.Visible)
            {
                userPassword = PasswordInput.Password;
            }
            else 
            {
                userPassword = VisiblePasswordInput.Text;
            }

            string sqlQuery = $"SELECT ID_user, UserLogin, UserPassword FROM Users WHERE UserLogin = '{userLogin}' and UserPassword = '{userPassword}'";

            SqlCommand sqlCommand = new SqlCommand(sqlQuery, database.getConnection());

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(datatable);

            if (datatable.Rows.Count == 1)
            {
                string sqlGetRole = $"SELECT FK_role FROM Users WHERE UserLogin = '{userLogin}'";

                SqlCommand comm = new SqlCommand(sqlGetRole, database.getConnection());


                Database.Login = userLogin;
                Database.Role = (int)comm.ExecuteScalar();

                MessageBox.Show("Вы успешно вошли!");

                MainPanel panel = new MainPanel();
                this.Close();
                panel.Show();

            }
            else
            {
                LoginWarning.Visibility = Visibility.Visible;
            }

            database.closeConnection();
        }

        private void WarningOff()
        {
            if (LoginWarning.Visibility == Visibility.Visible)
            {
                LoginWarning.Visibility = Visibility.Hidden;
            }
        }
    }


}
