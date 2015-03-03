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

        // /// <summary>
        // /// Test Method Call To Identifier transformation
        // /// </summary>
        // [Test]
        // public void MethodCallToIdentifierTest()
        // {
        //     bool passLocation = LocationTest.LocaleTest(FilePath.METHOD_CALL_TO_IDENTIFIER_INPUT, FilePath.METHOD_CALL_TO_IDENTIFIER_OUTPUT_SELECTION, FilePath.MAIN_CLASS_METHOD_CALL_TO_IDENTIFIER_PATH);

        //     bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_METHOD_CALL_TO_IDENTIFIER_PATH_AFTER_EDITING, FilePath.METHOD_CALL_TO_IDENTIFIER_EDITION, @"\method_call_to_identifier\");

        //     Assert.IsTrue(passLocation && passTransformation);
        // }

        // /// <summary>
        // /// Test Method Call To Identifier transformation
        // /// </summary>
        // [Test]
        // public void ParameterToConstantValueTest()
        // {
        //     bool passLocation = LocationTest.LocaleTest(FilePath.PARAMETER_TO_CONSTANT_VALUE_INPUT, FilePath.PARAMETER_TO_CONSTANT_VALUE_OUTPUT_SELECTION, FilePath.MAIN_CLASS_PARAMETER_TO_CONSTANT_VALUE_PATH);

        //     bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_PARAMETER_TO_CONSTANT_VALUE_AFTER_EDITING, FilePath.PARAMETER_TO_CONSTANT_VALUE_EDITION, @"\parameter_to_constant_value\");

        //     Assert.IsTrue(passLocation && passTransformation);
        // }

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

        // [Test]
        // public void ParameterChangeOnMethodTest()
        // {
        //     bool passLocation = LocationTest.LocaleTest(FilePath.PARAMETER_CHANGE_ON_METHOD_INPUT, FilePath.PARAMETER_CHANGE_ON_METHOD_OUTPUT_SELECTION, FilePath.MAIN_CLASS_PARAMETER_CHANGE_ON_METHOD_PATH);

        //     bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_PARAMETER_CHANGE_ON_METHOD_AFTER_EDITING, FilePath.PARAMETER_CHANGE_ON_METHOD_EDITION, @"\parameter_change_on_method\");

        //     Assert.IsTrue(passLocation && passTransformation);
        // }

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

        // [Test]
        // public void ChangeAnnotationOnClassTest()
        // {

        //     bool passLocation = LocationTest.LocaleTest(FilePath.CHANGE_ANNOTATION_ON_CLASS_INPUT, FilePath.CHANGE_ANNOTATION_ON_CLASS_OUTPUT_SELECTION, FilePath.MAIN_CLASS_CHANGE_ANNOTATION_ON_CLASS_PATH);

        //     bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_CHANGE_ANNOTATION_ON_CLASS_AFTER_EDITING, FilePath.CHANGE_ANNOTATION_ON_CLASS_EDITED, @"\change_annotation_on_class\");

        //     Assert.IsTrue(passLocation && passTransformation);
        // }

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
                //FileUtil.WriteToFile(@"commits\" + commit + @"\" + className, transformation.transformation.Item2);

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

