using Fclp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.CommandLine
{
    public static class ParameterParser
    {
        public static ICommandLineParserResult Parse(string[] args,out ApplicationParameter paramter)
        {
            var parser = new FluentCommandLineParser<ApplicationParameter>();

            parser.Setup(arg => arg.Directory).
                Configure(ParameterEnum.Directory);

            ICommandLineParserResult result = parser.Parse(args);
            paramter = parser.Object;
            return result;
        }
    }
}
