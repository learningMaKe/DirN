using DirN.Utils.Events.EventType;
using DirN.Utils.KManager;
using DirN.Utils.KManager.HKey;
using DirN.Utils.NgManager;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
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
        public DelegateCommand<MouseButtonEventArgs> MouseDownCommand { get; }


        private UIElement? view;

        public UIElement View
        {
            get
            {
                view ??= GetView?.Invoke() ?? throw new InvalidOperationException("GetView is null");
                return view;
            }
        }

        public Func<UIElement>? GetView { get; set; }

        public NodeGraphicsViewModel(IContainerProvider provider) : base(provider)
        {
            graphicsManager = provider.Resolve<INodeGraphicsManager>();
            LoadedCommand = new DelegateCommand(Loaded);
            MouseDownCommand = new(MouseDown);
        }


        private void MouseDown(MouseButtonEventArgs args)
        {

        }


        private void Loaded()
        {
            graphicsManager.StoredWordVisiblity = false;
        }
    }
}
