using System;
using System.Collections.Generic;
using Spg.ExampleRefactoring.AST;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Comparator;

namespace Spg.ExampleRefactoring.Synthesis
{
    /// <summary>
    /// Representation of a list of nodes
    /// </summary>
    public class ListNode
    {
        /// <summary>
        /// Get or set the list of nodes
        /// </summary>
        public List<SyntaxNodeOrToken> List { get; set; }

        /// <summary>
        /// Original text
        /// </summary>
        /// <returns>Original text</returns>
        public string OriginalText { get; set; }

        /// <summary>
        /// Length of ListNode
        /// </summary>
        /// <returns>Length of ListNode</returns>
        public int Length() {
            return List.Count;
        }

        /// <summary>
        /// List constructor
        /// </summary>
        /// <param name="list">List</param>
        public ListNode(List<SyntaxNodeOrToken> list)
        {
            this.List = list;
        }

        /// <summary>
        /// Simple constructor
        /// </summary>
        public ListNode()
        {
            this.List = new List<SyntaxNodeOrToken>();
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">Another object</param>
        /// <returns>True is object is equal to this</returns>
        public override bool Equals(object obj)
        {
            ListNode lnode = (ListNode)obj;
            if (List.Count > 0)
            {
                bool isEqual = (ASTManager.Matches(this, lnode, new NodeComparer()).Count > 0);
                return isEqual;
            }else
            {
                if(lnode.List.Count == 0)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            string s = "[";
            for (int i = 0; i < List.Count - 1; i++)
            {
                SyntaxNodeOrToken st = List[i];
                s += st.CSharpKind() + ", "; 
            }

            if (List.Count > 0) {
                s += List[List.Count - 1].CSharpKind() + "]";
            }
            return s;
        }

        /// <summary>
        /// Hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}




