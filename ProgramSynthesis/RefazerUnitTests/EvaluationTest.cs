using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Specifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RefazerFunctions;
using RefazerFunctions.Spg.Bean;
using RefazerManager;
using TreeElement;
using TreeElement.Spg.Node;
using UnitTests;

namespace RefazerUnitTests
{
    [TestClass]
    public class EvaluationTest
    {
        static string GetTestDataFolder(string testDataLocation)
        {
            string startupPath = System.AppDomain.CurrentDomain.BaseDirectory;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - 4));
            string result = projectPath + testDataLocation;
            return result;
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
        public static bool CompleteTestBase(string commit, List<Tuple<string, string>> documents)
        {
            //Load grammar
            var grammar = GetGrammar();

            //Examples
            var examplesInput = new List<SyntaxNodeOrToken>();
            var examplesOutput = new List<SyntaxNodeOrToken>();

            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (var document in documents)
            {
                string before = FileUtil.ReadFile(document.Item1);
                string after = FileUtil.ReadFile(document.Item2);
                SyntaxNodeOrToken inpTree = CSharpSyntaxTree.ParseText(before, path: document.Item1).GetRoot();
                SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(after, path: document.Item2).GetRoot();
                var elementsInput = GetNodesByType(inpTree, new List<SyntaxKind> { SyntaxKind.ClassDeclaration });
                var elementsOutput = GetNodesByType(outTree, new List<SyntaxKind> { SyntaxKind.ClassDeclaration });
                examplesInput.AddRange(elementsInput);
                examplesOutput.AddRange(elementsOutput);
            }

            for (int index = 0; index < examplesInput.Count; index++)
            {
                var inputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(examplesInput.ElementAt(index))));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(index) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            ProgramNode program = Utils.Learn(grammar, spec, new RankingScore(grammar), new WitnessFunctions(grammar));
            return true;
        }
    }
}
