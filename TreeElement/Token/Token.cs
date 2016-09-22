using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ProseSample.Substrings;

namespace ProseSample.Substrings
{
    public class Token
    {
        public SyntaxKind Kind { get; set; }

        public TreeNode<SyntaxNodeOrToken> Value;

        /// <summary>
        /// Create a new token
        /// </summary>
        /// <param name="kind">Syntax Kind</param>
        /// <param name="value">Node</param>
        public Token(SyntaxKind kind, TreeNode<SyntaxNodeOrToken> value)
        {
            Kind = kind;
            Value = value;
        }

        public virtual bool IsMatch(TreeNode<SyntaxNodeOrToken> node)
        {
            return node.IsLabel(new TLabel(Kind));
        }

        public override string ToString()
        {
            return $"Token({Kind})";
        }
    }
}
