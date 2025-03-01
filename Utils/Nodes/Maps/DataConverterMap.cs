using DirN.Utils.Extension;
using DirN.Utils.Maps;
using DirN.Utils.Nodes.DataConverter;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Maps
{
    public class DataConverterMap : IMapCreator<(Type, Type), IDataConverter>
    {
        private static KeyValuePair<(Type, Type), IDataConverter> MP<TDataConverter>(Type sourceType, Type targetType) where TDataConverter : IDataConverter, new()
        {
            return new KeyValuePair<(Type, Type), IDataConverter>((sourceType, targetType), new TDataConverter());
        }

        public void Create(Dictionary<(Type, Type), IDataConverter> source)
        {
            source.
                Set(MP<DCString2FileInfo>(typeof(string), typeof(FileInfo))).
                Set(MP<DCString2DirectoryInfo>(typeof(string), typeof(DirectoryInfo)));

        }
    }
}
