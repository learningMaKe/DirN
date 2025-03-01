using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DirN.ViewModels
{
    public interface IViewGetter
    {
        public FrameworkElement View { get; }

        public Func<FrameworkElement>? GetView { get; set; }
    }
}
