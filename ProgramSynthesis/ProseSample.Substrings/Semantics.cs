using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProseSample.Substrings.List;
using Spg.TreeEdit.Node;
using TreeEdit.Spg.Print;
using TreeEdit.Spg.Script;
using TreeEdit.Spg.TreeEdit.Update;
using Tutor.Spg.TreeEdit.Node;

namespace ProseSample.Substrings
{
    public static class Semantics
    {
        /// <summary>
        /// Store the current trees.
        /// </summary>
        private static readonly Dictionary<SyntaxNodeOrToken, ITreeNode<SyntaxNodeOrToken>> CurrentTrees = new Dictionary<SyntaxNodeOrToken, ITreeNode<SyntaxNodeOrToken>>();

        /// <summary>
        /// Store the tree update associate to each node
        /// </summary>
        private static readonly Dictionary<SyntaxNodeOrToken, TreeUpdate> TreeUpdateDictionary = new Dictionary<SyntaxNodeOrToken, TreeUpdate>();

        /// <summary>
        /// Match function. This function matches the first element on the tree that has the specified kind and child nodes.
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="kind">Syntax kind</param>
        /// <param name="children">Children nodes</param>
        /// <returns> Returns the first element on the tree that has the specified kind and child nodes.</returns>
        public static MatchResult C(SyntaxNodeOrToken node, SyntaxKind kind, IEnumerable<MatchResult> children)
        {
            var currentTree = GetCurrentTree(node);

            var klist = SplitToNodes(currentTree, kind);

            foreach (var item in klist)
            {
                //if(item.Children.Count != children.Count()) continue;

                for (int i = 0; i < children.Count(); i++)
                {
                    //var kindMatch = item.Children[i];
                    var expression = children.ElementAt(i);
                    if (MatchChildren(item.Value, expression.Match.Item1.Value))
                    {
                        var match = Tuple.Create<ITreeNode<SyntaxNodeOrToken>, Bindings>(item, null);
                        MatchResult matchResult = new MatchResult(match);
                        return matchResult;
                    }
                }
            }

            return null;
        }


