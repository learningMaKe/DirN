using DirN.Utils.Nodes.Attributes;
using DirN.Utils.Nodes.Bulider;
using DirN.Utils.Nodes.Exceptions;
using DirN.ViewModels.Node;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DirN.Utils.Nodes
{
    public abstract class TypedHandler:BindableBase,INodeHandler
    {
        public INode? Parent { get; init; }

        public abstract Type[] InputTypes { get; }
        public abstract Type[] OutputTypes { get; }

        public ObservableCollection<IPointer> InputGroup { get; set; } = [];
        public ObservableCollection<IPointer> OutputGroup { get; set; } = [];

        public string Header { get; set; } = "Undefined";

        [OnChangedMethod(nameof(OnMainColorChanged))]
        public Color MainColor { get; set; } = Colors.Black;

        public Brush HeaderBrush { get;private set; } = Brushes.Black;

        public Color HeaderEffectColor { get;private set; } = Colors.Black;

        public virtual void Init(INode parent) 
        {
            Type type = GetType();
            HDesAttribute? hdes=type.GetCustomAttributes(typeof(HDesAttribute), true).FirstOrDefault() as HDesAttribute;
            if (hdes is not null)
            {
                Header = hdes.Header;
                MainColor = hdes.MainColor;
            }
        }

        public void DataFlow()
        {
            IList<object?>? output = GetOutput();
            if (output is null) return;
            SetOutput(output);
        }

        public void SetOutput(IList<object?> output)
        {
            if (output is null) return;

            OutputCountNotMatchException.ThrowIf(Parent!, output.Count, OutputGroup.Count);

            for (int i = 0; i < OutputGroup.Count; i++)
            {
                OutputGroup[i].Data = output[i];
            }
        }

        public IList<object?>? GetOutput()
        {
            object?[] input = [.. InputGroup.Select(x => x.Data)];
            IList<object?>? output = Handle(input);
            return output;
        }

        protected abstract IList<object?> Handle(IList<object?> input);

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
