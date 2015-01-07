using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Tok;

namespace Spg.LocationRefactoring.Tok
{
    /// <summary>
    /// Represent dynamic token
    /// </summary>
    public class DymToken: Token
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">Dynamic token</param>
        public DymToken(SyntaxNodeOrToken token) : base(token) {
        }

        /// <summary>
        /// Match
        /// </summary>
        /// <param name="st">syntax node or token</param>
        /// <returns>True if a match exists</returns>
        public override bool Match(SyntaxNodeOrToken st)
        {
            DymToken dymToken = new DymToken(st);
            return Equals(dymToken);
        }

        /// <summary>
        /// Return a node comparer
        /// </summary>
        /// <returns>Node comparer</returns>
        public override ComparerBase Comparer()
        {
            return new NodeComparer();
        }

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return "DymTok(" + token.ToString() + ")";
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
            if (!(obj is DymToken))
            {
                return false;
            }

            DymToken st = (DymToken)obj;
            ComparerBase comparator = new NodeComparer();
            return comparator.IsEqual(this.token, st.token);
        }
    }
}
