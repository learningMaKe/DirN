using DirN.Utils.Nodes;
using DirN.ViewModels.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DirN.ViewModels.PointerControl
{
    public abstract class PViewModel:BindableBase,IPointerControl 
    {
        public IPContainer? Parent { get; set; }
        public IPointer? PointerParent { get; set; }
        public PointerConfig? Config { get; set; }

        public void Init(IPContainer parent) 
        {
            Parent = parent;
            PointerParent = parent.PointerParent;
            Config = PointerParent.PointerConfig;
            Init();
        }

        protected virtual void Init()
        {

        }

        public object? Data
        {
            get => GetData();
            set => SetData(value);
        }

        public abstract object? GetData();

        public virtual void SetData(object? data) { }
    }
}
