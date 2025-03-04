﻿using DirN.Utils.NgManager.Curves;
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

        public ObservableCollection<MenuItemInfo> CanvasContextMenu { get; }

        public NodeGroup Nodes { get; }

        public ObservableCollection<ICurve> BezierCurves { get; }

        public bool HaveLoopEdge();

        public void MoveNode(Vector delta, bool onlySelected = false);

        public void MultiSelectNodes(Rect rect);

        public void SelectNode(params INode[] nodes);

        public void AddNew();

        public void Remove(StoredWord word);

        public void Execute();

        public void ZoomIn();

        public void ZoomOut();
    }
}
