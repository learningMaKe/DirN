using DirN.Utils.Nodes.Attributes;
using DirN.Utils.Nodes.Datas;
using DirN.Utils.Tooltips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Achieved
{
    [HDes("文本预览","#003399")]
    public class HPreview : TypedHandler
    {
        protected override Type[] InputTypes => [typeof(string)];

        protected override Type[] OutputTypes => [];

        protected override IList<DataContainer> Handle(IList<DataContainer> input)
        {
            if (input.Count == 1 && input[0].GetData<string>() is string str)
            {
                TooltipManager.Instance.Tooltip(this.Parent!, str);
            }
            return [];
        }
    }
}
