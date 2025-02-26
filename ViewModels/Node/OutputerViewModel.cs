using DirN.Utils.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.ViewModels.Node
{
    public class OutputerViewModel : PointerViewModel
    {
        public override IConnector Connector => OutputConnector;

        public IConnector OutputConnector { get; private set; }

        public OutputerViewModel(INode Parent) : base(Parent)
        {
            OutputConnector = new OutputConnectorViewModel(this);
        }


        public override void UpdateLink()
        {
            OutputConnector.UpdateLink();
        }

        public override void CutLink()
        {
            OutputConnector.CutLink();
        }

        protected override void SetData(object? data)
        {
            if (PointerConfig?.Validate?.Invoke(data) == false) throw new ArgumentException("校验失败");

            OutputConnector.Data = data;
        }
    }
}
