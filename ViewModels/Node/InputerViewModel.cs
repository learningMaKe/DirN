using DirN.Utils.Nodes;
using DirN.ViewModels.PointerControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.ViewModels.Node
{
    public class InputerViewModel : PointerViewModel
    {
        public override IConnector Connector => InputConnector;

        public bool AcceptExternal { get; set; } = false;

        public bool UseConnector { get; set; } = true;

        public IConnector InputConnector { get; private set; }

        public IPContainer PointerContainer { get; private set; }

        public InputerViewModel(INode Parent) : base(Parent)
        {
            InputConnector = new InputConnectorViewModel(this);
            PointerContainer = new PContainerViewModel(this);
            InputConnector.ConnectorStateUpdated += OnConnectorStateUpdated;
        }

        public override void UpdateLink()
        {
            InputConnector.UpdateLink();
        }

        public override void CutLink()
        {
            InputConnector.CutLink();
        }

        protected override void PointerConfigChanged(PointerConfig config)
        {
            UseConnector = config.UseConnector;
        }

        protected override object? GetData()
        {
            object? data = AcceptExternal ? InputConnector.Data : PointerContainer.Data;
            if (PointerConfig is null) return data;

            if (PointerConfig.Validate is null) return data;

            bool isValid = PointerConfig.Validate(data);

            if (isValid) return data;

            throw new ArgumentException("校验失败");
        }

        protected override void SetData(object? data)
        {
            if (data is null) return;
            if (AcceptExternal)
            {
                InputConnector.Data = data;
            }
            else
            {
                PointerContainer.Data = data;
            }
        }

        private void OnConnectorStateUpdated(bool state)
        {
            AcceptExternal = state;
        }
    }
}
