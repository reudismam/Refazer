using System;
using LocationCodeRefactoring.Spg.LocationRefactor.Transformation;
using NUnit.Framework;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Util;
using Spg.LocationCodeRefactoring.Controller;
using Spg.NUnitTests.Location;
using Spg.NUnitTests.Util;

namespace NUnitTests.Spg.NUnitTests.Complete
{
    /// <summary>
    /// Test for complete systematic editing
    /// </summary>
    [TestFixture]
    public class CompleteTest
    {
        /// <summary>
        /// Simple API Test for locations
        /// </summary>
        [Test]
        public void SimpleAPIChangeTest()
        {
            bool passLocation = LocationTest.LocaleTest(FilePath.SIMPLE_API_CHANGE_INPUT, FilePath.SIMPLE_API_CHANGE_OUTPUT_SELECTION, FilePath.MAIN_CLASS_SIMPLE_API_CHANGE_PATH);

            bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_SIMPLE_API_CHANGE_AFTER_EDITING, @"files\change_api\");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Parameter test on if
        /// </summary>
        [Test]
        public void ParameterChangeOnIfTest()
        {
            bool passLocation = LocationTest.LocaleTest(FilePath.INTRODUCE_PARAM_ON_IF_INPUT, FilePath.INTRODUCE_PARAM_ON_IF_OUTPUT_SELECTION, FilePath.MAIN_CLASS_INTRODUCE_PARAM_ON_IF_PATH);

            bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_INTRODUCE_PARAM_ON_IF_AFTER_EDITING, @"files\parameter_change_on_if\");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void MethodCallToIdentifierTest()
        {
            bool passLocation = LocationTest.LocaleTest(FilePath.METHOD_CALL_TO_IDENTIFIER_INPUT, FilePath.METHOD_CALL_TO_IDENTIFIER_OUTPUT_SELECTION, FilePath.MAIN_CLASS_METHOD_CALL_TO_IDENTIFIER_PATH);

            bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_METHOD_CALL_TO_IDENTIFIER_PATH_AFTER_EDITING, @"files\method_call_to_identifier\");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void ParameterToConstantValueTest()
        {
            bool passLocation = LocationTest.LocaleTest(FilePath.PARAMETER_TO_CONSTANT_VALUE_INPUT, FilePath.PARAMETER_TO_CONSTANT_VALUE_OUTPUT_SELECTION, FilePath.MAIN_CLASS_PARAMETER_TO_CONSTANT_VALUE_PATH);

            bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_PARAMETER_TO_CONSTANT_VALUE_AFTER_EDITING, @"files\parameter_to_constant_value\");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void MethodToPropertyIfTest()
        {
            bool passLocation = LocationTest.LocaleTest(FilePath.METHOD_TO_PROPERTY_IF_INPUT, FilePath.METHOD_TO_PROPERTY_IF_OUTPUT_SELECTION, FilePath.MAIN_CLASS_METHOD_TO_PROPERTY_IF_PATH);

            bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_METHOD_TO_PROPERTY_IF_AFTER_EDITING, @"files\method_to_property_if\");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void ChangeExceptionTest()
        {
            bool passLocation = LocationTest.LocaleTest(FilePath.CHANGE_EXCEPTION_INPUT, FilePath.CHANGE_EXCEPTION_OUTPUT_SELECTION, FilePath.MAIN_CLASS_CHANGE_EXCEPTION_PATH);

            bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_CHANGE_EXCEPTION_AFTER_EDITING, @"files\change_exception\");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Complete test
        /// </summary>
        /// <param name="mainClassAfterEditing">Main class after edit</param>
        /// <param name="complement">Complement information</param>
        /// <returns></returns>
        public static bool CompleteTestBase(string mainClassAfterEditing, string complement)
        {
            EditorController controller = EditorController.GetInstance();
            controller.CurrentViewCodeAfter = FileUtil.ReadFile(complement + mainClassAfterEditing);
            controller.Refact();

            bool passTransformation = true;
            foreach (Transformation transformation in controller.SourceTransformations)
            {
                string classPath = transformation.SourcePath;
                string className = classPath.Substring(classPath.LastIndexOf(@"\") + 1, classPath.Length - (classPath.LastIndexOf(@"\") + 1));
                className = complement + className;

                Tuple<string, string> example = Tuple.Create(FileUtil.ReadFile(className), transformation.transformation.Item2);
                Tuple<ListNode, ListNode> lnode = ASTProgram.Example(example);

                NodeComparer comparator = new NodeComparer();
                bool isEqual = comparator.SequenceEqual(lnode.Item1, lnode.Item2);
                if (!isEqual)
                {
                    passTransformation = false;
                    break;
                }
            }
            return passTransformation;
        }
    }
}
