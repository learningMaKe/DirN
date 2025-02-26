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
    public abstract class PViewModel<TView,TViewModel>:BindableBase,IPointerControl where TView:UserControl,new() where TViewModel:IPointerControl,new()
    {
        public IPContainer? Parent { get; set; }
        public IPointer? PointerParent { get; set; }

        public static TView Create()
        {
            TViewModel vm = new();
            TView view = new()
            {
                DataContext = vm
            };
            return view;
        }

        public void Init(IPContainer parent) 
        {
            Parent = parent;
            PointerParent = parent.PointerParent;
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

        public virtual object? GetData() { return null; }

        public virtual void SetData(object? data) { }
    }
}
