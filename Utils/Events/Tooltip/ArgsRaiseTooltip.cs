using DirN.Utils.Tooltips;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirN.Utils.Extension;

namespace DirN.Utils.Events.Tooltip
{
    public class ArgsRaiseTooltip: EventArgs
    {
        public Point Position { get; set; }

        public string Text { get; set; } = string.Empty;

        public void Add(params ITooltipable[] tooltipables)
        {
            Position = Position.Avg([.. tooltipables.Select(t => t.RaisePosition)]);
        }
    }
}
