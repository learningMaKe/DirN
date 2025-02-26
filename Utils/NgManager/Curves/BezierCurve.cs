using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DirN.Utils.NgManager.Curves
{
    public class BezierCurve:CurveBase
    {
        protected override void Recalculate()
        {
            Point middle = new((StartPoint.X + EndPoint.X) / 2, (StartPoint.Y + EndPoint.Y) / 2);
            ControlPoint1 = new Point(middle.X, StartPoint.Y);
            ControlPoint2 = new Point(middle.X, EndPoint.Y);
            if(Brush is LinearGradientBrush linear)
            {
                linear.StartPoint = StartPoint;
                linear.EndPoint = EndPoint;
            }
        }
    }
}
