﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleRefactoring.Spg.ExampleRefactoring.Bean
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
        /// <param name="Start">Selection start</param>
        /// <param name="Length">Selection Length</param>
        /// <param name="SourcePath">Path to the class file</param>
        /// <param name="SourceCode">Source code</param>
        public Selection(int Start, int Length, string SourcePath, string SourceCode )
        {
            this.Start = Start;
            this.Length = Length;
            this.SourcePath = SourcePath;
            this.SourceCode = SourceCode;
        }
    }
}