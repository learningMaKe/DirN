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
using System.Diagnostics;
using DirN.Utils.Debugs;

namespace DirN.ViewModels
{
    public class SearcherViewModel : BaseViewModel
    {
        public PreviewerManager PreviewerManager { get; private set; }
        
        public string StartLocation { get; set; } = DirectoryManager.UserProfilePath;

        public DelegateCommand TestCommand { get; set; }
        public DelegateCommand BrowseCommand { get; set; }
        public DelegateCommand<KeyEventArgs> ConfirmCommand { get; set; }
        public DelegateCommand LoadedCommand { get; set; }
        public DelegateCommand ExecuteCommand { get; set; }
        public DelegateCommand JsonSelectCommand { get; set; }
        public DelegateCommand SaveNodeCommand { get; set; }
        public DelegateCommand SaveAsNodeCommand { get; set; }
        public DelegateCommand OpenJsonHomeCommand { get; set; }
        public DelegateCommand ExecuteOrderCommand { get; set; }

        public SearcherViewModel(IContainerProvider provider) : base(provider)
        {
            PreviewerManager = provider.Resolve<PreviewerManager>();
            
            TestCommand = new(Test);
            BrowseCommand = new(Browse);
            ConfirmCommand = new(Confirm);
            LoadedCommand = new(Loaded);
            ExecuteCommand= new(Execute);
            JsonSelectCommand = new(JsonSelect);
            SaveNodeCommand = new(SaveNode);
            SaveAsNodeCommand = new(SaveAsNode);
            OpenJsonHomeCommand = new(OpenJsonHome);
            ExecuteOrderCommand = new(ExecuteOrder);
            EventAggregator.GetEvent<DirectoryManagerEvent.DirectoryChangedEvent>().Subscribe(OnDirectoryChanged);
        }

        private void OpenJsonHome()
        {
            try
            {
                // 使用Process.Start打开资源管理器
                Process.Start("explorer.exe", DirectoryManager.JsonHome);
            }
            catch (Exception ex)
            {
                Console.WriteLine("打开文件夹时出错: " + ex.Message);
            }
        }

        private void SaveNode()
        {
            NodeGraphicsManager.Instance.SaveNode();
        }

        private void SaveAsNode()
        {
            NodeGraphicsManager.Instance.SaveAsNode();
        }

        private void JsonSelect()
        {
            NodeGraphicsManager.Instance.NodeDetailSelect();
        }

        private void Execute()
        {
            NodeGraphicsManager.Instance.Execute();
        }

        private void ExecuteOrder()
        {
            NodeGraphicsManager.Instance.ExecuteOrder();
        }

        private void Test()
        {
            foreach(var curve in NodeGraphicsManager.Instance.NodeDetail.BezierCurves)
            {
                TooltipManager.Instance.Tooltip(curve, $"{curve.StartPoint.X:F2},{curve.StartPoint.Y:F2}->{curve.EndPoint.X:F2},{curve.EndPoint.Y:F2}");
            }
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
            DirectoryManager.Instance.WorkDirectory = directory;
            PreviewerManager.PreviewerVisibility = true;
        }

        private void OnDirectoryChanged(string newDirectory)
        {
            StartLocation = newDirectory;
        }

        private void Loaded()
        {
            StartLocation = DirectoryManager.Instance.WorkDirectory;
        }

    }
}
