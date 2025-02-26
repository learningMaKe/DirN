using DirN.Utils.Base;
using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes
{
    public interface IPointer:INodePasser,INodeData
    {
        public Type PointerType { get; set; }

        public bool IsInput { get; }

        public INode NodeParent { get;}

        public IConnector Connector { get; }

        public PointerConfig? PointerConfig { get; set; }

        public Action<PointerConfig>? PointerConfigChangedEvent { get; set; }
    }
}
