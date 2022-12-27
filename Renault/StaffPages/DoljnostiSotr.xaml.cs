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
using static System.Net.Mime.MediaTypeNames;

namespace Renault.StaffPages
{
    
    public partial class DoljnostiSotr : Page
    {
        Database database = new Database();
        DataTable dt = new DataTable();
        DataSet dsDoljnost = new DataSet();
        DataSet dsFIO = new DataSet();

        public DoljnostiSotr()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            database.openConnection();

            string sql = $"SELECT ID_doljnsotrudnik AS ID, FK_sotrudnik, FIO, FK_doljnost, DoljnostName FROM [DoljnostOfSotrudniki] inner join Doljnost ON [DoljnostOfSotrudniki].[FK_doljnost] = [Doljnost].ID_doljnost inner join Sotrudniki ON [DoljnostOfSotrudniki].[FK_sotrudnik] = [Sotrudniki].ID_sotrudnik";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dt);


            string sql2 = $"SELECT ID_doljnost, DoljnostName FROM Doljnost";
            sqlCommand = new SqlCommand(sql2, database.getConnection());

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dsDoljnost);

            ComboDoljnost.ItemsSource = dsDoljnost.Tables[0].DefaultView;
            ComboDoljnost.SelectedValuePath = "ID_doljnost";
            ComboDoljnost.DisplayMemberPath = "DoljnostName";

            string sql3 = $"SELECT ID_sotrudnik, FIO FROM Sotrudniki";
            sqlCommand = new SqlCommand(sql3, database.getConnection());

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dsFIO);

            ComboFIO.ItemsSource = dsFIO.Tables[0].DefaultView;
            ComboFIO.SelectedValuePath = "ID_sotrudnik";
            ComboFIO.DisplayMemberPath = "FIO";

            listget.ItemsSource = dt.DefaultView;



            database.closeConnection();
        }

        private void listget_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listget.Items.Count != 0)
            {
                DataRowView drv = (DataRowView)listget.SelectedItem;
                TextId.Text = drv["ID"].ToString();
                ComboDoljnost.Text = drv["DoljnostName"].ToString();
                ComboFIO.Text = drv["FIO"].ToString();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var FIO = ComboFIO.SelectedValue;
            var Doljnost = ComboDoljnost.SelectedValue;

            if (FIO != null && Doljnost != null)
            {
                database.openConnection();

                string sql = $"INSERT INTO DoljnostOfSotrudniki Values ('{FIO}', '{Doljnost}')";
                SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

                sqlCommand.ExecuteNonQuery();

                database.closeConnection();
            } else
            {
                MessageBox.Show("Заполните все поля!", "Ошибка");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var ID = TextId.Text;

            database.openConnection();

            string sql = $"DELETE FROM DoljnostOfSotrudniki WHERE ID_doljnsotrudnik = '{ID}'";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            sqlCommand.ExecuteNonQuery();

            database.closeConnection();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var ID = TextId.Text;
            var FIO = ComboFIO.SelectedValue;
            var Doljnost = ComboDoljnost.SelectedValue;

            database.openConnection();

            string sql = $"UPDATE DoljnostOfSotrudniki SET FK_sotrudnik = '{FIO}', FK_doljnost = '{Doljnost}' WHERE ID_doljnsotrudnik = '{ID}'";
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

            string sql = $"SELECT ID_doljnsotrudnik AS ID, FK_sotrudnik, FIO, FK_doljnost, DoljnostName FROM [DoljnostOfSotrudniki] inner join Doljnost ON [DoljnostOfSotrudniki].[FK_doljnost] = [Doljnost].ID_doljnost inner join Sotrudniki ON [DoljnostOfSotrudniki].[FK_sotrudnik] = [Sotrudniki].ID_sotrudnik";
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

            string sql = $"SELECT ID_doljnsotrudnik AS ID, FK_sotrudnik, FIO, FK_doljnost, DoljnostName FROM [DoljnostOfSotrudniki] inner join Doljnost ON [DoljnostOfSotrudniki].[FK_doljnost] = [Doljnost].ID_doljnost inner join Sotrudniki ON [DoljnostOfSotrudniki].[FK_sotrudnik] = [Sotrudniki].ID_sotrudnik WHERE ID_doljnsotrudnik LIKE '%" + SearchText.Text + "%' OR FIO LIKE '%" + SearchText.Text + "%' OR DoljnostName LIKE '%" + SearchText.Text + "%'";
     
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
