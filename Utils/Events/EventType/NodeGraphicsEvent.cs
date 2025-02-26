using DirN.Utils.NgManager.Curves;
using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace DirN.Utils.Events.EventType
{
    public class NodeGraphicsEvent
    {
        public class StoredWordVisibilityEvent : PubSubEvent<bool> { }

        public class AddToCanvasEvent : PubSubEvent<NodeGraphicsArgs.AddToCanvasArgs> { }

        public class MakeLinkEvent : PubSubEvent<NodeGraphicsArgs.LinkArgs> { }

        public class GetCanvasRelativePointEvent : PubSubEvent<NodeGraphicsArgs.GetCanvasRelativePointArgs> { }
    
        public class NodeExecutionEvent: PubSubEvent<NodeGraphicsArgs.NodeExecutionArgs> { }

        public class MousePositionEvent : PubSubEvent<NodeGraphicsArgs.MousePositionArgs> { }
    }
}
