using DirN.Utils.Nodes.Attributes;
using DirN.Utils.Nodes.Bulider;
using DirN.Utils.Nodes.Datas;
using DirN.Utils.Tooltips;
using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Nodes
{
    public abstract class TypedHandler:BaseHandler
    {
        private Dictionary<string, IPointer> InputDict { get; set; } = [];
        private Dictionary<string, IPointer> OutputDict { get; set; } = [];

        protected abstract Type[] InputTypes { get; }
        protected abstract Type[] OutputTypes { get; }

        public override void Init(INode parent)
        {
            CreateInput(parent);
            CreateOutput(parent);
            ExtraInit();
        }

        public bool SetOutput(IList<DataContainer> output)
        {
            if (output is null) return false;

            //OutputCountNotMatchException.ThrowIf(Parent!, output.Count, OutputGroup.Count);
            if (output.Count != OutputGroup.Count)
            {
                return false;
            }

            for (int i = 0; i < OutputGroup.Count; i++)
            {
                OutputGroup[i].Data = output[i];
            }
            return true;
        }

        public bool GetInput(out IList<DataContainer> datas)
        {
            datas = [];
            if (Parent is null) return false;
            bool isError = false;
            string errorInfo = string.Empty;
            foreach (var input in InputGroup)
            {
                if (input.Data is not null)
                {
                    datas.Add(input.Data);
                    continue;
                }
                isError = true;
                errorInfo += $"Input {input.PointerConfig!.Header} is null. \n";
            }
            if (isError)
            {
                TooltipManager.Instance.Tooltip(Parent, errorInfo, TooltipType.Error);
                return false;
            }
            return true;
        }

        public sealed override bool DataFlow()
        {
            IList<DataContainer>? output = HandleData();
            if (output is null) return false;
            return SetOutput(output);
        }

        public IList<DataContainer> HandleData()
        {
            IList<DataContainer> output = [];
            if (GetInput(out IList<DataContainer> datas))
            {
                output = Handle([.. datas]);
            }
            return output;
        }

        protected abstract IList<DataContainer> Handle(IList<DataContainer> datas);


        /// <summary>
        /// 初始化 <see cref="InputDict"/> 和 <see cref="InputGroup"/>
        /// </summary>
        /// <param name="parent"></param>
        protected virtual void CreateInput(INode parent)
        {
            InputDict = HandlerBulider.Pointer<InputerViewModel>(parent, InputTypes);
            InputGroup = [.. InputDict.Values];
        }

        /// <summary>
        /// 初始化 <see cref="OutputDict"/> 和 <see cref="OutputGroup"/>
        /// </summary>
        /// <param name="parent"></param>
        protected virtual void CreateOutput(INode parent)
        {
            OutputDict = HandlerBulider.Pointer<OutputerViewModel>(parent, OutputTypes);
            OutputGroup = [.. OutputDict.Values];
        }

        protected virtual void ExtraInit() { }

        protected bool TryGetInputConfig<T>(out PointerConfig? config, int index = 0)
        {
            Type type = typeof(T);
            config = null;
            string key = type.Name + index;
            if (InputDict.TryGetValue(key, out IPointer? value))
            {
                config = value.PointerConfig;
            }
            return config is not null;
        }

        protected bool TryGetOutputConfig<T>(out PointerConfig? config, int index = 0)
        {
            Type type = typeof(T);
            config = null;
            string key = type.Name + index;
            if (OutputDict.TryGetValue(key, out IPointer? value))
            {
                config = value.PointerConfig;
            }
            return config is not null;
        }

    }
}
