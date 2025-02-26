

using DirN.ViewModels.Node;
using System.Windows;
using System.Windows.Media;

namespace DirN.Utils.NgManager.Curves
{
    public interface ICurve:INodeData
    {
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public Point ControlPoint1 { get;  }
        public Point ControlPoint2 { get; }

        public IConnector? StartPointOwner { get; set; }
        public IConnector? EndPointOwner { get; set; }

        public double Thickness { get; set; } 
        public Brush Brush { get; set; } 

        public void UpdateLink();

        public void CutLink(IConnector connector);

        public bool CanExist { get; }

        public void Remove();

        public void MakeSureLinkFlow();
    }
}
