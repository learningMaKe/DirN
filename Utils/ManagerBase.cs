using DirN.Utils.NgManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui;
using Wpf.Ui.Extensions;

namespace DirN.Utils
{
    public class ManagerBase<T>(IContainerProvider containerProvider) : BindableBase where T :ManagerBase<T>
    {
        private static T? instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new InvalidOperationException("NodeGraphicsManager is not initialized.");
                }
                return instance;
            }
            protected set
            {
                instance = value;
            }
        }

        protected IEventAggregator eventAggregator = containerProvider.Resolve<IEventAggregator>();

        protected IContentDialogService contentDialogService = containerProvider.Resolve<IContentDialogService>();

        public void ShowText(string content)
        {
            contentDialogService.ShowSimpleDialogAsync(new() 
            { 
                Title="展示信息",
                CloseButtonText="确认",
                Content=content,
            });
        }
    }
}
