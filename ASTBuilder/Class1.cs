using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASTBuilder
{
    public class Class1
    {
        public static void Main()
        {
            Main2();
            int a = 10;
            NameSyntax name = IdentifierName("System");
            name = QualifiedName(name, IdentifierName("Collections"));
            name = QualifiedName(name, IdentifierName("Generic"));

            SyntaxTree tree = CSharpSyntaxTree.ParseText(
@"using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, World!"");
        }
    }
}");

            var root = (CompilationUnitSyntax)tree.GetRoot();

            var oldUsing = root.Usings[1];
            var newUsing = oldUsing.WithName(name);

            root = root.ReplaceNode(oldUsing, newUsing);
        }


        public static void Main2()
        {
            var x = ParseStatement("NameSyntax name = Factory.IdentifierName(\"System\");");

            Console.WriteLine(x);

            //SingletonList<MemberDeclarationSyntax>(
            //FieldDeclaration(
            //    VariableDeclaration(IdentifierName("NameSyntax"),
            //        SingletonSeparatedList(
            //            VariableDeclarator(Identifier("name"))
            //            .WithInitializer(
            //                EqualsValueClause(
            //                    InvocationExpression(
            //                        MemberAccessExpression(
            //                            SyntaxKind.SimpleMemberAccessExpression,
            //                            IdentifierName("Factory"),
            //                            IdentifierName("IdentifierName")),
            //                        ArgumentList(
            //                            SingletonSeparatedList(
            //                                Argument(LiteralExpression(
            //                                        SyntaxKind.StringLiteralExpression,
            //                                        Literal(TriviaList(), @"""System""", @"""System""", TriviaList()))))))))))));                              
        }
    }
}

