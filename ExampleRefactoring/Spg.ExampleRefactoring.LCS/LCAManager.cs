using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeastCommonAncestor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ExampleRefactoring.Spg.ExampleRefactoring.LCS
{
    public class LCAManager
    {

        private static LCAManager instance;

        Dictionary<string, Tuple<Dictionary<Node, SyntaxNodeOrToken>, LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken>>> dic = new Dictionary<string,Tuple<Dictionary<Node,SyntaxNodeOrToken>,LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken>>>();
        private Dictionary<Node, SyntaxNodeOrToken> snodeMap; 

        private LCAManager()
        {
        }

        public static LCAManager GetInstance()
        {
            if (instance == null)
            {
                instance = new LCAManager();
            }
            return instance;
        }

        public LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken> ConvertToTreeNode(SyntaxNode sn)
        {
            Tuple<Dictionary<Node, SyntaxNodeOrToken>, LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken>> value;
            var str = sn.ToFullString();
            if (!dic.TryGetValue(str, out value))
            {
                snodeMap = new Dictionary<Node, SyntaxNodeOrToken>();
                LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken> tree = _ConvertToTreeNode(sn);
                value = Tuple.Create(snodeMap, tree);
                dic.Add(str, value);
            }
            
            LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken>  itree = dic[sn.ToFullString()].Item2;
            return itree;
        }

        internal LCA<SyntaxNodeOrToken>.ITreeNode<SyntaxNodeOrToken> Find(SyntaxNodeOrToken root, SyntaxNodeOrToken sot)
        {
            Node node = new Node(sot.SpanStart, sot.Span.End, sot);
            SyntaxNodeOrToken sn=  dic[root.ToFullString()].Item1[node];
            LCA<SyntaxNodeOrToken>.ITreeNode<SyntaxNodeOrToken> it = new LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken>(sn);
            return it;
        }


        /// <summary>
        /// Convert a syntax tree to a TreeNode
        /// </summary>
        /// <param name="st">Syntax tree root</param>
        /// <returns>TreeNode</returns>
        private LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken> _ConvertToTreeNode(SyntaxNodeOrToken st)
        {
            if (st.ChildNodesAndTokens().Count == 0)
            {
                return new LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken>(st);
            }

            List<LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken>> children = new List<LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken>>();
            foreach (SyntaxNodeOrToken sot in st.ChildNodesAndTokens())
            {
                //childrens.Add(sot);
                LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken> node = _ConvertToTreeNode(sot);
                children.Add(node);
                snodeMap.Add(new Node(sot.SpanStart, sot.Span.End, sot), sot);
            }

            LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken> tree = new LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken>(st, children.ToArray());
            return tree;
        }

        public SyntaxNode LeastCommonAncestor(SyntaxNodeOrToken root, SyntaxNodeOrToken n1, SyntaxNodeOrToken n2)
        {
            LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken> rootNode = ConvertToTreeNode(root.AsNode());
            LCA<SyntaxNodeOrToken>.ITreeNode<SyntaxNodeOrToken> x = Find(root, n1);
            LCA<SyntaxNodeOrToken>.ITreeNode<SyntaxNodeOrToken> y = Find(root, n2);
            LCA<SyntaxNodeOrToken> lca = new LCA<SyntaxNodeOrToken>();
            return lca.LeastCommonAncestor(root.ToFullString(), rootNode, x, y).AsNode();
        }

        public class Node
        {
            public SyntaxNodeOrToken Snt;
            public int Start { get; set; }

            public int End { get; set; }

            public SyntaxKind SyntaxKind { get; set; }

            public Node(int start, int end, SyntaxNodeOrToken snt)
            {
                this.Start = start;
                this.End = end;
                this.SyntaxKind = snt.CSharpKind();
                this.Snt = snt;
            }
            public override bool Equals(object obj)
            {
                if (!(obj is Node))
                {
                    return false;
                }
                Node other = obj as Node;
                return other.Start == this.Start && other.End == this.End && other.SyntaxKind == this.SyntaxKind;
            }

            public override string ToString()
            {
                return Snt.ToFullString();
            }

            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }
        }
    }
}
