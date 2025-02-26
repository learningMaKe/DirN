using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Events.EventType
{
    public class PreviewerEvent
    {
        public class PreviewerVisibilityChangedEvent : PubSubEvent<bool> { }
        public class PreviewerShowEvent : PubSubEvent { }
        public class PreviewerHideEvent : PubSubEvent { }
    }
}
