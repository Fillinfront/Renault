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
using static System.Net.Mime.MediaTypeNames;

namespace Renault.JournalPages
{
    public partial class JournalPage : Page
    {
        Database database = new Database();
        DataTable dt = new DataTable();
        DataSet dsInst = new DataSet();
        DataSet dsOper = new DataSet();
        DataSet dsSotr = new DataSet();
        DataSet dsClient = new DataSet();

        public JournalPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            database.openConnection();

            string sql = $"SELECT ID_request AS ID, FK_operation, OperationName, FK_instance, CarName = CarName + ' ' + ColorName + ' ' + EngineSize + ' ' + HorsePower + ' ' + ComplName, FK_sotrudnik, FIO, FK_client, ClientFIO FROM [Request] inner join Operation ON [Request].[FK_operation] = [Operation].[ID_operation] inner join Sotrudniki ON [Request].[FK_sotrudnik] = [Sotrudniki].[ID_sotrudnik] inner join Clients ON [Request].[FK_client] = [Clients].[ID_client] inner join CarInstance ON [Request].[FK_instance] = [CarInstance].[ID_instance] inner join Cars ON [CarInstance].[FK_VIN] = [Cars].[VIN] inner join Engines ON [CarInstance].[FK_engine] = [Engines].[ID_engine] inner join Colors ON [CarInstance].[FK_color] = [Colors].[ID_color] inner join CarComplectation ON [CarInstance].[FK_compl] = [CarComplectation].[ID_compl]";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dt);


            string sqlCars = $"SELECT ID_instance, CarName = CarName + ' ' + ColorName + ' ' + EngineSize + ' ' + HorsePower + ' ' + ComplName FROM CarInstance inner join Cars ON [CarInstance].[FK_VIN] = [Cars].[VIN] inner join Colors ON [CarInstance].[FK_color] = [Colors].[ID_color] inner join Engines ON [CarInstance].[FK_engine] = [Engines].[ID_engine] inner join CarComplectation ON [CarInstance].[FK_compl] = [CarComplectation].[ID_compl]";
            sqlCommand = new SqlCommand(sqlCars, database.getConnection());

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dsInst);

            ComboInstance.ItemsSource = dsInst.Tables[0].DefaultView;
            ComboInstance.SelectedValuePath = "ID_instance";
            ComboInstance.DisplayMemberPath = "CarName";

            string sqlColors = $"SELECT ID_operation, OperationName FROM Operation";
            sqlCommand = new SqlCommand(sqlColors, database.getConnection());

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dsOper);

            ComboOper.ItemsSource = dsOper.Tables[0].DefaultView;
            ComboOper.SelectedValuePath = "ID_operation";
            ComboOper.DisplayMemberPath = "OperationName";

            string sqlEngines = $"SELECT ID_sotrudnik, FIO FROM Sotrudniki";
            sqlCommand = new SqlCommand(sqlEngines, database.getConnection());

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dsSotr);

            ComboSotr.ItemsSource = dsSotr.Tables[0].DefaultView;
            ComboSotr.SelectedValuePath = "ID_sotrudnik";
            ComboSotr.DisplayMemberPath = "FIO";

            string sqlCompl = $"SELECT ID_client, ClientFIO FROM Clients";
            sqlCommand = new SqlCommand(sqlCompl, database.getConnection());

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dsClient);

            ComboClient.ItemsSource = dsClient.Tables[0].DefaultView;
            ComboClient.SelectedValuePath = "ID_client";
            ComboClient.DisplayMemberPath = "ClientFIO";

            listget.ItemsSource = dt.DefaultView;



            database.closeConnection();
        }

        private void listget_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listget.Items.Count != 0)
            {
                DataRowView drv = (DataRowView)listget.SelectedItem;
                TextId.Text = drv["ID"].ToString();
                ComboOper.Text = drv["OperationName"].ToString();
                ComboInstance.Text = drv["CarName"].ToString();
                ComboSotr.Text = drv["FIO"].ToString();
                ComboClient.Text = drv["ClientFIO"].ToString();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var Inst = ComboInstance.SelectedValue;
            var Oper = ComboOper.SelectedValue;
            var Sotr = ComboSotr.SelectedValue;
            var Client = ComboClient.SelectedValue;

            if (Inst != null && Oper != null && Sotr != null && Client != null)
            {
                database.openConnection();

                string sql = $"INSERT INTO Request (FK_operation, FK_instance, FK_sotrudnik, FK_client) Values ('{Oper}', '{Inst}', '{Sotr}', '{Client}')";
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

            string sql = $"DELETE FROM Request WHERE ID_request = '{ID}'";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            sqlCommand.ExecuteNonQuery();

            database.closeConnection();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var ID = TextId.Text;
            var Inst = ComboInstance.SelectedValue;
            var Oper = ComboOper.SelectedValue;
            var Sotr = ComboSotr.SelectedValue;
            var Client = ComboClient.SelectedValue;

            database.openConnection();

            string sql = $"UPDATE Request SET FK_operation = '{Oper}', FK_instance = '{Inst}', FK_sotrudnik = '{Sotr}', FK_client = '{Client}' WHERE ID_request = '{ID}'";
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

            string sql = $"SELECT ID_request AS ID, FK_operation, OperationName, FK_instance, CarName = CarName + ' ' + ColorName + ' ' + EngineSize + ' ' + HorsePower + ' ' + ComplName, FK_sotrudnik, FIO, FK_client, ClientFIO FROM [Request] inner join Operation ON [Request].[FK_operation] = [Operation].[ID_operation] inner join Sotrudniki ON [Request].[FK_sotrudnik] = [Sotrudniki].[ID_sotrudnik] inner join Clients ON [Request].[FK_client] = [Clients].[ID_client] inner join CarInstance ON [Request].[FK_instance] = [CarInstance].[ID_instance] inner join Cars ON [CarInstance].[FK_VIN] = [Cars].[VIN] inner join Engines ON [CarInstance].[FK_engine] = [Engines].[ID_engine] inner join Colors ON [CarInstance].[FK_color] = [Colors].[ID_color] inner join CarComplectation ON [CarInstance].[FK_compl] = [CarComplectation].[ID_compl]";
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

            string sql = $"SELECT ID_request AS ID, FK_operation, OperationName, FK_instance, CarName = CarName + ' ' + ColorName + ' ' + EngineSize + ' ' + HorsePower + ' ' + ComplName, FK_sotrudnik, FIO, FK_client, ClientFIO FROM [Request] inner join Operation ON [Request].[FK_operation] = [Operation].[ID_operation] inner join Sotrudniki ON [Request].[FK_sotrudnik] = [Sotrudniki].[ID_sotrudnik] inner join Clients ON [Request].[FK_client] = [Clients].[ID_client] inner join CarInstance ON [Request].[FK_instance] = [CarInstance].[ID_instance] inner join Cars ON [CarInstance].[FK_VIN] = [Cars].[VIN] inner join Engines ON [CarInstance].[FK_engine] = [Engines].[ID_engine] inner join Colors ON [CarInstance].[FK_color] = [Colors].[ID_color] inner join CarComplectation ON [CarInstance].[FK_compl] = [CarComplectation].[ID_compl] WHERE ID_request LIKE '%" + SearchText.Text + "%' OR OperationName LIKE '%" + SearchText.Text + "%' OR FK_instance LIKE '%" + SearchText.Text + "%'OR CarName LIKE '%" + SearchText.Text + "%' OR EngineSize LIKE '%" + SearchText.Text + "%' OR FIO LIKE '%" + SearchText.Text + "%' OR ClientFIO LIKE '%" + SearchText.Text + "%'";

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
