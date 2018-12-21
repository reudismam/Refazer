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
using RefazerObject.Transformation;
using RefazerObject.Region;
using RefazerObject.Constants;

namespace RefazerUnitTests
{
    [TestClass]
    public class Example
    {

        [TestMethod]
        public void E3()
        {
            var classes = new List<string> { "INTERNALCONTEXT", "HISTORYREPOSITORYTESTS" };
            var classesApply = new List<string> { "INTERNALCONTEXT", "HISTORYREPOSITORYTESTS" };
            CompleteTestBase(classes, classesApply, @"E3\");            
        }

        [TestMethod]
        public void E7()
        {
            var classes = new List<string> { "CommitFailureTests" };
            var classesApply = new List<string> { "CommitFailureTests" };
            CompleteTestBase(classes, classesApply, @"E7\");           
        }

        [TestMethod]
        public void E12()
        {
            var classes = new List<string> { "QueryableExtensions" };
            CompleteTestBase(classes);
        }

        [TestMethod]
        public void helloworld()
        {
            var classes = new List<string> { "hello" };
            CompleteTestBase(classes, classes, @"helloworld\");
        }

        [TestMethod]
        public void helloworldc()
        {
            var classes = new List<string> { "hello" };
            CompleteTestBase(classes, classes, @"helloworld\", file: "c");
        }

        [TestMethod]
        public void R35()
        {
            var classes = new List<string> { "SyntaxTreeExtensions" };
            var classesApply = new List<string> { "SyntaxTreeExtensions" };
            CompleteTestBase(classes, classesApply, @"E3\");
        }

        [TestMethod]
        public void N18()
        {
            var classes = new List<string> { "NuGetPowerShellBaseCommand" };
            var classesApply = new List<string> { "NuGetPowerShellBaseCommand", "OpenPackagePageCommand"};
            CompleteTestBase(classes, classesApply, @"N18\");
        }

        [TestMethod]
        public void N20()
        {
            var classes = new List<string> { "PackageRepositoryExtensions" };
            CompleteTestBase(classes);
        }

        [TestMethod]
        public void N21()
        {
            var classes = new List<string> { "DataServicePackageRepository", "VSPackageSourceRepository", "PackageRepositoryExtensions" };
            var classesApply = new List<string> { "DataServicePackageRepository","FallbackRepository", "LazyRepository", "MockServiceBasePackageRepository", "PackageRepositoryExtensions", "ServerPackageRepository", "VSPackageSourceRepository" };
            CompleteTestBase(classes, classesApply, @"N21\");
        }

        [TestMethod]
        public void N28()
        {
            var classes = new List<string> { "GetPackageCommand" };
            var classesApply = new List<string> { "GetPackageCommand", "InstallPackageCommand", "UninstallPackageCommand", "UpdatePackageCommand", "OpenPackagePageCommand" };
            CompleteTestBase(classes, classesApply, @"N28\");
        }

        [TestMethod]
        public void N29()
        {
            var classes = new List<string> { "PackageSolutionDetailControlModel" };
            var classesApply = new List<string> { "PackageSolutionDetailControlModel" };
            CompleteTestBase(classes, classesApply, @"N29\");
        }

        [TestMethod]
        public void R30()
        {
            var classes = new List<string> { "LanguageParser" };
            var classesApply = new List<string> { "LanguageParser" };
            CompleteTestBase(classes, classesApply, @"R30\");
        }

        [TestMethod]
        public void R50()
        {
            var classes = new List<string> { "SourceDelegateMethodSymbol" };
            var classesApply = new List<string> { "SourceDelegateMethodSymbol" };
            CompleteTestBase(classes, classesApply, @"R50\");
        }

        [TestMethod]
        public void R54()
        {            
            var classes = new List<string> { "ApplyDiagnosticAnalyzerAttributeFix" };
            var classesApply = new List<string> { "ApplyDiagnosticAnalyzerAttributeFix", "CA1008CodeFixProviderBase", "CA1012CodeFixProvider", "CA1309CodeFixProviderBase", "CA1309CSharpCodeFixProvider", "CA2101CodeFixProviderBase", "CA2101CSharpCodeFixProvider", "CA2213CSharpCodeFixProvider", "CA2229CodeFixProvider", "CA2235CodeFixProviderBase", "CA2237CodeFixProvider", "CodeGeneration", "EnumWithFlagsCodeFixProviderBase", "ISymbolExtensions", "SymbolEditorTests", "SyntaxNodeTests" };
            CompleteTestBase(classes, classesApply, @"R54\");
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
            var classes = new List<string> { "JArray", "JObject" };
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
            var classesApply = new List<string> { "JsonSerializerInternalReader", "JsonSerializerInternalWriter" };
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

        [TestMethod]
        public void S224()
        {
            var classes = new List<string> { "UploadersConfigForm" };
            var classesApply = new List<string> { "UploadersConfigForm" };
            CompleteTestBase(classes, classesApply, @"S224\");
        }
        [TestMethod]
        public void S236()
        {
            var classes = new List<string> { "ShapeManagerMenu" };
            var classesApply = new List<string> { "ShapeManagerMenu" };
            CompleteTestBase(classes, classesApply, @"S236\");
        }

        [TestMethod]
        public void S431()
        {
            var classes = new List<string> { "FFmpegDownloader" };
            var classesApply = new List<string> { "FFmpegDownloader", "SettingManager" };
            CompleteTestBase(classes, classesApply, @"S431\");
        }

        [TestMethod]
        public void S564()
        {
            var classes = new List<string> { "Helpers" };
            var classesApply = new List<string> { "GitHubUpdateChecker", "Helpers", "WorkerTask", "XMLUpdateChecker" };
            CompleteTestBase(classes, classesApply, @"S564\");
        }

        [TestMethod]
        public void S571()
        {
            var classes = new List<string> { "HotkeyInfo" };
            var classesApply = new List<string> { "HotkeyInfo" };
            CompleteTestBase(classes, classesApply, @"S571\");
        }

        [TestMethod]
        public void S583()
        {
            var classes = new List<string> { "ShapeManagerMenu" };
            var classesApply = new List<string> { "ShapeManagerMenu", "ShapeManager" };
            CompleteTestBase(classes, classesApply, @"S583\");
        }

        [TestMethod]
        public void S592()
        {
            var classes = new List<string> { "ShapeManager" };
            var classesApply = new List<string> { "ShapeManager" };
            CompleteTestBase(classes, classesApply, @"S592\");
        }

        [TestMethod]
        public void S761()
        {
            var classes = new List<string> { "Dropbox" };
            var classesApply = new List<string> { "Dropbox" };
            CompleteTestBase(classes, classesApply, @"S761\");
        }

        [TestMethod]
        public void S863()
        {
            var classes = new List<string> { "MainForm" };
            var classesApply = new List<string> { "MainForm" };
            CompleteTestBase(classes, classesApply, @"S863\");
        }

        [TestMethod]
        public void S897()
        {
            var classes = new List<string> { "TextAnimation" };
            var classesApply = new List<string> { "TextAnimation" };
            CompleteTestBase(classes, classesApply, @"S897\");
        }

        [TestMethod]
        public void S1021()
        {
            var classes = new List<string> { "RectangleRegionForm" };
            var classesApply = new List<string> { "RectangleRegionForm" };
            CompleteTestBase(classes, classesApply, @"S1021\");
        }

        [TestMethod]
        public void S1088()
        {
            var classes = new List<string> { "ShapeManager" };
            var classesApply = new List<string> { "ShapeManager" };
            CompleteTestBase(classes, classesApply, @"S1088\");
        }

        [TestMethod]
        public void S1549()
        {
            var classes = new List<string> { "AreaManager" };
            var classesApply = new List<string> { "AreaManager" };
            CompleteTestBase(classes, classesApply, @"S1549\");
        }

        [TestMethod]
        public void S1580()
        {
            var classes = new List<string> { "AreaManager" };
            var classesApply = new List<string> { "AreaManager" };
            CompleteTestBase(classes, classesApply, @"S1580\");
        }

        [TestMethod]
        public void S1810()
        {
            var classes = new List<string> { "ColorPickerForm" };
            var classesApply = new List<string> { "AutomateForm", "ColorPickerForm", "GradientPickerForm" };
            CompleteTestBase(classes, classesApply, @"S1810\");
        }

        [TestMethod]
        public void S2076()
        {
            var classes = new List<string> { "Program" };
            var classesApply = new List<string> { "Program" };
            CompleteTestBase(classes, classesApply, @"S2076\");
        }

        [TestMethod]
        public void S3778()
        {
            var classes = new List<string> { "MainForm" };
            var classesApply = new List<string> { "MainForm" };
            CompleteTestBase(classes, classesApply, @"S3778\");
        }

        [TestMethod]
        public void S3791()
        {
            var classes = new List<string> { "ImageHelpers" };
            var classesApply = new List<string> { "ImageHelpers" };
            CompleteTestBase(classes, classesApply, @"S3791\");
        }

        private void CompleteTestBase(List<string> examplesSet, List<string> toApply, string id = "", string file = "cs")
        {
            string exampleFolder = GetExampleFolder() + id;
            var examples = new List<Tuple<string, string>>();
            //Create the before and after version
            foreach (var exampleFile in examplesSet)
            {
                //just the before version of the file
                var before = exampleFolder + exampleFile + @"B." + file;
                //just the after version of the file
                var after = exampleFolder + exampleFile + @"A." + file;
                //create the examples
                var tuple = Tuple.Create(before, after);
                examples.Add(tuple);
            }
            if (file.Equals("cs"))
            {
                processCSharp(toApply, file, examples, exampleFolder);
            }
            else
            {
                processANTLR(toApply, file, examples, exampleFolder);
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


