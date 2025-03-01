using DirN.Utils.Base;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DirN.Utils.Maps
{
    public static class Mapper<TCreator, TClass> where TCreator : IMapCreator<TClass> ,new() where TClass:new()
    {
        private readonly static Lazy<TClass> instance = new(Create);

        public static TClass Instance
        {
            get
            {
                return instance.Value;
            }
        }

        private static TClass Create()
        {
            TCreator creator = new();
            TClass res = new();
            creator.Create(res);
            return res;
        }
    }

    public static class Mapper<TCreator, TKey,TValue> where TCreator:IMapCreator<TKey, TValue>,new() where TKey:notnull
    {
        private readonly static Lazy<Dictionary<TKey, TValue>> instance = new(Create);

        public static Dictionary<TKey, TValue> Instance
        {
            get
            {
                return instance.Value;
            }
        }

        private static Dictionary<TKey, TValue> Create()
        {
            Dictionary<TKey, TValue> res = [];
            TCreator creator = new();
            creator.Create(res);
            return res;
        }

    }
}
