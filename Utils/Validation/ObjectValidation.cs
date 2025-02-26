using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Validation
{
    public static class ObjectValidation<T> where T : class
    {
        public static bool SimpleValidation(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            return obj is T;
        }
    }
}
