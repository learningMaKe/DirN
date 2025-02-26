using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DirN.Utils.Nodes.Achieved
{
    public class DebugHandler : TypedHandler
    {
        protected override Type[] InputTypes => [typeof(object)];

        protected override Type[] OutputTypes => [];

        public override void Init(INode parent)
        {
            base.Init(parent);
            Header = "调试节点";
        }

        protected override IList<object?> Handle(IList<object?> input)
        {
            string s = string.Join(",", input.Select(x => x ?? "null"));
            MessageBox.Show(s);
            return [];
        }
    }
}
