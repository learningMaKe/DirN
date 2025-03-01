using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DirN.Utils.KManager
{
    public abstract class MapHandler<TKey,TArgs>:IMapHandler<TKey,TArgs>, IMapHandler where TKey:notnull where TArgs:EventArgs
    {
        protected abstract IList<(EventId,TKey)> DefaultList { get; }

        protected readonly MapDictionary<TKey, TArgs> mapDictionary = new();

        public IList<(EventId, TKey)> Export()
        {
            return mapDictionary.Export();
        }

        public void RegisterEvent(EventId eventId, Action<TArgs>? action = null)
        {
            mapDictionary.RegisterEvent(eventId, action);
        }

        public void ChangeMap(EventId eventId, TKey newKey)
        {
            mapDictionary.SetEventId(eventId, newKey);
        }

        public bool Init(IList<(EventId, TKey)>? keyMap)
        {
            bool res = true;
            if (keyMap == null)
            {
                res = false;
                keyMap = DefaultList;
            }
            mapDictionary.SetEventId([.. keyMap]);
            return res;
        }

        public bool Invoke(TKey key,TArgs args)
        {
            if(mapDictionary.GetAction(key,out var action))
            {
                action?.Invoke(args);
                return true;
            }
            return false;

        }

        void IMapHandler.RegisterEvent(EventId eventId, Action<EventArgs>? action)
        {
            RegisterEvent(eventId, action);
        }

        void IMapHandler.ChangeMap(EventId eventId, object newKey)
        {
            ChangeMap(eventId, (TKey)newKey);
        }

        bool IMapHandler.Init(IList<(EventId, object)>? keyMap)
        {
            return Init(keyMap?.Select(x => (x.Item1, ((JObject)x.Item2).ToObject<TKey>()!)).ToList());
        }

        bool IMapHandler.Invoke(object key, EventArgs args)
        {
            return Invoke((TKey)key, (TArgs)args);
        }

        IList<(EventId, object)> IMapHandler.Export()
        {
            return [.. Export().Select(x=> (x.Item1, (object)x.Item2))];
        }
    }
}
