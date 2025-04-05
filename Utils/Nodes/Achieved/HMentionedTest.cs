using DirN.Utils.Nodes.Attributes;
using DirN.ViewModels.PointerControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Achieved
{
    [HDes("测试节点", "测试节点的描述")]
    public class HMentionedTest:MentionedHandler
    {
        [HPIDes(0,"测试语句", "测试语句的描述")]
        public string TestWord { get; set; } = string.Empty;

        [HPIDes(1,"测试属性", "测试属性的描述",true,PointerControlType.PString,"#f00")]
        public string TestProperty { get; set; } = string.Empty;

        [HPODes(0, "输出", "输出的描述", "#0f0")]
        public string Output { get; set; } = string.Empty;

        protected override void Handle()
        {
            Output = TestWord + TestProperty;
        }
    }
}
