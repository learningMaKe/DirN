using DirN.Utils.NgManager;
using DirN.Utils.Nodes.Attributes;
using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Achieved
{
    [HDes("输出")]
    public class HOutput : TypedHandler
    {
        public override Type[] InputTypes => [typeof(object)];

        public override Type[] OutputTypes => [];

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
