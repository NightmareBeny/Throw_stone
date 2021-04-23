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
using System.Windows.Media.Animation;

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
            Animation.IsEnabled = false;
            textbox1.Focus();
        }

        private List<Data> Table = new List<Data>();

        double dt;

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            bool true1 = true, true2 = true;
            try
            {
                Table.Clear();
                if (Convert.ToInt32(textbox1.Text) < 0)
                    throw new Exception();
                else
                {
                    for (int i = 0; i < Convert.ToInt32(textbox1.Text); i++)
                    {
                        Data data = new Data();
                        data.Number = i + 1;
                        Table.Add(data);
                    }
                    DataGrid.ItemsSource = Table;
                    Error.Visibility = Visibility.Hidden;
                    true1 = true;
                }
            }
            catch(Exception)
            {
                Error.Visibility = Visibility.Visible;
                textbox1.Text = null;
                textbox1.Focus();
                true1 = false;
            }
            /////////////
            try
            {
                dt = Convert.ToDouble(textbox2.Text);
                if (dt < 0) throw new Exception();
                else
                {
                    ErrorDt.Visibility = Visibility.Hidden;
                    true2 = true;
                }
            }
            catch(Exception)
            {
                ErrorDt.Visibility = Visibility.Visible;
                textbox2.Text = null;
                textbox2.Focus();
                true2 = false;
            }
            /////////////
            if (true1&&true2)
            {
                Tables.IsEnabled = true;
                Tables.IsSelected = true;
                Data.IsEnabled = false;
            }
        }

        public SeriesCollection seriesCollection { get; set; } = new SeriesCollection();

        private void TableDataSend_Click(object sender, RoutedEventArgs e)
        {
            bool mistake = false;
            foreach (var data in Table)
            {
                if (data.Angle < 0 || data.Speed < 0)
                {
                    MessageBox.Show($"Исправьте данные в {data.Number} строке!\nЧисла должны быть больше 0!"); mistake = true; break;
                }
                else
                {
                    if (dt < 1)
                    {
                        seriesCollection.Add(new LineSeries
                        {
                            Title = $"Опыт {data.Number}",
                            Values = data.Grafik(dt),
                            DataLabels = false,
                            LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                            Fill = Brushes.Transparent,
                        });
                    }
                    else
                    {
                        seriesCollection.Add(new LineSeries
                        {
                            Title = $"Опыт {data.Number}",
                            Values = data.Grafik(dt),
                            DataLabels = true,
                            LineSmoothness = 1, //0: straight lines, 1: really smooth lines
                            Fill = Brushes.Transparent,
                        });
                    }
                }
            }
            if(!mistake)
            {
                DataContext = this;
                Tables.IsEnabled = false;
                Grafic.IsEnabled = true;
                Grafic.IsSelected = true;
            }
        }

        private void Repeat_Click(object sender, RoutedEventArgs e)
        {
            Grafic.IsEnabled = false;
            Animation.IsEnabled = false;
            Data.IsEnabled = true;
            Data.IsSelected = true;
            textbox1.Text = null;
            textbox2.Text = null;
            DataGrid.ItemsSource = null;
            seriesCollection.Clear();
            GC.Collect();
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
                ScrollViewer scroll = new ScrollViewer();
                scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                scroll.Content = groupBox;
                stackPanel.Children.Add(scroll);
            }
        }

        private void Farther_Click(object sender, RoutedEventArgs e)
        {
            Animation.IsEnabled = true;
            Animation.IsSelected = true;
            MyCanvas.Children.Clear();
            //foreach(var data in Table)
            for(int i=0;i<Table.Count;i++)
            {
                //Задаём Path(путь, по которому будет двигаться наш камень)
                var path = new Path();
                var pathGeom = new PathGeometry();
                var pathFigure = new PathFigure();
                var arc = new ArcSegment();
                arc.Point = new Point(Table[i].LMAX()*3, 0);
                arc.Size = new Size(Table[i].LMAX()*3, Table[i].hMAX()*12);
                pathFigure.Segments.Add(arc);
                pathGeom.Figures.Add(pathFigure);
                path.Data = pathGeom;
                Canvas.SetLeft(path, 0);
                Canvas.SetBottom(path, 0);
                //Создаём камень
                var ellipse = new Ellipse();
                ellipse.Width = 15;
                ellipse.Height = 15;
                //ellipse.Fill = seriesCollection.
                ellipse.Stroke = Brushes.Black;
                Canvas.SetLeft(ellipse, 0);
                Canvas.SetBottom(ellipse, 0);
                //Анимируем его
                var storyBoard = new Storyboard();
                var animationX = new DoubleAnimationUsingPath
                {
                    PathGeometry = pathGeom,
                    Source = PathAnimationSource.X,
                    Duration = TimeSpan.FromSeconds(Table[i].Time())
                };
                Storyboard.SetTarget(animationX, ellipse);
                Storyboard.SetTargetProperty(animationX, new PropertyPath("(Canvas.Left)"));
                storyBoard.Children.Add(animationX);
                var animationY = new DoubleAnimationUsingPath
                {
                    PathGeometry = pathGeom,
                    Source = PathAnimationSource.Y,
                    Duration = TimeSpan.FromSeconds(Table[i].Time())
                };
                Storyboard.SetTarget(animationY, ellipse);
                Storyboard.SetTargetProperty(animationY, new PropertyPath("(Canvas.Bottom)"));
                storyBoard.Children.Add(animationY);
                MyCanvas.Children.Add(path);
                MyCanvas.Children.Add(ellipse);
                
                storyBoard.Begin();
            }
        }
    }
}
