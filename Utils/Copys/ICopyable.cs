using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Copys
{
    public interface ICopyable<T>
    {
        public T Copy();
    }

    public interface ICopyable
    {
        public object Copy();
    }
}
