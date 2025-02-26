using DirN.Utils.Nodes.Achieved;
using DirN.Utils.Nodes.Maps;
using DirN.Utils.Validation;
using DirN.ViewModels.Node;
using DirN.ViewModels.PointerControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace DirN.Utils.Nodes
{
    public class HandlerManager
    {
        private static readonly Dictionary<HandlerType, HandlerConfig> handlers = HandlerMap.Create();

        private static readonly Dictionary<Type, PointerConfig> pointerHandlers = PointerMap.Create();

        private static readonly Dictionary<PointerControlType, Func<UserControl>> pointerControls = PointerControlMap.Create();

        public static bool GetHandler(INode parent, HandlerType type,out INodeHandler? handler)
        {
            if (handlers.TryGetValue(type, out HandlerConfig? value))
            {
                if (value is not null)
                {
                    handler = value.Create!(parent);
                    return true;
                }
            }
            handler = null;
            return false;
        }

        public static bool GetHandlerConfig(HandlerType type, out HandlerConfig? handler)
        {
            if (handlers.TryGetValue(type, out HandlerConfig? value))
            {
                if (value is not null)
                {
                    handler = value;
                    return true;
                }
            }
            handler = null;
            return false;
        }


        public static bool GetPointer(Type type, out PointerConfig? handler)
        {
            if (pointerHandlers.TryGetValue(type, out PointerConfig? value))
            {
                if (value is not null)
                {
                    handler = value;
                    return true;
                }
            }
            handler = null;
            return false;
        }

        public static bool GetPointerControl(PointerControlType type, out UserControl? control)
        {
            if (pointerControls.TryGetValue(type, out Func<UserControl>? value))
            {
                if (value is not null)
                {
                    control = value();
                    return true;
                }
            }
            control = null;
            return false;
        }
    }
}
