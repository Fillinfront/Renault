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
    public partial class RolesPage : Page
    {
        Database database = new Database();
        DataTable dt = new DataTable();

        public RolesPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            database.openConnection();

            string sql = $"SELECT ID_role, RoleName FROM Roles";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dt);

            listget.ItemsSource = dt.DefaultView;


            database.closeConnection();
        }

        private void listget_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (listget.Items.Count != 0)
            {
                DataRowView drv = (DataRowView)listget.SelectedItem;
                TextId.Text = drv["ID_role"].ToString();
                TextRole.Text = drv["RoleName"].ToString();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var Role = TextRole.Text;

            database.openConnection();

            string sql = $"INSERT INTO Roles Values ('{Role}')";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            sqlCommand.ExecuteNonQuery();

            database.closeConnection();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ID = TextId.Text;

            database.openConnection();

            string sql = $"DELETE FROM Roles WHERE ID_role = '{ID}'";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            sqlCommand.ExecuteNonQuery();

            database.closeConnection();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var ID = TextId.Text;
            var Role = TextRole.Text;

            database.openConnection();

            string sql = $"UPDATE Roles SET RoleName = '{Role}' WHERE ID_role = '{ID}'";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            sqlCommand.ExecuteNonQuery();

            database.closeConnection();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            database.openConnection();

            dt.Clear();
            listget.ItemsSource = null;
            listget.Items.Clear();

            string sql = $"SELECT ID_role, RoleName FROM Roles";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dt);

            listget.ItemsSource = dt.DefaultView;

            database.closeConnection();
        }

        private void SearchText_TextChanged_1(object sender, TextChangedEventArgs e)
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

            string sql = $"SELECT ID_role, RoleName FROM Roles WHERE ID_role LIKE '%" + SearchText.Text + "%' OR RoleName LIKE '%" + SearchText.Text + "%'";
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
