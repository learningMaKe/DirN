using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace DirN.Utils.Events.EventType
{
    public class MainEvent
    {
        public class WindowClosedEvent : PubSubEvent<CancelEventArgs> { }
    }
}
