using DirN.Utils.Nodes.Attributes;
using DirN.Utils.Nodes.Datas;
using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DirN.Utils.Nodes
{
    /// <summary>
    /// <para>
    /// 使用 <seealso cref="HPIDesAttribute"/>  标记的属性作为输入
    /// </para>
    /// <para>
    /// 使用 <seealso cref="HPODesAttribute"/>  标记的属性作为输出
    /// </para>
    /// <para>    
    /// 请 <u><b>不要</b></u> 读取 或者 修改 这些属性，因为它们并不保存属于节点的状态，而是节点的配置信息，仅用作节点的初始化。
    /// </para> 
    /// <para>
    /// 以 <see cref="Handle"/> 传入的数据为输入，返回值作为输出。
    /// </para> 
    /// </summary>
    public abstract class MentionedHandler :BaseHandler
    {
        private readonly Dictionary<InputerViewModel, PropertyInfo> inputMap = [];
        private readonly Dictionary<OutputerViewModel, PropertyInfo> outputMap = [];

        public override void Init(INode parent)
        {
            List<(int,InputerViewModel)> inputPointers = [];
            List<(int,OutputerViewModel)> outputPointers = [];
            // 从类属性中获取输入类型
            PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                HPDesAttribute? attribute = property.GetCustomAttribute<HPDesAttribute>();
                if (attribute is null) continue;

                if(attribute is HPIDesAttribute hpi)
                {
                    InputerViewModel pointer = new(parent)
                    {
                        PointerType = property.PropertyType,
                    };
                    pointer.PointerConfig!.Init(hpi);
                    inputMap.Add(pointer, property);
                    inputPointers.Add((hpi.Order, pointer));
                }
                else if(attribute is HPODesAttribute hpo)
                {
                    OutputerViewModel pointer = new(parent)
                    {
                        PointerType = property.PropertyType,
                    };
                    pointer.PointerConfig!.Init(hpo);
                    outputMap.Add(pointer, property);
                    outputPointers.Add((hpo.Order, pointer));
                }
            }

            InputGroup = [.. inputPointers.OrderBy(x => x.Item1).Select(x => x.Item2)];
            OutputGroup = [.. outputPointers.OrderBy(x => x.Item1).Select(x => x.Item2)];
        }

        public sealed override bool DataFlow()
        {
            try
            {
                UpdateInput();
                Handle();
                UpdateOutput();
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected abstract void Handle();

        protected void UpdateInput()
        {
            foreach (var input in inputMap)
            {
                input.Value.SetValue(this, input.Key.Data.GetData(input.Value.PropertyType));
            }
        }

        protected void UpdateOutput()
        {
            foreach (var output in outputMap)
            {
                output.Key.Data = new DataContainer(output.Value.GetValue(this));
            }
        }
    }

}
