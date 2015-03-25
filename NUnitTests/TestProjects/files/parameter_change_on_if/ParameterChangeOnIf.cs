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
                return FormatLiteral((string)obj, options);
            }

            if (type == typeof(bool))
            {
                return FormatLiteral((bool)obj);
            }

            if (type == typeof(char))
            {
                return FormatLiteral((char)obj, options);
            }

            if (type == typeof(byte))
            {
                return FormatLiteral((byte)obj, options);
            }

            if (type == typeof(short))
            {
                return FormatLiteral((short)obj, options);
            }

            if (type == typeof(long))
            {
                return FormatLiteral((long)obj, options);
            }

            if (type == typeof(double))
            {
                return FormatLiteral((double)obj);
            }

            if (type == typeof(ulong))
            {
                return FormatLiteral((ulong)obj, options);
            }

            if (type == typeof(uint))
            {
                return FormatLiteral((uint)obj, options);
            }

            if (type == typeof(ushort))
            {
                return FormatLiteral((ushort)obj, options);
            }

            if (type == typeof(sbyte))
            {
                return FormatLiteral((sbyte)obj, options);
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