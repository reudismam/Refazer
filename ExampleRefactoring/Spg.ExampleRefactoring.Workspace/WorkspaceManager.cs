using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ExampleRefactoring.Spg.ExampleRefactoring.LCS;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;

namespace ExampleRefactoring.Spg.ExampleRefactoring.Workspace
{
    /// <summary>
    /// Manager the workspace
    /// </summary>
    public class WorkspaceManager
    {

        private static WorkspaceManager _instance;
        private readonly Dictionary<Tuple<LCAManager.Node, string>, string> _dictionary;

        private WorkspaceManager()
        {
            _dictionary = new Dictionary<Tuple<LCAManager.Node, string>, string>();
        }

        /// <summary>
        /// Get a singleton instance of WorkspaceManager
        /// </summary>
        /// <returns>Singleton instance</returns>
        public static WorkspaceManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new WorkspaceManager();
            }
            return _instance;
        }

        /// <summary>
        /// Return .cs files in the solution
        /// </summary>
        /// <param name="projectName">Project name</param>
        /// <param name="solutionPath">Solution path</param>
        /// <returns>.cs files</returns>
        public List<Tuple<string, string>> GetSourcesFiles(List<string> projectName, string solutionPath)
        {
            //SymbolTable(solutionPath);
            List<Tuple<string, string>> sourceFiles = new List<Tuple<string, string>>();
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(solutionPath).Result;

            var originalSolution = workspace.CurrentSolution;

            Solution newSolution = originalSolution;
            //GetSemanticModel(projectName, solutionPath);

            foreach (var projectId in originalSolution.ProjectIds)
            {
                var project = newSolution.GetProject(projectId);
                if (/*project.Name.Equals(projectName)*/projectName.Contains(project.Name))
                {
                    //remove
                    //var compilation = project.GetCompilationAsync().Result;
                    //var globalNamespace = compilation.GlobalNamespace;
                    //var diagnostics = compilation.GetDiagnostics();
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

        /// <summary>
        /// Get fully qualified name of a node
        /// </summary>
        /// <param name="projectName">Project name</param>
        /// <param name="solutionPath">Solution path</param>
        /// <param name="node">Node to be analyzed</param>
        /// <param name="name">Name of the identifier</param>
        /// <returns>Fully qualified name of the node</returns>
        public string GetFullyQualifiedName(string projectName, string solutionPath, SyntaxNodeOrToken node, string name)
        {
            Tuple<LCAManager.Node, string> tuple =
                Tuple.Create(new LCAManager.Node(node.Span.Start, node.Span.End, node),
                    node.SyntaxTree.GetText().ToString());
            if (_dictionary.ContainsKey(tuple))
            {
                return _dictionary[tuple];
            }
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(solutionPath).Result;

            var originalSolution = workspace.CurrentSolution;

            Solution newSolution = originalSolution;

            foreach (var projectId in originalSolution.ProjectIds)
            {
                var project = newSolution.GetProject(projectId);
                if (project.Name.Equals(projectName))
                {
                    var compilation = project.GetCompilationAsync().Result;

                    foreach (var documentId in project.DocumentIds)
                    {
                        var document = solution.GetDocument(documentId);

                        SyntaxTree tree;
                        document.TryGetSyntaxTree(out tree);
                        if (tree.GetText().ToString().Equals(node.SyntaxTree.GetText().ToString()))
                        {
                            document.TryGetSyntaxTree(out tree);
                            SemanticModel model2 = compilation.GetSemanticModel(tree);
                            //SymbolFinder.

                            foreach (ISymbol symbol in model2.LookupSymbols(node.SpanStart, null, name))
                            {
                                if (symbol.CanBeReferencedByName && symbol.Name.Contains(name))
                                {
                                    IEnumerable<ISymbol> rlt = SymbolFinder.FindSourceDeclarationsAsync(project, symbol.Name, false).Result;
                                    //                           var rlts = symbol.DeclaringSyntaxReferences;
                                    _dictionary.Add(tuple, symbol.ToDisplayString());
                                    return symbol.ToDisplayString();
                                }
                            }
                        }
                    }
                }
            }
            _dictionary.Add(tuple, null);
            return null;
        }

        /// <summary>
        /// Get fully qualified name of a node
        /// </summary>
        /// <param name="projectName">Project name</param>
        /// <param name="solutionPath">Solution path</param>
        /// <param name="node">Node to be analyzed</param>
        /// <param name="name">Name of the identifier</param>
        /// <returns>Fully qualified name of the node</returns>
        public IEnumerable<ISymbol> GetReferences(string projectName, string solutionPath, SyntaxNodeOrToken node, string name)
        {
            Tuple<LCAManager.Node, string> tuple =
                Tuple.Create(new LCAManager.Node(node.Span.Start, node.Span.End, node),
                    node.SyntaxTree.GetText().ToString());
            if (_dictionary.ContainsKey(tuple))
            {
                //return _dictionary[tuple];
            }
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(solutionPath).Result;

            var originalSolution = workspace.CurrentSolution;

            Solution newSolution = originalSolution;

            foreach (var projectId in originalSolution.ProjectIds)
            {
                var project = newSolution.GetProject(projectId);
                if (project.Name.Equals(projectName))
                {
                    var compilation = project.GetCompilationAsync().Result;

                    foreach (var documentId in project.DocumentIds)
                    {
                        var document = solution.GetDocument(documentId);

                        SyntaxTree tree;
                        document.TryGetSyntaxTree(out tree);
                        if (tree.GetText().ToString().Equals(node.SyntaxTree.GetText().ToString()))
                        {
                            document.TryGetSyntaxTree(out tree);
                            SemanticModel model2 = compilation.GetSemanticModel(tree);
                            //SymbolFinder.

                            foreach (ISymbol symbol in model2.LookupSymbols(node.SpanStart, null, name))
                            {
                                if (symbol.CanBeReferencedByName && symbol.Name.Contains(name))
                                {
                                    IEnumerable<ISymbol> rlt = SymbolFinder.FindSourceDeclarationsAsync(project, symbol.Name, false).Result;
                                    //                           var rlts = symbol.DeclaringSyntaxReferences;
                                    //_dictionary.Add(tuple, symbol.ToDisplayString());
                                    //symbol.ToDisplayString();
                                    return rlt;
                                }
                            }
                        }
                    }
                }
            }
            _dictionary.Add(tuple, null);
            return new List<ISymbol>();
        }

        /// <summary>
        /// Get fully qualified name of a node
        /// </summary>
        /// <param name="projectName">Project name</param>
        /// <param name="solutionPath">Solution path</param>
        /// <param name="name">Name of the identifier</param>
        /// <returns>Fully qualified name of the node</returns>
        public Dictionary<string, Dictionary<string, List<TextSpan>>> GetDeclaredReferences(List<string> projectName, string solutionPath, string name)
        {
            var referenceDictionary = new Dictionary<string, Dictionary<string, List<TextSpan>>>();
            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(solutionPath).Result;

            IEnumerable<ISymbol> sourceDeclarations = new List<ISymbol>();
            try
            {
                sourceDeclarations = SymbolFinder.FindSourceDeclarationsAsync(solution, name, false).Result;
            }
            catch (AggregateException e)
            {
                //MessageBox.Show("Error: " + e.GetBaseException().Message);
            }

            foreach (ISymbol sourceDeclaration in sourceDeclarations)
            { 
                IEnumerable<ReferencedSymbol> references = SymbolFinder.FindReferencesAsync(sourceDeclaration, solution).Result;

                //MessageBox.Show(references.Count() + "");

                var spansDictionary = new Dictionary<string, List<TextSpan>>();
                foreach (ReferencedSymbol reference in references)
                {
                    foreach (ReferenceLocation location in reference.Locations)
                    {
                        SyntaxTree tree = location.Document.GetSyntaxTreeAsync().Result;
                        List<TextSpan> value;
                        if (!spansDictionary.TryGetValue(tree.FilePath.ToUpperInvariant(), out value))
                        {
                            value = new List<TextSpan>();
                            spansDictionary.Add(tree.FilePath.ToUpperInvariant(), value);
                        }
                        value.Add(location.Location.SourceSpan);
                    }
                }

                if (!referenceDictionary.ContainsKey(sourceDeclaration.ToDisplayString()))
                {
                    referenceDictionary.Add(sourceDeclaration.ToDisplayString(), spansDictionary);
                }
                else
                {
                    Console.WriteLine("The key alheady exist on dictionary.");
                }
            }
            return referenceDictionary;
        }

        /// <summary>
        /// Source files in solution on the format source code, source code path
        /// </summary>
        /// <param name="projectName">Name of the project</param>
        /// <param name="solutionPath">Solution path</param>
        /// <returns>List of source file in the solution</returns>
        public List<Tuple<string, string>> SourceFiles(List<string> projectName, string solutionPath)
        {
            //WorkspaceManager manager = new WorkspaceManager();
            List<Tuple<string, string>> sourceFiles = GetSourcesFiles(projectName, solutionPath);
            return sourceFiles;
        }

        //public string GetSemanticModel(string projectName, string solutionPath, SyntaxNodeOrToken node, string name)
        //{
        //    Tuple<LCAManager.Node, string> tuple = Tuple.Create(new LCAManager.Node(node.Span.Start, node.Span.End, node),
        //        node.SyntaxTree.GetText().ToString());

        //    if (dictionary.ContainsKey(tuple))
        //    {
        //        return dictionary[tuple];
        //    }

        //    var workspace = MSBuildWorkspace.Create();
        //    var solution = workspace.OpenSolutionAsync(solutionPath).Result;

        //    var originalSolution = workspace.CurrentSolution;

        //    Solution newSolution = originalSolution;

        //    foreach (var projectId in originalSolution.ProjectIds)
        //    {
        //        var project = newSolution.GetProject(projectId);
        //        if (project.Name.Equals(projectName))
        //        {
        //            //remove
        //            var compilation = project.GetCompilationAsync().Result;
        //            //Document document = null;
        //            //SyntaxTree tree;
        //            //foreach (var documentId in project.DocumentIds)
        //            //{
        //            //    if (!documents.TryGetValue(node.SyntaxTree.ToString(), out document))
        //            //    {
        //            //        document = solution.GetDocument(documentId);
        //            //        document.TryGetSyntaxTree(out tree);
        //            //        if (tree.GetText().ToString().Equals(node.SyntaxTree.GetText().ToString()))
        //            //        {
        //            //            documents.Add(tree.GetText().ToString(), document);
        //            //            break;
        //            //        }
        //            //        documents.Add(tree.GetText().ToString(), document);
        //            //    }
        //            //}

        //            foreach (var documentId in project.DocumentIds)
        //            {
        //                var document = solution.GetDocument(documentId);


        //                //foreach (var document in files)
        //                //    {
        //                //var document = solution.GetDocument(documentId);

        //                SyntaxTree tree;
        //                document.TryGetSyntaxTree(out tree);
        //                if (tree.GetText().ToString().Equals(node.SyntaxTree.GetText().ToString()))
        //                {
        //                    document.TryGetSyntaxTree(out tree);
        //                SemanticModel model2;
        //                document.TryGetSemanticModel(out model2);

        //                foreach (ISymbol symbol in model2.LookupSymbols(node.SpanStart, null, name))
        //                {
        //                    if (symbol.CanBeReferencedByName && symbol.Name.Contains(name))
        //                    {
        //                        var rlt =
        //                            SymbolFinder.FindSourceDeclarationsAsync(solution, symbol.Name, false).Result;
        //                        //                           var rlts = symbol.DeclaringSyntaxReferences;
        //                        dictionary.Add(tuple, symbol.ToDisplayString());
        //                        return symbol.ToDisplayString();
        //                    }
        //                }
        //            }
        //        }
        //        }
        //            //}
        //        //}
        //    }
        //    return null;
        //}

        //private void SymbolTable(string solutionPath)
        //{
        //    var workspace = MSBuildWorkspace.Create();
        //    var solution = workspace.OpenSolutionAsync(solutionPath).Result;
        //}


        //private static void ReportMethods(INamespaceSymbol namespaceSymbol)
        //{
        //    foreach (var type in namespaceSymbol.GetTypeMembers())
        //    {
        //        ReportMethods(type);
        //    }

        //    foreach (var childNs in namespaceSymbol.GetNamespaceMembers())
        //    {
        //        ReportMethods(childNs);
        //    }
        //}

        //private static void ReportMethods(INamedTypeSymbol type)
        //{
        //    foreach (var member in type.GetMembers())
        //    {
        //        Console.WriteLine("Found {0}", member.ToDisplayString());
        //        if (member.CanBeReferencedByName &&
        //            member.Name.Contains("a"))
        //        {
        //            Console.WriteLine("Found {0}", member.ToDisplayString());
        //        }
        //    }

        //    foreach (var nested in type.GetTypeMembers())
        //    {
        //        ReportMethods(nested);
        //    }
        //}
    }
}
