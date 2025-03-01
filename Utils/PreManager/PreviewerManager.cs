using DirN.Utils.Events.EventType;
using DirN.Utils.KManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DirN.Utils.PreManager
{
    public class PreviewerManager:BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly KeyManager keyManager;

        [PropertyChanged.OnChangedMethod(nameof(OnPreviewerVisibilityChanged))]
        public bool PreviewerVisibility { get; set; }

        public PreviewerManager(IContainerProvider containerProvider)
        {
            eventAggregator = containerProvider.Resolve<IEventAggregator>();
            keyManager = containerProvider.Resolve<KeyManager>();

            keyManager.RegisterEvent<KeyEventArgs>(EventId.V_Previewer, OnKeyEnter);
        }

        private void OnPreviewerVisibilityChanged()
        {
            if (PreviewerVisibility)
            {
                eventAggregator.GetEvent<PreviewerEvent.PreviewerShowEvent>().Publish();
            }
            else
            {
                eventAggregator.GetEvent<PreviewerEvent.PreviewerHideEvent>().Publish();
            }
            eventAggregator.GetEvent<PreviewerEvent.PreviewerVisibilityChangedEvent>().Publish(PreviewerVisibility);
        }

        private void OnKeyEnter(KeyEventArgs e)
        {
            e.Handled = true;
            PreviewerVisibility = !PreviewerVisibility;
        }

    }
}
