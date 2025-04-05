using DirN.Utils.Nodes.Datas;
using DirN.Views.PointerControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.ViewModels.PointerControl
{
    public class PAnyViewModel : PViewModel
    {
        protected override void Init()
        {

        }

        public override DataContainer GetData()
        {
            return new DataContainer("No Data");
        }
    }
}
