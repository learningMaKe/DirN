using DirN.Utils.Attirubutes;
using DirN.Utils.Nodes.Attributes;
using DirN.ViewModels.PointerControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Achieved.Utils
{
    [HPCDes("过滤方式",false,PointerControlType.PFilterSelection)]
    public enum FilterBy
    {
        [Des("文件名")]
        Name,
        [Des("扩展名")]
        Ext,
    }
}
