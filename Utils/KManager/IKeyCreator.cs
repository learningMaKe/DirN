using DirN.Utils.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.KManager
{
    public interface IKeyCreator<T> : IMapCreator<EventId,T>
    {
    }
}
