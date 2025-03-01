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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DirN.Views
{
    /// <summary>
    /// ViewStored.xaml 的交互逻辑
    /// </summary>
    public partial class ViewStored : UserControl
    {
        private readonly ViewStoredViewModel? viewModel;

        public ViewStored()
        {
            InitializeComponent();
            if (DataContext is ViewStoredViewModel)
            {
                viewModel = DataContext as ViewStoredViewModel;

                viewModel!.StoredWordVisibilityChangedEvent += OnStoredWordVisibilityChanged;
            }
        }

        private void OnStoredWordVisibilityChanged(bool isVisible)
        {
            double currentX = StoredWordList.RenderTransform.Value.OffsetX;
            double To = isVisible ? -(StoredWordList.ActualWidth + 10) : 0;
            DoubleAnimation animation = new();
            {
                animation.From = currentX;
                animation.To = To;
                animation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
            }
            StoredWordTranslateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }
    }
}
