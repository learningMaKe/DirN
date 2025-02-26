using DirN.Utils.Events.EventType;
using DirN.Utils.KManager;
using DirN.Utils.NgManager.Curves;
using DirN.Utils.Nodes;
using DirN.ViewModels.Node;
using DirN.Views;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DirN.Utils.NgManager
{
    public class NodeGraphicsManager:ManagerBase<NodeGraphicsManager>,INodeGraphicsManager
    {
        private readonly KeyManager keyManager;

        public static readonly string DefaultText = "undefined";

        public ObservableCollection<MenuItemInfo> CanvasContextMenu { get; private set; } = [];

        public ObservableCollection<INode> Nodes { get; private set; } = [];

        public ObservableCollection<StoredWord> StoredWords { get; private set; } = [];

        public ObservableCollection<ICurve> BezierCurves { get; private set; } = [];

        [OnChangedMethod(nameof(OnStoredWordVisiblityChanged))]
        public bool StoredWordVisiblity { get; set; }

        public DelegateCommand<HandlerType?> AddNewNodeCommand { get; private set; }

        public NodeGraphicsManager(IContainerProvider containerProvider):base(containerProvider)
        {
            Instance = this;
            AddNewNodeCommand = new(AddNewNode);

            eventAggregator = containerProvider.Resolve<IEventAggregator>();
            keyManager = containerProvider.Resolve<KeyManager>();

            keyManager.RegisterKeyBinding(KeyState.Up, Key.T, OnKeyUp);

            InitCanvasContextMenu();
            InitNodes();

            StoredWordVisiblity = false;
        }

        public void Execute()
        {
            ShowText("你被骗了!");
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

        public void AddToCanvas(NodeGraphicsArgs.AddToCanvasArgs e)
        {
            eventAggregator.GetEvent<NodeGraphicsEvent.AddToCanvasEvent>().Publish(e);
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
                Set("删除").
                Set("复制").
                Set("粘贴").
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

        private void OnKeyUp(KeyEventArgs e)
        {
            StoredWordVisiblity = !StoredWordVisiblity;
        }

    }
}
