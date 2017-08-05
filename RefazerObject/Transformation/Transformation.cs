using System;

namespace RefazerUnitTests.Spg.Transform
{
    /// <summary>
    /// Represents a BeforeAfter
    /// </summary>
    public class Transformation
    {
        /// <summary>
        /// Before and after source code BeforeAfter
        /// </summary>
        /// <returns>Before and after source code BeforeAfter</returns>
        public Tuple<string, string> BeforeAfter { get; set; }

        /// <summary>
        /// Source path
        /// </summary>
        /// <returns>Source path</returns>
        public string SourcePath { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="beforeAfter">Before and after BeforeAfter</param>
        /// <param name="sourcePath">Source path</param>
        public Transformation(Tuple<string, string> beforeAfter, string sourcePath)
        {
            BeforeAfter = beforeAfter;
            SourcePath = sourcePath;
        }
    }
}


