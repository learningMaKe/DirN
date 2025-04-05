using DirN.Utils.Nodes.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes
{
    public abstract class DecoratorHandler<TInput, TOutput> : TypedHandler
    {
        protected override Type[] InputTypes => [typeof(TInput)];

        protected override Type[] OutputTypes => [typeof(TOutput)];

        protected override IList<DataContainer> Handle(IList<DataContainer> input)
        {
            if (input.Count != 1)
            {
                throw new ArgumentException("DecoratorHandler can only handle one input");
            }
            TInput data = input[0].GetData<TInput>();
            if (data == null)
            {
                return [new DataContainer(null)];
            }
            return [new DataContainer(Decorate(data))];
        }

        protected abstract TOutput? Decorate(TInput input);
    }
}
