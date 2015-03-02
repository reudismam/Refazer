using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
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
            snodeMap.Add(new Node(st.SpanStart, st.Span.End, st), st);

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
                //snodeMap.Add(new Node(sot.SpanStart, sot.Span.End, sot), sot);
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

        /// <summary>
        /// Least common ancestor of nodes in the tree
        /// </summary>
        /// <param name="nodes">Nodes in the tree</param>
        /// <param name="tree">Tree</param>
        /// <returns>Least common ancestor of nodes in the tree</returns>
        public SyntaxNodeOrToken LeastCommonAncestor(List<SyntaxNode> nodes, SyntaxTree tree)
        {
            if (nodes == null) throw new ArgumentNullException("nodes");
            if (tree == null) throw new ArgumentNullException("tree");
            if (!nodes.Any()) throw new ArgumentException("Nodes cannot be empty");

            LCAManager lcaCalculator = LCAManager.GetInstance();
            SyntaxNodeOrToken lca = nodes[0];
            for (int i = 1; i < nodes.Count; i++)
            {
                SyntaxNodeOrToken node = nodes[i];
                lca = lcaCalculator.LeastCommonAncestor(tree.GetRoot(), lca, node);
            }
            return lca;
        }

        /// <summary>
        /// Least common ancestor of nodes in the tree
        /// </summary>
        /// <param name="nodes">Nodes in the tree</param>
        /// <param name="tree">Tree</param>
        /// <returns>Least common ancestor of nodes in the tree</returns>
        public SyntaxNodeOrToken LeastCommonAncestor(List<SyntaxNodeOrToken> nodes, SyntaxTree tree)
        {
            if (nodes == null) throw new ArgumentNullException("nodes");
            if (tree == null) throw new ArgumentNullException("tree");
            if (!nodes.Any()) throw new ArgumentException("Nodes cannot be empty");

            LCAManager lcaCalculator = LCAManager.GetInstance();
            SyntaxNodeOrToken lca = nodes[0];
            for (int i = 1; i < nodes.Count; i++)
            {
                SyntaxNodeOrToken node = nodes[i];
                lca = lcaCalculator.LeastCommonAncestor(tree.GetRoot(), lca, node);
            }
            return lca;
        }

        public List<SyntaxNode> LeastCommonAncestors(List<Tuple<ListNode, ListNode>> examples, string sourceCode)
        {
            List<SyntaxNode> syntaxList = new List<SyntaxNode>();
            foreach (var example in examples)
            {
                List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();

                if (example.Item2.List.Any())
                {
                    list.Add(example.Item2.List[0]);
                    list.Add(example.Item2.List[example.Item2.Length() - 1]);
                    SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);
                    SyntaxNodeOrToken snode = LeastCommonAncestor(list, tree);
                    syntaxList.Add(snode.AsNode());
                }
            }
            return syntaxList;
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
