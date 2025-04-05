using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class HPCSetAttribute(Type view, Type viewmodel) : Attribute
    {
        public Type View { get; set; } = view;
        public Type ViewModel { get; set; } = viewmodel;
    }
}
