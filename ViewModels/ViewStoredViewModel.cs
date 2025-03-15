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

        public bool RequireAsk { get; set; } = false;

        public INodeGraphicsManager GraphicsManager { get; set; }

        public DelegateCommand TestCommand { get; set; }
        public DelegateCommand AddStoredWordCommand { get; set; }
        public AsyncDelegateCommand RemoveStoredWordCommand { get; set; }
        public AsyncDelegateCommand ClearAllStoredWordsCommand { get; set; }

        public AsyncDelegateCommand<SWord> RemoveSWordCommand { get; set; }

        public Action<bool>? StoredWordVisibilityChangedEvent { get; set; }

        public ViewStoredViewModel(IContainerProvider provider) : base(provider)
        {
            TestCommand = new DelegateCommand(Test);
            AddStoredWordCommand = new DelegateCommand(AddStoredWord);
            RemoveStoredWordCommand = new AsyncDelegateCommand(RemoveStoredWord);
            ClearAllStoredWordsCommand = new AsyncDelegateCommand(ClearAllStoredWords);
            RemoveSWordCommand = new AsyncDelegateCommand<SWord>(RemoveSWord);

            contentDialogService = provider.Resolve<IContentDialogService>();
            GraphicsManager = provider.Resolve<INodeGraphicsManager>();

            EventAggregator.GetEvent<NodeGraphicsEvent.StoredWordVisibilityEvent>().Subscribe(OnStoredWordVisibilityChanged);

        }

        private async Task RemoveSWord(SWord word)
        {
            if (!RequireAsk)
            {
                NodeGraphicsManager.Instance.RemoveSWord(word);
                return;
            }
            ContentDialogResult result = await contentDialogService.ShowSimpleDialogAsync(new()
            {
                Title = "移除词汇 " + word.Word,
                Content = "确定要移除吗？",
                PrimaryButtonText = "确定",
                CloseButtonText = "取消"
            });
            if (result == ContentDialogResult.Primary)
            {
                NodeGraphicsManager.Instance.RemoveSWord(word);
            }
        }

        private async Task ClearAllStoredWords()
        {
            if (!RequireAsk)
            {
                NodeGraphicsManager.Instance.ClearAllSWords();
                return;
            }
            ContentDialogResult result = await contentDialogService.ShowSimpleDialogAsync(new()
            {
                Title = "清空全部词汇",
                Content = "确定要清空全部词汇吗？",
                PrimaryButtonText = "确定",
                CloseButtonText = "取消"
            });
            if (result == ContentDialogResult.Primary)
            {
                NodeGraphicsManager.Instance.ClearAllSWords();
            }
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
            NodeGraphicsManager.Instance.AddSWord();
        }

        private async Task RemoveStoredWord()
        {
            if (!RequireAsk)
            {
                NodeGraphicsManager.Instance.RemoveSWord(SelectedStoredWord);
                return;
            }
            ContentDialogResult result = await contentDialogService.ShowSimpleDialogAsync(new()
            {
                Title = "移除词汇 " + SelectedStoredWord.Word,
                Content = "确定要移除吗？",
                PrimaryButtonText = "确定",
                CloseButtonText = "取消"
            });
            if (result == ContentDialogResult.Primary)
            {
                NodeGraphicsManager.Instance.RemoveSWord(SelectedStoredWord);
            }
        }
    }
}
