using DirN.Utils.Debugs;
using DirN.Utils.Events.EventType;
using DirN.Utils.KManager;
using DirN.Utils.KManager.HKey;
using DirN.Utils.NgManager;
using DirN.Utils.NgManager.Curves;
using DirN.Utils.Nodes;
using DirN.ViewModels.Node;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DirN.ViewModels
{
    public class NodeCanvasViewModel : BaseViewModel,IViewGetter
    {
        private FrameworkElement? view;

        private Point PreviousPoint;
        private DateTime LastMoveTime;
        
        private Point StartPoint;
        public bool SelectionShow { get; set; }
        public int SelectionZIndex { get; set; }
        public Point LeftTopPoint { get; set; }
        public Size SelectionSize { get; set; }

        public FrameworkElement View
        {
            get
            {
                view ??= GetView?.Invoke()??throw new InvalidOperationException("GetView is null");
                return view;
            }
        }

        public INodeGraphicsManager NodeGraphicsManager { get;set; }

        public DelegateCommand<MouseEventArgs> MouseMoveCommand { get; set; }
        public DelegateCommand<MouseButtonEventArgs> MouseLeftButtonUpCommand { get; set; }
        public DelegateCommand<MouseButtonEventArgs> SelectionStartCommand { get; set; }
        public DelegateCommand<MouseButtonEventArgs> SelectionEndCommand { get; set; }
        public DelegateCommand<MouseEventArgs> SelectionMoveCommand { get; set; }

        public Action<NodeGraphicsArgs.LinkArgs>? MakeLinkEvent { get; set; }
        
        public Func<FrameworkElement>? GetView { get; set; }

        private NodeGraphicsArgs.LinkArgs? FocusedLink;

        public NodeCanvasViewModel(IContainerProvider provider) : base(provider)
        {
            NodeGraphicsManager = provider.Resolve<INodeGraphicsManager>();

            MouseMoveCommand = new(MouseMove);
            MouseLeftButtonUpCommand = new(MouseLeftButtonUp);
            SelectionStartCommand = new(SelectionStart);
            SelectionEndCommand = new(SelectionEnd);
            SelectionMoveCommand = new(SelectionMove);

            KeyManager.Instance.RegisterEvent<MouseEventArgs>(EventId.Mouse_Middle_Pressed, GraphicsMove);

            EventAggregator.GetEvent<NodeGraphicsEvent.MakeLinkEvent>().Subscribe(MakeLink);
            EventAggregator.GetEvent<NodeGraphicsEvent.GetCanvasRelativePointEvent>().Subscribe(GetCanvasRelativePoint);
            EventAggregator.GetEvent<NodeGraphicsEvent.MousePositionEvent>().Subscribe(MousePosition);
            EventAggregator.GetEvent<NodeGraphicsEvent.GetCentralPointEvent>().Subscribe(GetCentralPoint);
        }

        private void SelectionStart(MouseButtonEventArgs e)
        {
            StartPoint = e.GetPosition(View);
            GetRectancle(e);
            SelectionShow = true;
            SelectionZIndex = 1000;

        }

        private void SelectionEnd(MouseButtonEventArgs e)
        {
            SelectionShow = false;
            SelectionZIndex = 0;

            Rect rect = new(LeftTopPoint, SelectionSize);
            NodeGraphicsManager.MultiSelectNodes(rect);

        }

        private void SelectionMove(MouseEventArgs e)
        {
            if (!SelectionShow) return;
            GetRectancle(e);
        }

        private void GetRectancle(MouseEventArgs e)
        {
            var (point1, point2) = GetRectancle(StartPoint, e.GetPosition(View));
            if (point1.X < 0 || point1.Y < 0 || point2.Width < 0 || point2.Height < 0) return;
            (LeftTopPoint, SelectionSize) = (point1, point2);
        }

        private static (Point, Size) GetRectancle(Point point1, Point point2)
        {
            Point leftTop = new(Math.Min(point1.X, point2.X), Math.Min(point1.Y, point2.Y));
            Size size = new(Math.Max(point1.X, point2.X)- leftTop.X, Math.Max(point1.Y, point2.Y)- leftTop.Y);
            return (leftTop, size);
        }

        private void GetCentralPoint(NodeGraphicsArgs.GetCentralPointArgs args)
        {
            args.CentralPoint = new Point(View.ActualWidth / 2, View.ActualHeight / 2);
        }

        private void GraphicsMove(MouseEventArgs args)
        {
            Point currentPoint = Mouse.GetPosition(View);
            if(DateTime.Now - LastMoveTime > TimeSpan.FromMilliseconds(30))
            {
                PreviousPoint = currentPoint;
            }
            Vector vector = currentPoint - PreviousPoint;
            NodeGraphicsManager.MoveNode(vector);
            PreviousPoint = currentPoint;
            LastMoveTime = DateTime.Now;
        }

        private void MousePosition(NodeGraphicsArgs.MousePositionArgs args)
        {
            args.MousePosition = Mouse.GetPosition(View);
        }

        private void GetCanvasRelativePoint(NodeGraphicsArgs.GetCanvasRelativePointArgs args)
        {
            if (args.Element is null) return;
            args.CanvasRelativePoint = args.Element.TranslatePoint(args.ElementRelativePoint, View);
        }

        private void MakeLink(NodeGraphicsArgs.LinkArgs args)
        {
            Point point = Mouse.GetPosition(View);
            args.Curve!.EndPoint = point;
            FocusedLink = args;
            NodeGraphicsManager.BezierCurves.Add(args.Curve);
        }

        private void MouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (FocusedLink is not null)
            {
                Point pt = e.GetPosition(View);
                Debug.WriteLine(pt);
                PointHitTestParameters parameters = new(pt);
                VisualTreeHelper.HitTest(view, FocusedLink.FilterCallback, FocusedLink.ResultCallback, parameters);
                if(FocusedLink.Curve!.EndPointOwner is null)
                {
                    FocusedLink.Curve!.Remove();
                }
                
                FocusedLink = null;
            }
        }

        private void MouseMove(MouseEventArgs e)
        {
            Point point = e.GetPosition(View);
            if (FocusedLink is not null)
            {
                FocusedLink.Curve!.EndPoint = point;
            }
        }

    }
}
