using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DirN.Utils.Tooltips
{
    public interface ITooltipable
    {
        public Point RaisePosition { get; }
    }
}
