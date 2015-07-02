using ExampleRefactoring.Spg.ExampleRefactoring.AST;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Tok;

namespace Spg.LocationRefactoring.Tok
{
    /// <summary>
    /// Represent dynamic token
    /// </summary>
    public class IdenToStrToken: Token
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">Dynamic token</param>
        public IdenToStrToken(SyntaxNodeOrToken token) : base(token)
        {
        }

        /// <summary>
        /// Match
        /// </summary>
        /// <param name="st">syntax node or token</param>
        /// <returns>True if a match exists</returns>
        public override bool Match(SyntaxNodeOrToken st)
        {
            IdenToStrToken dymToken = new IdenToStrToken(st);
            return Equals(dymToken);
        }

        /// <summary>
        /// Return a node comparer
        /// </summary>
        /// <returns>Node comparer</returns>
        public override ComparerBase Comparer()
        {
            return new IdenToStrComparer();
        }

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return "DymTok(" + token + ")";
        }

        /// <summary>
        /// Hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            //return this.ToString().GetHashCode();
            return base.GetHashCode();
        }

        /// <summary>
        /// Two DymToken are equal
        /// </summary>
        /// <param name="obj">Dynamic token</param>
        /// <returns>True if dynamic tokens are equal</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is DymToken))
            {
                return false;
            }
           
            DymToken st = (DymToken)obj;
            ComparerBase comparator = new IdenToStrComparer();
            return comparator.IsEqual(this.token, st.token) && ASTManager.Parent(this.token).RawKind == ASTManager.Parent(st.token).RawKind;
        }
    }
}
