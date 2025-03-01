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

namespace DirN.Utils.Nodes.Maps
{
    public class PointerControlMap:IMapCreator<PCT, Func<UserControl>>
    {
        public void Create(Dictionary<PCT, Func<UserControl>> source)
        {
            source.
                Set(PCT.PString, FA.Create<PString, PStringViewModel>).
                Set(PCT.PInt, FA.Create<PInt, PIntViewModel>).
                Set(PCT.PAny, FA.Create<PAny, PAnyViewModel>).
                Set(PCT.PFileInfo, FA.Create<PFileInfo, PFileInfoViewModel>).
                Set(PCT.PFilterSelection, FA.Create<PFilterSelection, PFilterSelectionViewModel>);
            
        }

    }
}
