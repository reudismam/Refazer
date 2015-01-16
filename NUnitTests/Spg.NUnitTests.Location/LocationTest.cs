using System.Collections.Generic;
using System.Drawing;
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
            EditorController controller = EditorController.GetInstance();
            List<TRegion> selections = JsonUtil<List<TRegion>>.Read(FilePath.SIMPLE_API_CHANGE_INPUT);
            controller.RegionsBeforeEdition = selections;
            controller.CodeBefore = FileUtil.ReadFile(FilePath.MAIN_CLASS_PATH);

            controller.Extract();
            controller.solutionPath = FilePath.SOLUTION_PATH;
            controller.RetrieveRegions(controller.Progs.First().Key, controller.CodeBefore);

            List<Selection> locations = JsonUtil<List<Selection>>.Read(FilePath.SIMPLE_API_CHANGE_OUTPUT_SELECTION);
            bool passed = true;
            for (int i = 0; i < locations.Count; i++)
            {
                if (locations.Count != controller.locations.Count) { passed = false; break;  }

                if (!locations[i].SourcePath.Equals(controller.locations[i].SourceClass)){ passed = false; break; }

                if (locations[i].Start != controller.locations[i].Region.Start || locations[i].Length != controller.locations[i].Region.Length)
                {
                    passed = false;
                    break;
                }
            }
            Assert.IsTrue(passed);
        }
    }
}
