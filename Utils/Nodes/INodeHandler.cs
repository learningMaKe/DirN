using DirN.Utils.Base;
using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DirN.Utils.Nodes
{
    public interface INodeHandler:INodePasser
    {
        public void Init(INode parent);

        public INode? Parent { get; init; }

        public string Header { get; set; }

        public Color MainColor { get; set; }

        public Brush HeaderBrush { get; }

        public Color HeaderEffectColor { get; }

        public IList<object?>? GetOutput();

        /// <summary>
        /// 数据经由该节点流向下一个节点
        /// </summary>
        public void DataFlow();

        public ObservableCollection<IPointer> InputGroup { get; set; }
        public ObservableCollection<IPointer> OutputGroup { get; set; }

    }
}
