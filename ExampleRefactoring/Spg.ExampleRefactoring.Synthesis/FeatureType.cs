namespace Spg.ExampleRefactoring.Synthesis
{

    /// <summary>
    /// Feature type
    /// </summary>
    public enum FeatureType
    {
        /// <summary>
        /// ConstStr atomic expression
        /// </summary>
        CONSTSTR,
        /// <summary>
        /// Pos position expression
        /// </summary>
        POS, 
        /// <summary>
        /// CPos absolute position expression
        /// </summary>
        CPOS,
        /// <summary>
        /// CPos(0) absolute position
        /// </summary>
        CPOSBEGIN,
        /// <summary>
        /// CPos(-1) absolute position
        /// </summary>
        CPOSEND,
        /// <summary>
        /// Size
        /// </summary>
        SIZE,
        /// <summary>
        /// Empty expression
        /// </summary>
        EMPTY,
        /// <summary>
        /// Regular expression element
        /// </summary>
        SYNTAX,
        /// <summary>
        /// Dynamic token element
        /// </summary>
        DYMTOKEN
    }
}
