using DirN.Utils.NgManager.Curves;
using DirN.Utils.Nodes.Attributes;
using DirN.Utils.Nodes.Bulider;
using DirN.Utils.Nodes.Datas;
using DirN.Utils.Nodes.Exceptions;
using DirN.Utils.Tooltips;
using DirN.Utils.WithColors;
using DirN.ViewModels.Node;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace DirN.Utils.Nodes
{
    public abstract class BaseHandler:BindableBase,INodeHandler
    {
        #region Properties

        public INode? Parent { get; init; }

        public ObservableCollection<IPointer> InputGroup { get; set; } = [];
        public ObservableCollection<IPointer> OutputGroup { get; set; } = [];
        
        public IList<ICurve> OutputCurve=> [.. OutputGroup.SelectMany(x=>x.Connector.LinkedCurves).Distinct()];
        public IList<ICurve> InputCurve => [.. InputGroup.SelectMany(x => x.Connector.LinkedCurves).Distinct()];

        public string Header { get; set; } = "Undefined";

        public string Description { get; set; } = "Undefined";

        public WithColor Colorer { get; } = new WithColor();

        public IList<INode> Predecessors
        {
            get
            {
                IList<INode> predecessors = [];
                foreach (var input in InputGroup)
                {
                    IList<ICurve> linkedCurves = input.Connector.LinkedCurves;
                    if (linkedCurves.Count == 0) continue;
                    INode? predecessor = linkedCurves.Select(x => x.StartNode).FirstOrDefault();
                    if (predecessor is null) continue;
                    predecessors.Add(predecessor);
                }
                return predecessors;
            }
        }

        #endregion

        #region Constructor

        protected BaseHandler()
        {
            Type type = GetType();
            HDesAttribute? hdes = type.GetCustomAttributes(typeof(HDesAttribute), true).FirstOrDefault() as HDesAttribute;
            if (hdes is not null)
            {
                Header = hdes.Header;
                Description = hdes.Description;
                Colorer.MainColor = hdes.MainColor;
            }
        }

        #endregion

        #region Public Methods

        public abstract void Init(INode parent);

        public abstract bool DataFlow();

        public void UpdateLink()
        {
            foreach (var pointer in InputGroup)
            {
                pointer.UpdateLink();
            }
            foreach (var pointer in OutputGroup)
            {
                pointer.UpdateLink();
            }
        }

        public void CutLink()
        {
            foreach (var link in InputGroup)
            {
                link.CutLink();
            }
            foreach (var link in OutputGroup)
            {
                link.CutLink();
            }
        }

        #endregion
    }
}
