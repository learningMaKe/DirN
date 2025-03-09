using DirN.Utils.Debugs;
using DirN.Utils.Events.EventType;
using DirN.Utils.Extension;
using DirN.Utils.KManager;
using DirN.Utils.KManager.HKey;
using DirN.Utils.NgManager.Curves;
using DirN.Utils.Nodes;
using DirN.Utils.Tooltips;
using DirN.ViewModels.Node;
using DirN.Views;
using Fclp.Internals.Extensions;
using Newtonsoft.Json;
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

        private static readonly Vector CopyOffset = new(10, 10);
        private IList<(HandlerType,Point)> CopiedNode = [];
        #endregion

        public static readonly string DefaultText = "undefined";

        #region Properties

        public double NodeScale { get; set; } = 1;

        public Point CentralPoint => GetCentralPoint();

        public ObservableCollection<MenuItemInfo> CanvasContextMenu { get; private set; } = [];

        [OnChangedMethod(nameof(OnStoredWordVisiblityChanged))]
        public bool StoredWordVisiblity { get; set; }

        public NodeDetail NodeDetail { get; private set; } = new();

        public DelegateCommand<HandlerType?> AddNewNodeCommand { get; private set; }
        
        #endregion

        public NodeGraphicsManager(IContainerProvider containerProvider):base(containerProvider)
        {
            AddNewNodeCommand = new(AddNewNode);

            eventAggregator = containerProvider.Resolve<IEventAggregator>();
            keyManager = containerProvider.Resolve<KeyManager>();

            keyManager.RegisterEvent<KeyEventArgs>(EventId.V_StoredWord, OnKeyEnter);
            keyManager.RegisterEvent<KeyEventArgs>(EventId.Node_Focus, e => FocusNode());

            keyManager.RegisterEvent<KeyEventArgs>(EventId.Node_SelectAll, e => NodeDetail.Nodes.SelectAll());
            keyManager.RegisterEvent<KeyEventArgs>(EventId.Node_DeleteSelected, e => NodeDetail.Nodes.DeleteSelectedNodes());

            keyManager.RegisterEvent<KeyEventArgs>(EventId.Node_Align_Left, e => KeyAlign(e, NodeAlignment.Left));
            keyManager.RegisterEvent<KeyEventArgs>(EventId.Node_Align_Right, e => KeyAlign(e, NodeAlignment.Right));
            keyManager.RegisterEvent<KeyEventArgs>(EventId.Node_Align_Top, e => KeyAlign(e, NodeAlignment.Top));
            keyManager.RegisterEvent<KeyEventArgs>(EventId.Node_Align_Bottom, e => KeyAlign(e, NodeAlignment.Bottom));

            keyManager.RegisterEvent<KeyEventArgs>(EventId.Node_Copy, NodeCopy);
            keyManager.RegisterEvent<KeyEventArgs>(EventId.Node_Cut, NodeCut);
            keyManager.RegisterEvent<KeyEventArgs>(EventId.Node_Paste, NodePaste);

            InitCanvasContextMenu();
            InitNodes();
        }

        #region Public Methods

        public void AddNode(INode node)
        {
            NodeDetail.Nodes.Add(node);
        }

        public void AddCurve(ICurve curve)
        {
            NodeDetail.BezierCurves.Add(curve);
        }

        public void RemoveNode(INode node)
        {
            NodeDetail.Nodes.Remove(node);
        }

        public void RemoveCurve(ICurve curve)
        {
            NodeDetail.BezierCurves.Remove(curve);
        }

        public void AddNode(HandlerType handlerType, Point position = default)
        {
            NodeDetail.Nodes.Add(NodeFactory.Create(handlerType, position));
        }

        #region Execute Operations

        public bool LoopDetect()
        {
            return NodeDetail.LoopDetect();
        }

        public void Execute()
        {
            NodeDetail.Execute();
        }

        #endregion

        #region Node Operations

        public void MakeLink(NodeGraphicsArgs.LinkArgs args)
        {
            eventAggregator.GetEvent<NodeGraphicsEvent.MakeLinkEvent>().Publish(args);
        }

        public void MoveNode(Vector delta, bool onlySelected = false)
        {
            NodeDetail.MoveNode(delta, onlySelected);
        }

        public void MultiSelectNodes(Rect rect)
        {
            Point centralPoint = GetCentralPoint();
            IList<INode> selectedNodes = [];
            foreach (var node in NodeDetail.Nodes)
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
            NodeDetail.SelectNode(nodes);
        }

        public void FocusNode()
        {
            NodeDetail.Nodes.ToCentral(CentralPoint);
        }

        public void AlignNode(INode node, NodeAlignment alignment)
        {
            NodeDetail.AlignNode(node, alignment);
        }

        public void AlignNodes(NodeAlignment alignment)
        {
            NodeDetail.AlignNodes(alignment);
        }


        public void SaveNode()
        {
            string str = JsonConvert.SerializeObject(NodeDetail, Formatting.Indented);
            Debug.WriteLine(str);
            NodeDetail? detail = JsonConvert.DeserializeObject<NodeDetail>(str);
        }

        #endregion

        #region Stored Word Operations
        public void AddSWord()
        {
            NodeDetail.AddSWord();
        }

        public void RemoveSWord(SWord word)
        {
            NodeDetail.RemoveSWord(word);
        }
        #endregion
        
        #region Get Point

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
        #endregion

        #region Zoom

        public void ZoomIn()
        {
            NodeScale = Math.Min(MaxNodeScale, NodeScale + NodeScaleStep);
        }

        public void ZoomOut()
        {
            NodeScale = Math.Max(MinNodeScale, NodeScale - NodeScaleStep);
        }

        #endregion

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

        private void AddNewNode(HandlerType? handlerType)
        {
            if (handlerType is null) return;
            AddNode(handlerType.Value, GetCanvasMousePosition());
        }

        private void NodePaste(KeyEventArgs args)
        {
            IList<INode> nodes = [];
            foreach(var (handlerType,position) in CopiedNode)
            {
                nodes.Add(NodeFactory.Create(handlerType, position));
            }
            foreach(var node in nodes)
            {
                node.Position += CopyOffset;
            }
            NodeDetail.Nodes.AddRange(nodes);
            NodeDetail.Nodes.SelectNode(true, true, [.. nodes]);
        }

        private void NodeCut(KeyEventArgs args)
        {
            NodeCopy(args);
            NodeDetail.Nodes.DeleteSelectedNodes();
        }

        private void NodeCopy(KeyEventArgs args)
        {
            CopiedNode = [.. NodeDetail.Nodes.SelectedNodes.Select(x => (x.HandlerType,x.Position))];
        }

        private void KeyAlign(KeyEventArgs args,NodeAlignment alignment)
        {
            args.Handled = true;
            AlignNodes(alignment);
        }

        private void OnKeyEnter(KeyEventArgs e)
        {
            StoredWordVisiblity = !StoredWordVisiblity;
        }

        private void OnStoredWordVisiblityChanged()
        {
            eventAggregator.GetEvent<NodeGraphicsEvent.StoredWordVisibilityEvent>().Publish(StoredWordVisiblity);
        }

        #endregion
    }
}
