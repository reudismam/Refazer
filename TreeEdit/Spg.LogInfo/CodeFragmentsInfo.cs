using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

namespace TreeEdit.Spg.Log
{
    /// <summary>
    /// Logs the node that will be modified
    /// </summary>
    public class CodeFragmentsInfo
    {
        /// <summary>
        /// Specifies the node that will be modified
        /// </summary>
        public List<SyntaxNodeOrToken> Locations { get; set; }
        
        /// <summary>
        /// Defines a single instance of this class
        /// </summary>
        private static CodeFragmentsInfo _instance;

        /// <summary>
        /// Get singleton instance
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "ConvertIfStatementToNullCoalescingExpression")]
        public static CodeFragmentsInfo GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CodeFragmentsInfo();
            }
            return _instance;
        }

        /// <summary>
        /// Initiates singleton instance
        /// </summary>
        public void  Init()
        {
            Locations = new List<SyntaxNodeOrToken>();
        }

        /// <summary>
        /// Adds a new location to the list of locations
        /// </summary>
        /// <param name="location">Location to be added</param>
        public void Add(SyntaxNodeOrToken location)
        {
            Locations.Add(location);
        }
    }
}
