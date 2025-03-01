using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.DataConverter
{
    public class DCString2FileInfo : IDataConverter
    {
        public object? Convert(object data)
        {
            if (data is not string str)
            {
                return null;
            }
            if (!File.Exists(str))
            {
                return null;
            }
            return new FileInfo(str);
        }
    }
}
