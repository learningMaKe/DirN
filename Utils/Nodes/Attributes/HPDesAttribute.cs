using DirN.ViewModels.PointerControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DirN.Utils.Nodes.Attributes
{
    /// <summary>
    /// 描述 <seealso cref="PointerConfig"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Enum|AttributeTargets.Property)]
    public class HPDesAttribute:Attribute
    {
        public HPDesAttribute(string Header,  bool UseConnector = true, PointerControlType ControlType = PointerControlType.PAny, string PointerColor = "Black")
        {
            this.Header = Header;
            this.UseConnector = UseConnector;
            this.PointerColor = (Color)ColorConverter.ConvertFromString(PointerColor);
            this.ControlType = ControlType;
        }

        public HPDesAttribute(string Header,string Description = "", bool UseConnector = true, PointerControlType ControlType = PointerControlType.PAny, string PointerColor = "Black")
        {
            this.Header = Header;
            this.Description = Description;
            this.UseConnector = UseConnector;
            this.PointerColor = (Color)ColorConverter.ConvertFromString(PointerColor);
            this.ControlType = ControlType;
        }

        public string Header = "无标题";
        public string Description = "";
        public bool UseConnector = true;
        public Color PointerColor = Colors.Black;
        public PointerControlType ControlType = PointerControlType.PAny;
    }

    /// <summary>
    /// 用于描述 输入 的 <see cref="PointerConfig"/>
    /// </summary>
    public class HPIDesAttribute : HPDesAttribute
    {
        public int Order = 0;

        public HPIDesAttribute(int order, string Header, bool UseConnector = true, PointerControlType ControlType = PointerControlType.PAny, string PointerColor = "Black"):base(Header, UseConnector, ControlType, PointerColor)
        {
            Order = order;
        }

        public HPIDesAttribute(int order, string Header, string Description = "", bool UseConnector = true, PointerControlType ControlType = PointerControlType.PAny, string PointerColor = "Black") : base(Header, Description, UseConnector, ControlType, PointerColor)
        {
            Order = order;
        }
    }

    /// <summary>
    ///  用于描述 输出 的 <see cref="PointerConfig"/>
    /// </summary>
    public class HPODesAttribute(int order, string Header, string Description = "", string PointerColor = "Black") : HPDesAttribute(Header, Description, false, PointerControlType.PAny, PointerColor)
    {
        public int Order = order;
    }
}
