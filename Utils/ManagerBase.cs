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
    public class ManagerBase(IContainerProvider containerProvider) : BindableBase
    {
        protected IEventAggregator eventAggregator = containerProvider.Resolve<IEventAggregator>();

        protected IContentDialogService contentDialogService = containerProvider.Resolve<IContentDialogService>();

        public void ShowText(string content)
        {
            contentDialogService.ShowSimpleDialogAsync(new()
            {
                Title = "展示信息",
                CloseButtonText = "确认",
                Content = content,
            });
        }
    }

    public class ManagerBase<T> : ManagerBase where T :ManagerBase<T>
    {

        private static T? instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new InvalidOperationException($"{typeof(T).Name} is not initialized.");
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        public ManagerBase(IContainerProvider containerProvider):base(containerProvider)
        {
            Instance = (T)this;
        }

    }

    public class ManagerBase<TManager, TIManager> : ManagerBase where TManager : ManagerBase<TManager, TIManager>,TIManager
    {
        private static TIManager? instance;

        public static TIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new InvalidOperationException($"{typeof(TIManager).Name} is not initialized.");
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        public ManagerBase(IContainerProvider containerProvider) : base(containerProvider)
        {
            Instance = (TManager)this;
        }
    }
}
