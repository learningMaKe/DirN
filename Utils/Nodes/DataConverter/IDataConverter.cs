using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.DataConverter
{
    public interface IDataConverter
    {
        object? Convert(object data);
    }
}
