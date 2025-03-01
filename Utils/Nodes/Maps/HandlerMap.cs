#define LAZY_MODE
using DirN.Utils.Extension;
using DirN.Utils.Maps;
using DirN.Utils.Nodes.Achieved;
using DirN.Utils.Nodes.Attributes;
using DirN.ViewModels.Node;
using System.Diagnostics;
using System.Reflection;
using static DirN.Utils.Nodes.HandlerFactory;
using HT = DirN.Utils.Nodes.HandlerType;

namespace DirN.Utils.Nodes.Maps
{
    public class HandlerMap: IMapCreator<HT, HandlerConfig>
    {
        public void Create(Dictionary<HT, HandlerConfig> source)
        {
            // 注册节点
            // 偷懒模式通过反射注册节点，
            // 非偷懒模式通过配置注册节点
            #if LAZY_MODE
            Assembly assembly = Assembly.GetExecutingAssembly();
            HT[] handlers = (HT[])Enum.GetValues(typeof(HT));
            Type[] handleTypes= [.. assembly.GetTypes().Where(t => typeof(TypedHandler).IsAssignableFrom(t))];
            foreach(HT ht in handlers)
            {
                string typeName = "H" + ht.ToString();
                Type? type = handleTypes.FirstOrDefault(t => t.Name == typeName);
                if(type == null)
                {
                    Debug.WriteLine("未找到类型：" + typeName);
                    continue;
                }
                if (!typeof(TypedHandler).IsAssignableFrom(type))
                {
                    Debug.WriteLine("类型不匹配：" + typeName);
                    continue;
                }
                HDesAttribute des = type.GetCustomAttribute<HDesAttribute>() ?? new(ht.ToString());
                HandlerConfig? hc = CC(type, des.Header);
                if(hc == null)
                {
                    Debug.WriteLine("未找到配置：" + typeName);
                    continue;
                }
                source.Set(ht, hc);
            }

            #else
            source.
                Set(HT.Input,
                CC<HInput>("输入")).

                Set(HT.Output,
                CC<HOutput>("输出")).

                Set(HT.Debug,
                CC<HDebug>("调试")).

                Set(HT.Join,
                CC<HJoin>("连接")).

                Set(HT.SWord,
                CC<HSWord>("存储单词")).

                Set(HT.GFile,
                CC<HGFile>("获取文件"));

            #endif
        }

    }
}
