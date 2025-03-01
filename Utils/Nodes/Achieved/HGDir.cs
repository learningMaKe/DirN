using DirN.Utils.Nodes.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Achieved
{
    [HDes("获取文件", "#780")]
    public class HGDir : DecoratorHandler<DirectoryInfo, FileInfo[]>
    {
        protected override FileInfo[]? Decorate(DirectoryInfo input)
        {
            if (input == null)
            {
                return null;
            }
            return input.GetFiles();
        }
    }
}
