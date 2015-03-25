// Copyright (c) Microsoft Open Technologies, Inc.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Analyzers.MetaAnalyzers.CodeFixes;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Simplification;

namespace Microsoft.CodeAnalysis.CSharp.Analyzers.MetaAnalyzers.CodeFixes
{
        /// <summary>
    /// CA1001: Types that own disposable fields should be disposable
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CSharpApplyDiagnosticAnalyzerAttributeFix)), Shared]

    public sealed class CSharpApplyDiagnosticAnalyzerAttributeFix : ApplyDiagnosticAnalyzerAttributeFix
    {
        protected override SyntaxNode ParseExpression(string expression)
        {
            return SyntaxFactory.ParseExpression(expression).WithAdditionalAnnotations(Simplifier.Annotation);
        }
    }
}
