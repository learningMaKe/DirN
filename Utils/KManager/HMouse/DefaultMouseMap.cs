using DirN.Utils.Extension;
using DirN.Utils.KManager.HMouseButton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DirN.Utils.KManager.HMouse
{
    public class DefaultMouseMap : IKeyCreator<MouseMap>
    {
        public void Create(Dictionary<EventId, MouseMap> source)
        {
            source.
                Set(EventId.Mouse_Middle_Pressed, new() { Middle = MouseButtonState.Pressed });
        }
    }
}
