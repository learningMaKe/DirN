using DirN.Utils.Events.EventType;
using DirN.Utils.NgManager.Curves;
using DirN.Utils.NgManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.ViewModels.Node
{
    /// <summary>
    ///  输入连接器 只能有一个输入
    /// </summary>
    public class InputConnectorViewModel : ConnectorViewModel,IInputConnector
    {
        protected ICurve? InputCurve;

        public override IList<ICurve> LinkedCurves { 
            get 
            {
                if (InputCurve == null) return [];
                return [InputCurve];
            }
        } 

        public InputConnectorViewModel(PointerViewModel parent) : base(parent)
        {

        }

        public override object? GetData()
        {
            return InputCurve?.Data;
        }

        public override void AddLink(ICurve curve)
        {
            CutLink();
            InputCurve = curve;
            ConnectorStateUpdated?.Invoke(true);
        }

        public override void CutLink()
        {
            InputCurve?.Remove();
        }

        public override void RemoveLink(ICurve curve)
        {
            if (InputCurve == curve)
            {
                InputCurve = null;
                ConnectorStateUpdated?.Invoke(false);
            }
        }

        public override void UpdateLink()
        {
            InputCurve?.UpdateLink();
        }

        protected override void MakeLink()
        {

            BezierCurve bezierCurve = new()
            {
                Thickness = 2,
                Brush=InputCurve?.Brush??ConnectorBrush,
                Starter=InputCurve?.Starter??this,
            };
            InputCurve?.Remove();
            InputCurve = null;
            NodeGraphicsArgs.LinkArgs args = GetLinkArgs(bezierCurve);

            NodeGraphicsManager.Instance.MakeLink(args);

        }

        
    }
}
