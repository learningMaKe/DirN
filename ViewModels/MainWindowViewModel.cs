using DirN.Utils.CommandLine;
using DirN.Utils.Events.EventType;
using Fclp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DirN.ViewModels
{
    public class MainWindowViewModel:BaseViewModel
    {
        public MainWindowViewModel(IContainerProvider containerProvider):base(containerProvider)
        {

        }


        public string Title { get;private set; } = "文件管理";

        public string Icon { get; } = "pack://application:,,,/DirN;component/Resources/Images/folder.ico";


        
    }
}
