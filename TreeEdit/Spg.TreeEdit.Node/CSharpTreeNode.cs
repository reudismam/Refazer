//using System.Collections.Generic;
//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;

//namespace Spg.TreeEdit.Node
//{
//    public class CSharpTreeNode : TreeNode<SyntaxNodeOrToken>
//    {
//        public CSharpTreeNode(SyntaxNodeOrToken value, List<ITreeNode<SyntaxNodeOrToken>> children) : base(value, children)
//        {
//        }

//        public CSharpTreeNode(SyntaxNodeOrToken value) : base(value)
//        {
//        }

//        public override bool Equals(object obj)
//        {
//            if (!(obj is SyntaxNodeOrToken)) return false;

//            var other = (SyntaxNodeOrToken) obj;

//            return Value.IsKind(other.Kind()) && Value.Span.CompareTo(other.Span) == 0;
//        }
//    }
//}
