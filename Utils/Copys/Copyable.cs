using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace DirN.Utils.Copys
{

    public class Copyable<T> : BindableBase where T : class, new()
    {
        private static readonly Dictionary<Type, FieldInfo[]> _fieldCache = new Dictionary<Type, FieldInfo[]>();
        private static readonly Dictionary<FieldInfo, Func<object, object>> _getters = new Dictionary<FieldInfo, Func<object, object>>();
        private static readonly Dictionary<FieldInfo, Action<object, object>> _setters = new Dictionary<FieldInfo, Action<object, object>>();

        public T Copy()
        {
            var copy = new T();
            var type = GetType();

            if (!_fieldCache.TryGetValue(type, out var fields))
            {
                fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                _fieldCache[type] = fields;
            }

            foreach (var field in fields)
            {
                var value = GetFieldValue(field, this);
                var copiedValue = DeepCopy(value);
                SetFieldValue(field, copy, copiedValue);
            }

            return copy;
        }

        private object? DeepCopy(object value)
        {
            if (value == null) return null;

            var type = value.GetType();

            // 处理基元类型和字符串
            if (type.IsPrimitive || type == typeof(string))
                return value;

            // 处理 Copyable<T> 派生类型
            if (value is Copyable<T> copyable)
                return copyable.Copy();

            // 处理可克隆对象
            if (value is ICloneable cloneable)
                return cloneable.Clone();

            // 处理数组
            if (type.IsArray)
                return CopyArray((Array)value);

            // 处理集合
            if (value is IEnumerable enumerable)
                return CopyCollection(enumerable, type);

            // 处理复杂对象
            return CopyComplexObject(value, type);
        }

        private Array CopyArray(Array original)
        {
            var copied = (Array)original.Clone();
            for (int i = 0; i < copied.Length; i++)
            {
                var element = copied.GetValue(i);
                copied.SetValue(DeepCopy(element), i);
            }
            return copied;
        }

        private object CopyCollection(IEnumerable original, Type collectionType)
        {
            if (collectionType.IsGenericType)
            {
                var genericType = collectionType.GetGenericTypeDefinition();
                var elementType = collectionType.GetGenericArguments()[0];

                if (genericType == typeof(List<>))
                {
                    var list = (IList)Activator.CreateInstance(collectionType);
                    foreach (var item in (IEnumerable)original)
                        list.Add(DeepCopy(item));
                    return list;
                }
            }

            // 处理其他集合类型（如 Dictionary 等）
            // 可根据需要扩展更多集合类型支持
            throw new NotSupportedException($"Unsupported collection type: {collectionType}");
        }

        private object CopyComplexObject(object original, Type type)
        {
            // 尝试查找并调用 Copy 方法
            var copyMethod = type.GetMethod("Copy", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
            if (copyMethod != null && copyMethod.ReturnType == type)
                return copyMethod.Invoke(original, null);

            // 使用反射创建新实例并复制字段
            var copy = Activator.CreateInstance(type);
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var value = GetFieldValue(field, original);
                SetFieldValue(field, copy, DeepCopy(value));
            }
            return copy;
        }

        private static object GetFieldValue(FieldInfo field, object obj)
        {
            if (!_getters.TryGetValue(field, out var getter))
            {
                var param = Expression.Parameter(typeof(object));
                var access = Expression.Field(Expression.Convert(param, field.DeclaringType), field);
                getter = Expression.Lambda<Func<object, object>>(Expression.Convert(access, typeof(object)), param).Compile();
                _getters[field] = getter;
            }
            return getter(obj);
        }

        private static void SetFieldValue(FieldInfo field, object target, object? value)
        {
            if (!_setters.TryGetValue(field, out var setter))
            {
                var targetParam = Expression.Parameter(typeof(object));
                var valueParam = Expression.Parameter(typeof(object));
                var fieldAccess = Expression.Field(Expression.Convert(targetParam, field.DeclaringType), field);
                var assignment = Expression.Assign(fieldAccess, Expression.Convert(valueParam, field.FieldType));
                setter = Expression.Lambda<Action<object, object>>(assignment, targetParam, valueParam).Compile();
                _setters[field] = setter;
            }
            setter(target, value);
        }
    }
}