using DryIoc;
using Newtonsoft.Json;
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
    public interface IMapHandler
    {
        public void RegisterEvent(EventId eventId, Action<EventArgs>? action = null);

        public void ChangeMap(EventId eventId, object newKey);

        public bool Init(IList<(EventId, object)>? keyMap);

        public bool Invoke(object key, EventArgs args);

        public IList<(EventId, object)> Export();
    }

    public interface IMapHandler<TKey, TArgs> where TKey : notnull where TArgs : EventArgs
    {
        public void RegisterEvent(EventId eventId, Action<TArgs>? action = null);

        public void ChangeMap(EventId eventId, TKey newKey);

        public bool Init(IList<(EventId, TKey)>? keyMap);

        public bool Invoke(TKey key, TArgs args);

        public IList<(EventId, TKey)> Export();
    }
}
