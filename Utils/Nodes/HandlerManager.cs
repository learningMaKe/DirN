using DirN.Utils.Maps;
using DirN.Utils.Nodes.Achieved;
using DirN.Utils.Nodes.DataConverter;
using DirN.Utils.Nodes.Maps;
using DirN.Utils.Validation;
using DirN.ViewModels.Node;
using DirN.ViewModels.PointerControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace DirN.Utils.Nodes
{
    public class HandlerManager
    {
        private static readonly Dictionary<HandlerType, HandlerConfig> handlers = 
            Mapper<HandlerMap, HandlerType, HandlerConfig>.Instance;

        private static readonly Dictionary<Type, PointerConfig> pointerHandlers =
            Mapper<PointerMap, Type, PointerConfig>.Instance;

        private static readonly Dictionary<PointerControlType, Func<UserControl>> pointerControls =
            Mapper<PointerControlMap, PointerControlType, Func<UserControl>>.Instance;

        private static readonly Dictionary<(Type,Type),IDataConverter> dataConverters =
            Mapper<DataConverterMap, (Type,Type), IDataConverter>.Instance;

        public static bool GetHandler(INode parent, HandlerType type,out INodeHandler? handler)
        {
            if (handlers.TryGetValue(type, out HandlerConfig? value))
            {
                if (value is not null)
                {
                    handler = value.Create!(parent);
                    return true;
                }
            }
            handler = null;
            return false;
        }

        public static bool GetHandlerConfig(HandlerType type, out HandlerConfig? handler)
        {
            if (handlers.TryGetValue(type, out HandlerConfig? value))
            {
                if (value is not null)
                {
                    handler = value;
                    return true;
                }
            }
            handler = null;
            return false;
        }

        public static bool GetPointer(Type type, out PointerConfig? handler)
        {
            if (pointerHandlers.TryGetValue(type, out PointerConfig? value))
            {
                if (value is not null)
                {
                    handler = value.Copy();
                    return true;
                }
            }
            handler = null;
            return false;
        }

        public static bool GetPointerControl(PointerControlType type, out UserControl? control)
        {
            if (pointerControls.TryGetValue(type, out Func<UserControl>? value))
            {
                if (value is not null)
                {
                    control = value();
                    return true;
                }
            }
            Debug.WriteLine("Pointer control not found: " + type);
            control = null;
            return false;
        }
    
        public static bool ConvertData<Tin,Tout>(Tin input, out Tout? output)
        {
            if(input is null)
            {
                output = default;
                return false;
            }
            Type inT=typeof(Tin);
            Type outT=typeof(Tout);
            if (inT == outT)
            {
                if (input is Tout outData)
                {
                    output = outData;
                    return true;
                }
            }
            if (dataConverters.TryGetValue((inT, outT), out IDataConverter? value))
            {
                if (value is not null)
                {
                    Tout? data = (Tout?)value.Convert(input);
                    if (data is not null)
                    {
                        output = data;
                        return true;
                    }
                }
            }
            output = default;
            return false;
        }

        public static bool CanConvertData(Type inT, Type outT)
        {
            if (inT == outT)
            {
                return true;
            }
            if (dataConverters.ContainsKey((inT, outT)))
            {
                return true;
            }
            return false;
        }

        public static bool ConvertData(Type inT ,Type outT, object? input, out object? output)
        {
            if (input is null)
            {
                output = default;
                return false;
            }
            if (inT == outT)
            {
                output = input;
                return true;
            }
            if (dataConverters.TryGetValue((inT, outT), out IDataConverter? value))
            {
                if (value is not null)
                {
                    object? data =value.Convert(input);
                    if (data is not null)
                    {
                        output = data;
                        return true;
                    }
                }
            }
            output = default;
            return false;
        }

    }
}
