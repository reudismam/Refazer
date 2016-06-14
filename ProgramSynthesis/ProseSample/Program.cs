using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Specifications;
using Microsoft.ProgramSynthesis.Utils;
using static ProseSample.Utils;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace ProseSample
{
    //Main class
    internal static class Program
    {
        private static void Main(string[] args)
        {
            LoadAndRunRepetitiveChangeMultipleEditions();
        }

        private static void LoadAndRunRepetitiveChangeMultipleEditions()
        {
            //Load grammar
            var grammar = LoadGrammar("ProseSample.Edit.Code.grammar");

            //input data
            string inputText = File.ReadAllText(@"SyntaxTreeExtensionsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(@"SyntaxTreeExtensionsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Getting examples methods
            var examplesInput = GetMethods(inpTree);
            var examplesOutput = GetMethods(outTree);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();

            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, examplesInput.ElementAt(i));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program

            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Learn(grammar, spec);

            //Run program
            object[] output = program.Invoke(ioExamples.First().Key).ToEnumerable().ToArray();
            WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        }


        private static List<object> GetMethods(SyntaxNodeOrToken outTree)
        {
            //select nodes of type method declaration
            var exampleMethodsInput = from inode in outTree.AsNode().DescendantNodes()
                                      where inode.IsKind(SyntaxKind.MethodDeclaration)
                                      select inode;

            //Select two examples
            var examplesSotInput = exampleMethodsInput.Select(sot => (SyntaxNodeOrToken)sot).ToList().GetRange(0, 1);
            var examplesInput = examplesSotInput.Select(o => (object)o).ToList();
            return examplesInput;
        }

        /*private static void LoadAndRunRepetitiveChangeMultipleEditions8()
        {
            //Load grammar
            var grammar = LoadGrammar("ProseSample.Edit.Code.grammar");

            //input data
            string inputText = File.ReadAllText(@"SyntaxTreeExtensionsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(@"SyntaxTreeExtensionsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesNodes = from inode in outTree.AsNode().DescendantNodes()
                                where inode.IsKind(SyntaxKind.MethodDeclaration)
                                select inode;
            //Select two examples
            var examplesSot = examplesNodes.Select(sot => (SyntaxNodeOrToken)sot).ToList().GetRange(0, 2);
            var examples = examplesSot.Select(o => (object)o).ToList();

            //Learn program
            var input = State.Create(grammar.InputSymbol, inpTree);
            var spec = new SubsequenceSpec(input, examples);
            ProgramNode program = Learn(grammar, spec);

            //Run program
            object[] output = program.Invoke(input).ToEnumerable().ToArray();
            WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        }

        private static void LoadAndRunRepetitiveChangeMultipleEditions4()
        {
            //Load grammar
            var grammar = LoadGrammar("ProseSample.Edit.Code.grammar");

            //input data
            string inputText = File.ReadAllText(@"CommonCommandLineParserTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(@"CommonCommandLineParserTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesNodes = from inode in outTree.AsNode().DescendantNodes()
                                where inode.IsKind(SyntaxKind.MethodDeclaration)
                                select inode;
            //Select two examples
            var example1 = (SyntaxNodeOrToken) examplesNodes.ElementAt(4);
            //var example2 = (SyntaxNodeOrToken) examplesNodes.ElementAt(14);
            var examplesSot = new List<SyntaxNodeOrToken> {example1};
            var examples = examplesSot.Select(o => (object)o).ToList();

            //Learn program
            var input = State.Create(grammar.InputSymbol, inpTree);
            var spec = new SubsequenceSpec(input, examples);
            ProgramNode program = Learn(grammar, spec);

            //Run program
            object[] output = program.Invoke(input).ToEnumerable().ToArray();
            WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        }

        private static void LoadAndRunRepetitiveChangeMultipleEditions3()
        {
            //Load grammar
            var grammar = LoadGrammar("ProseSample.Edit.Code.grammar");

            //input data
            string inputText = File.ReadAllText(@"SyntaxTreeExtensionsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(@"SyntaxTreeExtensionsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesNodes = from inode in outTree.AsNode().DescendantNodes()
                                where inode.IsKind(SyntaxKind.MethodDeclaration)
                                select inode;
            //Select two examples
            var examplesSot = examplesNodes.Select(sot => (SyntaxNodeOrToken)sot).ToList().GetRange(0, 2);
            var examples = examplesSot.Select(o => (object)o).ToList();

            //Learn program
            var input = State.Create(grammar.InputSymbol, inpTree);
            var spec = new SubsequenceSpec(input, examples);
            ProgramNode program = Learn(grammar, spec);

            //Run program
            object[] output = program.Invoke(input).ToEnumerable().ToArray();
            WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        }

        /*private static void LoadAndRunRepetitiveChangeMultipleEditions6()
        {
            //Load grammar
            var grammar = LoadGrammar("ProseSample.Edit.Code.grammar");

            //input data
            string inputText = File.ReadAllText(@"TokenBasedFormattingRuleB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(@"TokenBasedFormattingRuleA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesNodes = from inode in outTree.AsNode().DescendantNodes()
                                where inode.IsKind(SyntaxKind.MethodDeclaration)
                                select inode;
            //Select two examples
            var examplesSot = examplesNodes.Select(sot => (SyntaxNodeOrToken)sot).ToList().GetRange(0, 1);
            var examples = examplesSot.Select(o => (object)o).ToList();

            //Learn program
            var input = State.Create(grammar.InputSymbol, inpTree);
            var spec = new SubsequenceSpec(input, examples);
            ProgramNode program = Learn(grammar, spec);

            //Run program
            object[] output = program.Invoke(input).ToEnumerable().ToArray();
            WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        }

        private static void LoadAndRunRepetitiveChangeMultipleEditions7()
        {
            //Load grammar
            var grammar = LoadGrammar("ProseSample.Edit.Code.grammar");

            //input data
            string inputText = File.ReadAllText(@"ObjectDisplayB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(@"ObjectDisplayA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesNodes = from inode in outTree.AsNode().DescendantNodes()
                                where inode.IsKind(SyntaxKind.MethodDeclaration)
                                select inode;
            //Select two examples
            var examplesSot = examplesNodes.Select(sot => (SyntaxNodeOrToken)sot).ToList().GetRange(0, 1);
            var examples = examplesSot.Select(o => (object)o).ToList();

            //Learn program
            var input = State.Create(grammar.InputSymbol, inpTree);
            var spec = new SubsequenceSpec(input, examples);
            ProgramNode program = Learn(grammar, spec);

            //Run program
            object[] output = program.Invoke(input).ToEnumerable().ToArray();
            WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        }

        //public String bar(int i)
        //{
        //    if (M(receiver).CSharpKind() == kind)
        //    {
        //    }
        //}

        //public String bar(int i)
        //{
        //    if (M(receiver).IsKind(kind))
        //    {
        //    }
        //}
        //public String foo(int i)
        //{
        //    if (receiver.IsKind(SyntaxKind.ParenthesizedExpression))
        //    {
        //    }
        //}

        //public String foo(int i)
        //{
        //    if (receiver.CSharpKind() == SyntaxKind.ParenthesizedExpression)
        //    {
        //    }
        //}

        private static void LoadAndRunRepetitiveChangeMultipleEditions2()
        {
            //Load grammar
            var grammar = LoadGrammar("ProseSample.Edit.Code.grammar");

            //input data
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(
            @"using System;

            public class Test
            {
                public String foo(int i)
                {
                     if(receiver.CSharpKind() == SyntaxKind.ParenthesizedExpression){
                     }

                     kind = SyntaxKind.ParenthesizedExpression;
                     if(M(receiver).CSharpKind() == kind){
                     }
                }

                public String bar(int i)
                {
                     kind = SyntaxKind.ParenthesizedExpression;
                     if(M(receiver).CSharpKind() == kind){
                     }
                }

                public String silly(int i)
                {
                     if(receiver.CSharpKind() == SyntaxKind.ParenthesizedExpression){
                     }
                }
            }").GetRoot();

            //output with some code fragments edited.
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(
             @"using System;

            public class Test
            {
                public String foo(int i)
                {
                     if(receiver.IsKind(SyntaxKind.ParenthesizedExpression)){
                     }

                     kind = SyntaxKind.ParenthesizedExpression;
                     if(M(receiver).IsKind(kind)){
                     }
                }

                public String bar(int i)
                {
                     kind = SyntaxKind.ParenthesizedExpression;
                     if(M(receiver).IsKind(kind)){
                     }
                }

                public String silly(int i)
                {
                     if(receiver.CSharpKind() == SyntaxKind.ParenthesizedExpression){
                     }
                }
            }").GetRoot();


            //Examples
            var examplesNodes = from inode in outTree.AsNode().DescendantNodes()
                                where inode.IsKind(SyntaxKind.MethodDeclaration)
                                select inode;
            //Select two examples
            var examplesSot = examplesNodes.Select(sot => (SyntaxNodeOrToken)sot).ToList().GetRange(0, 2);
            var examples = examplesSot.Select(o => (object)o).ToList();

            //Learn program
            var input = State.Create(grammar.InputSymbol, inpTree);
            var spec = new SubsequenceSpec(input, examples);
            ProgramNode program = Learn(grammar, spec);

            //Run program
            object[] output = program.Invoke(input).ToEnumerable().ToArray();
            WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        }

        private static void LoadAndRunRepetitiveChangeMultipleEditions()
        {
            //Load grammar
            var grammar = LoadGrammar("ProseSample.Edit.Code.grammar");

            //input data
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(
            @"using System;

            public class Test
            {
                public String foo(int i)
                {
                     if(i == 0) return ""Foo!"";
                }

                public String bar(int i)
                {
                     if(i == 3) return ""Foo!"";
                }

                public String silly(int i)
                {
                     if(i == 4) return ""Foo!"";
                }
            }").GetRoot();

            //output with some code fragments edited.
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(
            @"using System;

            public class Test
            {
                public String foo(int i)
                {
                     if(i == 0){
                        return ""Bar!"";
                     }
                     else if(i == -1) return ""Foo!"";
                }

                public String bar(int i)
                {
                     if(i == 3){
                        return ""Bar!"";
                     }
                     else if(i == -1) return ""Foo!"";
                }

                public String silly(int i)
                {
                     if(i == 4) return ""Foo!"";
                }
            }").GetRoot();


            //Examples
            var examplesNodes = from inode in outTree.AsNode().DescendantNodes()
                                where inode.IsKind(SyntaxKind.MethodDeclaration)
                                select inode;
            //Select two examples
            var examplesSot = examplesNodes.Select(sot => (SyntaxNodeOrToken)sot).ToList().GetRange(0, 2);
            var examples = examplesSot.Select(o => (object)o).ToList();

            //Learn program
            var input = State.Create(grammar.InputSymbol, inpTree);
            var spec = new PrefixSpec(input, examples);
            ProgramNode program = Learn(grammar, spec);

            //Run program
            object[] output = program.Invoke(input).ToEnumerable().ToArray();
            WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        }

        private static void LoadAndRunRepetitiveChange()
        {
            //Load grammar
            var grammar = LoadGrammar("ProseSample.Edit.Code.grammar");

            //input data
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(
            @"using System;

            public class Test
            {
                public String foo(int i)
                {
                     if(i == 0) return ""Foo!"";
                }

                public String bar(int i)
                {
                     if(i == 3) return ""Foo!"";
                }

                public String silly(int i)
                {
                     if(i == 4) return ""Foo!"";
                }
            }").GetRoot();

            //output with some code fragments edited.
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(
            @"using System;

            public class Test
            {
                public String foo(int i)
                {
                     if(i == 2){}
                     if(i == 0) return ""Foo!"";
                }

                public String bar(int i)
                {
                     if(i == 2){}
                     if(i == 3) return ""Foo!"";
                }

                public String silly(int i)
                {
                     if(i == 4) return ""Foo!"";
                }
            }").GetRoot();


            //Examples
            var examplesNodes = from inode in outTree.AsNode().DescendantNodes()
                                where inode.IsKind(SyntaxKind.MethodDeclaration)
                                select inode;
            //Select two examples
            var examplesSot = examplesNodes.Select(sot => (SyntaxNodeOrToken)sot).ToList().GetRange(0, 2);
            var examples = examplesSot.Select(o => (object)o).ToList();

            //Learn program
            var input = State.Create(grammar.InputSymbol, inpTree);
            var spec = new PrefixSpec(input, examples);
            ProgramNode program = Learn(grammar, spec);

            //Run program
            SyntaxNodeOrToken[] output = program.Invoke(input).ToEnumerable().Select(s => (SyntaxNodeOrToken)s).ToArray();
            WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        }*/
    }
}
