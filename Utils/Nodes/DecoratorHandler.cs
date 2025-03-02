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

        protected override IList<object?> Handle(IList<object?> input)
        {
            if (input.Count != 1)
            {
                throw new ArgumentException("DecoratorHandler can only handle one input");
            }

            return [.. input.Cast<TInput>().Select(x => (object?)Decorate(x)).Cast<object?>()];
        }

        protected abstract TOutput? Decorate(TInput input);
    }
}
