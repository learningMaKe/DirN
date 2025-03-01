using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DirN.Utils.Extension
{
    public static class RectExtension
    {
        public static Rect ScaleTransform(this Rect rect, Point centralPoint,double scale)
        {
            Point leftTop = new(rect.Left - centralPoint.X, rect.Top - centralPoint.Y);
            leftTop = new Point(centralPoint.X + leftTop.X * scale, centralPoint.Y + leftTop.Y * scale);
            return new Rect(leftTop, new Size(rect.Width * scale, rect.Height * scale));
        }
    }
}
