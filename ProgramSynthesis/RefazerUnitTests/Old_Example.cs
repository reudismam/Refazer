﻿using System;
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
            var classes = new List<string> { "SyntaxTreeExtensions" };
            CompleteTestBase(classes);
        }

        [TestMethod]
        public void N20()
        {
            var classes = new List<string> { "PackageRepositoryExtensions" };
            CompleteTestBase(classes);
        }

        [TestMethod]
        public void P1014()
        {
            var classes = new List<string> { "WriteConsoleCmdlet", "Write-Object" };
            var classesApply = new List<string> { "Eventlog", "write", "WriteConsoleCmdlet", "Write-Object", "WriteProgressCmdlet" };
            CompleteTestBase(classes, classesApply, @"1014\");
        }

        [TestMethod]
        public void NJ025()
        {
            var classes = new List<string> { "IsoDateTimeConverter" };
            var classesApply = new List<string> { "IsoDateTimeConverter" };
            CompleteTestBase(classes, classesApply, @"NJ025\");
        }

        [TestMethod]
        public void NJ023()
        {
            var classes = new List<string> { "JToken" };
            var classesApply = new List<string> { "JToken" };
            CompleteTestBase(classes, classesApply, @"NJ023\");
        }

        [TestMethod]
        public void NJ059()
        {
            var classes = new List<string> { "JsonTextWriter.Async" };
            var classesApply = new List<string> { "JsonTextWriter.Async" };
            CompleteTestBase(classes, classesApply, @"NJ059\");
        }

        [TestMethod]
        public void NJ224()
        {
            var classes = new List<string> { "XmlNodeConverter" };
            var classesApply = new List<string> { "XmlNodeConverter" };
            CompleteTestBase(classes, classesApply, @"NJ224\");
        }


        [TestMethod]
        public void NJ225()
        {
            var classes = new List<string> { "JArray", "JObject", "JToken" };
            var classesApply = new List<string> { "JArray", "JObject", "JToken" };
            CompleteTestBase(classes, classesApply, @"NJ225\");
        }


        [TestMethod]
        public void NJ234()
        {
            var classes = new List<string> { "JsonTextReader.Async", "JsonTextWriter.Async" };
            var classesApply = new List<string> { "JsonTextReader.Async", "JsonTextWriter.Async" };
            CompleteTestBase(classes, classesApply, @"NJ234\");
        }

        [TestMethod]
        public void NJ236()
        {
            var classes = new List<string> { "BinaryConverter" };
            var classesApply = new List<string> { "BinaryConverter" };
            CompleteTestBase(classes, classesApply, @"NJ236\");
        }


        [TestMethod]
        public void NJ241()
        {
            var classes = new List<string> { "JsonTextWriter.Async" };
            var classesApply = new List<string> { "JsonTextWriter.Async" };
            CompleteTestBase(classes, classesApply, @"NJ241\");
        }

        [TestMethod]
        public void NJ242()
        {
            var classes = new List<string> { "JsonTextWriter.Async" };
            var classesApply = new List<string> { "JsonTextWriter.Async" };
            CompleteTestBase(classes, classesApply, @"NJ242\");
        }

        [TestMethod]
        public void NJ844()
        {
            var classes = new List<string> { "DataTableConverter" };
            var classesApply = new List<string> { "DataTableConverter" };
            CompleteTestBase(classes, classesApply, @"NJ844\");
        }

        [TestMethod]
        public void NJ1428()
        {
            var classes = new List<string> { "DynamicProxy" };
            var classesApply = new List<string> { "DynamicProxy" };
            CompleteTestBase(classes, classesApply, @"NJ1428\");
        }

        [TestMethod]
        public void NJ1479()
        {
            var classes = new List<string> { "XmlNodeConverterTest" };
            var classesApply = new List<string> { "XmlNodeConverterTest" };
            CompleteTestBase(classes, classesApply, @"NJ1479\");
        }

        [TestMethod]
        public void NJ1491()
        {
            var classes = new List<string> { "JsonSerializerInternalReader" };
            var classesApply = new List<string> { "JsonSerializerInternalReader" };
            CompleteTestBase(classes, classesApply, @"NJ1491\");
        }

        [TestMethod]
        public void S002()
        {
            var classes = new List<string> { "ShapeManagerMenu" };
            var classesApply = new List<string> { "ShapeManagerMenu" };
            CompleteTestBase(classes, classesApply, @"S002\");
        }

        [TestMethod]
        public void S007()
        {
            var classes = new List<string> { "ShapeManagerMenu" };
            var classesApply = new List<string> { "ShapeManagerMenu" };
            CompleteTestBase(classes, classesApply, @"S007\");
        }

        [TestMethod]
        public void S043()
        {
            var classes = new List<string> { "RegionCaptureForm" };
            var classesApply = new List<string> { "RegionCaptureForm" };
            CompleteTestBase(classes, classesApply, @"S043\");
        }

        [TestMethod]
        public void S058()
        {
            var classes = new List<string> { "InputManager" };
            var classesApply = new List<string> { "InputManager" };
            CompleteTestBase(classes, classesApply, @"S058\");
        }


        [TestMethod]
        public void S044()
        {
            var classes = new List<string> { "RegionCaptureForm" };
            var classesApply = new List<string> { "RegionCaptureForm" };
            CompleteTestBase(classes, classesApply, @"S044\");
        }

        private void CompleteTestBase(List<string> examplesSet, List<string> toApply, string id = "")
        {
            string exampleFolder = GetExampleFolder() + id;
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
            foreach (var exampleFile in toApply)
            {
                //just the before version of the file
                var before = exampleFolder + exampleFile + @"B.cs";
                Refazer4CSharp.Apply(program, before);
            }
            //Get the before and after version of each transformed file.
            var transformedDocuments = ASTTransformer.Transform(TransformationInfos.GetInstance().Transformations);
            //Get the modified version
            var document = transformedDocuments.Select(o => o.Item2.ToString()).ToList();
        }

        public void CompleteTestBase(List<string> examples)
        {
            var toApply = new List<string>();
            string exampleFolder = GetExampleFolder();
            foreach (var example in examples)
            {
                //just the before version of the file
                var before = exampleFolder + example + @"B.cs";
                toApply.Add(before);
            }
            CompleteTestBase(examples, examples);
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


