using Spg.ExampleRefactoring.Synthesis;

namespace Spg.ExampleRefactoring.Position
{
    /// <summary>
    /// CPos positioning element
    /// </summary>
    public class CPos: IPosition
    {
        /// <summary>
        /// position inside the string.
        /// </summary>
        public int position {get; set;}

        /// <summary>
        /// Construct a position expression
        /// </summary>
        /// <param name="position">Position that the expression represents</param>
        public CPos(int position) {
            this.position = position;
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

            if(position < 0){
                int index = input.Length() + position + 1;
                return index;
            }

            return position;
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
            return "CPos(" + this.position + ")";
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
            return another.position == this.position;
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
