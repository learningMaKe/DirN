using DirN.Utils.NgManager.Curves;
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

        public ObservableCollection<MenuItemInfo> CanvasContextMenu { get; }

        public ObservableCollection<INode> Nodes { get; }

        public ObservableCollection<ICurve> BezierCurves { get; }

        public void AddNew();

        public void Remove(StoredWord word);

        public void Execute();
    }
}
