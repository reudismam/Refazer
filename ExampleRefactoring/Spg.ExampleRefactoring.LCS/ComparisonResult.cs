using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCS2
{
    /// <summary>
    /// Comparison result
    /// </summary>
    /// <typeparam name="T">Object to be compared</typeparam>
    public class ComparisonResult<T>
    {
        /// <summary>
        /// Modification type
        /// </summary>
        /// <returns>Modification type</returns>
        public ModificationType ModificationType { get; set; }

        /// <summary>
        /// Data compared
        /// </summary>
        /// <returns>Object compared</returns>
        public T DataCompared { get; set; }
    }
}
