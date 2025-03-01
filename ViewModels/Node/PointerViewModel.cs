using DirN.Utils.Base;
using DirN.Utils.NgManager;
using DirN.Utils.Nodes;
using DirN.ViewModels.PointerControl;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DirN.ViewModels.Node
{
    public abstract class PointerViewModel:BindableBase,IPointer
    {
        public INode NodeParent { get;private set; }

        public abstract IConnector Connector { get; }

        public bool IsInput => this.GetType() == typeof(InputerViewModel);

        public object? Data
        {
            get => GetData();
            set => SetData(value);
        }

        [OnChangedMethod(nameof(OnPointerTypeChanged))]
        public required Type PointerType { get; set; } = typeof(object);

        [OnChangedMethod(nameof(OnPointerConfigChanged))]
        public PointerConfig? PointerConfig { get; set; }

        public Action<PointerConfig>? PointerConfigChangedEvent { get; set; }

        public DelegateCommand LoadedCommand { get; private set; }

        public PointerViewModel(INode Parent)
        {
            this.NodeParent = Parent;
            PointerConfigChangedEvent += PointerConfigChanged;
            LoadedCommand = new(Loaded);
        }

        protected virtual void PointerConfigChanged(PointerConfig config)
        {
            
        }

        public abstract void UpdateLink();

        public abstract void CutLink();

        protected virtual object? GetData() { return null; }
        protected virtual void SetData(object? data) { }
        /// <summary>
        /// 获取指针数据
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">校验失败</exception>
        
        private void OnPointerConfigChanged()
        {
            if (PointerConfig is null) return;
            PointerConfigChangedEvent?.Invoke(PointerConfig);
        }

        private void OnPointerTypeChanged()
        {
            if (HandlerManager.GetPointer(PointerType, out PointerConfig? pointerConfig))
            {
                if (pointerConfig is not null)
                {
                    PointerConfig = pointerConfig;
                }
            }
        }

        private void Loaded()
        {
            OnPointerTypeChanged();
        }
    }
}