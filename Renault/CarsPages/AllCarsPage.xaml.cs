using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
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
using System.IO;
using Microsoft.Win32;

namespace Renault.CarsPages
{
    public partial class AllCarsPage : Page
    {
        Database database = new Database();
        DataTable dt = new DataTable();
        OpenFileDialog openFile = new OpenFileDialog();
        public static byte[] bytes;

        public AllCarsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            database.openConnection();

            string sql = $"SELECT VIN, CarName, BodyType, GearBox, TrunkCapacity FROM Cars";
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

                TextId.Text = drv["VIN"].ToString();
                TextName.Text = drv["CarName"].ToString();
                TextBody.Text = drv["BodyType"].ToString();
                TextKPP.Text = drv["GearBox"].ToString();
                TextTrunk.Text = drv["TrunkCapacity"].ToString();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var Name = TextName.Text;
            var Body = TextBody.Text;
            var KPP = TextKPP.Text;
            var Trunk = TextTrunk.Text;

            if (Name.Length != 0 && Body.Length != 0 && KPP.Length != 0 && Trunk.Length != 0)
            {
                database.openConnection();

                string sql = $"INSERT INTO Cars (CarName, BodyType, GearBox, TrunkCapacity) Values ('{Name}', '{Body}', '{KPP}', '{Trunk}')";
                SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

                sqlCommand.ExecuteNonQuery();

                database.closeConnection();
            }
            else
            {
                MessageBox.Show("Заполните все поля", "Ошибка");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var ID = TextId.Text;

            database.openConnection();

            string sql = $"DELETE FROM Cars WHERE VIN = '{ID}'";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            sqlCommand.ExecuteNonQuery();

            database.closeConnection();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var ID = TextId.Text;
            var Name = TextName.Text;
            var Body = TextBody.Text;
            var KPP = TextKPP.Text;
            var Trunk = TextTrunk.Text;

            database.openConnection();

            string sql = $"UPDATE Cars SET CarName = '{Name}', BodyType = '{Body}', GearBox = '{KPP}', TrunkCapacity = '{Trunk}' WHERE VIN = '{ID}'";
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

            string sql = $"SELECT VIN, CarName, BodyType, GearBox, TrunkCapacity FROM Cars";
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

            string sql = $"SELECT VIN, CarName, BodyType, GearBox, TrunkCapacity FROM Cars WHERE VIN LIKE '%" + SearchText.Text + "%' OR CarName LIKE '%" + SearchText.Text + "%' OR BodyType LIKE '%" + SearchText.Text + "%' OR GearBox LIKE '%" + SearchText.Text + "%' OR TrunkCapacity LIKE '%" + SearchText.Text + "%'";
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
