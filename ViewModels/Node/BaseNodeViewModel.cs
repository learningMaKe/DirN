using DirN.Utils.Debugs;
using DirN.Utils.Extension;
using DirN.Utils.NgManager;
using DirN.Utils.Nodes;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DirN.ViewModels.Node
{
    public class BaseNodeViewModel : BindableBase,INode,IViewGetter
    {
        public bool IsSelected { get; set; }

        public NodeGraphicsManager NodeGraphicsManager;

        [OnChangedMethod(nameof(OnPositionChanged))]
        public Point Position { get; set; }

        public int ZIndex { get; set; }

        public IList<INode> Next
        {
            get
            {
                if(Handler == null)
                {
                    return [];
                }
                return Handler.Next;
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

        public BaseNodeViewModel(NodeGraphicsManager nodeGraphicsManager)
        {
            this.NodeGraphicsManager = nodeGraphicsManager;
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
            NodeGraphicsManager.AlignNode(this, alignment.Value);
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
            return GetRect().ScaleTransform(NodeGraphicsManager.GetCentralPoint(), NodeGraphicsManager.NodeScale);
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
            NodeGraphicsManager.SelectNode(this);
        }

        private void OnPositionChanged()
        {
            UpdateLink();
        }

        private void DragDelta(DragDeltaEventArgs e)
        {
            Vector delta = new(e.HorizontalChange, e.VerticalChange);
            NodeGraphicsManager.MoveNode(delta, true);
        }

        private void Remove()
        {
            CutLink();
            NodeGraphicsManager.Nodes.Remove(this);
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
            //DebugManager.Instance.DrawRect(GetRect().ScaleTransform(NodeGraphicsManager.GetCentralPoint(), NodeGraphicsManager.NodeScale));
            string strs = string.Join("\n", Handler!.InputGroup.Select(i => i.PointerConfig!.Header + " " + i.PointerConfig!.GetHashCode().ToString()));
            MessageBox.Show(strs);
        }

        private void OnTestOutput()
        {
            string outputs = string.Join("\n", Handler!.GetOutput() ?? ["No output"]);
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
