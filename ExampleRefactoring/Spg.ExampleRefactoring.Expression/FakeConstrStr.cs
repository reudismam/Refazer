using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Comparator;

namespace Spg.ExampleRefactoring.Expression
{
    /// <summary>
    /// ConstruStr expression
    /// </summary>
    public class FakeConstrStr : ConstruStr
    {
        /// <summary>
        /// Construct a string with the passed word.
        /// </summary>
        /// <param name="nodes">Nodes to represent the regular expression.</param>
        public FakeConstrStr(ListNode nodes):base(nodes) {
        }


        /// <summary>
        /// Return the internal List
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Internal list</returns>
        public override ListNode RetrieveSubNodes(ListNode input) {
            ListNode replace = new ListNode();
            if (Nodes.Length() == 1 && Nodes.List[0].IsKind(SyntaxKind.IdentifierToken))
            {
                SyntaxNodeOrToken identifier = Nodes.List[0];
                var newLiteral = SyntaxFactory.ParseExpression(@"var _v1 = 0;");
                List<SyntaxNodeOrToken> listTokens = ASTManager.EnumerateSyntaxNodesAndTokens(newLiteral, new List<SyntaxNodeOrToken>());
                SyntaxNodeOrToken id = listTokens[1];

                id = id.WithLeadingTrivia(identifier.GetLeadingTrivia())
                    .WithTrailingTrivia(identifier.GetTrailingTrivia())
                    .WithAdditionalAnnotations(Formatter.Annotation);
                replace.List.Add(id);
            }
            else
            {
                throw new IndexOutOfRangeException("Cannot convert nodes to literal stirng.");
            }
            return replace;
        }

        /// <summary>
        /// To String
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return "FakeConstructNodes(" + Nodes + ")";
        }

        /// <summary>
        /// Equals method
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>True is obj is equals to this object</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is FakeConstrStr))
            {
                return false;
            }
            ConstruStr another = obj as ConstruStr;

            NodeComparer compare = new NodeComparer();
            return compare.SequenceEqual(another.Nodes, this.Nodes);
        }

        /// <summary>
        /// Hash code method
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}



