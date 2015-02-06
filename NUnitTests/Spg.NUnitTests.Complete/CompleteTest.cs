using System;
using System.Collections.Generic;
using System.IO;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using LocationCodeRefactoring.Spg.LocationCodeRefactoring.Controller;
using LocationCodeRefactoring.Spg.LocationRefactor.Transformation;
using NUnit.Framework;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Util;
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
        public void IntroduceParamOnIf()
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

        /// <summary>
        /// Change exception test
        /// </summary>
        [Test]
        public void ChangeExceptionTest()
        {
            bool passLocation = LocationTest.LocaleTest(FilePath.CHANGE_EXCEPTION_INPUT, FilePath.CHANGE_EXCEPTION_OUTPUT_SELECTION, FilePath.MAIN_CLASS_CHANGE_EXCEPTION_PATH);

            bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_CHANGE_EXCEPTION_AFTER_EDITING, @"files\change_exception\");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Change parameter on method test
        /// </summary>
        [Test]
        public void ChangeParamOnMethodTest()
        {
            bool passLocation = LocationTest.LocaleTest(FilePath.CHANGE_PARAM_ON_METHOD_INPUT, FilePath.CHANGE_PARAM_ON_METHOD_OUTPUT_SELECTION, FilePath.MAIN_CLASS_CHANGE_PARAM_ON_METHOD_PATH);

            bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_CHANGE_PARAM_ON_METHOD_EDITING, @"files\change_param_on_method\");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void ParameterChangeOnMethodTest()
        {
            bool passLocation = LocationTest.LocaleTest(FilePath.PARAMETER_CHANGE_ON_METHOD_INPUT, FilePath.PARAMETER_CHANGE_ON_METHOD_OUTPUT_SELECTION, FilePath.MAIN_CLASS_PARAMETER_CHANGE_ON_METHOD_PATH);

            bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_PARAMETER_CHANGE_ON_METHOD_AFTER_EDITING, @"files\parameter_change_on_method\");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void ReturnToGetTest()
        {
            bool passLocation = LocationTest.LocaleTest(FilePath.RETURN_TO_GET_INPUT, FilePath.RETURN_TO_GET_OUTPUT_SELECTION, FilePath.MAIN_CLASS_RETURN_TO_GET_PATH);

            bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_RETURN_TO_GET_AFTER_EDITING, @"files\return_to_get\");

            Assert.IsTrue(passLocation && passTransformation);
       }

        [Test]
        public void AddIOParamTest()
        {
            bool passLocation = LocationTest.LocaleTest(FilePath.ADD_IO_PARAM_INPUT, FilePath.ADD_IO_PARAM_OUTPUT_SELECTION, FilePath.MAIN_CLASS_ADD_IO_PARAM_PATH);

            bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_ADD_IO_PARAM_AFTER_EDITING, @"files\add_io_param\");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void AddContextParamTest()
        {
            bool passLocation = LocationTest.LocaleTest(FilePath.ADD_CONTEXR_PARAM_INPUT, FilePath.ADD_CONTEXR_PARAM_OUTPUT_SELECTION, FilePath.MAIN_CLASS_ADD_CONTEXT_PARAM_PATH);

            bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_ADD_CONTEXT_PARAM_AFTER_EDITING, @"files\add_context_param\");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void ChangeAnnotationOnClassTest()
        {

            bool passLocation = LocationTest.LocaleTest(FilePath.CHANGE_ANNOTATION_ON_CLASS_INPUT, FilePath.CHANGE_ANNOTATION_ON_CLASS_OUTPUT_SELECTION, FilePath.MAIN_CLASS_CHANGE_ANNOTATION_ON_CLASS_PATH);

            bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_CHANGE_ANNOTATION_ON_CLASS_AFTER_EDITING, @"files\change_annotation_on_class\");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void ASTManagerToParentTest()
        {

            bool passLocation = LocationTest.LocaleTest(FilePath.ASTMANAGER_TO_PARENT_INPUT, FilePath.ASTMANAGER_TO_PARENT_OUTPUT_SELECTION, FilePath.MAIN_CLASS_ASTMANAGER_TO_PARENT_PATH);

            bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_ASTMANAGER_TO_PARENT_AFTER_EDITING, @"files\astmanager_to_parent\");

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

            Tuple<string, string> beforeAfter = Tuple.Create(controller.CurrentViewCodeBefore, controller.CurrentViewCodeAfter);
            List<Tuple<string, string>> documents = new List<Tuple<string, string>>();
            documents.Add(beforeAfter);
            controller.DocumentsBeforeAndAfter = documents;
            
  
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
