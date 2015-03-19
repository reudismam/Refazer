using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Tok;

namespace Spg.LocationRefactoring.Tok
{
    /// <summary>
    /// Represent dynamic token
    /// </summary>
    public class ArgumentToken : Token
    {
        //private SyntaxNodeOrToken _identifier;

        //public ArgumentToken(SyntaxNodeOrToken token, SyntaxNodeOrToken identifier) : base(token)
        //{
        //    _identifier = identifier;
        //}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">Dynamic token</param>
        public ArgumentToken(SyntaxNodeOrToken token) : base(token)
        {
            
        }

        /// <summary>
        /// Match
        /// </summary>
        /// <param name="st">syntax node or token</param>
        /// <returns>True if a match exists</returns>
        public override bool Match(SyntaxNodeOrToken st)
        {
            ArgumentToken argumentToken = new ArgumentToken(st);
            return Equals(argumentToken);
        }

        /// <summary>
        /// Return a node comparer
        /// </summary>
        /// <returns>SubStrNode comparer</returns>
        public override ComparerBase Comparer()
        {
            return new ArgumentComparer();
        }

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return "ArgumentToken";
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
            Token st = obj as Token;
            ComparerBase comparator = new ArgumentComparer();
            bool isEqual = comparator.IsEqual(this.token, st.token);
            return isEqual;
        }
    }
}
