using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DirN.Utils.KManager
{
    public class MapHandlerGroup
    {
        private readonly Dictionary<Type, IMapHandler> handlers = [];
        private readonly Dictionary<Type,IMapHandler> keyToHandler = [];
        private readonly Dictionary<Type,IMapHandler> ArgsToHandler = [];

        public MapHandlerGroup AddHandler<TMapHandler>() where TMapHandler : IMapHandler, new()
        {
            TMapHandler handler = new();
            Adder(handler);
            return this;
        }

        private IMapHandler AddHandler(Type type)
        {
            object? obj = Activator.CreateInstance(type);
            if (obj is not IMapHandler handler) throw new ArgumentException("TMapHandler must implement IMapHandler");

            Adder(handler);
            return handler;
        }

        private void Adder(IMapHandler handler)
        {
            handlers.TryAdd(handler.GetType(), handler);
            Type interfaces = handler.GetType().
                GetInterfaces().
                FirstOrDefault(x => x.Name.StartsWith(typeof(IMapHandler<,>).Name)) ??
                throw new ArgumentException("TMapHandler must implement IMapHandler<TKey,TArgs>");
            Type[] args = interfaces.GetGenericArguments();
            keyToHandler.TryAdd(args[0], handler);
            ArgsToHandler.TryAdd(args[1], handler);
        }

        public IList<(Type, IList<(EventId,object)>)> Export()
        {
            List<(Type, IList<(EventId, object)>)> result = [];
            foreach (var handler in handlers.Values)
            {
                result.Add((handler.GetType(), handler.Export()));
            }
            return result;
        }

        public void ChangeMap<TKey>(EventId eventId, TKey newKey) where TKey : notnull
        {
            if(keyToHandler.TryGetValue(typeof(TKey), out IMapHandler? handler))
            {
                handler.ChangeMap(eventId, newKey);
            }
        }

        // 特定值初始化
        public bool Init<TKey>(IList<(EventId, TKey)>? keyMap) where TKey : notnull
        {
            if(keyToHandler.TryGetValue(typeof(TKey), out IMapHandler? handler))
            {
                return handler.Init(keyMap?.Cast<(EventId, object)>().ToList());
            }
            return false;
        }

        // 导入数据
        public bool Init(IList<(Type, IList<(EventId, object)>)> data)
        {
            bool result = true;
            foreach(var (type, dataList) in data)
            {
                IMapHandler handler = AddHandler(type);
                result &= handler.Init(dataList);
            }
            return result;
        }

        // 全部初始化
        public bool Init()
        {
            foreach(var handler in handlers.Values)
            {
                handler.Init(null);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns> true if any handler is invoked successfully.</returns>
        public bool Invoke<TMapHandler>(object key, EventArgs args) where TMapHandler : IMapHandler
        {
            if(handlers.TryGetValue(typeof(TMapHandler), out IMapHandler? handler))
            {
                return handler.Invoke(key, args);
            }
            return false;
        }

        public bool Invoke<TArgs>(object key, TArgs args) where TArgs : EventArgs
        {
            if(ArgsToHandler.TryGetValue(typeof(TArgs), out IMapHandler? handler))
            {
                return handler.Invoke(key, args);
            }
            return false;
        }

        public void RegisterEvent<TArgs>(EventId eventId, Action<TArgs>? action = null) where TArgs:EventArgs
        {
            if (ArgsToHandler.TryGetValue(typeof(TArgs), out IMapHandler? handler))
            {
                handler.RegisterEvent(eventId, e => action?.Invoke((TArgs)e));
            }
        }

    }
}
