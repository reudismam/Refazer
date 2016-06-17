using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProseSample.Substrings.List;
using ProseSample.Substrings.Spg.Semantic;
using TreeEdit.Spg.Match;
using TreeEdit.Spg.TreeEdit.Update;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    public static class Semantics
    {
        /// <summary>
        /// Store the tree update associate to each node
        /// </summary>
        public static readonly Dictionary<SyntaxNodeOrToken, TreeUpdate> TreeUpdateDictionary = new Dictionary<SyntaxNodeOrToken, TreeUpdate>();

        //Before transformation mapping
        private static readonly Dictionary<SyntaxNodeOrToken, List<SyntaxNodeOrToken>> BeforeAfterMapping = new Dictionary<SyntaxNodeOrToken, List<SyntaxNodeOrToken>>();

        //After transformation mapping
        private static readonly Dictionary<SyntaxNodeOrToken, List<SyntaxNodeOrToken>> MappingRegions = new Dictionary<SyntaxNodeOrToken, List<SyntaxNodeOrToken>>();

        /// <summary>
        /// Matches the element on the tree with specified kind and child nodes.
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="kind">Syntax kind</param>
        /// <param name="children">Children nodes</param>
        /// <returns>The element on the tree with specified kind and child nodes</returns>
        public static MatchResult C(SyntaxNodeOrToken node, SyntaxKind kind, IEnumerable<MatchResult> children)
        {
            return Match.C(node, kind, children);
        }


        /// <summary>
        /// Match function. This function matches the first element on the tree that has the specified kind and child nodes.
        /// </summary>
        /// <param name="kind">Syntax kind</param>
        /// <param name="children">Children nodes</param>
        /// <returns> Returns the first element on the tree that has the specified kind and child nodes.</returns>
        public static Pattern P(SyntaxKind kind, IEnumerable<Pattern> children)
        {
            var pchildren = children.Select(child => child.Tree).ToList();

            var token = new Token(kind);
            var inode = new TreeNode<Token>(token, null, pchildren);
            var pattern = new Pattern(inode);
            return pattern;
        }

        /// <summary>
        /// Splits the source node in the elements of type kind.
        /// </summary>
        /// <param name="node">Source node</param>
        /// <param name="kind">Syntax kind</param>
        /// <returns></returns>
        public static List<ITreeNode<SyntaxNodeOrToken>> SplitToNodes(ITreeNode<SyntaxNodeOrToken> node, SyntaxKind kind)
        {
            TLabel label = new TLabel(kind);
            var descendantNodes = node.DescendantNodesAndSelf();
            var kinds = from k in descendantNodes
                        where k.IsLabel(label)
                        select k;

            return kinds.ToList();
        }

        ///// <summary>
        ///// Verify if the parent contains the parameter child
        ///// </summary>
        ///// <param name="parent">Parent node</param>
        ///// <param name="child">Child node</param>
        ///// <returns>True, if parent contains the child, false otherwise.</returns>
        //private static bool MatchChildren(SyntaxNodeOrToken parent, SyntaxNodeOrToken child)
        //{
        //    foreach (var item in parent.ChildNodesAndTokens())
        //    {
        //        if (item.IsKind(child.Kind()))
        //        {
        //            return true;
        //        }

        //        if (child.IsKind(SyntaxKind.IdentifierToken) || child.IsKind(SyntaxKind.IdentifierName) ||
        //            (child.IsKind(SyntaxKind.NumericLiteralToken) || child.IsKind(SyntaxKind.NumericLiteralExpression))
        //            || (child.IsKind(SyntaxKind.StringLiteralToken) || child.IsKind(SyntaxKind.StringLiteralExpression)))
        //        {
        //            string itemString = item.ToString();
        //            string childString = child.ToString();
        //            if (itemString.Equals(childString))
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        /// <summary>
        /// Build a literal
        /// </summary>
        /// <param name="lookFor">Literal itself.</param>
        /// <returns>Match of parameter literal in the source code.</returns>
        public static Pattern Concrete(SyntaxNodeOrToken lookFor)
        {
            var token = new DynToken(lookFor.Kind(), lookFor);
            var label = new TLabel(lookFor.Kind());
            var inode = new TreeNode<Token>(token, label);
            var pattern = new Pattern(inode);
            return pattern;
        }

        public static Pattern Abstract(SyntaxKind kind)
        {
            var token = new Token(kind);
            var inode = new TreeNode<Token>(token, null);
            var pattern = new Pattern(inode);
            return pattern;
        }

        /// <summary>
        /// Build a literal
        /// </summary>
        /// <param name="node">Input node</param>
        /// <param name="lookFor">Literal itself.</param>
        /// <param name="k">Index</param>
        /// <returns>Match of parameter literal in the source code.</returns>
        public static MatchResult Literal(SyntaxNodeOrToken node, SyntaxNodeOrToken lookFor, int k)
        {
            var currentTree = GetCurrentTree(node);
            var matches = MatchManager.ConcreteMatches(currentTree, lookFor);
            if (matches.Count >= k)
            {
                var item = matches.ElementAt(k - 1);
                var match = Tuple.Create<ITreeNode<SyntaxNodeOrToken>, Bindings>(item, null);
                MatchResult matchResult = new MatchResult(match);
                matchResult.Type = MatchResult.Literal;
                return matchResult;
            }

            return null;
        }

        /// <summary>
        /// Insert the ast node as in the k position of the node in the matching result 
        /// </summary>
        /// <param name="node">Input data</param>
        /// <param name="k">Position in witch the node will be inserted.</param>
        /// <param name="mresult">Matching result</param>
        /// <param name="ast">Node that will be insert</param>
        /// <returns>New node with the ast node inserted as the k child</returns>
        public static SyntaxNodeOrToken Insert(SyntaxNodeOrToken node, MatchResult mresult, Node ast, int k)
        {
            return EditOperation.Insert(node, mresult, ast, k);
        }

        /// <summary>
        /// Move the from node such that it is the k child of the node
        /// </summary>
        /// <param name="node">Source node</param>
        /// <param name="k">Child index</param>
        /// <param name="parent">Parent</param>
        /// <param name="from">Moved node</param>
        /// <returns></returns>
        public static SyntaxNodeOrToken Move(SyntaxNodeOrToken node, MatchResult parent, MatchResult from, int k)
        {
            return EditOperation.Move(node, parent, from, k);
        }

        public static SyntaxNodeOrToken Update(SyntaxNodeOrToken node, MatchResult from, Node to)
        {
            return EditOperation.Update(node, from, to);
        }

        public static SyntaxNodeOrToken Delete(SyntaxNodeOrToken node, MatchResult delete)
        {
            return EditOperation.Delete(node, delete);
        }

        public static MatchResult Tree(MatchResult variable)
        {
            return variable;
        }

        public static MatchResult Variable(SyntaxNodeOrToken node, SyntaxKind kind, int k)
        {
            var currentTree = GetCurrentTree(node);

            var matches = SplitToNodes(currentTree, kind);

            if (matches.Any())
            {
                var child = matches.ElementAt(k - 1);
                var result = new MatchResult(Tuple.Create(child, new Bindings(new List<SyntaxNodeOrToken> { child.Value })));
                result.Type = MatchResult.Variable;
                return result;
            }
            return null;
        }

        public static MatchResult Parent(SyntaxNodeOrToken node, MatchResult variable, int k)
        {
            var currentTree = GetCurrentTree(node);
            var child = TreeUpdate.FindNode(currentTree, variable.Match.Item1.Value).Children.ElementAt(k - 1);
            var result = new MatchResult(Tuple.Create(child, new Bindings(new List<SyntaxNodeOrToken> { child.Value })));
            return result;
        }

        public static MatchResult RightChild(SyntaxNodeOrToken node, MatchResult variable)
        {
            var currentTree = GetCurrentTree(node);
            var position = Spg.Witness.RightChild.NodePosition(variable.Match.Item1);
            if (position + 1 >= variable.Match.Item1.Parent.Children.Count)
            {
                return null;
            }
            var child = TreeUpdate.FindNode(currentTree, variable.Match.Item1.Parent.Children.ElementAt(position + 1).Value);
            var result = new MatchResult(Tuple.Create(child, new Bindings(new List<SyntaxNodeOrToken> { child.Value })));
            return result;
        }


        public static MatchResult Child(SyntaxNodeOrToken node, MatchResult variable)
        {
            var currentTree = GetCurrentTree(node);
            var child = TreeUpdate.FindNode(currentTree, variable.Match.Item1.Value);
            var result = new MatchResult(Tuple.Create(child.Parent, new Bindings(new List<SyntaxNodeOrToken> { child.Parent.Value })));
            return result;
        }

        public static ITreeNode<SyntaxNodeOrToken> GetCurrentTree(SyntaxNodeOrToken n)
        {
            if (!TreeUpdateDictionary.ContainsKey(n))
            {
                TreeUpdate update = new TreeUpdate(n);
                TreeUpdateDictionary[n] = update;
            }
            var node = TreeUpdateDictionary[n].CurrentTree;

            return node;
        }

        /// <summary>
        /// Script semantic function
        /// </summary>
        /// <param name="node">Input node</param>
        /// <param name="editOperations">Edit operations</param>
        /// <returns>Transformed node.</returns>
        public static SyntaxNodeOrToken Script(SyntaxNodeOrToken node, IEnumerable<SyntaxNodeOrToken> editOperations)
        {
            var current = GetCurrentTree(node);

            var afterFlorest = current.Children.Select(ReconstructTree).ToList();

            //var beforeFlorest = current.Children.Select(o => o.Value).ToList();

            //BeforeAfterMapping[node] = beforeFlorest;
            MappingRegions[node] = afterFlorest;

            return node;
        }

        /// <summary>
        /// Return a new node
        /// </summary>
        /// <param name="kind">Returned node SyntaxKind</param>
        /// <param name="childrenNodes">Children nodes</param>
        /// <returns>A new node with kind and child</returns>
        public static Node Node(SyntaxKind kind, IEnumerable<Node> childrenNodes)
        {
            var childrenList = (List<Node>) childrenNodes;

            if (!childrenList.Any()) return null;

            var parent = childrenNodes.First().Value.Parent;

            for (int i = 0; i < childrenList.Count(); i++)
            {
                var child = childrenList.ElementAt(i).Value;
                parent.AddChild(child, i);
            }

            var node = new Node(parent);
            return node;
        }

        /// <summary>
        /// Create a constant node
        /// </summary>
        /// <param name="cst">Constant</param>
        /// <returns>A new constant node.</returns>
        public static Node Const(SyntaxNodeOrToken cst)
        {
            var parent = new TreeNode<SyntaxNodeOrToken>(cst.Parent, new TLabel(cst.Parent.Kind()));
            var itreeNode = new TreeNode<SyntaxNodeOrToken>(cst, new TLabel(cst.Kind()));
            itreeNode.Parent = parent;
            var node = new Node(itreeNode);
            return node;
        }

        /// <summary>
        /// Reference semantic function
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="result">Result of the pattern</param>
        /// <returns>Result of the pattern</returns>
        public static Node Ref(SyntaxNodeOrToken node, MatchResult result)
        {
            var itreeNode = result.Match.Item1;
            var nnode = new Node(itreeNode);
            return nnode;
        }

        public static IEnumerable<MatchResult> CList(MatchResult child1, IEnumerable<MatchResult> cList)
        {
            return GList<MatchResult>.List(child1, cList);
        }

        public static IEnumerable<MatchResult> SC(MatchResult child)
        {
            return GList<MatchResult>.Single(child);
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

        public static IEnumerable<SyntaxNodeOrToken> EList(SyntaxNodeOrToken child1, IEnumerable<SyntaxNodeOrToken> cList)
        {
            return GList<SyntaxNodeOrToken>.List(child1, cList);
        }

        public static IEnumerable<SyntaxNodeOrToken> SE(SyntaxNodeOrToken child)
        {
            return GList<SyntaxNodeOrToken>.Single(child);
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

        public static bool FTrue()
        {
            return true;
        }

        public static SyntaxNodeOrToken Transformation(SyntaxNodeOrToken node, IEnumerable<SyntaxNodeOrToken> loop)
        {
            List<SyntaxNodeOrToken> afterNodeList;
            List<SyntaxNodeOrToken> beforeNodeList;
            FillBeforeAfterList(out afterNodeList, loop, out beforeNodeList);

            //traversal index of the nodes before the transformation
            var traversalIndices = PostOrderTraversalIndices(node, beforeNodeList);

            //Annotate edited nodes
            node = AnnotateNodeEditedNodes(node, traversalIndices);

            //Update annotated nodes
            node = UpdateAnnotatedNodes(node, traversalIndices, afterNodeList);

            var stringNode = node.ToFullString();
            return node;
        }

        private static SyntaxNodeOrToken UpdateAnnotatedNodes(SyntaxNodeOrToken node, List<int> traversalIndices, List<SyntaxNodeOrToken> afterNodeList)
        {
            for (int i = 0; i < traversalIndices.Count; i++)
            {
                var index = traversalIndices[i];
                var snode = node.AsNode().GetAnnotatedNodes($"ANN{index}");
                if (snode.Any())
                {
                    var rewriter = new UpdateTreeRewriter(snode.First(), afterNodeList.ElementAt(i).AsNode());
                    node = rewriter.Visit(node.AsNode());
                }
            }
            return node;
        }

        private static SyntaxNodeOrToken AnnotateNodeEditedNodes(SyntaxNodeOrToken node, List<int> traversalIndices)
        {
            foreach (var index in traversalIndices)
            {
                var treeNode = ConverterHelper.ConvertCSharpToTreeNode(node);
                var traversalNodes = treeNode.DescendantNodesAndSelf();
                var ann = new AddAnnotationRewriter(traversalNodes.ElementAt(index).Value.AsNode(),
                    new List<SyntaxAnnotation> {new SyntaxAnnotation($"ANN{index}")});
                node = ann.Visit(node.AsNode());
            }
            return node;
        }

        private static void FillBeforeAfterList(out List<SyntaxNodeOrToken> afterNodeList, IEnumerable<SyntaxNodeOrToken> loop, out List<SyntaxNodeOrToken> beforeNodeList)
        {
            afterNodeList = new List<SyntaxNodeOrToken>();
            beforeNodeList = new List<SyntaxNodeOrToken>();
            var list = loop.ToList();
            foreach (var snode in list)
            {
                afterNodeList.AddRange(MappingRegions[snode]);
                beforeNodeList.AddRange(BeforeAfterMapping[snode]);
            }
        }

        private static List<int> PostOrderTraversalIndices(SyntaxNodeOrToken node, List<SyntaxNodeOrToken> beforeNodeList)
        {
            var treeNode = ConverterHelper.ConvertCSharpToTreeNode(node);
            var traversalNodes = treeNode.DescendantNodesAndSelf();
            var traversalIndices = new List<int>();

            for (int i = 0; i < traversalNodes.Count; i++)
            {
                var snode = traversalNodes[i];
                foreach (var v in beforeNodeList)
                {
                    if (snode.Value.Equals(v))
                    {
                        traversalIndices.Add(i);
                    }
                }
            }
            return traversalIndices;
        }

        public static IEnumerable<SyntaxNodeOrToken> Template(SyntaxNodeOrToken node, Pattern pattern)
        {
            var currentTree = GetCurrentTree(node);

            var res = new List<SyntaxNodeOrToken>();
            if (pattern.Tree.Value.Kind == SyntaxKind.EmptyStatement)
            {
                var list = FlorestByKind(pattern, currentTree);

                if (list.Any()) res = CreateRegions(list);
            }
            res = SingleLocations(res);
            return res;
        }

        private static List<SyntaxNodeOrToken> SingleLocations(List<SyntaxNodeOrToken> res)
        {
            var list = new List<SyntaxNodeOrToken>();
            var candidates = new List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>> ();
            bool [] intersect = new bool[res.Count]; 

            for (int i = 0; i < res.Count; i++)
            {
                var v = res[i];
                for (int j =  i + 1; j < res.Count; j++)
                {
                    var c = res[j];
                    if (v.Span.Contains(c.Span) || c.Span.Contains(v.Span))
                    {
                        intersect[i] = true;
                        intersect[j] = true;
                        candidates.Add(Tuple.Create(v, c));
                        break;
                    }
                }

                if (!intersect[i])
                {
                    list.Add(v);
                }
            }

            list.AddRange(candidates.Select(v => v.Item1.Span.Contains(v.Item2.Span) ? v.Item2 : v.Item1));
            return list.OrderBy(o => o.SpanStart).ToList();
        }

        private static List<SyntaxNodeOrToken> CreateRegions(List<List<SyntaxNodeOrToken>> list)
        {
            var regions = new List<SyntaxNodeOrToken>();
            for (int j = 0; j < list.First().Count; j++)
            {
                ITreeNode<SyntaxNodeOrToken> iTree = new TreeNode<SyntaxNodeOrToken>(SyntaxFactory.EmptyStatement(), new TLabel(SyntaxKind.EmptyStatement));
                for (int i = 0; i < list.Count; i++)
                {
                    var child = list[i][j];
                    var newchild = ConverterHelper.ConvertCSharpToTreeNode(child);
                    iTree.AddChild(newchild, i);
                }
                TreeUpdateDictionary[iTree.Children.First().Value] = new TreeUpdate(iTree); //each column represent a new region

                var beforeFlorest = iTree.Children.Select(o => o.Value).ToList();
                BeforeAfterMapping[iTree.Children.First().Value] = beforeFlorest;

                regions.Add(iTree.Children.First().Value);
            }
            return regions;
        }

        private static List<List<SyntaxNodeOrToken>> FlorestByKind(Pattern match, ITreeNode<SyntaxNodeOrToken> currentTree)
        {
            var list = new List<List<SyntaxNodeOrToken>>();
            foreach (var child in match.Tree.Children)
            {
                var nodeList = SplitToNodes(currentTree, child.Value.Kind);
                var result = (from node in nodeList where IsValue(node, child) select node.Value).ToList();

                if (result.Any())
                {
                    list.Add(result);
                }
            }
            return list;
        }

        private static bool IsValue(ITreeNode<SyntaxNodeOrToken> snode, ITreeNode<Token> pattern)
        {
            if (!snode.Value.IsKind(pattern.Value.Kind)) return false; //root pattern
            foreach (var child in pattern.Children)
            {
                var valid = snode.Children.Any(tchild => IsValue(tchild, child));

                if (!valid) return false;
            }
            return true;
        }

        /// <summary>
        /// Syntax node factory. This method will be removed in future
        /// </summary>
        /// <param name="kind">SyntaxKind of the node that will be created.</param>
        /// <param name="children">Children nodes.</param>
        /// <param name="node">Node</param>
        /// <returns>A SyntaxNode with specific king and children</returns>
        private static SyntaxNodeOrToken GetSyntaxElement(SyntaxKind kind, List<SyntaxNodeOrToken> children, SyntaxNodeOrToken node = default(SyntaxNodeOrToken))
        {
            switch (kind)
            {
            case SyntaxKind.NameColon:
                {
                    var identifier = (IdentifierNameSyntax)children[0];
                    var nameColon = SyntaxFactory.NameColon(identifier);
                    return nameColon;
                }
            case SyntaxKind.LogicalAndExpression:
                {
                    var leftExpression = (ExpressionSyntax)children[0];
                    var rightExpresssion = (ExpressionSyntax)children[1];
                    var logicalAndExpression = SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression,
                        leftExpression, rightExpresssion);
                    return logicalAndExpression;
                }
            case SyntaxKind.ExpressionStatement:
                {
                    ExpressionSyntax expression = (ExpressionSyntax)children.First();
                    ExpressionStatementSyntax expressionStatement = SyntaxFactory.ExpressionStatement(expression);
                    return expressionStatement;
                }
            case SyntaxKind.Block:
                {
                    var statetements = children.Select(child => (StatementSyntax)child).ToList();

                    var block = SyntaxFactory.Block(statetements);
                    return block;
                }
            case SyntaxKind.InvocationExpression:
                {
                    var expressionSyntax = (ExpressionSyntax)children[0];
                    ArgumentListSyntax argumentList = (ArgumentListSyntax)children[1];
                    var invocation = SyntaxFactory.InvocationExpression(expressionSyntax, argumentList);
                    return invocation;
                }
            case SyntaxKind.SimpleMemberAccessExpression:
                {
                    var expressionSyntax = (ExpressionSyntax)children[0];
                    var syntaxName = (SimpleNameSyntax)children[1];
                    var simpleMemberExpression = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expressionSyntax, syntaxName);
                    return simpleMemberExpression;
                }
            case SyntaxKind.ParameterList:
                {
                    var parameter = (ParameterSyntax)children[0];
                    var spal = SyntaxFactory.SeparatedList(new[] { parameter });
                    var parameterList = SyntaxFactory.ParameterList(spal);
                    return parameterList;
                }
            case SyntaxKind.Parameter:
                {
                    return children.First().Parent;
                }
            case SyntaxKind.ArgumentList:
                {
                    var listArguments = children.Select(child => (ArgumentSyntax)child).ToList();

                    var spal = SyntaxFactory.SeparatedList(listArguments);
                    var argumentList = SyntaxFactory.ArgumentList(spal);
                    return argumentList;
                }
            case SyntaxKind.Argument:
                if (children.Count() == 1)
                {
                    ExpressionSyntax s = (ExpressionSyntax)children.First();
                    var argument = SyntaxFactory.Argument(s);
                    return argument;
                }
                else
                {
                    var ncolon = (NameColonSyntax)children[0];
                    var expression = (ExpressionSyntax)children[1];
                    var argument = SyntaxFactory.Argument(ncolon, default(SyntaxToken), expression);
                    return argument;
                }
            case SyntaxKind.ParenthesizedLambdaExpression:
                {
                    var parameterList = (ParameterListSyntax)children[0];
                    var csharpbody = (CSharpSyntaxNode)children[1];
                    var parenthizedLambdaExpression = SyntaxFactory.ParenthesizedLambdaExpression(parameterList, csharpbody);
                    return parenthizedLambdaExpression;
                }
            case SyntaxKind.EqualsExpression:
                {
                    var left = (ExpressionSyntax)children[0];
                    var right = (ExpressionSyntax)children[1];
                    var equalsExpression = SyntaxFactory.BinaryExpression(SyntaxKind.EqualsExpression, left, right);
                    return equalsExpression;
                }
            case SyntaxKind.IfStatement:
                {
                    var condition = (ExpressionSyntax)children[0];
                    var statementSyntax = (StatementSyntax)children[1];
                    var ifStatement = SyntaxFactory.IfStatement(condition, statementSyntax);
                    return ifStatement;
                }
            case SyntaxKind.UnaryMinusExpression:
                {
                    ExpressionSyntax expression = (ExpressionSyntax)children[0];
                    var unary = SyntaxFactory.PrefixUnaryExpression(kind, expression);
                    return unary;
                }
            case SyntaxKind.ReturnStatement:
                {
                    ExpressionSyntax expression = (ExpressionSyntax)children[0];
                    var returnStatement = SyntaxFactory.ReturnStatement(expression);
                    return returnStatement;
                }
            case SyntaxKind.ElseClause:
                {
                    var statatementSyntax = (StatementSyntax)children[0];
                    var elseClause = SyntaxFactory.ElseClause(statatementSyntax);
                    return elseClause;
                }
            case SyntaxKind.IdentifierName:
                {
                    SyntaxToken stoken = (SyntaxToken)children.First();
                    var identifierName = SyntaxFactory.IdentifierName(stoken);
                    return identifierName;
                }
            case SyntaxKind.TypeArgumentList:
                {
                    var listType = children.Select(child => (TypeSyntax)child).ToList();

                    var typespal = SyntaxFactory.SeparatedList(listType);
                    var typeArgument = SyntaxFactory.TypeArgumentList(typespal);
                    return typeArgument;
                }
            case SyntaxKind.GenericName:
                {
                    var gName = (GenericNameSyntax)node;
                    var typeArg = (TypeArgumentListSyntax)children[0];
                    var genericName = SyntaxFactory.GenericName(gName.Identifier, typeArg);
                    return genericName;
                }
            case SyntaxKind.NotEqualsExpression:
                {
                    var leftExpression = (ExpressionSyntax)children[0];
                    var rightExpression = (ExpressionSyntax)children[1];
                    var notEqualsExpression = SyntaxFactory.BinaryExpression(SyntaxKind.NotEqualsExpression,
                        leftExpression, rightExpression);
                    return notEqualsExpression;
                }
            }

            return null;
        }

        /// <summary>
        /// Reconstruct the tree
        /// </summary>
        /// <param name="tree">Tree in another format</param>
        /// <returns>Reconstructed tree</returns>
        public static SyntaxNodeOrToken ReconstructTree(ITreeNode<SyntaxNodeOrToken> tree)
        {
            if (!tree.Children.Any())
            {
                return tree.Value;
            }

            var children = tree.Children.Select(ReconstructTree).ToList();

            //if (tree.Value.IsKind(SyntaxKind.MethodDeclaration))
            //{
            //    var method = (MethodDeclarationSyntax)tree.Value;
            //    method = method.WithReturnType((TypeSyntax)children[0]);
            //    method = method.WithParameterList((ParameterListSyntax)children[1]);
            //    method = method.WithBody((BlockSyntax)children[2]);
            //    return method.NormalizeWhitespace();
            //}

            var node = GetSyntaxElement(tree.Value.Kind(), children, tree.Value);
            return node;
        }
    }
}
