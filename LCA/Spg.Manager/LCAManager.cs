using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace LCA.Spg.Manager
{
    /// <summary>
    /// LCA manager
    /// </summary>
    public class LCAManager
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static LCAManager _instance;

        /// <summary>
        /// Pre computed trees
        /// </summary>
        readonly Dictionary<string, Tuple<Dictionary<Node, SyntaxNodeOrToken>, LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken>>> _dic = new Dictionary<string, Tuple<Dictionary<Node, SyntaxNodeOrToken>, LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken>>>();

        /// <summary>
        /// Mapping between node and syntax node
        /// </summary>
        private Dictionary<Node, SyntaxNodeOrToken> _snodeMap; 

        private LCAManager()
        {
        }

        /// <summary>
        /// Initiate a new instance
        /// </summary>
        public static void Init()
        {
            _instance = null;
            LCA<SyntaxNodeOrToken>.LeastCommonAncestorFinder<SyntaxNodeOrToken>.Init();
        }

        /// <summary>
        /// Return a singleton instance
        /// </summary>
        /// <returns></returns>
        public static LCAManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LCAManager();
            }
            return _instance;
        }

        /// <summary>
        /// Convert node to tree
        /// </summary>
        /// <param name="sn">Syntax node root</param>
        /// <returns></returns>
        public LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken> ConvertToTreeNode(SyntaxNode sn)
        {
            Tuple<Dictionary<Node, SyntaxNodeOrToken>, LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken>> value;
            var str = sn.ToFullString();
            if (!_dic.TryGetValue(str, out value))
            {
                _snodeMap = new Dictionary<Node, SyntaxNodeOrToken>();
                LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken> tree = _ConvertToTreeNode(sn);
                value = Tuple.Create(_snodeMap, tree);
                _dic.Add(str, value);
            }

            LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken>  itree = _dic[sn.ToFullString()].Item2;
            return itree;
        }

        /// <summary>
        /// Find specific node
        /// </summary>
        /// <param name="root">Root node</param>
        /// <param name="sot">Syntax node or token</param>
        /// <returns>Specified node</returns>
        internal LCA<SyntaxNodeOrToken>.ITreeNode<SyntaxNodeOrToken> Find(SyntaxNodeOrToken root, SyntaxNodeOrToken sot)
        {
            Node node = new Node(sot.SpanStart, sot.Span.End, sot);
            SyntaxNodeOrToken sn=  _dic[root.ToFullString()].Item1[node];
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
            Node key = new Node(st.SpanStart, st.Span.End, st);
            if (!_snodeMap.ContainsKey(key))
            {
                _snodeMap.Add(key, st);
            }
            if (st.ChildNodesAndTokens().Count == 0)
            {
                return new LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken>(st);
            }
            List<LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken>> children = new List<LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken>>();
            foreach (SyntaxNodeOrToken sot in st.ChildNodesAndTokens())
            {
                LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken> node = _ConvertToTreeNode(sot);
                children.Add(node);
            }
            LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken> tree = new LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken>(st, children.ToArray());
            return tree;
        }

        /// <summary>
        /// Least common ancestor
        /// </summary>
        /// <param name="root">Root node</param>
        /// <param name="n1">First node to be analyzed</param>
        /// <param name="n2">Second node to be analyzed</param>
        /// <returns>Least common ancestor of n1 and n2</returns>
        public SyntaxNode LeastCommonAncestor(SyntaxNodeOrToken root, SyntaxNodeOrToken n1, SyntaxNodeOrToken n2)
        {
            LCA<SyntaxNodeOrToken>.TreeNode<SyntaxNodeOrToken> rootNode = ConvertToTreeNode(root.AsNode());
            LCA<SyntaxNodeOrToken>.ITreeNode<SyntaxNodeOrToken> x = Find(root, n1);
            LCA<SyntaxNodeOrToken>.ITreeNode<SyntaxNodeOrToken> y = Find(root, n2);
            LCA<SyntaxNodeOrToken> lca = new LCA<SyntaxNodeOrToken>();
            return lca.LeastCommonAncestor(root.ToFullString(), rootNode, x, y).AsNode();
        }

        public SyntaxNodeOrToken LeastCommonAncestor(List<SyntaxNodeOrToken> nodes, SyntaxNodeOrToken tree)
        {
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));
            if (tree == null) throw new ArgumentNullException(nameof(tree));
            if (!nodes.Any()) throw new ArgumentException("Nodes cannot be empty");

            LCAManager lcaCalculator = GetInstance();
            SyntaxNodeOrToken lca = nodes[0];
            for (int i = 1; i < nodes.Count; i++)
            {
                SyntaxNodeOrToken node = nodes[i];
                lca = lcaCalculator.LeastCommonAncestor(tree, lca, node);
            }
            return lca;
        }

        /// <summary>
        /// Class node
        /// </summary>
        public class Node
        {
            /// <summary>
            /// Syntax node or token reference
            /// </summary>
            public SyntaxNodeOrToken Snt;

            /// <summary>
            /// Start position
            /// </summary>
            public int Start { get; set; }

            /// <summary>
            /// End position
            /// </summary>
            public int End { get; set; }

            /// <summary>
            /// SyntaxKind
            /// </summary>
            public SyntaxKind SyntaxKind { get; set; }

            /// <summary>
            /// Create a new Node instance
            /// </summary>
            /// <param name="start">Start position</param>
            /// <param name="end">End position</param>
            /// <param name="snt">Syntax node of token reference</param>
            public Node(int start, int end, SyntaxNodeOrToken snt)
            {
                this.Start = start;
                this.End = end;
                this.SyntaxKind = snt.Kind();
                this.Snt = snt;
            }

            /// <summary>
            /// Determine if obj is equal to this.
            /// </summary>
            /// <param name="obj">Object</param>
            /// <returns>True is obj is equal to this.</returns>
            public override bool Equals(object obj)
            {
                if (!(obj is Node))
                {
                    return false;
                }
                Node other = (Node) obj;
                return other.Start == this.Start && other.End == this.End && other.SyntaxKind == this.SyntaxKind;
            }

            /// <summary>
            /// String representation of this node.
            /// </summary>
            /// <returns>String representation of this node.</returns>
            public override string ToString()
            {
                return Snt.ToFullString();
            }

            /// <summary>
            /// Hash code for this node.
            /// </summary>
            /// <returns>Hash code for this node.</returns>
            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }
        }
    }
}


