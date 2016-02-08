using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleProject.IntelliMeta.ASTManagerChange
{
    class ASTManagerChange
    {
        /// <summary>
        /// Is dynamic token
        /// </summary>
        /// <param name="st">Syntax token or node</param>
        /// <param name="next">Syntax token or node</param>
        /// <returns>True if is a dynamic token</returns>
        private static bool IsDym(SyntaxNodeOrToken st, SyntaxNodeOrToken next)
        {
            if (st == null) { throw new ArgumentNullException("st"); }
            if (next == null) { throw new ArgumentNullException("next"); }

            if (!st.IsKind(SyntaxKind.IdentifierToken)) { return false; }

            SyntaxNodeOrToken parent = ASTManager.Parent(st);

            if (ASTManager.Parent(st).IsKind(SyntaxKind.VariableDeclaration)) { return true; }

            if (ASTManager.Parent(st).IsKind(SyntaxKind.ObjectCreationExpression)) { return true; }

            if (ASTManager.Parent(st).IsKind(SyntaxKind.AttributeList)) { return true; }

            if (ASTManager.Parent(st).IsKind(SyntaxKind.InvocationExpression)) { return true; }

            if (ASTManager.Parent(st).IsKind(SyntaxKind.SimpleMemberAccessExpression))
            {
                string value = next.ToString();
                if (value.Equals("("))
                    return true;
            }
            return false;
        }
    }
}

