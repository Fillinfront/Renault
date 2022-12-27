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
using System.IO;
using Microsoft.Win32;

namespace Renault.CarsPages
{
    public partial class CarPage : Page
    {
        Database database = new Database();
        DataTable dt = new DataTable();
        DataSet dsVIN = new DataSet();
        DataSet dsColor = new DataSet();
        DataSet dsEngine = new DataSet();
        DataSet dsCompl = new DataSet();
        OpenFileDialog openFile = new OpenFileDialog();
        public static byte[] bytes;

        public CarPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            database.openConnection();

            string sql = $"SELECT ID_instance AS ID, FK_VIN, CarImage, Speed, CarName, FK_color, ColorName, FK_engine, EngineSize = EngineSize + ' ' + HorsePower, FK_compl, ComplName FROM [CarInstance] inner join Cars ON [CarInstance].[FK_VIN] = [Cars].[VIN] inner join Colors ON [CarInstance].[FK_color] = [Colors].[ID_color] inner join Engines ON [CarInstance].[FK_engine] = [Engines].[ID_engine] inner join CarComplectation ON [CarInstance].[FK_compl] = [CarComplectation].[ID_compl]";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dt);


            string sqlCars = $"SELECT VIN, CarName FROM Cars";
            sqlCommand = new SqlCommand(sqlCars, database.getConnection());

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dsVIN);

            ComboVIN.ItemsSource = dsVIN.Tables[0].DefaultView;
            ComboVIN.SelectedValuePath = "VIN";
            ComboVIN.DisplayMemberPath = "CarName";

            string sqlColors = $"SELECT ID_color, ColorName FROM Colors";
            sqlCommand = new SqlCommand(sqlColors, database.getConnection());

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dsColor);

            ComboColor.ItemsSource = dsColor.Tables[0].DefaultView;
            ComboColor.SelectedValuePath = "ID_color";
            ComboColor.DisplayMemberPath = "ColorName";

            string sqlEngines = $"SELECT ID_engine, EngineSize = EngineSize + ' ' + HorsePower FROM Engines";
            sqlCommand = new SqlCommand(sqlEngines, database.getConnection());

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dsEngine);

            ComboEngine.ItemsSource = dsEngine.Tables[0].DefaultView;
            ComboEngine.SelectedValuePath = "ID_engine";
            ComboEngine.DisplayMemberPath = "EngineSize";

            string sqlCompl = $"SELECT ID_compl, ComplName FROM CarComplectation";
            sqlCommand = new SqlCommand(sqlCompl, database.getConnection());

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dsCompl);

            ComboCompl.ItemsSource = dsCompl.Tables[0].DefaultView;
            ComboCompl.SelectedValuePath = "ID_compl";
            ComboCompl.DisplayMemberPath = "ComplName";

            listget.ItemsSource = dt.DefaultView;



            database.closeConnection();
        }

        private void listget_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listget.Items.Count != 0)
            {
                DataRowView drv = (DataRowView)listget.SelectedItem;
                CarImage.Source = null;


                if (drv["CarImage"].ToString() != "")
                {
                    byte[] img = (byte[])drv["CarImage"];
                    BitmapImage biimg = new BitmapImage();
                    MemoryStream ms = new MemoryStream(img);
                    biimg.BeginInit();
                    biimg.StreamSource = ms;
                    biimg.EndInit();
                    CarImage.Source = biimg;
                }

                TextId.Text = drv["ID"].ToString();
                ComboVIN.Text = drv["CarName"].ToString();
                ComboColor.Text = drv["ColorName"].ToString();
                ComboEngine.Text = drv["EngineSize"].ToString();
                TextSpeed.Text = drv["Speed"].ToString();
                ComboCompl.Text = drv["ComplName"].ToString();
            }
        }

        private void ImageAdd_MouseDown(object sender, MouseButtonEventArgs e)
        {
            openFile.Filter = "Image files|*.png";
            openFile.FilterIndex = 1;
            if (openFile.ShowDialog() == true)
            {
                bytes = File.ReadAllBytes(openFile.FileName);
                bytes.ToArray();
                CarImage.Source = new BitmapImage(new Uri(openFile.FileName));
            }
        }

        public byte[] getPNGFromImageControl(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            byte[] ImageCar = null;
            if (CarImage.Source != null)
            {
                ImageCar = getPNGFromImageControl(CarImage.Source as BitmapImage);
            }
            else
            {
                MessageBox.Show("Выберите фото", "Ошибка");
            }

            var VIN = ComboVIN.SelectedValue;
            var Color = ComboColor.SelectedValue;
            var Engine = ComboEngine.SelectedValue;
            var Speed = TextSpeed.Text;
            var Compl = ComboCompl.SelectedValue;

            if (VIN != null && Color != null && Engine != null && Compl != null && Speed != null && CarImage != null)
            {
                database.openConnection();

                string sql = $"INSERT INTO CarInstance (CarImage, Speed, FK_VIN, FK_color, FK_engine, FK_compl) Values (@binaryValue, '{Speed}', '{VIN}', '{Color}', '{Engine}', '{Compl}')";
                SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

                sqlCommand.Parameters.Add("@binaryValue", SqlDbType.VarBinary, ImageCar.Length).Value = ImageCar;
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

            string sql = $"DELETE FROM CarInstance WHERE ID_instance = '{ID}'";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            sqlCommand.ExecuteNonQuery();

            database.closeConnection();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var ID = TextId.Text;
            var VIN = ComboVIN.SelectedValue;
            var Color = ComboColor.SelectedValue;
            var Engine = ComboEngine.SelectedValue;
            var Speed = TextSpeed.Text;
            var Compl = ComboCompl.SelectedValue;
            var Image = getPNGFromImageControl(CarImage.Source as BitmapImage);

            database.openConnection();

            string sql = $"UPDATE CarInstance SET CarImage = @binaryValue, Speed = '{Speed}', FK_VIN = '{VIN}', FK_color = '{Color}', FK_engine = '{Engine}', FK_compl = '{Compl}' WHERE ID_instance = '{ID}'";
            SqlCommand sqlCommand = new SqlCommand(sql, database.getConnection());

            sqlCommand.Parameters.Add("@binaryValue", SqlDbType.VarBinary, Image.Length).Value = Image;
            sqlCommand.ExecuteNonQuery();

            database.closeConnection();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            database.openConnection();

            dt.Clear();
            listget.ItemsSource = null;
            listget.Items.Clear();

            string sql = $"SELECT ID_instance AS ID, FK_VIN, CarName, CarImage, Speed, FK_color, ColorName, FK_engine, EngineSize = EngineSize + ' ' + HorsePower, FK_compl, ComplName FROM [CarInstance] inner join Cars ON [CarInstance].[FK_VIN] = [Cars].[VIN] inner join Colors ON [CarInstance].[FK_color] = [Colors].[ID_color] inner join Engines ON [CarInstance].[FK_engine] = [Engines].[ID_engine] inner join CarComplectation ON [CarInstance].[FK_compl] = [CarComplectation].[ID_compl]";
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

            string sql = $"SELECT ID_instance AS ID, FK_VIN, CarName, FK_color, CarImage, Speed, ColorName, FK_engine, EngineSize = EngineSize + ' ' + HorsePower, FK_compl, ComplName FROM [CarInstance] inner join Cars ON [CarInstance].[FK_VIN] = [Cars].[VIN] inner join Colors ON [CarInstance].[FK_color] = [Colors].[ID_color] inner join Engines ON [CarInstance].[FK_engine] = [Engines].[ID_engine] inner join CarComplectation ON [CarInstance].[FK_compl] = [CarComplectation].[ID_compl] WHERE ID_instance LIKE '%" + SearchText.Text + "%' OR CarName LIKE '%" + SearchText.Text + "%' OR ColorName LIKE '%" + SearchText.Text + "%' OR EngineSize LIKE '%" + SearchText.Text + "%' OR ComplName LIKE '%" + SearchText.Text + "%' OR Speed LIKE '%" + SearchText.Text + "%'";

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
