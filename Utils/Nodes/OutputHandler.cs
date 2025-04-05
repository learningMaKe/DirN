using DirN.Utils.Nodes.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes
{
    public abstract class OutputHandler<TOutput> : TypedHandler
    {
        protected override Type[] InputTypes => [];

        protected override Type[] OutputTypes => [typeof(TOutput)];

        protected override IList<DataContainer> Handle(IList<DataContainer> input)
        {
            return [new (Output())];
        }

        protected abstract TOutput Output();
    }
}
