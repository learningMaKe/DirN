using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.CommandLine
{
    public class ParameterEnum
    {
        #region Methods
        private const string NotFoundDescription = "Parameter not found";

        public static string GetDescription(string longName)
        {
            if (Description.TryGetValue(longName, out string? value))
            {
                return value;
            }
            return NotFoundDescription;
        }

        public static char Short(string longName) => longName[0];

        #endregion

        public const string Directory = "directory";

        private static readonly Dictionary<string, string> Description = new()
        {
            { Directory, "The directory to process" }
        };
    }
}