        /// <summary>
        /// Match function. This function matches the first element on the tree that has the specified kind and child nodes.
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="kind">Syntax kind</param>
        /// <param name="children">Children nodes</param>
        /// <returns> Returns the first element on the tree that has the specified kind and child nodes.</returns>
        public static MatchResult P(SyntaxNodeOrToken node, SyntaxKind kind, IEnumerable<MatchResult> children)
        {
            var currentTree = GetCurrentTree(node);

            var klist = SplitToNodes(currentTree, kind);

            foreach (var item in klist)
            {
                //if(item.Children.Count != children.Count()) continue;

                for (int i = 0; i < children.Count(); i++)
                {
                    //var kindMatch = item.Children[i];
                    var expression = children.ElementAt(i);
                    if (MatchChildren(item.Value, expression.Match.Item1.Value))
                    {
                        var match = Tuple.Create<ITreeNode<SyntaxNodeOrToken>, Bindings>(item, null);
                        MatchResult matchResult = new MatchResult(match);
                        return matchResult;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Splits the source node in the elements of type kind.
        /// </summary>
        /// <param name="node">Source node</param>
        /// <param name="kind">Syntax kind</param>
        /// <returns></returns>
        private static List<ITreeNode<SyntaxNodeOrToken>> SplitToNodes(ITreeNode<SyntaxNodeOrToken> node, SyntaxKind kind)
        {
            TLabel label= new TLabel(kind);
            var descendantNodes = node.DescendantNodesAndSelf();
            var kinds = from k in descendantNodes
                where k.IsLabel(label)
                select k;

            return kinds.ToList();
        }

        //TODO refactor this method
        /// <summary>
        /// Verify if the parent contains the parameter child
        /// </summary>
        /// <param name="parent">Parent node</param>
        /// <param name="child">Child node</param>
        /// <returns>True, if parent contains the child, false otherwise.</returns>
        private static bool MatchChildren(SyntaxNodeOrToken parent, SyntaxNodeOrToken child)
        {
            foreach (var item in parent.ChildNodesAndTokens())
            {
                if (item.IsKind(child.Kind()) /*&& item.ToString().Equals(child.ToString())*/)
                {
                    return true;
                }

                if (child.IsKind(SyntaxKind.IdentifierToken) || child.IsKind(SyntaxKind.IdentifierName))
                {
                    string itemString = item.ToString();
                    string childString = child.ToString();
                    if (itemString.Equals(childString))
                    {
                        return true;
                    }
                }

                if (child.IsKind(SyntaxKind.NumericLiteralToken) || child.IsKind(SyntaxKind.NumericLiteralExpression))
                {
                    string itemString = item.ToString();
                    string childString = child.ToString();
                    if (itemString.Equals(childString))
                    {
                        return true;
                    }
                }

                if (child.IsKind(SyntaxKind.StringLiteralToken) || child.IsKind(SyntaxKind.StringLiteralExpression))
                {
                    string itemString = item.ToString();
                    string childString = child.ToString();
                    if (itemString.Equals(childString))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Build a literal
        /// </summary>
        /// <param name="node">Input node</param>
        /// <param name="lookFor">Literal itself.</param>
        /// <returns>Match of parameter literal in the source code.</returns>
        public static MatchResult Concrete(SyntaxNodeOrToken lookFor)
        {
            var currentTree = GetCurrentTree(lookFor);
            var matches = Matches(currentTree, lookFor);
            if (matches.Any())
            {
                var match = Tuple.Create<ITreeNode<SyntaxNodeOrToken>, Bindings>(ConverterHelper.ConvertCSharpToTreeNode(matches.First()), null);
                MatchResult matchResult = new MatchResult(match);
                return matchResult;
            }

            return null;
        }

        public static MatchResult Abstract(SyntaxKind kind)
        {
            //var currentTree = GetCurrentTree(node);

            //var matches = SplitToNodes(currentTree, kind);

            //if (matches.Any())
            //{
            //    var result = new MatchResult(Tuple.Create(matches.ElementAt(k - 1), new Bindings(new List<SyntaxNodeOrToken> { matches.ElementAt(k - 1).Value })));
            //    return result;
            //}
            return null;
        }

        /// <summary>
        /// Build a literal
        /// </summary>
        /// <param name="node">Input node</param>
        /// <param name="lookFor">Literal itself.</param>
        /// <returns>Match of parameter literal in the source code.</returns>
        public static MatchResult Literal(SyntaxNodeOrToken node, SyntaxNodeOrToken lookFor)
        {
            var currentTree = GetCurrentTree(node);
            var matches = Matches(currentTree, lookFor);
            if (matches.Any())
            {
                var match = Tuple.Create<ITreeNode<SyntaxNodeOrToken>, Bindings>(ConverterHelper.ConvertCSharpToTreeNode(matches.First()), null);
                MatchResult matchResult = new MatchResult(match);
                return matchResult;
            }

            return null;
        }

        /// <summary>
        /// Concrete matches
        /// </summary>
        /// <param name="inpTree">Source node</param>
        /// <param name="sot">Concrete node to look for.</param>
        /// <returns>Concrete matches</returns>
        private static List<SyntaxNodeOrToken> Matches(ITreeNode<SyntaxNodeOrToken> inpTree, SyntaxNodeOrToken sot)
        {
            var descendants = inpTree.DescendantNodes();
            var matches = from item in descendants
                          where item.Value.IsKind(sot.Kind()) && item.ToString().Equals(sot.ToString())
                          select item.Value;
            return matches.ToList();
        }

        /// <summary>
        /// Insert the ast node as in the k position of the node in the matching result 
        /// </summary>
        /// <param name="node">Input data</param>
        /// <param name="k">Position in witch the node will be inserted.</param>
        /// <param name="mresult">Matching result</param>
        /// <param name="ast">Node that will be insert</param>
        /// <returns>New node with the ast node inserted as the k child</returns>
        public static SyntaxNodeOrToken Insert(SyntaxNodeOrToken node, int k, MatchResult mresult, SyntaxNodeOrToken ast)
        {
            TreeUpdate update = TreeUpdateDictionary[node];

            var parent = ConverterHelper.ConvertCSharpToTreeNode(mresult.Match.Item1.Value);
            var child = ConverterHelper.ConvertCSharpToTreeNode(ast);


            var insert = new Insert<SyntaxNodeOrToken>(child, parent, k);
            update.ProcessEditOperation(insert);

            PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
            return update.CurrentTree.Value;
        }

        /// <summary>
        /// Move the from node such that it is the k child of the node
        /// </summary>
        /// <param name="node">Source node</param>
        /// <param name="k">Child index</param>
        /// <param name="parent">Parent</param>
        /// <param name="from">Moved node</param>
        /// <returns></returns>
        public static SyntaxNodeOrToken Move(SyntaxNodeOrToken node, int k, MatchResult parent, MatchResult from)
        {
            TreeUpdate update = TreeUpdateDictionary[node];

            var parentNode = ConverterHelper.ConvertCSharpToTreeNode(parent.Match.Item1.Value);
            var child = ConverterHelper.ConvertCSharpToTreeNode(from.Match.Item1.Value);

            var move = new Move<SyntaxNodeOrToken>(child, parentNode, k);
            update.ProcessEditOperation(move);

            PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
            return update.CurrentTree.Value;
        }

        public static SyntaxNodeOrToken Update(SyntaxNodeOrToken node, MatchResult from, SyntaxNodeOrToken to)
        {
            TreeUpdate update = TreeUpdateDictionary[node];

            var fromTreeNode = ConverterHelper.ConvertCSharpToTreeNode(from.Match.Item1.Value);
            var toTreeNode = ConverterHelper.ConvertCSharpToTreeNode(to);

            var updateEdit = new Update<SyntaxNodeOrToken>(fromTreeNode, toTreeNode, null);
            update.ProcessEditOperation(updateEdit);

            PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
            return update.CurrentTree.Value;
        }

        public static SyntaxNodeOrToken Delete(SyntaxNodeOrToken node, MatchResult delete)
        {
            TreeUpdate update = TreeUpdateDictionary[node];

            var t1Node = ConverterHelper.ConvertCSharpToTreeNode(delete.Match.Item1.Value);

            var updateEdit = new Delete<SyntaxNodeOrToken>(t1Node);
            update.ProcessEditOperation(updateEdit);

            PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
            return update.CurrentTree.Value;
        }

        public static MatchResult Tree(MatchResult kindRef)
        {
            return kindRef;
        }

        public static MatchResult KindRef(SyntaxNodeOrToken node, SyntaxKind kind, int k)
        {
            var currentTree = GetCurrentTree(node);

            var matches = SplitToNodes(currentTree, kind);

            if (matches.Any())
            {
                var result = new MatchResult(Tuple.Create(matches.ElementAt(k - 1), new Bindings(new List<SyntaxNodeOrToken> { matches.ElementAt(k - 1).Value })));
                return result;
            }
            return null;
        }

        //TODO rename to child
        public static MatchResult Parent(SyntaxNodeOrToken node, MatchResult kindRef, int k)
        {
            //SyntaxNodeOrToken child = kindRef.match.Item1.AsNode().ChildNodes().ElementAt(k - 1);
            var child = TreeUpdate.FindNode(GetCurrentTree(node), kindRef.Match.Item1.Value).Children.ElementAt(k - 1);
            var result = new MatchResult(Tuple.Create(child, new Bindings(new List<SyntaxNodeOrToken> { child.Value })));
            return result;
        }

        private static ITreeNode<SyntaxNodeOrToken> GetCurrentTree(SyntaxNodeOrToken n)
        {
            if (!CurrentTrees.ContainsKey(n))
            {
                CurrentTrees[n] = ConverterHelper.ConvertCSharpToTreeNode(n);
                TreeUpdate update = new TreeUpdate(n);
                TreeUpdateDictionary[n] = update;
            }
            var node = TreeUpdateDictionary[n].CurrentTree;

            return node;
        }


        public static SyntaxNodeOrToken Script(SyntaxNodeOrToken node, IEnumerable<SyntaxNodeOrToken> edit)
        {
            var tree = ReconstructTree(GetCurrentTree(node));
            return tree;
        }

        /// <summary>
        /// Return a new node
        /// </summary>
        /// <param name="kind">Returned node SyntaxKind</param>
        /// <param name="childrenNodes">Children nodes</param>
        /// <returns>A new node with kind and child</returns>
        public static SyntaxNodeOrToken Node(SyntaxKind kind, IEnumerable<SyntaxNodeOrToken> childrenNodes)
        {
            var node = GetSyntaxElement(kind, childrenNodes.ToList());
            return node;
        }

        /// <summary>
        /// Create a constant node
        /// </summary>
        /// <param name="cst">Constant</param>
        /// <returns>A new constant node.</returns>
        public static SyntaxNodeOrToken Const(SyntaxNodeOrToken cst)
        {
            return cst;
        }

        public static SyntaxNodeOrToken Ref(SyntaxNodeOrToken node, MatchResult result)
        {
            return result.Match.Item1.Value;
        }

        public static IEnumerable<MatchResult> CList(MatchResult child1, IEnumerable<MatchResult> cList)
        {
            return GList<MatchResult>.List(child1, cList);
        }

        public static IEnumerable<MatchResult> SC(MatchResult child)
        {
            return GList<MatchResult>.Single(child);
        }

        public static IEnumerable<MatchResult> PList(MatchResult child1, IEnumerable<MatchResult> cList)
        {
            return GList<MatchResult>.List(child1, cList);
        }

        public static IEnumerable<MatchResult> PC(MatchResult child)
        {
            return GList<MatchResult>.Single(child);
        }

        public static IEnumerable<SyntaxNodeOrToken> NList(SyntaxNodeOrToken child1, IEnumerable<SyntaxNodeOrToken> cList)
        {
            return GList<SyntaxNodeOrToken>.List(child1, cList);
        }

        public static IEnumerable<SyntaxNodeOrToken> SN(SyntaxNodeOrToken child)
        {
            return GList<SyntaxNodeOrToken>.Single(child);
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

        public static SyntaxNodeOrToken OneTrans(SyntaxNodeOrToken node, SyntaxNodeOrToken script)
        {
            return null;
        }

        public static SyntaxNodeOrToken ManyTrans(SyntaxNodeOrToken node, IEnumerable<SyntaxNodeOrToken> loop)
        {
            var list = loop.ToList();
            var nodeElements = from snode in node.AsNode().DescendantNodesAndSelf()
                where snode.IsKind(list.First().Kind())
                select snode;

            for(int i = 0; i < nodeElements.Count(); i++)
            {
                var kinds = from snode in node.AsNode().DescendantNodesAndSelf()
                                   where snode.IsKind(list.First().Kind())
                                   select snode;

                var item = kinds.ElementAt(i);
                var rewriter = new UpdateTreeRewriter(item, list.ElementAt(i).AsNode());
                node = rewriter.Visit(node.AsNode());
            }

            return node;
        }

        public static IEnumerable<SyntaxNodeOrToken> BreakByKind(SyntaxNodeOrToken node, MatchResult match)
        {
            //var currentTree = GetCurrentTree(node);
            //var nodeList = SplitToNodes(currentTree, kind);

            //var kList = nodeList.Select(o => o.Value);

            //return kList;
            return null;
        }

        /// <summary>
        /// Syntax node factory. This method will be removed in future
        /// </summary>
        /// <param name="kind">SyntaxKind of the node that will be created.</param>
        /// <param name="children">Children nodes.</param>
        /// <returns>A SyntaxNode with specific king and children</returns>
        private static SyntaxNodeOrToken GetSyntaxElement(SyntaxKind kind, List<SyntaxNodeOrToken> children)
        {
            if (kind == SyntaxKind.LogicalAndExpression)
            {
                var leftExpression = (ExpressionSyntax) children[0];
                var rightExpresssion = (ExpressionSyntax) children[1];
                var logicalAndExpression = SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, leftExpression, rightExpresssion);
                return logicalAndExpression;
            }

            if (kind == SyntaxKind.ExpressionStatement)
            {
                ExpressionSyntax expression = (ExpressionSyntax)children.First();
                ExpressionStatementSyntax expressionStatement = SyntaxFactory.ExpressionStatement(expression);
                return expressionStatement;
            }

            if (kind == SyntaxKind.Block)
            {
                var statetements = children.Select(child => (StatementSyntax) child).ToList();

                var block = SyntaxFactory.Block(statetements);
                return block;
            }

            if (kind == SyntaxKind.InvocationExpression)
            {
                var expressionSyntax = (ExpressionSyntax)children[0]; //expression syntax
                ArgumentListSyntax argumentList = (ArgumentListSyntax)children[1]; //argument list
                var invocation = SyntaxFactory.InvocationExpression(expressionSyntax, argumentList);
                return invocation;
            }

            if (kind == SyntaxKind.SimpleMemberAccessExpression)
            {
                var expressionSyntax = (ExpressionSyntax) children[0];
                var syntaxName = (SimpleNameSyntax) children[1];
                var simpleMemberExpression = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expressionSyntax, syntaxName);
                return simpleMemberExpression;
            }

            if (kind == SyntaxKind.ParameterList)
            {
                var parameter = (ParameterSyntax) children[0];
                var spal = SyntaxFactory.SeparatedList(new[] { parameter });
                var parameterList = SyntaxFactory.ParameterList(spal);
                return parameterList;
            }

            if (kind == SyntaxKind.Parameter)
            {
                return children.First().Parent;
            }

            if (kind == SyntaxKind.ArgumentList)
            {
                ArgumentSyntax argument = (ArgumentSyntax)children.First();
                var spal = SyntaxFactory.SeparatedList(new[] { argument });
                var argumentList = SyntaxFactory.ArgumentList(spal);
                return argumentList;
            }

            if (kind == SyntaxKind.Argument)
            {
                ExpressionSyntax s = (ExpressionSyntax)children.First();
                var argument = SyntaxFactory.Argument(s);
                return argument;
            }

            if (kind == SyntaxKind.EqualsExpression)
            {
                var left = (ExpressionSyntax)children[0];
                var right = (ExpressionSyntax)children[1];
                var equalsExpression = SyntaxFactory.BinaryExpression(SyntaxKind.EqualsExpression, left, right);
                return equalsExpression;
            }

            if (kind == SyntaxKind.IfStatement)
            {
                var condition = (ExpressionSyntax)children[0];
                var statementSyntax = (StatementSyntax) children[1];
                var ifStatement = SyntaxFactory.IfStatement(condition, statementSyntax);
                return ifStatement;
            }

            if (kind == SyntaxKind.UnaryMinusExpression)
            {
                ExpressionSyntax expression = (ExpressionSyntax) children[0];
                var unary = SyntaxFactory.PrefixUnaryExpression(kind, expression);
                return unary;
            }

            if (kind == SyntaxKind.ReturnStatement)
            {
                ExpressionSyntax expression = (ExpressionSyntax) children[0];
                var returnStatement = SyntaxFactory.ReturnStatement(expression);
                return returnStatement;
            }

            if (kind == SyntaxKind.ElseClause)
            {
                var statatementSyntax = (StatementSyntax) children[0];
                var elseClause = SyntaxFactory.ElseClause(statatementSyntax);
                return elseClause;
            }

            if (kind == SyntaxKind.IdentifierName)
            {
                SyntaxToken stoken = (SyntaxToken) children.First();
                var identifierName = SyntaxFactory.IdentifierName(stoken);
                return identifierName;
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

            if (tree.Value.IsKind(SyntaxKind.MethodDeclaration))
            {
                var method = (MethodDeclarationSyntax)tree.Value;
                method = method.WithReturnType((TypeSyntax)children[0]);
                method = method.WithParameterList((ParameterListSyntax) children[1]);
                method = method.WithBody((BlockSyntax) children[2]);
                return method.NormalizeWhitespace();
            }

            var node = GetSyntaxElement(tree.Value.Kind(), children);
            return node;
        }
    }
}
