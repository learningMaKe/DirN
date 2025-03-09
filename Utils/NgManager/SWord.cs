using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.NgManager
{
    public class SWord:BindableBase
    {
        public string Word { get; set; } = string.Empty;

        [JsonIgnore]
        public bool IsEditable { get; set; } = false;

        [JsonIgnore]
        public DelegateCommand<bool?> EditCommand { get; set; } 

        public int Index { get;set; }

        public SWord()
        {
            EditCommand = new DelegateCommand<bool?>(OnEdit);
        }

        private void OnEdit(bool? isEditable)
        {
            IsEditable = isEditable.GetValueOrDefault();
        }

    }
}
