using DirN.Utils.NgManager;
using DirN.Utils.Nodes.Datas;
using DirN.ViewModels.Node;
using DirN.Views.PointerControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.ViewModels.PointerControl
{
    public class PStringViewModel:PViewModel
    {
        private string word = string.Empty;

        public SWord SelectedWord { get; set; } = new();

        public NodeGraphicsManager NodeGraphicsManager { get; set; }

        public DelegateCommand LoadedCommand { get; set; }

        public PStringViewModel()
        {
            LoadedCommand = new DelegateCommand(Loaded);
            NodeGraphicsManager = NodeGraphicsManager.Instance;
        }

        protected override void Init()
        {
            
        }

        public override DataContainer GetData()
        {
            return new(SelectedWord.Word);
        }

        public override void SetData(DataContainer data)
        {
            string? word = data.GetData<string>();
            if(string.IsNullOrEmpty(word)) return;
            this.word = word;
            Loaded();
        }

        private void Loaded()
        {
            foreach (var sword in NodeGraphicsManager.NodeDetail.SWords)
            {
                if (sword.Word == word)
                {
                    SelectedWord = sword;
                }
            }
        }
    }
}
