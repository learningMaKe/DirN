using DirN.Utils.DirManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf.Ui.Extensions;
using Wpf.Ui;
using DirN.Utils.Events.EventType;
using DirN.Utils.PreManager;
using DirN.Utils.NgManager;
using System.Windows;
using DirN.Utils.Tooltips;

namespace DirN.ViewModels
{
    public class SearcherViewModel : BaseViewModel
    {
        public DirectoryManager DirectoryManager { get;private set; }
        public PreviewerManager PreviewerManager { get; private set; }
        public INodeGraphicsManager NodeGraphicsManager { get; private set; }

        public string StartLocation { get; set; } = DirectoryManager.UserProfilePath;

        public DelegateCommand TestCommand { get; set; }
        public DelegateCommand BrowseCommand { get; set; }
        public DelegateCommand<KeyEventArgs> ConfirmCommand { get; set; }
        public DelegateCommand LoadedCommand { get; set; }
        public DelegateCommand ExecuteCommand { get; set; }

        public SearcherViewModel(IContainerProvider provider) : base(provider)
        {
            DirectoryManager = provider.Resolve<DirectoryManager>();
            PreviewerManager = provider.Resolve<PreviewerManager>();
            NodeGraphicsManager = provider.Resolve<INodeGraphicsManager>();

            TestCommand = new(Test);
            BrowseCommand = new(Browse);
            ConfirmCommand = new(Confirm);
            LoadedCommand = new(Loaded);
            ExecuteCommand= new(Execute);
            EventAggregator.GetEvent<DirectoryManagerEvent.DirectoryChangedEvent>().Subscribe(OnDirectoryChanged);
        }

        private void Execute()
        {
            NodeGraphicsManager.Execute();
        }


        private void Test()
        {
            NodeGraphicsManager.SaveNode();
        }

        private void Browse()
        {

            var dialog = new Microsoft.Win32.OpenFolderDialog
            {
                InitialDirectory = StartLocation,
                DefaultDirectory = DirectoryManager.UserProfilePath,
            };
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                SetDirectory(dialog.FolderName);
            }
        }

        private void Confirm(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetDirectory(StartLocation);
            }
        }

        private void SetDirectory(string directory)
        {
            DirectoryManager.WorkDirectory = directory;
            PreviewerManager.PreviewerVisibility = true;
        }

        private void OnDirectoryChanged(string newDirectory)
        {
            StartLocation = newDirectory;
        }

        private void Loaded()
        {
            StartLocation = DirectoryManager.WorkDirectory;
        }

    }
}
