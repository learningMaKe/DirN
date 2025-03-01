using DirN.Utils;
using DirN.Utils.Events.EventType;
using DirN.Utils.KManager;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace DirN.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        public MainWindow(IContainerProvider containerProvider)
        {
            InitializeComponent();
            IContentDialogService contentDialogService = containerProvider.Resolve<IContentDialogService>();
            contentDialogService.SetDialogHost(RootContentDialog);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
