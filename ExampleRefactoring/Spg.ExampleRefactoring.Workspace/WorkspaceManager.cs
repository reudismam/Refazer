using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace ExampleRefactoring.Spg.ExampleRefactoring.Workspace
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
        public List<Tuple<string, string>> GetSourcesFiles(string projectName, string solutionPath)
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
                if (project.Name.Equals(projectName))
                {
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

                        try
                        {
                            StreamReader sr = new StreamReader(document.FilePath);
                            string text = sr.ReadToEnd();

                            Tuple<string, string> tuple = Tuple.Create(text, document.FilePath);
                            sourceFiles.Add(tuple);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Could not load document on the path: " + document.FilePath);
                        }
                    }
                }
            }

            return sourceFiles;
        }

        private void SymbolTable(string solutionPath)
        {
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(solutionPath).Result;
        }

        /// <summary>
        /// Source files in solution on the format source code, source code path
        /// </summary>
        /// <param name="solutionPath">Solution path</param>
        /// <returns>List of source file in the solution</returns>
        public List<Tuple<string, string>> SourceFiles(string projectName, string solutionPath)
        {
            //WorkspaceManager manager = new WorkspaceManager();
            List<Tuple<string, string>> sourceFiles = GetSourcesFiles(projectName, solutionPath);
            return sourceFiles;
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
