using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.NgManager
{
    public class StoredWord:BindableBase
    {
        public string Word { get; set; } = string.Empty;

        public bool IsEditable { get; set; } = false;

        public DelegateCommand<bool?> EditCommand { get; set; } 

        public int Index { get;set; }

        public StoredWord()
        {
            EditCommand = new DelegateCommand<bool?>(OnEdit);
        }

        private void OnEdit(bool? isEditable)
        {
            IsEditable = isEditable.GetValueOrDefault();
        }

    }
}
