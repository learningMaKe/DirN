using DirN.Utils.NgManager;
using DirN.Utils.Nodes;
using PropertyChanged;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace DirN.ViewModels.Node
{
    public class BaseNodeViewModel : BindableBase,INode
    {
        public NodeGraphicsManager NodeGraphicsManager;

        public Point Position { get; set; }
        public int ZIndex { get; set; }

        public DelegateCommand<DragDeltaEventArgs> DragDeltaCommand { get; private set; }
        public DelegateCommand<DragCompletedEventArgs> DragCompletedCommand { get; private set; }
        public DelegateCommand<DragStartedEventArgs> DragStartedCommand { get; private set; }
        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand RemoveCommand { get; private set; }
        public DelegateCommand DebugCommand { get; private set; }
        public DelegateCommand TestOutputCommand { get; private set; }
        public DelegateCommand CutLinkCommand { get; private set; }
        public DelegateCommand DataFlowCommand { get; private set; }


        [OnChangedMethod(nameof(OnHandlerTypeChanged))]
        public HandlerType HandlerType { get; set; }

        public INodeHandler? Handler { get;private set; }

        public BaseNodeViewModel(NodeGraphicsManager nodeGraphicsManager)
        {
            this.NodeGraphicsManager = nodeGraphicsManager;

            DragDeltaCommand = new(DragDelta);
            DragCompletedCommand= new(DragCompleted);
            DragStartedCommand = new(DragStarted);
            LoadedCommand = new(Loaded);
            RemoveCommand = new(Remove);
            DebugCommand = new(OnDebug);
            CutLinkCommand = new(CutLink);
            TestOutputCommand = new(OnTestOutput);
            DataFlowCommand = new(OnDataFlow);
        }

        public void UpdateLink()
        {
            Handler?.UpdateLink();
        }

        public void CutLink()
        {
            Handler?.CutLink();
        }

        private void DragDelta(DragDeltaEventArgs e)
        {
            Position = new(Position.X + e.HorizontalChange, Position.Y + e.VerticalChange);
            UpdateLink();
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
            OnHandlerTypeChanged();
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
            MessageBox.Show(string.Join("\n", Handler!.GetOutput()?? ["No output"]));
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
