using DirN.Utils.Events.EventType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DirN.Utils.KManager
{
    public enum KeyState 
    {
        Down,
        Up
    }


    public class KeyManager : ManagerBase<KeyManager>
    {
        private readonly Dictionary<KeyState, Dictionary<Key, Action<KeyEventArgs>?>> KeyBindings = [];

        private UIElement? keyReceiver;

        public KeyManager(IContainerProvider containerProvider) : base(containerProvider)
        {
            Initilize();

            eventAggregator.GetEvent<KeyEvent.KeyDownEvent>().Subscribe(e => KeyEnter(KeyState.Down, e));
            eventAggregator.GetEvent<KeyEvent.KeyUpEvent>().Subscribe(e => KeyEnter(KeyState.Up, e));
        }

        private void Initilize()
        {
            foreach (var state in Enum.GetValues(typeof(KeyState)))
            {
                KeyBindings[(KeyState)state] = [];
                foreach (var key in Enum.GetValues(typeof(Key)))
                {
                    KeyBindings[(KeyState)state][(Key)key] = null;
                }
            }
        }

        public void SetKeyReceiver(UIElement element)
        {
            if(keyReceiver!= null)
            {
                keyReceiver.KeyDown -= KeyDown;
                keyReceiver.KeyUp -= KeyUp;
            }
            keyReceiver = element;
            keyReceiver.KeyDown += KeyDown;
            keyReceiver.KeyUp += KeyUp;
            keyReceiver.Focus();
        }

        public void RegisterKeyBinding(KeyState state, Key key, Action<KeyEventArgs> action)
        {
            KeyBindings[state][key] += action;
        }

        private void KeyDown(object sender, KeyEventArgs e)
        {
            KeyEnter(KeyState.Down, e);
        }

        private void KeyUp(object sender, KeyEventArgs e)
        {
            KeyEnter(KeyState.Up, e);
        }

        private void KeyEnter(KeyState state,KeyEventArgs e)
        {
            KeyBindings[state][e.Key]?.Invoke(e);
        }

    }
}
