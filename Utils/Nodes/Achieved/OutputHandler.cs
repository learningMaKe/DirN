using DirN.Utils.NgManager;
using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Achieved
{
    public class OutputHandler : TypedHandler
    {
        protected override Type[] InputTypes => [typeof(object)];

        protected override Type[] OutputTypes => [];

        public override void Init(INode parent)
        {
            base.Init(parent);
            Header = "输出";
        }

        protected override IList<object?> Handle(IList<object?> input)
        {
            NodeGraphicsManager.Instance.ShowText("输出：" + input[0]);
            return [];
        }
    }
}
