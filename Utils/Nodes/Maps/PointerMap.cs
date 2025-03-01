using DirN.Utils.Extension;
using DirN.Utils.Maps;
using DirN.Utils.Nodes.Attributes;
using DirN.Utils.Validation;
using DirN.ViewModels.PointerControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DirN.Utils.Nodes.Maps
{
    public class PointerMap:IMapCreator<Type, PointerConfig>
    {
        public void Create(Dictionary<Type, PointerConfig> source)
        {

            source.Set(typeof(object), new PointerConfig()
            {
                Header = "任意类型",
                PointerColor = Colors.Black,
                ControlType = PointerControlType.PAny,
            }).
            Set(typeof(string), new PointerConfig()
            {
                Header = "字符串",
                PointerColor = Colors.Orange,
                ControlType = PointerControlType.PString,
            }).
            Set(typeof(int), new PointerConfig()
            {
                Header = "整数",
                PointerColor = Colors.Red,
                ControlType = PointerControlType.PInt,
            }).

            Set(typeof(FileInfo), new PointerConfig()
            {
                Header = "文件",
                PointerColor = Colors.Green,
                ControlType = PointerControlType.PFileInfo,
            }).

            Set(typeof(FileInfo[]),new PointerConfig()
            {
                Header = "文件数组",
                PointerColor = Colors.Green,
                ControlType = PointerControlType.PAny,
            }).

            Set(typeof(DirectoryInfo), new PointerConfig()
            {
                Header = "目录",
                PointerColor = Colors.Blue,
                ControlType = PointerControlType.PAny,
            });

            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.GetTypes())
            {
                HPCDesAttribute? attribute = type.GetCustomAttribute<HPCDesAttribute>();
                if (attribute == null) continue;
                source.Set(type, new PointerConfig()
                {
                    Header = attribute.Header,
                    PointerColor = attribute.PointerColor,
                    ControlType = attribute.ControlType,
                    UseConnector = attribute.UseConnector,
                });
            }

        }
    }

}
