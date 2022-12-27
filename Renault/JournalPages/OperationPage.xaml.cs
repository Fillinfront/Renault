using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace Renault.JournalPages
{
    public partial class OperationPage : Page
    {
        Database database = new Database();
        DataTable dt = new DataTable();

        public OperationPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            database.openConnection();

            string sql = $"SELECT ID_operation as ID, OperationName FROM Operation";
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
                TextId.Text = drv["ID"].ToString();
                TextOper.Text = drv["OperationName"].ToString();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var Oper = TextOper.Text;

            database.openConnection();

            string sql = $"INSERT INTO Operation Values ('{Oper}')";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            sqlCommand.ExecuteNonQuery();

            database.closeConnection();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ID = TextId.Text;

            database.openConnection();

            string sql = $"DELETE FROM Operation WHERE ID_operation = '{ID}'";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            sqlCommand.ExecuteNonQuery();

            database.closeConnection();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var ID = TextId.Text;
            var Oper = TextOper.Text;

            database.openConnection();

            string sql = $"UPDATE Operation SET OperationName = '{Oper}' WHERE ID_operation = '{ID}'";
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

            string sql = $"SELECT ID_operation as ID, OperationName FROM Operation";
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

            string sql = $"SELECT ID_operation as ID, OperationName FROM Operation WHERE ID_operation LIKE '%" + SearchText.Text + "%' OR OperationName LIKE '%" + SearchText.Text + "%'";
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
