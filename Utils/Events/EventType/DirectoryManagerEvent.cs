using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Events.EventType
{
    public class DirectoryManagerEvent
    {
        public class DirectoryChangedEvent : PubSubEvent<string> { }
    }
}
