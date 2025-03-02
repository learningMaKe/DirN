using DirN.Utils.DirManager;
using DirN.Utils.NgManager;
using DirN.Utils.Nodes.Attributes;
using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Achieved
{
    [HDes("输出")]
    public class HOutput : InputHandler<FileInfo[]>
    {
        protected override void Input(FileInfo[] input)
        {
            DirectoryManager.Instance.PreviewFiles = [.. input.Select(x => x.FullName)];
        }
    }
}
