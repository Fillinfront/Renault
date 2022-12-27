using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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

namespace Renault
{
    public partial class RegisterPanel : Window
    {
        Database database = new Database();

        public RegisterPanel()
        {
            InitializeComponent();
        }

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

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
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

            string sqlQuery = $"SELECT ID_user, UserLogin, UserPassword FROM Users WHERE UserLogin = '{userLogin}'";

            SqlCommand sqlCommand = new SqlCommand(sqlQuery, database.getConnection());

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(datatable);

            if (datatable.Rows.Count > 0)
            {
                RegisterWarning.Visibility = Visibility.Visible;
            }
            else
            {
                if (userLogin.Length == 0 || userPassword.Length == 0) 
                {
                    LoginWarning.Visibility = Visibility.Visible;
                }
                else if (userPassword.Length <= 4)
                {
                    SmallWarning.Visibility = Visibility.Visible;
                }
                else
                {
                    database.openConnection();

                    sqlQuery = $"INSERT INTO Users (FK_role, UserLogin, UserPassword) Values (4, '{userLogin}', '{userPassword}')";

                    sqlCommand = new SqlCommand(sqlQuery, database.getConnection());

                    adapter.SelectCommand = sqlCommand;
                    adapter.Fill(datatable);

                    MessageBox.Show("Аккаунт успешно создан!");

                    database.closeConnection();
                }
            }
        }

        private void Registration_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LoginPanel  loginPanel= new LoginPanel();
            this.Close();
            loginPanel.Show();
        }

        private void WarningOff()
        {
            if (RegisterWarning.Visibility == Visibility.Visible || SmallWarning.Visibility == Visibility.Visible || LoginWarning.Visibility == Visibility.Visible)
            {
                RegisterWarning.Visibility = Visibility.Hidden;
                SmallWarning.Visibility = Visibility.Hidden;
                LoginWarning.Visibility = Visibility.Hidden;
            }
        }
    }
}
