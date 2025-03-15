using DirN.Utils.NgManager.Curves;
using DirN.Utils.Nodes;
using DirN.Utils.Seralizes;
using DirN.Utils.Tooltips;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DirN.ViewModels.Node
{
    public interface INode:INodePasser,ITooltipable,IJsonSerialize
    {
        public bool IsSelected { get; set; }

        public Point Position { get; set; }

        public HandlerType HandlerType { get;set; }

        public INodeHandler? Handler { get; }

        /// <summary>
        /// 节点的所有输出节点
        /// </summary>
        public IList<ICurve> Output { get; }

        /// <summary>
        /// 节点的所有输入节点
        /// </summary>
        public IList<ICurve> Input { get; }

        public IList<INode> OutputNodes => Output.Where(x => x.Ender != null).Select(x => x.Ender!.PointerParent.NodeParent).Distinct().ToList() ?? [];

        public IList<INode> InputNodes => Input.Where(x => x.Starter != null).Select(x => x.Starter!.PointerParent.NodeParent).Distinct().ToList() ?? [];

        public IList<ICurve> Linked => [.. Input.Concat(Output).Distinct()];

        public IList<object?> InputDataGroup => Handler?.InputGroup.Select(x => x.Data).ToList() ?? [];

        public IList<IConnector> InputConnectors => Handler?.InputGroup.Select(x => x.Connector).ToList() ?? [];

        public IList<IConnector> OutputConnectors => Handler?.OutputGroup.Select(x => x.Connector).ToList() ?? [];

        public void Move(Vector delta);

        public void Delete() { }

        public Rect GetRect();

        public Rect GetScaledRect();

        public bool DataFlow();
    }
}
