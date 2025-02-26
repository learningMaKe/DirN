using Fclp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace DirN.Utils.CommandLine
{
    public class ApplicationParameter:BindableBase
    {
        public string Directory { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);


    }
}
