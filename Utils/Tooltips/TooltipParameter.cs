using DirN.Utils.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DirN.Utils.Tooltips
{
    public static class TooltipParameter
    {
        private readonly static Dictionary<TooltipType, Color> colorDict = 
            Mapper<TooltipColorMap, TooltipType, Color>.Instance;

        public const double UpOffset = -2;

        public static Color GetColor(TooltipType type)
        {
            if (colorDict.TryGetValue(type, out Color value))
            {
                return value;
            }
            return Colors.White;

        }


    }
}
