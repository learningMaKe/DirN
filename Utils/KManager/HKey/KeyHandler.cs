using DirN.Utils.Extension;
using DirN.Utils.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DirN.Utils.KManager.HKey
{
    public struct KeyMap(KeyState state, Key key, ModifierKeys modifiers=ModifierKeys.None)
    {
        public KeyState KeyState = state;
        public Key Key = key;
        public ModifierKeys Modifiers = modifiers;
    }

    public class KeyHandler : CreatorHandler<DefaultKeyMap,KeyMap,KeyEventArgs>
    {

    } 
}
