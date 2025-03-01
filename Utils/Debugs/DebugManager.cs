using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DirN.Utils.Debugs
{
    public class DebugManager : SingletonBase<DebugManager>
    {
        public object? DebugObject { get; set; }

        public Canvas? DebugCanvas { get; set; }

        public void Clear()
        {
            if (DebugCanvas == null)
            {
                return;
            }
            DebugCanvas.Children.Clear();
        }

        public void DrawRect(Rect rect,Brush? brush=null)
        {
            brush ??= Brushes.Blue;
            if (DebugCanvas == null)
            {
                return;
            }
            var rectElement = new Rectangle()
            {
                Width = rect.Width,
                Height = rect.Height,
                Fill = brush,
                Stroke = brush,
                StrokeThickness = 1
            };

            Canvas.SetLeft(rectElement, rect.Left);
            Canvas.SetTop(rectElement, rect.Top);

            DebugCanvas.Children.Add(rectElement);
        }
    }
}
