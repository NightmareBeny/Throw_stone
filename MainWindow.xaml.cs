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
using LiveCharts.Defaults;

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

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            try
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
                Error.Visibility = Visibility.Hidden;
            }
            catch(Exception)
            {
                Error.Visibility = Visibility.Visible;
                textbox1.Text = null;
                textbox1.Focus();
            }
        }

        public SeriesCollection SeriesCollection { get; set; } = new SeriesCollection();

        private void TableDataSend_Click(object sender, RoutedEventArgs e)
        {
            Tables.IsEnabled = false;
            Grafic.IsEnabled = true;
            Grafic.IsSelected = true;
            foreach (var data in Table)
            {
                SeriesCollection.Add(new LineSeries
                {
                    Title = $"Опыт {data.Number}",
                    Values = data.Grafik(),
                    DataLabels = true,
                    LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                    Fill = Brushes.Transparent,
                });
            }
            DataContext = this;
        }

        private void Repeat_Click(object sender, RoutedEventArgs e)
        {
            Grafic.IsEnabled = false;
            Data.IsEnabled = true;
            Data.IsSelected = true;
            textbox1.Text = null;
            DataGrid.ItemsSource = null;
            SeriesCollection.Clear();
            Table.Clear();
        }

        private void Data_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Send_Click(sender, e);
        }

        private void Grafic_GotFocus(object sender, RoutedEventArgs e)
        {
            stackPanel.Children.Clear();
            foreach (var data in Table)
            {
                StackPanel panel = new StackPanel();
                TextBlock textBlock1 = new TextBlock();
                textBlock1.Text = $"Скорость броска: {data.Speed} м/сек";
                panel.Children.Add(textBlock1);
                TextBlock textBlock2 = new TextBlock();
                textBlock2.Text = $"Угол броска: {data.Angle} градусов";
                panel.Children.Add(textBlock2);
                TextBlock textBlock3 = new TextBlock();
                textBlock3.Text = $"Максимальная высота: {data.hMAX()} м";
                panel.Children.Add(textBlock3);
                TextBlock textBlock4 = new TextBlock();
                textBlock4.Text = $"Максимальная дальность броска: {data.LMAX()} м";
                panel.Children.Add(textBlock4);
                TextBlock textBlock5 = new TextBlock();
                textBlock5.Text = $"Максимальное время полёта: {data.Time()} сек";
                panel.Children.Add(textBlock5);
                panel.UpdateLayout();
                GroupBox groupBox = new GroupBox();
                groupBox.Header = $"Опыт {data.Number}";
                groupBox.Content = panel;
                stackPanel.Children.Add(groupBox);
            }
        }
    }
}
