using System.Collections.Generic;
using System.Linq;
using ExampleRefactoring.Spg.ExampleRefactoring.Bean;
using ExampleRefactoring.Spg.ExampleRefactoring.Util;
using NUnit.Framework;
using Spg.ExampleRefactoring.Util;
using Spg.LocationCodeRefactoring.Controller;
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

        [Test]
        public void ChangeExceptionTest()
        {
            bool isValid = LocaleTest(FilePath.CHANGE_EXCEPTION_INPUT, FilePath.CHANGE_EXCEPTION_OUTPUT_SELECTION, FilePath.MAIN_CLASS_CHANGE_EXCEPTION_PATH);
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

            controller.Extract();
            controller.solutionPath = FilePath.SOLUTION_PATH;
            controller.RetrieveLocations(controller.CurrentViewCodeBefore);

            List<Selection> locations = JsonUtil<List<Selection>>.Read(output);
            bool passed = true;
            for (int i = 0; i < locations.Count; i++)
            {
                if (locations.Count != controller.locations.Count) { passed = false; break; }

                if (!locations[i].SourcePath.Equals(controller.locations[i].SourceClass)) { passed = false; break; }

                if (locations[i].Start != controller.locations[i].Region.Start || locations[i].Length != controller.locations[i].Region.Length)
                {
                    passed = false;
                    break;
                }
            }
            return passed;
        }
    }
}
