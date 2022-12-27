using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
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
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Renault.CarsPages
{
    public partial class EnginesPage : Page
    {
        Database database = new Database();
        DataTable dt = new DataTable();

        public EnginesPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            database.openConnection();

            string sql = $"SELECT ID_engine AS ID, EngineSize, EngineType, HorsePower FROM Engines";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dt);

            listget.ItemsSource = dt.DefaultView;



            database.closeConnection();
        }

        private void listget_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listget.Items.Count != 0)
            {
                DataRowView drv = (DataRowView)listget.SelectedItem;

                TextId.Text = drv["ID"].ToString();
                TextSize.Text = drv["EngineSize"].ToString();
                TextType.Text = drv["EngineType"].ToString();
                TextHP.Text = drv["HorsePower"].ToString();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var Size = TextSize.Text;
            var Type = TextType.Text;
            var HP = TextHP.Text;

            database.openConnection();

            string sql = $"INSERT INTO Engines (EngineSize, EngineType, HorsePower) Values ('{Size}', '{Type}', '{HP}')";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            sqlCommand.ExecuteNonQuery();

            database.closeConnection();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var ID = TextId.Text;

            database.openConnection();

            string sql = $"DELETE FROM Engines WHERE ID_engine = '{ID}'";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            sqlCommand.ExecuteNonQuery();

            database.closeConnection();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var ID = TextId.Text;
            var Size = TextSize.Text;
            var Type = TextType.Text;
            var HP = TextHP.Text;

            database.openConnection();

            string sql = $"UPDATE Engines SET EngineSize = '{Size}', EngineType = '{Type}', HorsePower = '{HP}' WHERE ID_engine = '{ID}'";
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

            string sql = $"SELECT ID_engine AS ID, EngineSize, EngineType, HorsePower FROM Engines";
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

            string sql = $"SELECT ID_engine AS ID, EngineSize, EngineType, HorsePower FROM Engines WHERE ID_engine LIKE '%" + SearchText.Text + "%' OR EngineSize LIKE '%" + SearchText.Text + "%' OR EngineType LIKE '%" + SearchText.Text + "%' OR HorsePower LIKE '%" + SearchText.Text + "%'";
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
