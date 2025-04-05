using DirN.Utils.Attirubutes;
using DirN.Utils.Nodes.Datas;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace DirN.ViewModels.PointerControl
{
    public class PEnumViewModel:PViewModel
    {
        private readonly static Dictionary<Type,Dictionary<string,object>> enumValues = [];

        [OnChangedMethod(nameof(OnPointerTypeChanged))]
        public Type PointerType { get; set; }=typeof(object);

        public ObservableCollection<string> EnumValues { get; set; } = [];

        public string SelectedItem { get; set; } = string.Empty;

        public override DataContainer GetData()
        {
            if (SelectedItem == string.Empty) return new(null);
            if(enumValues.TryGetValue(PointerType, out Dictionary<string, object>? existValues))
            {
                if(existValues.TryGetValue(SelectedItem, out object? value))
                {
                    return new(value);
                }
            }
            return new(null);
        }

        protected override void Init()
        {
            PointerType = PointerParent!.PointerType;
        }

        private void OnPointerTypeChanged()
        {
            if (PointerType == null)
            {
                EnumValues.Clear();
                return;
            }
            if (enumValues.TryGetValue(PointerType, out Dictionary<string, object>? existValues))
            {
                EnumValues = [.. existValues.Keys];
            }
            else if(PointerType.IsEnum)
            {
                FieldInfo[] fields = PointerType.GetFields(BindingFlags.Public | BindingFlags.Static);
                Dictionary<string, object> newValues = [];
                foreach(var field in fields)
                {
                    DesAttribute? des=field.GetCustomAttribute<DesAttribute>();
                    if (des == null) continue;
                    object? value = field.GetValue(null);
                    if(value == null) continue;
                    newValues.Add(des.Description, value);
                }
                EnumValues = [.. newValues.Keys];
                enumValues.Add(PointerType, newValues);
            }
            if (EnumValues.Count > 0)
            {
                SelectedItem = EnumValues[0];
            }
        }
    }
}
