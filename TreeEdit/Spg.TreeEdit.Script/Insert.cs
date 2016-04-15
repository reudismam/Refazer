﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace TreeEdit.Spg.TreeEdit.Script
{
    public class Insert : EditOperation
    {
        /// <summary>
        /// Construct a new insert object
        /// </summary>
        /// <param name="insertedNode">Node that will be inserted</param>
        /// <param name="parent">Parent of the node that will be inserted</param>
        /// <param name="k">Position where the node will be inserted</param>
        public Insert(SyntaxNodeOrToken insertedNode, SyntaxNodeOrToken parent, int k) : base(insertedNode, parent, k)
        {
        }

        /// <summary>
        /// String representation of this object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Insert(" + T1Node.Kind() + ", " + Parent.Kind() + ", " + K + ")";
        }
    }
}