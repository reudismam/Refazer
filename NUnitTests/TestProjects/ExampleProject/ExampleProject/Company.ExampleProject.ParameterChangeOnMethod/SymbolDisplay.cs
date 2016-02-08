using ExampleProject.Company.ExampleProject.Boolean_ParameterToConstantValue;
using ExampleProject.Company.ParameterChangeIf.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleProject.Company.ExampleProject.ParameterChangeOnMethod
{
    public class SymbolDisplay
    {
        public static string FormatLiteral(string value, bool quote)
        {
            return ObjectDisplay.FormatLiteral(value, quote);
        }

        public static string FormatLiteral(char c, bool quote)
        {
            return ObjectDisplay.FormatLiteral(c, quote);
        }
    }
}
