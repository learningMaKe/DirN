using DirN.Utils.Events.EventType;
using DirN.Utils.Extension;
using DirN.Utils.Nodes;
using DirN.Utils.Nodes.Converters;
using DirN.Utils.Tooltips;
using DirN.ViewModels.Node;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DirN.Utils.NgManager.Curves
{
    [JsonConverter(typeof(CurveConverter))]
    public abstract class CurveBase : BindableBase, ICurve
    {
        private object? data;

        [OnChangedMethod(nameof(OnPointChanged))]
        public Point StartPoint { get; set; }

        [OnChangedMethod(nameof(OnPointChanged))]
        public Point EndPoint { get; set; }

        public Point RaisePosition { get;private set; }

        public Point ControlPoint1 { get;protected set; }

        public Point ControlPoint2 { get;protected set; }

        public double Thickness { get; set; } = 2;

        public Brush Brush { get; set; } = Brushes.Red;

        [OnChangedMethod(nameof(OnStartPointOwnerSet))]
        public IConnector? Starter { get; set; }

        [OnChangedMethod(nameof(OnEndPointOwnerSet))]
        public IConnector? Ender { get; set; }

        public INode? StartNode => Starter?.PointerParent.NodeParent;
        public INode? EndNode => Ender?.PointerParent.NodeParent;

        public bool CanExist => Starter is not null || Ender is not null;

        public object? Data
        {
            get
            {
                if(HandlerManager.ConvertData(Starter!.PointerParent.PointerType, Ender!.PointerParent.PointerType, data,out object? result))
                {
                    return result;
                }
                Debug.WriteLine($"数据转换失败 StartPointOwner:{Starter.PointerParent.PointerType} EndPointOwner:{Ender.PointerParent.PointerType}");
                return null ;
            }
            set
            {
                data = value;
                SetProperty(ref data, value);
            }
        }

        public void MakeSureLinkFlow()
        {
            if (Starter is null || Ender is null) return;

            if (!(Starter.IsInput ^ Ender.IsInput)) return;

            if (Starter.IsInput)
            {
                (Starter, Ender) = (Ender, Starter);
            }
            ReBrush();
        }

        public void Remove()
        {
            Starter?.RemoveLink(this);
            Ender?.RemoveLink(this);
            TooltipManager.Instance.RemoveTooltip(this);
            NodeGraphicsManager.Instance.RemoveCurve(this);
        }

        private void OnPointChanged()
        {
            Recalculate();
            RaisePosition = Point.Add(StartPoint.Avg(EndPoint), new Vector(0, 3));
        }

        protected abstract void Recalculate();

        public void UpdateLink()
        {
            if(Starter is not null)
            {
                StartPoint = RelativeTo(Starter.Connector);
            }
            if(Ender is not null)
            {
                EndPoint = RelativeTo(Ender.Connector);
            }
        }

        /// <summary>
        /// 切断与指定连接器的连接
        /// 如果没有起点和终点，则删除
        /// </summary>
        public void CutLink(IConnector connector)
        {
            if(Starter == connector)
            {
                Starter.RemoveLink(this);
                Starter = null;
            }
            else if(Ender == connector)
            {
                Ender.RemoveLink(this);
                Ender = null;
            }
        }

        private void ReBrush()
        {
            if (Starter is null || Ender is null) return;
            Brush = new LinearGradientBrush(Starter.ConnectorColor, Ender.ConnectorColor, StartPoint, EndPoint)
            {
                MappingMode = BrushMappingMode.Absolute,
            };
        }

        private static Point RelativeTo(FrameworkElement owner)
        {
            Point point = new(owner.ActualWidth / 2, owner.ActualHeight / 2);
           return  NodeGraphicsManager.Instance.GetCanvasRelativePoint(owner,point);
        }

        private void OnStartPointOwnerSet()
        {
            if(OnOwnerSet(Starter,out Point startPoint))
            {
                StartPoint = startPoint;
            }
            CheckLinkConvertable();
        }

        private void OnEndPointOwnerSet()
        {
            if(OnOwnerSet(Ender,out Point endPoint))
            {
                EndPoint = endPoint;
            }
            CheckLinkConvertable();
        }

        private bool OnOwnerSet(IConnector? connector,out Point point)
        {
            if (connector is null) return false;
            point = RelativeTo(connector.Connector);
            connector.AddLink(this);
            ReBrush();
            return true;
        }

        private void CheckLinkConvertable()
        {
            if(Starter is null || Ender is null)
            {
                TooltipManager.Instance.RemoveTooltip(this);
                return;
            }
            Type startType = Starter.PointerParent.PointerType;
            Type endType = Ender.PointerParent.PointerType;
            if (!HandlerManager.CanConvertData(startType, endType))
            {
                TooltipManager.Instance.Tooltip(this, $"{startType.Name}->{endType.Name} 不能转换", TooltipType.Error);
            }
        }

        public override string ToString()
        {
            return $"{StartPoint.X:F2},{StartPoint.Y:F2} -> {EndPoint.X:F2},{EndPoint.Y:F2}";
        }

    }
}
