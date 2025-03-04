﻿using DirN.Utils.Events.EventType;
using DirN.Utils.Extension;
using DirN.Utils.Nodes;
using DirN.Utils.Tooltips;
using DirN.ViewModels.Node;
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
    public abstract class CurveBase : BindableBase, ICurve
    {
        private object? data;

        private Type? StarterType=>StartPointOwner?.PointerParent.PointerType;

        private Type? EnderType => EndPointOwner?.PointerParent.PointerType;

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
        public IConnector? StartPointOwner { get; set; }

        [OnChangedMethod(nameof(OnEndPointOwnerSet))]
        public IConnector? EndPointOwner { get; set; }

        public bool CanExist => StartPointOwner is not null || EndPointOwner is not null;

        public object? Data
        {
            get
            {
                if(HandlerManager.ConvertData(StartPointOwner!.PointerParent.PointerType, EndPointOwner!.PointerParent.PointerType, data,out object? result))
                {
                    return result;
                }
                Debug.WriteLine($"数据转换失败 StartPointOwner:{StartPointOwner.PointerParent.PointerType} EndPointOwner:{EndPointOwner.PointerParent.PointerType}");
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

        public void Remove()
        {
            StartPointOwner?.RemoveLink(this);
            EndPointOwner?.RemoveLink(this);
            TooltipManager.Instance.RemoveTooltip(this);
            NodeGraphicsManager.Instance.BezierCurves.Remove(this);
        }

        private void OnPointChanged()
        {
            Recalculate();
            RaisePosition = Point.Add(StartPoint.Avg(EndPoint), new Vector(0, 3));
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

        private static Point RelativeTo(FrameworkElement owner)
        {
            Point point = new(owner.ActualWidth / 2, owner.ActualHeight / 2);
           return  NodeGraphicsManager.Instance.GetCanvasRelativePoint(owner,point);
        }

        private void OnStartPointOwnerSet()
        {
            if(OnOwnerSet(StartPointOwner,out Point startPoint))
            {
                StartPoint = startPoint;
            }
            CheckLinkConvertable();
        }

        private void OnEndPointOwnerSet()
        {
            if(OnOwnerSet(EndPointOwner,out Point endPoint))
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
            return true;
        }

        private void CheckLinkConvertable()
        {
            if(StartPointOwner is null || EndPointOwner is null)
            {
                TooltipManager.Instance.RemoveTooltip(this);
                return;
            }
            Type startType = StartPointOwner.PointerParent.PointerType;
            Type endType = EndPointOwner.PointerParent.PointerType;
            if (!HandlerManager.CanConvertData(startType, endType))
            {
                TooltipManager.Instance.Tooltip(this, $"{startType.Name}->{endType.Name} 不能转换", TooltipType.Error);
            }
        }


    }
}
