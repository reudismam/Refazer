using System;
using Microsoft.CodeAnalysis;

namespace Spg.ExampleRefactoring.Change
{

    /// <summary>
    /// Manage abstract syntactic tree changes
    /// </summary>
    [Obsolete("Not used anymore",true)]
    public static class ChangeManager
    {
        /// <summary>
        /// Methods where changes exists
        /// </summary>
        /// <param name="st1">First syntax tree</param>
        /// <param name="st2">Second syntax tree</param>
        /// <returns>Methods changed</returns>
        public static Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken> MethodsChanged(SyntaxTree st1, SyntaxTree st2) {
            Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken> tuple = Tuple.Create((SyntaxNodeOrToken) st1.GetRoot(), (SyntaxNodeOrToken) st2.GetRoot());

            return tuple;
        }
    }
}
