using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace DirN.ViewModels.PointerControl
{
    public static class PointerControlFactory
    {
        public static TView Create<TView,TViewModel>() where TView : UserControl, new() where TViewModel : IPointerControl, new()
        {
            TViewModel vm = new();
            TView view = new()
            {
                DataContext = vm
            };
            return view;
        }
    }
}
