using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DirN.Utils.Tooltips
{
    public class TooltipGroup:ObservableCollection<Tooltiper>
    {
        private readonly Dictionary<ITooltipable, Tooltiper> tooltips = [];

        public void Add(ITooltipable tooltipable ,string tooltipText,Color? color)
        {
            color ??= Colors.White;
            if (tooltips.TryGetValue(tooltipable, out Tooltiper? value))
            {
                value.ToolTipText = tooltipText;
                value.Colorer.MainColor = color.Value;
                return;
            }
            Tooltiper tooltiper = new(tooltipable)
            {
                ToolTipText = tooltipText,
                Colorer = new()
            };
            tooltiper.Colorer.MainColor = color.Value;
            Add(tooltiper);
        }

        public void Add(ITooltipable tooltipable, string tooltipText,TooltipType type)
        {
            Color color=TooltipParameter.GetColor(type);
            Add(tooltipable, tooltipText, color);
        }

        public new void Add(Tooltiper tooltiper)
        {
            if (tooltips.ContainsKey(tooltiper.Tooltipable))
            {
                return;
            }
            tooltips.Add(tooltiper.Tooltipable, tooltiper);
            base.Add(tooltiper);
        }

        public new void Remove(Tooltiper tooltiper)
        {
            tooltips.Remove(tooltiper.Tooltipable);
            base.Remove(tooltiper);
        }

        public void Remove(ITooltipable tooltipable)
        {
            if (tooltips.TryGetValue(tooltipable, out Tooltiper? value))
            {
                Remove(value);
            }
        }
    }
}
