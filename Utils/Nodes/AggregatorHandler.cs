using DirN.Utils.Nodes.Datas;
using DirN.Utils.Nodes.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes
{
    public abstract class AggregatorHandler<TInput, TOutput> : TypedHandler where TInput : ITuple
    {
        protected override Type[] InputTypes => typeof(TInput).GetGenericArguments();

        protected override Type[] OutputTypes => [typeof(TOutput)];

        protected override IList<DataContainer> Handle(IList<DataContainer> input)
        {
            TInput inputTuple = GenericBuilder.MakeTuple<TInput>([.. input]) ?? throw new ArgumentException("Input is not of type TInput");
            TOutput output = Aggregate(inputTuple);
            return [new DataContainer(output)];

        }

        protected abstract TOutput Aggregate(TInput input);

    }
}
