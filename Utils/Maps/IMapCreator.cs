using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Maps
{
    public interface IMapCreator<TKey, TValue> where TKey:notnull
    {
        public void Create(Dictionary<TKey, TValue> source);
    }

    public interface IMapCreator<TClass>
    {
        public void Create(TClass source);
    }
}
