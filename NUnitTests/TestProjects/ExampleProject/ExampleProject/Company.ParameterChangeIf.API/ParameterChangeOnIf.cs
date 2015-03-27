using ExampleProject.Company.ParameterChangeIf.API;
using System;

namespace Microsoft.CodeAnalysis.CSharp
{
    internal static class ObjectDisplay
    {
        public static string FormatPrimitive(object obj, ObjectDisplayOptions options)
        {
            if (obj == null)
            {
                return NullLiteral;
            }

            Type type = obj.GetType();
            if (type.GetType().IsEnum)
            {
                type = Enum.GetUnderlyingType(type);
            }

            if (type == typeof(string))
            {
                return FormatLiteral((string)obj, quoteStrings);
            }

            if (type == typeof(bool))
            {
                return FormatLiteral((bool)obj);
            }

            if (type == typeof(char))
            {
                return FormatLiteral((char)obj, quoteStrings);
            }

            if (type == typeof(byte))
            {
                return FormatLiteral((byte)obj, useHexadecimalNumbers);
            }

            if (type == typeof(short))
            {
                return FormatLiteral((short)obj, useHexadecimalNumbers);
            }

            if (type == typeof(long))
            {
                return FormatLiteral((long)obj, useHexadecimalNumbers);
            }

            if (type == typeof(double))
            {
                return FormatLiteral((double)obj);
            }

            if (type == typeof(ulong))
            {
                return FormatLiteral((ulong)obj, useHexadecimalNumbers);
            }

            if (type == typeof(uint))
            {
                return FormatLiteral((uint)obj, useHexadecimalNumbers);
            }

            if (type == typeof(ushort))
            {
                return FormatLiteral((ushort)obj, useHexadecimalNumbers);
            }

            if (type == typeof(sbyte))
            {
                return FormatLiteral((sbyte)obj, useHexadecimalNumbers);
            }

            if (type == typeof(float))
            {
                return FormatLiteral((float)obj);
            }

            if (type == typeof(decimal))
            {
                return FormatLiteral((decimal)obj);
            }

            return null;
        }

        private static string FormatLiteral(object p, object useHexadecimalNumbers)
        {
            throw new System.NotImplementedException();
        }

        public static string FormatLiteral(object obj)
        {
            return "obj";
        }

        public static object useHexadecimalNumbers
        {
            get;
            set;
        }

        public static object quoteStrings
        {
            get;
            set;
        }

        public static string NullLiteral
        {
            get;
            set;
        }

        internal static int FormatLiteral(int value, bool useHexadecimalNumbers)
        {
            throw new NotImplementedException();
        }
    }
}