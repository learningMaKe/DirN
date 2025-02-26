using DirN.Utils.Validation;
using DirN.ViewModels.PointerControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DirN.Utils.Nodes.Maps
{
    public static class PointerMap
    {
        public static Dictionary<Type, PointerConfig> Create()
        {
            Dictionary<Type, PointerConfig> pointerMap = [];

            pointerMap.Add(typeof(object), new PointerConfig()
            {
                Header="任意类型",
                PointerColor=Colors.Black,
                ControlType=PointerControlType.PAny,
            });

            pointerMap.Add(typeof(string), new PointerConfig()
            {
                Header="字符串",
                PointerColor=Colors.Orange,
                ControlType=PointerControlType.PString,
            });

            pointerMap.Add(typeof(int), new PointerConfig()
            {
                Header="整数",
                PointerColor=Colors.Red,
                ControlType=PointerControlType.PInt,
            });

            return pointerMap;
        }
    }

}
