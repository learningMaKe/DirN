using DirN.Utils.WithColors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DirN.Utils.Tooltips
{
    public class Tooltiper(ITooltipable tooltipable) : BindableBase
    {
        public string ToolTipText { get; set; } = string.Empty;

        public TooltipType TooltipType { get; set; } = TooltipType.Normal;

        public ITooltipable Tooltipable { get; set; } = tooltipable;

        public WithColor Colorer { get; set; } = new WithColor();
    }
}
