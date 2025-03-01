using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes
{
    public class HandlerConfig
    {
        public HandlerConfig(string Header, Func<INode, INodeHandler> Create)
        {
            this.Header = Header;
            this.Create = Create;
        }

        public string Header { get; set; }

        public Func<INode, INodeHandler> Create { get; set; }

    }
}
