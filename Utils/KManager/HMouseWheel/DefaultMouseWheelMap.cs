using DirN.Utils.Extension;
using DirN.Utils.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.KManager.HMouseWheel
{
    public class DefaultMouseWheelMap : IKeyCreator<MouseWheelMap>
    {
        public void Create(Dictionary<EventId, MouseWheelMap> source)
        {
            source.
                Set(EventId.Node_Enlarge, new(MoveDirection.Up)).
                Set(EventId.Node_Shrink, new(MoveDirection.Down));
        }
    }
}
