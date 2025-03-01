using DirN.Utils;
using DirN.Utils.CommandLine;
using DirN.Utils.Events.EventType;
using DirN.Utils.KManager;
using DirN.Utils.KManager.HKey;
using DirN.Utils.KManager.HMouseWheel;
using Fclp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DirN.ViewModels
{
    public class MainWindowViewModel:BaseViewModel
    {
        public string Title { get; private set; } = "文件管理";

        public string Icon { get; } = "pack://application:,,,/DirN;component/Resources/Images/folder.ico";

        public DelegateCommand<CancelEventArgs> CloseCommand { get;private set; }
        public DelegateCommand<KeyEventArgs> KeyDownCommand { get; private set; }
        public DelegateCommand<KeyEventArgs> KeyUpCommand { get; private set; }
        public DelegateCommand<MouseWheelEventArgs> MouseWheelCommand { get; private set; }
        public DelegateCommand<MouseEventArgs> MouseMoveCommand { get; private set; }
        public DelegateCommand<MouseButtonEventArgs> MouseDownCommand { get; private set; }
        public DelegateCommand<MouseButtonEventArgs> MouseUpCommand { get; private set; }

        public MainWindowViewModel(IContainerProvider containerProvider):base(containerProvider)
        {
            CloseCommand = new(Close);
            KeyDownCommand = new(KeyDown);
            KeyUpCommand = new(KeyUp);
            MouseWheelCommand = new(MouseWheel);
            MouseMoveCommand = new(MouseMove);
            MouseDownCommand = new(MouseDown);
            MouseUpCommand = new(MouseUp);
        }

        private void Close(CancelEventArgs args)
        {
            EventAggregator.GetEvent<MainEvent.WindowClosedEvent>().Publish(args);
        }

        private void KeyDown(KeyEventArgs args)
        {
            KeyManager.Instance.InvokeKeyEvent(args, KeyState.Down);
        }

        private void KeyUp(KeyEventArgs args)
        {
            KeyManager.Instance.InvokeKeyEvent(args, KeyState.Up);
        }

        private void MouseWheel(MouseWheelEventArgs args)
        {
            KeyManager.Instance.InvokeMouseWheelEvent(args);
        }

        private void MouseMove(MouseEventArgs args)
        {
            KeyManager.Instance.InvokeMouseMoveEvent(args);
        }

        private void MouseDown(MouseButtonEventArgs args)
        {
            KeyManager.Instance.InvokeMouseButtonEvent(args);
        }

        private void MouseUp(MouseButtonEventArgs args)
        {
            KeyManager.Instance.InvokeMouseButtonEvent(args);
        }
    }
}
