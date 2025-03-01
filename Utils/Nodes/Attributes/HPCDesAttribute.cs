using DirN.ViewModels.PointerControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DirN.Utils.Nodes.Attributes
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Enum)]
    public class HPCDesAttribute:Attribute
    {
        public HPCDesAttribute(string Header, bool UseConnector = true, PointerControlType ControlType = PointerControlType.PAny, string PointerColor = "Black")
        {
            this.Header = Header;
            this.UseConnector = UseConnector;
            this.PointerColor = (Color)ColorConverter.ConvertFromString(PointerColor);
            this.ControlType = ControlType;
        }

        public string Header = "无标题";
        public bool UseConnector = true;
        public Color PointerColor = Colors.Black;
        public PointerControlType ControlType = PointerControlType.PAny;
    }
}
