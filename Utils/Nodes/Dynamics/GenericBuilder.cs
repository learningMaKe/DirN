using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Dynamics
{
    public static class GenericBuilder
    {
        public static T? MakeTuple<T>(params object?[] values) where T :ITuple
        {
            Type[] tupleTypes = typeof(T).GetGenericArguments();
            // 创建元组类型
            Type? tupleType = Type.GetType($"System.ValueTuple`{tupleTypes.Length}");

            if (tupleType == null) return default;

            // 构造泛型参数
            Type genericTupleType = tupleType.MakeGenericType(tupleTypes);

            // 获取构造函数
            ConstructorInfo? constructor = genericTupleType.GetConstructor(tupleTypes);

            if (constructor is null) return default;

            // 创建元组实例
            object? tupleInstance = constructor.Invoke([.. values]);

            T? tuple = (T?)tupleInstance;
            if (tuple is null) return default;
            return tuple;
        }

        public  static IList<object?> UnpackTuple<T>(ITuple tuple) where T : ITuple
        {
            Type type = typeof(T);
            Type[] tupleTypes = type.GetGenericArguments();
            IList<object?> values = [];
            for(int i=1; i<=tupleTypes.Length; i++)
            {
                PropertyInfo? property = type.GetProperty($"Item{i}");
                if (property is null) continue;
                values.Add(property.GetValue(tuple));
            }
            return values;
        }
    }
}
