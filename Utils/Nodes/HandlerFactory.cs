using DirN.Utils.Nodes.Bulider;
using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes
{
    public static class HandlerFactory
    {
        public static INodeHandler Create<THandler>(INode parent) where THandler : TypedHandler, new()
        {
            if (parent is null)
            {
                throw new NullReferenceException("Parent cannot be null");
            }
            THandler handler = new()
            {
                Parent = parent
            };
            handler.Init(parent);
            return handler;
        }

        public static HandlerConfig CreateConfig<THandler>(string Header) where THandler : TypedHandler, new()
        {
            HandlerConfig config = new(Header, Create<THandler>);
            return config;
        }

        /// <summary>
        /// Same as <see cref="CreateConfig{THandler}(string)"/>
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <param name="Header"></param>
        /// <returns></returns>
        public static HandlerConfig CC<THandler>(string Header) where THandler : TypedHandler, new()=> CreateConfig<THandler>(Header);
    
        public static HandlerConfig? CC(Type type, string Header,string description = "")
        {
            if (!typeof(TypedHandler).IsAssignableFrom(type))
            {
                return null;
            }
            Type fType = typeof(Func<,>).MakeGenericType(typeof(INode), typeof(INodeHandler));
            MethodInfo? mi = typeof(HandlerFactory).GetMethod("Create");
            if (mi is null)
            {
                return null;
            }
            MethodInfo gmi = mi.MakeGenericMethod(type);
            Delegate? func = Delegate.CreateDelegate(fType, gmi);
            if (func is null)
            {
                return null;
            }
            Type hcType = typeof(HandlerConfig);
            ConstructorInfo? ci = hcType.GetConstructor([typeof(string), typeof(Func<INode, INodeHandler>)]);
            if (ci is null)
            {
                return null;
            }
            HandlerConfig config = (HandlerConfig)ci.Invoke([Header, func]);
            config.Description = description;
            return config;
        }
    }
}
