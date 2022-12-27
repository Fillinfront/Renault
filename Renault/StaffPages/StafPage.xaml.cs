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
using System.ComponentModel;

namespace Renault.StaffPages
{
    public partial class StafPage : Page
    {
        Database database = new Database();
        DataTable dt = new DataTable();

        public StafPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            database.openConnection();

            string sql = $"SELECT ID_sotrudnik, FIO, SotrudnikBirthday, SotrudnikTelephone FROM Sotrudniki";
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
                TextId.Text = drv["ID_sotrudnik"].ToString();
                TextFIO.Text = drv["FIO"].ToString();
                TextBirthday.Text = drv["SotrudnikBirthday"].ToString();
                TextTelephone.Text = drv["SotrudnikTelephone"].ToString();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var FIO = TextFIO.Text;
            var Birthday = TextBirthday.Text;
            var Telephone = TextTelephone.Text;

            database.openConnection();

            string sql = $"INSERT INTO Sotrudniki (FIO, SotrudnikBirthday, SotrudnikTelephone) Values ('{FIO}', '{Birthday}', '{Telephone}')";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            sqlCommand.ExecuteNonQuery();

            database.closeConnection();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var ID = TextId.Text;

            database.openConnection();

            string sql = $"DELETE FROM Sotrudniki WHERE ID_sotrudnik = '{ID}'";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            sqlCommand.ExecuteNonQuery();

            database.closeConnection();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var ID = TextId.Text;
            var FIO = TextFIO.Text;
            var Birthday = TextBirthday.Text;
            var Telephone = TextTelephone.Text;

            database.openConnection();

            string sql = $"UPDATE Sotrudniki SET FIO = '{FIO}', SotrudnikBirthday = '{Birthday}', SotrudnikTelephone = '{Telephone}' WHERE ID_sotrudnik = '{ID}'";
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

            string sql = $"SELECT ID_sotrudnik, FIO, SotrudnikBirthday, SotrudnikTelephone FROM Sotrudniki";
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

            string sql = $"SELECT ID_sotrudnik, FIO, SotrudnikBirthday, SotrudnikTelephone FROM Sotrudniki WHERE ID_sotrudnik LIKE '%" + SearchText.Text + "%' OR FIO LIKE '%" + SearchText.Text + "%' OR SotrudnikBirthday LIKE '%" + SearchText.Text + "%' OR SotrudnikTelephone LIKE '%" + SearchText.Text + "%'";
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
