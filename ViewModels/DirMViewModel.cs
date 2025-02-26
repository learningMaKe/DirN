using DirN.Utils.CommandLine;
using DirN.Utils.DirManager;
using DirN.Utils.Events.EventType;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf.Ui;
using Wpf.Ui.Extensions;

namespace DirN.ViewModels
{
    public class DirMViewModel : BaseViewModel
    {

        public event Action? PreviewerShow;
        public event Action? PreviewerHide;

        public DirMViewModel(IContainerProvider provider) : base(provider)
        {
            EventAggregator.GetEvent<PreviewerEvent.PreviewerShowEvent>().Subscribe(OnPreviewerShow);
            EventAggregator.GetEvent<PreviewerEvent.PreviewerHideEvent>().Subscribe(OnPreviewerHide);

        }



        private void OnPreviewerShow()
        {
            PreviewerShow?.Invoke();
        }

        private void OnPreviewerHide()
        {
            PreviewerHide?.Invoke();
        }
    }
}
