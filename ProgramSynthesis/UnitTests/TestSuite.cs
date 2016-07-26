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
            string inputText = File.ReadAllText(path +  @"benchmarks\SyntaxTreeExtensionsB.cs");
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
