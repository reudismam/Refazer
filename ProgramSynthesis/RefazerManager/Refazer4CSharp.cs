using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Specifications;
using RefazerFunctions.Substrings;
using System;
using System.Collections.Generic;
using System.IO;
using static RefazerManager.Utils;
using System.Linq;
using RefazerFunctions;
using RefazerFunctions.Spg.Bean;
using RefazerManager;
using TreeElement;
using TreeElement.Spg.Node;

namespace ProseManager
{
    public class Refazer4CSharp
    {
        public string LearnTransformationsPath(Grammar grammar, Tuple<string, string> examples)
        {
            //input data
            string inputText = File.ReadAllText(examples.Item1);
            SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(inputText).GetRoot();

            //output with some code fragments edited.
            string outputText = File.ReadAllText(examples.Item2);
            SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(outputText).GetRoot();

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(inpTree)));
                ioExamples.Add(inputState, new List<object> { outTree });
           

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Learn(grammar, spec, new RankingScore(grammar), new WitnessFunctions(grammar));
            return program.ToString();
        }

        public ProgramNode LearnTransformations(Grammar grammar, List<Tuple<string, string>> examples)
        {
            //input data
            var astsInput = examples.Select(o => CSharpSyntaxTree.ParseText(o.Item1).GetRoot()).ToList();
            //output with some code fragments edited.
            var astsOutput = examples.Select(o => CSharpSyntaxTree.ParseText(o.Item2).GetRoot()).ToList();

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            for (int i = 0; i < astsInput.Count(); i++)
            {
                var inpTree = astsInput.ElementAt(0);
                var outTree = astsOutput.ElementAt(0);
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(inpTree)));
                ioExamples.Add(inputState, new List<object> {outTree});
            }


            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Learn(grammar, spec, new RankingScore(grammar), new WitnessFunctions(grammar));
            return program;
        }

        public static Grammar GetGrammar()
        {
            string path = FileUtil.GetBasePath();
            var grammar = Utils.LoadGrammar(path + @"\ProgramSynthesis\grammar\Transformation.grammar");
            return grammar;
        }
    }
}
