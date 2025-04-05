using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes.Bulider
{
    public class HandlerBulider
    {
        public static Dictionary<string, IPointer> Pointer<TPointer>(INode parent, params Type[] pointerTypes) where TPointer:PointerViewModel
        {
            Dictionary<string, IPointer> pointerDict = [];
            foreach (var pointerType in pointerTypes)
            {
                IPointer pointer= (IPointer?)Activator.CreateInstance(typeof(TPointer), parent) ?? throw new NullReferenceException("Pointer is null");
                pointer.PointerType = pointerType;
                int index = 0;
                while (pointerDict.ContainsKey(pointerType.Name + index))
                {
                    index++;
                }
                string key = pointerType.Name + index;
                pointerDict.Add(key, pointer);
            }
            return pointerDict;
        }

    }
}
