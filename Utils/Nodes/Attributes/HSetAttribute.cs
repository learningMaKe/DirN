using DirN.Utils.Attirubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class HSetAttribute(Type type):Attribute
    {
        public Type HType = type;
    }
}
