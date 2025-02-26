using DirN.Utils.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DirN.Utils.NgManager
{
    public static class MenuItemInfoFactory
    {
        public static MenuItemInfo? Create(HandlerType handlerType,ICommand? command=default)
        {
            
            if(HandlerManager.GetHandlerConfig(handlerType,out var handlerConfig))
            {
                MenuItemInfo menuItemInfo = new()
                {
                    Header = handlerConfig!.Header,
                    Command = command,
                    CommandParameter = handlerType
                };
                return menuItemInfo;
            }
            return null;
        }
    }
}
