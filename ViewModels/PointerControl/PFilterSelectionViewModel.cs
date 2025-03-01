using DirN.Utils.Attirubutes;
using DirN.Utils.Nodes.Achieved.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DirN.ViewModels.PointerControl
{
    public class PFilterSelectionViewModel:PViewModel
    {
        public static Dictionary<string,FilterBy> FilterMethodList { get; set; }

        static PFilterSelectionViewModel()
        {
            FilterMethodList = [];
            foreach (var filter in typeof(FilterBy).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                DesAttribute? des = filter.GetCustomAttribute<DesAttribute>();
                string description = des?.Description?? filter.Name;
                FilterBy? value = (FilterBy?)filter.GetValue(null);
                if (value.HasValue)
                {
                    FilterMethodList.Add(description, value.Value);
                }
            }
        }

        public string SelectedFilter { get; set; } = string.Empty;

        public ObservableCollection<string> FilterMethods { get; set; }

        public PFilterSelectionViewModel()
        {
            FilterMethods = [.. FilterMethodList.Keys];
            SelectedFilter = FilterMethods[0];
        }

        public override object? GetData()
        {
            return FilterMethodList[SelectedFilter];
        }
    }
}
