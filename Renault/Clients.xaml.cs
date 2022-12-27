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
    public partial class Clients : Window
    {
        Database database = new Database();
        DataTable dt = new DataTable();

        public Clients()
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

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            MainPanel mainPanel = new MainPanel();
            this.Close();
            mainPanel.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            database.openConnection();

            string sql = $"SELECT ID_client, ClientFIO, ClientBirthday, ClientTelephone FROM Clients";
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
                TextId.Text = drv["ID_client"].ToString();
                TextFIO.Text = drv["ClientFIO"].ToString();
                TextBirthday.Text = drv["ClientBirthday"].ToString();
                TextTelephone.Text = drv["ClientTelephone"].ToString();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var FIO = TextFIO.Text;
            var Birthday = TextBirthday.Text;
            var Telephone = TextTelephone.Text;

            database.openConnection();

            string sql = $"INSERT INTO Clients (ClientFIO, ClientBirthday, ClientTelephone) Values ('{FIO}', '{Birthday}', '{Telephone}')";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            sqlCommand.ExecuteNonQuery();

            database.closeConnection();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var ID = TextId.Text;

            database.openConnection();

            string sql = $"DELETE FROM Clients WHERE ID_client = '{ID}'";
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

            string sql = $"UPDATE Clients SET ClientFIO = '{FIO}', ClientBirthday = '{Birthday}', ClientTelephone = '{Telephone}' WHERE ID_client = '{ID}'";
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

            string sql = $"SELECT ID_client, ClientFIO, ClientBirthday, ClientTelephone FROM Clients";
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

            string sql = $"SELECT ID_client, ClientFIO, ClientBirthday, ClientTelephone FROM Clients WHERE ID_client LIKE '%" + SearchText.Text + "%' OR ClientFIO LIKE '%" + SearchText.Text + "%' OR ClientBirthday LIKE '%" + SearchText.Text + "%' OR ClientTelephone LIKE '%" + SearchText.Text + "%'";
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
