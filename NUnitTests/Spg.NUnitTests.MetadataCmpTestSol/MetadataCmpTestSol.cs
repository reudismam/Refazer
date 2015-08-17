using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Office.Interop.Excel;
using NUnit.Framework;
using NUnitTests.Spg.NUnitTests.Util;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Bean;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Util;
using Spg.LocationRefactor.Controller;
using Spg.LocationRefactor.Location;
using Spg.LocationRefactor.Node;
using Spg.LocationRefactor.TextRegion;
using Spg.LocationRefactor.Transform;
using Taramon.Exceller;

namespace NUnitTests.Spg.NUnitTests.CompleteTestSolution
{
    /// <summary>
    /// Test for complete systematic editing
    /// </summary>
    [TestFixture]
    public class MetadataCmpTestSol
    {
        [Test]
        public void Proj0086821()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\0086821", @"Roslyn\roslyn12\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\0086821");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj00552fc()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\00552fc", @"Roslyn\roslyn11\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\00552fc");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj8ecd058()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\8ecd058", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\8ecd058");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Projb495c9a()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\b495c9a", @"Roslyn\roslyn16\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\b495c9a");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\673f18e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysisTest");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\2_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\2_673f18e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj3_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\3_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\3_673f18e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj5_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCompilerSyntaxTest");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\5_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\5_673f18e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj6_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCompilerSymbolTest");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\6_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\6_673f18e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj8_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCompilerSymbolTest");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\8_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\8_673f18e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj9_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCompilerEmitTest");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\9_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\9_673f18e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj4_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCommandLineTest");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\4_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\4_673f18e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_8c14644()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\2_8c14644", @"Roslyn\roslyn\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\2_8c14644");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj83e4349()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\83e4349", @"Roslyn\roslyn15\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\83e4349");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proje817dab()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpWorkspace");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\e817dab", @"Roslyn\roslyn17\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\e817dab");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Projcd68d03()
        {
            List<string> projects = new List<string>();
            projects.Add("Workspaces");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\cd68d03", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\cd68d03");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_cd68d03()
        {
            List<string> projects = new List<string>();
            projects.Add("Workspaces");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\2_cd68d03", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\2_cd68d03");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj3_cd68d03()
        {
            List<string> projects = new List<string>();
            projects.Add("Workspaces");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\3_cd68d03", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\3_cd68d03");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Projf66696e()
        {
            List<string> projects = new List<string>();
            projects.Add("VBCSCompilerTests");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\f66696e", @"Roslyn\roslyn8\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\f66696e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj4b40293()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            projects.Add("CSharpCompilerEmitTest");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\4b40293", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\4b40293");

            Assert.IsTrue(passLocation && passTransformation);
        }

        //    /// <summary>
        //    /// Test Method Call To Identifier transformation
        //    /// </summary>
        //    [Test]
        //    public void Proj8ecd05880b478e4ca997a4789b976ef73b070546()
        //    {
        //        List<string> projects = new List<string>();
        //        projects.Add("Proj4b402939708adf35a7a5e12ffc99dc14cc1f4766");
        //        bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("8ecd05880b478e4ca997a4789b976ef73b070546", @"..\..\TestProjects\Projects\Portable7\Proj4b402939708adf35a7a5e12ffc99dc14cc1f4766.sln", projects);

        //        bool passTransformation = CompleteTestBase(@"8ecd05880b478e4ca997a4789b976ef73b070546");

        //        Assert.IsTrue(passLocation && passTransformation);
        //    }

        [Test]
        public void Proj04d0604()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpFxCopRulesDiagnosticAnalyzers");
            projects.Add("FxCopRulesDiagnosticAnalyzers");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\04d0604", @"Roslyn\roslyn18\Src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\04d0604");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj1113fd3()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\1113fd3", @"Roslyn\roslyn2\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\1113fd3");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Projc96d9ce()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\c96d9ce", @"Roslyn\roslyn4\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\c96d9ce");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj3_c96d9ce()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\3_c96d9ce", @"Roslyn\roslyn4\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\3_c96d9ce");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj4_c96d9ce()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\4_c96d9ce", @"Roslyn\roslyn4\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\4_c96d9ce");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj5_c96d9ce()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\5_c96d9ce", @"Roslyn\roslyn4\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\5_c96d9ce");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Projcfd9b46()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\cfd9b46", @"Roslyn\roslyn6\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\cfd9b46");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_cfd9b46()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\2_cfd9b46", @"Roslyn\roslyn6\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\2_cfd9b46");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj7c885ca()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCommandLineTest");
            projects.Add("CodeAnalysisTest");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\7c885ca", @"Roslyn\roslyn14\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\7c885ca");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_7c885ca()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpFxCopRulesDiagnosticAnalyzers");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\2_7c885ca", @"Roslyn\roslyn14\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\2_7c885ca");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proje28c812()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysisDiagnosticAnalyzers");
            projects.Add("CSharpFxCopRulesDiagnosticAnalyzers");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"Roslyn\e28c812", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\e28c812");

            Assert.IsTrue(passLocation && passTransformation);
        }

        //Entity Framework tests
        [Test]
        public void Projd83cdfa()
        {
            List<string> projects = new List<string>();
            projects.Add("UnitTests");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"EntityFramewok\d83cdfa", @"EntityFramework\entityframework1\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\d83cdfa");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Projd8e9409()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"EntityFramewok\d8e9409", @"EntityFramework\entityframework11\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\d8e9409");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj14623da()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            projects.Add("UnitTests");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"EntityFramewok\14623da", @"EntityFramework\entityframework10\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\14623da");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_14623da()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            projects.Add("UnitTests");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"EntityFramewok\2_14623da", @"EntityFramework\entityframework10\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\2_14623da");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj3_14623da()
        {
            List<string> projects = new List<string>();
            projects.Add("UnitTests");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"EntityFramewok\3_14623da", @"EntityFramework\entityframework10\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\3_14623da");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj4_14623da()
        {
            List<string> projects = new List<string>();
            projects.Add("UnitTests");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"EntityFramewok\4_14623da", @"EntityFramework\entityframework10\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\4_14623da");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2bae908()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"EntityFramewok\2bae908", @"EntityFramework\entityframework3\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\2bae908");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj8d45249()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"EntityFramewok\8d45249", @"EntityFramework\entityframework4\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\8d45249");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj1571862()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"EntityFramewok\1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\1571862");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_1571862()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"EntityFramewok\2_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\2_1571862");

            Assert.IsTrue(passLocation && passTransformation);
        }

        //    [Test]
        //    public void Proj4_1571862()
        //    {
        //        List<string> projects = new List<string>();
        //        projects.Add("FunctionalTests");
        //        bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\4_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);

        //        bool passTransformation = CompleteTestBase(@"EntityFramewok\4_1571862");

        //        Assert.IsTrue(passLocation && passTransformation);
        //    }

        [Test]
        public void Proj5_1571862()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"EntityFramewok\5_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\5_1571862");

            Assert.IsTrue(passLocation && passTransformation);
        }

        //    [Test]
        //    public void Proj6_1571862()
        //    {
        //        List<string> projects = new List<string>();
        //        projects.Add("FunctionalTests.Transitional");
        //        bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\6_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);

        //        bool passTransformation = CompleteTestBase(@"EntityFramewok\6_1571862");

        //        Assert.IsTrue(passLocation && passTransformation);
        //    }

        [Test]
        public void Projce1e333()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"EntityFramewok\ce1e333", @"EntityFramework\entityframework5\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\ce1e333");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj8b9180b()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"EntityFramewok\8b9180b", @"EntityFramework\entityframework6\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\8b9180b");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_8b9180b()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"EntityFramewok\2_8b9180b", @"EntityFramework\entityframework6\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\2_8b9180b");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj829dec5()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"EntityFramewok\829dec5", @"EntityFramework\entityframework7\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\829dec5");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj326d525()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests.Transitional");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"EntityFramewok\326d525", @"EntityFramework\entityframework9\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\326d525");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_326d525()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"EntityFramewok\2_326d525", @"EntityFramework\entityframework9\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\2_326d525");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proja883600()
        {
            List<string> projects = new List<string>();
            projects.Add("Core.Test");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"NuGet\a883600", @"NuGet\nuget4\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\a883600");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_a883600()
        {
            List<string> projects = new List<string>();
            projects.Add("CommandLine");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"NuGet\2_a883600", @"NuGet\nuget4\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\2_a883600");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj3_a883600()
        {
            List<string> projects = new List<string>();
            projects.Add("Core");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"NuGet\3_a883600", @"NuGet\nuget4\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\3_a883600");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj5_a883600()
        {
            List<string> projects = new List<string>();
            projects.Add("Core.Test");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"NuGet\5_a883600", @"NuGet\nuget4\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\5_a883600");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("Core.Test");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"NuGet\8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\8da9f0e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("Core.Test");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"NuGet\2_8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\2_8da9f0e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj3_8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("Test.Utility");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"NuGet\3_8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\3_8da9f0e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj4_8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("VisualStudio.Test");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"NuGet\4_8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\4_8da9f0e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj5_8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests.Transitional");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"NuGet\5_8da9f0e",
                @"NuGet\nuget3\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\5_8da9f0e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proja569c55()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"NuGet\a569c55", @"NuGet\nuget5\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\a569c55");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_a569c55()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"NuGet\2_a569c55", @"NuGet\nuget5\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\2_a569c55");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Projd9f64ea()
        {
            List<string> projects = new List<string>();
            projects.Add("Core");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"NuGet\d9f64ea", @"NuGet\nuget7\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\d9f64ea");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_d9f64ea()
        {
            List<string> projects = new List<string>();
            projects.Add("Core");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"NuGet\2_d9f64ea", @"NuGet\nuget7\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\2_d9f64ea");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Projdfc4e3d()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.VisualStudio.PowerShell");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"NuGet\dfc4e3d", @"NuGet\nuget6\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\dfc4e3d");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Pro2dea84e()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.VisualStudio.PowerShell");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"NuGet\2dea84e", @"NuGet\nuget2\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\2dea84e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj74d4d32()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.CommandLine");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"NuGet\74d4d32", @"NuGet\nuget8\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\74d4d32");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj7d11ddd()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.CommandLine");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"NuGet\7d11ddd", @"NuGet\nuget9\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\7d11ddd");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Projee953e8()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.VisualStudio.UI");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"NuGet\ee953e8", @"NuGet\nuget10\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\ee953e8");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_ee953e8()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.VisualStudio.UI");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"NuGet\2_ee953e8", @"NuGet\nuget10\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\2_ee953e8");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj4ff8771()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.VisualStudio.UI");
            bool passLocation = LocationTestSolution.MetadataLocTestSol.LocaleTestSolution(@"NuGet\4ff8771", @"NuGet\nuget11\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\4ff8771");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Complete test
        /// </summary>
        /// <param name="commit">Commit where the change occurs</param>
        /// <returns>True if pass test</returns>
        public static bool CompleteTestBase(string commit)
        {
            long millBefore = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            EditorController controller = EditorController.GetInstance();

            string expHome = Environment.GetEnvironmentVariable("EXP_HOME", EnvironmentVariableTarget.User);

            //remove
            List<TRegion> selections = JsonUtil<List<TRegion>>.Read(expHome + @"commit\" + commit + @"\metadata\locations_on_commit.json");
            List<CodeLocation> locations = TestUtil.GetAllLocationsOnCommit(selections, controller.Locations);
            controller.Locations = locations;
            //remove

            List<TRegion> regions = new List<TRegion>();
            List<CodeTransformation> codeTransformations = JsonUtil<List<CodeTransformation>>.Read(expHome + @"commit\" + commit + @"\metadata\transformed_locations.json");
            foreach (var entry in codeTransformations)
            {
                regions.Add(entry.Trans);
            }

            List<TRegion> metadataRegions = new List<TRegion>();
            metadataRegions.AddRange(regions.GetRange(0, 2));

            var globalTransformations = RegionManager.GetInstance().GroupTransformationsBySourcePath(codeTransformations);
            for (int i = 0; i < regions.Count; i++)
            {
                var dicionarySelection = RegionManager.GetInstance().GroupRegionBySourcePath(metadataRegions);

                List<Tuple<string, string>> documents = new List<Tuple<string, string>>();
                Dictionary<string, List<Selection>> dicSelections = new Dictionary<string, List<Selection>>();
                foreach (KeyValuePair<string, List<TRegion>> entry in dicionarySelection)
                {
                    string sourceCode = FileUtil.ReadFile(entry.Key);
                    Tuple<string, List<TRegion>> tu = Transform(sourceCode, globalTransformations[entry.Key], metadataRegions);
                    string sourceCodeAfter = tu.Item1;
                    List <Selection> selectionsList = new List<Selection>();
                    foreach (TRegion region in tu.Item2)
                    {
                        Selection selectionRegion = new Selection(region.Start, region.Length, region.Path, sourceCodeAfter, region.Text);
                        selectionsList.Add(selectionRegion);
                    }

                    controller.CurrentViewCodeAfter = sourceCodeAfter;

                    Tuple<string, string> tuple = Tuple.Create(sourceCode, sourceCodeAfter);
                    documents.Add(tuple);
                    controller.FilesOpened[entry.Key.ToUpperInvariant()] = true;
                    dicSelections.Add(entry.Key.ToUpperInvariant(), selectionsList);
                }

                controller.DocumentsBeforeAndAfter = documents;
                controller.EditedLocations = dicSelections;

                controller.Refact();
                //controller.Undo();
                CodeTransformation tregion = MatchesLocationsOnCommit(codeTransformations);
                if (tregion == null)
                {
                    break;
                }

                if (ContainsTRegion(metadataRegions, tregion))
                {
                    return false;
                }
                metadataRegions.Add(tregion.Trans);
            }

            long millAfer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long totalTime = (millAfer - millBefore);
            List<CodeTransformation> transformationsList = JsonUtil<List<CodeTransformation>>.Read(@"transformed_locations.json");

            Log(commit, totalTime, metadataRegions.Count, transformationsList.Count);
    
            FileUtil.WriteToFile(expHome + @"commit\" + commit + @"\edit.t", totalTime.ToString());

            string transformations = FileUtil.ReadFile("transformed_locations.json");
            FileUtil.WriteToFile(expHome + @"commit\" + commit + @"\" + "transformed_locations.json", transformations);
            FileUtil.DeleteFile("transformed_locations.json");
            return true;
        }

        private static Workbook mWorkBook;
        private static Sheets mWorkSheets;
        private static Worksheet mWSheet1;
        private static Application oXL;

        public static void Log(string commit, double time, int exTransformations, int acTrasnformation)
        {
            using (ExcelManager em = new ExcelManager())
            {

                em.Open(@"C:\Users\SPG-04\Documents\Research\Log2.xlsx");

                int empty;
                for (int i = 1;; i++)
                {
                    string comt = em.GetValue("A" + i, Category.Formatted).ToString();
                    if (comt.Equals(commit))
                    {
                        empty = i;
                        break;
                    }
                }
                em.SetValue("H" + empty, time / 1000);
                em.SetValue("I" + empty, exTransformations);
                em.SetValue("J" + empty, acTrasnformation);
                em.Save();
            }
        }

        public static Tuple<string, List<TRegion>> Transform(string source, List<CodeTransformation> transformations, List<TRegion> regions)
        {
            List<TRegion> tRegions = new List<TRegion>();
            int nextStart = 0;
            string sourceCode = source;
            foreach (CodeTransformation item in transformations)
            {
                Tuple<TRegion, TRegion> tregion = GetTRegionShift(regions, item);
                TRegion region = tregion.Item1;
                string transformation = tregion.Item2.Text;

                int start = nextStart + region.Start;
                int end = start + region.Length;
                sourceCode = sourceCode.Substring(0, start) + transformation +
                sourceCode.Substring(end);

                TRegion tr = new TRegion();
                tr.Start = start - 1;
                tr.Length = tregion.Item2.Length + 2;
                tr.Text = tregion.Item2.Text;
                tr.Path = tregion.Item1.Path;
                tRegions.Add(tr);

                nextStart += transformation.Length - region.Length;
            }
            Tuple<string, List<TRegion>> t = Tuple.Create(sourceCode, tRegions);
            return t;
            //return sourceCode;
        }

        public static List<TRegion> ListTransform(List<CodeTransformation> transformations, List<TRegion> regions)
        {
            List<TRegion> regionList = new List<TRegion>();
            foreach (CodeTransformation item in transformations)
            {
                TRegion region = GetTRegion(regions, item);
                regionList.Add(region);
            }
            return regionList;
        }

        private static TRegion GetTRegion(List<TRegion> regions, CodeTransformation codeTransformation)
        {
            foreach (TRegion tr in regions)
            {
                if (codeTransformation.Trans.Equals(tr))
                {
                    return tr;
                }
            }
            return codeTransformation.Location.Region;
        }

        private static Tuple<TRegion, TRegion> GetTRegionShift(List<TRegion> regions, CodeTransformation codeTransformation)
        {
            Tuple<TRegion, TRegion> t;
            foreach (TRegion tr in regions)
            {
                if (codeTransformation.Trans.Equals(tr))
                {
                    TRegion region = new TRegion();
                    region.Start = tr.Start;
                    region.Length = tr.Length - 2;
                    region.Path = tr.Path;
                    region.Text = tr.Text;
                    t = Tuple.Create(codeTransformation.Location.Region, region);
                    return t;
                }
            }
            t = Tuple.Create(codeTransformation.Location.Region, codeTransformation.Location.Region);
            return t;
        }

        private static bool ContainsTRegion(List<TRegion> metadataRegions, CodeTransformation tregion)
        {
            foreach (var location in metadataRegions)
            {
                if (location.Start == tregion.Trans.Start &&
                    location.Length == tregion.Trans.Length &&
                    location.Path.ToUpperInvariant().Equals(tregion.Trans.Path.ToUpperInvariant()))
                {
                    return true;
                }
            }
            return false;
        }

        private static CodeTransformation MatchesLocationsOnCommit(List<CodeTransformation> codeTransformations)
        {
            List<CodeTransformation> transformations = JsonUtil<List<CodeTransformation>>.Read(@"transformed_locations.json");

            foreach (CodeTransformation metadata in codeTransformations)
            {
                bool isFound = false;
                foreach (CodeTransformation transformation in transformations)
                {
                    if (metadata.Location.Region.Equals(transformation.Location.Region))
                    {
                        isFound = true;
                        break;
                    }
                }

                if (!isFound)
                {
                    return metadata;
                }
            }
            return null;
        }
    }
}








