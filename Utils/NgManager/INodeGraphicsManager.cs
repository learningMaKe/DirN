using DirN.Utils.NgManager.Curves;
using DirN.Utils.Nodes;
using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DirN.Utils.NgManager
{
    public interface INodeGraphicsManager
    {
        public bool StoredWordVisiblity { get; set; }

        public double NodeScale { get;  set; }

        public NodeDetail NodeDetail { get; }

        public ObservableCollection<MenuItemInfo> CanvasContextMenu { get; }

        public void AddNode(INode node);

        public void AddCurve(ICurve curve);

        public void RemoveNode(INode node);

        public void RemoveCurve(ICurve curve);

        public void AddNode(HandlerType handlerType, Point position = default);

        public bool LoopDetect();

        public void MoveNode(Vector delta, bool onlySelected = false);

        public void MultiSelectNodes(Rect rect);

        public void SelectNode(params INode[] nodes);

        public void AddSWord();

        public void RemoveSWord(SWord word);

        public void Execute();

        public void ZoomIn();

        public void ZoomOut();

        public void AlignNodes(NodeAlignment alignment);

        public void SaveNode();
    }
}
