using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExampleRefactoring.Spg.ExampleRefactoring.Bean;
using ExampleRefactoring.Spg.ExampleRefactoring.Util;
using LocationCodeRefactoring.Spg.LocationCodeRefactoring.Controller;
using Microsoft.SqlServer.Server;
using NUnit.Framework;
using Spg.ExampleRefactoring.Data.Dig;
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

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj00552fc2287f820ae9d42fd259aa6c07c2c5a805()
        {
            bool isValid = LocationTestProject.LocaleTest("00552fc2287f820ae9d42fd259aa6c07c2c5a805", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable6\Proj00552fc2287f820ae9d42fd259aa6c07c2c5a805.sln", "Proj00552fc2287f820ae9d42fd259aa6c07c2c5a805");
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj008682140dfded0956241e81214a860a978b2395()
        {
            bool isValid = LocationTestProject.LocaleTest("00552fc2287f820ae9d42fd259aa6c07c2c5a805", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable13\Proj008682140dfded0956241e81214a860a978b2395.sln", "Proj008682140dfded0956241e81214a860a978b2395");
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj8c146441b4ecedbf7648e890d33f946f9b206e01()
        {
            bool isValid = LocationTestProject.LocaleTest("8c146441b4ecedbf7648e890d33f946f9b206e01", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable12\Proj8c146441b4ecedbf7648e890d33f946f9b206e01.sln", "Proj8c146441b4ecedbf7648e890d33f946f9b206e01");
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            bool isValid = LocationTestProject.LocaleTest("673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable11\Proj673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", "Proj673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            bool isValid = LocationTestProject.LocaleTest("2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\CodeAnalysisTest\Proj2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", "Proj2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj3_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            bool isValid = LocationTestProject.LocaleTest("3_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\CodeAnalysisTest\Proj2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", "Proj2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj4_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            bool isValid = LocationTestProject.LocaleTest("4_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\CommandLine\Proj4_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", "Proj4_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj5_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            bool isValid = LocationTestProject.LocaleTest("5_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Syntax\Proj5_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", "Proj5_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj6_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            bool isValid = LocationTestProject.LocaleTest("6_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Symbol\Proj6_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", "Proj6_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj8_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            bool isValid = LocationTestProject.LocaleTest("8_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Symbol\Proj6_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", "Proj6_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            Assert.IsTrue(isValid);
        }

        // <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj9_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            bool isValid = LocationTestProject.LocaleTest("9_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Emit\Proj9_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", "Proj9_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj83e4349133d27a8f4dd5a85b69eb4ba00d41e6f8()
        {
            bool isValid = LocationTestProject.LocaleTest("83e4349133d27a8f4dd5a85b69eb4ba00d41e6f8", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable10\Proj83e4349133d27a8f4dd5a85b69eb4ba00d41e6f8.sln", "Proj83e4349133d27a8f4dd5a85b69eb4ba00d41e6f8");
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proje817dab72dd5199cb5c7f661bc6b289f63ae706b()
        {
            bool isValid = LocationTestProject.LocaleTest("e817dab72dd5199cb5c7f661bc6b289f63ae706b", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable9\Proje817dab72dd5199cb5c7f661bc6b289f63ae706b.sln", "Proje817dab72dd5199cb5c7f661bc6b289f63ae706b");
            Assert.IsTrue(isValid);
        }



        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj2_cd68d0323eb97f18c10281847c831f8e361506b9()
        {
            bool isValid = LocationTestProject.LocaleTest("2_cd68d0323eb97f18c10281847c831f8e361506b9", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable8\Projcd68d0323eb97f18c10281847c831f8e361506b9.sln", "Projcd68d0323eb97f18c10281847c831f8e361506b9");
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj3_cd68d0323eb97f18c10281847c831f8e361506b9()
        {
            bool isValid = LocationTestProject.LocaleTest("3_cd68d0323eb97f18c10281847c831f8e361506b9", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable8\Projcd68d0323eb97f18c10281847c831f8e361506b9.sln", "Projcd68d0323eb97f18c10281847c831f8e361506b9");
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Projf66696e70c90ce7fa1476d53cc84cc18e438d19b()
        {
            bool isValid = LocationTestProject.LocaleTest("f66696e70c90ce7fa1476d53cc84cc18e438d19b", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\VBCSCompilerTests\Proje4d141a3c5f51ce4021d105e2b330564e02069fc.sln", "Proje4d141a3c5f51ce4021d105e2b330564e02069fc");
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj4b402939708adf35a7a5e12ffc99dc14cc1f4766()
        {
            bool isValid = LocationTestProject.LocaleTest("2_4b402939708adf35a7a5e12ffc99dc14cc1f4766", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\CSharp2\Proj2_4b402939708adf35a7a5e12ffc99dc14cc1f4766.sln", "Proj2_4b402939708adf35a7a5e12ffc99dc14cc1f4766");
            Assert.IsTrue(isValid);
        }


        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj8ecd05880b478e4ca997a4789b976ef73b070546()
        {
            bool isValid = LocationTestProject.LocaleTest("8ecd05880b478e4ca997a4789b976ef73b070546", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable7\Proj4b402939708adf35a7a5e12ffc99dc14cc1f4766.sln", "Proj4b402939708adf35a7a5e12ffc99dc14cc1f4766");
            Assert.IsTrue(isValid);
        }
        


        [Test]
        public void Proj04d060498bc0c30403bb05872e396052d826d082()
        {
            bool isValid = LocationTestProject.LocaleTest("04d060498bc0c30403bb05872e396052d826d082", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Diagnostics2\Proj04d060498bc0c30403bb05872e396052d826d082.sln", "Proj04d060498bc0c30403bb05872e396052d826d082");
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj318b2b0e476a122ebc033b13d41449ef1c814c1d()
        {
            bool isValid = LocationTestProject.LocaleTest("318b2b0e476a122ebc033b13d41449ef1c814c1d", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Core2\Proj318b2b0e476a122ebc033b13d41449ef1c814c1d.sln", "Proj318b2b0e476a122ebc033b13d41449ef1c814c1d");
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test case for parameter to constant value
        /// </summary>
        [Test]
        public void Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            bool isValid = LocationTestProject.LocaleTest("c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", "Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");
            Assert.IsTrue(isValid);
        }

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

        [Test]
        public void Proj2_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            bool isValid = LocationTestProject.LocaleTest("2_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", "Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj3_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            bool isValid = LocationTestProject.LocaleTest("3_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", "Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj4_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            bool isValid = LocationTestProject.LocaleTest("4_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", "Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj5_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            bool isValid = LocationTestProject.LocaleTest("5_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", "Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");
            Assert.IsTrue(isValid);
        }



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

        [Test]
        public void Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99()
        {
            bool isValid = LocationTestProject.LocaleTest("7c885ca20209ca95cfec1ed5bfaf1d43db06be99", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Diagnostics\Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99.sln", "Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99");
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_7c885ca20209ca95cfec1ed5bfaf1d43db06be99()
        {
            bool isValid = LocationTestProject.LocaleTest("2_7c885ca20209ca95cfec1ed5bfaf1d43db06be99", @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\IntelliMeta\NUnitTests\bin\Debug\Projects\Diagnostics\Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99.sln", "Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99");
            Assert.IsTrue(isValid);
        }

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
        /// <returns>True if locale passed</returns>
        public static bool LocaleTest(string commit, string solution, string project)
        {
            long millBefore = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            EditorController.ReInit();
            EditorController controller = EditorController.GetInstance();
            List<TRegion> selections = JsonUtil<List<TRegion>>.Read(@"commits\"+ commit + @"\input_selection.json");
            controller.SelectedLocations = selections;
            controller.CurrentViewCodeBefore = FileUtil.ReadFile(selections.First().Path);
            controller.CurrentViewCodePath = selections.First().Path;
            controller.SetProject(project);
            controller.SetSolution(solution);
            controller.SelectedLocations = selections;

            controller.Extract();
            
            controller.RetrieveLocations(controller.CurrentViewCodeBefore);

            if (File.Exists(@"commits\" + commit + @"\negatives.json"))
            {
                List<int> negatives = JsonUtil<List<int>>.Read(@"commits\" + commit + @"\negatives.json");
                List<TRegion> negativesRegions = new List<TRegion>();
                List<TRegion> positivesRegions = new List<TRegion>();
                //foreach (var negative in negatives)
                //{
                for (int i = 0; i < controller.Locations.Count; i++)
                {
                    TRegion parent = new TRegion();
                    parent.Text = controller.Locations[i].SourceCode;
                    controller.Locations[i].Region.Parent = parent;
                    if (negatives.Contains(i))
                    {
                        negativesRegions.Add(controller.Locations[i].Region);
                    }
                    else
                    {
                        positivesRegions.Add(controller.Locations[i].Region);
                    }
                }
                //}

                //controller.Extract(controller.SelectedLocations, negativesRegions);
                controller.Extract(positivesRegions, negativesRegions);
        }
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
            long millAfer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long totalTime = (millAfer - millBefore);
            FileUtil.WriteToFile(@"commits\" + commit + @"\time.t", totalTime.ToString());
            return passed;
        }
    }
}
