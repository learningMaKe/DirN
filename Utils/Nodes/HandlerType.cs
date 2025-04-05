using DirN.Utils.Nodes.Achieved;
using DirN.Utils.Nodes.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes
{
    public enum HandlerType
    {
        None,

        [HSet(typeof(HInput))]
        Input,

        [HSet(typeof(HOutput))]
        Output,

        [HSet(typeof(HDebug))]
        Debug,

        [HSet(typeof(HJoin))]
        Join,

        [HSet(typeof(HSWord))]
        SWord,

        [HSet(typeof(HFileFilter))]
        FileFilter,

        [HSet(typeof(HPreview))]
        Preview,

        [HSet(typeof(HMentionedTest))]
        MentionedTest
    }
}
