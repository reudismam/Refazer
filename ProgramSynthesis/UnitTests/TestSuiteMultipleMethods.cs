using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Specifications;
using NUnit.Framework;
using ProseSample;
using ProseSample.Substrings;
using TreeElement.Spg.Node;

namespace UnitTests
{
    [TestFixture]
    public class TestSuiteMultipleMethods
    {
        [Test]
        public void R2_8c14644()
        {

            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();
            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\SyntaxTreeExtensionsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\SyntaxTreeExtensionsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Getting examples methods
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration);

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void R2_673f18e()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\CommonCommandLineParserTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\CommonCommandLineParserTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(4, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(4, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(4, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void R2_673f18e_1()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\CommonCommandLineParserTestsB_1.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\CommonCommandLineParserTestsA_1.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(4, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(4, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(4, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void R2_673f18e_2()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksM\CommonCommandLineParserTestsB_2.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksM\CommonCommandLineParserTestsA_2.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(4, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(4, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(4, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void Re817dab()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\TokenBasedFormattingRuleB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\TokenBasedFormattingRuleA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void R4_c96d9ce()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\ObjectDisplayB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\ObjectDisplayA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void R5_c96d9ce()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\ObjectDisplayB_1.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\ObjectDisplayA_1.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }


        [Test]
        public void R2_cfd9b46()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\MetadataWriterB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\MetadataWriterA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void R3_8c14644()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\CSharpExtensionsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\CSharpExtensionsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void R00552fc()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\LanguageParserB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\LanguageParserA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }


        [Test]
        public void R1113fd3()
        {
            //todo: maybe you can see as two transformation
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\MetadataWriterB_1.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\MetadataWriterA_1.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void R4_673f18e()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\CommandLineTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\CommandLineTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void R4b40293()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\InternalsVisibleToAndStrongNameTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\InternalsVisibleToAndStrongNameTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void R5_673f18e()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\ParsingErrorRecoveryTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\ParsingErrorRecoveryTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void R673f18e()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\DiagnosticFormatterB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\DiagnosticFormatterA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void R6_673f18e()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\BaseClassTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\BaseClassTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void R83e4349()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\UserDefinedImplicitConversionsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\UserDefinedImplicitConversionsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }


        [Test]
        public void R8_673f18e()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\CSharpCompilationOptionsTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\CSharpCompilationOptionsTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void R8ecd058()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\EmitExpressionB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\EmitExpressionA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void R9_673f18e()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\InternalsVisibleToAndStrongNameTestsB_1.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\InternalsVisibleToAndStrongNameTestsA_1.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void Rb495c9a()
        {
            //todo this operations occurs at a constructor non a method
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\SourceDelegateMethodSymbolB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\SourceDelegateMethodSymbolA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void Rc96d9ce()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\SyntaxFactoryB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\SyntaxFactoryA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void Rcfd9b46()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\MetadataWriterB_2.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\MetadataWriterA_2.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void E14323da()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks2m\HistoryRepositoryB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2m\HistoryRepositoryA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void E2_14323da()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks2m\HistoryRepositoryTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2m\HistoryRepositoryTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void E2_326d525()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks2m\CommitFailureTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2m\CommitFailureTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void E2bae908()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks2m\StringTranslatorUtilB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2m\StringTranslatorUtilA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void E4_14623da()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks2m\HistoryRepositoryTestsB_1.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2m\HistoryRepositoryTestsA_1.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void E4_1571862()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks2m\CommitFailureTestsB_1.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2m\CommitFailureTestsA_1.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void E5_1571862()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks2m\CommitFailureTestsB_2.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2m\CommitFailureTestsA_2.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void E829dec5()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks2m\AssociationSetMappingTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2m\AssociationSetMappingTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void E8b9180b()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks2m\DatabaseExistsInInitializerTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2m\DatabaseExistsInInitializerTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void E8d45249()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks2m\SimpleScenariosForLocalDbB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2m\SimpleScenariosForLocalDbA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void Ebc42e49()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks2m\RelationshipManagerB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2m\RelationshipManagerA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void Ece1e333()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks2m\QueryableExtensionsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2m\QueryableExtensionsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void Ed83cdfa()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks2m\CommitFailureHandlerTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2m\CommitFailureHandlerTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void N2_8da9f0e()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks3m\PackageSourceProviderTestB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3m\PackageSourceProviderTestA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void N2_a569c55()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks3m\V3SourceRepositoryB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3m\V3SourceRepositoryA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void N2_a883600()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks3m\PackageRepositoryTestB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3m\PackageRepositoryTestA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void N2_ee953e8()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks3m\PackageManagerControl.xamlB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3m\PackageManagerControl.xamlA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void N2dea84e()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks3m\NuGetPowerShellBaseCommandB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3m\NuGetPowerShellBaseCommandA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void N3_a883600()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks3m\PackageRepositoryExtensionsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3m\PackageRepositoryExtensionsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void N4ff8771()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks3m\PackageManagerControl.xamlB_1.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3m\PackageManagerControl.xamlA_1.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void N5_8da9f0e()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks3m\LocalPackageRepositoryTestB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3m\LocalPackageRepositoryTestA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void N5_a883600()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks3m\PackageWalkerTestB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3m\PackageWalkerTestA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void N74d4d32()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks3m\ConsoleB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3m\ConsoleA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void N8da9f0e()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks3m\PackageSourceProviderTestB_1.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3m\PackageSourceProviderTestA_1.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void Na569c55()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks3m\AutoDetectSourceRepositoryB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3m\AutoDetectSourceRepositoryA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void Na883600()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks3m\PackageWalkerTestB_1.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3m\PackageWalkerTestA_1.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void Nee953e8()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks3m\PackageManagerControl.xamlB_2.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3m\PackageManagerControl.xamlA_2.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        //The tests below deal with tranformations that occurs at multiples methods only, for example on the parameterlist.

        [Test]
        public void R04d0604()
        {
            //todo work with property declaration non method. Error tudo to the different types of nodes: method and property.
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\CA1052CSharpCodeFixProviderB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\CA1052CSharpCodeFixProviderA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.PropertyDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }


        [Test]
        public void R2_7c885ca()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\CA1012CodeFixProviderB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\CA1012CodeFixProviderA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.ClassDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.ClassDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void R3_c96d9ce()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\ObjectDisplayB_2.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\ObjectDisplayA_2.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void Rcd68d03()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\ProjectB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\ProjectA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void Re28c812()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\ApplyDiagnosticAnalyzerAttributeFixB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\ApplyDiagnosticAnalyzerAttributeFixA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.UsingDirective).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.UsingDirective).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        [Test]
        public void Rf66696e()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarksm\CompilerServerApiTestB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarksm\CompilerServerApiTestA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examplesInput.Count; i++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec);

            ////Run program
            //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            //foreach (var method in methods)
            //{
            //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
            //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            //}
        }

        static string GetTestDataFolder(string testDataLocation)
        {
            string startupPath = System.AppDomain.CurrentDomain.BaseDirectory;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - 4));
            string result = projectPath + testDataLocation;
            return result;
        }

        private static List<SyntaxNodeOrToken> GetNodesByType(SyntaxNodeOrToken outTree, SyntaxKind kind)
        {
            //select nodes of type method declaration
            var exampleMethodsInput = from inode in outTree.AsNode().DescendantNodes()
                                      where inode.IsKind(kind)
                                      select inode;

            //Select two examples
            var examplesSotInput = exampleMethodsInput.Select(sot => (SyntaxNodeOrToken)sot).ToList();//.GetRange(0, 1);
            //var examplesInput = examplesSotInput.Select(o => (object)o).ToList();
            return examplesSotInput;
        }

        public static string GetBasePath()
        {
            string path = GetTestDataFolder(@"\");
            return path;
        }

        public static Grammar GetGrammar()
        {
            string path = GetBasePath();
            var grammar = Utils.LoadGrammar(path + @"grammar\Transformation.grammar");
            return grammar;
        }
    }
}
