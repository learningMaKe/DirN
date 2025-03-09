using DirN.Utils.Events.EventType;
using DirN.Utils.NgManager.Curves;
using DirN.Utils.NgManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.ViewModels.Node
{
    /// <summary>
    /// 输出连接器能有多个输出
    /// </summary>
    public class OutputConnectorViewModel : ConnectorViewModel,IOutputConnector
    {
        protected ObservableCollection<ICurve> CurveGroup = [];

        public override IList<ICurve> LinkedCurves 
        {
            get => CurveGroup;
        } 

        public OutputConnectorViewModel(PointerViewModel parent) : base(parent)
        {

        }

        public override void SetData(object? data)
        {
            foreach (ICurve curve in CurveGroup)
            {
                curve.Data = data;
            }
        }
        // ToDo: 需要检查是否有重复的连接
        public override void AddLink(ICurve curve)
        {
            CurveGroup.Add(curve);
        }

        public override void CutLink()
        {
            while (CurveGroup.Count > 0)
            {
                CurveGroup.First().Remove();
            }
            CurveGroup.Clear();
        }

        public override void RemoveLink(ICurve curve)
        {
            CurveGroup.Remove(curve);
        }

        public override void UpdateLink()
        {
            foreach (ICurve curve in CurveGroup)
            {
                curve.UpdateLink();
            }
        }

        protected override void MakeLink()
        {
            BezierCurve curve = new()
            {
                Starter = this,
                Thickness = 2,
                Brush = ConnectorBrush
            };

            NodeGraphicsArgs.LinkArgs args = GetLinkArgs(curve);

            NodeGraphicsManager.Instance.MakeLink(args);
        }
    }
}
