using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Office.Interop.Excel;
using NUnit.Framework;
using Spg.ExampleRefactoring.Util;
using Spg.LocationRefactor.Controller;
using Spg.LocationRefactor.Location;
using Spg.LocationRefactor.TextRegion;
using Taramon.Exceller;

namespace NUnitTests.Spg.NUnitTests.LocationTestSolution
{
    [TestFixture]
    public class MetadataLocTestSol
    {
        [Test]
        public void Proj00552fc()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\00552fc", @"Roslyn\roslyn11\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Projb495c9a()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\b495c9a", @"Roslyn\roslyn16\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj0086821()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\0086821", @"Roslyn\roslyn12\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        ///// <summary>
        ///// Test Method Call To Identifier transformation
        ///// </summary>
        //[Test]
        //public void Proj8c146441b4ecedbf7648e890d33f946f9b206e01()
        //{
        //    List<string> projects = new List<string>();
        //    projects.Add("Proj8c146441b4ecedbf7648e890d33f946f9b206e01");
        //    bool isValid = LocaleTestSolution("8c146441b4ecedbf7648e890d33f946f9b206e01", @"..\..\TestProjects\Projects\Portable12\Proj8c146441b4ecedbf7648e890d33f946f9b206e01.sln", projects);
        //    Assert.IsTrue(isValid);
        //}

