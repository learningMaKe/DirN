using DirN.Utils.Extension;
using DirN.Utils.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DirN.Utils.KManager.HMouseButton
{
    public class DefaultMouseButtonMap : IKeyCreator<MouseButtonMap>
    {
        public void Create(Dictionary<EventId, MouseButtonMap> source)
        {
            source.Set(EventId.Mouse_Middle_Pressed, new(MouseButtonState.Pressed, MouseButton.Middle));
            source.Set(EventId.Mouse_Middle_Released, new(MouseButtonState.Released, MouseButton.Middle));
            source.Set(EventId.Mouse_Left_Pressed, new(MouseButtonState.Pressed, MouseButton.Left));
            source.Set(EventId.Mouse_Left_Released, new(MouseButtonState.Released, MouseButton.Left));
            source.Set(EventId.Mouse_Right_Pressed, new(MouseButtonState.Pressed, MouseButton.Right));
            source.Set(EventId.Mouse_Right_Released, new(MouseButtonState.Released, MouseButton.Right));
        }
    }
}
