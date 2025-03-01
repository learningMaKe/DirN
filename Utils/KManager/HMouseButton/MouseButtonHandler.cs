using DirN.Utils.KManager.HKey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DirN.Utils.KManager.HMouseButton
{
    public struct MouseButtonMap(MouseButtonState state, MouseButton button)
    {
        public MouseButtonState State = state;
        public MouseButton Button = button;
    }

    public class MouseButtonHandler : CreatorHandler<DefaultMouseButtonMap, MouseButtonMap, MouseButtonEventArgs>
    {

    }
}
