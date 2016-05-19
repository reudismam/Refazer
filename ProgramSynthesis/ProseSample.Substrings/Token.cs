using Microsoft.CodeAnalysis.CSharp;

namespace ProseSample.Substrings
{
    public class Token
    {
        public SyntaxKind Kind { get; set; }

        public Token(SyntaxKind kind)
        {
            Kind = kind;
        }

        public override string ToString()
        {
            return $"Token({Kind})";
        }
    }
}
