using DirN.Utils.Extension;
using DirN.ViewModels.PointerControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DirN.Utils.Nodes.Maps
{
    public static class PointerControlMap
    {
        public static Dictionary<PointerControlType, Func<UserControl>> Create()
        {
            Dictionary<PointerControlType, Func<UserControl>> pointerControlMap = [];

            pointerControlMap.
                Set(PointerControlType.PString, PStringViewModel.Create).
                Set(PointerControlType.PInt, PIntViewModel.Create).
                Set(PointerControlType.PAny, PAnyViewModel.Create);
            
            return pointerControlMap;

        }

    }
}
