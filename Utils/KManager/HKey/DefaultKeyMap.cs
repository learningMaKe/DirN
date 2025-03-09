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
                Set(EventId.V_StoredWord,new(KeyState.Down, Key.T)).

                Set(EventId.V_Previewer,new(KeyState.Down, Key.Tab)).

                Set(EventId.Node_Focus,new(KeyState.Down, Key.F)).
                Set(EventId.Node_SelectAll,new(KeyState.Down, Key.A)).
                Set(EventId.Node_DeleteSelected,new(KeyState.Down, Key.Delete)).

                Set(EventId.Node_Align_Left, new(KeyState.Down, Key.Left)).
                Set(EventId.Node_Align_Right, new(KeyState.Down, Key.Right)).
                Set(EventId.Node_Align_Top, new(KeyState.Down, Key.Up)).
                Set(EventId.Node_Align_Bottom, new(KeyState.Down, Key.Down)).

                Set(EventId.Node_Copy, new(KeyState.Down, Key.C, ModifierKeys.Control)).
                Set(EventId.Node_Cut, new(KeyState.Down, Key.X, ModifierKeys.Control)).
                Set(EventId.Node_Paste, new(KeyState.Down, Key.V, ModifierKeys.Control));



        }
    }
}
