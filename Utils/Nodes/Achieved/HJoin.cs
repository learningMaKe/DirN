using DirN.Utils.Nodes.Attributes;
using DirN.Utils.Nodes.Bulider;
using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DirN.Utils.Nodes.Achieved
{
    [HDes("连接")]
    public class HJoin : AggregatorHandler<Tuple<string,string>,string>
    {

        public override void Init(INode parent)
        {
            Header = "Join";
            MainColor = Colors.ForestGreen;
        }

        protected override string Aggregate(Tuple<string, string> input)
        {
            return input.Item1 + input.Item2;
        }
    }
}
