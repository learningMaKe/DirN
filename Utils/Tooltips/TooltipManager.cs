using System.Windows.Media;

namespace DirN.Utils.Tooltips
{
    public class TooltipManager : ManagerBase<TooltipManager>
    {
        public TooltipGroup Tooltipers { get; } = [];

        public TooltipManager(IContainerProvider containerProvider) : base(containerProvider)
        {

        }

        public void Tooltip(ITooltipable tooltipable, string tooltipText,Color? color=null)
        {
            Tooltipers.Add(tooltipable, tooltipText, color);
        }

        public void Tooltip(ITooltipable tooltipable, string tooltipText, TooltipType type)
        {
            Tooltipers.Add(tooltipable, tooltipText, type);
        }

        public void RemoveTooltip(ITooltipable tooltipable)
        {
            Tooltipers.Remove(tooltipable);
        }
    }
}
