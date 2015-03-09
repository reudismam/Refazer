using System;
using System.Collections.Generic;
using System.Linq;
using ExampleRefactoring.Spg.ExampleRefactoring.Bean;
using ExampleRefactoring.Spg.ExampleRefactoring.RegularExpression;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using ExampleRefactoring.Spg.ExampleRefactoring.Util;
using LocationCodeRefactoring.Spg.LocationCodeRefactoring.Controller;
using LocationCodeRefactoring.Spg.LocationRefactor.Transformation;
using NUnit.Framework;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Util;
using Spg.NUnitTests.Util;
using NUnitTests.Spg.NUnitTests.LocationTestProject;

namespace NUnitTests.Spg.NUnitTests.CompleteTestProject
{
    /// <summary>
    /// Test for complete systematic editing
    /// </summary>
    [TestFixture]
    public class CompleteTestProject
    {
        // /// <summary>
        // /// Simple API Test for locations
        // /// </summary>
        // [Test]
        // public void SimpleAPIChangeTest()
        // {
        //     bool passLocation = LocationTest.LocaleTest(FilePath.SIMPLE_API_CHANGE_INPUT, FilePath.SIMPLE_API_CHANGE_OUTPUT_SELECTION, FilePath.MAIN_CLASS_SIMPLE_API_CHANGE_PATH);

        //     bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_SIMPLE_API_CHANGE_AFTER_EDITING, FilePath.SIMPLE_API_CHANGE_EDITION, @"\change_api\");

        //     Assert.IsTrue(passLocation && passTransformation);
        // }

        // /// <summary>
        // /// Parameter test on if
        // /// </summary>
        // [Test]
        // public void IntroduceParamOnIf()
        // {
        //     bool passLocation = LocationTest.LocaleTest(FilePath.INTRODUCE_PARAM_ON_IF_INPUT, FilePath.INTRODUCE_PARAM_ON_IF_OUTPUT_SELECTION, FilePath.MAIN_CLASS_INTRODUCE_PARAM_ON_IF_PATH);

        //     bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_INTRODUCE_PARAM_ON_IF_AFTER_EDITING, FilePath.INTRODUCE_PARAM_ON_IF_EDITION, @"\parameter_change_on_if\");

        //     Assert.IsTrue(passLocation && passTransformation);
        // }

