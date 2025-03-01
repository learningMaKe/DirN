using DirN.Utils.KManager.HKey;
using DirN.Utils.KManager.HMouse;
using DirN.Utils.KManager.HMouseButton;
using DirN.Utils.KManager.HMouseWheel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DirN.Utils.KManager
{
    public static class KeyManagerExtension
    {
        #region ChangeMap

        public static void ChangeKey(this KeyManager keyManager, EventId eventId, KeyState state, Key newKey,ModifierKeys modifier=ModifierKeys.None)
        {
            keyManager.ChangeMap(eventId, new KeyMap(state, newKey, modifier));
        }

        #endregion

        #region InvokeElementEvent

        public static void  InvokeKeyEvent(this KeyManager keyManager,KeyEventArgs e, KeyState state)
        {
            keyManager.InvokeElementEvent(e, new KeyMap(state, e.Key, Keyboard.Modifiers));
        }

        public static void InvokeMouseWheelEvent(this KeyManager keyManager, MouseWheelEventArgs e)
        {
            MoveDirection direction = e.Delta > 0? MoveDirection.Up : MoveDirection.Down;
            keyManager.InvokeElementEvent(e, new MouseWheelMap(direction));
        }

        public static void InvokeMouseButtonEvent(this KeyManager keyManager, MouseButtonEventArgs e)
        {
            keyManager.InvokeElementEvent(e, new MouseButtonMap(e.ButtonState, e.ChangedButton));
        }

        public static void InvokeMouseMoveEvent(this KeyManager keyManager, MouseEventArgs e)
        {

            keyManager.InvokeElementEvent(e, new MouseMap()
            {
                Middle = e.MiddleButton,
                Right = e.RightButton,
                Left = e.LeftButton
            });
        }
        #endregion
    }
}
