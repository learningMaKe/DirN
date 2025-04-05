using DirN.Utils.Nodes.Datas;
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

        protected override IList<DataContainer> Handle(IList<DataContainer> input)
        {
            TInput? inputValue = input[0].GetData<TInput>() ?? throw new ArgumentException("Input value is null");
            Input(inputValue);
            return [];
        }

        protected abstract void Input(TInput input);
    }
}
