using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RefazerManager;
using TreeEdit.Spg.Isomorphic;
using TreeEdit.Spg.LogInfo;
using TreeEdit.Spg.Transform;
using TreeElement;
using TreeElement.Spg.Node;

namespace RefazerUnitTests
{
    [TestClass]
    public class Example
    {
        [TestMethod]
        public void E12()
        {
            var classes = new List<string> { "QueryableExtensions" };
            CompleteTestBase(classes);
        }

        [TestMethod]
        public void R35()
        {
            var classes = new List<string> { "EditAndContinueTestBase", "SyntaxTreeExtensions" };
            CompleteTestBase(classes);
        }

        [TestMethod]
        public void N20()
        {
            var classes = new List<string> { "QueryableExtensions" };
            CompleteTestBase(classes);
        }

        [TestMethod]
        public void hello()
        {
            var classes = new List<string> { "hello" };
            CompleteTestBase(classes);
        }

        private void CompleteTestBase(List<string> examplesSet)
        {
            string exampleFolder = GetExampleFolder();
            var examples = new List<Tuple<string, string>>();
            //Create the before and after version
            foreach (var exampleFile in examplesSet)
            {
                //just the before version of the file
                var before = exampleFolder + exampleFile + @"B.cs";
                //just the after version of the file
                var after = exampleFolder + exampleFile + @"A.cs";
                //create the examples
                var tuple = Tuple.Create(before, after);
                examples.Add(tuple);
            }
            //learn a transformation using Refazer
            var program = Refazer4CSharp.LearnTransformation(examples);
            //Apply the transformation to some files.
            foreach (var example in examples)
            {
                var before = example.Item1;
                Refazer4CSharp.Apply(program, before);
            }
            var transformations = TransformationInfos.GetInstance().Transformations;
            //Get the before and after version of each transformed file.
            var transformedDocuments = ASTTransformer.Transform(transformations);
            //Get the modified version
            var document = transformedDocuments.Select(o => o.Item2.ToString()).ToList();
        }

        private void CompleteTestBaseType(string exampleId)
        {
            string exampleFolder = GetExampleFolder();
            var before = exampleFolder + exampleId + @"B.cs";
            var after = exampleFolder + exampleId + @"A.cs";
            var tuple = Tuple.Create(before, after);
            var examples = new List<Tuple<string, string>>();
            examples.Add(tuple);
            var exampleMethods = BuildExamples(examples);
            var program = Refazer4CSharp.LearnTransformation(exampleMethods);
            Refazer4CSharp.Apply(program, before);
            var transformedDocuments = ASTTransformer.Transform(TransformationInfos.GetInstance().Transformations);
            var document = transformedDocuments.First().Item2.ToString();
        }

        public List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>> BuildExamples(List<Tuple<string, string>> examples)
        {
            var methods = new List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>>();
            foreach (var example in examples)
            {
                var inputText = FileUtil.ReadFile(example.Item1);
                var outputText = FileUtil.ReadFile(example.Item2);
                var inpTree = (SyntaxNodeOrToken) CSharpSyntaxTree.ParseText(inputText, path: example.Item1).GetRoot();
                var outTree = (SyntaxNodeOrToken) CSharpSyntaxTree.ParseText(outputText, path: example.Item2).GetRoot();
                var typesInput = GetNodesByType(inpTree, new List<SyntaxKind> {SyntaxKind.MethodDeclaration});
                var typesOutput = GetNodesByType(outTree, new List<SyntaxKind> { SyntaxKind.MethodDeclaration });
                for (int i = 0; i < typesInput.Count; i++)
                {
                    var mInput = typesInput[i];
                    var mOutput = typesOutput[i];
                    if (!IsomorphicManager<SyntaxNodeOrToken>.IsIsomorphic(ConverterHelper.ConvertCSharpToTreeNode(mInput), ConverterHelper.ConvertCSharpToTreeNode(mOutput)))
                    {
                        methods.Add(Tuple.Create(mInput, mOutput));
                    }
                }
            }
            return methods;
        }

        /// <summary>
        /// Gets example folder
        /// </summary>
        public static string GetExampleFolder()
        {
            string path = FileUtil.GetBasePath() + @"\ProgramSynthesis\example\";
            return path;
        }

        private static List<SyntaxNodeOrToken> GetNodesByType(SyntaxNodeOrToken outTree, List<SyntaxKind> kinds)
        {
            //select nodes of type method declaration
            var exampleMethodsInput = from inode in outTree.AsNode().DescendantNodesAndSelf()
                                      where kinds.Where(inode.IsKind).Any()
                                      select inode;
            //Select two examples
            var examplesSotInput = exampleMethodsInput.Select(sot => (SyntaxNodeOrToken)sot).ToList();
            //.GetRange(0, 1);
            //var examplesInput = examplesSotInput.Select(o => (object)o).ToList();
            return examplesSotInput;
        }
    }
}


