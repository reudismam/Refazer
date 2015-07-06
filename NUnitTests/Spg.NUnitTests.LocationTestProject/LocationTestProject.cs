using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Spg.ExampleRefactoring.Bean;
using Spg.ExampleRefactoring.Util;
using Spg.LocationRefactor.Controller;
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
            List<string> projects = new List<string>();
            projects.Add("Proj00552fc2287f820ae9d42fd259aa6c07c2c5a805");
            bool isValid = LocationTestProject.LocaleTest("00552fc2287f820ae9d42fd259aa6c07c2c5a805", @"..\..\TestProjects\Projects\Portable6\Proj00552fc2287f820ae9d42fd259aa6c07c2c5a805.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Projb495c9ac44440cda289f09eb40e276c8d8e27ee9()
        {
            List<string> projects = new List<string>();
            projects.Add("Projb495c9ac44440cda289f09eb40e276c8d8e27ee9");
            bool isValid = LocationTestProject.LocaleTest("b495c9ac44440cda289f09eb40e276c8d8e27ee9", @"..\..\TestProjects\Projects\Portable14\Projb495c9ac44440cda289f09eb40e276c8d8e27ee9.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj008682140dfded0956241e81214a860a978b2395()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj008682140dfded0956241e81214a860a978b2395");
            bool isValid = LocationTestProject.LocaleTest("00552fc2287f820ae9d42fd259aa6c07c2c5a805", @"..\..\TestProjects\Projects\Portable13\Proj008682140dfded0956241e81214a860a978b2395.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj8c146441b4ecedbf7648e890d33f946f9b206e01()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj8c146441b4ecedbf7648e890d33f946f9b206e01");
            bool isValid = LocationTestProject.LocaleTest("8c146441b4ecedbf7648e890d33f946f9b206e01", @"..\..\TestProjects\Projects\Portable12\Proj8c146441b4ecedbf7648e890d33f946f9b206e01.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            bool isValid = LocationTestProject.LocaleTest("673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"..\..\TestProjects\Projects\Portable11\Proj673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            bool isValid = LocationTestProject.LocaleTest("2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"..\..\TestProjects\Projects\CodeAnalysisTest\Proj2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj3_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            bool isValid = LocationTestProject.LocaleTest("3_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"..\..\TestProjects\Projects\CodeAnalysisTest\Proj2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj4_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj4_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            bool isValid = LocationTestProject.LocaleTest("4_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"..\..\TestProjects\Projects\CommandLine\Proj4_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj5_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj5_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            bool isValid = LocationTestProject.LocaleTest("5_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"..\..\TestProjects\Projects\Syntax\Proj5_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj6_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj6_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            bool isValid = LocationTestProject.LocaleTest("6_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"..\..\TestProjects\Projects\Symbol\Proj6_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj8_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj6_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            bool isValid = LocationTestProject.LocaleTest("8_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"..\..\TestProjects\Projects\Symbol\Proj6_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", projects);
            Assert.IsTrue(isValid);
        }

        // <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj9_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj9_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            bool isValid = LocationTestProject.LocaleTest("9_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"..\..\TestProjects\Projects\Emit\Proj9_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj83e4349133d27a8f4dd5a85b69eb4ba00d41e6f8()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj83e4349133d27a8f4dd5a85b69eb4ba00d41e6f8");
            bool isValid = LocationTestProject.LocaleTest("83e4349133d27a8f4dd5a85b69eb4ba00d41e6f8", @"..\..\TestProjects\Projects\Portable10\Proj83e4349133d27a8f4dd5a85b69eb4ba00d41e6f8.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proje817dab72dd5199cb5c7f661bc6b289f63ae706b()
        {
            List<string> projects = new List<string>();
            projects.Add("Proje817dab72dd5199cb5c7f661bc6b289f63ae706b");
            bool isValid = LocationTestProject.LocaleTest("e817dab72dd5199cb5c7f661bc6b289f63ae706b", @"..\..\TestProjects\Projects\Portable9\Proje817dab72dd5199cb5c7f661bc6b289f63ae706b.sln", projects);
            Assert.IsTrue(isValid);
        }



        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj2_cd68d0323eb97f18c10281847c831f8e361506b9()
        {
            List<string> projects = new List<string>();
            projects.Add("Projcd68d0323eb97f18c10281847c831f8e361506b9");
            bool isValid = LocationTestProject.LocaleTest("2_cd68d0323eb97f18c10281847c831f8e361506b9", @"..\..\TestProjects\Projects\Portable8\Projcd68d0323eb97f18c10281847c831f8e361506b9.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj3_cd68d0323eb97f18c10281847c831f8e361506b9()
        {
            List<string> projects = new List<string>();
            projects.Add("Projcd68d0323eb97f18c10281847c831f8e361506b9");
            bool isValid = LocationTestProject.LocaleTest("3_cd68d0323eb97f18c10281847c831f8e361506b9", @"..\..\TestProjects\Projects\Portable8\Projcd68d0323eb97f18c10281847c831f8e361506b9.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Projf66696e70c90ce7fa1476d53cc84cc18e438d19b()
        {
            List<string> projects = new List<string>();
            projects.Add("Proje4d141a3c5f51ce4021d105e2b330564e02069fc");
            bool isValid = LocationTestProject.LocaleTest("f66696e70c90ce7fa1476d53cc84cc18e438d19b", @"..\..\TestProjects\Projects\VBCSCompilerTests\Proje4d141a3c5f51ce4021d105e2b330564e02069fc.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj4b402939708adf35a7a5e12ffc99dc14cc1f4766()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj2_4b402939708adf35a7a5e12ffc99dc14cc1f4766");
            bool isValid = LocationTestProject.LocaleTest("2_4b402939708adf35a7a5e12ffc99dc14cc1f4766", @"..\..\TestProjects\Projects\CSharp2\Proj2_4b402939708adf35a7a5e12ffc99dc14cc1f4766.sln", projects);
            Assert.IsTrue(isValid);
        }


        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj8ecd05880b478e4ca997a4789b976ef73b070546()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj4b402939708adf35a7a5e12ffc99dc14cc1f4766");
            bool isValid = LocationTestProject.LocaleTest("8ecd05880b478e4ca997a4789b976ef73b070546", @"..\..\TestProjects\Projects\Portable7\Proj4b402939708adf35a7a5e12ffc99dc14cc1f4766.sln", projects);
            Assert.IsTrue(isValid);
        }
        


        [Test]
        public void Proj04d060498bc0c30403bb05872e396052d826d082()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj04d060498bc0c30403bb05872e396052d826d082");
            bool isValid = LocationTestProject.LocaleTest("04d060498bc0c30403bb05872e396052d826d082", @"..\..\TestProjects\Projects\Diagnostics2\Proj04d060498bc0c30403bb05872e396052d826d082.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj318b2b0e476a122ebc033b13d41449ef1c814c1d()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj318b2b0e476a122ebc033b13d41449ef1c814c1d");
            bool isValid = LocationTestProject.LocaleTest("318b2b0e476a122ebc033b13d41449ef1c814c1d", @"..\..\TestProjects\Projects\Core2\Proj318b2b0e476a122ebc033b13d41449ef1c814c1d.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test case for parameter to constant value
        /// </summary>
        [Test]
        public void Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            List<string> projects = new List<string>();
            projects.Add("Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");
            bool isValid = LocationTestProject.LocaleTest("c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"..\..\TestProjects\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd()
        {
            List<string> projects = new List<string>();
            projects.Add("Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd");
            bool isValid = LocationTestProject.LocaleTest("e7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd", @"..\..\TestProjects\Projects\Portable3\Portable\Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj1113fd3db14fd23fc081e90f27f4ddafad7b244d()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj1113fd3db14fd23fc081e90f27f4ddafad7b244d");
            bool isValid = LocationTestProject.LocaleTest("1113fd3db14fd23fc081e90f27f4ddafad7b244d", @"..\..\TestProjects\Projects\Portable\Proj1113fd3db14fd23fc081e90f27f4ddafad7b244d.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Change Exception test
        /// </summary>
        [Test]
        public void Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf()
        {
            List<string> projects = new List<string>();
            projects.Add("Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf");
            bool isValid = LocationTestProject.LocaleTest("cc3d32746f60ed5a9f3775ef0ec44424b03d65cf", @"..\..\TestProjects\Projects\Portable2\Portable\Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            List<string> projects = new List<string>();
            projects.Add("Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");
            bool isValid = LocationTestProject.LocaleTest("2_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"..\..\TestProjects\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj3_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            List<string> projects = new List<string>();
            projects.Add("Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");
            bool isValid = LocationTestProject.LocaleTest("3_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"..\..\TestProjects\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj4_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            List<string> projects = new List<string>();
            projects.Add("Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");
            bool isValid = LocationTestProject.LocaleTest("4_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"..\..\TestProjects\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj5_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            List<string> projects = new List<string>();
            projects.Add("Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");
            bool isValid = LocationTestProject.LocaleTest("5_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"..\..\TestProjects\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", projects);
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
            List<string> projects = new List<string>();
            projects.Add("Proj49cdaceb2828acc1f50223826d478a00a80a59e2");
            bool isValid = LocationTestProject.LocaleTest("49cdaceb2828acc1f50223826d478a00a80a59e2", @"..\..\TestProjects\Projects\CSharp\Proj49cdaceb2828acc1f50223826d478a00a80a59e2.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Projcfd9b464dbb07c8b183d89a403a8bc877b3e929d()
        {
            List<string> projects = new List<string>();
            projects.Add("Projcfd9b464dbb07c8b183d89a403a8bc877b3e929d");
            bool isValid = LocationTestProject.LocaleTest("cfd9b464dbb07c8b183d89a403a8bc877b3e929d", @"..\..\TestProjects\Projects\Portable4\Portable\Projcfd9b464dbb07c8b183d89a403a8bc877b3e929d.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_cfd9b464dbb07c8b183d89a403a8bc877b3e929d()
        {
            List<string> projects = new List<string>();
            projects.Add("Projcfd9b464dbb07c8b183d89a403a8bc877b3e929d");
            bool isValid = LocationTestProject.LocaleTest("2-cfd9b464dbb07c8b183d89a403a8bc877b3e929d", @"..\..\TestProjects\Projects\Portable4\Portable\Projcfd9b464dbb07c8b183d89a403a8bc877b3e929d.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99");
            bool isValid = LocationTestProject.LocaleTest("7c885ca20209ca95cfec1ed5bfaf1d43db06be99", @"..\..\TestProjects\Projects\Diagnostics\Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_7c885ca20209ca95cfec1ed5bfaf1d43db06be99()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99");
            bool isValid = LocationTestProject.LocaleTest("2_7c885ca20209ca95cfec1ed5bfaf1d43db06be99", @"..\..\TestProjects\Projects\Diagnostics\Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99.sln", projects);
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
            List<string> projects = new List<string>();
            projects.Add("Proje28c81243206f1bb26b861ca0162678ce11b538c");
            bool isValid = LocationTestProject.LocaleTest("e28c81243206f1bb26b861ca0162678ce11b538c", @"..\..\TestProjects\Projects\Core\Proje28c81243206f1bb26b861ca0162678ce11b538c.sln", projects);
            Assert.IsTrue(isValid);
        }

        //Entity Framework tests
        [Test]
        public void Projd83cdfa88557bdf399a2f52dc79aad9d69bce007Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Projd83cdfa88557bdf399a2f52dc79aad9d69bce007");
            bool isValid = LocationTestProject.LocaleTest("d83cdfa88557bdf399a2f52dc79aad9d69bce007", @"..\..\TestProjects\Projects\UnitTests\Projd83cdfa88557bdf399a2f52dc79aad9d69bce007.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Projd8e9409b652bc0fb9aa222f0b36a66a8fd7cede6Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Projd8e9409b652bc0fb9aa222f0b36a66a8fd7cede6");
            bool isValid = LocationTestProject.LocaleTest("d8e9409b652bc0fb9aa222f0b36a66a8fd7cede6", @"..\..\TestProjects\Projects\EntityFramework\Projd8e9409b652bc0fb9aa222f0b36a66a8fd7cede6.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj14623da1e612d16c52b331bbd37ac1294c856658Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj14623da1e612d16c52b331bbd37ac1294c856658");
            bool isValid = LocationTestProject.LocaleTest("14623da1e612d16c52b331bbd37ac1294c856658", @"..\..\TestProjects\Projects\UnitTests2\Proj14623da1e612d16c52b331bbd37ac1294c856658.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2bae908ad3f457eaee6cb2f7e2982b39580991d4Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj2bae908ad3f457eaee6cb2f7e2982b39580991d4");
            bool isValid = LocationTestProject.LocaleTest("2bae908ad3f457eaee6cb2f7e2982b39580991d4", @"..\..\TestProjects\Projects\EntityFramework3\Proj2bae908ad3f457eaee6cb2f7e2982b39580991d4.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj8d452499b23e250232406fa9c875973a054b17f9Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj8d452499b23e250232406fa9c875973a054b17f9");
            bool isValid = LocationTestProject.LocaleTest("8d452499b23e250232406fa9c875973a054b17f9", @"..\..\TestProjects\Projects\FunctionalTests\Proj8d452499b23e250232406fa9c875973a054b17f9.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_8d452499b23e250232406fa9c875973a054b17f9Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj2_8d452499b23e250232406fa9c875973a054b17f9");
            bool isValid = LocationTestProject.LocaleTest("2_8d452499b23e250232406fa9c875973a054b17f9", @"..\..\TestProjects\Projects\EntityFramework4\Proj2_8d452499b23e250232406fa9c875973a054b17f9.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj1571862b0479b8f1e1976abceac0bca9f3cdd2d7Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj1571862b0479b8f1e1976abceac0bca9f3cdd2d7");
            bool isValid = LocationTestProject.LocaleTest("1571862b0479b8f1e1976abceac0bca9f3cdd2d7", @"..\..\TestProjects\Projects\FunctionalTests2\FunctionalTests\Proj1571862b0479b8f1e1976abceac0bca9f3cdd2d7.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_1571862b0479b8f1e1976abceac0bca9f3cdd2d7Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj1571862b0479b8f1e1976abceac0bca9f3cdd2d7");
            bool isValid = LocationTestProject.LocaleTest("2_1571862b0479b8f1e1976abceac0bca9f3cdd2d7", @"..\..\TestProjects\Projects\FunctionalTests2\FunctionalTests\Proj1571862b0479b8f1e1976abceac0bca9f3cdd2d7.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj4_1571862b0479b8f1e1976abceac0bca9f3cdd2d7Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj1571862b0479b8f1e1976abceac0bca9f3cdd2d7");
            bool isValid = LocationTestProject.LocaleTest("4_1571862b0479b8f1e1976abceac0bca9f3cdd2d7", @"..\..\TestProjects\Projects\FunctionalTests2\FunctionalTests\Proj1571862b0479b8f1e1976abceac0bca9f3cdd2d7.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj5_1571862b0479b8f1e1976abceac0bca9f3cdd2d7Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj1571862b0479b8f1e1976abceac0bca9f3cdd2d7");
            bool isValid = LocationTestProject.LocaleTest("5_1571862b0479b8f1e1976abceac0bca9f3cdd2d7", @"..\..\TestProjects\Projects\FunctionalTests2\FunctionalTests\Proj1571862b0479b8f1e1976abceac0bca9f3cdd2d7.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Projce1e33311465a6694c16f2289ab2a6d86e29bf18Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Projce1e33311465a6694c16f2289ab2a6d86e29bf18");
            bool isValid = LocationTestProject.LocaleTest("ce1e33311465a6694c16f2289ab2a6d86e29bf18", @"..\..\TestProjects\Projects\EntityFramework5\Projce1e33311465a6694c16f2289ab2a6d86e29bf18.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj8b9180bea7178d8348de47e28237c05ddb8a8244Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj8b9180bea7178d8348de47e28237c05ddb8a8244");
            bool isValid = LocationTestProject.LocaleTest("8b9180bea7178d8348de47e28237c05ddb8a8244", @"..\..\TestProjects\Projects\EntityFramework6\Proj8b9180bea7178d8348de47e28237c05ddb8a8244.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_8b9180bea7178d8348de47e28237c05ddb8a8244Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj8b9180bea7178d8348de47e28237c05ddb8a8244");
            bool isValid = LocationTestProject.LocaleTest("2_8b9180bea7178d8348de47e28237c05ddb8a8244", @"..\..\TestProjects\Projects\EntityFramework6\Proj8b9180bea7178d8348de47e28237c05ddb8a8244.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj829dec5d15c3d930815d72ce2d6909c51a97b1efTest()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj829dec5d15c3d930815d72ce2d6909c51a97b1ef");
            bool isValid = LocationTestProject.LocaleTest("829dec5d15c3d930815d72ce2d6909c51a97b1ef", @"..\..\TestProjects\Projects\EntityFramework7\Proj829dec5d15c3d930815d72ce2d6909c51a97b1ef.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj326d525b1d7d8e1c95f7227be63238ec783a9e92Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj326d525b1d7d8e1c95f7227be63238ec783a9e92");
            bool isValid = LocationTestProject.LocaleTest("326d525b1d7d8e1c95f7227be63238ec783a9e92", @"..\..\TestProjects\Projects\EntityFramework9\Proj326d525b1d7d8e1c95f7227be63238ec783a9e92.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj3_a8836009a81d24b05d5d1ad7c480eeec6cbde31bTest()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj3_a8836009a81d24b05d5d1ad7c480eeec6cbde31b");
            bool isValid = LocationTestProject.LocaleTest("3_a8836009a81d24b05d5d1ad7c480eeec6cbde31b", @"..\..\TestProjects\Projects\Core3\Proj3_a8836009a81d24b05d5d1ad7c480eeec6cbde31b.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj8da9f0ea9d04cdc8fe7c14adc4c186dc7302a50dTest()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj8da9f0ea9d04cdc8fe7c14adc4c186dc7302a50d");
            bool isValid = LocationTestProject.LocaleTest("8da9f0ea9d04cdc8fe7c14adc4c186dc7302a50d", @"..\..\TestProjects\Projects\Core.Test2\Proj8da9f0ea9d04cdc8fe7c14adc4c186dc7302a50d.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_8da9f0ea9d04cdc8fe7c14adc4c186dc7302a50dTest()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj8da9f0ea9d04cdc8fe7c14adc4c186dc7302a50d");
            bool isValid = LocationTestProject.LocaleTest("2_8da9f0ea9d04cdc8fe7c14adc4c186dc7302a50d", @"..\..\TestProjects\Projects\Core.Test2\Proj8da9f0ea9d04cdc8fe7c14adc4c186dc7302a50d.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj3_8da9f0ea9d04cdc8fe7c14adc4c186dc7302a50dTest()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj3_8da9f0ea9d04cdc8fe7c14adc4c186dc7302a50d");
            bool isValid = LocationTestProject.LocaleTest("3_8da9f0ea9d04cdc8fe7c14adc4c186dc7302a50d", @"..\..\TestProjects\Projects\Test.Utility\Proj3_8da9f0ea9d04cdc8fe7c14adc4c186dc7302a50d.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Projd9f64ea1704f8d99087ffdb646aa39003fc7c50aTest()
        {
            List<string> projects = new List<string>();
            projects.Add("Projd9f64ea1704f8d99087ffdb646aa39003fc7c50a");
            bool isValid = LocationTestProject.LocaleTest("d9f64ea1704f8d99087ffdb646aa39003fc7c50a", @"..\..\TestProjects\Projects\Core4\Projd9f64ea1704f8d99087ffdb646aa39003fc7c50a.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proja569c556805ce4f788b9644c1cc9a578719df3abTest()
        {
            List<string> projects = new List<string>();
            projects.Add("Proja569c556805ce4f788b9644c1cc9a578719df3ab");
            bool isValid = LocationTestProject.LocaleTest("a569c556805ce4f788b9644c1cc9a578719df3ab", @"..\..\TestProjects\Projects\NuGet.Client\Proja569c556805ce4f788b9644c1cc9a578719df3ab.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_a569c556805ce4f788b9644c1cc9a578719df3abTest()
        {
            List<string> projects = new List<string>();
            projects.Add("Proja569c556805ce4f788b9644c1cc9a578719df3ab");
            bool isValid = LocationTestProject.LocaleTest("2_a569c556805ce4f788b9644c1cc9a578719df3ab", @"..\..\TestProjects\Projects\NuGet.Client\Proja569c556805ce4f788b9644c1cc9a578719df3ab.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj6cf11e1d98dcc2c1c441e674af3cfc23fbd44d51Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj6cf11e1d98dcc2c1c441e674af3cfc23fbd44d51");
            bool isValid = LocationTestProject.LocaleTest("2_a569c556805ce4f788b9644c1cc9a578719df3ab", @"..\..\TestProjects\Projects\NuGet.Client2\Proj6cf11e1d98dcc2c1c441e674af3cfc23fbd44d51.sln", projects);
            Assert.IsTrue(isValid);
        }


        /// <summary>
        /// Locale test base method
        /// </summary>
        /// <param name="commit">commit id</param>
        /// <returns>True if locale passed</returns>
        public static bool LocaleTest(string commit, string solution, List<string> project)
        {
            long millBefore = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            EditorController.ReInit();
            EditorController controller = EditorController.GetInstance();
            List<TRegion> selections = JsonUtil<List<TRegion>>.Read(@"..\..\TestProjects\commits\" + commit + @"\input_selection.json");
            controller.SelectedLocations = selections;
            controller.CurrentViewCodeBefore = FileUtil.ReadFile(selections.First().Path);
            string exactPath = Path.GetFullPath(selections.First().Path);

            controller.CurrentViewCodePath = exactPath;
            controller.SetProject(project);
            controller.SetSolution(solution);
            controller.SelectedLocations = selections;

            controller.Extract();

            controller.RetrieveLocations(controller.CurrentViewCodeBefore);

            if (File.Exists(@"..\..\TestProjects\commits\" + commit + @"\negatives.json"))
            {
                List<int> negatives = JsonUtil<List<int>>.Read(@"..\..\TestProjects\commits\" + commit + @"\negatives.json");
                List<TRegion> negativesRegions = new List<TRegion>();
                List<TRegion> positivesRegions = new List<TRegion>();
                //foreach (var negative in negatives)         
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

            List<Selection> locations = JsonUtil<List<Selection>>.Read(@"..\..\TestProjects\commits\" + commit + @"\found_locations.json");
            if (locations.Count != controller.Locations.Count) return false;

            bool passed = true;
            for (int i = 0; i < locations.Count; i++)
            {
                if (!Path.GetFullPath(locations[i].SourcePath).ToUpperInvariant().Equals(controller.Locations[i].SourceClass.ToUpperInvariant())) { passed = false; break; }

                if (locations[i].Start != controller.Locations[i].Region.Start || locations[i].Length != controller.Locations[i].Region.Length)
                {
                    passed = false;
                    break;
                }
            }
            long millAfer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long totalTime = (millAfer - millBefore);
            FileUtil.WriteToFile(@"..\..\TestProjects\commits\" + commit + @"\time.t", totalTime.ToString());
            return passed;
        }
    }
}



