using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace TreeElement.Spg.Node
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
                var treeNode = new TreeNode<SyntaxNodeOrToken>(st, new TLabel(st.Kind()));
                treeNode.Start = st.SpanStart;
                return treeNode;
            }

            List<ITreeNode<SyntaxNodeOrToken>> children = new List<ITreeNode<SyntaxNodeOrToken>>();
            foreach (SyntaxNodeOrToken sot in st.AsNode().ChildNodes())
            {
                ITreeNode<SyntaxNodeOrToken> node = ConvertCSharpToTreeNode(sot);
                node.Start = sot.SpanStart;
                children.Add(node);
            }

            ITreeNode<SyntaxNodeOrToken> tree = new TreeNode<SyntaxNodeOrToken>(st, new TLabel(st.Kind()), children);
            tree.Start = st.SpanStart;
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



        /// <summary>
        /// Convert a syntax tree to a TreeNode
        /// </summary>
        /// <param name="st">Syntax tree root</param>
        /// <returns>TreeNode</returns>
        public static string ConvertTreeNodeToString<T>(ITreeNode<T> st)
        {           
            var list = st.Children;
            if (!list.Any())
            {
                if (st.IsLabel(new TLabel(SyntaxKind.StringLiteralExpression)))
                {
                    var tNode = "{" + st.Label + "}";
                    return tNode;
                }
                var treeNode = "{"+st.Label+"("+st.Value.ToString().Trim()+")}";
                return treeNode;
            }
            var tree = "{"+ st.Label;
            foreach (var sot in st.Children)
            {
                var node = ConvertTreeNodeToString(sot);
                tree += node;
            }

            tree += "}";
            return tree;
        }
    }
}
