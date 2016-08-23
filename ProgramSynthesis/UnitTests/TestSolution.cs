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
using Spg.ExampleRefactoring.Workspace;
using Spg.LocationRefactor.Location;
using Spg.LocationRefactor.TextRegion;
using Spg.LocationRefactor.Transform;
using Taramon.Exceller;
using TreeElement.Spg.Node;

namespace UnitTests
{
    [TestFixture]
    public class TestSolution
    {
        [Test]
        public void R2_8c14644()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\2_8c14644", solutionPath: @"Roslyn\roslyn\src\Roslyn.sln", examples: new List<int> { 0, 1, 3, 18 });
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void R0086821()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\0086821", solutionPath: @"Roslyn\roslyn7\src\Roslyn.sln", kinds: new List<SyntaxKind> {SyntaxKind.ClassDeclaration}, examples: new List<int> {0, 1, 2, 6});
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void Rcd68d03()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\cd68d03", solutionPath: @"Roslyn\roslyn7\src\Roslyn.sln");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void N3_d9f64ea()
        {
            var isCorrect = CompleteTestBase(@"Nuget\3_d9f64ea", solutionPath: @"NuGet\nuget7\NuGet.sln");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void R2_7c885ca()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\2_7c885ca", solutionPath: @"Roslyn\roslyn14\src\Roslyn.sln", kinds: new List<SyntaxKind> { SyntaxKind.AttributeList });
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void R2_673f18e()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\2_673f18e");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void R04d0604()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\04d0604", solutionPath: @"Roslyn\roslyn18\Src\Roslyn.sln", kinds: new List<SyntaxKind> { SyntaxKind.MethodDeclaration, SyntaxKind.PropertyDeclaration }, examples: new List<int> {0, 1, 3});
            Assert.IsTrue(isCorrect);
        }

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

