using DirN.Utils.DirManager;
using DirN.Utils.Events.EventType;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DirN.ViewModels
{
    public class PreviewerViewModel : BaseViewModel
    {
        private readonly DirectoryManager directoryManager;

        public ObservableCollection<string> Files { get; set; } = [];

        public DelegateCommand LoadedCommand { get; set; }

        public PreviewerViewModel(IContainerProvider provider) : base(provider)
        {
            LoadedCommand = new DelegateCommand(Loaded);

            directoryManager = provider.Resolve<DirectoryManager>();

            EventAggregator.GetEvent<DirectoryManagerEvent.DirectoryChangedEvent>().Subscribe(OnDirectoryChanged);

        }

        private void OnDirectoryChanged(string newDirectory)
        {
            GetFiles(newDirectory);
        }

        private void Loaded()
        {
            GetFiles(directoryManager.Directory);
        }

        private void GetFiles(string directory)
        {
            if (directory == null) return;
            if (!Directory.Exists(directory)) return;
            Files.Clear();
            DirectoryInfo di = new(directory);
            Files.AddRange(di.GetFiles().Select(p => p.Name));
        }
    }
}
