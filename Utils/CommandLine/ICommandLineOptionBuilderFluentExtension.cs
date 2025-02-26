using Fclp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.CommandLine
{
    public static class ICommandLineOptionBuilderFluentExtension
    {
        public static ICommandLineOptionFluent<string> Configure(this ICommandLineOptionBuilderFluent<string> builder, string longName)
        {
            return builder.As(ParameterEnum.Short(longName), longName).WithDescription(ParameterEnum.GetDescription(longName));
        }
    }
}
