using DirN.Utils.Debugs;
using DirN.Utils.Extension;
using DirN.Utils.NgManager;
using DirN.Utils.NgManager.Curves;
using DirN.Utils.Nodes;
using DirN.Utils.Nodes.Converters;
using DirN.Utils.Tooltips;
using Newtonsoft.Json;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DirN.ViewModels.Node
{
    [DebuggerDisplay("HashCode = {GetHashCode()}")]
    public class BaseNodeViewModel : BindableBase,INode,IViewGetter
    {
        public bool IsSelected { get; set; }

        [OnChangedMethod(nameof(OnPositionChanged))]
        public Point Position { get; set; }

        public Point RaisePosition { get;private set; }

        public int ZIndex { get; set; }

        public bool IsExpanded { get; set; } = true;

        public Guid Id { get; set; } = Guid.NewGuid();

        public IList<ICurve> Output
        {
            get
            {
                if(Handler == null)
                {
                    return [];
                }
                return Handler.OutputCurve;
            }
        }

        public IList<ICurve> Input
        {
            get
            {
                if (Handler == null)
                {
                    return [];
                }
                return Handler.InputCurve;
            }
        }

        public DelegateCommand<DragDeltaEventArgs> DragDeltaCommand { get; private set; }
        public DelegateCommand<DragCompletedEventArgs> DragCompletedCommand { get; private set; }
        public DelegateCommand<DragStartedEventArgs> DragStartedCommand { get; private set; }
        public DelegateCommand<MouseButtonEventArgs> MouseLeftButtonDownCommand { get; private set; }
        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand RemoveCommand { get; private set; }
        public DelegateCommand DebugCommand { get; private set; }
        public DelegateCommand TestOutputCommand { get; private set; }
        public DelegateCommand CutLinkCommand { get; private set; }
        public DelegateCommand DataFlowCommand { get; private set; }

        public DelegateCommand<NodeAlignment?> AlignCommand { get; private set; }

        [OnChangedMethod(nameof(OnHandlerTypeChanged))]
        public HandlerType HandlerType { get; set; }

        public INodeHandler? Handler { get;private set; }

        private FrameworkElement? _view;
        public FrameworkElement View
        {
            get 
            {
                if(_view == null && GetView!= null)
                {
                    _view = GetView();
                }
                return _view?? throw new NullReferenceException("View is null");
            } 
        }

        public Func<FrameworkElement>? GetView { get; set; }

        public BaseNodeViewModel()
        {
            DragDeltaCommand = new(DragDelta);
            DragCompletedCommand= new(DragCompleted);
            DragStartedCommand = new(DragStarted);
            AlignCommand = new(Align);
            MouseLeftButtonDownCommand = new(MouseLeftButtonDown);
            LoadedCommand = new(Loaded);
            RemoveCommand = new(Remove);
            DebugCommand = new(OnDebug);
            CutLinkCommand = new(CutLink);
            TestOutputCommand = new(OnTestOutput);
            DataFlowCommand = new(OnDataFlow);
        }

        private void Align(NodeAlignment? alignment)
        {
            if (alignment == null) return;
            NodeGraphicsManager.Instance.AlignNode(this, alignment.Value);
        }

        public bool DataFlow()
        {
            return Handler?.DataFlow() ?? false;
        }

        public void Move(Vector delta)
        {
            Position = new(Position.X + delta.X, Position.Y + delta.Y);
        }

        public void Delete()
        {
            Remove();
        }

        public Rect GetRect()
        {
            return new(Position, new Size(View.ActualWidth, View.ActualHeight));
        }

        public Rect GetScaledRect()
        {
            return GetRect().ScaleRestore();
        }

        public void UpdateLink()
        {
            Handler?.UpdateLink();
        }

        public void CutLink()
        {
            Handler?.CutLink();
        }

        private void MouseLeftButtonDown(MouseButtonEventArgs e)
        {
            NodeGraphicsManager.Instance.SelectNode(this);
        }

        private void OnPositionChanged()
        {
            UpdateLink();
            if (GetView != null) 
            {
                RaisePosition = Point.Add(Position, new Vector(View.ActualWidth / 2, TooltipParameter.UpOffset));
            }
        }

        private void DragDelta(DragDeltaEventArgs e)
        {
            Vector delta = new(e.HorizontalChange, e.VerticalChange);
            NodeGraphicsManager.Instance.MoveNode(delta, true);
        }

        private void Remove()
        {
            CutLink();
            TooltipManager.Instance.RemoveTooltip(this);
            NodeGraphicsManager.Instance.RemoveNode(this);
        }

        private void DragCompleted(DragCompletedEventArgs e)
        {
            ZIndex -= 100;
        }

        private void DragStarted(DragStartedEventArgs e)
        {
            ZIndex += 100;
        }

        private void Loaded()
        {
            OnPositionChanged();
        }

        private void OnHandlerTypeChanged()
        {
            if (HandlerManager.GetHandler(this,HandlerType, out var handler))
            {
                Handler = handler;
            }
        }

        private void OnDebug()
        {
            foreach (var predecessor in Handler!.Predecessors)
            {
                TooltipManager.Instance.Tooltip(predecessor, "Predecessor");
            }
        }

        private void OnTestOutput()
        {
            string outputs = string.Join("\n", Handler!.HandleData() ?? ["No output"]);
            if (!string.IsNullOrEmpty(outputs))
            {
                MessageBox.Show(outputs);
            }
        }

        private void OnDataFlow()
        {
            Handler!.DataFlow();
        }

    }
}
