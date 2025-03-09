using DirN.Utils.Tooltips;
using Fclp.Internals.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace DirN.ViewModels.Node
{
    public class NodeGroup:ObservableCollection<INode>,INodePasser
    {

        public IList<INode> SelectedNodes=> [.. this.Where(n => n.IsSelected)];

        public Point Central
        {
            get
            {
                return new Point(this.Average(x => x.Position.X), this.Average(x => x.Position.Y));
            }
        }

        public void SelectNode(bool clearCurrentSelections, bool isSelected = true, params INode[] nodes)
        {
            if (clearCurrentSelections)
            {
                SelectedNodes.ForEach(n => n.IsSelected = false);
            }
            nodes.ForEach(n => n.IsSelected = isSelected);
        }

        public void SelectAll()
        {
            this.ForEach(n => n.IsSelected = true);
        }

        public void DeleteSelectedNodes()
        {
            IList<INode> seletedNodes = SelectedNodes;
            while (seletedNodes.Count > 0)
            {
                INode node = seletedNodes[0];
                node.Delete();
                seletedNodes.Remove(node);
            }
        }

        public void MoveNode(Vector delta, bool onlySelected = false)
        {
            IList<INode> selectedNodes = onlySelected ? SelectedNodes : this;
            foreach (var node in selectedNodes)
            {
                node.Move(delta);
            }
        }

        public void ToCentral(Point centralPoint)
        {
            Point[] points = [.. this.Select(x => x.Position)];
            double avgX = points.Sum(x => x.X) / points.Length;
            double avgY = points.Sum(x => x.Y) / points.Length;
            Point nodeCenter = new(avgX, avgY);
            Vector delta = centralPoint - nodeCenter;
            MoveNode(delta);
        }

        public void UpdateLink()
        {
            foreach(var node in this)
            {
                node.UpdateLink();
            }
        }

        public void CutLink()
        {
            foreach(var node in this)
            {
                node.CutLink();
            }
        }
    }
}
