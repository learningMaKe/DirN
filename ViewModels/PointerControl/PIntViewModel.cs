using DirN.Utils.Nodes.Datas;
using DirN.Views.PointerControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.ViewModels.PointerControl
{
    public class PIntViewModel:PViewModel
    {
        public override DataContainer GetData()
        {
            return new("O,Hello");
        }

    }
}
