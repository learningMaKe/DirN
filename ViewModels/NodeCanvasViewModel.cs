using DirN.Utils.Events.EventType;
using DirN.Utils.NgManager;
using DirN.Utils.NgManager.Curves;
using DirN.Utils.Nodes;
using DirN.ViewModels.Node;
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
    public class NodeCanvasViewModel : BaseViewModel
    {
        private UIElement? view;

        public UIElement View
        {
            get
            {
                view ??= GetView?.Invoke()??throw new InvalidOperationException("GetView is null");
                return view;
            }
        }

        public INodeGraphicsManager NodeGraphicsManager { get;set; }

        public DelegateCommand TestCommand { get; set; }
        public DelegateCommand<MouseEventArgs> MouseMoveCommand { get; set; }
        public DelegateCommand<MouseButtonEventArgs> MouseLeftButtonUpCommand { get; set; }

        public Action<NodeGraphicsArgs.AddToCanvasArgs>? AddToCanvasEvent { get; set; }
        public Action<NodeGraphicsArgs.LinkArgs>? MakeLinkEvent { get; set; }
        public Func<UIElement>? GetView { get; set; }

        private NodeGraphicsArgs.LinkArgs? FocusedLink;

        public NodeCanvasViewModel(IContainerProvider provider) : base(provider)
        {
            NodeGraphicsManager = provider.Resolve<INodeGraphicsManager>();

            TestCommand = new(Test);
            MouseMoveCommand = new(MouseMove);
            MouseLeftButtonUpCommand = new(MouseLeftButtonUp);

            EventAggregator.GetEvent<NodeGraphicsEvent.AddToCanvasEvent>().Subscribe(AddToCanvas);
            EventAggregator.GetEvent<NodeGraphicsEvent.MakeLinkEvent>().Subscribe(MakeLink);
            EventAggregator.GetEvent<NodeGraphicsEvent.GetCanvasRelativePointEvent>().Subscribe(GetCanvasRelativePoint);
            EventAggregator.GetEvent<NodeGraphicsEvent.MousePositionEvent>().Subscribe(MousePosition);
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

        private void Test()
        {
           

        }

        private void AddToCanvas(NodeGraphicsArgs.AddToCanvasArgs e)
        {
            AddToCanvasEvent?.Invoke(e);
        }
    }
}
