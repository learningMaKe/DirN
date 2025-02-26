using DirN.Utils.DirManager;
using DirN.Utils.NgManager;
using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Achieved
{
    public class InputHandler : TypedHandler
    {
        protected override Type[] InputTypes => [];

        protected override Type[] OutputTypes => [typeof(string)];

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
