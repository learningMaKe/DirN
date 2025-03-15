using DirN.Utils.NgManager.Curves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace DirN.Utils.Events.EventType
{
    public class NodeGraphicsArgs
    {
        public class LinkArgs : EventArgs
        {
            public ICurve? Curve { get; set; }

            public HitTestFilterCallback FilterCallback { get; set; } = new(visual => HitTestFilterBehavior.Stop);
            public HitTestResultCallback ResultCallback { get; set; } = new(visual => HitTestResultBehavior.Stop);
        }

        public class GetCanvasRelativePointArgs : EventArgs
        {
            public FrameworkElement? Element { get; set; }
            public Point ElementRelativePoint { get; set; }
            public Point CanvasRelativePoint { get; set; }
        }

        public class GetCentralPointArgs : EventArgs
        {
            public Point CentralPoint { get; set; }
        }

        public class NodeExecutionArgs : EventArgs
        {

        }

        public class MousePositionArgs : EventArgs
        {
            public Point MousePosition { get; set; }
        }
    }
}
