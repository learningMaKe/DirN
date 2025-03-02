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
    [HDes("连接", "ForestGreen")]
    public class HJoin : AggregatorHandler<Tuple<string,string>,string>
    {
        protected override string Aggregate(Tuple<string, string> input)
        {
            return input.Item1 + input.Item2;
        }
    }
}
