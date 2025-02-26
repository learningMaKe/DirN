using DirN.Utils.CommandLine;
using DirN.Utils.Events.EventType;
using DryIoc.FastExpressionCompiler.LightExpression;
using Prism.Events;
using System;
using System.CodeDom;
using System.Collections.Generic;
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
    public class DirectoryManager:BindableBase
    {
        private readonly IEventAggregator eventAggregator;

        private readonly IContentDialogService contentDialogService;

        private readonly ApplicationParameter applicationParameter;

        private static DirectoryManager? instance;

        public static DirectoryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new NullReferenceException("DirectoryManager instance is null. Please initialize it first.");
                }
                return instance;
            }
        }

        public static string UserProfilePath => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        private string directory = UserProfilePath;

        public string Directory
        {
            get => directory;
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
                eventAggregator.GetEvent<DirectoryChangedEvent>().Publish(value);
                directory = value;
            }
        }

        public event EventHandler<string>? OnDirectoryChanged;

        public DirectoryManager(IContainerProvider containerProvider)
        {
            instance = this;

            eventAggregator = containerProvider.Resolve<IEventAggregator>();
            applicationParameter = containerProvider.Resolve<ApplicationParameter>();
            contentDialogService = containerProvider.Resolve<IContentDialogService>();

            Directory = applicationParameter.Directory;
        }


    }
}
