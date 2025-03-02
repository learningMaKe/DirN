using DirN.Utils.Debugs;
using DirN.Utils.Events.EventType;
using DirN.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DirN.Views
{
    /// <summary>
    /// NodeCanvas.xaml 的交互逻辑
    /// </summary>
    public partial class NodeCanvas : UserControl
    {
        private readonly NodeCanvasViewModel? viewModel;

        public NodeCanvas()
        {
            InitializeComponent();
            if (DataContext is NodeCanvasViewModel vm)
            {
                viewModel = vm;
            }
        }

        private void OnDragBegun(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("DragBegun");
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (viewModel == null) return;
            viewModel!.GetCanvas += () => CanvasContainer;
            viewModel!.GetView += () => MainCanvas;
            DebugManager.Instance.DebugCanvas = DecorateCanvas;
        }

        

    }
}
