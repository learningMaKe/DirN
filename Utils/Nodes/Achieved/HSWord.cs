using DirN.Utils.Nodes.Attributes;
using DirN.Utils.Nodes.Datas;
using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Achieved
{
    [HDes("保留字")]
    public class HSWord : TypedHandler
    {
        protected override Type[] InputTypes => [];

        protected override Type[] OutputTypes => [typeof(string)];

        protected override IList<DataContainer> Handle(IList<DataContainer> input)
        {
            throw new NotImplementedException();
        }
    }
}
