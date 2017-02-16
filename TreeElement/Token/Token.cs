using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TreeElement.Spg.Node;

namespace ProseFunctions.Substrings
{
    public class Token
    {
        public string Kind { get; set; }
        public const string Expression = "<exp>";

        public TreeNode<SyntaxNodeOrToken> Value;

        /// <summary>
        /// Create a new token
        /// </summary>
        /// <param name="kind">Syntax Kind</param>
        /// <param name="value">Node</param>
        public Token(string kind, TreeNode<SyntaxNodeOrToken> value)
        {
            Kind = kind;
            Value = value;
        }

        public virtual bool IsMatch(TreeNode<SyntaxNodeOrToken> node)
        {
            return IsLabel(node);

        }

        public bool IsLabel(TreeNode<SyntaxNodeOrToken> node)
        {
            return node.Value.Kind().ToString().Equals(Kind);
        }

        public override string ToString()
        {
            return $"Token({Kind})";
        }
    }
}
