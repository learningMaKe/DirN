using DirN.Utils.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DirN.ViewModels.Node
{
    public interface INode:INodePasser
    {
        public Point Position { get; set; }

        public HandlerType HandlerType { get;set; }

        public INodeHandler? Handler { get; }
    }
}