        [Test]
        public void Proj2_8c14644()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\2_8c14644", @"Roslyn\roslyn\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj3_8c14644()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\3_8c14644", @"Roslyn\roslyn\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysisTest");
            bool isValid = LocaleTestSolution(@"Roslyn\2_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj3_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\3_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj4_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCommandLineTest");
            bool isValid = LocaleTestSolution(@"Roslyn\4_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj5_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCompilerSyntaxTest");
            bool isValid = LocaleTestSolution(@"Roslyn\5_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj6_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCompilerSymbolTest");
            bool isValid = LocaleTestSolution(@"Roslyn\6_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj8_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCompilerSymbolTest");
            bool isValid = LocaleTestSolution(@"Roslyn\8_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj9_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCompilerEmitTest");
            bool isValid = LocaleTestSolution(@"Roslyn\9_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj83e4349()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\83e4349", @"Roslyn\roslyn15\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proje817dab()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpWorkspace");
            bool isValid = LocaleTestSolution(@"Roslyn\e817dab", @"Roslyn\roslyn17\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Projcd68d03()
        {
            List<string> projects = new List<string>();
            projects.Add("Workspaces");
            bool isValid = LocaleTestSolution(@"Roslyn\cd68d03", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_cd68d03()
        {
            List<string> projects = new List<string>();
            projects.Add("Workspaces");
            bool isValid = LocaleTestSolution(@"Roslyn\2_cd68d03", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj3_cd68d03()
        {
            List<string> projects = new List<string>();
            projects.Add("Workspaces");
            bool isValid = LocaleTestSolution(@"Roslyn\3_cd68d03", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Projf66696e()
        {
            List<string> projects = new List<string>();
            projects.Add("VBCSCompilerTests");
            bool isValid = LocaleTestSolution(@"Roslyn\f66696e", @"Roslyn\roslyn8\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj4b40293()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            projects.Add("CSharpCompilerEmitTest");
            bool isValid = LocaleTestSolution(@"Roslyn\4b40293", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj8ecd058()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\cfd9b46", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj04d0604()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpFxCopRulesDiagnosticAnalyzers");
            projects.Add("FxCopRulesDiagnosticAnalyzers");
            bool isValid = LocaleTestSolution(@"Roslyn\04d0604", @"Roslyn\roslyn18\Src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        ////[Test]
        ////public void Proj318b2b0e476a122ebc033b13d41449ef1c814c1d()
        ////{
        ////    List<string> projects = new List<string>();
        ////    projects.Add("Proj318b2b0e476a122ebc033b13d41449ef1c814c1d");
        ////    bool isValid = LocaleTestSolution("318b2b0e476a122ebc033b13d41449ef1c814c1d", @"..\..\TestProjects\Projects\Core2\Proj318b2b0e476a122ebc033b13d41449ef1c814c1d.sln", projects);
        ////    Assert.IsTrue(isValid);
        ////}

        /// <summary>
        /// Test case for parameter to constant value
        /// </summary>
        [Test]
        public void Projc96d9ce()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\c96d9ce", @"Roslyn\roslyn4\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        ////[Test]
        ////public void Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd()
        ////{
        ////    List<string> projects = new List<string>();
        ////    projects.Add("Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd");
        ////    bool isValid = LocaleTestSolution("e7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd", @"..\..\TestProjects\Projects\Portable3\Portable\Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd.sln", projects);
        ////    Assert.IsTrue(isValid);
        ////}

        [Test]
        public void Proj1113fd3()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\1113fd3", @"Roslyn\roslyn2\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        /////// <summary>
        /////// Change Exception test
        /////// </summary>
        ////[Test]
        ////public void Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf()
        ////{
        ////    List<string> projects = new List<string>();
        ////    projects.Add("Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf");
        ////    bool isValid = LocaleTestSolution("cc3d32746f60ed5a9f3775ef0ec44424b03d65cf", @"..\..\TestProjects\Projects\Portable2\Portable\Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf.sln", projects);
        ////    Assert.IsTrue(isValid);
        ////}

        ////[Test]
        ////public void Proj2_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        ////{
        ////    List<string> projects = new List<string>();
        ////    projects.Add("Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");
        ////    bool isValid = LocaleTestSolution("2_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"..\..\TestProjects\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", projects);
        ////    Assert.IsTrue(isValid);
        ////}

        [Test]
        public void Proj3_c96d9ce()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\3_c96d9ce", @"Roslyn\roslyn4\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj4_c96d9ce()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\4_c96d9ce", @"Roslyn\roslyn4\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj5_c96d9ce()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\5_c96d9ce", @"Roslyn\roslyn4\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        ////[Test]
        ////public void Proj49cdaceb2828acc1f50223826d478a00a80a59e2()
        ////{
        ////    List<string> projects = new List<string>();
        ////    projects.Add("Proj49cdaceb2828acc1f50223826d478a00a80a59e2");
        ////    bool isValid = LocaleTestSolution("49cdaceb2828acc1f50223826d478a00a80a59e2", @"..\..\TestProjects\Projects\CSharp\Proj49cdaceb2828acc1f50223826d478a00a80a59e2.sln", projects);
        ////    Assert.IsTrue(isValid);
        ////}

        [Test]
        public void Projcfd9b46()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\cfd9b46", @"Roslyn\roslyn6\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_cfd9b46()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\2_cfd9b46", @"Roslyn\roslyn6\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj7c885ca()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCommandLineTest");
            projects.Add("CodeAnalysisTest");
            bool isValid = LocaleTestSolution(@"Roslyn\7c885ca", @"Roslyn\roslyn14\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_7c885ca()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpFxCopRulesDiagnosticAnalyzers");
            bool isValid = LocaleTestSolution(@"Roslyn\2_7c885ca", @"Roslyn\roslyn14\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proje28c812()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysisDiagnosticAnalyzers");
            projects.Add("CSharpFxCopRulesDiagnosticAnalyzers");
            bool isValid = LocaleTestSolution(@"Roslyn\e28c812", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        //Entity Framework tests
        [Test]
        public void Projd83cdfa()
        {
            List<string> projects = new List<string>();
            projects.Add("UnitTests");
            bool isValid = LocaleTestSolution(@"EntityFramewok\d83cdfa", @"EntityFramework\entityframework1\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Projd8e9409()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            bool isValid = LocaleTestSolution(@"EntityFramewok\d8e9409", @"EntityFramework\entityframework11\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Projbc42e49()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            bool isValid = LocaleTestSolution(@"EntityFramewok\bc42e49", @"EntityFramework\entityframework2\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj14623da()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            projects.Add("UnitTests");
            bool isValid = LocaleTestSolution(@"EntityFramewok\14623da", @"EntityFramework\entityframework10\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_14623da()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            projects.Add("UnitTests");
            bool isValid = LocaleTestSolution(@"EntityFramewok\2_14623da", @"EntityFramework\entityframework10\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj3_14623da()
        {
            List<string> projects = new List<string>();
            projects.Add("UnitTests");
            bool isValid = LocaleTestSolution(@"EntityFramewok\3_14623da", @"EntityFramework\entityframework10\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj4_14623da()
        {
            List<string> projects = new List<string>();
            projects.Add("UnitTests");
            bool isValid = LocaleTestSolution(@"EntityFramewok\4_14623da", @"EntityFramework\entityframework10\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2bae908()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            bool isValid = LocaleTestSolution(@"EntityFramewok\2bae908", @"EntityFramework\entityframework3\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj8d45249()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool isValid = LocaleTestSolution(@"EntityFramewok\8d45249", @"EntityFramework\entityframework4\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        //[Test]
        //public void Proj2_8d452499b23e250232406fa9c875973a054b17f9Test()
        //{
        //    List<string> projects = new List<string>();
        //    projects.Add("Proj2_8d452499b23e250232406fa9c875973a054b17f9");
        //    bool isValid = LocaleTestSolution("2_8d452499b23e250232406fa9c875973a054b17f9", @"..\..\TestProjects\Projects\EntityFramework4\Proj2_8d452499b23e250232406fa9c875973a054b17f9.sln", projects);
        //    Assert.IsTrue(isValid);
        //}

        [Test]
        public void Proj1571862()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool isValid = LocaleTestSolution(@"EntityFramewok\1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_1571862()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool isValid = LocaleTestSolution(@"EntityFramewok\2_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        //[Test]
        //public void Proj4_1571862()
        //{
        //    List<string> projects = new List<string>();
        //    projects.Add("FunctionalTests");
        //    bool isValid = LocaleTestSolution(@"EntityFramewok\4_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);
        //    Assert.IsTrue(isValid);
        //}

        [Test]
        public void Proj5_1571862()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool isValid = LocaleTestSolution(@"EntityFramewok\5_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        //[Test]
        //public void Proj6_1571862()
        //{
        //    List<string> projects = new List<string>();
        //    projects.Add("FunctionalTests.Transitional");
        //    bool isValid = LocaleTestSolution(@"EntityFramewok\6_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);
        //    Assert.IsTrue(isValid);
        //}

        [Test]
        public void Projce1e333()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            bool isValid = LocaleTestSolution(@"EntityFramewok\ce1e333", @"EntityFramework\entityframework5\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj8b9180b()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool isValid = LocaleTestSolution(@"EntityFramewok\8b9180b", @"EntityFramework\entityframework6\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_8b9180b()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool isValid = LocaleTestSolution(@"EntityFramewok\2_8b9180b", @"EntityFramework\entityframework6\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj829dec5()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            bool isValid = LocaleTestSolution(@"EntityFramewok\829dec5", @"EntityFramework\entityframework7\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj326d525()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests.Transitional");
            bool isValid = LocaleTestSolution(@"EntityFramewok\326d525", @"EntityFramework\entityframework9\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_326d525()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool isValid = LocaleTestSolution(@"EntityFramewok\2_326d525", @"EntityFramework\entityframework9\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proja883600()
        {
            List<string> projects = new List<string>();
            projects.Add("Core.Test");
            bool isValid = LocaleTestSolution(@"NuGet\a883600", @"NuGet\nuget4\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_a883600()
        {
            List<string> projects = new List<string>();
            projects.Add("CommandLine");
            bool isValid = LocaleTestSolution(@"NuGet\2_a883600", @"NuGet\nuget4\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj3_a883600()
        {
            List<string> projects = new List<string>();
            projects.Add("Core");
            bool isValid = LocaleTestSolution(@"NuGet\3_a883600", @"NuGet\nuget4\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        //[Test]
        //public void Proj4_a883600()
        //{
        //    List<string> projects = new List<string>();
        //    projects.Add("NuGet.Client.VisualStudio.PowerShell");
        //    bool isValid = LocaleTestSolution(@"NuGet\4_a883600", @"NuGet\nuget4\NuGet.sln", projects);
        //    Assert.IsTrue(isValid);
        //}

        [Test]
        public void Proj5_a883600()
        {
            List<string> projects = new List<string>();
            projects.Add("Core.Test");
            bool isValid = LocaleTestSolution(@"NuGet\5_a883600", @"NuGet\nuget4\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proja569c55()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client");
            bool isValid = LocaleTestSolution(@"NuGet\a569c55", @"NuGet\nuget5\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_a569c55()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client");
            bool isValid = LocaleTestSolution(@"NuGet\2_a569c55", @"NuGet\nuget5\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2dea84e()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.VisualStudio.PowerShell");
            bool isValid = LocaleTestSolution(@"NuGet\2dea84e", @"NuGet\nuget2\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("Core.Test");
            bool isValid = LocaleTestSolution(@"NuGet\8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("Core.Test");
            bool isValid = LocaleTestSolution(@"NuGet\2_8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj3_8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("Test.Utility");
            bool isValid = LocaleTestSolution(@"NuGet\3_8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj4_8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("VisualStudio.Test");
            bool isValid = LocaleTestSolution(@"NuGet\4_8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj5_8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests.Transitional");
            bool isValid = LocaleTestSolution(@"NuGet\5_8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Projd9f64ea()
        {
            List<string> projects = new List<string>();
            projects.Add("Core");
            bool isValid = LocaleTestSolution(@"NuGet\d9f64ea", @"NuGet\nuget7\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_d9f64ea()
        {
            List<string> projects = new List<string>();
            projects.Add("Core");
            bool isValid = LocaleTestSolution(@"NuGet\2_d9f64ea", @"NuGet\nuget7\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj3_d9f64ea()
        {
            List<string> projects = new List<string>();
            projects.Add("Core");
            bool isValid = LocaleTestSolution(@"NuGet\3_d9f64ea", @"NuGet\nuget7\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Projdfc4e3d()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.VisualStudio.PowerShell");
            bool isValid = LocaleTestSolution(@"NuGet\dfc4e3d", @"NuGet\nuget6\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj74d4d32()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.CommandLine");
            bool isValid = LocaleTestSolution(@"NuGet\74d4d32", @"NuGet\nuget8\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj7d11ddd()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.CommandLine");
            bool isValid = LocaleTestSolution(@"NuGet\7d11ddd", @"NuGet\nuget9\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_7d11ddd()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.CommandLine");
            bool isValid = LocaleTestSolution(@"NuGet\2_7d11ddd", @"NuGet\nuget9\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Projee953e8()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.VisualStudio.UI");
            bool isValid = LocaleTestSolution(@"NuGet\ee953e8", @"NuGet\nuget10\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_ee953e8()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.VisualStudio.UI");
            bool isValid = LocaleTestSolution(@"NuGet\2_ee953e8", @"NuGet\nuget10\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj4ff8771()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.VisualStudio.UI");
            bool isValid = LocaleTestSolution(@"NuGet\4ff8771", @"NuGet\nuget11\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        ////[Test]
        ////public void Proj6cf11e1d98dcc2c1c441e674af3cfc23fbd44d51Test()
        ////{
        ////    List<string> projects = new List<string>();
        ////    projects.Add("Proj6cf11e1d98dcc2c1c441e674af3cfc23fbd44d51");
        ////    bool isValid = LocaleTestSolution("2_a569c556805ce4f788b9644c1cc9a578719df3ab", @"..\..\TestProjects\Projects\NuGet.Client2\Proj6cf11e1d98dcc2c1c441e674af3cfc23fbd44d51.sln", projects);
        ////    Assert.IsTrue(isValid);
        ////}

        ///// <summary>
        ///// Locale test base method
        ///// </summary>
        ///// <param name="commit">commit id</param>
        ///// <param name="solution">Solution</param>
        ///// <param name="project">Project</param>
        ///// <returns>True if locale passed</returns>
        //public static bool LocaleTestSolution(string commit, string solution, List<string> project)
        //{
        //    long millBefore;// = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        //    long totalTimeToExtract = 0;
        //    EditorController.ReInit();
        //    EditorController controller = EditorController.GetInstance();

        //    string expHome = Environment.GetEnvironmentVariable("EXP_HOME", EnvironmentVariableTarget.User);

        //    List<TRegion> selections = JsonUtil<List<TRegion>>.Read(expHome + @"commit\" + commit + @"\metadata\locations_on_commit.json");

        //    controller.SelectedLocations = selections;
        //    controller.CurrentViewCodeBefore = FileUtil.ReadFile(selections.First().Path);
        //    string exactPath = Path.GetFullPath(selections.First().Path);

        //    controller.CurrentViewCodePath = exactPath;
        //    controller.SetProject(project);
        //    controller.SetSolution(expHome + solution);

        //    List<TRegion> metadataLocations = new List<TRegion>();
        //    metadataLocations.AddRange(selections.GetRange(0, 2));
        //    while (true)
        //    {
        //        controller.SelectedLocations = metadataLocations;
        //        millBefore = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        //        controller.Extract();
        //        controller.RetrieveLocations();

        //        TRegion tregion = MatchesLocationsOnCommit(selections, controller.Locations);
        //        if (tregion == null)
        //        {
        //            break;
        //        }

        //        if (ContainsTRegion(metadataLocations, tregion))
        //        {
        //            return false;
        //        }
        //        metadataLocations.Add(tregion);
        //    }

        //    List<TRegion> negativesRegions = new List<TRegion>();
        //    if (File.Exists(expHome + @"commit\" + commit + @"\negatives.json"))
        //    {
        //        List<int> negatives = JsonUtil<List<int>>.Read(expHome + @"commit\" + commit + @"\negatives.json");
        //        List<TRegion> positivesRegions = new List<TRegion>();
        //        for (int i = 0; i < controller.Locations.Count; i++)
        //        {
        //            TRegion parent = new TRegion();
        //            parent.Text = controller.Locations[i].SourceCode;
        //            controller.Locations[i].Region.Parent = parent;
        //            if (negatives.Contains(i))
        //            {
        //                negativesRegions.Add(controller.Locations[i].Region);
        //            }
        //            else
        //            {
        //                positivesRegions.Add(controller.Locations[i].Region);
        //            }
        //        }

        //        millBefore = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        //        controller.Extract(positivesRegions, negativesRegions);
        //        long millAfterExtract = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        //        totalTimeToExtract = (millAfterExtract - millBefore);
        //        controller.RetrieveLocations();
        //    }

        //    long millAfer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        //    long totalTime = (millAfer - millBefore);
        //    Log(commit, totalTime, totalTimeToExtract, metadataLocations.Count, negativesRegions.Count, controller.Locations.Count, selections.Count);
        //    //remove
        //    List<TRegion> nselections = new List<TRegion>();
        //    foreach (CodeLocation location in controller.Locations)
        //    {
        //        TRegion selection = new TRegion();
        //        selection.Start = location.Region.Start;
        //        selection.Length = location.Region.Length;
        //        selection.Parent = location.Region.Parent;
        //        selection.Color = location.Region.Color;
        //        selection.Path = location.SourceClass;
        //        selection.Text = location.Region.Text;
        //        nselections.Add(selection);
        //    }

        //    FileInfo file = new FileInfo(expHome + @"commit\" + commit + @"\metadata\");
        //    file.Directory.Create();
        //    file = new FileInfo(expHome + @"commit\" + commit + @"\metadata_tool\");
        //    file.Directory.Create();

        //    JsonUtil<List<TRegion>>.Write(nselections, expHome + @"commit\" + commit + @"\metadata_tool\locations_on_commit.json");
        //    //remove
        //    return true;
        //}

        /// <summary>
        /// Locale test base method
        /// </summary>
        /// <param name="commit">commit id</param>
        /// <param name="solution">Solution</param>
        /// <param name="project">Project</param>
        /// <returns>True if locale passed</returns>
        public static bool LocaleTestSolution(string commit, string solution, List<string> project)
        {
            long millBefore;// = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long totalTimeToExtract = 0;
            EditorController.ReInit();
            EditorController controller = EditorController.GetInstance();

            string expHome = Environment.GetEnvironmentVariable("EXP_HOME", EnvironmentVariableTarget.User);

            List<TRegion> selections = JsonUtil<List<TRegion>>.Read(expHome + @"commit\" + commit + @"\metadata\locations_on_commit.json");

            controller.SelectedLocations = selections;
            controller.CurrentViewCodeBefore = FileUtil.ReadFile(selections.First().Path);
            string exactPath = Path.GetFullPath(selections.First().Path);

            controller.CurrentViewCodePath = exactPath;
            controller.SetProject(project);
            controller.SetSolution(expHome + solution);

            List<TRegion> metadataLocations = new List<TRegion>();
            metadataLocations.AddRange(selections.GetRange(0, 2));
            while (true)
            {
                controller.SelectedLocations = metadataLocations;
                millBefore = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                controller.Extract();
                controller.RetrieveLocations();

                TRegion tregion = MatchesLocationsOnCommit(selections, controller.Locations, metadataLocations);
                if (tregion == null)
                {
                    if(MatchesLocationsOnCommit(selections, controller.Locations)) // all locations on selections are present on commits.
                    {
                        break;
                    }
                    return false;
                }

                metadataLocations.Add(tregion);
            }

            List<TRegion> negativesRegions = new List<TRegion>();
            if (File.Exists(expHome + @"commit\" + commit + @"\negatives.json"))
            {
                List<TRegion> negatives = JsonUtil<List<TRegion>>.Read(expHome + @"commit\" + commit + @"\negatives.json");
                List<TRegion> positivesRegions = new List<TRegion>();
                foreach (var item in controller.Locations)
                {
                    TRegion parent = new TRegion();
                    parent.Text = item.SourceCode;
                    item.Region.Parent = parent;
                    if (negatives.Contains(item.Region))
                    {
                        negativesRegions.Add(item.Region);
                    }
                    else
                    {
                        positivesRegions.Add(item.Region);
                    }
                }

                millBefore = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                controller.Extract(positivesRegions, negativesRegions);
                long millAfterExtract = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                totalTimeToExtract = (millAfterExtract - millBefore);
                controller.RetrieveLocations();
            }

            long millAfer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long totalTime = (millAfer - millBefore);
            Log(commit, totalTime, totalTimeToExtract, metadataLocations.Count, negativesRegions.Count, controller.Locations.Count, selections.Count);
            //remove
            List<TRegion> nselections = new List<TRegion>();
            foreach (CodeLocation location in controller.Locations)
            {
                TRegion selection = new TRegion();
                selection.Start = location.Region.Start;
                selection.Length = location.Region.Length;
                selection.Parent = location.Region.Parent;
                selection.Color = location.Region.Color;
                selection.Path = location.SourceClass;
                selection.Text = location.Region.Text;
                nselections.Add(selection);
            }

            FileInfo file = new FileInfo(expHome + @"commit\" + commit + @"\metadata\");
            file.Directory.Create();
            file = new FileInfo(expHome + @"commit\" + commit + @"\metadata_tool\");
            file.Directory.Create();

            JsonUtil<List<TRegion>>.Write(nselections, expHome + @"commit\" + commit + @"\metadata_tool\locations_on_commit.json");
            //remove
            return true;
        }

        private static Workbook mWorkBook;
        private static Sheets mWorkSheets;
        private static Worksheet mWSheet1;
        private static Application oXL;
        public static void Log(string commit, double time, double timeToExtract, int exLocations, int negs, int acLocations, int locations)
        {
            using (ExcelManager em = new ExcelManager())
            {

                em.Open(@"C:\Users\SPG-04\Documents\Research\Log2.xlsx");

                int empty;
                for (int i = 1;; i++)
                {
                    if (em.GetValue("A" + i, Category.Formatted).ToString().Equals(""))
                    {
                        empty = i;
                        break;
                    }
                }
                em.SetValue("A" + empty, commit);
                em.SetValue("B" + empty, time / 1000);
                em.SetValue("C" + empty, exLocations);
                em.SetValue("D" + empty, negs);
                em.SetValue("E" + empty, exLocations + negs);
                em.SetValue("F" + empty, acLocations);
                em.SetValue("G" + empty, locations);
                em.SetValue("H" + empty, timeToExtract);
                Console.WriteLine("" + empty);
                em.Save();
            }

            //string path = @"C:\Users\SPG-04\Documents\Research\Log.xls";

            //oXL = new Microsoft.Office.Interop.Excel.Application();
            //oXL.Visible = true;
            //oXL.DisplayAlerts = false;
            //mWorkBook = oXL.Workbooks.Open(path, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
            ////Get all the sheets in the workbook
            //mWorkSheets = mWorkBook.Worksheets;
            ////Get the allready exists sheet
            //mWSheet1 = (Microsoft.Office.Interop.Excel.Worksheet)mWorkSheets.get_Item("1");
            //Microsoft.Office.Interop.Excel.Range range = mWSheet1.UsedRange;
            //int colCount = range.Columns.Count;
            //int rowCount = range.Rows.Count;
            //Console.WriteLine(rowCount);
            //for (int index = 1; index < 15; index++)
            //{
            //    mWSheet1.Cells[rowCount + index, 1] = rowCount + index;
            //    mWSheet1.Cells[rowCount + index, 2] = "New Item" + index;
            //}
            //mWorkBook.SaveAs(path, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal,
            //Missing.Value, Missing.Value, Missing.Value, Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
            //Missing.Value, Missing.Value, Missing.Value,
            //Missing.Value, Missing.Value);
            //mWorkBook.Close(Missing.Value, Missing.Value, Missing.Value);
            //mWSheet1 = null;
            //mWorkBook = null;
            //oXL.Quit();
            //GC.WaitForPendingFinalizers();
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //GC.Collect();

        }

        private static bool ContainsTRegion(List<TRegion> metadataLocations, TRegion tregion)
        {
            foreach (var location in metadataLocations)
            {
                if (location.Start == tregion.Start &&
                    location.Length == tregion.Length &&
                    location.Path.ToUpperInvariant().Equals(tregion.Path.ToUpperInvariant()))
                {
                    return true;
                }
            }
            return false;
        }

        private static TRegion MatchesLocationsOnCommit(List<TRegion> metadatas, List<CodeLocation> locations, List<TRegion> metadataLocations)
        {
            foreach (TRegion metadata in metadatas)
            {
                bool isFound = false;
                foreach (var found in locations)
                {
                    if (metadata.Start == found.Region.Start &&
                        metadata.Length == found.Region.Length &&
                        metadata.Path.ToUpperInvariant().Equals(found.SourceClass.ToUpperInvariant()))
                    {
                        isFound = true;
                        break;
                    }
                }

                if (!isFound)
                {
                    if (!ContainsTRegion(metadataLocations, metadata))
                    {
                        return metadata;
                    }
                   
                }
            }
            return null;
        }

        private static bool MatchesLocationsOnCommit(List<TRegion> metadatas, List<CodeLocation> locations)
        {
            foreach (TRegion metadata in metadatas)
            {
                bool isFound = false;
                foreach (var found in locations)
                {
                    if (metadata.Start == found.Region.Start &&
                        metadata.Length == found.Region.Length &&
                        metadata.Path.ToUpperInvariant().Equals(found.SourceClass.ToUpperInvariant()))
                    {
                        isFound = true;
                        break;
                    }
                }

                if (!isFound) // if the selection on metadata is not found on locations return false.
                {
                    return false;
                }
            }
            return true;
        }
    }
}




