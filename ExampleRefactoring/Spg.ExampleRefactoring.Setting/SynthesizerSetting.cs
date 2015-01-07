using System;

namespace Spg.ExampleRefactoring.Setting
{
    /// <summary>
    /// Synthesizer setting
    /// </summary>
    public class SynthesizerSetting
    {
        /// <summary>
        /// Indicate if we are using dynamic tokens or not
        /// </summary>
        /// <returns>Get of set dynamic tokens</returns>
        public Boolean dynamicTokens { get; set; }
        /// <summary>
        /// Indicate if we are using ConstStr expression or not
        /// </summary>
        /// <returns>Get or set consider ConstrStr</returns>
        public Boolean considerConstrStr { get; set; }
        /// <summary>
        /// Indicate if we are using empty regular expression or not
        /// </summary>
        /// <returns>Get or set consider empty</returns>
        public Boolean considerEmpty { get; set; }
        /// <summary>
        /// Indicate the value of deviation when generating positions
        /// </summary>
        /// <returns>Get or set deviation value</returns>
        public int deviation { get; set; }

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
        public SynthesizerSetting(Boolean dynamicTokens, int deviation, Boolean considerConstrStr, Boolean considerEmpty)
        {
            this.dynamicTokens = dynamicTokens;
            this.deviation = deviation;
            this.considerConstrStr = considerConstrStr;
            this.considerEmpty = considerEmpty;
        }
    }
}
