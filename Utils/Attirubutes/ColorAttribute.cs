using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DirN.Utils.Attirubutes
{
    [AttributeUsage(AttributeTargets.All)]
    public class ColorAttribute(string color="White"):Attribute
    {
        public Color Color { get; set; } = (Color)ColorConverter.ConvertFromString(color);
    }
}
