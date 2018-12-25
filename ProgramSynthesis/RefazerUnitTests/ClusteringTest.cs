using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
using RefazerObject.Region;
using RefazerObject.Constants;
using Clustering;

namespace RefazerUnitTests
{
    [TestClass]
    public class ClusteringTest
    {

        [TestMethod]
        public void E3()
        {
            var students = new List<int> { 1 };
            var question = 1;
            var toFixStudent = 2;
            CompleteTestBase(students, question, toFixStudent);
        }

        private void CompleteTestBase(List<int> students, int question, int target, string id = @"submissions\")
        {
            string exampleFolder = GetExampleFolder() + id;
            var examples = new List<Tuple<string, string>>();
            //Create the before and after version
            foreach (var student in students)
            {
                //just the before version of the file
                var before = exampleFolder + "st_" + student + "_q_" + question + "_sub_" + 1 + @".c";
                //just the after version of the file
                var after = exampleFolder + "st_" + student + "_q_" + question + "_sub_" + 2 + @".c";
                //create the examples
                var tuple = Tuple.Create(before, after);
                examples.Add(tuple);
            }
            var clusters = ClusteringAlgorithm.Clusters(examples);
            var toApply = exampleFolder + "st_" + target + "_q_" + question + "_sub_" + 1 + @".c";
            foreach (var cluster in clusters)
            {
                var programStr = cluster.Program.ToString();
                Refazer4CSharp.Apply(cluster.Program, toApply);
                //Get the before and after version of each transformed file.
                var transformations = TransformationInfos.GetInstance().Transformations;
                var transformedDocuments = ASTTransformer.StringTransformation(transformations);
                string outputFilePath = exampleFolder + "main.c";
                FileUtil.WriteToFile(outputFilePath, transformedDocuments.First());
                EvaluateSubmission(outputFilePath, question);
            }
        }

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        private static void EvaluateSubmission(string cfile, int q)
        {
            var folderOutput = cfile.Substring(0, cfile.LastIndexOf('\\') + 1);
            var pathoutput = folderOutput + $"q_{q}_output.txt";
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = $"/C gcc -o a.exe \"{cfile}\" | a.exe < \"{folderOutput}q_{q}_input.txt\"";
            process.StartInfo = startInfo;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            var errors = process.StandardError.ReadToEnd();
            if (!output.IsEmpty())
            {
                FileUtil.WriteToFile(folderOutput + $"q_{q}_output_tool.txt", output);
            }
            else if (!errors.IsEmpty())
            {
                FileUtil.WriteToFile(pathoutput, errors);
            }
            else
            {
                FileUtil.WriteToFile(pathoutput, "Occurs an error while running process.");
            }
        }

        private void processANTLR(List<string> toApply, string file, List<Tuple<string, string>> examples, string exampleFolder)
        {
            //learn a transformation using Refazer
            var program = Refazer4CSharp.LearnTransformationANTLR(examples);
            //Apply the transformation to some files.
        }

        private static void processCSharp(List<string> toApply, string file, List<Tuple<string, string>> examples, string exampleFolder)
        {
            //learn a transformation using Refazer
            var program = Refazer4CSharp.LearnTransformation(examples);
            //Apply the transformation to some files.
            foreach (var exampleFile in toApply)
            {
                //just the before version of the file
                var before = exampleFolder + exampleFile + @"B." + file;
                Refazer4CSharp.Apply(program, before);
            }
            try
            {
                //Get the before and after version of each transformed file.
                var transformations = TransformationInfos.GetInstance().Transformations;
                var beforeAfter = TestUtil.GetBeforeAfterList(GetExampleFolder());
                JsonUtil<List<Tuple<Region, string, string>>>.Write(beforeAfter,
                    exampleFolder + TestConstants.BeforeAfterLocations + "ranking" + ".json");
                var transformedDocuments = ASTTransformer.Transform(transformations);
                //Get the modified version
                var document = transformedDocuments.Select(o => o.Item2.ToString()).ToList();
            }
            catch (Exception e)
            {
                //Ignored.
            }
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
                var inpTree = (SyntaxNodeOrToken)CSharpSyntaxTree.ParseText(inputText, path: example.Item1).GetRoot();
                var outTree = (SyntaxNodeOrToken)CSharpSyntaxTree.ParseText(outputText, path: example.Item2).GetRoot();
                var typesInput = GetNodesByType(inpTree, new List<SyntaxKind> { SyntaxKind.MethodDeclaration });
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


