using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Attirubutes
{
    [AttributeUsage(AttributeTargets.All)]
    public class DesAttribute(string des):Attribute
    {
        public string Description { get; set; } = des;
    }
}
