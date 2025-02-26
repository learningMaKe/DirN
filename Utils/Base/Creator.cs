using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Base
{
    public abstract class Creator<TCreator>:BindableBase where TCreator :ICreate,new() 
    {
        public abstract void Init();

        public static TCreator Create()
        {
            TCreator instance = new();
            instance.Init();
            return instance;
        }
    }
}
