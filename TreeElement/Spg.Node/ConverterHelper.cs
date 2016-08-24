using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ProseSample.Substrings;

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
            if (!Valid(st)) return null;

            var list = GetChildren(st); //st.AsNode().ChildNodes();
            if (!list.Any())
            {
                var treeNode = new TreeNode<SyntaxNodeOrToken>(st, new TLabel(st.Kind()));
                treeNode.Start = st.SpanStart;
                return treeNode;
            }

            List<ITreeNode<SyntaxNodeOrToken>> children = new List<ITreeNode<SyntaxNodeOrToken>>();
            foreach (SyntaxNodeOrToken sot in list)
            {
                ITreeNode<SyntaxNodeOrToken> node = ConvertCSharpToTreeNode(sot);
                node.Start = sot.SpanStart;
                children.Add(node);
            }

            ITreeNode<SyntaxNodeOrToken> tree = new TreeNode<SyntaxNodeOrToken>(st, new TLabel(st.Kind()), children);
            tree.Start = st.SpanStart;
            return tree;
        }

        public static bool Valid(SyntaxNodeOrToken st)
        {
            return st.IsNode /*|| st.IsKind(SyntaxKind.IdentifierToken)*/;
        }

        private static List<SyntaxNodeOrToken> GetChildren(SyntaxNodeOrToken st)
        {
            var list = new List<SyntaxNodeOrToken>();
            foreach (var v in st.ChildNodesAndTokens())
            {
                if(Valid(v))
                { 
                    list.Add(v);
                }
            }
            return list;
        }

        public static ITreeNode<Token> ConvertITreeNodeToToken(ITreeNode<SyntaxNodeOrToken> st)
        {
            var token = new Token(st.Value.Kind(), st);
            if (!st.Children.Any())
            {
                var dtoken = new DynToken(st.Value.Kind(), st);
                var dtreeNode = new TreeNode<Token>(dtoken, new TLabel(dtoken.Kind));
                return dtreeNode;
            }
            var children = new List<ITreeNode<Token>>();
            foreach (var sot in st.Children)
            {
                var node = ConvertITreeNodeToToken(sot);
                children.Add(node);
            }
            var tree = new TreeNode<Token>(token, new TLabel(token.Kind), children);
            return tree;
        }

        public static SyntaxNodeOrToken ConvertTreeNodeToCSsharp(ITreeNode<SyntaxNodeOrToken> treeNode)
        {
            return null;
        }

        public static ITreeNode<T> MakeACopy<T>(ITreeNode<T> st)
        {
            var list = st.Children;
            if (!list.Any())
            {
                return new TreeNode<T>(st.Value, st.Label);
            }

            List<ITreeNode<T>> children = new List<ITreeNode<T>>();
            foreach (ITreeNode<T> sot in st.Children)
            {
                ITreeNode<T> node = MakeACopy(sot);
                children.Add(node);
            }

            ITreeNode<T> tree = new TreeNode<T>(st.Value, st.Label, children);
            return tree;
        }

        public static ITreeNode<T> TreeAtHeight<T>(ITreeNode<T> st, Dictionary<ITreeNode<T>, int> dist, int height)
        {
            var list = st.Children;
            if (!list.Any() || dist[st] >= height)
            {
                return new TreeNode<T>(st.Value, st.Label);
            }

            List<ITreeNode<T>> children = new List<ITreeNode<T>>();
            foreach (ITreeNode<T> sot in st.Children)
            {
                ITreeNode<T> node = TreeAtHeight(sot, dist, height);
                children.Add(node);
            }

            ITreeNode<T> tree = new TreeNode<T>(st.Value, st.Label, children);
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
                //if (st.IsLabel(new TLabel(SyntaxKind.StringLiteralExpression)))
                //{
                //    var tNode = "{" + st.Label + "}";
                //    return tNode;
                //}
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
