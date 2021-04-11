using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace Проект.Models
{
    class Data
    {
        public int Number { get; set; }
        public double Angle { get; set; }
        public double Speed { get; set; }

        public double hMAX()
        {
            return Round(Pow(Speed, 2) * Pow(Sin(Angle * PI / 180), 2) / (2 * 9.81), 2);
        }

        public double LMAX()
        {
            return Round(Pow(Speed, 2) * Sin(2*(Angle* PI / 180)) / 9.81, 2);
        }

        public double Time()
        {
            return Round(2 * Speed * Sin(Angle*PI/180) / 9.81, 2);
        }

        public ChartValues<ObservablePoint> Grafik()
        {
            ChartValues<ObservablePoint> list = new ChartValues<ObservablePoint>();
            double y;
            double x;
            for (double i = 0; i <= Time(); i++)
            {
                y = Round(Speed * i * Sin(Angle * PI / 180) - 9.81 * Pow(i, 2) / 2, 1);
                x = Round(Speed * i * Cos(Angle * PI / 180), 1);
                list.Add(new ObservablePoint(x, y));
            }
            if (list.Last().Y != 0)
            {
                x = Round(Speed * Time() * Cos(Angle * PI / 180), 1);
                list.Add(new ObservablePoint(x, 0));
            }
            return list;
        }
    }
}
