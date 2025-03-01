using DirN.Utils.DirManager;
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
    [HDes("输入")]
    public class HInput : TypedHandler
    {
        public override Type[] InputTypes => [];

        public override Type[] OutputTypes => [typeof(string)];

        public override void Init(INode parent)
        {
            base.Init(parent);
            Header = "输入";
        }

        protected override IList<object?> Handle(IList<object?> input)
        {
            return [DirectoryManager.Instance.Directory];
        }
    }
}
