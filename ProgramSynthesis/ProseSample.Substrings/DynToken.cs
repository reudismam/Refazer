using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ProseSample.Substrings
{
    public class DynToken : Token
    {
        public SyntaxNodeOrToken Value;

        public DynToken(SyntaxKind kind, SyntaxNodeOrToken value) : base(kind)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"DynToken({Value})";
        }
    }
}
