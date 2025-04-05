using DirN.Utils.Nodes.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.ViewModels.PointerControl
{
    public class PBoolViewModel:PViewModel
    {
        public bool IsChecked { get; set; } = false;

        public override DataContainer GetData()
        {
            return new DataContainer(IsChecked);
        }
    }
}
