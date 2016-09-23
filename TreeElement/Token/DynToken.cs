using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TreeEdit.Spg.Isomorphic;
using ProseSample.Substrings;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    public class DynToken : Token
    {
        public DynToken(SyntaxKind kind, TreeNode<SyntaxNodeOrToken> value) : base(kind, value)
        {
            //Value = value;
            if (value == null) throw new ArgumentException("value cannot be null");
        }

        public override bool IsMatch(TreeNode<SyntaxNodeOrToken> node)
        {
            return node.Value.IsKind(Kind) && IsomorphicManager<SyntaxNodeOrToken>.IsIsomorphic(Value, node);
        }

        public override string ToString()
        {
            return $"DynToken({Value})";
        }
    }
}
