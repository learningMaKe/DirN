using DirN.Utils.Extension;
using DirN.Utils.KManager;
using DirN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DirN.Views
{
    /// <summary>
    /// DirM.xaml 的交互逻辑
    /// </summary>
    public partial class DirM : UserControl
    {
        private readonly DirMViewModel? viewModel;

        public DirM()
        {
            InitializeComponent();

            viewModel = DataContext as DirMViewModel;

            if(viewModel!= null)
            {
                viewModel.PreviewerShow += PreviewerShow;
                viewModel.PreviewerHide += PreviewerHide;
            }
        }

        private void PreviewerShow()
        {
            Previewer.Visibility = Visibility.Visible;
            NodeGraphics.OpacityAnimation(0.2);
            Previewer.OpacityAnimation(1);
        }

        private void PreviewerHide()
        {

            NodeGraphics.OpacityAnimation(1);
            Previewer.OpacityAnimation(0, completed: (s, e) => { Previewer.Visibility = Visibility.Hidden; });
        }

    }
}
