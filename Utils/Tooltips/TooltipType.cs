using DirN.Utils.Attirubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Tooltips
{
    public enum TooltipType
    {
        [Color("#ffffff")]
        Normal,
        [Color("#ff0000")]
        Error,
        [Color("#ffff00")]
        Warning,
        [Color("#00ff00")]
        Info
    }
}
