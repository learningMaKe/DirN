

using DirN.Utils.Tooltips;
using DirN.ViewModels.Node;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Media;

namespace DirN.Utils.NgManager.Curves
{
    public interface ICurve:INodeData,ITooltipable
    {
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public Point ControlPoint1 { get;  }
        public Point ControlPoint2 { get; }

        public IConnector? Starter { get; set; }
        public IConnector? Ender { get; set; }

        public INode? StartNode { get; }
        public INode? EndNode { get; }

        public double Thickness { get; set; } 
        public Brush Brush { get; set; } 

        public void UpdateLink();

        public void CutLink(IConnector connector);

        public bool CanExist { get; }

        public void Remove();

        public void MakeSureLinkFlow();
    }
}
