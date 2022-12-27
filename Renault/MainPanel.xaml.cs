using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace Renault
{

    public partial class MainPanel : Window
    {

        public MainPanel()
        {
            InitializeComponent();
        }

        static Database database = new Database();

        static DataTable dt = new DataTable();

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Database.Role == 1)
            {
                StaffBtn.Visibility = Visibility.Visible;
                ClientsBtn.Visibility = Visibility.Visible;
                AllCarsBtn.Visibility = Visibility.Visible;
                JournalBtn.Visibility = Visibility.Visible;
                AccountsBtn.Visibility = Visibility.Visible;
            }
            else if (Database.Role == 2)
            {
                StaffBtn.Visibility = Visibility.Visible;
                ClientsBtn.Visibility = Visibility.Visible;
                AllCarsBtn.Visibility = Visibility.Visible;
                JournalBtn.Visibility = Visibility.Visible;
            }
            else if (Database.Role == 3)
            {
                StaffBtn.Visibility = Visibility.Visible;
                ClientsBtn.Visibility = Visibility.Visible;
                JournalBtn.Visibility = Visibility.Visible;
            }
            else
            {
                LowRole.Visibility = Visibility.Visible;
            }

            dt.Clear();
            listget.ItemsSource = null;
            listget.Items.Clear();

            database.openConnection();

            string sql = $"SELECT ID_instance, FK_engine, FK_color, FK_compl, FK_VIN, EngineSize, HorsePower, EngineType, CarImage, ComplName, ColorName, CarName, BodyType, GearBox, Speed, TrunkCapacity FROM [CarInstance] inner join Engines ON [CarInstance].[FK_engine] = [Engines].[ID_engine] inner join Colors ON [CarInstance].[FK_color] = [Colors].[ID_color] inner join CarComplectation ON [CarInstance].[FK_compl] = [CarComplectation].[ID_compl] inner join Cars ON [CarInstance].[FK_VIN] = [Cars].[VIN]";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dt);

            listget.ItemsSource = dt.DefaultView;

            TextUserName.Text = Database.Login + "!";


            database.closeConnection();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
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

            string sql = $"SELECT ID_instance, FK_engine, FK_color, FK_compl, FK_VIN, EngineSize, EngineType, HorsePower, CarImage, ComplName, ColorName, CarName, BodyType, GearBox, Speed, TrunkCapacity FROM [CarInstance] inner join Engines ON [CarInstance].[FK_engine] = [Engines].[ID_engine] inner join Colors ON [CarInstance].[FK_color] = [Colors].[ID_color] inner join CarComplectation ON [CarInstance].[FK_compl] = [CarComplectation].[ID_compl] inner join Cars ON [CarInstance].[FK_VIN] = [Cars].[VIN] WHERE FK_VIN LIKE '%" + SearchText.Text + "%' OR CarName LIKE '%" + SearchText.Text + "%' OR BodyType LIKE '%" + SearchText.Text + "%' OR GearBox LIKE '%" + SearchText.Text + "%' OR HorsePower LIKE '%" + SearchText.Text + "%' OR Speed LIKE '%" + SearchText.Text + "%' OR TrunkCapacity LIKE '%" + SearchText.Text + "%' OR ColorName LIKE '%" + SearchText.Text + "%' OR EngineSize LIKE '%" + SearchText.Text + "%' OR EngineType LIKE '%" + SearchText.Text + "%'";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            SqlDataAdapter adapter = new SqlDataAdapter();

            dt.Clear();
            listget.ItemsSource = null;
            listget.Items.Clear();

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dt);
           
            listget.ItemsSource = dt.DefaultView;

            database.closeConnection();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LoginPanel LoginPanel = new LoginPanel();
            this.Close();
            LoginPanel.Show();
            
        }

        private void StaffBtn_Click(object sender, RoutedEventArgs e)
        {
            Staff staff = new Staff();
            this.Close();
            staff.Show();
        }

        private void ClientsBtn_Click(object sender, RoutedEventArgs e)
        {
            Clients clients = new Clients();
            this.Close();
            clients.Show();
        }

        private void AccountsBtn_Click(object sender, RoutedEventArgs e)
        {
            Accounts accounts = new Accounts();
            this.Close();
            accounts.Show();
        }

        private void AllCarsBtn_Click(object sender, RoutedEventArgs e)
        {
            Cars cars = new Cars();
            this.Close();
            cars.Show();
        }

        private void JournalBtn_Click(object sender, RoutedEventArgs e)
        {
            Journal journal = new Journal();
            this.Close();
            journal.Show();
        }
    }
}
