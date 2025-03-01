using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils
{
    public class SingletonBase<T> where T : SingletonBase<T>
    {
        private static T? instance;

        public static T Instance
        {
            get
            {
                instance ??= Activator.CreateInstance<T>();
                return instance;
            }
        }

        public SingletonBase()
        {
            instance = this as T;
        }
    }
}