        [Test]
        public void Proj00552fc2287f820ae9d42fd259aa6c07c2c5a805()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("00552fc2287f820ae9d42fd259aa6c07c2c5a805", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable6\Proj00552fc2287f820ae9d42fd259aa6c07c2c5a805.sln", "Proj00552fc2287f820ae9d42fd259aa6c07c2c5a805");

            List<string> list = new List<string>();
            list.Add("LanguageParser.cs");
            bool passTransformation = CompleteTestBase(list, @"00552fc2287f820ae9d42fd259aa6c07c2c5a805");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj2_cd68d0323eb97f18c10281847c831f8e361506b9()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("2_cd68d0323eb97f18c10281847c831f8e361506b9", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable8\Projcd68d0323eb97f18c10281847c831f8e361506b9.sln", "Projcd68d0323eb97f18c10281847c831f8e361506b9");

            List<string> list = new List<string>();
            list.Add("Project.cs");
            bool passTransformation = CompleteTestBase(list, @"2_cd68d0323eb97f18c10281847c831f8e361506b9");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj3_cd68d0323eb97f18c10281847c831f8e361506b9()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("3_cd68d0323eb97f18c10281847c831f8e361506b9", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable8\Projcd68d0323eb97f18c10281847c831f8e361506b9.sln", "Projcd68d0323eb97f18c10281847c831f8e361506b9");

            List<string> list = new List<string>();
            list.Add("Project.cs");
            bool passTransformation = CompleteTestBase(list, @"3_cd68d0323eb97f18c10281847c831f8e361506b9");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Projf66696e70c90ce7fa1476d53cc84cc18e438d19b()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("f66696e70c90ce7fa1476d53cc84cc18e438d19b", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\VBCSCompilerTests\Proje4d141a3c5f51ce4021d105e2b330564e02069fc.sln", "Proje4d141a3c5f51ce4021d105e2b330564e02069fc");

            List<string> list = new List<string>();
            list.Add("CompilerServerApiTest.cs");
            bool passTransformation = CompleteTestBase(list, @"f66696e70c90ce7fa1476d53cc84cc18e438d19b");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj4b402939708adf35a7a5e12ffc99dc14cc1f4766()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("2_4b402939708adf35a7a5e12ffc99dc14cc1f4766", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\CSharp2\Proj2_4b402939708adf35a7a5e12ffc99dc14cc1f4766.sln", "Proj2_4b402939708adf35a7a5e12ffc99dc14cc1f4766");

            List<string> list = new List<string>();
            list.Add("AssemblySymbol.cs"); list.Add("InternalsVisibleToAndStrongNameTests.cs");
            bool passTransformation = CompleteTestBase(list, @"2_4b402939708adf35a7a5e12ffc99dc14cc1f4766");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj8ecd05880b478e4ca997a4789b976ef73b070546()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("8ecd05880b478e4ca997a4789b976ef73b070546", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable7\Proj4b402939708adf35a7a5e12ffc99dc14cc1f4766.sln", "Proj4b402939708adf35a7a5e12ffc99dc14cc1f4766");

            List<string> list = new List<string>();
            list.Add("EmitExpression.cs");
            bool passTransformation = CompleteTestBase(list, @"8ecd05880b478e4ca997a4789b976ef73b070546");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj04d060498bc0c30403bb05872e396052d826d082()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("04d060498bc0c30403bb05872e396052d826d082", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Diagnostics2\Proj04d060498bc0c30403bb05872e396052d826d082.sln", "Proj04d060498bc0c30403bb05872e396052d826d082");

            List<string> list = new List<string>();
            list.Add("ApplyDiagnosticAnalyzerAttributeFix.cs"); list.Add("CA1052CSharpCodeFixProvider.cs");
            bool passTransformation = CompleteTestBase(list, @"04d060498bc0c30403bb05872e396052d826d082");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj318b2b0e476a122ebc033b13d41449ef1c814c1d()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("318b2b0e476a122ebc033b13d41449ef1c814c1d", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Core2\Proj318b2b0e476a122ebc033b13d41449ef1c814c1d.sln", "Proj318b2b0e476a122ebc033b13d41449ef1c814c1d");

            List<string> list = new List<string>();
            list.Add("DeclarePublicAPIFix.cs");
            bool passTransformation = CompleteTestBase(list, @"318b2b0e476a122ebc033b13d41449ef1c814c1d");

            Assert.IsTrue(passLocation && passTransformation);

        }

        /// <summary>
        /// Test case for parameter to constant value
        /// </summary>
        [Test]
        public void Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", "Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");

            List<string> list = new List<string>();
            list.Add("SyntaxFactory.cs");
            bool passTransformation = CompleteTestBase(list, @"c96d9ce1b2626b464cf2746ca53cb338d7d2ce38");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj1113fd3db14fd23fc081e90f27f4ddafad7b244d()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("1113fd3db14fd23fc081e90f27f4ddafad7b244d", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable\Proj1113fd3db14fd23fc081e90f27f4ddafad7b244d.sln", "Proj1113fd3db14fd23fc081e90f27f4ddafad7b244d");

            List<string> list = new List<string>();
            list.Add("MetadataWriter.cs");
            bool passTransformation = CompleteTestBase(list, @"1113fd3db14fd23fc081e90f27f4ddafad7b244d");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Change Exception test
        /// </summary>
        [Test]
        public void Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("cc3d32746f60ed5a9f3775ef0ec44424b03d65cf", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable2\Portable\Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf.sln", "Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf");

            List<string> list = new List<string>();
            list.Add("Contract.cs");
            bool passTransformation = CompleteTestBase(list, @"cc3d32746f60ed5a9f3775ef0ec44424b03d65cf");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Change parameter on method test
        /// </summary>
        [Test]
        public void Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("e7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable3\Portable\Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd.sln", "Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd");

            List<string> list = new List<string>();
            list.Add("TaskExtensions.cs");
            bool passTransformation = CompleteTestBase(list, @"e7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("2_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", "Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");

            List<string> list = new List<string>();
            list.Add("SymbolDisplay.cs");
            bool passTransformation = CompleteTestBase(list, @"2_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj3_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("3_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", "Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");

            List<string> list = new List<string>();
            list.Add("ObjectDisplay.cs");
            bool passTransformation = CompleteTestBase(list, @"3_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj4_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("4_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", "Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");

            List<string> list = new List<string>();
            list.Add("ObjectDisplay.cs");
            bool passTransformation = CompleteTestBase(list, @"4_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj5_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("5_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", "Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");

            List<string> list = new List<string>();
            list.Add("ObjectDisplay.cs");
            bool passTransformation = CompleteTestBase(list, @"5_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38");

            Assert.IsTrue(passLocation && passTransformation);
        }


        // [Test]
        // public void ReturnToGetTest()
        // {
        //     bool passLocation = LocationTest.LocaleTest(FilePath.RETURN_TO_GET_INPUT, FilePath.RETURN_TO_GET_OUTPUT_SELECTION, FilePath.MAIN_CLASS_RETURN_TO_GET_PATH);

        //     bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_RETURN_TO_GET_AFTER_EDITING,  FilePath.RETURN_TO_GET_EDITION, @"\return_to_get\");

        //     Assert.IsTrue(passLocation && passTransformation);
        //}

        [Test]
        public void Proj49cdaceb2828acc1f50223826d478a00a80a59e2()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("49cdaceb2828acc1f50223826d478a00a80a59e2", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\CSharp\Proj49cdaceb2828acc1f50223826d478a00a80a59e2.sln", "Proj49cdaceb2828acc1f50223826d478a00a80a59e2");

            List<string> list = new List<string>();
            list.Add("MockCSharpCompiler.cs"); list.Add("MockCsi.cs");
            bool passTransformation = CompleteTestBase(list, @"49cdaceb2828acc1f50223826d478a00a80a59e2");

            Assert.IsTrue(passLocation && passTransformation);
        }
        [Test]
        public void Projcfd9b464dbb07c8b183d89a403a8bc877b3e929d()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("cfd9b464dbb07c8b183d89a403a8bc877b3e929d", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable4\Portable\Projcfd9b464dbb07c8b183d89a403a8bc877b3e929d.sln", "Projcfd9b464dbb07c8b183d89a403a8bc877b3e929d");

            List<string> list = new List<string>();
            list.Add("MetadataWriter.cs"); 
            bool passTransformation = CompleteTestBase(list, @"cfd9b464dbb07c8b183d89a403a8bc877b3e929d");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_cfd9b464dbb07c8b183d89a403a8bc877b3e929d()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("2-cfd9b464dbb07c8b183d89a403a8bc877b3e929d", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable4\Portable\Projcfd9b464dbb07c8b183d89a403a8bc877b3e929d.sln", "Projcfd9b464dbb07c8b183d89a403a8bc877b3e929d");

            List<string> list = new List<string>();
            list.Add("MetadataWriter.cs");
            bool passTransformation = CompleteTestBase(list, @"2-cfd9b464dbb07c8b183d89a403a8bc877b3e929d");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("7c885ca20209ca95cfec1ed5bfaf1d43db06be99", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Diagnostics\Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99.sln", "Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99");

            List<string> list = new List<string>();
            list.Add("InternalImplementationOnlyAnalyzer.cs"); list.Add("DiagnosticAnalyzerAttributeAnalyzer.cs");
            bool passTransformation = CompleteTestBase(list, @"7c885ca20209ca95cfec1ed5bfaf1d43db06be99");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_7c885ca20209ca95cfec1ed5bfaf1d43db06be99()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("2_7c885ca20209ca95cfec1ed5bfaf1d43db06be99", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Diagnostics\Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99.sln", "Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99");

            List<string> list = new List<string>();
            list.Add("CA1001CSharpCodeFixProvider.cs"); list.Add("CA1008CSharpCodeFixProvider.cs");
            bool passTransformation = CompleteTestBase(list, @"2_7c885ca20209ca95cfec1ed5bfaf1d43db06be99");

            Assert.IsTrue(passLocation && passTransformation);
        }

        // [Test]
        // public void ASTManagerToParentTest()
        // {

        //     bool passLocation = LocationTest.LocaleTest(FilePath.ASTMANAGER_TO_PARENT_INPUT, FilePath.ASTMANAGER_TO_PARENT_OUTPUT_SELECTION, FilePath.MAIN_CLASS_ASTMANAGER_TO_PARENT_PATH);

        //     bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_ASTMANAGER_TO_PARENT_AFTER_EDITING, FilePath.ASTMANAGER_TO_PARENT_EDITED, @"\astmanager_to_parent\");

        //     Assert.IsTrue(passLocation && passTransformation);
        // }

        [Test]
        public void Proje28c81243206f1bb26b861ca0162678ce11b538cTest()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("e28c81243206f1bb26b861ca0162678ce11b538c", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Core\Proje28c81243206f1bb26b861ca0162678ce11b538c.sln", "Proje28c81243206f1bb26b861ca0162678ce11b538c");

            List<string> list = new List<string>();
            list.Add("CA1309CodeFixProviderBase.cs"); list.Add("CA2101CodeFixProviderBase.cs");
            bool passTransformation = CompleteTestBase(list, @"e28c81243206f1bb26b861ca0162678ce11b538c");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Complete test
        /// </summary>
        /// <param name="mainClassAfterEditing">Main class after edit</param>
        /// <param name="complement">Complement information</param>
        /// <returns></returns>
        public static bool CompleteTestBase(List<string> editeds, string commit)
        {
            EditorController controller = EditorController.GetInstance();
            controller.CurrentViewCodeAfter = FileUtil.ReadFile(@"commits\" + commit + @"\" + editeds.First());

            var dicionarySelection = JsonUtil<Dictionary<string, List<Selection>>>.Read(@"commits\" + commit + @"\edited_selections.json");

            //Tuple<string, string> beforeAfter = Tuple.Create(controller.CurrentViewCodeBefore, controller.CurrentViewCodeAfter);
            List<Tuple<string, string>> documents = new List<Tuple<string, string>>();
            foreach (var item in editeds)
            {
                string sourceCodeAfter = FileUtil.ReadFile(@"commits\" + commit + @"\" + item);
                string pattern = System.Text.RegularExpressions.Regex.Escape(item);
                foreach (var entry in dicionarySelection)
                {
                    string sourceCode = FileUtil.ReadFile(entry.Key);
                    bool containsPattern = System.Text.RegularExpressions.Regex.IsMatch(entry.Key, pattern);
                    if (containsPattern)
                    {
                        Tuple<string, string> tuple = Tuple.Create(sourceCode, sourceCodeAfter);
                        documents.Add(tuple);
                        controller.FilesOpened[entry.Key] = true;
                    }
                }
                
            }

            //documents.Add(beforeAfter);
            controller.DocumentsBeforeAndAfter = documents;
            controller.EditedLocations = dicionarySelection;
            
            controller.Refact();

            bool passTransformation = true;
            foreach (Transformation transformation in controller.SourceTransformations)
            { 
                string classPath = transformation.SourcePath;
                string className = classPath.Substring(classPath.LastIndexOf(@"\") + 1, classPath.Length - (classPath.LastIndexOf(@"\") + 1));
                //className = "files" + complement + className;
                //string source = FileUtil.ReadFile(classPath);
                //FileUtil.WriteToFile(@"commits\" + commit + @"\tool\" + className, transformation.transformation.Item2);

                Tuple<string, string> example = Tuple.Create(FileUtil.ReadFile(@"commits\" + commit + @"\tool\" + className), transformation.transformation.Item2);
                Tuple<ListNode, ListNode> lnode = ASTProgram.Example(example);

                NodeComparer comparator = new NodeComparer();
                bool isEqual = comparator.SequenceEqual(lnode.Item1, lnode.Item2);
                if (!isEqual)
                {
                    passTransformation = false;
                    break;
                }
            }
            controller.Undo();
            return passTransformation;
        }

        private static string GetClassPath()
        {
            return null;
        }
    }
}

