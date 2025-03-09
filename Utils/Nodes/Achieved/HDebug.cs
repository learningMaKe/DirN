using DirN.Utils.Nodes.Attributes;
using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DirN.Utils.Nodes.Achieved
{
    [HDes("调试","#F060","调试节点")]
    public class HDebug : TypedHandler
    {
        protected override Type[] InputTypes => [typeof(object)];

        protected override Type[] OutputTypes => [];

        protected override void ExtraInit()
        {
            Header = "调试节点";
        }

        protected override IList<object?> Handle(IList<object> input)
        {
            string s = string.Join(",", input.Select(x => x ?? "null"));
            MessageBox.Show(s);
            return [];
        }
    }
}
