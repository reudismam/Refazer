using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    public class Token
    {
        public SyntaxKind Kind { get; set; }

        public ITreeNode<SyntaxNodeOrToken> Value;

        /// <summary>
        /// Create a new token
        /// </summary>
        /// <param name="kind">Syntax Kind</param>
        /// <param name="value">Node</param>
        public Token(SyntaxKind kind, ITreeNode<SyntaxNodeOrToken> value)
        {
            Kind = kind;
            Value = value;
        }

        public virtual bool IsMatch(ITreeNode<SyntaxNodeOrToken> node)
        {
            return node.Value.IsKind(Kind);
        }

        public override string ToString()
        {
            return $"Token({Kind})";
        }
    }
}
