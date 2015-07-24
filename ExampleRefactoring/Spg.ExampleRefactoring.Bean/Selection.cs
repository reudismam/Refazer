using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Spg.ExampleRefactoring.Bean
{
    /// <summary>
    /// Represents a selection
    /// </summary>
    public class Selection
    {
        /// <summary>
        /// Selection start
        /// </summary>
        /// <returns>start of the selection</returns>
        public int Start { get; set; }

        /// <summary>
        /// Selection length
        /// </summary>
        /// <returns>Length of the selection</returns>
        public int Length { get; set; }

        /// <summary>
        /// Path to the source code of a class
        /// </summary>
        /// <returns>Path to the source code of a class</returns>
        public string SourcePath { get; set; }

        /// <summary>
        /// Source code of class that has the selection
        /// </summary>
        /// <returns>Source code of class that has the selection</returns>
        public string SourceCode { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="start">Selection start</param>
        /// <param name="length">Selection Length</param>
        /// <param name="sourcePath">Path to the class file</param>
        /// <param name="sourceCode">Source code</param>
        public Selection(int start, int length, string sourcePath, string sourceCode )
        {
            this.Start = start;
            this.Length = length;
            this.SourcePath = sourcePath;
            this.SourceCode = sourceCode;
        }

        public override bool Equals(object obj)
        {
            if(!(obj is Selection)) return false;

            Selection another = (Selection) obj;

            return Start == another.Start && Length == another.Length && SourcePath.ToUpperInvariant().Equals(another.SourcePath.ToUpperInvariant());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return "Start: " + Start + "\n" + "Length: " + Length + "\n" + "Source Path: " + SourcePath + "\n";
        }
    }
}

