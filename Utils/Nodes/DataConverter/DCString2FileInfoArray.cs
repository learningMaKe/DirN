using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.DataConverter
{
    public class DCString2FileInfoArray : IDataConverter
    {
        public object? Convert(object data)
        {
            if (data is string str)
            {
                if (Directory.Exists(str))
                {
                    DirectoryInfo dirInfo = new(str);
                    return dirInfo.GetFiles();
                }
            }
            return null;
        }
    }

}
