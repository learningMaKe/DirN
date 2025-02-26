using DirN.ViewModels.Node;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DirN.Views.Node
{
    /// <summary>
    /// Connector.xaml 的交互逻辑
    /// </summary>
    public partial class Connector : UserControl
    {
        public Connector()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if(DataContext is ConnectorViewModel vm)
            {
                vm.GetConnector = () => this;
            }
        }
    }
}
