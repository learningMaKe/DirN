using DirN.Utils.Nodes;
using DirN.Utils.Nodes.Datas;
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

        /// <summary>
        /// 输出节点不存储数据，因此此处不实现
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected override DataContainer GetData()
        {
            return Connector.Data;
        }

        protected override void SetData(DataContainer data)
        {
            if (PointerConfig?.Validate?.Invoke(data) == false) throw new ArgumentException("校验失败");

            OutputConnector.Data = data;
        }
    }
}
