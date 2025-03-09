using DirN.Utils.Events.EventType;
using DirN.Utils.NgManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;
using Wpf.Ui;
using Wpf.Ui.Extensions;

namespace DirN.ViewModels
{
    public class ViewStoredViewModel:BaseViewModel
    {
        private readonly IContentDialogService contentDialogService;

        public SWord SelectedStoredWord { get; set; } = new();

        public INodeGraphicsManager NodeGraphicsManager { get; set; }

        public DelegateCommand TestCommand { get; set; }
        public DelegateCommand AddStoredWordCommand { get; set; }
        public AsyncDelegateCommand RemoveStoredWordCommand { get; set; }

        public Action<bool>? StoredWordVisibilityChangedEvent { get; set; }

        public ViewStoredViewModel(IContainerProvider provider) : base(provider)
        {
            TestCommand = new DelegateCommand(Test);
            AddStoredWordCommand = new DelegateCommand(AddStoredWord);
            RemoveStoredWordCommand = new AsyncDelegateCommand(RemoveStoredWord);

            NodeGraphicsManager = provider.Resolve<INodeGraphicsManager>();
            contentDialogService = provider.Resolve<IContentDialogService>();

            EventAggregator.GetEvent<NodeGraphicsEvent.StoredWordVisibilityEvent>().Subscribe(OnStoredWordVisibilityChanged);

        }


        private void Test()
        {

        }

        private void OnStoredWordVisibilityChanged(bool isVisible)
        {
            StoredWordVisibilityChangedEvent?.Invoke(isVisible);
        }

        private void AddStoredWord()
        {
            NodeGraphicsManager.AddSWord();
        }

        private async Task RemoveStoredWord()
        {
            ContentDialogResult result = await contentDialogService.ShowSimpleDialogAsync(new()
            {
                Title = "移除词汇 " + SelectedStoredWord.Word,
                Content = "确定要移除吗？",
                PrimaryButtonText = "确定",
                CloseButtonText = "取消"
            });
            if (result == ContentDialogResult.Primary)
            {
                NodeGraphicsManager.RemoveSWord(SelectedStoredWord);
            }
        }
    }
}
