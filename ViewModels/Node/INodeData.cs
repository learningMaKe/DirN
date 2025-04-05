using DirN.Utils.Nodes.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.ViewModels.Node
{
    public interface INodeData
    {
        public DataContainer Data { get; set; }
    }
}
