using Renault.StaffPages;
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

    public partial class Staff : Window
    {
        MainPanel mainPanel = new MainPanel();

        public Staff()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new StafPage();
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
            MainFrame.Content = new StafPage();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new DoljnostiSotr();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Doljnosti();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainPanel.Show();
        }
    }
}
