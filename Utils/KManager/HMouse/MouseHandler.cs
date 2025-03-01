using DirN.Utils.KManager.HMouseButton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DirN.Utils.KManager.HMouse
{
    public struct MouseMap
    {
        public MouseButtonState Middle;
        public MouseButtonState Left;
        public MouseButtonState Right;
    }

    public class MouseHandler: CreatorHandler<DefaultMouseMap, MouseMap, MouseEventArgs>
    {

    }
}
