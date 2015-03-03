using System.Collections.Generic;
using System.Linq;
using ExampleRefactoring.Spg.ExampleRefactoring.Bean;
using ExampleRefactoring.Spg.ExampleRefactoring.Util;
using LocationCodeRefactoring.Spg.LocationCodeRefactoring.Controller;
using NUnit.Framework;
using Spg.ExampleRefactoring.Util;
using Spg.LocationRefactor.TextRegion;
using Spg.NUnitTests.Location;
using Spg.NUnitTests.Util;

namespace NUnitTests.Spg.NUnitTests.LocationTestProject
{
    [TestFixture]
    public class LocationTestProject
    {
        ///// <summary>
        ///// Simple API Test for locations
        ///// </summary>
        //[Test]
        //public void SimpleAPIChangeTest()
        //{
        //    bool isValid = LocaleTest(FilePath.SIMPLE_API_CHANGE_INPUT, FilePath.SIMPLE_API_CHANGE_OUTPUT_SELECTION, FilePath.MAIN_CLASS_SIMPLE_API_CHANGE_PATH);
        //    Assert.IsTrue(isValid);
        //}

        ///// <summary>
        ///// Test Introduce parameter on if
        ///// </summary>
        //[Test]
        //public void IntroduceParamOnIf()
        //{
        //    bool isValid = LocaleTest(FilePath.INTRODUCE_PARAM_ON_IF_INPUT, FilePath.INTRODUCE_PARAM_ON_IF_OUTPUT_SELECTION, FilePath.MAIN_CLASS_INTRODUCE_PARAM_ON_IF_PATH);
        //    Assert.IsTrue(isValid);
        //}

        ///// <summary>
        ///// Test Method Call To Identifier transformation
        ///// </summary>
        //[Test]
        //public void MethodCallToIdentifierTest()
        //{
        //    bool isValid = LocaleTest(FilePath.METHOD_CALL_TO_IDENTIFIER_INPUT, FilePath.METHOD_CALL_TO_IDENTIFIER_OUTPUT_SELECTION, FilePath.MAIN_CLASS_METHOD_CALL_TO_IDENTIFIER_PATH);
        //    Assert.IsTrue(isValid);
        //}

        ///// <summary>
        ///// Test case for parameter to constant value
        ///// </summary>
        //[Test]
        //public void ParameterToConstantValueTest()
        //{
        //    bool isValid = LocaleTest(FilePath.PARAMETER_TO_CONSTANT_VALUE_INPUT, FilePath.PARAMETER_TO_CONSTANT_VALUE_OUTPUT_SELECTION, FilePath.MAIN_CLASS_PARAMETER_TO_CONSTANT_VALUE_PATH);
        //    Assert.IsTrue(isValid);
        //}

