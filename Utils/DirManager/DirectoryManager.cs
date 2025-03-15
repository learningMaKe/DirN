using DirN.Utils.CommandLine;
using DirN.Utils.Events.EventType;
using DirN.Utils.NgManager;
using DryIoc.FastExpressionCompiler.LightExpression;
using Newtonsoft.Json.Linq;
using Prism.Events;
using PropertyChanged;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui;
using Wpf.Ui.Extensions;
using static DirN.Utils.Events.EventType.DirectoryManagerEvent;

namespace DirN.Utils.DirManager
{
    public class DirectoryManager:ManagerBase<DirectoryManager>
    {
        private readonly ApplicationParameter applicationParameter;

        public static string UserProfilePath => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        public static string JsonHome => Path.Combine(Environment.CurrentDirectory,"Nodes");

        static DirectoryManager()
        {
            if (!Directory.Exists(JsonHome))
            {
                Directory.CreateDirectory(JsonHome);
            }
        }

        private string workDirectory = UserProfilePath;

        public string WorkDirectory
        {
            get => workDirectory;
            set
            {
                if (!System.IO.Directory.Exists(value))
                {
                    contentDialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions() 
                    {
                        CloseButtonText = "OK",
                        Title = "Directory not found",
                        Content = "The directory you entered does not exist. Please enter a valid directory."
                    });
                    return;
                }
                workDirectory = value;
                OnWorkDirectoryChanged();
            }
        }

        public ObservableCollection<string> Files { get; set; } = [];

        public ObservableCollection<string> PreviewFiles { get; set; } = [];

       
        public DirectoryManager(IContainerProvider containerProvider):base(containerProvider)
        {
            applicationParameter = containerProvider.Resolve<ApplicationParameter>();
            
            WorkDirectory = applicationParameter.Directory;
        }

        private void OnWorkDirectoryChanged()
        {
            eventAggregator.GetEvent<DirectoryChangedEvent>().Publish(WorkDirectory);
            Files.Clear();
            if (Directory.Exists(WorkDirectory))
            {
                Files = [.. Directory.GetFiles(WorkDirectory)];
            }
        }


    }
}
