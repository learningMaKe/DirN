using DirN.Utils.KManager.HKey;
using DirN.Utils.KManager.HMouse;
using DirN.Utils.KManager.HMouseButton;
using DirN.Utils.KManager.HMouseWheel;
using DirN.Utils.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.KManager
{
    public class DefaultMapManager : IMapCreator<MapHandlerGroup>
    {
        public void Create(MapHandlerGroup source)
        {
            source.
                AddHandler<KeyHandler>().
                AddHandler<MouseButtonHandler>().
                AddHandler<MouseHandler>().
                AddHandler<MouseWheelHandler>();
            source.Init();
        }
    }
}
