using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Specifications;
using Microsoft.ProgramSynthesis.Utils;
using NUnit.Framework;
using NUnitTests.Spg.NUnitTests.Util;
using ProseSample;
using ProseSample.Substrings;
using Spg.ExampleRefactoring.Util;
using Spg.LocationRefactor.Location;
using Spg.LocationRefactor.TextRegion;
using Spg.LocationRefactor.Transform;
using TreeElement.Spg.Node;

namespace UnitTests
{
    [TestFixture]
    public class TestSolution
    {
        //[Test]
        //public void R2_8c14644()
        //{

        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();
        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\SyntaxTreeExtensionsB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\SyntaxTreeExtensionsA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Getting examples methods
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration);

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void R2_673f18e()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\CommonCommandLineParserTestsB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\CommonCommandLineParserTestsA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(4, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(4, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(4, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void R2_673f18e_1()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\CommonCommandLineParserTestsB_1.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\CommonCommandLineParserTestsA_1.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(4, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(4, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(4, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void R2_673f18e_2()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksM\CommonCommandLineParserTestsB_2.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksM\CommonCommandLineParserTestsA_2.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(4, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(4, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(4, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void Re817dab()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\TokenBasedFormattingRuleB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\TokenBasedFormattingRuleA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void R4_c96d9ce()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\ObjectDisplayB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\ObjectDisplayA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void R5_c96d9ce()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\ObjectDisplayB_1.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\ObjectDisplayA_1.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}


        //[Test]
        //public void R2_cfd9b46()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\MetadataWriterB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\MetadataWriterA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void R3_8c14644()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\CSharpExtensionsB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\CSharpExtensionsA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        [Test]
        public void R00552fc()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\00552fc");
            Assert.IsTrue(isCorrect);
        }


        //[Test]
        //public void R1113fd3()
        //{
        //    //todo: maybe you can see as two transformation
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\MetadataWriterB_1.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\MetadataWriterA_1.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void R4_673f18e()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\CommandLineTestsB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\CommandLineTestsA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void R4b40293()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\InternalsVisibleToAndStrongNameTestsB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\InternalsVisibleToAndStrongNameTestsA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void R5_673f18e()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\ParsingErrorRecoveryTestsB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\ParsingErrorRecoveryTestsA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void R673f18e()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\DiagnosticFormatterB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\DiagnosticFormatterA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void R6_673f18e()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\BaseClassTestsB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\BaseClassTestsA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void R83e4349()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\UserDefinedImplicitConversionsB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\UserDefinedImplicitConversionsA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}


        //[Test]
        //public void R8_673f18e()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\CSharpCompilationOptionsTestsB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\CSharpCompilationOptionsTestsA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void R8ecd058()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\EmitExpressionB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\EmitExpressionA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void R9_673f18e()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\InternalsVisibleToAndStrongNameTestsB_1.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\InternalsVisibleToAndStrongNameTestsA_1.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void Rb495c9a()
        //{
        //    //todo this operations occurs at a constructor non a method
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\SourceDelegateMethodSymbolB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\SourceDelegateMethodSymbolA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void Rc96d9ce()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\SyntaxFactoryB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\SyntaxFactoryA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void Rcfd9b46()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\MetadataWriterB_2.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\MetadataWriterA_2.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void E14323da()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks2m\HistoryRepositoryB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks2m\HistoryRepositoryA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void E2_14323da()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks2m\HistoryRepositoryTestsB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks2m\HistoryRepositoryTestsA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void E2_326d525()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks2m\CommitFailureTestsB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks2m\CommitFailureTestsA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void E2bae908()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks2m\StringTranslatorUtilB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks2m\StringTranslatorUtilA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void E4_14623da()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks2m\HistoryRepositoryTestsB_1.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks2m\HistoryRepositoryTestsA_1.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void E4_1571862()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks2m\CommitFailureTestsB_1.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks2m\CommitFailureTestsA_1.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void E5_1571862()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks2m\CommitFailureTestsB_2.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks2m\CommitFailureTestsA_2.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void E829dec5()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks2m\AssociationSetMappingTestsB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks2m\AssociationSetMappingTestsA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void E8b9180b()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks2m\DatabaseExistsInInitializerTestsB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks2m\DatabaseExistsInInitializerTestsA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void E8d45249()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks2m\SimpleScenariosForLocalDbB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks2m\SimpleScenariosForLocalDbA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void Ebc42e49()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks2m\RelationshipManagerB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks2m\RelationshipManagerA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void Ece1e333()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks2m\QueryableExtensionsB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks2m\QueryableExtensionsA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void Ed83cdfa()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks2m\CommitFailureHandlerTestsB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks2m\CommitFailureHandlerTestsA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void N2_8da9f0e()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks3m\PackageSourceProviderTestB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks3m\PackageSourceProviderTestA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void N2_a569c55()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks3m\V3SourceRepositoryB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks3m\V3SourceRepositoryA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void N2_a883600()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks3m\PackageRepositoryTestB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks3m\PackageRepositoryTestA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void N2_ee953e8()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks3m\PackageManagerControl.xamlB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks3m\PackageManagerControl.xamlA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void N2dea84e()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks3m\NuGetPowerShellBaseCommandB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks3m\NuGetPowerShellBaseCommandA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void N3_a883600()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks3m\PackageRepositoryExtensionsB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks3m\PackageRepositoryExtensionsA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void N4ff8771()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks3m\PackageManagerControl.xamlB_1.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks3m\PackageManagerControl.xamlA_1.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void N5_8da9f0e()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks3m\LocalPackageRepositoryTestB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks3m\LocalPackageRepositoryTestA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void N5_a883600()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks3m\PackageWalkerTestB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks3m\PackageWalkerTestA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void N74d4d32()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks3m\ConsoleB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks3m\ConsoleA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void N8da9f0e()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks3m\PackageSourceProviderTestB_1.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks3m\PackageSourceProviderTestA_1.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void Na569c55()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks3m\AutoDetectSourceRepositoryB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks3m\AutoDetectSourceRepositoryA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void Na883600()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks3m\PackageWalkerTestB_1.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks3m\PackageWalkerTestA_1.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void Nee953e8()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarks3m\PackageManagerControl.xamlB_2.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarks3m\PackageManagerControl.xamlA_2.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        ////The tests below deal with tranformations that occurs at multiples methods only, for example on the parameterlist.

        //[Test]
        //public void R04d0604()
        //{
        //    //todo work with property declaration non method. Error tudo to the different types of nodes: method and property.
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\CA1052CSharpCodeFixProviderB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\CA1052CSharpCodeFixProviderA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.PropertyDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}


        //[Test]
        //public void R2_7c885ca()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\CA1012CodeFixProviderB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\CA1012CodeFixProviderA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.ClassDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.ClassDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void R3_c96d9ce()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\ObjectDisplayB_2.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\ObjectDisplayA_2.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void Rcd68d03()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\ProjectB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\ProjectA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void Re28c812()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\ApplyDiagnosticAnalyzerAttributeFixB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\ApplyDiagnosticAnalyzerAttributeFixA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.UsingDirective).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.UsingDirective).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

        //[Test]
        //public void Rf66696e()
        //{
        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string path = GetBasePath();

        //    //input data
        //    string inputText = File.ReadAllText(path + @"benchmarksm\CompilerServerApiTestB.cs");
        //    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

        //    //output with some code fragments edited.
        //    string outputText = File.ReadAllText(path + @"benchmarksm\CompilerServerApiTestA.cs");
        //    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

        //    //Examples
        //    var examplesInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);
        //    var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 2);

        //    //building example methods
        //    var ioExamples = new Dictionary<State, IEnumerable<object>>();
        //    for (int i = 0; i < examplesInput.Count; i++)
        //    {
        //        var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
        //        ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
        //    }

        //    //Learn program
        //    var spec = DisjunctiveExamplesSpec.From(ioExamples);
        //    ProgramNode program = Utils.Learn(grammar, spec);

        //    ////Run program
        //    //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

        //    //foreach (var method in methods)
        //    //{
        //    //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
        //    //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
        //    //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
        //    //}
        //}

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

        /// <summary>
        /// Complete test
        /// </summary>
        /// <param name="commit">Commit where the change occurs</param>
        /// <returns>True if pass test</returns>
        public static bool CompleteTestBase(string commit)
        {
            long millBefore = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            //Load grammar
            var grammar = GetGrammar();

            string expHome = Environment.GetEnvironmentVariable("EXP_HOME", EnvironmentVariableTarget.User);
            List<CodeTransformation> codeTransformations = JsonUtil<List<CodeTransformation>>.Read(expHome + @"cprose\" + commit + @"\metadata\transformed_locations.json");
            List<TRegion> regions = codeTransformations.Select(entry => entry.Trans).ToList();

            List<TRegion> metadataRegions = new List<TRegion>();
            metadataRegions.AddRange(regions.GetRange(0, 2));
            var globalTransformations = RegionManager.GetInstance().GroupTransformationsBySourcePath(codeTransformations);

            for (int i = 0; i < regions.Count; i++)
            {
                var dicionarySelection = RegionManager.GetInstance().GroupRegionBySourcePath(metadataRegions);
                foreach (KeyValuePair<string, List<TRegion>> entry in dicionarySelection)
                {
                    string sourceCode = FileUtil.ReadFile(entry.Key);             
                    Tuple<string, List<TRegion>> tu = Transform(sourceCode, globalTransformations[entry.Key.ToUpperInvariant()], metadataRegions);
                    string sourceCodeAfter = tu.Item1;

                    SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(sourceCode).GetRoot();
                    SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(sourceCodeAfter).GetRoot();

                    var allMethodsInput = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration);
                    var allMethodsOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration);
                    var inputMethods = new List<int>();
                    foreach (var region in metadataRegions)
                    {
                        for (int index = 0; index < allMethodsInput.Count; index++)
                        {
                            var method = allMethodsInput[index];
                            var tRegion = new TRegion
                            {
                                Start = method.SpanStart,
                                Length = method.Span.Length
                            };

                            if (region.IsInside(tRegion))
                            {
                                inputMethods.Add(index);
                            }
                        }
                    }

                    //Examples
                    var examplesInput = inputMethods.Distinct().Select(index => allMethodsInput[index]).ToList();
                    var examplesOutput = inputMethods.Distinct().Select(index => allMethodsOutput[index]).ToList();

                    //building example methods
                    var ioExamples = new Dictionary<State, IEnumerable<object>>();
                    for (int index = 0; index < examplesInput.Count; index++)
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

                //var examplesOutput = GetNodesByType(outTree, SyntaxKind.MethodDeclaration).GetRange(0, 1);

                ////building example methods
                //var ioExamples = new Dictionary<State, IEnumerable<object>>();
                //for (int i = 0; i < examplesInput.Count; i++)
                //{
                //    var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(i))));
                //    ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(i) });
                //}

                ////Learn program
                //var spec = DisjunctiveExamplesSpec.From(ioExamples);
                //ProgramNode program = Utils.Learn(grammar, spec);

                ////Run program
                //var methods = GetNodesByType(inpTree, SyntaxKind.MethodDeclaration).GetRange(0, 1); ;

                //foreach (var method in methods)
                //{
                //    var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
                //    object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
                //    Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));
                //}
                //to be removed

                //controller.DocumentsBeforeAndAfter = documents;
                //controller.EditedLocations = dicSelections;

                millBefore = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                //controller.Refact();
                //controller.Undo();
                CodeTransformation tregion = MatchesLocationsOnCommit(codeTransformations);
                if (tregion == null)
                {
                    break;
                }

                if (ContainsTRegion(metadataRegions, tregion))
                {
                    return false;
                }
                metadataRegions.Add(tregion.Trans);
            }

            long millAfer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long totalTime = (millAfer - millBefore);
            //List<CodeTransformation> transformationsList = controller.CodeTransformations;//JsonUtil<List<CodeTransformation>>.Read(@"transformed_locations.json");

            long timeToLearnEdit = long.Parse(FileUtil.ReadFile("edit_learn.t"));
            long timeToTransformEdit = long.Parse(FileUtil.ReadFile("edit_transform.t"));

            //Log(commit, totalTime, metadataRegions.Count, transformationsList.Count, globalTransformations.Count, controller.Program.ToString(), timeToLearnEdit, timeToTransformEdit);

            FileUtil.WriteToFile(expHome + @"commit\" + commit + @"\edit.t", totalTime.ToString());

            try
            {
                string transformations = FileUtil.ReadFile("transformed_locations.json");
                FileUtil.WriteToFile(expHome + @"commit\" + commit + @"\" + "transformed_locations.json", transformations);
                FileUtil.DeleteFile("transformed_locations.json");
            }
            catch (Exception e)
            {
                Console.WriteLine("Cold not log transformed_locations.json");
            }
            return true;
        }

        ///// <summary>
        ///// Complete test
        ///// </summary>
        ///// <param name="commit">Commit where the change occurs</param>
        ///// <returns>True if pass test</returns>
        //public static bool CompleteTestBase(string commit)
        //{
        //    long millBefore = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        //    //EditorController controller = EditorController.GetInstance();

        //    string expHome = Environment.GetEnvironmentVariable("EXP_HOME", EnvironmentVariableTarget.User);

        //    //remove
        //    //List<TRegion> selections = JsonUtil<List<TRegion>>.Read(expHome + @"cprose\" + commit + @"\metadata\locations_on_commit.json");
        //    //List<CodeLocation> locations = TestUtil.GetAllLocationsOnCommit(selections, controller.Locations);
        //    //controller.Locations = locations;
        //    //remove

        //    List<TRegion> regions = new List<TRegion>();
        //    List<CodeTransformation> codeTransformations = JsonUtil<List<CodeTransformation>>.Read(expHome + @"cprose\" + commit + @"\metadata\transformed_locations.json");
        //    foreach (var entry in codeTransformations)
        //    {
        //        regions.Add(entry.Trans);
        //    }

        //    List<TRegion> metadataRegions = new List<TRegion>();
        //    metadataRegions.AddRange(regions.GetRange(0, 2));

        //    var globalTransformations = RegionManager.GetInstance().GroupTransformationsBySourcePath(codeTransformations);
        //    for (int i = 0; i < regions.Count; i++)
        //    {
        //        var dicionarySelection = RegionManager.GetInstance().GroupRegionBySourcePath(metadataRegions);

        //        List<Tuple<string, string>> documents = new List<Tuple<string, string>>();
        //        //Dictionary<string, List<Selection>> dicSelections = new Dictionary<string, List<Selection>>();
        //        foreach (KeyValuePair<string, List<TRegion>> entry in dicionarySelection)
        //        {
        //            string sourceCode = FileUtil.ReadFile(entry.Key);
        //            Tuple<string, List<TRegion>> tu = Transform(sourceCode, globalTransformations[entry.Key.ToUpperInvariant()], metadataRegions);
        //            string sourceCodeAfter = tu.Item1;
        //            //List<Selection> selectionsList = new List<Selection>();
        //            //foreach (TRegion region in tu.Item2)
        //            //{
        //            //    Selection selectionRegion = new Selection(region.Start, region.Length, region.Path, sourceCodeAfter, region.Text);
        //            //    selectionsList.Add(selectionRegion);
        //            //}

        //            //controller.CurrentViewCodeAfter = sourceCodeAfter;

        //            Tuple<string, string> tuple = Tuple.Create(sourceCode, sourceCodeAfter);
        //            documents.Add(tuple);
        //            //controller.FilesOpened[entry.Key.ToUpperInvariant()] = true;
        //            //dicSelections.Add(entry.Key.ToUpperInvariant(), selectionsList);
        //        }

        //        //controller.DocumentsBeforeAndAfter = documents;
        //        //controller.EditedLocations = dicSelections;

        //        millBefore = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        //        //controller.Refact();
        //        //controller.Undo();
        //        CodeTransformation tregion = MatchesLocationsOnCommit(codeTransformations);
        //        if (tregion == null)
        //        {
        //            break;
        //        }

        //        if (ContainsTRegion(metadataRegions, tregion))
        //        {
        //            return false;
        //        }
        //        metadataRegions.Add(tregion.Trans);
        //    }

        //    long millAfer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        //    long totalTime = (millAfer - millBefore);
        //    //List<CodeTransformation> transformationsList = controller.CodeTransformations;//JsonUtil<List<CodeTransformation>>.Read(@"transformed_locations.json");

        //    long timeToLearnEdit = long.Parse(FileUtil.ReadFile("edit_learn.t"));
        //    long timeToTransformEdit = long.Parse(FileUtil.ReadFile("edit_transform.t"));

        //    //Log(commit, totalTime, metadataRegions.Count, transformationsList.Count, globalTransformations.Count, controller.Program.ToString(), timeToLearnEdit, timeToTransformEdit);

        //    FileUtil.WriteToFile(expHome + @"commit\" + commit + @"\edit.t", totalTime.ToString());

        //    try
        //    {
        //        string transformations = FileUtil.ReadFile("transformed_locations.json");
        //        FileUtil.WriteToFile(expHome + @"commit\" + commit + @"\" + "transformed_locations.json", transformations);
        //        FileUtil.DeleteFile("transformed_locations.json");
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Cold not log transformed_locations.json");
        //    }
        //    return true;
        //}

        //public static void Log(string commit, double time, int exTransformations, int acTrasnformation, int documents, string program, double timeToLearnEdit, double timeToTransformEdit)
        //{
        //    string commitFirstLetter = commit.ElementAt(0).ToString();
        //    string commitId = commit.Substring(commit.IndexOf(@"\") + 1);

        //    commit = commitFirstLetter + "-" + commitId;

        //    string path = TestUtil.LOG_PATH;
        //    using (ExcelManager em = new ExcelManager())
        //    {

        //        em.Open(path);

        //        int empty = -1;
        //        for (int i = 1; i < 1000; i++)
        //        {
        //            string comt = em.GetValue("A" + i, Category.Formatted).ToString();
        //            if (comt.Equals(commit))
        //            {
        //                empty = i;
        //                break;
        //            }
        //        }

        //        if (empty != -1)
        //        {
        //            em.SetValue("K" + empty, time / 1000);
        //            em.SetValue("L" + empty, exTransformations);
        //            em.SetValue("M" + empty, acTrasnformation);
        //            em.SetValue("N" + empty, documents);
        //            em.SetValue("O" + empty, program);
        //            em.SetValue("P" + empty, timeToLearnEdit / 1000);
        //            em.SetValue("Q" + empty, timeToTransformEdit / 1000);
        //            em.Save();
        //        }
        //        else {
        //            Console.WriteLine("Could not write log to " + path);
        //        }
        //    }
        //}

        public static Tuple<string, List<TRegion>> Transform(string source, List<CodeTransformation> transformations, List<TRegion> regions)
        {
            List<TRegion> tRegions = new List<TRegion>();
            int nextStart = 0;
            string sourceCode = source;
            foreach (CodeTransformation item in transformations)
            {
                Tuple<TRegion, TRegion> tregion = GetTRegionShift(regions, item);
                TRegion region = tregion.Item1;
                string transformation = tregion.Item2.Text;

                int start = nextStart + region.Start;
                int end = start + region.Length;
                sourceCode = sourceCode.Substring(0, start) + transformation +
                sourceCode.Substring(end);

                TRegion tr = new TRegion();
                tr.Start = start - 1;
                tr.Length = tregion.Item2.Length + 2;
                tr.Text = tregion.Item2.Text;
                tr.Path = tregion.Item1.Path;
                tRegions.Add(tr);

                nextStart += transformation.Length - region.Length;
            }
            Tuple<string, List<TRegion>> t = Tuple.Create(sourceCode, tRegions);
            return t;
            //return sourceCode;
        }

        public static List<TRegion> ListTransform(List<CodeTransformation> transformations, List<TRegion> regions)
        {
            List<TRegion> regionList = new List<TRegion>();
            foreach (CodeTransformation item in transformations)
            {
                TRegion region = GetTRegion(regions, item);
                regionList.Add(region);
            }
            return regionList;
        }

        private static TRegion GetTRegion(List<TRegion> regions, CodeTransformation codeTransformation)
        {
            foreach (TRegion tr in regions)
            {
                if (codeTransformation.Trans.Equals(tr))
                {
                    return tr;
                }
            }
            return codeTransformation.Location.Region;
        }

        private static Tuple<TRegion, TRegion> GetTRegionShift(List<TRegion> regions, CodeTransformation codeTransformation)
        {
            Tuple<TRegion, TRegion> t;
            foreach (TRegion tr in regions)
            {
                if (codeTransformation.Trans.Equals(tr))
                {
                    TRegion region = new TRegion();
                    region.Start = tr.Start;
                    region.Length = tr.Length - 2;
                    region.Path = tr.Path;
                    region.Text = tr.Text;
                    t = Tuple.Create(codeTransformation.Location.Region, region);
                    return t;
                }
            }
            t = Tuple.Create(codeTransformation.Location.Region, codeTransformation.Location.Region);
            return t;
        }

        private static bool ContainsTRegion(List<TRegion> metadataRegions, CodeTransformation tregion)
        {
            foreach (var location in metadataRegions)
            {
                if (location.Start == tregion.Trans.Start &&
                    location.Length == tregion.Trans.Length &&
                    location.Path.ToUpperInvariant().Equals(tregion.Trans.Path.ToUpperInvariant()))
                {
                    return true;
                }
            }
            return false;
        }

        //private static CodeTransformation MatchesLocationsOnCommit(List<CodeTransformation> codeTransformations)
        //{
        //    EditorController controler = EditorController.GetInstance();
        //    List<CodeTransformation> transformations = controler.CodeTransformations;//JsonUtil<List<CodeTransformation>>.Read(@"transformed_locations.json");

        //    foreach (CodeTransformation metadata in codeTransformations)
        //    {
        //        bool isFound = false;
        //        foreach (CodeTransformation transformation in transformations)
        //        {
        //            if (metadata.Location.Region.Equals(transformation.Location.Region))
        //            {
        //                isFound = true;
        //                break;
        //            }
        //        }

        //        if (!isFound)
        //        {
        //            return metadata;
        //        }
        //    }
        //    return null;
        //}

        private static CodeTransformation MatchesLocationsOnCommit(List<CodeTransformation> codeTransformations)
        {
            //EditorController controler = EditorController.GetInstance();
            List<CodeTransformation> transformations = null;//controler.CodeTransformations;//JsonUtil<List<CodeTransformation>>.Read(@"transformed_locations.json");

            List<TRegion> codeTransformationsRegions = codeTransformations.Select(o => o.Trans).ToList();
            List<TRegion> transformationsRegions = transformations.Select(o => o.Trans).ToList();

            //var x = RegionManager.GetInstance().ElementsSelectionBeforeAndAfterEditing(controler.Locations);

            foreach (CodeTransformation metadata in codeTransformations)
            {
                bool isFound = false;
                CodeTransformation found = null;
                foreach (CodeTransformation transformation in transformations)
                {
                    //var expression = SyntaxFactory.ParseExpression(transformation.Trans.Text);
                    //var metadataExpresssion = SyntaxFactory.ParseExpression(metadata.Trans.Text);
                    //Tuple<SyntaxNode, SyntaxNode> te = Tuple.Create(expression.Parent, expression.Parent);
                    //Tuple<SyntaxNode, SyntaxNode> tme = Tuple.Create(metadataExpresssion.Parent, metadataExpresssion.Parent);
                    //Tuple<string, string> te = Tuple.Create(transformation.Trans.Text, transformation.Trans.Text);
                    //Tuple<string, string> tme = Tuple.Create(metadata.Trans.Text, metadata.Trans.Text);

                    //Tuple<ListNode, ListNode> lte = ASTProgram.Example(te);
                    //Tuple<ListNode, ListNode> ltme = ASTProgram.Example(tme); 

                    //int index = GetIndexLocation(controler.Locations, transformation.Location);
                    //Tuple<ListNode, ListNode> t = x[index];
                    List<TRegion> metadataRegion = new List<TRegion> { metadata.Trans };
                    List<TRegion> transformationRegion = new List<TRegion> { transformation.Trans };

                    //var x = Decomposer.GetInstance().DecomposeToOutput(metadataRegion);
                    //var y = Decomposer.GetInstance().DecomposeToOutput(transformationRegion);

                    string a = metadata.Trans.Text.Replace("\r\n", "").Trim();
                    a = Regex.Replace(a, @"\s+", "");
                    string b = transformation.Trans.Text.Replace("\r\n", "").Trim();
                    b = Regex.Replace(b, @"\s+", "");

                    bool isEqual = a.Equals(b);

                    if (metadata.Location.Region.Equals(transformation.Location.Region) && isEqual)
                    {
                        isFound = true;
                        found = transformation;
                        break;
                    }
                }

                if (!isFound)
                {
                    return metadata;
                }
            }
            return null;
        }

        private static int GetIndexLocation(List<CodeLocation> locations, CodeLocation location)
        {
            for (int i = 0; i < locations.Count; i++)
            {
                CodeLocation current = locations[i];
                if (current.Region.Equals(location.Region)) return i;
            }

            return -1;
        }
    }
}
