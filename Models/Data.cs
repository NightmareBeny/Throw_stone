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

        public List<double> OY()
        {
            List<double> y = new List<double>();
            for (double i = 0; i <= Time(); i++)
            {
                double oy = Speed * i * Sin(Angle * PI / 180) - 9.81 * Pow(i, 2) / 2;
                if (oy >= 0) y.Add(Round(oy,2));
                else { y.Add(0); break; }
            }
            if (y.Last()!=0) y.Add(0);
            return y;
        }

        public List<double> OX()
        {
            List<double> x = new List<double>();
            for (double i = 0; i <= Time(); i++)
            {
                x.Add(Speed * i * Cos(Angle * PI / 180));
            }
            if (x.Last() != Pow(Speed, 2) * Sin(2 * (Angle * PI / 180)) / 9.81)
                x.Add(Round(Pow(Speed, 2) * Sin(2 * (Angle * PI / 180)) / 9.81,2));
            return x;
        }
    }
}
