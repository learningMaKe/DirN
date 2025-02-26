using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Validation
{
    public static class TypeValidation
    {
        public static bool ClassValidation<T>(object? value) where T : class
        {
            if(value == null)
            {
                return false;
            }
            return value.GetType() == typeof(T);
        }

        public static bool ForeverTrue(object? value)
        {
            return true;
        }

    }
}
