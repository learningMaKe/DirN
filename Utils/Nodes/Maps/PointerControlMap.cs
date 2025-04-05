using DirN.Utils.Extension;
using DirN.Utils.Maps;
using DirN.ViewModels.PointerControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using PCT = DirN.ViewModels.PointerControl.PointerControlType;
using FA = DirN.ViewModels.PointerControl.PointerControlFactory;
using DirN.Views.PointerControl;
using System.Reflection;
using DirN.Utils.Nodes.Attributes;

namespace DirN.Utils.Nodes.Maps
{
    public class PointerControlMap:IMapCreator<PCT, Func<UserControl>>
    {
        public void Create(Dictionary<PCT, Func<UserControl>> source)
        {
            FieldInfo[] fields = typeof(PCT).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                var value = field.GetValue(null);
                if (value == null) continue;
                PCT key = (PCT)value;
                HPCSetAttribute? hPC=field.GetCustomAttribute<HPCSetAttribute>();
                if (hPC == null) continue;
                UserControl func() => FA.Create(hPC.View, hPC.ViewModel);
                source.Set(key, func);
            }

            /*
            source.
                Set(PCT.PString, FA.Create<PString, PStringViewModel>).
                Set(PCT.PInt, FA.Create<PInt, PIntViewModel>).
                Set(PCT.PAny, FA.Create<PAny, PAnyViewModel>).
                Set(PCT.PFileInfo, FA.Create<PFileInfo, PFileInfoViewModel>).
                Set(PCT.PEnum, FA.Create<PEnum, PEnumViewModel>).
                Set(PCT.PBool, FA.Create<PBool, PBoolViewModel>);
            */
            
        }

    }
}
