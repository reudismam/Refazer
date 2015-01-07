using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spg.ExampleRefactoring.Tok;

namespace LCS2
{
    /// <summary>
    /// Comparison object
    /// </summary>
    public class ComparisonObject
    {
        /// <summary>
        /// Token element
        /// </summary>
        /// <returns>Token element</returns>
        public Token Token { get; set; }

        /// <summary>
        /// Index element
        /// </summary>
        /// <returns>Index element</returns>
        public int Index { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Token">Token element</param>
        /// <param name="Index">Index of the element</param>
        public ComparisonObject(Token Token, int Index)
        {
            this.Token = Token;
            this.Index = Index;
        }

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return Token.ToString();
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>True if object is equals</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ComparisonObject))
            {
                return false;
            }

            ComparisonObject another = obj as ComparisonObject;
            return Token.Equals(another.Token);
        }

        /// <summary>
        /// Hash code method
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return Token.GetHashCode();
        }
    }
}
