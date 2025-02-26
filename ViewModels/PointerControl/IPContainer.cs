using DirN.Utils.Nodes;
using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.ViewModels.PointerControl
{
    public interface IPContainer:INodeData
    {
        public PointerControlType ControlType { get; }

        public IPointer PointerParent { get; set; }
    }
}
