using System;
using System.Collections.Generic;
using ExampleRefactoring.Spg.ExampleRefactoring.AST;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Synthesis;

namespace Spg.ExampleRefactoring.Comparator
{
    /// <summary>
    /// Comparer for regular expression
    /// </summary>
    public class RegexEndComparer: RegexComparer
    {
        /// <summary>
        /// Input nodes ends with sub nodes
        /// </summary>
        /// <param name="input">Input nodes</param>
        /// <param name="subNodes">Sub nodes</param>
        /// <returns>True if input nodes ends with sub nodes</returns>
        public override List<int> Matches(ListNode input, ListNode subNodes) {
            List<int> matches = base.Matches(input, subNodes);

            if (matches != null && matches.Count > 0)
            {
                int i = matches[matches.Count - 1];
                int j = (input.Length() - i);
                ListNode tokens = ASTManager.SubNotes(input, i, j);
                Boolean isMatch = SequenceEqual(tokens, subNodes);

                matches = new List<int>();

                if (isMatch)
                {
                    matches.Add(i);
                }  
            }
            return matches;
        }
    }
}
