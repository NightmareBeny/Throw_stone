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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Проект.Models;
using LiveCharts;
using LiveCharts.Wpf;

namespace Проект
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Tables.IsEnabled = false;
            Grafic.IsEnabled = false;
            textbox1.Focus();
        }

        private List<Data> Table = new List<Data>();

        private void send_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Convert.ToInt32(textbox1.Text); i++)
            {
                Data data = new Data();
                data.Number = i + 1;
                Table.Add(data);
            }
            Tables.IsEnabled = true;
            Tables.IsSelected = true;
            Data.IsEnabled = false;
            DataGrid.ItemsSource = Table;
        }

        public ChartValues<double> Values1 { get; set; }
        public ChartValues<double> Values2 { get; set; }

        private void TableDataSend_Click(object sender, RoutedEventArgs e)
        {
            Tables.IsEnabled = false;
            Grafic.IsEnabled = true;
            Grafic.IsSelected = true;
            Values1 = new ChartValues<double> { 3, 4, 6, 3, 2, 6 };
            Values2 = new ChartValues<double> { 5, 3, 5, 7, 3, 9 };
            DataContext = this;
        }

        private void Repeat_Click(object sender, RoutedEventArgs e)
        {
            Grafic.IsEnabled = false;
            Data.IsEnabled = true;
            Data.IsSelected = true;
            textbox1.Text = null;
            DataGrid.ItemsSource = null;
            Table.Clear();
        }
    }
}
