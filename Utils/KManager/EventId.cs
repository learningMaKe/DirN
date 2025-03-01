using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.KManager
{
    public enum EventId:int
    {
        V_Previewer,
        V_StoredWord,
        Node_Enlarge,
        Node_Shrink,
        Graphics_Move,
        Mouse_Middle_Pressed,
        Mouse_Middle_Released,
        Mouse_Left_Pressed,
        Mouse_Left_Released,
        Mouse_Right_Pressed,
        Mouse_Right_Released,
        Node_Focus,
        Node_SelectAll,
        Node_DeleteSelected,
    }
}
