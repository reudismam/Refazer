using System;
using Spg.LocationRefactor.Location;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Transform
{
    /// <summary>
    /// Represents a BeforeAfter
    /// </summary>
    public class CodeTransformation
    {
        public TRegion Trans { get; set; }

        /// <summary>
        /// Location to be transformed
        /// </summary>
        /// <returns></returns>
        public CodeLocation Location { get; set; }

        /// <summary>
        /// Before and after BeforeAfter
        /// </summary>
        /// <returns>Before and after BeforeAfter</returns>
        public Tuple<string, string> Transformation { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="location">Location</param>
        /// <param name="trans">Location of the BeforeAfter</param>
        /// <param name="transformation">Transformation</param>
        public CodeTransformation(CodeLocation location, TRegion trans, Tuple<string, string> transformation)
        {
            this.Trans = trans;
            this.Location = location;
            this.Transformation = transformation;
        }
    }
}


