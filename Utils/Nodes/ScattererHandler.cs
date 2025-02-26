using DirN.Utils.Nodes.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes
{
    public abstract class ScattererHandler<TInput, TOutput> : TypedHandler where TOutput : class, ITuple where TInput : class
    {
        protected override Type[] InputTypes => [typeof(TInput)];

        protected override Type[] OutputTypes => typeof(TOutput).GetGenericArguments();

        protected override IList<object?> Handle(IList<object?> input)
        {
            if (input.FirstOrDefault() is not TInput inputValue)
            {
                throw new ArgumentException("Input value is not of type " + typeof(TInput).Name);
            }

            TOutput output = Scatter(inputValue);
            return GenericBuilder.UnpackTuple<TOutput>(output);

        }

        protected abstract TOutput Scatter(TInput input);
    }
}
