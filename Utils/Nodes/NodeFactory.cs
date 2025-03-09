using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DirN.Utils.Nodes
{
   public static class NodeFactory
    {
        public static BaseNodeViewModel Create(HandlerType handlerType, Point position=default)
        {
            BaseNodeViewModel node = new()
            {
                Position = position,
                HandlerType = handlerType
            };
            return node;
        }
    }
}
