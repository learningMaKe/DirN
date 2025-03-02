using DirN.Utils.Nodes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DirN.ViewModels.Node
{
    public interface INode:INodePasser
    {
        public bool IsSelected { get; set; }

        public Point Position { get; set; }

        public HandlerType HandlerType { get;set; }

        public INodeHandler? Handler { get; }

        public IList<INode> Next { get; }

        public void Move(Vector delta);

        public void Delete();

        public Rect GetRect();

        public Rect GetScaledRect();
    }
}
