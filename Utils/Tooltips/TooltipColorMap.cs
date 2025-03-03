using DirN.Utils.Attirubutes;
using DirN.Utils.Maps;
using DirN.Utils.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DirN.Utils.Tooltips
{
    public class TooltipColorMap : IMapCreator<TooltipType, Color>
    {
        public void Create(Dictionary<TooltipType, Color> source)
        {
            FieldInfo[] fields = typeof(TooltipType).GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo field in fields)
            {
                ColorAttribute? attributes = field.GetCustomAttribute<ColorAttribute>();
                TooltipType? tooltipType = (TooltipType?)field.GetValue(null);
                if (tooltipType == null) continue;
                Color color = attributes?.Color ?? Colors.White;
                source.Add(tooltipType.Value, color);
            }
        }
    }
}
