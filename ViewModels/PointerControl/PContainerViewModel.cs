using DirN.Utils.Nodes;
using DirN.Utils.Nodes.Datas;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DirN.ViewModels.PointerControl
{
    public class PContainerViewModel:BindableBase,IPContainer
    {
        public DataContainer Data
        {
            get => GetData();
            set => SetData(value);
        }

        [OnChangedMethod(nameof(OnControlTypeChanged))]
        public PointerControlType ControlType { get; set; } = PointerControlType.None;

        public bool ControlVisibility { get; set; } = true;

        public IPointer PointerParent { get; set; }

        public UserControl? PointerControlView { get;private set; }

        public IPointerControl? PointerControlViewModel { get;private set; }

        public PContainerViewModel(IPointer pointer)
        {
            PointerParent = pointer;

            PointerParent.PointerConfigChangedEvent += config => ControlType = config.ControlType;
        }

        private void OnControlTypeChanged()
        {
            if(HandlerManager.GetPointerControl(ControlType,out UserControl? pointerControl))
            {
                PointerControlView = pointerControl;
                PointerControlViewModel = pointerControl!.DataContext as IPointerControl;
                PointerControlViewModel?.Init(this);
            }
        }

        public DataContainer GetData()
        {
            if(PointerControlViewModel is null) throw new NullReferenceException("PointerControlViewModel is null");
            return PointerControlViewModel.Data;
        }

        public void SetData(DataContainer data)
        {
            if(PointerControlViewModel is null)
            {
                return;
            }
            PointerControlViewModel.Data = data;
        }

    }
}
