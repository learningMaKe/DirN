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

        protected override IList<object?> Handle(IList<object?> input)
        {
            return [Output()];
        }

        protected abstract TOutput Output();
    }
}
