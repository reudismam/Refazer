using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TreeElement.Spg.Node;

namespace TreeElement.Token
{
    public class Token
    {
        public Label Label { get; set; }

        public const string Expression = "<exp>";

        public TreeNode<SyntaxNodeOrToken> Value;

        /// <summary>
        /// Create a new token
        /// </summary>
        /// <param name="label">Syntax Label</param>
        /// <param name="value">Node</param>
        public Token(Label label, TreeNode<SyntaxNodeOrToken> value)
        {
            Label = label;
            Value = value;
        }

        public virtual bool IsMatch(TreeNode<SyntaxNodeOrToken> node)
        {
            return IsLabel(node);

        }

        public bool IsLabel(TreeNode<SyntaxNodeOrToken> node)
        {
            var otherLabel = new Label(node.Value.Kind().ToString());
            return otherLabel.Equals(Label);
        }

        public override string ToString()
        {
            return $"Token({Label})";
        }
    }
}
