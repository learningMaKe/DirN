using DirN.Utils.Nodes.Achieved.Utils;
using DirN.Utils.Nodes.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Achieved
{
    [HDes("文件过滤", "#f506")]
    public class HFileFilter : AggregatorHandler<(FilterBy,string, FileInfo[]), FileInfo[]>
    {
        protected override FileInfo[] Aggregate((FilterBy,string, FileInfo[]) input)
        {
            throw new NotImplementedException();
        }
    }
}
