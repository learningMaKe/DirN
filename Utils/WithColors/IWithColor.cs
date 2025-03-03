using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DirN.Utils.WithColors
{
    public interface IWithColor
    {
        public Color MainColor { get; set; }

        public Brush MainBrush { get; }
    }
}
