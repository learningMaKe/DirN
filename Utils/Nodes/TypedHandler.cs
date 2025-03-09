using DirN.Utils.NgManager.Curves;
using DirN.Utils.Nodes.Attributes;
using DirN.Utils.Nodes.Bulider;
using DirN.Utils.Nodes.Exceptions;
using DirN.Utils.Tooltips;
using DirN.ViewModels.Node;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace DirN.Utils.Nodes
{
    public abstract class TypedHandler:BindableBase,INodeHandler
    {
        public INode? Parent { get; init; }

        private Dictionary<string, IPointer> InputDict { get; set; } = [];
        private Dictionary<string, IPointer> OutputDict { get; set; } = [];

        protected abstract Type[] InputTypes { get; }
        protected abstract Type[] OutputTypes { get; }

        public ObservableCollection<IPointer> InputGroup { get; set; } = [];
        public ObservableCollection<IPointer> OutputGroup { get; set; } = [];
        
        public IList<ICurve> OutputCurve=> [.. OutputGroup.SelectMany(x=>x.Connector.LinkedCurves).Distinct()];
        public IList<ICurve> InputCurve => [.. InputGroup.SelectMany(x => x.Connector.LinkedCurves).Distinct()];

        public string Header { get; set; } = "Undefined";

        public string Description { get; set; } = "Undefined";

        [OnChangedMethod(nameof(OnMainColorChanged))]
        public Color MainColor { get; set; } = Colors.Black;

        public Brush HeaderBrush { get;private set; } = Brushes.Black;

        public Color HeaderEffectColor { get;private set; } = Colors.Black;

        public IList<INode> Predecessors
        {
            get
            {
                IList<INode> predecessors = [];
                foreach (var input in InputGroup)
                {
                    IList<ICurve> linkedCurves = input.Connector.LinkedCurves;
                    if (linkedCurves.Count == 0) continue;
                    INode? predecessor = linkedCurves.Select(x => x.StartNode).FirstOrDefault();
                    if (predecessor is null) continue;
                    predecessors.Add(predecessor);
                }
                return predecessors;
            }
        }

        public void Init(INode parent) 
        {
            InputDict = HandlerBulider.Pointer<InputerViewModel>(parent, InputTypes);
            OutputDict = HandlerBulider.Pointer<OutputerViewModel>(parent, OutputTypes);
            
            InputGroup = [.. InputDict.Values];
            OutputGroup = [.. OutputDict.Values];

            Type type = GetType();
            HDesAttribute? hdes=type.GetCustomAttributes(typeof(HDesAttribute), true).FirstOrDefault() as HDesAttribute;
            if (hdes is not null)
            {
                Header = hdes.Header;
                Description = hdes.Description;
                MainColor = hdes.MainColor;
            }
            ExtraInit();
        }

        public bool DataFlow()
        {
            IList<object?>? output = HandleData();
            if (output is null) return false;
            return SetOutput(output);
        }

        public bool SetOutput(IList<object?> output)
        {
            if (output is null) return false;

            //OutputCountNotMatchException.ThrowIf(Parent!, output.Count, OutputGroup.Count);
            if(output.Count != OutputGroup.Count)
            {
                return false;
            }

            for (int i = 0; i < OutputGroup.Count; i++)
            {
                OutputGroup[i].Data = output[i];
            }
            return true;
        }

        public bool GetInput(out IList<object> datas)
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

        public IList<object?>? HandleData()
        {
            IList<object?>? output = null;
            if(GetInput(out IList<object> datas))
            {
                output = Handle([.. datas]);
            }
            return output;
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

        protected bool TryGetConfig<T>(bool isInput, out PointerConfig? config, int index = 0)
        {
            Type type = typeof(T);
            config = null;
            string key = type.Name + index;
            if (isInput)
            {
                if (InputDict.TryGetValue(key, out IPointer? value))
                {
                    config = value.PointerConfig;
                }
            }
            else
            {
                if (OutputDict.TryGetValue(key, out IPointer? value))
                {
                    config = value.PointerConfig;
                }
            }
            return config is not null;
        }

        protected abstract IList<object?> Handle(IList<object> input);

        private void OnMainColorChanged()
        {
            HeaderBrush = new SolidColorBrush(MainColor);
            HeaderEffectColor = Color.FromArgb(128, MainColor.R, MainColor.G, MainColor.B);
        }

        public void UpdateLink()
        {
            foreach (var pointer in InputGroup)
            {
                pointer.UpdateLink();
            }
            foreach (var pointer in OutputGroup)
            {
                pointer.UpdateLink();
            }
        }

        public void CutLink()
        {
            foreach (var link in InputGroup)
            {
                link.CutLink();
            }
            foreach (var link in OutputGroup)
            {
                link.CutLink();
            }
        }
    }
}
