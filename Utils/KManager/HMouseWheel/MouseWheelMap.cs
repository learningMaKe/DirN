using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.KManager.HMouseWheel
{
    public enum MoveDirection
    {
        Up,
        Down
    }

    public struct MouseWheelMap(MoveDirection Direction)
    {
        public MoveDirection Direction = Direction;
    }
}
