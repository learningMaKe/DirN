using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Media;

namespace DirN.Utils.Extension
{
    public static class UIElementExtension
    {
        public static void OpacityAnimation(this UIElement element, double opacity, double duration = 0.2,EventHandler? completed = null)
        {
            element.Visibility=Visibility.Visible;
            DoubleAnimation animation = new()
            {
                From = element.Opacity,
                To = opacity,
                Duration = new Duration(TimeSpan.FromSeconds(duration))
            };
            animation.Completed += (sender, e) =>
            {
                completed?.Invoke(sender, e); 
            };
            element.BeginAnimation(UIElement.OpacityProperty, null);
            element.BeginAnimation(UIElement.OpacityProperty, animation);

        }
    
        public static T? GetParent<T>(this UIElement element) where T : UIElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(element);
            while (parent!= null)
            {
                if (parent is T)
                {
                    return (T)parent;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }
    }
}
