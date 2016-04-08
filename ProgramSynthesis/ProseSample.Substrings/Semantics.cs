using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactoring.Tok;
using Spg.ExampleRefactoring.RegularExpression;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProseSample.Substrings
{
    public static class Semantics
    {
        #region Concatenation Operators
        public static MatchResult C1(SyntaxNodeOrToken n, string kind, MatchResult expression)
        {
            SyntaxKind skind;
            Enum.TryParse(kind, out skind);

            var kinds = from k in n.AsNode().DescendantNodes()
                        where k.IsKind(skind)
                        select k;

            var klist = kinds.ToList();

            foreach (var kindMatch in klist)
            {
                if (MatchChildren(kindMatch, expression.match.Item1))
                {
                    Tuple<SyntaxNodeOrToken, Bindings> match = Tuple.Create<SyntaxNodeOrToken, Bindings>(kindMatch, null);
                    MatchResult matchResult = new MatchResult(match);
                    return matchResult;
                }
            }
            return null;
        }

        public static MatchResult C2(SyntaxNodeOrToken n, string kind, MatchResult expression1, MatchResult expression2)
        {
            SyntaxKind skind;
            Enum.TryParse(kind, out skind);

            var kinds = from k in n.AsNode().DescendantNodes()
                        where k.IsKind(skind)
                        select k;

            var klist = kinds.ToList();

            foreach (var kindMatch in klist)
            {
                if (MatchChildren(kindMatch, expression1.match.Item1) && MatchChildren(kindMatch, expression2.match.Item1))
                {
                    Tuple<SyntaxNodeOrToken, Bindings> match = Tuple.Create<SyntaxNodeOrToken, Bindings>(kindMatch, null);
                    MatchResult matchResult = new MatchResult(match);
                    return matchResult;
                }
            }

            return null;
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

        #endregion

        #region Literal Operator
        /// <summary>
        /// Build a indentifier
        /// </summary>
        /// <param name="name">Identifier name</param>
        /// <returns>A new identifier</returns>
        public static SyntaxNodeOrToken Identifier(string name)
        {
            SyntaxNode n = SyntaxFactory.IdentifierName(name);
            return n;
        }

        /// <summary>
        /// Build a prededefined type
        /// </summary>
        /// <param name="type"></param>
        /// <returns>A new predefined type</returns>
        public static SyntaxNodeOrToken PredefinedType(string type)
        {
            SyntaxToken predType = SyntaxFactory.ParseToken(type);
            SyntaxNode n = SyntaxFactory.PredefinedType(predType);
            return n;
        }

        /// <summary>
        /// Build a numeric literal expression
        /// </summary>
        /// <param name="number">Literal expression</param>
        /// <returns>A new numeric literal expression</returns>
        public static SyntaxNodeOrToken NumericLiteralExpression(string number)
        {
            double d = double.Parse(number);
            SyntaxNode numericLiteral = SyntaxFactory.ParseExpression(d.ToString(CultureInfo.InvariantCulture));
            return numericLiteral;
        }

        /// <summary>
        /// Build a string literal expression
        /// </summary>
        /// <param name="s">Literal expression</param>
        /// <returns>A new numeric literal expression</returns>
        public static SyntaxNodeOrToken StringLiteralExpression(string s)
        {
            SyntaxNode stringLiteralExpression = SyntaxFactory.ParseExpression(s.ToString(CultureInfo.InvariantCulture));
            return stringLiteralExpression;
        }

        /// <summary>
        /// Build a string literal expression
        /// </summary>
        /// <param name="s">Literal expression</param>
        /// <returns>A new numeric literal expression</returns>
        public static SyntaxNodeOrToken Block(string s)
        {
            var block = SyntaxFactory.Block();
            return block;
        }

        /// <summary>
        /// Build a literal
        /// </summary>
        /// <param name="n">Input node</param>
        /// <param name="node">Literal itself.</param>
        /// <returns>Match of parameter literal in the source code.</returns>
        public static MatchResult Literal(SyntaxNodeOrToken n, SyntaxNodeOrToken node)
        {
            Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken> tuple = Tuple.Create(n, node);
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
        #endregion

        #region Edit Operators       
        /// <summary>
        /// Insert the ast node before the matching
        /// </summary>
        /// <param name="n">Input tree node</param>
        /// <param name="mresult">The result of a matching</param>
        /// <param name="ast">Node that will be inserted</param>
        /// <returns>New node with the ast node inserted before the matching</returns>
        public static SyntaxNodeOrToken InsertBefore(SyntaxNodeOrToken n, MatchResult mresult, SyntaxNodeOrToken ast)
        {
            List<SyntaxNode> nodes = new List<SyntaxNode> { ast.AsNode() };

            SyntaxNodeOrToken node = mresult.match.Item1;
            var root = n.AsNode();
            root = root.InsertNodesBefore(root.FindNode(node.Span), nodes);

            return root.NormalizeWhitespace();
        }

        /// <summary>
        /// Insert the ast node as in the k position of the node in the matching result 
        /// </summary>
        /// <param name="n">Input data</param>
        /// <param name="k">Position in witch the node will be inserted.</param>
        /// <param name="mresult">Matching result</param>
        /// <param name="ast">Node that will be insert</param>
        /// <returns>New node with the ast node inserted as the k child</returns>
        public static SyntaxNodeOrToken Insert(SyntaxNodeOrToken n, int k, MatchResult mresult, SyntaxNodeOrToken ast)
        {
            SyntaxNodeOrToken node = mresult.match.Item1;

            List<SyntaxNode> nodes = new List<SyntaxNode> { ast.AsNode() };

            var root = node.AsNode();

            if (root.ChildNodes().Count() >= k)
            {
                var select = root.ChildNodes().ElementAt(k - 1);
                root = root.InsertNodesBefore(root.FindNode(select.Span), nodes);
            }
            else
            {
                //TODO decide what to do what the children are empty.
            }

            return root.NormalizeWhitespace();
        }

        #endregion

        #region Script Operators

        public static SyntaxNodeOrToken Script1(SyntaxNodeOrToken n, SyntaxNodeOrToken edit)
        {
            return edit;
        }

        #endregion

        #region Node Operators
        /// <summary>
        /// Return a new node
        /// </summary>
        /// <param name="kind">Returned node SyntaxKind</param>
        /// <param name="child">Child of the returned SyntaxNode</param>
        /// <returns>A new node with kind and child</returns>
        public static SyntaxNodeOrToken Node1(string kind, SyntaxNodeOrToken child)
        {
            SyntaxKind skind;
            Enum.TryParse(kind, out skind);
            List<SyntaxNodeOrToken> children = new List<SyntaxNodeOrToken> { child };
            var node = GetSyntaxElement(skind, children);
            return node;
        }

        /// <summary>
        /// Return a new node
        /// </summary>
        /// <param name="kind">SyntaxKind of the node that will be returned</param>
        /// <param name="child">First child of the node</param>
        /// <param name="child2">Second child of the node</param>
        /// <returns>A node with two child and SyntaxKind passed as parameter</returns>
        public static SyntaxNodeOrToken Node2(string kind, SyntaxNodeOrToken child, SyntaxNodeOrToken child2)
        {
            SyntaxKind skind;
            Enum.TryParse(kind, out skind);
            List<SyntaxNodeOrToken> children = new List<SyntaxNodeOrToken> { child, child2 };
            var node = GetSyntaxElement(skind, children);
            return node;
        }

        #endregion

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

        #region SplitNodes
        public static IEnumerable<SyntaxNodeOrToken> SplitNodes(SyntaxNodeOrToken n)
        {
            SyntaxKind targetKind = SyntaxKind.MethodDeclaration;
            
            SyntaxNode node = n.AsNode();

            var nodes = from snode in node.DescendantNodes()
                        where snode.IsKind(targetKind)
                        select snode;


            return nodes.Select(snot => (SyntaxNodeOrToken) snot).ToList();
        }
        #endregion

        #region Utilities       
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
                var block = SyntaxFactory.Block();
                var ifStatement = SyntaxFactory.IfStatement(condition, block);
                return ifStatement;
            }

            return null;
        }
        #endregion
    }
}
