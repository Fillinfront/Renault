using Renault.CarsPages;
using Renault.JournalPages;
using System;
using System.Collections.Generic;
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
    public partial class Cars : Window
    {
        public Cars()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new CarPage();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new AllCarsPage();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new EnginesPage();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ColorsPage();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ComplPage();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new CarPage();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            MainPanel mainPanel = new MainPanel();
            this.Close();
            mainPanel.Show();
        }
    }
}
