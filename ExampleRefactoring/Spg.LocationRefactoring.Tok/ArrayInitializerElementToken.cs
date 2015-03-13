using ExampleRefactoring.Spg.ExampleRefactoring.AST;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Tok;

namespace Spg.LocationRefactoring.Tok
{
    /// <summary>
    /// Represent dynamic token
    /// </summary>
    public class ArrayInitializerElementToken : Token
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">Dynamic token</param>
        public ArrayInitializerElementToken(SyntaxNodeOrToken token) : base(token){}

        /// <summary>
        /// Match
        /// </summary>
        /// <param name="st">syntax node or token</param>
        /// <returns>True if a match exists</returns>
        public override bool Match(SyntaxNodeOrToken st)
        {
            SubStrToken dymToken = new SubStrToken(st);
            return Equals(dymToken);
        }

        /// <summary>
        /// Return a node comparer
        /// </summary>
        /// <returns>SubStrNode comparer</returns>
        public override ComparerBase Comparer()
        {
            return new ArrayInitializerElementTokenComparer();
        }

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return "ArrayInitializerElementToken";
        }

        /// <summary>
        /// Hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Two DymToken are equal
        /// </summary>
        /// <param name="obj">Dynamic token</param>
        /// <returns>True if dynamic tokens are equal</returns>
        public override bool Equals(object obj)
        {
            //if (!(obj is SubStrToken))
            //{
            //    return false;
            //}

            Token st = obj as Token;
            //bool isEqual =  ASTManager.Parent(st.token).IsKind(SyntaxKind.ArrayInitializerExpression) &&
            //       (st.token.IsKind(SyntaxKind.StringLiteralToken) || st.token.IsKind(SyntaxKind.NullKeyword) ||
            //        st.token.IsKind(SyntaxKind.IdentifierToken));
            //return isEqual;
            ComparerBase comparator = new ArrayInitializerElementTokenComparer();
            bool isEqual = comparator.IsEqual(this.token, st.token);
            return isEqual;
        }
    }
}
