using System;
using Microsoft.CodeAnalysis;
using TreeEdit.Spg.Isomorphic;
using TreeElement.Spg.Node;

namespace TreeElement.Token
{
    public class DynToken : Token
    {
        public DynToken(Label label, TreeNode<SyntaxNodeOrToken> value) : base(label, value)
        {
            //Value = value;
            if (value == null) throw new ArgumentException("value cannot be null");
        }

        public override bool IsMatch(TreeNode<SyntaxNodeOrToken> node)
        {
            return IsLabel(node) && IsomorphicManager<SyntaxNodeOrToken>.IsIsomorphic(Value, node);
        }

        public override string ToString()
        {
            return $"DynToken({Value})";
        }
    }
}
