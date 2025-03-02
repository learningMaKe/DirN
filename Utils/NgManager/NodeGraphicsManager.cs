using DirN.Utils.Debugs;
using DirN.Utils.Events.EventType;
using DirN.Utils.Extension;
using DirN.Utils.KManager;
using DirN.Utils.KManager.HKey;
using DirN.Utils.NgManager.Curves;
using DirN.Utils.Nodes;
using DirN.ViewModels.Node;
using DirN.Views;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DirN.Utils.NgManager
{
    public class NodeGraphicsManager:ManagerBase<NodeGraphicsManager>,INodeGraphicsManager
    {
        #region Fields
        private readonly KeyManager keyManager;

        private const double MaxNodeScale = 1.5;
        private const double MinNodeScale = 0.3;
        private const double NodeScaleStep = 0.01;

        private Point? centralPoint;
        #endregion

        public static readonly string DefaultText = "undefined";

        #region Properties
        public double NodeScale { get; set; } = 1;

        public Point CentralPoint
        {
            get
            {
                if (centralPoint == null)
                {
                    centralPoint = GetCentralPoint();
                }
                return centralPoint.Value;
            }
        }

        public ObservableCollection<MenuItemInfo> CanvasContextMenu { get; private set; } = [];

        public NodeGroup Nodes { get; private set; } = [];

        public ObservableCollection<StoredWord> StoredWords { get; private set; } = [];

        public ObservableCollection<ICurve> BezierCurves { get; private set; } = [];

        [OnChangedMethod(nameof(OnStoredWordVisiblityChanged))]
        public bool StoredWordVisiblity { get; set; }

        public DelegateCommand<HandlerType?> AddNewNodeCommand { get; private set; }
        #endregion

        public NodeGraphicsManager(IContainerProvider containerProvider):base(containerProvider)
        {
            AddNewNodeCommand = new(AddNewNode);

            eventAggregator = containerProvider.Resolve<IEventAggregator>();
            keyManager = containerProvider.Resolve<KeyManager>();

            keyManager.RegisterEvent<KeyEventArgs>(EventId.V_StoredWord, OnKeyEnter);
            keyManager.RegisterEvent<KeyEventArgs>(EventId.Node_Focus, e => FocusNode());

            keyManager.RegisterEvent<KeyEventArgs>(EventId.Node_SelectAll, e => Nodes.SelectAll());
            keyManager.RegisterEvent<KeyEventArgs>(EventId.Node_DeleteSelected, e => Nodes.DeleteSelectedNodes());

            InitCanvasContextMenu();
            InitNodes();
        }

        #region Public Methods
        public void MoveNode(Vector delta, bool onlySelected = false)
        {
            IList<INode> selectedNodes = onlySelected? Nodes.SelectedNodes : Nodes;
            foreach (var node in selectedNodes)
            {
                node.Move(delta);
            }
        }

        public void UpdateLink()
        {
            foreach (var node in Nodes)
            {
                node.UpdateLink();
            }
        }

        public void MultiSelectNodes(Rect rect)
        {
            Point centralPoint = GetCentralPoint();
            IList<INode> selectedNodes = [];
            foreach (var node in Nodes)
            {
                Rect nodeRect = node.GetRect().ScaleTransform(centralPoint, NodeScale);
                if (nodeRect.IntersectsWith(rect))
                {
                    selectedNodes.Add(node);
                }
            }
            SelectNode([.. selectedNodes]);
        }

        public void SelectNode(params INode[] nodes)
        {

            ModifierKeys modifiers = Keyboard.Modifiers;
            Nodes.SelectNode(
                modifiers ==ModifierKeys.None,
                modifiers != ModifierKeys.Alt,
                [.. nodes]);
        }

        public void FocusNode()
        {
            Point[] points= [.. Nodes.Select(x => x.Position)];
            double avgX = points.Sum(x => x.X) / points.Length;
            double avgY = points.Sum(x => x.Y) / points.Length;
            Point nodeCenter = new(avgX, avgY);
            Point centralPoint = GetCentralPoint();
            Vector delta = centralPoint - nodeCenter;
            MoveNode(delta);
            UpdateLink();
        }

        public void AlignNode(INode node, NodeAlignment alignment)
        {
            IList<INode> SelectedNodes = Nodes.SelectedNodes;
            if (SelectedNodes.Count == 0) return;
            if (alignment == NodeAlignment.Left)
            {
                foreach (var selectedNode in SelectedNodes)
                {
                    selectedNode.Position = new(node.Position.X, selectedNode.Position.Y);
                }
            }
            else if (alignment == NodeAlignment.Top)
            {
                foreach (var selectedNode in SelectedNodes)
                {
                    selectedNode.Position = new(selectedNode.Position.X, node.Position.Y);
                }
            }
        }

        public void AlignNodes(NodeAlignment alignment)
        {
            IList<INode> SelectedNodes = Nodes.SelectedNodes;
            if (SelectedNodes.Count == 0) return;
            if(alignment == NodeAlignment.Left)
            {
                double minX = SelectedNodes.Min(x => x.Position.X);
                foreach (var node in SelectedNodes)
                {
                    node.Position = new(minX, node.Position.Y);
                }
            }
            else if(alignment == NodeAlignment.Right)
            {
                double maxX = SelectedNodes.Max(x => x.Position.X);
                foreach (var node in SelectedNodes)
                {
                    node.Position = new(maxX, node.Position.Y);
                }
            }
            else if(alignment == NodeAlignment.Top)
            {
                double minY = SelectedNodes.Min(x => x.Position.Y);
                foreach (var node in SelectedNodes)
                {
                    node.Position = new(node.Position.X, minY);
                }
            }
            else if(alignment == NodeAlignment.Bottom)
            {
                double maxY = SelectedNodes.Max(x => x.Position.Y);
                foreach (var node in SelectedNodes)
                {
                    node.Position = new(node.Position.X, maxY);
                }
            }
        }

        private void OnKeyEnter(KeyEventArgs e)
        {
            StoredWordVisiblity =!StoredWordVisiblity;
        }

        public void Execute()
        {
            //KeyManager.Instance.ChangeKey<KeyMap>(EventId.V_StoredWord, new(KeyState.Down, Key.V));
            KeyManager.Instance.UserInit();
        }

        public void AddNewNode(HandlerType? handlerType)
        {
            if (handlerType == null) return;
            Nodes.Add(new BaseNodeViewModel(this)
            {
                Position = GetCanvasMousePosition(),
                HandlerType = handlerType.Value,
            });
        }

        public void AddNew()
        {
            StoredWords.Add(new StoredWord()
            {
                Word = DefaultText,
                Index = StoredWords.Count
            });
        }

        public void MakeLink(NodeGraphicsArgs.LinkArgs args)
        {
            eventAggregator.GetEvent<NodeGraphicsEvent.MakeLinkEvent>().Publish(args);
        }

        public void Remove(StoredWord word)
        {
            int index = StoredWords.IndexOf(word);
            if (index == -1) return;
            StoredWords.Remove(word);
            for (int i = index; i < StoredWords.Count; i++)
            {
                StoredWords[i].Index = i;
            }

        }

        public Point GetCentralPoint()
        {
            NodeGraphicsArgs.GetCentralPointArgs args = new();
            eventAggregator.GetEvent<NodeGraphicsEvent.GetCentralPointEvent>().Publish(args);
            return args.CentralPoint;
        }

        public Point GetCanvasRelativePoint(UIElement element,Point pointRelativeToElement)
        {
            NodeGraphicsArgs.GetCanvasRelativePointArgs args = new()
            {
                Element = element,
                ElementRelativePoint = pointRelativeToElement
            };
            eventAggregator.GetEvent<NodeGraphicsEvent.GetCanvasRelativePointEvent>().Publish(args);
            return args.CanvasRelativePoint;
        }

        public Point GetCanvasMousePosition()
        {
            NodeGraphicsArgs.MousePositionArgs args = new();
            eventAggregator.GetEvent<NodeGraphicsEvent.MousePositionEvent>().Publish(args);
            return args.MousePosition;
        }

        public void ZoomIn()
        {
            NodeScale = Math.Min(MaxNodeScale, NodeScale + NodeScaleStep);
        }

        public void ZoomOut()
        {
            NodeScale = Math.Max(MinNodeScale, NodeScale - NodeScaleStep);
        }

        #endregion

        #region Private Methods
        #region Init

        private void InitCanvasContextMenu()
        {
            HandlerType[]? handlerTypes = Enum.GetValues(typeof(HandlerType)) as HandlerType[] ?? throw new InvalidOperationException("HandlerType should not be null.");
            MenuItemInfo[] AddHandlers = [.. handlerTypes.
                Select(x => MenuItemInfoFactory.Create(x, AddNewNodeCommand)).
                Where(x => x!= null).
                Cast<MenuItemInfo>()
                ];

            CanvasContextMenu = [.. MenuBuilder.BuildMenu().
                Set("新建").
                    Next().
                    SetRange(AddHandlers).
                    Back().
                Set("对齐").
                    Next().
                    Set("左对齐", new DelegateCommand(() => AlignNodes(NodeAlignment.Left))).
                    Set("右对齐", new DelegateCommand(() => AlignNodes(NodeAlignment.Left))).
                    Set("上对齐", new DelegateCommand(() => AlignNodes(NodeAlignment.Top))).
                    Set("下对齐", new DelegateCommand(() => AlignNodes(NodeAlignment.Bottom))).
                    Back().
                Build()];
        }

        private void InitNodes()
        {

        }
        #endregion

        private void OnStoredWordVisiblityChanged()
        {
            eventAggregator.GetEvent<NodeGraphicsEvent.StoredWordVisibilityEvent>().Publish(StoredWordVisiblity);
        }

        private void NodeEnlarge(MouseWheelEventArgs args)
        {
            NodeScale = Math.Min(MaxNodeScale, NodeScale + NodeScaleStep);
        }

        private void NodeShrink(MouseWheelEventArgs args)
        {
            NodeScale = Math.Max(MinNodeScale, NodeScale - NodeScaleStep);
        }


        #endregion
    }
}
