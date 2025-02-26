using DirN.Utils.Events.EventType;
using DirN.Utils.NgManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;

namespace DirN.ViewModels
{
    public class NodeGraphicsViewModel : BaseViewModel
    {
        private readonly INodeGraphicsManager graphicsManager;

        public DelegateCommand LoadedCommand { get; }

        public NodeGraphicsViewModel(IContainerProvider provider) : base(provider)
        {
            graphicsManager = provider.Resolve<INodeGraphicsManager>();
            LoadedCommand = new DelegateCommand(Loaded);
        }

        private void Loaded()
        {
            graphicsManager.StoredWordVisiblity = false;
        }
    }
}
