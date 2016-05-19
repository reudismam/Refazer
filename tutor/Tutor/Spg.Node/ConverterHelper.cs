using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Tutor.Spg.Node
{
    public class ConverterHelper
    {

        /// <summary>
        /// Convert a syntax tree to a TreeNode
        /// </summary>
        /// <param name="st">Syntax tree root</param>
        /// <returns>TreeNode</returns>
        public static ITreeNode<SyntaxNodeOrToken> ConvertCSharpToTreeNode(SyntaxNodeOrToken st)
        {
            if (!st.IsNode) return null;

            var list = st.AsNode().ChildNodes();
            if (!list.Any())
            {
                return new TreeNode<SyntaxNodeOrToken>(st, new TLabel(st.Kind()));
            }

            List<ITreeNode<SyntaxNodeOrToken>> children = new List<ITreeNode<SyntaxNodeOrToken>>();
            foreach (SyntaxNodeOrToken sot in st.AsNode().ChildNodes())
            {
                ITreeNode<SyntaxNodeOrToken> node = ConvertCSharpToTreeNode(sot);
                children.Add(node);
            }

            ITreeNode<SyntaxNodeOrToken> tree = new TreeNode<SyntaxNodeOrToken>(st, new TLabel(st.Kind()), children);
            return tree;
        }

        public static SyntaxNodeOrToken ConvertTreeNodeToCSsharp(ITreeNode<SyntaxNodeOrToken> treeNode)
        {
            return null;
        }

        public static ITreeNode<SyntaxNodeOrToken> MakeACopy(ITreeNode<SyntaxNodeOrToken> st)
        {
            var list = st.Children;
            if (!list.Any())
            {
                return new TreeNode<SyntaxNodeOrToken>(st.Value, st.Label);
            }

            List<ITreeNode<SyntaxNodeOrToken>> children = new List<ITreeNode<SyntaxNodeOrToken>>();
            foreach (ITreeNode<SyntaxNodeOrToken> sot in st.Children)
            {
                ITreeNode<SyntaxNodeOrToken> node = MakeACopy(sot);
                children.Add(node);
            }

            ITreeNode<SyntaxNodeOrToken> tree = new TreeNode<SyntaxNodeOrToken>(st.Value, st.Label, children);
            return tree;
        }
    }
}
