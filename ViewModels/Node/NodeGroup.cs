using Fclp.Internals.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.ViewModels.Node
{
    public class NodeGroup:ObservableCollection<INode>
    {
        public IList<INode> SelectedNodes=> [.. this.Where(n => n.IsSelected)];

        public void SelectNode(bool clearCurrentSelections, bool isSelected, params INode[] nodes)
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
    }
}
