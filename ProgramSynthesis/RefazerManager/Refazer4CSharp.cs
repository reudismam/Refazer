using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Demo;
using Irony.Parsing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Specifications;
using Microsoft.ProgramSynthesis.Utils;
using RefazerFunctions;
using RefazerFunctions.Bean;
using TreeElement;
using TreeElement.Spg.Node;
using TreeEdit.Spg.Print;
using Grammar = Microsoft.ProgramSynthesis.Grammar;
using Parser = Antlr4.Runtime.Parser;

namespace RefazerManager
{
    public class Refazer4CSharp
    {
        private static Grammar _grammar;
      
        public static object [] Apply(ProgramNode program, string toApply)
        {
            var inputText = FileUtil.ReadFile(toApply);
            var inpTree = (SyntaxNodeOrToken) CSharpSyntaxTree.ParseText(inputText, path: toApply).GetRoot();
            var newInputState = State.Create(_grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(inpTree)));
            object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            return output;
        }

        public static ProgramNode LearnTransformation(List<Tuple<string, string>> examples)
        {
            _grammar = GetGrammar();
            //building examples
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examples.Count; i++)
            {
                var example = examples[i];
                var inputText = FileUtil.ReadFile(example.Item1);
                var outputText = FileUtil.ReadFile(example.Item2);
                var inpTree = (SyntaxNodeOrToken) CSharpSyntaxTree.ParseText(inputText, path: example.Item1).GetRoot();
                var outTree = (SyntaxNodeOrToken) CSharpSyntaxTree.ParseText(outputText, path: example.Item2).GetRoot();
                var inputState = State.Create(_grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(inpTree)));
                ioExamples.Add(inputState, new List<object> {outTree});
            }
            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(_grammar, spec, new RankingScore(_grammar), new WitnessFunctions(_grammar));
            return program;
        }

        public static ParseTreeNode GetRoot(string sourceCode, Irony.Parsing.Grammar grammar)
        {
            LanguageData language = new LanguageData(grammar);
            Irony.Parsing.Parser parser = new Irony.Parsing.Parser(grammar);
            ParseTree parseTree = parser.Parse(sourceCode);
            ParseTreeNode root = parseTree.Root;
            return root;
        }

        public static ProgramNode LearnTransformationANTLR(List<Tuple<string, string>> examples)
        {
            return null;
            /*_grammar = GetGrammar();
            //building examples
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examples.Count; i++)
            {
                var example = examples[i];
                var inputText = FileUtil.ReadFile(example.Item2);
                var outputText = FileUtil.ReadFile(example.Item2);
                StringBuilder text = new StringBuilder(inputText);
                AntlrInputStream inputStream = new AntlrInputStream(text.ToString());
                CLexer speakLexer = new CLexer(inputStream);
                MyCGrammarExtended grammar = new MyCGrammarExtended();
                ParseTreeNode root = GetRoot(inputText, grammar);
                CommonTokenStream commonTokenStream = new CommonTokenStream(speakLexer);
                CParser speakParser = new CParser(commonTokenStream);
                speakParser.BuildParseTree = true;

                var compilationUnit = speakParser.compilationUnit();
                var node = ConverterHelper.ConvertANTLRToTreeNode(compilationUnit, commonTokenStream);
                var str = PrintUtil<RefazerNode>.PrettyPrintString(node);
                
                var children = compilationUnit.children;
                var child = children.First();
       
                var inpTree = (SyntaxNodeOrToken)CSharpSyntaxTree.ParseText(inputText, path: example.Item1).GetRoot();
                var outTree = (SyntaxNodeOrToken)CSharpSyntaxTree.ParseText(outputText, path: example.Item2).GetRoot();
                var inputState = State.Create(_grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(inpTree)));
                ioExamples.Add(inputState, new List<object> { outTree });
            }
            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(_grammar, spec, new RankingScore(_grammar), new WitnessFunctions(_grammar));
            return program;*/
        }

        public static ProgramNode LearnTransformation(List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>> examples)
        {
            _grammar = GetGrammar();
            //building examples
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examples.Count; i++)
            {
                var example = examples[i];
                var inpTree = example.Item1;
                var outTree = example.Item2;
                var inputState = State.Create(_grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(inpTree)));
                ioExamples.Add(inputState, new List<object> { outTree });
            }
            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(_grammar, spec, new RankingScore(_grammar), new WitnessFunctions(_grammar));
            return program;
        }

        /// <summary>
        /// Gets the grammar
        /// </summary>
        /// <returns>Grammar</returns>
        public static Grammar GetGrammar()
        {
            string path = FileUtil.GetBasePath();
            var grammar = Utils.LoadGrammar(path + @"\ProgramSynthesis\grammar\Transformation.grammar");
            return grammar;
        }
    }
}
