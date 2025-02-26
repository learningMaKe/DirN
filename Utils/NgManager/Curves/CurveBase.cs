using DirN.Utils.Events.EventType;
using DirN.ViewModels.Node;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DirN.Utils.NgManager.Curves
{
    public abstract class CurveBase : BindableBase, ICurve
    {
        [OnChangedMethod(nameof(Recalculate))]
        public Point StartPoint { get; set; }

        [OnChangedMethod(nameof(Recalculate))]
        public Point EndPoint { get; set; }

        public Point ControlPoint1 { get;protected set; }

        public Point ControlPoint2 { get;protected set; }

        public double Thickness { get; set; } = 2;
        public Brush Brush { get; set; } = Brushes.Red;

        [OnChangedMethod(nameof(OnStartPointOwnerSet))]
        public IConnector? StartPointOwner { get; set; }

        [OnChangedMethod(nameof(OnEndPointOwnerSet))]
        public IConnector? EndPointOwner { get; set; }

        public bool CanExist => StartPointOwner is not null || EndPointOwner is not null;

        public object? Data { get; set; }

        public void Remove()
        {
            StartPointOwner?.RemoveLink(this);
            EndPointOwner?.RemoveLink(this);
            NodeGraphicsManager.Instance.BezierCurves.Remove(this);
        }

        protected abstract void Recalculate();

        public void UpdateLink()
        {
            if(StartPointOwner is not null)
            {
                StartPoint = RelativeTo(StartPointOwner.Connector);
            }
            if(EndPointOwner is not null)
            {
                EndPoint = RelativeTo(EndPointOwner.Connector);
            }
        }

        /// <summary>
        /// 切断与指定连接器的连接
        /// 如何没有起点和终点，则删除
        /// </summary>
        public void CutLink(IConnector connector)
        {
            if(StartPointOwner == connector)
            {
                StartPointOwner.RemoveLink(this);
                StartPointOwner = null;
            }
            else if(EndPointOwner == connector)
            {
                EndPointOwner.RemoveLink(this);
                EndPointOwner = null;
            }
        }

        private static Point RelativeTo(UIElement owner)
        {
            Point point = new(owner.RenderSize.Width / 2, owner.RenderSize.Height / 2);
           return  NodeGraphicsManager.Instance.GetCanvasRelativePoint(owner,point);
        }

        private void OnStartPointOwnerSet()
        {
            if(OnOwnerSet(StartPointOwner,out Point startPoint))
            {
                StartPoint = startPoint;
            }
        }

        private void OnEndPointOwnerSet()
        {
            if(OnOwnerSet(EndPointOwner,out Point endPoint))
            {
                EndPoint = endPoint;
            }
        }

        private bool OnOwnerSet(IConnector? connector,out Point point)
        {
            if (connector is null) return false;
            point = RelativeTo(connector.Connector);
            connector.AddLink(this);
            return true;
        }

        public void MakeSureLinkFlow()
        {
            if (StartPointOwner is null || EndPointOwner is null) return;

            if (!(StartPointOwner.IsInput ^ EndPointOwner.IsInput)) return;

            if (StartPointOwner.IsInput)
            {
                (StartPointOwner, EndPointOwner) = (EndPointOwner, StartPointOwner);
            }
            Brush = new LinearGradientBrush(StartPointOwner.ConnectorColor, EndPointOwner.ConnectorColor, StartPoint, EndPoint)
            {
                MappingMode = BrushMappingMode.Absolute,
            };
        }

    }
}
