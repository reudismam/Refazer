using ExampleProject.Company.ParameterChangeIf.API;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleProject.Company.ExampleProject.Boolean_ParameterToConstantValue
{
    public static partial class ParameterToConstantValue
    {
        public static SyntaxToken Literal(int value)
        {
            return Literal(ObjectDisplay.FormatLiteral(value, useHexadecimalNumbers: false), value);
        }

        public static SyntaxToken Literal(uint value)
        {
            return Literal(ObjectDisplay.FormatLiteral(value, useHexadecimalNumbers: false) + "U", value);
        }

        public static SyntaxToken Literal(long value)
        {
            return Literal(ObjectDisplay.FormatLiteral(value, useHexadecimalNumbers: false) + "L", value);
        }

        public static SyntaxToken Literal(ulong value)
        {
            return Literal(ObjectDisplay.FormatLiteral(value, useHexadecimalNumbers: false) + "UL", value);
        }

        public static SyntaxToken Literal(float value)
        {
            return Literal(ObjectDisplay.FormatLiteral(value) + "F", value);
        }

        public static SyntaxToken Literal(double value)
        {
            return Literal(ObjectDisplay.FormatLiteral(value), value);
        }

        public static SyntaxToken Literal(decimal value)
        {
            return Literal(ObjectDisplay.FormatLiteral(value) + "M", value);
        }

        public static SyntaxToken Literal(char value)
        {
            return Literal(ObjectDisplay.FormatLiteral(value, quote: true, includeCodePoints: false, useHexadecimalNumbers: false), value);
        }

        private static SyntaxToken Literal(object p, object value)
        {
            throw new NotImplementedException();
        }
    }
}