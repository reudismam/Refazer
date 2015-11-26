using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Comparator;

namespace Spg.LocationRefactoring.Tok
{
    /// <summary>
    /// Represent dynamic token
    /// </summary>
    public class RawDymToken: DymToken
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">Dynamic token</param>
        /// <param name="getFullyQualifiedName">Indicate if fully qualified name have to be used</param>
        public RawDymToken(SyntaxNodeOrToken token) : base(token, false)
        {
        }

        /// <summary>
        /// Match
        /// </summary>
        /// <param name="st">syntax node or token</param>
        /// <returns>True if a match exists</returns>
        public override bool Match(SyntaxNodeOrToken st)
        {
            RawDymToken dymToken = new RawDymToken(st);
            return Equals(dymToken);
        }

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return "RawDymTok(" + token + ")";
        }

        /// <summary>
        /// Hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Two DymToken are equal
        /// </summary>
        /// <param name="obj">Dynamic token</param>
        /// <returns>True if dynamic tokens are equal</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is RawDymToken))
            {
                return false;
            }

            RawDymToken st = (RawDymToken)obj;
            ComparerBase comparator = new NodeComparer();
            return comparator.IsEqual(this.token, st.token);
        }
    }
}

