using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactoring.Tok;
using Spg.ExampleRefactoring.RegularExpression;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProseSample.Substrings.List;
using TreeEdit.Spg.TreeEdit.Update;

namespace ProseSample.Substrings
{
    public static class Semantics
    {
        private static readonly Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> CurrentTrees = new Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken>();

        public static MatchResult C(SyntaxNodeOrToken node, SyntaxKind kind, IEnumerable<MatchResult> children)
        {
            var currentTree = GetCurrentTree(node).AsNode();

            var klist = SplitToNodes(currentTree, kind);

            for (int i = 0; i < klist.Count; i++)
            {
                var kindMatch = klist[i];
                var expression = children.ElementAt(i);
                if (MatchChildren(kindMatch, expression.match.Item1))
                {
                    Tuple<SyntaxNodeOrToken, Bindings> match = Tuple.Create<SyntaxNodeOrToken, Bindings>(kindMatch, null);
                    MatchResult matchResult = new MatchResult(match);
                    return matchResult;
                }
            }

            return null;
        }

        private static List<SyntaxNode> SplitToNodes(SyntaxNodeOrToken node, SyntaxKind kind)
        {
            var kinds = from k in node.AsNode().DescendantNodes()
                where k.IsKind(kind)
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
                if (item.IsKind(child.Kind()) && item.ToString().Equals(child.ToString()))
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
        public static MatchResult Literal(SyntaxNodeOrToken node, SyntaxNodeOrToken lookFor)
        {
            var currentTree = GetCurrentTree(node);
            Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken> tuple = Tuple.Create(currentTree, lookFor);
            Tuple<ListNode, ListNode> lnode = ASTProgram.Example(tuple);

            TokenSeq seq = DymTokens(lnode.Item2.List);
            var matches = Regex.Matches(lnode.Item1, seq);

            bool m = matches.Any();
            if (m)
            {
                Tuple<SyntaxNodeOrToken, Bindings> match = Tuple.Create<SyntaxNodeOrToken, Bindings>(matches.First().Item2.List.First(), null);
                MatchResult matchResult = new MatchResult(match);
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
        public static SyntaxNodeOrToken Insert(SyntaxNodeOrToken node, int k, MatchResult mresult, SyntaxNodeOrToken ast)
        {
            var currentTree = GetCurrentTree(node);
            SyntaxNodeOrToken syntaxNodeOrToken = mresult.match.Item1;

            List<SyntaxNode> nodes = new List<SyntaxNode> { ast.AsNode() };

            var root = syntaxNodeOrToken.AsNode();

            if (root.ChildNodes().Count() >= k)
            {
                var select = root.ChildNodes().ElementAt(k - 1);
                try
                {
                    root = root.InsertNodesBefore(root.FindNode(select.Span), nodes);
                }
                catch (Exception e)
                {
                    root = root.ReplaceNode(root.FindNode(select.Span), nodes.First());
                }
            }
            else if (root.ChildNodes().Count() + 1 == k)
            {
                if (root.IsKind(SyntaxKind.IfStatement) && ast.IsKind(SyntaxKind.ElseClause))
                {
                    var ifStatementSyntax = (IfStatementSyntax) root;
                    var elseClause = (ElseClauseSyntax) ast;
                    root = ifStatementSyntax.WithElse(elseClause);
                } 
                else
                {
                    var select = root.ChildNodes().Last();
                    root = root.InsertNodesAfter(root.FindNode(select.Span), nodes);
                    //TODO decide what to do what the children are empty.
                }
            }
            else
            {
                
            }

            CSharpSyntaxRewriter rewriter = new UpdateTreeRewriter(syntaxNodeOrToken.AsNode(), root.NormalizeWhitespace());
            root = rewriter.Visit(currentTree.AsNode());
            CurrentTrees[node] = root;
            return root.NormalizeWhitespace();
        }

        public static SyntaxNodeOrToken Move(SyntaxNodeOrToken node, int k, MatchResult parent, SyntaxNodeOrToken ast)
        {
            var root = Insert(node, k, parent, ast);
            return root;
        }

        public static SyntaxNodeOrToken Update(SyntaxNodeOrToken node, MatchResult from, SyntaxNodeOrToken to)
        {
            return null;
        }

        public static SyntaxNodeOrToken Delete(SyntaxNodeOrToken node, MatchResult from)
        {
            return null;
        }

        public static MatchResult Abstract(SyntaxNodeOrToken node, SyntaxKind kind, int k)
        {
            var currentTree = GetCurrentTree(node).AsNode();

            var matches = SplitToNodes(currentTree, kind);

            if (matches.Any())
            {
                Tuple<SyntaxNodeOrToken, Bindings> match = Tuple.Create<SyntaxNodeOrToken, Bindings>(matches.ElementAt(k - 1), null);
                MatchResult matchResult = new MatchResult(match);
                return matchResult;
            }
            return null;
        }

        private static SyntaxNodeOrToken GetCurrentTree(SyntaxNodeOrToken n)
        {
            if (!CurrentTrees.ContainsKey(n))
            {
                CurrentTrees[n] = n; 
            }

            SyntaxNodeOrToken node = CurrentTrees[n];
            
            return node;
        }


        public static SyntaxNodeOrToken Script(SyntaxNodeOrToken node, IEnumerable<SyntaxNodeOrToken> edit)
        {
            return edit.Last();
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

        #region Constant Operators
        /// <summary>
        /// Create a constant node
        /// </summary>
        /// <param name="cst">Constant</param>
        /// <returns>A new constant node.</returns>
        public static SyntaxNodeOrToken Const(SyntaxNodeOrToken cst)
        {
            return cst;
        }

        #endregion

        public static IEnumerable<MatchResult> CList(MatchResult child1, IEnumerable<MatchResult> cList)
        {
            return GList<MatchResult>.List(child1, cList);
        }

        public static IEnumerable<MatchResult> SC(MatchResult child)
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
        
             
        /// <summary>
        /// Return a list of dynamic tokens. This method will be removed in future.
        /// </summary>
        /// <param name="list">List of node</param>
        /// <returns>List of dynamic tokens</returns>
        private static TokenSeq DymTokens(List<SyntaxNodeOrToken> list)
        {
            var tokens = new List<Spg.ExampleRefactoring.Tok.Token>();
            foreach (var item in list)
            {
                RawDymToken t = new RawDymToken(item);
                tokens.Add(t);
            }

            TokenSeq seq = new TokenSeq(tokens);
            return seq;
        }

        /// <summary>
        /// Syntax node factory. This method will be removed in future
        /// </summary>
        /// <param name="kind">SyntaxKind of the node that will be created.</param>
        /// <param name="children">Children nodes.</param>
        /// <returns>A SyntaxNode with specific king and children</returns>
        private static SyntaxNodeOrToken GetSyntaxElement(SyntaxKind kind, List<SyntaxNodeOrToken> children)
        {
            if (kind == SyntaxKind.ExpressionStatement)
            {
                ExpressionSyntax expression = (ExpressionSyntax)children.First();
                ExpressionStatementSyntax expressionStatement = SyntaxFactory.ExpressionStatement(expression);
                return expressionStatement;
            }

            if (kind == SyntaxKind.InvocationExpression)
            {
                IdentifierNameSyntax identifier = (IdentifierNameSyntax)children[0]; //identifier name
                ArgumentListSyntax argumentList = (ArgumentListSyntax)children[1]; //argument list
                var invocation = SyntaxFactory.InvocationExpression(identifier, argumentList);
                return invocation;
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
                IdentifierNameSyntax s = (IdentifierNameSyntax)children.First();
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
            return null;
        }
    }
}
