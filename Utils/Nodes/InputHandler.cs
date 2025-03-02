using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes
{
    public abstract class InputHandler<TInput> : TypedHandler
    {
        protected override Type[] InputTypes => [typeof(TInput)];

        protected override Type[] OutputTypes => [];

        protected override IList<object?> Handle(IList<object?> input)
        {
            TInput? inputValue = (TInput?)input[0] ?? throw new ArgumentException("Input value is null");
            Input(inputValue);
            return [];
        }

        protected abstract void Input(TInput input);
    }
}
