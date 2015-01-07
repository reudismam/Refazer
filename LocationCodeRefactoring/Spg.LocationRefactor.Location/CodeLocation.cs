using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Location
{
    /// <summary>
    /// Represents a location
    /// </summary>
    public class CodeLocation
    {
        /// <summary>
        /// Source code
        /// </summary>
        /// <returns>source code</returns>
        public string SourceCode { get; set; }

        /// <summary>
        /// Region in the source code
        /// </summary>
        /// <returns>Region</returns>
        public TRegion Region { get; set; }

        /// <summary>
        /// Source class
        /// </summary>
        /// <returns>Source class</returns>
        public string SourceClass { get; set; }
    }
}
