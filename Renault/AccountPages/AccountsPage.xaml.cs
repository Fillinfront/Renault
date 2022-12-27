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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Renault.AccountPages
{
    public partial class AccountsPage : Page
    {
        Database database = new Database();
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();

        public AccountsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            database.openConnection();

            string sql = $"SELECT ID_user, FK_role, RoleName, UserLogin, UserPassword FROM [Users] inner join Roles ON [Users].[FK_role] = [Roles].[ID_role]";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dt);

            string sql2 = $"SELECT ID_role, RoleName FROM Roles";
            sqlCommand = new SqlCommand(sql2, database.getConnection());

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(ds);

            ComboRole.ItemsSource = ds.Tables[0].DefaultView;
            ComboRole.SelectedValuePath = "ID_role";
            ComboRole.DisplayMemberPath = "RoleName";

            listget.ItemsSource = dt.DefaultView;



            database.closeConnection();
        }

        private void listget_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listget.Items.Count != 0)
            {
                DataRowView drv = (DataRowView)listget.SelectedItem;
                TextId.Text = drv["ID_user"].ToString();
                ComboRole.Text = drv["RoleName"].ToString();
                TextLogin.Text = drv["UserLogin"].ToString();
                TextPassword.Text = drv["UserPassword"].ToString();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var Role = ComboRole.SelectedValue;
            var Login = TextLogin.Text;
            var Password = TextPassword.Text;

            if (Login.Length != 0 && Password.Length != 0 && Role != null)
            {
                database.openConnection();

                string sql = $"INSERT INTO Users (FK_role, UserLogin, UserPassword) Values ('{Role}', '{Login}', '{Password}')";
                SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

                sqlCommand.ExecuteNonQuery();

                database.closeConnection();
            }
            else
            {
                MessageBox.Show("Заполните все поля!", "Ошибка");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var ID = TextId.Text;

                database.openConnection();

                string sql = $"DELETE FROM Users WHERE ID_user = '{ID}'";
                SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

                sqlCommand.ExecuteNonQuery();

                database.closeConnection();    
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var ID = TextId.Text;
            var Role = ComboRole.SelectedValue;
            var Login = TextLogin.Text;
            var Password = TextPassword.Text;

            database.openConnection();

            string sql = $"UPDATE Users SET FK_role = '{Role}', UserLogin = '{Login}', UserPassword = '{Password}' WHERE ID_user = '{ID}'";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            sqlCommand.ExecuteNonQuery();

            database.closeConnection();
        }

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            database.openConnection();

            dt.Clear();
            listget.ItemsSource = null;
            listget.Items.Clear();

            string sql = $"SELECT ID_user, FK_role, RoleName, UserLogin, UserPassword FROM [Users] inner join Roles ON [Users].[FK_role] = [Roles].[ID_role]";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dt);

            listget.ItemsSource = dt.DefaultView;

            database.closeConnection();
        }

        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchText.Text.Length == 0)
            {
                SearchMask.Visibility = Visibility.Visible;
            }
            else
            {
                SearchMask.Visibility = Visibility.Hidden;
            }

            database.openConnection();

            string sql = $"SELECT ID_user, FK_role, RoleName, UserLogin, UserPassword FROM [Users] inner join Roles ON [Users].[FK_role] = [Roles].[ID_role] WHERE ID_user LIKE '%" + SearchText.Text + "%' OR RoleName LIKE '%" + SearchText.Text + "%' OR UserLogin LIKE '%" + SearchText.Text + "%' OR UserPassword LIKE '%" + SearchText.Text + "%'";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            SqlDataAdapter adapter = new SqlDataAdapter();

            dt.Clear();
            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dt);

            listget.ItemsSource = dt.DefaultView;

            database.closeConnection();
        }
    }
}
