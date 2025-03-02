using DirN.Utils.Events.EventType;
using DirN.Utils.Extension;
using DirN.Utils.NgManager.Curves;
using DirN.Utils.Nodes;
using DirN.Views.Node;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DirN.ViewModels.Node
{
    public abstract class ConnectorViewModel:BindableBase,IConnector
    {
        private Connector? connector;

        public object? Data
        {
            get => GetData();
            set => SetData(value);
        }

        public DelegateCommand<MouseButtonEventArgs> MouseLeftButtonDownCommand { get;private set; }
        
        public IPointer PointerParent { get; set; }

        public Color ConnectorColor =>PointerParent.PointerConfig!.PointerColor;

        public Brush ConnectorBrush => PointerParent.PointerConfig!.PointerBrush;

        public bool IsInput => PointerParent.IsInput;

        public abstract IList<INode> LinkedNodes { get; }

        public Connector Connector
        {
            get
            {
                connector ??= GetConnector?.Invoke() ?? throw new NullReferenceException("GetConnector is null");
                return connector;
            }
        }

        public Func<Connector>? GetConnector;

        public Action<bool>? ConnectorStateUpdated { get; set; }

        public ConnectorViewModel(IPointer parent)
        {
            this.PointerParent = parent;
            MouseLeftButtonDownCommand = new(MouseLeftButtonDown);
        }

        public abstract void RemoveLink(ICurve curve);
        public abstract void AddLink(ICurve curve);
        public abstract void UpdateLink();
        public abstract void CutLink();
        public virtual object? GetData() { return null; }
        public virtual void SetData(object? data) { }

        protected abstract void MakeLink();

        protected NodeGraphicsArgs.LinkArgs GetLinkArgs(ICurve linkCurve)
        {
            NodeGraphicsArgs.LinkArgs args = new()
            {
                Curve = linkCurve,
                FilterCallback = elelment =>
                {
                    return HitTestFilterBehavior.Continue;
                },

                ResultCallback = result =>
                {
                    if (result.VisualHit is not Ellipse ellipse) return HitTestResultBehavior.Continue;
                    Connector? connector = ellipse.GetParent<Connector>();

                    if (connector is null) return HitTestResultBehavior.Continue;

                    if (connector.DataContext is not ConnectorViewModel viewModel) return HitTestResultBehavior.Continue;

                    if (viewModel.PointerParent!.NodeParent == PointerParent!.NodeParent) return HitTestResultBehavior.Continue;

                    if (!(viewModel.IsInput ^ IsInput)) return HitTestResultBehavior.Continue;

                    linkCurve.EndPointOwner = viewModel;
                    linkCurve.MakeSureLinkFlow();

                    return HitTestResultBehavior.Stop;
                }
            };
            return args;
        }

        private void MouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
            MakeLink();
        }
    }
}
