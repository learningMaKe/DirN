using DirN.Utils.Extension;
using DirN.Utils.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DirN.Utils.KManager
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TCreator">对应的创造器</typeparam>
    /// <typeparam name="TKey">映射事件的参数</typeparam>
    /// <typeparam name="TValue">事件类型 </typeparam>
    public abstract class CreatorHandler<TCreator, TKey, TValue> : MapHandler<TKey, TValue> 
        where TCreator:IKeyCreator<TKey>,new() 
        where TKey : notnull 
        where TValue : EventArgs
    {
        protected override IList<(EventId, TKey)> DefaultList
            => Mapper<TCreator, EventId,TKey>.Instance.AsTupleList();

    }
}
