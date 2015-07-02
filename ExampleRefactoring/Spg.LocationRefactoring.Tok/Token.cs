using System.Collections.Generic;
using ExampleRefactoring.Spg.ExampleRefactoring.AST;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Comparator;

namespace Spg.ExampleRefactoring.Tok
{
    /// <summary>
    /// Token
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Get or set token
        /// </summary>
        /// <returns>Get or set token</returns>
        public SyntaxNodeOrToken token { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token"></param>
        public Token(SyntaxNodeOrToken token)
        {
            this.token = token;
        }

        /// <summary>
        /// Match method
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public virtual bool Match(SyntaxNodeOrToken st)
        {
            if (token.AsNode() == null)
            {
                return ASTManager.Parent(this.token).RawKind == ASTManager.Parent(token).RawKind && (st.RawKind == token.RawKind);
            }

            return token.RawKind == ASTManager.Parent(st).RawKind;
        }

        /// <summary>
        /// Sub nodes that matches with this token
        /// </summary>
        /// <param name="nodes">Nodes</param>
        /// <returns>Sub nodes that matches with this token</returns>
        public virtual ListNode Match(ListNode nodes)
        {
            ListNode listNode;
            if (nodes.List.Count == 0) {
                listNode = new ListNode();
                return listNode;
            }

            //the syntax node or token is a token
            if (token.AsNode() == null)
            {
                SyntaxNodeOrToken firstNode = nodes.List[0];
                if (Comparer().IsEqual(firstNode, this.token))
                {
                    List<SyntaxNodeOrToken> temp = new List<SyntaxNodeOrToken>();
                    temp.Add(firstNode);
                    listNode = new ListNode(temp);
                    return listNode;
                }

                listNode = new ListNode();
                return listNode;
            }

            int i = 0;
            while (i < nodes.List.Count && Comparer().IsEqual(nodes.List[i], token))
            {
                i++;
            }

            listNode = ASTManager.SubNotes(nodes, 0, i);

            return listNode;
        }

        /// <summary>
        /// Regular expression comparer
        /// </summary>
        /// <returns>Regular expression comparer</returns>
        public virtual ComparerBase Comparer() {
            return new RegexComparer();
        }

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return token.CSharpKind().ToString();
        }

        /// <summary>
        /// Two tokens are equal
        /// </summary>
        /// <param name="otherToken">Token</param>
        /// <returns>True is tokens are equal</returns>
        public override bool Equals(object otherToken)
        {
            Token st = (Token) otherToken;
            ComparerBase comparator = new RegexComparer();
            return comparator.IsEqual(this.token, st.token);
        }

        /// <summary>
        /// Hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return token.ToString().GetHashCode();
        }

    }
}
