using DirN.Views.PointerControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.ViewModels.PointerControl
{
    public class PAnyViewModel : PViewModel<PAny, PAnyViewModel>
    {
        protected override void Init()
        {

        }

        public override object? GetData()
        {
            return "No Data";
        }
    }
}