        [Test]
        public void Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd()
        {
            bool isValid = LocationTestProject.LocaleTest("e7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable3\Portable\Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd.sln", "Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd");
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj1113fd3db14fd23fc081e90f27f4ddafad7b244d()
        {
            bool isValid = LocationTestProject.LocaleTest("1113fd3db14fd23fc081e90f27f4ddafad7b244d", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable\Proj1113fd3db14fd23fc081e90f27f4ddafad7b244d.sln", "Proj1113fd3db14fd23fc081e90f27f4ddafad7b244d");
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Change Exception test
        /// </summary>
        [Test]
        public void Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf()
        {
            bool isValid = LocationTestProject.LocaleTest("cc3d32746f60ed5a9f3775ef0ec44424b03d65cf", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable2\Portable\Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf.sln", "Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf");
            Assert.IsTrue(isValid);
        }

        //[Test]
        //public void ChangeParamOnMethodTest()
        //{
        //    bool isValid = LocaleTest(FilePath.CHANGE_PARAM_ON_METHOD_INPUT, FilePath.CHANGE_PARAM_ON_METHOD_OUTPUT_SELECTION, FilePath.MAIN_CLASS_CHANGE_PARAM_ON_METHOD_PATH);
        //    Assert.IsTrue(isValid);
        //}

        //[Test]
        //public void ReturnToGetTest()
        //{
        //    bool isValid = LocaleTest(FilePath.RETURN_TO_GET_INPUT, FilePath.RETURN_TO_GET_OUTPUT_SELECTION, FilePath.MAIN_CLASS_RETURN_TO_GET_PATH);
        //    Assert.IsTrue(isValid);
        //}

        [Test]
        public void Proj49cdaceb2828acc1f50223826d478a00a80a59e2()
        {
            bool isValid = LocationTestProject.LocaleTest("49cdaceb2828acc1f50223826d478a00a80a59e2", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\CSharp\Proj49cdaceb2828acc1f50223826d478a00a80a59e2.sln", "Proj49cdaceb2828acc1f50223826d478a00a80a59e2");
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Projcfd9b464dbb07c8b183d89a403a8bc877b3e929d()
        {
            bool isValid = LocationTestProject.LocaleTest("cfd9b464dbb07c8b183d89a403a8bc877b3e929d", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable4\Portable\Projcfd9b464dbb07c8b183d89a403a8bc877b3e929d.sln", "Projcfd9b464dbb07c8b183d89a403a8bc877b3e929d");
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_cfd9b464dbb07c8b183d89a403a8bc877b3e929d()
        {
            bool isValid = LocationTestProject.LocaleTest("2-cfd9b464dbb07c8b183d89a403a8bc877b3e929d", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable4\Portable\Projcfd9b464dbb07c8b183d89a403a8bc877b3e929d.sln", "Projcfd9b464dbb07c8b183d89a403a8bc877b3e929d");
            Assert.IsTrue(isValid);
        }

        //[Test]
        //public void ChangeAnnotationOnClassTest()
        //{
        //    bool isValid = LocaleTest(FilePath.CHANGE_ANNOTATION_ON_CLASS_INPUT, FilePath.CHANGE_ANNOTATION_ON_CLASS_OUTPUT_SELECTION, FilePath.MAIN_CLASS_CHANGE_ANNOTATION_ON_CLASS_PATH);
        //    Assert.IsTrue(isValid);
        //}

        ///// <summary>
        ///// ASTManager to parent test case
        ///// </summary>
        //[Test]
        //public void ASTManagerToParentTest()
        //{
        //    bool isValid = LocationTest.LocaleTest(FilePath.ASTMANAGER_TO_PARENT_INPUT, FilePath.ASTMANAGER_TO_PARENT_OUTPUT_SELECTION, FilePath.MAIN_CLASS_ASTMANAGER_TO_PARENT_PATH);
        //    Assert.IsTrue(isValid);
        //}

        [Test]
        public void Proje28c81243206f1bb26b861ca0162678ce11b538cTest()
        {
            bool isValid = LocationTestProject.LocaleTest("e28c81243206f1bb26b861ca0162678ce11b538c", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Core\Proje28c81243206f1bb26b861ca0162678ce11b538c.sln", "Proje28c81243206f1bb26b861ca0162678ce11b538c");
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Locale test base method
        /// </summary>
        /// <param name="commit">commit id</param>
        /// <param name="mainClass">Main class</param>
        /// <returns>True if locale passed</returns>
        public static bool LocaleTest(string commit, string solution, string project)
        {
            EditorController controller = EditorController.GetInstance();
            controller.Init();
            List<TRegion> selections = JsonUtil<List<TRegion>>.Read(@"commits\"+ commit + @"\input_selection.json");
            controller.SelectedLocations = selections;
            controller.CurrentViewCodeBefore = FileUtil.ReadFile(selections.First().Path);
            controller.CurrentViewCodePath = selections.First().Path;
            controller.CurrentProject = project;
            controller.SelectedLocations = selections;

            controller.Extract();
            controller.SolutionPath = solution;
            controller.RetrieveLocations(controller.CurrentViewCodeBefore);

            List<Selection> locations = JsonUtil<List<Selection>>.Read(@"commits\" + commit + @"\found_locations.json");
            bool passed = true;
            for (int i = 0; i < locations.Count; i++)
            {
                if (locations.Count != controller.Locations.Count) { passed = false; break; }

                if (!locations[i].SourcePath.Equals(controller.Locations[i].SourceClass)) { passed = false; break; }

                if (locations[i].Start != controller.Locations[i].Region.Start || locations[i].Length != controller.Locations[i].Region.Length)
                {
                    passed = false;
                    break;
                }
            }
            return passed;
        }
    }
}
