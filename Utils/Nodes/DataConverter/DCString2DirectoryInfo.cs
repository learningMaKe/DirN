using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.DataConverter
{
    public class DCString2DirectoryInfo : IDataConverter
    {
        public object? Convert(object data)
        {
            if (data is string str)
            {
                return new System.IO.DirectoryInfo(str);
            }
            else
            {
                return null;
            }
        }
    }
}
