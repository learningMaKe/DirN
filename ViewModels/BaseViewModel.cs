using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui;

namespace DirN.ViewModels
{
    public class BaseViewModel(IContainerProvider provider):BindableBase
    {
        protected IContainerProvider ContainerProvider { get; } = provider;

        protected IEventAggregator EventAggregator { get; } = provider.Resolve<IEventAggregator>();

        protected IContentDialogService ContentDialogService { get; } = provider.Resolve<IContentDialogService>();

    }
}
