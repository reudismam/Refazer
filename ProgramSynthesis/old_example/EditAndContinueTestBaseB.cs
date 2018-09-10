// Copyright (c) Microsoft Open Technologies, Inc.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using Microsoft.CodeAnalysis.CodeGen;
using Microsoft.CodeAnalysis.CSharp.Emit;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp.Test.Utilities;
using Microsoft.CodeAnalysis.CSharp.UnitTests;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Test.Utilities;
using Microsoft.VisualStudio.SymReaderInterop;
using Roslyn.Test.MetadataUtilities;
using Roslyn.Test.PdbUtilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.CSharp.EditAndContinue.UnitTests
{
    public abstract class EditAndContinueTestBase : EmitMetadataTestBase
    {
        internal static Func<SyntaxNode, SyntaxNode> GetSyntaxMapByKind(MethodSymbol method0, params SyntaxKind[] kinds)
        {
            return newNode =>
            {
                foreach (SyntaxKind kind in kinds) 
                {
                    if (newNode.CSharpKind() == kind)
                    {
                        return method0.DeclaringSyntaxReferences.Single().SyntaxTree.GetRoot().DescendantNodes().Single(n => n.CSharpKind() == kind);
                    }
                }

                return null;
            };
        }
}
