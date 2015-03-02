using System;
using System.Collections.Generic;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using NUnit.Framework;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Data;
using Spg.ExampleRefactoring.Data.Dig;
using Spg.ExampleRefactoring.Synthesis;

namespace Spg.NUnitTests.Refactoring
{
    [TestFixture]
    public class RefactoringTests
    {
        [Test]
        public void DeleteConsoleTest()
        {
            ExampleCommand command = new DeletePrint();
            Boolean isEqual = Core(command);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void AddParameter()
        {
            ExampleCommand command = new AddParameter();
            Boolean isEqual = Core(command);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void ChangeAPISimple()
        {
            ExampleCommand command = new ChangeAPISimple();
            Boolean isEqual = Core(command);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void ChangeAPI()
        {
            ExampleCommand command = new ChangeAPI();
            Boolean isEqual = Core(command);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void ChangeConstantToValue()
        {
            ExampleCommand command = new ChangeConstantToValue();
            Boolean isEqual = Core(command);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void ParameterChangeOnIfs()
        {
            ExampleCommand command = new ParameterChangeOnIfs();
            Boolean isEqual = Core(command);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void MethodCallToIdentifier()
        {
            ExampleCommand command = new ParameterChangeOnIfs();
            Boolean isEqual = Core(command);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void ParameterChangeOnMethod()
        {
            ExampleCommand command = new ParameterChangeOnMethod();
            Boolean isEqual = Core(command);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void ChangeStringValueToConstant()
        {
            ExampleCommand command = new ChangeStringValueToConstant();
            Boolean isEqual = Core(command);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void AddAnnotation()
        {
            ExampleCommand command = new AddAnnotation();
            Boolean isEqual = Core(command);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void IntroduceIf()
        {
            ExampleCommand command = new IntroduceIf();
            Boolean isEqual = Core(command);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void ConvertElementToCollection()
        {
            ExampleCommand command = new ConvertElementToCollection();
            Boolean isEqual = Core(command);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void AddLoopCollector()
        {
            ExampleCommand command = new AddLoopCollector();
            Boolean isEqual = Core(command);
            Assert.IsTrue(isEqual);
        }


        [Test]
        public void WrapLoopWithTimer()
        {
            ExampleCommand command = new WrapLoopWithTimer();
            Boolean isEqual = Core(command);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void CopyFieldInitializer()
        {
            ExampleCommand command = new CopyFieldInitializer();
            Boolean isEqual = Core(command);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void CreateAndInitializeNewField()
        {
            ExampleCommand command = new CreateAndInitializeNewField();
            Boolean isEqual = Core(command);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void MoveInterfaceImplementationToInnerClass()
        {
            ExampleCommand command = new MoveInterfaceImplementationToInnerClass();
            Boolean isEqual = Core(command);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void ChangeAndPropagateFieldType()
        {
            ExampleCommand command = new ChangeAndPropagateFieldType();
            Boolean isEqual = Core(command);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void ChangeAndPropagateFieldTypeParameter()
        {
            ExampleCommand command = new ChangeAndPropagateFieldTypeParameter();
            Boolean isEqual = Core(command);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void ChangeClassVisibility()
        {
            ExampleCommand command = new ChangeClassVisibility();
            Boolean isEqual = Core(command);
            Assert.IsTrue(isEqual);
        }

        private Boolean Core(ExampleCommand command)
        {
            List<Tuple<String, String>> examples = command.Train();
            List<Tuple<ListNode, ListNode>> data = ASTProgram.Examples(examples);

            //List<int> boundaryPoints = SynthesisManager.CreateBoundaryPoints(examples[0].Item1, examples[0].Item2, data[0]);
            ASTProgram program = new ASTProgram();
            //program.boundary = boundaryPoints;

            Tuple<String, String> test = command.Test();
            Tuple<String, String> outputtest = Tuple.Create(test.Item1, test.Item2);
            Tuple<ListNode, ListNode> output = ASTProgram.Example(outputtest);

            List<SynthesizedProgram> synthesizedProgram = program.GenerateStringProgram(data);
            ASTTransformation result = ASTProgram.TransformString(test.Item1, synthesizedProgram[0]);
            Tuple<String, String> transformationTest = Tuple.Create(result.transformation, result.transformation);
            Tuple<ListNode, ListNode> transformation = ASTProgram.Example(transformationTest);

            NodeComparer comparator = new NodeComparer();
            Boolean isEqual = comparator.SequenceEqual(transformation.Item1, output.Item2);

            return isEqual;
        }
    }
}
