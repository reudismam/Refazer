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

        private CodeFragmentsInfo()
        {
            Locations = new List<SyntaxNodeOrToken>();
        }

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
        /// <param name="newLocation">Location to be added</param>
        public void Add(SyntaxNodeOrToken newLocation)
        {
            bool toAdd = true;
            for (int i = 0; i < Locations.Count; i++)
            {
                SyntaxNodeOrToken currentLocation = Locations[i];
                if (isInside(currentLocation, newLocation))
                {
                    Locations[i] = newLocation;
                    toAdd = false;
                    break;
                }
                if (isInside(newLocation, currentLocation))
                {
                    toAdd = false;
                    break;
                }
            }
            if (toAdd)
            {
                Locations.Add(newLocation);
            }
            //Locations.Add(newLocation);
        }

        /// <summary>
        /// Determines if a location is inside other location
        /// </summary>
        /// <param name="outer"></param>
        /// <param name="inner"></param>
        public bool isInside(SyntaxNodeOrToken outer, SyntaxNodeOrToken inner)
        {
            if (!outer.SyntaxTree.FilePath.ToUpperInvariant().Equals(inner.SyntaxTree.FilePath.ToUpperInvariant()))
            {
                return false;
            }
            int locationStart = outer.SpanStart;
            int locationEnd = outer.SpanStart + outer.Span.Length;
            int locStart = inner.SpanStart;
            int locEnd = inner.SpanStart + inner.Span.Length;
            return locationStart <= locStart && locEnd <= locationEnd;    
        }
    }
}
