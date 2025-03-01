using DirN.Utils.Events.EventType;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace DirN.Utils
{
    public class MainManager : ManagerBase<MainManager>
    {
        public MainManager(IContainerProvider containerProvider) : base(containerProvider)
        {
            eventAggregator.GetEvent<MainEvent.WindowClosedEvent>().Subscribe(OnWindowClosed);
        }

        public event Action<CancelEventArgs>? WindowClosed;

        public void PrintHi()
        {
            Debug.WriteLine("Hi from MainManager");
        }

        private void OnWindowClosed(CancelEventArgs args)
        {
            WindowClosed?.Invoke(args);
        }
    }
}
