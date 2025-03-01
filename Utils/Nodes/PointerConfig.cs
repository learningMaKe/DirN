using DirN.Utils.Copys;
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

        public string Description { get; set; } = "Configure the pointer";

        public bool UseConnector { get; set; } = true;

        public Func<object?, bool> Validate = TypeValidation.ForeverTrue;

        [OnChangedMethod(nameof(OnPointerColorChanged))]
        public Color PointerColor { get; set; }

        public Brush PointerBrush { get;private set; } = Brushes.Green;

        public PointerControlType ControlType { get; init; } = PointerControlType.PInt;

        private void OnPointerColorChanged()
        {
            PointerBrush = new SolidColorBrush(PointerColor);
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
    }

}
