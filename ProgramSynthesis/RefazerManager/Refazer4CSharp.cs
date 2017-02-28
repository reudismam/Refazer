using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Specifications;
using ProseFunctions.Spg.Bean;
using ProseFunctions.Substrings;
using System;
using System.Collections.Generic;
using System.IO;
using static ProseFunctions.Utils;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProseFunctions;

namespace ProseManager
{
    public class Refazer4CSharp
    {
        public string LearnTransformations(Tuple<string, string> examples)
        {
            //Load grammar
            var grammar = GetGrammar();

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

        public static Grammar GetGrammar()
        {
            string path = GetBasePath();
            var grammar = Utils.LoadGrammar(path + @"\grammar\Transformation.grammar");
            return grammar;
        }
        public static string GetBasePath()
        {
            //var grammar = Utils.LoadGrammar(path + @"grammar\Transformation.grammar");
            //return grammar;
            //string path = GetTestDataFolder(@"\");
            //return path;
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            //string startupPath2 = Environment.CurrentDirectory;
            //string startupPath = System.AppDomain.CurrentDomain.BaseDirectory;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - 3));
            string result = projectPath;
            return result;
        }
        static string GetTestDataFolder(string testDataLocation)
        {
            string startupPath = System.AppDomain.CurrentDomain.BaseDirectory;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - 4));
            string result = projectPath + testDataLocation;
            return result;
        }
    }
}
