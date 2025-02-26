using DirN.Utils.Extension;
using DirN.Utils.Nodes.Achieved;
using DirN.ViewModels.Node;

namespace DirN.Utils.Nodes.Maps
{
    public static class HandlerMap
    {
        public static Dictionary<HandlerType, HandlerConfig> Create()
        {
            Dictionary<HandlerType, HandlerConfig> handlerMap = [];
            handlerMap.
                Set(HandlerType.Input, new("输入", TypedHandler.Create<InputHandler>)).
                Set(HandlerType.Output, new("输出", TypedHandler.Create<OutputHandler>)).
                Set(HandlerType.Debug, new("调试", TypedHandler.Create<DebugHandler>)).
                Set(HandlerType.Join, new("连接", TypedHandler.Create<JoinHandler>)).
                Set(HandlerType.StoredWord, new("存储单词", TypedHandler.Create<StoredWordHandler>));

            return handlerMap;
        }

    }
}
