using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DirN.Utils.Extension
{
    public static class PointExtension
    {
        public static Point ScaleTransform(this Point point,Point central, double scale)
        {
            double x = (point.X - central.X) * scale + central.X;
            double y = (point.Y - central.Y) * scale + central.Y;
            return new Point(x, y);
        }
    }
}
