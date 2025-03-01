using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DirN.Utils.KManager
{
    public static class KeyEventIdMehods
    {
        #region Methods
        private readonly static Dictionary<string, int> NameToId = [];

        static KeyEventIdMehods()
        {
            var type = typeof(EventId);
            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var field in fields)
            {
                int? value = (int?)field.GetValue(null);
                if (value is null) continue;
                NameToId.Add(field.Name, value.Value);
            }
        }

        public static int? GetId(string name)
        {
            if (NameToId.TryGetValue(name, out int value))
            {
                return value;
            }
            return null;
        }
        #endregion




    }
}
