using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DirN.Utils.KManager
{
    public class MapDictionary<TKey,TArgs> where TKey:notnull
    {
        protected static readonly Dictionary<EventId, Action<TArgs>?> EventIdToAction = [];
        protected readonly Dictionary<EventId, TKey> EventIdToKey = [];
        protected readonly Dictionary<TKey, EventId> KeyToEventId = [];

        public void RegisterEvent(EventId eventId, Action<TArgs>? action)
        {
            if (EventIdToAction.ContainsKey(eventId))
            {
                throw new InvalidOperationException("One Id can only mapping one event, please check your code.");
            }
            EventIdToAction.TryAdd(eventId, action);
        }

        public void SetEventId(EventId eventId, TKey newKey)
        {
            RemoveId(eventId);
            Add(newKey, eventId);
        }

        public void SetEventId(params (EventId eventId, TKey newKey)[] pairs)
        {
            foreach (var (eventId, newKey) in pairs)
            {
                SetEventId(eventId, newKey);
            }
        }

        public bool GetAction(EventId eventId, out Action<TArgs>? action)
        {
            if (EventIdToAction.TryGetValue(eventId, out action))
            {
                return true;
            }
            return false;
        }

        public bool GetAction(TKey key, out Action<TArgs>? action)
        {
            action = null;
            if (KeyToEventId.TryGetValue(key, out EventId eventId))
            {
                return GetAction(eventId, out action);
            }
            return false;
        }

        public IList<(EventId, TKey)> Export()
        {
            return EventIdToKey.Select(pair => (pair.Key, pair.Value)).ToList();
        }

        #region Private Methods

        protected void Add(TKey key, EventId eventId)
        {
            if (KeyToEventId.ContainsKey(key))
            {
                throw new InvalidOperationException("One key can only mapping one event, please check your code.");
            }
            EventIdToKey.TryAdd(eventId, key);
            KeyToEventId.TryAdd(key, eventId);
        }

        protected void RemoveId(EventId eventId)
        {
            if (EventIdToKey.TryGetValue(eventId, out TKey? key))
            {
                EventIdToKey.Remove(eventId);
                KeyToEventId.Remove(key);
            }
        }

        #endregion
    }
}
