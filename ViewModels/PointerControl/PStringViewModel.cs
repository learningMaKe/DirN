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
        public SWord SelectedWord { get; set; } = new();

        public ObservableCollection<SWord> StoredWords { get; set; }

        public PStringViewModel()
        {
            StoredWords = NodeGraphicsManager.Instance.NodeDetail.SWords;
        }

        protected override void Init()
        {

        }

        public override string? GetData()
        {
            return SelectedWord.Word;
        }

        public override void SetData(object? data)
        {
            if (data is string word)
            {
                SelectedWord = new() { Word = word, Index = 0 };
            }
        }
    }
}
