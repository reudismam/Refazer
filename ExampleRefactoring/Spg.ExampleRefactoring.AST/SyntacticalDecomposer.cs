using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Synthesis;

namespace Spg.ExampleRefactoring.AST
{
    /// <summary>
    /// Decompose token into syntactical elements
    /// </summary>
    [Obsolete("Not used anymore", true)]
    public class SyntacticalDecomposer
    {
        /*/// <summary>
        /// Extract the position of syntactical element with specified depth in ListNode
        /// </summary>
        /// <param name="listNode">ListNode</param>
        /// <param name="depth">Depth</param>
        /// <returns>
        /// Positions
        /// </returns>
        public static List<int> ExtractPositions(ListNode listNode, int depth)
        {
            List<int> positions = new List<int>();

            int current = 0;
            SyntaxNodeOrToken stCurrent = listNode.List[0];
            for ( int i = 1; i < listNode.List.Count; i++)
            {
                SyntaxNodeOrToken st = listNode.List[i];
                if (!(Parent(stCurrent, depth) == Parent(st, depth)))
                {
                    positions.Add(i);
                    current = i;
                    stCurrent = st;
                }
            }

            return positions;
        }

        /// <summary>
        /// Get parent with the specified depth
        /// </summary>
        /// <param name="st">Syntax node or token</param>
        /// <param name="depth">Depth</param>
        /// <returns>Parent with the depth</returns>
        private static SyntaxNode Parent(SyntaxNodeOrToken st, int depth)
        {
            int i = 0;
            SyntaxNodeOrToken parent = null;
            while (i < depth)
            {
                parent = ASTManager.Parent(st);
                i++;
            }

            return parent.AsNode();
        }*/
    }
}
