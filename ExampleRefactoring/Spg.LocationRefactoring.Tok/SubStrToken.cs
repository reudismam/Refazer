using Spg.ExampleRefactoring.AST;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Tok;

namespace Spg.LocationRefactoring.Tok
{
    /// <summary>
    /// Represent dynamic token
    /// </summary>
    public class SubStrToken : Token
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">Dynamic token</param>
        public SubStrToken(SyntaxNodeOrToken token) : base(token){}

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
            return new SubStrNodeComparer();
        }

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return "SubStrTok(" + token + ")";
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
            ComparerBase comparator = new SubStrNodeComparer();
            bool isEqual = comparator.IsEqual(this.token, st.token);
            return isEqual;
        }
    }
}

