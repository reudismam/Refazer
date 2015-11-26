namespace Spg.ExampleRefactoring.Setting
{
    /// <summary>
    /// Synthesizer setting
    /// </summary>
    public class SynthesizerSetting
    {
        public bool _getFullyQualifiedName;

        /// <summary>
        /// Indicates if create token sequence is to be created
        /// </summary>
        /// <returns>True if create token sequence is to be created.</returns>
        public bool CreateTokenSeq { get; set; }

        /// <summary>
        /// Indicate if we are using dynamic tokens or not
        /// </summary>
        /// <returns>Get of set dynamic tokens</returns>
        public bool DynamicTokens { get; set; }

        /// <summary>
        /// Indicate if we are using ConstStr expression or not
        /// </summary>
        /// <returns>Get or set consider ConstrStr</returns>
        public bool ConsiderConstrStr { get; set; }

        /// <summary>
        /// Indicate if we are using empty regular expression or not
        /// </summary>
        /// <returns>Get or set consider empty</returns>
        public bool ConsiderEmpty { get; set; }

        /// <summary>
        /// Indicate the value of deviation when generating positions
        /// </summary>
        /// <returns>Get or set deviation value</returns>
        public int Deviation { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SynthesizerSetting() {
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="dynamicTokens">Indicate if we are using dynamic tokens</param>
        /// <param name="deviation">Deviation value</param>
        /// <param name="considerConstrStr">Indicate if we are using ConstrStr</param>
        /// <param name="considerEmpty">Indicate if we are using empty token or not</param>
        public SynthesizerSetting(bool dynamicTokens, int deviation, bool considerConstrStr, bool considerEmpty, bool createTokenSeq = false)
        {
            this.DynamicTokens = dynamicTokens;
            this.Deviation = deviation;
            this.ConsiderConstrStr = considerConstrStr;
            this.ConsiderEmpty = considerEmpty;
            this.CreateTokenSeq = createTokenSeq;
        }
    }
}

