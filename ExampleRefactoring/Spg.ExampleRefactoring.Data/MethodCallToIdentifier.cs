using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.AST;

namespace Spg.ExampleRefactoring.Data
{
    /// <summary>
    /// Add parameter command
    /// </summary>
    public class MethodCallToIdentifier : ExampleCommand
    {
        /// <summary>
        /// Return the train data set.
        /// </summary>
        /// <returns>List of examples</returns>
        public override List<Tuple<String, String>> Train()
        {
            List<Tuple<String, String>> tuples = new List<Tuple<string, string>>();

            String input01 =
@"private TypeDeclarationSyntax ParseClassOrStructOrInterfaceDeclaration1(SyntaxListBuilder<AttributeListSyntax> attributes, SyntaxListBuilder modifiers)
          {
              switch (classOrStructOrInterface.Kind)
              {
                  case SyntaxKind.ClassKeyword:
                      return syntaxFactory.ClassDeclaration(
                          attributes,
                          modifiers.ToTokenList(),
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

                  default:
                      throw ExceptionUtilities.UnexpectedValue(classOrStructOrInterface.Kind);
              }
          }
";

            String output01 =
@"private TypeDeclarationSyntax ParseClassOrStructOrInterfaceDeclaration1(SyntaxListBuilder<AttributeListSyntax> attributes, SyntaxListBuilder modifiers)
          {
              switch (classOrStructOrInterface.Kind)
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

                  default:
                      throw ExceptionUtilities.UnexpectedValue(classOrStructOrInterface.Kind);
              }
          }
";

            Tuple<String, String> tuple01 = Tuple.Create(input01, output01);
            Console.WriteLine(input01);
            Console.WriteLine(output01);
            tuples.Add(tuple01);

            String input02 =
@"private TypeDeclarationSyntax ParseClassOrStructOrInterfaceDeclaration2(SyntaxListBuilder<AttributeListSyntax> attributes, SyntaxListBuilder modifiers)
          {
              switch (classOrStructOrInterface.Kind)
              {
                  case SyntaxKind.StructKeyword:
                      return syntaxFactory.StructDeclaration(
                          attributes,
                          modifiers.ToTokenList(),
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

                  default:
                      throw ExceptionUtilities.UnexpectedValue(classOrStructOrInterface.Kind);
              }
          }
";


            String output02 =
@"private TypeDeclarationSyntax ParseClassOrStructOrInterfaceDeclaration2(SyntaxListBuilder<AttributeListSyntax> attributes, SyntaxListBuilder modifiers)
          {
              switch (classOrStructOrInterface.Kind)
              {
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

                  default:
                      throw ExceptionUtilities.UnexpectedValue(classOrStructOrInterface.Kind);
              }
          }
";
            Tuple<String, String> tuple02 = Tuple.Create(input02, output02);
            Console.WriteLine(input02);
            Console.WriteLine(output02);
            tuples.Add(tuple02);
            return tuples;
        }

        /// <summary>
        /// Return the test data.
        /// </summary>
        /// <returns>Return a string to be tested.</returns>
        public override Tuple<String, String> Test()
        {
            String input01 =
 @"private TypeDeclarationSyntax ParseClassOrStructOrInterfaceDeclaration3(SyntaxListBuilder<AttributeListSyntax> attributes, SyntaxListBuilder modifiers)
          {
              switch (classOrStructOrInterface.Kind)
              {
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
                      throw ExceptionUtilities.UnexpectedValue(classOrStructOrInterface.Kind);
              }
          }
";

            String output01 =
@"private TypeDeclarationSyntax ParseClassOrStructOrInterfaceDeclaration3(SyntaxListBuilder<AttributeListSyntax> attributes, SyntaxListBuilder modifiers)
          {
              switch (classOrStructOrInterface.Kind)
              {
                  case SyntaxKind.InterfaceKeyword:
                      return syntaxFactory.InterfaceDeclaration(
                          attributes,
                          tokenList,
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
                      throw ExceptionUtilities.UnexpectedValue(classOrStructOrInterface.Kind);
              }
          }
";
            Tuple<String, String> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}
