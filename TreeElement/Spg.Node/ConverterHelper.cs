using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ProseSample.Substrings;

namespace ProseSample.Substrings
{
    public class ConverterHelper
    {

        /// <summary>
        /// Convert a syntax tree to a TreeNode
        /// </summary>
        /// <param name="st">Syntax tree root</param>
        /// <returns>TreeNode</returns>
        public static TreeNode<SyntaxNodeOrToken> ConvertCSharpToTreeNode(SyntaxNodeOrToken st)
        {
            if (!Valid(st)) return null;

            var list = GetChildren(st); //st.AsNode().ChildNodes();
            if (!list.Any())
            {
                var treeNode = new TreeNode<SyntaxNodeOrToken>(st, new TLabel(st.Kind()));
                treeNode.Start = st.SpanStart;
                return treeNode;
            }

            List<TreeNode<SyntaxNodeOrToken>> children = new List<TreeNode<SyntaxNodeOrToken>>();
            foreach (SyntaxNodeOrToken sot in list)
            {
                TreeNode<SyntaxNodeOrToken> node = ConvertCSharpToTreeNode(sot);
                node.Start = sot.SpanStart;
                children.Add(node);
            }

            TreeNode<SyntaxNodeOrToken> tree = new TreeNode<SyntaxNodeOrToken>(st, new TLabel(st.Kind()), children);
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

        public static TreeNode<Token> ConvertITreeNodeToToken(TreeNode<SyntaxNodeOrToken> st)
        {
            var token = new Token(st.Value.Kind(), st);
            if (!st.Children.Any())
            {
                var dtoken = new DynToken(st.Value.Kind(), st);
                var dtreeNode = new TreeNode<Token>(dtoken, new TLabel(dtoken.Kind));
                return dtreeNode;
            }
            var children = new List<TreeNode<Token>>();
            foreach (var sot in st.Children)
            {
                var node = ConvertITreeNodeToToken(sot);
                children.Add(node);
            }
            var tree = new TreeNode<Token>(token, new TLabel(token.Kind), children);
            return tree;
        }

        public static SyntaxNodeOrToken ConvertTreeNodeToCSsharp(TreeNode<SyntaxNodeOrToken> treeNode)
        {
            return null;
        }

        public static TreeNode<T> MakeACopy<T>(TreeNode<T> st)
        {
            var list = st.Children;
            if (!list.Any())
            {
                return new TreeNode<T>(st.Value, st.Label);
            }

            List<TreeNode<T>> children = new List<TreeNode<T>>();
            foreach (TreeNode<T> sot in st.Children)
            {
                TreeNode<T> node = MakeACopy(sot);
                children.Add(node);
            }

            TreeNode<T> tree = new TreeNode<T>(st.Value, st.Label, children);
            return tree;
        }

        public static TreeNode<T> TreeAtHeight<T>(TreeNode<T> st, Dictionary<TreeNode<T>, int> dist, int height)
        {
            var list = st.Children;
            if (!list.Any() || dist[st] >= height)
            {
                return new TreeNode<T>(st.Value, st.Label);
            }

            List<TreeNode<T>> children = new List<TreeNode<T>>();
            foreach (TreeNode<T> sot in st.Children)
            {
                TreeNode<T> node = TreeAtHeight(sot, dist, height);
                children.Add(node);
            }

            TreeNode<T> tree = new TreeNode<T>(st.Value, st.Label, children);
            return tree;
        }

        /// <summary>
        /// Convert a syntax tree to a TreeNode
        /// </summary>
        /// <param name="st">Syntax tree root</param>
        /// <returns>TreeNode</returns>
        public static string ConvertTreeNodeToString<T>(TreeNode<T> st)
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
