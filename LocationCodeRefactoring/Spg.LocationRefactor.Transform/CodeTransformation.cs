using System;
using Spg.LocationRefactor.Location;

namespace Spg.LocationRefactor.Transform
{
    /// <summary>
    /// Represents a transformation
    /// </summary>
    public class CodeTransformation
    {
        /// <summary>
        /// Location to be transformed
        /// </summary>
        /// <returns></returns>
        public CodeLocation location { get; set; }

        /// <summary>
        /// Before and after transformation
        /// </summary>
        /// <returns>Before and after transformation</returns>
        public Tuple<string, string> transformation { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="location">Location</param>
        /// <param name="transformation">Transformation</param>
        public CodeTransformation(CodeLocation location, Tuple<string, string> transformation)
        {
            this.location = location;
            this.transformation = transformation;
        }
    }
}


