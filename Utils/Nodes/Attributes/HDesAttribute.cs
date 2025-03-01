using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DirN.Utils.Nodes.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HDesAttribute: Attribute
    {

        public HDesAttribute(string header = "None", string color = "Black", string description = "No description provided.")
        {
            Header = header;
            Description = description;
            try
            {
                MainColor = (Color)ColorConverter.ConvertFromString(color);
            }
            catch (FormatException)
            {
                MainColor = Colors.Black;
            }
        }

        public string Header { get; init; }

        public string Description { get; init; }

        public Color MainColor { get; init; }
        
    }
}
