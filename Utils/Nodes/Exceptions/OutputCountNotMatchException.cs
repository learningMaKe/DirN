using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Exceptions
{
    public class OutputCountNotMatchException:Exception,INodeException
    {
        public INode ErrorNode { get; private set; }

        public OutputCountNotMatchException(INode node, string message):base(message)
        {
            ErrorNode = node;
        }

        public static void ThrowIf(INode TestNode, int now,int shouldBe)
        {
            if (now!= shouldBe)
            {
                throw new OutputCountNotMatchException(TestNode, $"Output count not match, now is {now}, should be {shouldBe}");
            }
        }
    }
}
