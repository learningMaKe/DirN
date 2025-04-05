using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Datas
{
    public class DataContainer(object? data) : BindableBase
    {
        [JsonIgnore]
        public Type DataType => Content?.GetType() ?? typeof(object);

        [OnChangedMethod(nameof(OnContentChanged))]
        public object? Content { get; set; } = data;

        public event EventHandler<object?>? DataChanged;

        public void SetData(Type type, object? data)
        {
            ArgumentNullException.ThrowIfNull(type);

            ArgumentNullException.ThrowIfNull(data);

            //if (!HandlerManager.CanConvertData(type, data.GetType())) throw new InvalidCastException("Cannot convert data to type " + type.Name);

            Content = data;
        }

        public void SetData<T>(T data)
        {
            SetData(typeof(T), data);
        }

        public T? GetData<T>() 
        {
            if (Content == null) return default;
            if(!HandlerManager.ConvertData(Content.GetType(), typeof(T), Content, out var result)) throw new InvalidCastException("Cannot convert data to type " + typeof(T).Name);
            if (result == null) return default;
            return (T)result;
        }

        public object? GetData(Type type)
        {
            if (Content == null) throw new NullReferenceException("Data is null");
            if (!HandlerManager.CanConvertData(Content.GetType(), type)) throw new InvalidCastException("Cannot convert data to type " + type.Name);
            if (!HandlerManager.ConvertData(Content.GetType(), type, Content, out var result)) throw new InvalidCastException("Cannot convert data to type " + type.Name);
            return result;
        }


        public bool TryGetData(Type type,out object? result)
        {
            result = null;
            if (Content == null)
            {
                return false; 
            }
            HandlerManager.ConvertData(Content.GetType(), type, Content, out result);
            return true;
        }

        private void OnContentChanged(object sender, object? e)
        {
            DataChanged?.Invoke(sender, e);
        }
    }
}
