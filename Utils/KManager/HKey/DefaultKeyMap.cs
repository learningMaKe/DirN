using DirN.Utils.Extension;
using DirN.Utils.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DirN.Utils.KManager.HKey
{
    public class DefaultKeyMap : IKeyCreator<KeyMap>
    {
        public void Create(Dictionary<EventId, KeyMap> source)
        {
            source.
                Set(EventId.V_StoredWord,
                new(KeyState.Down, Key.T)).

                Set(EventId.V_Previewer, 
                new(KeyState.Down, Key.Tab)).

                Set(EventId.Node_Focus, 
                new(KeyState.Down, Key.F)).

                Set(EventId.Node_SelectAll, 
                new(KeyState.Down, Key.A)).

                Set(EventId.Node_DeleteSelected, 
                new(KeyState.Down, Key.Delete));
        }
    }
}
