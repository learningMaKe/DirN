using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Exceptions
{
    public interface INodeException
    {
        public INode ErrorNode { get; }
    }
}
