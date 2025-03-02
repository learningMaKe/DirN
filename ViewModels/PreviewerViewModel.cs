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
        public DirectoryManager DirectoryManager { get; set; }

        public PreviewerViewModel(IContainerProvider provider) : base(provider)
        {
            DirectoryManager = provider.Resolve<DirectoryManager>();
        }

    }
}
