using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Validation
{
    public static class StructValidation<T> where T : struct
    {
        public static bool SimpleValidation()
        {
            return true;
        }
    }
}
