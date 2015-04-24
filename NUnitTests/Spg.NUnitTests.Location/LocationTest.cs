using System.Collections.Generic;
using System.Linq;
using ExampleRefactoring.Spg.ExampleRefactoring.Bean;
using ExampleRefactoring.Spg.ExampleRefactoring.Util;
using Spg.LocationCodeRefactoring.Controller;
using NUnit.Framework;
using Spg.ExampleRefactoring.Util;
using Spg.LocationRefactor.TextRegion;
using Spg.NUnitTests.Util;

namespace Spg.NUnitTests.Location
{
    [TestFixture]
    public class LocationTest
    {
        /// <summary>
        /// Simple API Test for locations
        /// </summary>
        [Test]
        public void SimpleAPIChangeTest()
        {
            bool isValid = LocaleTest(FilePath.SIMPLE_API_CHANGE_INPUT, FilePath.SIMPLE_API_CHANGE_OUTPUT_SELECTION, FilePath.MAIN_CLASS_SIMPLE_API_CHANGE_PATH);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Introduce parameter on if
        /// </summary>
        [Test]
        public void IntroduceParamOnIf()
        {
            bool isValid = LocaleTest(FilePath.INTRODUCE_PARAM_ON_IF_INPUT, FilePath.INTRODUCE_PARAM_ON_IF_OUTPUT_SELECTION, FilePath.MAIN_CLASS_INTRODUCE_PARAM_ON_IF_PATH);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void MethodCallToIdentifierTest()
        {
            bool isValid = LocaleTest(FilePath.METHOD_CALL_TO_IDENTIFIER_INPUT, FilePath.METHOD_CALL_TO_IDENTIFIER_OUTPUT_SELECTION, FilePath.MAIN_CLASS_METHOD_CALL_TO_IDENTIFIER_PATH);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test case for parameter to constant value
        /// </summary>
        [Test]
        public void ParameterToConstantValueTest()
        {
            bool isValid = LocaleTest(FilePath.PARAMETER_TO_CONSTANT_VALUE_INPUT, FilePath.PARAMETER_TO_CONSTANT_VALUE_OUTPUT_SELECTION, FilePath.MAIN_CLASS_PARAMETER_TO_CONSTANT_VALUE_PATH);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void ParameterChangeOnMethodTest()
        {
            bool isValid = LocaleTest(FilePath.PARAMETER_CHANGE_ON_METHOD_INPUT, FilePath.PARAMETER_CHANGE_ON_METHOD_OUTPUT_SELECTION, FilePath.MAIN_CLASS_PARAMETER_CHANGE_ON_METHOD_PATH);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void MethodToPropertyIfTest()
        {
            bool isValid = LocaleTest(FilePath.METHOD_TO_PROPERTY_IF_INPUT, FilePath.METHOD_TO_PROPERTY_IF_OUTPUT_SELECTION, FilePath.MAIN_CLASS_METHOD_TO_PROPERTY_IF_PATH);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Change Exception test
        /// </summary>
        [Test]
        public void ChangeExceptionTest()
        {
            bool isValid = LocaleTest(FilePath.CHANGE_EXCEPTION_INPUT, FilePath.CHANGE_EXCEPTION_OUTPUT_SELECTION, FilePath.MAIN_CLASS_CHANGE_EXCEPTION_PATH);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void ChangeParamOnMethodTest()
        {
            bool isValid = LocaleTest(FilePath.CHANGE_PARAM_ON_METHOD_INPUT, FilePath.CHANGE_PARAM_ON_METHOD_OUTPUT_SELECTION, FilePath.MAIN_CLASS_CHANGE_PARAM_ON_METHOD_PATH);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void ReturnToGetTest()
        {
            bool isValid = LocaleTest(FilePath.RETURN_TO_GET_INPUT, FilePath.RETURN_TO_GET_OUTPUT_SELECTION, FilePath.MAIN_CLASS_RETURN_TO_GET_PATH);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void AddIOParamTest()
        {
            bool isValid = LocaleTest(FilePath.ADD_IO_PARAM_INPUT, FilePath.ADD_IO_PARAM_OUTPUT_SELECTION, FilePath.MAIN_CLASS_ADD_IO_PARAM_PATH);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void AddContextParamTest()
        {
            bool isValid = LocaleTest(FilePath.ADD_CONTEXR_PARAM_INPUT, FilePath.ADD_CONTEXR_PARAM_OUTPUT_SELECTION, FilePath.MAIN_CLASS_ADD_CONTEXT_PARAM_PATH);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void ChangeAnnotationOnClassTest()
        {
            bool isValid = LocaleTest(FilePath.CHANGE_ANNOTATION_ON_CLASS_INPUT, FilePath.CHANGE_ANNOTATION_ON_CLASS_OUTPUT_SELECTION, FilePath.MAIN_CLASS_CHANGE_ANNOTATION_ON_CLASS_PATH);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// ASTManager to parent test case
        /// </summary>
        [Test]
        public void ASTManagerToParentTest()
        {
            bool isValid = LocationTest.LocaleTest(FilePath.ASTMANAGER_TO_PARENT_INPUT, FilePath.ASTMANAGER_TO_PARENT_OUTPUT_SELECTION, FilePath.MAIN_CLASS_ASTMANAGER_TO_PARENT_PATH);
            Assert.IsTrue(isValid);
        }
        /// <summary>
        /// Locale test base method
        /// </summary>
        /// <param name="input">Input file</param>
        /// <param name="output">Output file</param>
        /// <param name="mainClass">Main class</param>
        /// <returns>True if locale passed</returns>
        public static bool LocaleTest(string input, string output, string mainClass)
        {
            EditorController controller = EditorController.GetInstance();
            controller.Init();
            List<TRegion> selections = JsonUtil<List<TRegion>>.Read(input);
            controller.SelectedLocations = selections;
            controller.CurrentViewCodeBefore = FileUtil.ReadFile(mainClass);
            controller.CurrentViewCodePath = mainClass;

            controller.SetSolution(FilePath.SOLUTION_PATH);
            controller.Extract();
            
            controller.RetrieveLocations(controller.CurrentViewCodeBefore);

            List<Selection> locations = JsonUtil<List<Selection>>.Read(output);
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
