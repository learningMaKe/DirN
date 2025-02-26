using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.ViewModels.PointerControl
{
    public interface IPointerControl:INodeData
    {
        public void Init(IPContainer parent);
    }
}
