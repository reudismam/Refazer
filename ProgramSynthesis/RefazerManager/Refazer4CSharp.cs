using System;
using System.Collections.Generic;
using System.Linq;
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

namespace RefazerManager
{
    public class Refazer4CSharp
    {
        private static Grammar _grammar;
      
        public static object [] Apply(ProgramNode program, string toApply)
        {
            var inputText = FileUtil.ReadFile(toApply);
            var inpTree = (SyntaxNodeOrToken)CSharpSyntaxTree.ParseText(inputText).GetRoot();
            var newInputState = State.Create(_grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(inpTree)));
            object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
            return output;
        }

        public static ProgramNode LearnTransformation(List<Tuple<string, string>> examples)
        {
            _grammar = GetGrammar();
            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < examples.Count; i++)
            {
                var example = examples[i];
                var inputText = FileUtil.ReadFile(example.Item1);
                var outputText = FileUtil.ReadFile(example.Item2);
                var inpTree = (SyntaxNodeOrToken) CSharpSyntaxTree.ParseText(inputText).GetRoot();
                var outTree = (SyntaxNodeOrToken) CSharpSyntaxTree.ParseText(outputText).GetRoot();
                var inputState = State.Create(_grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(inpTree)));
                ioExamples.Add(inputState, new List<object> {outTree});
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