        [Test]
        public void Re817dab()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\e817dab");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void R4_c96d9ce()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\4_c96d9ce");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void R5_c96d9ce()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\5_c96d9ce", examples: new List<int> {0, 1, 6, 11});
            Assert.IsTrue(isCorrect);
        }


        [Test]
        public void R2_cfd9b46()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\2_cfd9b46", examples: new List<int> { 0, 1, 3 });
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void R3_8c14644()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\3_8c14644");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void R00552fc()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\00552fc", examples: new List<int> { 0, 1, 2 });
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void R1113fd3()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\1113fd3");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void R4_673f18e()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\4_673f18e", examples: new List<int> {0, 1, 4, 5, 6, 7});
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void R4b40293()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\4b40293", solutionPath: @"Roslyn\roslyn7\src\Roslyn.sln", examples: new List<int> {0, 1, 2 });
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void R7c885ca()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\7c885ca", solutionPath: @"Roslyn\roslyn7\src\Roslyn.sln", kinds: new List<SyntaxKind> { SyntaxKind.ClassDeclaration }, examples: new List<int> {0, 1, 2});
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void Re28c812()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\e28c812", solutionPath: @"Roslyn\roslyn7\src\Roslyn.sln", kinds: new List<SyntaxKind> { SyntaxKind.UsingDirective });
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void E14623da()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\14623da", solutionPath: @"EntityFramework\entityframework10\EntityFramework.sln");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void R5_673f18e()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\5_673f18e");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void R673f18e()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\673f18e");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void R6_673f18e()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\6_673f18e");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void R83e4349()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\83e4349");
            Assert.IsTrue(isCorrect);
        }


        [Test]
        public void R8_673f18e()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\8_673f18e");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void R8ecd058()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\8ecd058", examples: new List<int> { 1, 2, 10 });
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void R9_673f18e()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\9_673f18e");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void Rb495c9a()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\b495c9a", kinds: new List<SyntaxKind> { SyntaxKind.ConstructorDeclaration });
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void Rc96d9ce()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\c96d9ce", examples: new List<int> { 0, 1, 3 });
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void Rcfd9b46()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\cfd9b46", examples: new List<int> { 0, 1, 2 });
            Assert.IsTrue(isCorrect);
        }

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

        [Test]
        public void E2_14323da()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\2_14623da", solutionPath: @"EntityFramework\entityframework10\EntityFramework.sln", examples: new List<int> { 0, 1 });
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void E2_326d525()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\2_326d525", examples: new List<int> {0, 1, 2});
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void E2bae908()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\2bae908");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void E3_14623da()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\3_14623da");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void E4_14623da()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\4_14623da");
            Assert.IsTrue(isCorrect);
        }

        //[Test]
        //public void E4_1571862()
        //{
        //    var isCorrect = CompleteTestBase(@"EntityFramewok\4_1571862");
        //    Assert.IsTrue(isCorrect);
        //}

        [Test]
        public void E5_1571862()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\5_1571862");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void E829dec5()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\829dec5", solutionPath: @"EntityFramework\entityframework7\EntityFramework.sln", examples: new List<int> {0, 1, 2});
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void E8b9180b()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\8b9180b");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void E2_8b9180b()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\2_8b9180b");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void E8d45249()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\8d45249");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void Ebc42e49()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\bc42e49", solutionPath: @"EntityFramework\entityframework2\EntityFramework.sln", examples: new List<int> { 0, 1, 2, 3, 4});
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void Ece1e333()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\ce1e333");
            Assert.IsTrue(isCorrect);
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
        public void N2_7d11ddd()
        {
            var isCorrect = CompleteTestBase(@"NuGet\2_7d11ddd", kinds: new List<SyntaxKind> { SyntaxKind.PropertyDeclaration });
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void N2_8da9f0e()
        {
            var isCorrect = CompleteTestBase(@"NuGet\2_8da9f0e", solutionPath: @"NuGet\nuget3\NuGet.sln");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void N3_8da9f0e()
        {
            var isCorrect = CompleteTestBase(@"NuGet\3_8da9f0e", examples: new List<int> {0, 1, 2});
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void N4_8da9f0e()
        {
            var isCorrect = CompleteTestBase(@"NuGet\4_8da9f0e", solutionPath: @"NuGet\nuget3\NuGet.sln");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void N2_a569c55()
        {
            var isCorrect = CompleteTestBase(@"NuGet\2_a569c55", examples: new List<int> {0, 1, 2});
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void N2_a883600()
        {
            var isCorrect = CompleteTestBase(@"NuGet\2_a883600", @"NuGet\nuget4\NuGet.sln");
            Assert.IsTrue(false);
        }

        [Test]
        public void N2_ee953e8()
        {
            var isCorrect = CompleteTestBase(@"NuGet\2_ee953e8");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void N2dea84e()
        {
            var isCorrect = CompleteTestBase(@"NuGet\2dea84e", solutionPath: @"NuGet\nuget2\NuGet.sln", examples: new List<int> { 0, 1, 2, 3, 7});
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void Ndfc4e3d()
        {
            var isCorrect = CompleteTestBase(@"NuGet\dfc4e3d", solutionPath: @"NuGet\nuget6\NuGet.sln", kinds: new List<SyntaxKind> { SyntaxKind.AttributeList });
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void N3_a883600()
        {
            var isCorrect = CompleteTestBase(@"NuGet\3_a883600", examples: new List<int> { 0, 1, 2, 3 });
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void N4ff8771()
        {
            var isCorrect = CompleteTestBase(@"NuGet\4ff8771");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void N5_8da9f0e()
        {
            var isCorrect = CompleteTestBase(@"NuGet\5_8da9f0e", examples: new List<int> {0, 1, 2});
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void N5_a883600()
        {
            var isCorrect = CompleteTestBase(@"NuGet\5_a883600");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void N74d4d32()
        {
            var isCorrect = CompleteTestBase(@"NuGet\74d4d32", examples: new List<int> { 0, 1, 2 });
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void N8da9f0e()
        {
            var isCorrect = CompleteTestBase(@"NuGet\8da9f0e");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void Na569c55()
        {
            var isCorrect = CompleteTestBase(@"NuGet\a569c55");
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void Na883600()
        {
            var isCorrect = CompleteTestBase(@"NuGet\a883600", examples: new List<int> { 0, 1, 3, 11 });
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void Nee953e8()
        {
            var isCorrect = CompleteTestBase(@"NuGet\ee953e8", solutionPath: @"NuGet\nuget10\NuGet.sln");
            Assert.IsTrue(true);
        }

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

        [Test]
        public void R3_c96d9ce()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\3_c96d9ce");
            Assert.IsTrue(isCorrect);
        }

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

        [Test]
        public void Rf66696e()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\f66696e", examples: new List<int> { 0, 1, 3 });
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void E1571862()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\1571862", examples: new List<int> { 0, 1, 8 });
            Assert.IsTrue(isCorrect);
        }

        [Test]
        public void E2_1571862()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\2_1571862", examples: new List<int> {2, 3});
            Assert.IsTrue(isCorrect);
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
            var exampleMethodsInput = from inode in outTree.AsNode().DescendantNodes()
                                      where inode.IsKind(kind)
                                      select inode;

            var examplesSotInput = exampleMethodsInput.Select(sot => (SyntaxNodeOrToken)sot).ToList();
            return examplesSotInput;
        }

        private static List<SyntaxNodeOrToken> GetNodesByType(SyntaxNodeOrToken outTree, List<SyntaxKind> kinds)
        {
            //select nodes of type method declaration
            var exampleMethodsInput = from inode in outTree.AsNode().DescendantNodes()
                                      where kinds.Where(inode.IsKind).Any()
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
        public static bool CompleteTestBase(string commit, string solutionPath = null, List<int> examples = null, List<SyntaxKind> kinds = null)
        {
            if (examples == null) examples = new List<int> { 0, 1 };
            if (kinds == null) kinds = new List<SyntaxKind> { SyntaxKind.MethodDeclaration, SyntaxKind.ConstructorDeclaration };

            //Load grammar
            var grammar = GetGrammar();

            string expHome = Environment.GetEnvironmentVariable("EXP_HOME", EnvironmentVariableTarget.User);
            List<CodeTransformation> codeTransformations = JsonUtil<List<CodeTransformation>>.Read(expHome + @"cprose\" + commit + @"\metadata\transformed_locations.json");
            List<TRegion> regions = codeTransformations.Select(entry => entry.Trans).ToList();
            var locations = codeTransformations.Select(entry => entry.Location).ToList();

            var metadataRegions = examples.Select(index => regions[index]).ToList();//regions.GetRange(0, examples).ToList();
            var locationRegions = examples.Select(index => locations[index]).ToList();//locations.GetRange(0, examples).ToList();

            var globalTransformations = RegionManager.GetInstance().GroupTransformationsBySourcePath(codeTransformations);

            //            for (int i = 0; i < regions.Count; i++)
            //k            {
            var dicionarySelection = RegionManager.GetInstance().GroupRegionBySourcePath(metadataRegions);
            //Examples
            var examplesInput = new List<SyntaxNodeOrToken>();
            var examplesOutput = new List<SyntaxNodeOrToken>();

            SyntaxNodeOrToken inpTree = default(SyntaxNodeOrToken);
            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (KeyValuePair<string, List<TRegion>> entry in dicionarySelection)
            {
                string sourceCode = FileUtil.ReadFile(entry.Key);
                Tuple<string, List<TRegion>> tu = Transform(sourceCode, globalTransformations[entry.Key.ToUpperInvariant()], metadataRegions);
                string sourceCodeAfter = tu.Item1;

                inpTree = CSharpSyntaxTree.ParseText(sourceCode, path: entry.Key).GetRoot();
                SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(sourceCodeAfter).GetRoot();

                var allMethodsInput = GetNodesByType(inpTree, kinds);
                var allMethodsOutput = GetNodesByType(outTree, kinds);
                var inputMethods = new List<int>();
                foreach (var region in locationRegions.Where(o => o.SourceClass.ToUpperInvariant().Equals(entry.Key.ToUpperInvariant())))
                {
                    for (int index = 0; index < allMethodsInput.Count; index++)
                    {
                        var method = allMethodsInput[index];
                        var tRegion = new TRegion
                        {
                            Start = method.SpanStart,
                            Length = method.Span.Length
                        };

                        if (region.Region.IsInside(tRegion))
                        {
                            inputMethods.Add(index);
                        }
                    }
                }

                //Examples
                var inpExs = inputMethods.Distinct().Select(index => allMethodsInput[index]).ToList();
                var outExs = inputMethods.Distinct().Select(index => allMethodsOutput[index]).ToList();

                examplesInput.AddRange(inpExs);
                examplesOutput.AddRange(outExs);
            }

            examplesInput = RemoveDuplicates(examplesInput);
            for (int index = 0; index < examplesInput.Count; index++)
            {
                //var inps = examplesInput.ElementAt(index).ToFullString();
                //var outps = examplesOutput.ElementAt(index).ToFullString();
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)examplesInput.ElementAt(index))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(index) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);

            long millBeforeLearn = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            ProgramNode program = Utils.Learn(grammar, spec);
            long millAfterLearn = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long totalTimeToLearn = millAfterLearn - millBeforeLearn;

            var methods = new List<SyntaxNodeOrToken>();
            if (solutionPath == null)
            {
                //Run program
                methods = GetNodesByType(inpTree, kinds);
            }
            else
            {
                string path = expHome + solutionPath;
                var files = WorkspaceManager.GetInstance().GetSourcesFiles(null, path);
                foreach (var v in files.Where(o => o.Item2.ToUpperInvariant().Equals(@"C:\Users\SPG-04\Documents\Exp\Roslyn\roslyn7\src\Compilers\Core\Portable\DiagnosticAnalyzer\DiagnosticAnalyzer.cs".ToUpperInvariant())))
                {
                    var tree = CSharpSyntaxTree.ParseText(v.Item1, path: v.Item2).GetRoot();
                    var vnodes = GetNodesByType(tree, kinds);
                    methods.AddRange(vnodes);
                }
            }

            var transformed = new List<object>();
            var dicTrans = new Dictionary<string, List<object>>();

            long millBeforeExecution = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            foreach (var method in methods)
            {
                var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
                object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
                transformed.AddRange(output);
                Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));

                if (output.Any())
                {
                    if (dicTrans.ContainsKey(method.SyntaxTree.FilePath.ToUpperInvariant()))
                    {
                        dicTrans[method.SyntaxTree.FilePath.ToUpperInvariant()].AddRange(output);
                    }
                    else
                    {
                        dicTrans[method.SyntaxTree.FilePath.ToUpperInvariant()] = output.ToList();
                    }
                }
            }
            long millAfterExecution = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            var totalTimeToExecute = millAfterExecution - millBeforeExecution;

            string s = "";
            foreach (var v in dicTrans)
            {
                s += $"{v.Key} \n====================\n";
                v.Value.ForEach(o => s += $"{o}\n");
            }

            Console.WriteLine($"Count: \n {transformed.Count}");
            s += $"Count: \n {transformed.Count}";
            FileUtil.WriteToFile(expHome + @"cprose\" + commit + @"\result.res", s);
            //                break;
            //            }

            //long millAfer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            //long totalTime = (millAfer - millBefore);
            Log(commit, totalTimeToLearn + totalTimeToExecute, examples.Count, regions.Count, transformed.Count,
                dicionarySelection.Count, program.ToString(), totalTimeToLearn, totalTimeToExecute);
            return true;
        }

        public static void Log(string commit, double time, int exTransformations, int locations, int acTrasnformation, int documents, string program, double timeToLearnEdit, double timeToTransformEdit)
        {
            string commitFirstLetter = commit.ElementAt(0).ToString();
            string commitId = commit.Substring(commit.IndexOf(@"\") + 1);

            commit = commitFirstLetter + "-" + commitId;

            string path = TestUtil.LOG_PATH;
            using (ExcelManager em = new ExcelManager())
            {

                em.Open(path);

                int empty;
                for (int i = 1; ; i++)
                {
                    if (em.GetValue("A" + i, Category.Formatted).ToString().Equals("") || em.GetValue("A" + i, Category.Formatted).ToString().Equals(commit))
                    {
                        empty = i;
                        break;
                    }
                }

                //if (empty != -1)
                //{
                em.SetValue("A" + empty, commit);
                em.SetValue("B" + empty, time / 1000);
                em.SetValue("C" + empty, exTransformations);
                em.SetValue("D" + empty, locations);
                em.SetValue("E" + empty, acTrasnformation);
                em.SetValue("F" + empty, documents);
                em.SetValue("G" + empty, program);
                em.SetValue("H" + empty, timeToLearnEdit / 1000);
                em.SetValue("I" + empty, timeToTransformEdit / 1000);
                em.Save();
                //}
                //else {
                //    Console.WriteLine("Could not write log to " + path);
                //}
            }
        }

        private static List<SyntaxNodeOrToken> RemoveDuplicates(List<SyntaxNodeOrToken> methods)
        {
            var removes = new List<SyntaxNodeOrToken>();
            for (int i = 0; i < methods.Count; i++)
            {
                for (int j = 0; j < methods.Count; j++)
                {
                    if (i != j && methods[i].SyntaxTree.FilePath.ToUpperInvariant().Equals(methods[j].SyntaxTree.FilePath.ToUpperInvariant()) && methods[i].Span.Contains(methods[j].Span))
                    {
                        removes.Add(methods[j]);
                    }
                }
            }

            var result = methods.Except(removes).ToList();
            return result;
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
                var sourceCodeUntilStart = sourceCode.Substring(0, start);
                var sourceCodeAfterSelection = sourceCode.Substring(end);
                sourceCode = sourceCodeUntilStart + transformation + sourceCodeAfterSelection;

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
