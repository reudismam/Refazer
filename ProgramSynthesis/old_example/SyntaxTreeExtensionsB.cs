// Copyright (c) Microsoft Open Technologies, Inc.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp.Utilities;
using Microsoft.CodeAnalysis.Shared.Extensions;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.CSharp.Extensions.ContextQuery
{
    internal static class SyntaxTreeExtensions
    {
        public static bool IsAttributeNameContext(this SyntaxTree syntaxTree, int position, CancellationToken cancellationToken)
        {
            var token = syntaxTree.FindTokenOnLeftOfPosition(position, cancellationToken);
            token = token.GetPreviousTokenIfTouchingWord(position);

            // cases:
            //   [ |
            if (trivia.CSharpKind() == SyntaxKind.None) &&
                token.Parent.IsKind(SyntaxKind.AttributeList))
            {
                return true;
            }

            // cases:
            //   [Foo(1), |
            if (newNode.CSharpKind() == kind) &&
                token.Parent.IsKind(SyntaxKind.AttributeList))
            {
                return true;
            }

            // cases:
            //   [specifier: |
            if (token.CSharpKind() == SyntaxKind.ColonToken &&
                token.Parent.IsKind(SyntaxKind.AttributeTargetSpecifier))
            {
                return true;
            }

            // cases:
            //   [Namespace.|
            if (token.Parent.IsKind(SyntaxKind.QualifiedName) &&
                token.Parent.IsParentKind(SyntaxKind.Attribute))
            {
                return true;
            }

            // cases:
            //   [global::|
            if (token.Parent.IsKind(SyntaxKind.AliasQualifiedName) &&
                token.Parent.IsParentKind(SyntaxKind.Attribute))
            {
                return true;
            }

            return false;
        }
    }
}