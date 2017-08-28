using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using RefazerObject.Transformation;

namespace TreeEdit.Spg.LogInfo
{
    /// <summary>
    /// Logs the transformations
    /// </summary>
    public class TransformationInfos
    {
        /// <summary>
        /// Specifies the transformations performed into the source code in terms of
        /// before and after code fragments
        /// </summary>
        public List<TransformationInfo> Transformations { get; set; }

        public TransformationInfos()
        {
            Transformations = new List<TransformationInfo>();
        }
        
        /// <summary>
        /// Defines a single instance of this class
        /// </summary>
        private static TransformationInfos _instance;

        /// <summary>
        /// Get singleton instance
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "ConvertIfStatementToNullCoalescingExpression")]
        public static TransformationInfos GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TransformationInfos();
            }
            return _instance;
        }

        /// <summary>
        /// Initiates singleton instance
        /// </summary>
        public void  Init()
        {
            Transformations = new List<TransformationInfo>();
        }

        /// <summary>
        /// Adds a new transformation to the list of transformations
        /// </summary>
        /// <param name="transformation">Transformation to be added</param>
        public void Add(TransformationInfo transformation)
        {
            Transformations.Add(transformation);
        }
    }
}
