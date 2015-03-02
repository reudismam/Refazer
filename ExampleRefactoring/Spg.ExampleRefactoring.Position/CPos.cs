using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Position;

namespace ExampleRefactoring.Spg.ExampleRefactoring.Position
{
    /// <summary>
    /// CPos positioning element
    /// </summary>
    public class CPos: IPosition
    {
        /// <summary>
        /// position inside the string.
        /// </summary>
        public int Position {get; set;}

        /// <summary>
        /// Construct a position expression
        /// </summary>
        /// <param name="position">Position that the expression represents</param>
        public CPos(int position) {
            this.Position = position;
        }


        /// <summary>
        /// Get the position index
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Index of position</returns>
        public int GetPositionIndex(ListNode input) {
            if (input == null)
            {
                return -1;
            }

            if(Position < 0){
                int index = input.Length() + Position + 1;
                return index;
            }

            return Position;
        }

        /// <summary>
        /// Returns the size of the string.
        /// </summary>
        /// <returns>This CPos(size)</returns>
        public int Size() {
            return 1;
        }

        /// <summary>
        /// To String method
        /// </summary>
        /// <returns>String representation of this object</returns>
        public override string ToString()
        {
            return "CPos(" + this.Position + ")";
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">Another object</param>
        /// <returns>True if the another object is equal to this</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is CPos))
            {
                return false;
            }

            CPos another = obj as CPos;
            return another.Position == this.Position;
        }

        /// <summary>
        /// Hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
