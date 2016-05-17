using Spg.ExampleRefactoring.Tok;

namespace LongestCommonSubsequence
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

        public int Indicator { get; set; }

        /// <summary>
        /// Indicate if token is contained on input
        /// </summary>
        public static int INPUT = 0;

        /// <summary>
        /// Indicates if token is contained on output
        /// </summary>
        public static int OUTPUT = 1;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Token">Token element</param>
        /// <param name="Index">Index of the element</param>
        public ComparisonObject(Token Token, int Index, int Indicator)
        {
            this.Token = Token;
            this.Index = Index;
            this.Indicator = Indicator;
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
