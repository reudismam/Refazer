using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Extraction.Text.Semantics;
using Microsoft.ProgramSynthesis.Specifications;
using Microsoft.ProgramSynthesis.Specifications.Extensions;
using Microsoft.ProgramSynthesis.Utils;
using static ProseSample.Utils;
using ProseSample.Substrings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace ProseSample
{
    internal static class Program
    {
        //TODO refactor this class
        private static void Main(string[] args)
        {
            //LoadAndTestSubstrings1();
            //LoadAndTestSubstrings2();
            //LoadAndTestSubstrings3();
            //LoadAndTestSubstrings4();
            //LoadAndTestSubstrings5();
            //LoadAndTestSubstrings6();
            LoadAndTestSubstrings7();

            //TreeEditMain main = new TreeEditMain();
            //main.main();
        }

        //static bool ma()
        //{
        //    bool a = false;
        //    if (a)
        //    {
        //        return a;
        //    }
        //    return false;
        //}

        //static bool mb()
        //{
        //    bool a = false;
        //    Validate(a);
        //    if (a)
        //    {
        //        return a;
        //    }
        //    return false;
        //}

        //private static void Validate(bool a)
        //{
        //    throw new NotImplementedException();
        //}


        private static void LoadAndTestSubstrings7()
        {
            var grammar = LoadGrammar("ProseSample.Edit.Code.grammar");
            if (grammar == null) return;

            ProgramNode p = grammar.ParseAST(@"Insert(n, 1, C1(n, ""Block"", 
                                                               C1(n, ""IfStatement"",
                                                                    C1(n, ""EqualsExpression"",
                                                                        Literal(n, Identifier(""i""))))),

                                                                Node1(""IfStatement"",
                                                                     Node2(""EqualsExpression"",
                                                                            Const(Identifier(""i"")),
                                                                            Const(NumericLiteralExpression(""0"")) ) ) )",
                                             ASTSerializationFormat.HumanReadable);

            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(
            @"using System;

            public class Test
            {
                public String foo(int i)
                {
                     if(i == 0) return ""Foo!"";
                }
            }").GetRoot();

            //SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(
            //@"using System;

            //public class Test
            //{
            //    public String foo(int i)
            //    {
            //         if(i == 2){}
            //         if(i == 0) return ""Foo!"";
            //    }
            //}").GetRoot();


            var examplesNodes = from inode in inpTree.AsNode().DescendantNodes()
                where inode.IsKind(SyntaxKind.MethodDeclaration)
                           select inode;

            var examplesSot = examplesNodes.Select(sot => (SyntaxNodeOrToken) sot).ToList();

            var examples = examplesSot.Select(o => (object) o).ToList();

            var input = State.Create(grammar.InputSymbol, inpTree);
            var spec = new PrefixSpec(input, examples);
            ProgramNode program = Learn(grammar, spec);


            //string[] output = program.Invoke(input).ToEnumerable().Select(s => ((StringRegion)s).Value).ToArray();
            //WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));


            //Spec spec = ShouldConvert.Given(grammar).To(inpTree, outTree);
            //var program = Learn(grammar, spec);

            //State input = State.Create(grammar.InputSymbol, inpTree);
            //var result = (SyntaxNodeOrToken)program.Invoke(input);
            //string r = result.ToFullString();
            //Console.WriteLine(r);
        }

        private static void LoadAndTestSubstrings6()
        {
            var grammar = LoadGrammar("ProseSample.Edit.Code.grammar");
            if (grammar == null) return;

            ProgramNode p = grammar.ParseAST(@"Insert(n, 1, C1(n, ""Block"", 
                                                               C1(n, ""IfStatement"",
                                                                    C1(n, ""EqualsExpression"",
                                                                        Literal(n, Identifier(""i""))))),

                                                                Node1(""IfStatement"",
                                                                     Node2(""EqualsExpression"",
                                                                            Const(Identifier(""i"")),
                                                                            Const(NumericLiteralExpression(""0"")) ) ) )",
                                             ASTSerializationFormat.HumanReadable);

            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(
            @"using System;

            public class Test
            {
                public String foo(int i)
                {
                     if(i == 0) return ""Foo!"";
                }
            }").GetRoot();

            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(
            @"using System;

            public class Test
            {
                public String foo(int i)
                {
                     if(i == 2){}
                     if(i == 0) return ""Foo!"";
                }
            }").GetRoot();

            Spec spec = ShouldConvert.Given(grammar).To(inpTree, outTree);
            var program = Learn(grammar, spec);

            State input = State.Create(grammar.InputSymbol, inpTree);
            var result = (SyntaxNodeOrToken)program.Invoke(input);
            string r = result.ToFullString();
            Console.WriteLine(r);
        }

        private static void LoadAndTestSubstrings5()
        {
            var grammar = LoadGrammar("ProseSample.Edit.Code.grammar");
            if (grammar == null) return;
            ProgramNode p = grammar.ParseAST(@"Script1(n, ""Block"",
                                                    Insert(n, 1, C1(n, ""Block"", 
                                                               C1(n, ""IfStatement"",
                                                                    C1(n, ""EqualsExpression"",
                                                                        Literal(n, Identifier(""i""))))),

                                                                Node1(""IfStatement"",
                                                                     Node2(""EqualsExpression"",
                                                                            Const(Identifier(""i"")),
                                                                            Const(NumericLiteralExpression(""0"")) ) ) ) )",
                                            ASTSerializationFormat.HumanReadable);

            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(
            @"using System;

            public class Test
            {
                public String foo(int i)
                {
                     if(i == 0) return ""Foo!"";
                }
            }").GetRoot();

            var li = from nd in inpTree.AsNode().DescendantNodes()
                where nd.IsKind(SyntaxKind.Block)
                select nd;
            SyntaxNodeOrToken x = li.ToList().First();

                SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(
            @"using System;

            public class Test
            {
                public String foo(int i)
                {
                     if(i == 0){}
                     if(i == 0) return ""Foo!"";
                }
            }").GetRoot();

            var lo = from nd in outTree.AsNode().DescendantNodes()
                    where nd.IsKind(SyntaxKind.Block)
                    select nd;

            SyntaxNodeOrToken y = lo.ToList().First();

            Spec spec = ShouldConvert.Given(grammar).To(x, y);
            var program = Learn(grammar, spec);
        }

        private static void LoadAndTestSubstrings4()
        {
            var grammar = LoadGrammar("ProseSample.Edit.Code.grammar");
            if (grammar == null) return;

            ProgramNode p = grammar.ParseAST(@"Script1(n, ""Block"",
                                                    Insert(n, 1, C1(n, ""Block"", 
                                                               C1(n, ""IfStatement"",
                                                                    C2(n, ""EqualsExpression"",
                                                                        Literal(n, Identifier(""i"")),
                                                                        Literal(n, NumericLiteralExpression(""0"")))
                                                                    )),

                                                                Node1(""IfStatement"",
                                                                     Node2(""EqualsExpression"",
                                                                            Const(Identifier(""i"")),
                                                                            Const(NumericLiteralExpression(""0"")) ) ) ) )",
                                            ASTSerializationFormat.HumanReadable);

            //Script1(n, "Block", Insert(n, 1, C1(n, "Block", C2(n, "IfStatement", C2(n, "EqualsExpression", Literal(n, Identifier("i")), Literal(n, NumericLiteralExpression("0"))), C1(n, "ReturnStatement", Literal(n, StringLiteralExpression("\"Foo!\""))))), Node2("IfStatement", Node2("EqualsExpression", Const(Identifier("i")), Const(NumericLiteralExpression("2"))), Const(Block("{}")))))
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(
            @"using System;

            public class Test
            {
                public String foo(int i)
                {
                     if(i == 0) return ""Foo!"";
                }
            }").GetRoot();

            SyntaxNodeOrToken outTree = SyntaxFactory.ParseStatement("int a = 10;");

            Bindings bs = null;
            Tuple<SyntaxNodeOrToken, Bindings> t = Tuple.Create(outTree, bs);
            MatchResult m = new MatchResult(t);


            State input = State.Create(grammar.InputSymbol, inpTree);
            var result = (SyntaxNodeOrToken)p.Invoke(input);
            string r = result.ToFullString();
            Console.WriteLine(r);
        }

        private static void LoadAndTestSubstrings3()
        {
            var grammar = LoadGrammar("ProseSample.Edit.Code.grammar");
            if (grammar == null) return;

            ProgramNode p = grammar.ParseAST(@"InsertBefore(n, C1(n, ""IfStatement"",
                                                   Literal(n, Identifier(""a""))),
                                                      Node1(""ExpressionStatement"", 
                                                        Node2(""InvocationExpression"", 
                                                            Const(Identifier(""Validate"")),
                                                            Node1(""ArgumentList"", 
                                                                    Node1(""Argument"", 
                                                                        Const(Identifier(""a"")))))))",
                                             ASTSerializationFormat.HumanReadable);

            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(
            @"using System;

            public class Test
            {
                static bool ma()
                {
                     bool a = false;
                     if (a){
                        return a;
                     }
                    return false;
                }
            }").GetRoot();

            SyntaxNodeOrToken outTree = SyntaxFactory.ParseStatement("int a = 10;");

            Bindings bs = null;
            Tuple<SyntaxNodeOrToken, Bindings> t = Tuple.Create(outTree, bs);
            MatchResult m = new MatchResult(t);


            State input = State.Create(grammar.InputSymbol, inpTree);
            var result = (SyntaxNodeOrToken)p.Invoke(input);
            string r = result.ToFullString();
            Console.WriteLine(r);
        }

        private static void LoadAndTestSubstrings1()
        {
            var grammar = LoadGrammar("ProseSample.Code.grammar");
            if (grammar == null) return;

            ProgramNode p = grammar.ParseAST(@"C(n, ""LocalDeclarationStatement"", 
                                                    C(n, ""VariableDeclaration"",
                                                            Literal(n, PredefinedType(""int"")),
                                                            C(n, ""VariableDeclarator"", 
                                                                Literal(n, Identifier(""a"")),
                                                                C(n, ""EqualsValueClause"",
                                                                    Literal(n, NumericLiteralExpression(""10""))))))",
                                             ASTSerializationFormat.HumanReadable);

            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(
            @"using System;

            public class Test
            {
                public static void Main()
                {
                    int a = 10;
                }
            }").GetRoot();

            SyntaxNodeOrToken outTree = SyntaxFactory.ParseStatement("int a = 10;");

            Bindings bs = null;
            Tuple<SyntaxNodeOrToken, Bindings> t = Tuple.Create(outTree, bs);
            MatchResult m = new MatchResult(t);


            State input = State.Create(grammar.InputSymbol, inpTree);
            var result = (MatchResult)p.Invoke(input);
            //Console.WriteLine(result);

            Console.WriteLine(result);
        }

        private static void LoadAndTestSubstrings2()
        {
            var grammar = LoadGrammar("ProseSample.Code.grammar");
            if (grammar == null) return;

            ProgramNode p = grammar.ParseAST(@"Literal(n, Identifier(""a""))",
                                             ASTSerializationFormat.HumanReadable);

            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(
            @"using System;

            public class Test
            {
                public static void Main()
                {
                    int a = 10;
                }
            }").GetRoot();

            //SyntaxToken predType = SyntaxFactory.ParseToken("int");
            //SyntaxNodeOrToken outTree = SyntaxFactory.PredefinedType(predType);

            SyntaxNodeOrToken outTree = SyntaxFactory.ParseStatement("int a = 10;");

            Bindings bs = null;
            Tuple<SyntaxNodeOrToken, Bindings> t = Tuple.Create(outTree, bs);
            MatchResult m = new MatchResult(t);

            //SyntaxNodeOrToken outTree = SyntaxFactory.ParseExpression("10");


            State input = State.Create(grammar.InputSymbol, inpTree);
            var result = (MatchResult)p.Invoke(input);
            //Console.WriteLine(result);

            Spec spec = ShouldConvert.Given(grammar).To(inpTree, m);
            Learn(grammar, spec);        
        }

        //private static void TestFlashFillBenchmark(Grammar grammar, string benchmark, int exampleCount = 2)
        //{
        //    string[] lines = File.ReadAllLines($"benchmarks/{benchmark}.tsv");
        //    Tuple<string, string>[] data = lines.Select(l =>
        //    {
        //        var parts = l.Split(new[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
        //        return Tuple.Create(parts[0], parts[1]);
        //    }).ToArray();
        //    var examples = data.Take(exampleCount)
        //                       .ToDictionary(t => State.Create(grammar.InputSymbol, StringRegion.Create(t.Item1)),
        //                                     t => (object) StringRegion.Create(t.Item2));
        //    var spec = new ExampleSpec(examples);
        //    ProgramNode program = Learn(grammar, spec);
        //    foreach (Tuple<string, string> row in data.Skip(exampleCount))
        //    {
        //        State input = State.Create(grammar.InputSymbol, StringRegion.Create(row.Item1));
        //        var output = (StringRegion) program.Invoke(input);
        //        WriteColored(ConsoleColor.DarkCyan, $"{row.Item1} => {output.Value}");
        //    }
        //}

        //private static void LoadAndTestTextExtraction()
        //{
        //    var grammar = LoadGrammar("ProseSample.TextExtraction.grammar", "ProseSample.Substrings.grammar");
        //    if (grammar == null) return;

        //    TestExtractionBenchmark(grammar, "countries");
        //}

        private static void TestExtractionBenchmark(Grammar grammar, string benchmark)
        {
            StringRegion document;
            List<StringRegion> examples = LoadBenchmark($"benchmarks/{benchmark}.txt", out document);
            var input = State.Create(grammar.InputSymbol, document);
            var spec = new PrefixSpec(input, examples);
            ProgramNode program = Learn(grammar, spec);
            string[] output = program.Invoke(input).ToEnumerable().Select(s => ((StringRegion) s).Value).ToArray();
            WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        }
    }
}
