using DirN.Utils.NgManager;
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
        public StoredWord SelectedWord { get; set; } = new();
        public string DisplayText { get; set; } = "保留字";

        public ObservableCollection<StoredWord> StoredWords { get; set; }

        public PStringViewModel()
        {
            StoredWords = NodeGraphicsManager.Instance.StoredWords;
        }

        protected override void Init()
        {

        }

        public override string? GetData()
        {
            return SelectedWord.Word;
        }
    }
}
