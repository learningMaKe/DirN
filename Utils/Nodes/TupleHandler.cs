using DirN.Utils.Nodes.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes
{
    public abstract class TupleHandler<TInput,TOutput>:TypedHandler where TInput:class, ITuple where TOutput:class, ITuple
    {
        public TupleHandler()
        {
            
        }

        protected override Type[] InputTypes => typeof(TInput).GetGenericArguments();
        protected override Type[] OutputTypes => typeof(TOutput).GetGenericArguments();

        protected override IList<object?> Handle(IList<object> input)
        {
            TInput inputTuple = GenericBuilder.MakeTuple<TInput>([.. input]) ?? throw new ArgumentException("Input is not of type TInput");
            TOutput outputTuple = HandleTuple(inputTuple);
            return GenericBuilder.UnpackTuple<TOutput>(outputTuple);
        }

        protected abstract TOutput HandleTuple(TInput input);
    }

}
