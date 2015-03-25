using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleProject.Company.ExampleProject.MethodCallToIdentifier
{
    internal class LanguageParser
    {
        private SyntaxNode classOrStructOrInterface;
        private SyntaxFactory syntaxFactory;
        private object name;
        private object typeParameters;
        private List<object> parameterList;
        private object baseList;
        private object constraints;
        private object openBrace;
        private object members;
        private object closeBrace;
        private object semicolon;
        private object tokenList;

        private TypeDeclarationSyntax ParseClassOrStructOrInterfaceDeclaration(SyntaxListBuilder<AttributeListSyntax> attributes, SyntaxListBuilder modifiers)
        {
            switch (classOrStructOrInterface.CSharpKind())
            {
                case SyntaxKind.ClassKeyword:
                    return syntaxFactory.ClassDeclaration(
                        attributes,
                        tokenList,
                        classOrStructOrInterface,
                        name,
                        typeParameters,
                        parameterList,
                        baseList,
                        constraints,
                        openBrace,
                        members,
                        closeBrace,
                        semicolon);

                case SyntaxKind.StructKeyword:
                    return syntaxFactory.StructDeclaration(
                        attributes,
                        tokenList,
                        classOrStructOrInterface,
                        name,
                        typeParameters,
                        parameterList,
                        baseList,
                        constraints,
                        openBrace,
                        members,
                        closeBrace,
                        semicolon);

                case SyntaxKind.InterfaceKeyword:
                    return syntaxFactory.InterfaceDeclaration(
                        attributes,
                        modifiers.ToTokenList(),
                        classOrStructOrInterface,
                        name,
                        typeParameters,
                        baseList,
                        constraints,
                        openBrace,
                        members,
                        closeBrace,
                        semicolon);

                default:
                    throw new Exception("Exception");
            }
        }
    }
}
