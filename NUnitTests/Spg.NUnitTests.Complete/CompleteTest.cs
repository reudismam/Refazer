using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ExampleRefactoring.Spg.ExampleRefactoring.Bean;
using ExampleRefactoring.Spg.ExampleRefactoring.Util;
using LocationCodeRefactoring.Spg.LocationRefactor.Transformation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Util;
using Spg.LocationCodeRefactoring.Controller;
using Spg.LocationRefactor.Location;
using Spg.LocationRefactor.TextRegion;
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
            EditorController controller = EditorController.GetInstance();
            List<TRegion> selections = JsonUtil<List<TRegion>>.Read(FilePath.SIMPLE_API_CHANGE_INPUT);
            controller.RegionsBeforeEdit[Color.LightGreen] = selections;
            controller.CodeBefore = FileUtil.ReadFile(FilePath.MAIN_CLASS_PATH);

            controller.Extract();
            controller.solutionPath = FilePath.SOLUTION_PATH;
            controller.RetrieveRegions(controller.Progs.First().Key, controller.CodeBefore);

            List<Selection> locations = JsonUtil<List<Selection>>.Read(FilePath.SIMPLE_API_CHANGE_OUTPUT_SELECTION);
            bool passLocation = true;
            for (int i = 0; i < locations.Count; i++)
            {
                if (locations.Count != controller.locations.Count) { passLocation = false; break; }

                if (!locations[i].SourcePath.Equals(controller.locations[i].SourceClass)) { passLocation = false; break; }

                if (locations[i].Start != controller.locations[i].Region.Start || locations[i].Length != controller.locations[i].Region.Length)
                {
                    passLocation = false;
                    break;
                }
            }

            controller.CodeAfter = FileUtil.ReadFile(FilePath.MAIN_CLASS_AFTER_EDITING);
            controller.Refact();

            bool passTransformation = true;
            foreach (Transformation transformation in controller.SourceTransformations)
            {
                string classPath = transformation.SourcePath;
                string className = classPath.Substring(classPath.LastIndexOf(@"\") + 1, classPath.Length - (classPath.LastIndexOf(@"\") + 1));

                Tuple<string, string> example = Tuple.Create(FileUtil.ReadFile(className), transformation.transformation.Item2);
                Tuple<ListNode, ListNode> lnode = ASTProgram.Example(example);

                NodeComparer comparator = new NodeComparer();
                Boolean isEqual = comparator.SequenceEqual(lnode.Item1, lnode.Item2);
                if (!isEqual)
                {
                    passTransformation = false;
                    break;
                }

            }

            Assert.IsTrue(passLocation || passTransformation);
        }
    }
}
