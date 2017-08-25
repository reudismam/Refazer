using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

namespace TreeEdit.Spg.LogInfo
{
    /// <summary>
    /// Logs the transformations
    /// </summary>
    public class TransformationsInfo
    {
        /// <summary>
        /// Specifies the transformations performed into the source code in terms of
        /// before and after code fragments
        /// </summary>
        public List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>> Transformations { get; set; }

        public TransformationsInfo()
        {
            Transformations = new List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>>();
        }
        
        /// <summary>
        /// Defines a single instance of this class
        /// </summary>
        private static TransformationsInfo _instance;

        /// <summary>
        /// Get singleton instance
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "ConvertIfStatementToNullCoalescingExpression")]
        public static TransformationsInfo GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TransformationsInfo();
            }
            return _instance;
        }

        /// <summary>
        /// Initiates singleton instance
        /// </summary>
        public void  Init()
        {
            Transformations = new List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>>();
        }

        /// <summary>
        /// Adds a new transformation to the list of transformations
        /// </summary>
        /// <param name="transformation">Transformation to be added</param>
        public void Add(Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken> transformation)
        {
            Transformations.Add(transformation);
        }
    }
}
