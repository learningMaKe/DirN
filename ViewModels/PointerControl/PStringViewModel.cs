using DirN.ViewModels.Node;
using DirN.Views.PointerControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.ViewModels.PointerControl
{
    public class PStringViewModel:PViewModel<PString, PStringViewModel>
    {
        public string Name { get; set; } = "O,World";

        public override string? GetData()
        {
            return Name;
        }
    }
}
