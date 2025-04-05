using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DirN.Utils.WithColors
{
    public class WithColor : BindableBase, IWithColor
    {
        [OnChangedMethod(nameof(OnMainColorChanged))]
        public Color MainColor { get; set; } = Colors.Black;

        public Brush MainBrush { get;private set; } = Brushes.Black;

        public WithColor()
        {
            
        }

        public WithColor(Color mainColor)
        {
            this.MainColor = mainColor;
        }

        private void OnMainColorChanged()
        {
            MainBrush = new SolidColorBrush(MainColor);
        }
    }
}
