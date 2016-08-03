using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Specifications;
using Microsoft.ProgramSynthesis.Utils;
using NUnit.Framework;
using ProseSample;
//using static ProseSample.Utils;
using ProseSample.Substrings;
using TreeElement.Spg.Node;

namespace UnitTests
{
    [TestFixture]
    public class TestSuite
    {
        [Test]
        public void R2_8c14644()
        {

            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();
            //input data
            string inputText = File.ReadAllText(path + @"benchmarks\SyntaxTreeExtensionsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\SyntaxTreeExtensionsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Getting examples methods
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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

            //Run program
            var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration);

            foreach (var method in methods)
            {
                var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
                object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
                Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            }
        }

        [Test]
        public void R2_673f18e()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks\CommonCommandLineParserTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\CommonCommandLineParserTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(4, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(4, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks\CommonCommandLineParserTestsB_1.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\CommonCommandLineParserTestsA_1.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(4, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(4, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks\CommonCommandLineParserTestsB_2.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\CommonCommandLineParserTestsA_2.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(4, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(4, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks\TokenBasedFormattingRuleB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\TokenBasedFormattingRuleA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks\ObjectDisplayB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\ObjectDisplayA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks\ObjectDisplayB_1.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\ObjectDisplayA_1.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks\MetadataWriterB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\MetadataWriterA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks\CSharpExtensionsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\CSharpExtensionsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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

            //Run program
            var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            foreach (var method in methods)
            {
                var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
                object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
                Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            }
        }

        [Test]
        public void R00552fc()
        {
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks\LanguageParserB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\LanguageParserA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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

            //Run program
            var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

            foreach (var method in methods)
            {
                var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
                object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
                Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
            }
        }


        [Test]
        public void R1113fd3()
        {
            //todo the technique view the examples in the examples as two distint transformations.
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks\MetadataWriterB_1.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\MetadataWriterA_1.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            //todo the technique view the examples in the examples as two distint transformations.
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks\CommandLineTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\CommandLineTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            //todo the technique view the examples in the examples as two distint transformations.
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks\InternalsVisibleToAndStrongNameTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\InternalsVisibleToAndStrongNameTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks\ParsingErrorRecoveryTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\ParsingErrorRecoveryTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks\DiagnosticFormatterB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\DiagnosticFormatterA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks\BaseClassTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\BaseClassTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks\UserDefinedImplicitConversionsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\UserDefinedImplicitConversionsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks\CSharpCompilationOptionsTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\CSharpCompilationOptionsTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            //todo better defines the cluster function. What is to be executed in a single operation. In here are executed in two.
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks\EmitExpressionB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\EmitExpressionA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks\InternalsVisibleToAndStrongNameTestsB_1.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\InternalsVisibleToAndStrongNameTestsA_1.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks\SourceDelegateMethodSymbolB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\SourceDelegateMethodSymbolA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks\SyntaxFactoryB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\SyntaxFactoryA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks\MetadataWriterB_2.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks\MetadataWriterA_2.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            //todo there is another parts of the transformation that you must decide if it will make part of the transformation
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks2\HistoryRepositoryB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2\HistoryRepositoryA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            //todo there is another parts of the transformation that you must decide if it will make part of the transformation
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks2\HistoryRepositoryTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2\HistoryRepositoryTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            //todo there is another parts of the transformation that you must decide if it will make part of the transformation
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks2\CommitFailureTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2\CommitFailureTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            //todo there are two things to evaluate here. The first is the k, the second is the need from a insertAfter operator.
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks2\StringTranslatorUtilB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2\StringTranslatorUtilA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks2\HistoryRepositoryTestsB_1.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2\HistoryRepositoryTestsA_1.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            //todo create more than one cluster when it would create only one.
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks2\CommitFailureTestsB_1.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2\CommitFailureTestsA_1.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            //todo create more than one cluster when it would create only one.
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks2\CommitFailureTestsB_2.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2\CommitFailureTestsA_2.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks2\AssociationSetMappingTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2\AssociationSetMappingTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks2\DatabaseExistsInInitializerTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2\DatabaseExistsInInitializerTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks2\SimpleScenariosForLocalDbB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2\SimpleScenariosForLocalDbA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks2\RelationshipManagerB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2\RelationshipManagerA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks2\QueryableExtensionsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2\QueryableExtensionsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            //todo create two cluster when must create only one.
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks2\CommitFailureHandlerTestsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks2\CommitFailureHandlerTestsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks3\PackageSourceProviderTestB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3\PackageSourceProviderTestA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            //todo two cluster are create. Only one is needed.
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks3\V3SourceRepositoryB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3\V3SourceRepositoryA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            //todo create to cluster. I think the error is that the element that is being updated are not present on the tree. Maybe because the operations are executed in the wrong order.
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks3\PackageRepositoryTestB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3\PackageRepositoryTestA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks3\PackageManagerControl.xamlB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3\PackageManagerControl.xamlA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks3\NuGetPowerShellBaseCommandB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3\NuGetPowerShellBaseCommandA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks3\PackageRepositoryExtensionsB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3\PackageRepositoryExtensionsA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks3\PackageManagerControl.xamlB_1.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3\PackageManagerControl.xamlA_1.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks3\LocalPackageRepositoryTestB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3\LocalPackageRepositoryTestA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            //todo must cleate only a cluster but create more than one.
            //Load grammar
            var grammar = GetGrammar();

            string path = GetBasePath();

            //input data
            string inputText = File.ReadAllText(path + @"benchmarks3\PackageWalkerTestB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3\PackageWalkerTestA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks3\ConsoleB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3\ConsoleA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks3\PackageSourceProviderTestB_1.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3\PackageSourceProviderTestA_1.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks3\AutoDetectSourceRepositoryB.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3\AutoDetectSourceRepositoryA.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks3\PackageWalkerTestB_1.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3\PackageWalkerTestA_1.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
            string inputText = File.ReadAllText(path + @"benchmarks3\PackageManagerControl.xamlB_2.cs");
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(path + @"benchmarks3\PackageManagerControl.xamlA_2.cs");
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //Examples
            var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);
            var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

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
