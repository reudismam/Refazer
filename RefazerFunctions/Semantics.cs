using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RefazerFunctions.Bean;
using RefazerFunctions.Substrings;
using RefazerFunctions.List;
using RefazerFunctions.Spg.Bean;
using RefazerFunctions.Spg.Semantic;
using TreeEdit.Spg.Builder;
using TreeEdit.Spg.Log;
using TreeEdit.Spg.LogInfo;
using TreeEdit.Spg.Match;
using TreeEdit.Spg.Print;
using TreeElement;
using TreeElement.Spg.Node;
using TreeElement.Token;

namespace RefazerFunctions
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class Semantics
    {
        private static readonly Dictionary<Node, Node> DicBeforeAfter = new Dictionary<Node, Node>();

        /// <summary>
        /// Matches the element on the tree with specified kind and child nodes.
        /// </summary>
        /// <param name="kind">Syntax kind</param>
        /// <param name="children">Children nodes</param>
        /// <returns>The element on the tree with specified kind and child nodes</returns>
        public static Pattern Pattern(string kind, IEnumerable<Pattern> children)
        {
            return MatchSemanticFunctions.C(new Label(kind), children);
        }

        /// <summary>
        /// Splits node in elements of kind type.
        /// </summary>
        /// <param name="node">Source node</param>
        /// <param name="kind">Syntax kind</param>
        /// <returns>Elements of kind type</returns>
        public static List<TreeNode<SyntaxNodeOrToken>> SplitToNodes(TreeNode<SyntaxNodeOrToken> node, SyntaxKind kind)
        {
            TLabel label = new TLabel(kind);
            var descendantNodes = node.DescendantNodesAndSelf();

            var kinds = from k in descendantNodes
                        where k.IsLabel(label)
                        select k;
            return kinds.ToList();
        }

        /// <summary>
        /// Return the tree
        /// </summary>
        /// <param name="variable">Result of a match result</param>
        /// <returns></returns>
        public static Pattern Tree(Pattern variable)
        {
            return variable;
        }

        /// <summary>
        /// Searches a node with kind and occurrence
        /// </summary>
        /// <param name="kind">Label</param>
        /// <returns>Search result</returns>
        public static Pattern Abstract(string kind)
        {
            return MatchSemanticFunctions.Variable(new Label(kind));
        }

        public static Pattern Context(Pattern match, string k)
        {
            var pattern = new Pattern(match.Tree, k);
            return pattern;
        }

        public static Pattern SContext(Pattern match)
        {
            var pattern = new Pattern(match.Tree, ".");
            return pattern;
        }

        public static Pattern ContextPPP(Pattern match, string k)
        {
            var pattern = new Pattern(match.Tree, k);
            return pattern;
        }

        /// <summary>
        /// Literal
        /// </summary>
        /// <param name="tree">Value</param>
        /// <returns>Literal</returns>
        public static Pattern Concrete(SyntaxNodeOrToken tree)
        {
            return MatchSemanticFunctions.Literal(tree);
        }

        public static Pattern Variable(string id)
        {
            return null;
        }

        /// <summary>
        /// Insert the newNode node as in the k position of the node in the matching result 
        /// </summary>
        /// <param name="target">Target node</param>
        /// <param name="k">Position in witch the node will be inserted.</param>
        /// <param name="newNode">Node that will be insert</param>
        /// <returns>New node with the newNode node inserted as the k child</returns>
        public static Node Insert(Node target, Node newNode, int k)
        {
            var result = EditOperationSemanticFunctions.Insert(target, newNode, k);
            if (result != null)
            {
                DicBeforeAfter.Add(result, target);
            }
            return result;
        }

        /// <summary>
        /// Insert the newNode node as in the k position of the node in the matching result 
        /// </summary>
        /// <param name="target">Target node</param>
        /// <param name="node">Input data</param>
        /// <param name="newNode">Node that will be insert</param>
        /// <returns>New node with the newNode node inserted as the k child</returns>
        public static Node InsertBefore(Node target, Node node, Node newNode)
        {
            var result = EditOperationSemanticFunctions.InsertBefore(target, node, newNode);
            DicBeforeAfter.Add(result, target);
            return result;
        }

        /// <summary>
        /// Update edit operation
        /// </summary>
        /// <param name="target">Target node</param>
        /// <param name="to">New value</param>
        public static Node Update(Node target, Node to)
        {
            var result = EditOperationSemanticFunctions.Update(target, to);
            DicBeforeAfter.Add(result, target);
            return result;
        }

        /// <summary>
        /// Delete edit operation
        /// </summary>
        /// <param name="target">target</param>
        /// <param name="node">Input node</param>
        /// <returns>Result of the edit operation</returns>
        public static Node Delete(Node target, Node node)
        {
            var result = EditOperationSemanticFunctions.Delete(target, node);
            DicBeforeAfter.Add(result, target);
            return result;
        }

        /// <summary>
        /// Script semantic function
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="patch">Edit operations</param>
        /// <returns>Transformed node.</returns>
        [SuppressMessage("ReSharper", "LoopCanBePartlyConvertedToQuery")]
        public static IEnumerable<Node> Transformation(Node node, Patch patch)
        {
            var beforeFlorest = patch.Edits.Select(o => o.ToList());

            var resultList = new List<Node>();
            foreach (var edited in beforeFlorest)
            {
                foreach (var v in edited)
                {
                    if (v == null) continue; //if we are unable to transformation an location
                    if (!v.Value.IsLabel(new TLabel(SyntaxKind.None)))
                    {
                        var before = DicBeforeAfter[v];

                        SyntaxNodeOrToken n;
                        if (v.LeftNode != null)
                        {
                            n = ASTBuilder.ReconstructTree(node.Value.Value, v.LeftNode.Value);
                        }
                        else if (v.RightNode != null)
                        {
                            n = ASTBuilder.ReconstructTree(node.Value.Value, v.RightNode.Value);
                        }
                        else
                        {
                            PrintUtil<SyntaxNodeOrToken>.PrintPrettyDebug(v.Value, "", false);
                            n = ASTBuilder.ReconstructTree(node.Value.Value, v.Value);
                        }
                        TransformationsInfo.GetInstance().Add(Tuple.Create(before.Value.Value, n));
                        resultList.Add(new Node(ConverterHelper.ConvertCSharpToTreeNode(n)));
                    }
                    else
                    {
                        var before = DicBeforeAfter[v];
                        var n = ASTBuilder.ReconstructTree(node.Value.Value, v.Value);
                        TransformationsInfo.GetInstance().Add(Tuple.Create(before.Value.Value, n));
                        var treeNode = new TreeNode<SyntaxNodeOrToken>(n,
                            new TLabel(SyntaxKind.None));
                        resultList.Add(new Node(treeNode));
                    }
                }
            }
            return resultList;
        }

        /// <summary>
        /// Return a new node
        /// </summary>
        /// <param name="kind">Returned node SyntaxKind</param>
        /// <param name="childrenNodes">Children nodes</param>
        /// <returns>A new node with kind and child</returns>
        public static Node Node(SyntaxKind kind, IEnumerable<Node> childrenNodes)
        {
            var childrenList = (List<Node>)childrenNodes;
            if (!childrenList.Any()) return null;
            TreeNode<SyntaxNodeOrToken> parent = new TreeNode<SyntaxNodeOrToken>(null, new TLabel(kind));
            SyntaxNodeOrToken nodevalue = null;

            if (childrenList.Any(o => o.Value.IsLabel(new TLabel(SyntaxKind.None))))
            {
                var treeNode = new TreeNode<SyntaxNodeOrToken>(default(SyntaxNodeOrToken), new TLabel(SyntaxKind.None));
                return new Node(treeNode);
            }
            for (int i = 0; i < childrenList.Count(); i++)
            {
                var child = childrenList.ElementAt(i).Value;
                parent.AddChild(child, i);
                if (child.Value.Parent.IsKind(kind))
                {
                    nodevalue = child.Value.Parent;
                }
            }
            if (nodevalue != null)
            {
                var copy = ConverterHelper.MakeACopy(ConverterHelper.ConvertCSharpToTreeNode(nodevalue));
                copy.Children = parent.Children;
                var node = new Node(copy);
                return node;
            }
            else
            {
                var node = new Node(parent);
                return node;
            }
        }

        /// <summary>
        /// Create a constant node
        /// </summary>
        /// <param name="cst">Constant</param>
        /// <returns>A new constant node.</returns>
        public static Node ConstNode(SyntaxNodeOrToken cst)
        {
            var parent = new TreeNode<SyntaxNodeOrToken>(cst.Parent, new TLabel(cst.Parent.Kind()));
            var itreeNode = new TreeNode<SyntaxNodeOrToken>(cst, new TLabel(cst.Kind()));
            itreeNode.Parent = parent;
            var node = new Node(itreeNode);
            return node;
        }

        public static string ConstNode(string cst)
        {
            return null;
        }

        public static IEnumerable<Pattern> CList(Pattern child1, IEnumerable<Pattern> cList)
        {
            return GList<Pattern>.List(child1, cList);
        }

        public static IEnumerable<Pattern> SC(Pattern child)
        {
            return GList<Pattern>.Single(child);
        }

        public static IEnumerable<Pattern> PList(Pattern child1, IEnumerable<Pattern> cList)
        {
            return GList<Pattern>.List(child1, cList);
        }

        public static IEnumerable<Pattern> SP(Pattern child)
        {
            return GList<Pattern>.Single(child);
        }

        public static IEnumerable<Node> NList(Node child1, IEnumerable<Node> cList)
        {
            return GList<Node>.List(child1, cList);
        }

        public static IEnumerable<Node> SN(Node child)
        {
            return GList<Node>.Single(child);
        }

        public static Patch EList(IEnumerable<Node> child1, Patch cList)
        {
            var editList = GList<IEnumerable<Node>>.List(child1, cList.Edits).ToList();
            var patch = new Patch(editList);
            return patch;
        }

        public static Patch SE(IEnumerable<Node> child)
        {
            var editList = GList<IEnumerable<Node>>.Single(child).ToList();
            var patch = new Patch(editList);
            return patch;
        }

        public static IEnumerable<SyntaxNodeOrToken> SplitNodes(SyntaxNodeOrToken n)
        {
            SyntaxKind targetKind = SyntaxKind.MethodDeclaration;
            SyntaxNode node = n.AsNode();
            var nodes = from snode in node.DescendantNodes()
                        where snode.IsKind(targetKind)
                        select snode;

            return nodes.Select(snot => (SyntaxNodeOrToken)snot).ToList();
        }

        public static bool Match(Node sx, Pattern template)
        {
            var patternP = template;
            var parent = FindParent(sx.Value, patternP.XPath);
            if (parent == null) return false;
            var isValue = MatchManager.IsValueEachChild(parent, template.Tree);
            if (!isValue) return false;

            var node = FindChild(parent, patternP.XPath);
            if (node == null) return false; //the XPath matches nothing.
            var isValid = node.Equals(sx.Value);
            if (isValid)
            {
                var codeFragmentsLogger = CodeFragmentsInfo.GetInstance();
                codeFragmentsLogger.Add(node.Value);
            }
            return isValid;
        }

        private static TreeNode<SyntaxNodeOrToken> FindParent(TreeNode<SyntaxNodeOrToken> value, string s)
        {
            var matches = Regex.Matches(s, "[0-9]");
            var current = value;
            foreach (var match in matches)
            {
                if (current == null) return null;
                current = current.Parent;
            }
            return current;
        }

        public static TreeNode<T> FindChild<T>(TreeNode<T> parent, string s)
        {
            var matches = Regex.Matches(s, "[0-9]");
            var current = parent;
            foreach (Match match in matches)
            {
                var index = Int32.Parse(match.Groups[0].Value);
                if (index > current.Children.Count) return null;
                current = current.Children[index - 1];
            }
            return current;
        }

        /// <summary>
        /// Defines semantic functions for the Reference operator.
        /// </summary>
        /// <param name="target">Target node</param>
        /// <param name="kmatch">Pattern</param>
        /// <param name="k">Index of the match</param>
        public static Node Reference(Node target, Pattern kmatch, int k)
        {
            var patternP = kmatch;
            if (k >= 0)
            {
                var nodes = MatchManager.Matches(target.Value, kmatch.Tree);
                if (!nodes.Any())
                {
                    var treeNode = new TreeNode<SyntaxNodeOrToken>(default(SyntaxNodeOrToken), new TLabel(SyntaxKind.None));
                    return new Node(treeNode);
                }
                if (nodes.Count < k) return null; //not enough nodes
                var match = nodes.ElementAt(k - 1);
                var node = FindChild(match, patternP.XPath);
                return new Node(node);
            }
            else
            {
                var ancestor = ConverterHelper.ConvertCSharpToTreeNode(target.Value.Value.Parent.Parent);
                var nodes = MatchManager.Matches(ancestor, kmatch.Tree);
                if (nodes.Any())
                {
                    var matches = MatchManager.Matches(ancestor, kmatch.Tree, target.Value);
                    matches = matches.OrderByDescending(o => o.Start).ToList();
                    if (matches.Any())
                    {
                        var match = matches.ElementAt(Math.Abs(k) - 1);
                        var node = FindChild(match, patternP.XPath);
                        return new Node(node);
                    }
                }
                var treeNode = new TreeNode<SyntaxNodeOrToken>(default(SyntaxNodeOrToken), new TLabel(SyntaxKind.None));
                return new Node(treeNode);
            }
        }

        public static IEnumerable<Node> AllNodes(Node node, string type)
        {
            var traversal = new TreeTraversal<SyntaxNodeOrToken>();
            var itreenode = node.Value;
            var nodes = traversal.PostOrderTraversal(itreenode).ToList();
            var result = new List<Node>();
            foreach (var n in nodes)
            {
                result.Add(new Node(n));
            }
            return result;
        }
    }
}
