using DirN.Utils.Copys;
using DirN.Utils.Nodes.Attributes;
using DirN.Utils.Validation;
using DirN.ViewModels.Node;
using DirN.ViewModels.PointerControl;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace DirN.Utils.Nodes
{
    public class PointerConfig:BindableBase,ICopyable<PointerConfig>,ICopyable
    {
        public string Header { get; set; } = "Pointer";

        public string Description { get; set; } = "啥也没写";

        public bool UseConnector { get; set; } = true;

        public Func<object?, bool> Validate = TypeValidation.ForeverTrue;

        [OnChangedMethod(nameof(OnPointerColorChanged))]
        public Color PointerColor { get; set; }

        public Brush PointerBrush { get;private set; } = Brushes.Green;

        public PointerControlType ControlType { get; set; } = PointerControlType.PInt;

        public void Init(HPDesAttribute attribute)
        {
            Header = attribute.Header;
            Description = attribute.Description;
            PointerColor = attribute.PointerColor;
            ControlType = attribute.ControlType;
            UseConnector = attribute.UseConnector;
        }

        public static PointerConfig Create(HPDesAttribute attribute)
        {
            PointerConfig pointerConfig = new();
            pointerConfig.Init(attribute);
            return pointerConfig;
        }

        public PointerConfig Copy()
        {
            PointerConfig copy = new()
            {
                Header = Header,
                Description = Description,
                UseConnector = UseConnector,
                PointerColor = PointerColor,
                ControlType = ControlType
            };
            return copy;
        }

        object ICopyable.Copy()
        {
            return Copy();
        }

        private void OnPointerColorChanged()
        {
            PointerBrush = new SolidColorBrush(PointerColor);
        }
    }

}
