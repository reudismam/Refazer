using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;

namespace ExampleRefactoring.Spg.ExampleRefactoring.Position
{
    /// <summary>
    /// IPosition expression
    /// </summary>
    public interface IPosition
    {
        /// <summary>
        /// Get the position index of this position expression
        /// </summary>
        /// <returns>The index of this match.</returns>
        int GetPositionIndex(ListNode input);

        /// <summary>
        /// Return the size of the position expression
        /// </summary>
        /// <returns>The size of the position expression</returns>
        int Size();
    }
}
