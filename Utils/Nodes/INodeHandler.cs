using DirN.Utils.Base;
using DirN.Utils.NgManager.Curves;
using DirN.Utils.Nodes.Datas;
using DirN.Utils.WithColors;
using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DirN.Utils.Nodes
{
    public interface INodeHandler:INodePasser
    {
        public void Init(INode parent);

        public INode? Parent { get; init; }

        public string Header { get; set; }

        public string Description { get; set; }

        public IList<ICurve> OutputCurve { get; }

        public IList<ICurve> InputCurve { get; }

        public WithColor Colorer { get; }

        public IList<INode> Predecessors { get; }

        public ObservableCollection<IPointer> InputGroup { get; set; }
        public ObservableCollection<IPointer> OutputGroup { get; set; }

        /// <summary>
        /// 数据经由该节点流向下一个节点
        /// </summary>
        public bool DataFlow();
    }
}
