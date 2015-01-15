using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spg.LocationRefactor.Location;

namespace LocationCodeRefactoring.Spg.LocationRefactor.Transformation
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
