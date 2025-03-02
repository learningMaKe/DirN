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
    public class HInput : OutputHandler<string>
    {
        protected override void ExtraInit()
        {
            if(TryGetOutputConfig<string>(out PointerConfig? config))
            {
                config!.Header = "目录";
                config!.Description = "就目录栏那个";
            }
        }

        protected override string Output()
        {
            return DirectoryManager.Instance.WorkDirectory;
        }
    }
}
