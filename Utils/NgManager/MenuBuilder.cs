using DirN.Utils.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DirN.Utils.NgManager
{
    public class MenuItemInfo:BindableBase
    {
        public string Header { get; set; }=string.Empty;
        public ICommand? Command { get; set; }
        public object? CommandParameter { get; set; }
        public IList<MenuItemInfo> Items { get; } = [];
        public MenuItemInfo? Parent { get; set; }
    }

    public class MenuBuilder
    {
        public static MenuItemBuilder BuildMenu()
        {
            MenuItemInfo menuItem = new();
            return new(menuItem);
        }

        public class MenuItemBuilder
        {
            private readonly MenuItemInfo Source;
            private MenuItemInfo Node;

            public MenuItemBuilder(MenuItemInfo node)
            {
                Source = node;
                Node = node;
            }

            public IList<MenuItemInfo> Build() 
            {
                return [.. Source.Items.Cast<MenuItemInfo>()];
            }

            public MenuItemBuilder Next()
            {
                if (Node.Items.LastOrDefault() is not MenuItemInfo item)
                {
                    item = new MenuItemInfo();
                    Node.Items.Add(item);
                }
                Node = item;
                return this;
            }

            public MenuItemBuilder Back()
            {
                MenuItemInfo? parent = Node.Parent;
                if(parent is not null)
                {
                    Node = parent;
                }
                return this;
            }


            public MenuItemBuilder Set(string header,ICommand? command = default, object? parameter=default)
            {
                MenuItemInfo item = new()
                {
                    Header = header,
                    Command = command,
                    CommandParameter = parameter,
                    Parent = Node
                };
                Node.Items.Add(item);
                return this;
            }

            public MenuItemBuilder SetRange(IList<MenuItemInfo> items)
            {
                foreach (MenuItemInfo item in items)
                {
                    Node.Items.Add(item);
                }
                return this;
            }

        }
    }
}
