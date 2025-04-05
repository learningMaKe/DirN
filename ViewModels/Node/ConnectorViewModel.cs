using DirN.Utils.Events.EventType;
using DirN.Utils.Extension;
using DirN.Utils.NgManager.Curves;
using DirN.Utils.Nodes;
using DirN.Utils.Nodes.Datas;
using DirN.Views.Node;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DirN.ViewModels.Node
{
    public abstract class ConnectorViewModel:BindableBase,IConnector
    {
        private Connector? connector;

        public DataContainer Data
        {
            get => GetData();
            set => SetData(value);
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        public event Action? LoadedCallback;

        public DelegateCommand<MouseButtonEventArgs> MouseLeftButtonDownCommand { get;private set; }
        public DelegateCommand LoadedCommand { get; private set; }

        public IPointer PointerParent { get; set; }

        public Color ConnectorColor =>PointerParent.PointerConfig!.PointerColor;

        public Brush ConnectorBrush => PointerParent.PointerConfig!.PointerBrush;

        public bool IsInput => PointerParent.IsInput;

        public abstract IList<ICurve> LinkedCurves { get; }

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
            LoadedCommand = new(Loaded);
        }

        public abstract void RemoveLink(ICurve curve);
        public abstract void AddLink(ICurve curve);
        public abstract void UpdateLink();
        public abstract void CutLink();
        public virtual DataContainer GetData() { return new(null); }
        public virtual void SetData(DataContainer data) { }

        protected abstract void MakeLink();

        protected virtual void Loaded()
        {
            LoadedCallback?.Invoke();
            LoadedCallback = null;
        }

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

                    linkCurve.Ender = viewModel;
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
