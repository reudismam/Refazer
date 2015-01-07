using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.FindSymbols;

namespace Spg.ExampleRefactoring.Workspace
{
    /// <summary>
    /// Manager the workspace
    /// </summary>
    public class WorkspaceManager
    {

        /// <summary>
        /// Return .cs files in the solution
        /// </summary>
        /// <param name="solutionPath">Solution path</param>
        /// <returns>.cs files</returns>
        public List<Tuple<string, string>> GetSourcesFiles(string solutionPath = "")
        {
            //SymbolTable(solutionPath);
            List<Tuple<string, string>> sourceFiles = new List<Tuple<string, string>>();
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(solutionPath).Result;

            var originalSolution = workspace.CurrentSolution;

            Solution newSolution = originalSolution;

            foreach (var projectId in originalSolution.ProjectIds)
            {
                var project = newSolution.GetProject(projectId);
                //remove
                var compilation = project.GetCompilationAsync().Result;
                var globalNamespace = compilation.GlobalNamespace;
                var diagnostics = compilation.GetDiagnostics();
                //remove
                foreach (var documentId in project.DocumentIds)
                {
                    var document = newSolution.GetDocument(documentId);
                    
                    ////remove
                    //SyntaxTree tree;
                    //document.TryGetSyntaxTree(out tree);
                    //SemanticModel model2 = compilation.GetSemanticModel(tree);
                    ////SemanticModel model;
                    ////document.TryGetSemanticModel(out model);
                    //foreach (ISymbol symbol in model2.LookupSymbols(241))
                    //{
                    //    if (symbol.CanBeReferencedByName && symbol.Name.Equals("a"))
                    //    {
                    //        var rlt = SymbolFinder.FindSourceDeclarationsAsync(project, symbol.Name, false).Result;
                    //        var rlts = symbol.DeclaringSyntaxReferences;
                    //    }
                    //}
                    ////remove

                    StreamReader sr = new
                    StreamReader(document.FilePath);

                    String text = sr.ReadToEnd();

                    //text = text.Replace("\r\n", "\n");
                    Tuple<string, string> tuple = Tuple.Create(text, document.FilePath);
                    sourceFiles.Add(tuple);
                }
            }

            return sourceFiles;
        }

        private void SymbolTable(string solutionPath)
        {
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(solutionPath).Result;
        }

        private static void ReportMethods(INamespaceSymbol namespaceSymbol)
        {
            foreach (var type in namespaceSymbol.GetTypeMembers())
            {
                ReportMethods(type);
            }

            foreach (var childNs in namespaceSymbol.GetNamespaceMembers())
            {
                ReportMethods(childNs);
            }
        }

        private static void ReportMethods(INamedTypeSymbol type)
        {
            foreach (var member in type.GetMembers())
            {
                Console.WriteLine("Found {0}", member.ToDisplayString());
                if (member.CanBeReferencedByName &&
                    member.Name.Contains("a"))
                {
                    Console.WriteLine("Found {0}", member.ToDisplayString());
                }
            }

            foreach (var nested in type.GetTypeMembers())
            {
                ReportMethods(nested);
            }
        }
    }
}

//public void GetProjects(string solutionPath = "")
//{
//    var workspace = MSBuildWorkspace.Create();
//    var solution = workspace.OpenSolutionAsync(solutionPath).Result;

//    var originalSolution = workspace.CurrentSolution;

//    // Declare a variable to store the intermediate solution snapshot at each step.
//    Solution newSolution = originalSolution;

//    foreach (var projectId in originalSolution.ProjectIds)
//    {
//        // Look up the snapshot for the original project in the latest forked solution.
//        var project = newSolution.GetProject(projectId);
//        foreach (var documentId in project.DocumentIds)
//        {
//            // Look up the snapshot for the original document in the latest forked solution.
//            var document = newSolution.GetDocument(documentId);
//            //VersionStamp stamp;
//            //var text = document.TryGetTextVersion(out stamp);

//            StreamReader sr = new
//            StreamReader(document.FilePath);

//            String text = sr.ReadToEnd();

//            text = text.Replace("\r\n", "\n");

//        }


//        /*//var project = solution.Projects.Where(p => p.Name == "ExampleProject").First();
//        var compilation = project.GetCompilationAsync().Result;
//        //var programClass = compilation.GetTypeByMetadataName("HelloWorld.Program");
//        DocumentId id = DocumentId.CreateNewId(project.Id, "HelloWorld.Program");
//        var result = project.GetDocument(id);

//        //var barMethod = programClass.GetMembers("using");
//        //var fooMethod = programClass.GetMembers();

//        //var barResult = SymbolFinder.FindReferencesAsync(barMethod.First(), solution).Result.ToList();
//        //var fooResult = SymbolFinder.FindReferencesAsync(fooMethod.First(), solution).Result.ToList();

//        //Debug.Assert(barResult.First().Locations.Count() == 1);
//        //Debug.Assert(fooResult.First().Locations.Count() == 0);*/
//    }
//}