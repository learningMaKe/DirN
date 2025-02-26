using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Achieved
{
    public class StoredWordHandler : TypedHandler
    {
        protected override Type[] InputTypes => [];

        protected override Type[] OutputTypes => [typeof(string)];

        protected override IList<object?> Handle(IList<object?> input)
        {
            throw new NotImplementedException();
        }
    }
}
