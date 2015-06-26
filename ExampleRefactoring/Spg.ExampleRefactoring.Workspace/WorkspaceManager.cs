using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            List<Tuple<string, string>> sourceFiles = new List<Tuple<string, string>>();
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Solution solution = workspace.OpenSolutionAsync(solutionPath).Result;

            foreach (ProjectId projectId in solution.ProjectIds)
            {
                Project project = solution.GetProject(projectId);
                if (/*project.Name.Equals(projectName)projectName.Contains(project.Name)*/true)
                {
                    foreach (DocumentId documentId in project.DocumentIds)
                    {
                        var document = solution.GetDocument(documentId);
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
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Solution solution = workspace.OpenSolutionAsync(solutionPath).Result;

            foreach (ProjectId projectId in solution.ProjectIds)
            {
                Project project = solution.GetProject(projectId);
                if (/*project.Name.Equals(projectName)*/ true)
                {
                    Compilation compilation = project.GetCompilationAsync().Result;
                    foreach (DocumentId documentId in project.DocumentIds)
                    {
                        var document = solution.GetDocument(documentId);

                        SyntaxTree tree;
                        document.TryGetSyntaxTree(out tree);
                        if (tree.GetText().ToString().Equals(node.SyntaxTree.GetText().ToString()))
                        {
                            document.TryGetSyntaxTree(out tree);
                            SemanticModel model2 = compilation.GetSemanticModel(tree);
                            foreach (ISymbol symbol in model2.LookupSymbols(node.SpanStart, null, name))
                            {
                                if (symbol.CanBeReferencedByName && symbol.Name.Contains(name))
                                {
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
        public Dictionary<string, Dictionary<string, List<TextSpan>>> GetLocalReferences(string projectName, string solutionPath, SyntaxNodeOrToken node, string name)
        {
            var referenceDictionary = new Dictionary<string, Dictionary<string, List<TextSpan>>>();
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Solution solution = workspace.OpenSolutionAsync(solutionPath).Result;

            foreach (ProjectId projectId in solution.ProjectIds)
            {
                Project project = solution.GetProject(projectId);
                Compilation compilation = project.GetCompilationAsync().Result;
                foreach (DocumentId documentId in project.DocumentIds)
                {
                    var document = solution.GetDocument(documentId);
                    SyntaxTree tree;
                    document.TryGetSyntaxTree(out tree);
                    if (tree.GetText().ToString().Equals(node.SyntaxTree.GetText().ToString()))
                    {
                        document.TryGetSyntaxTree(out tree);
                        SemanticModel model2 = compilation.GetSemanticModel(tree);
                        foreach (ISymbol symbol in model2.LookupSymbols(node.SpanStart, null, name))
                        {
                            if (symbol.CanBeReferencedByName && symbol.Name.Contains(name))
                            {
                                IEnumerable<ReferencedSymbol> references = SymbolFinder.FindReferencesAsync(symbol, solution).Result;
                                var spansDictionary = new Dictionary<string, List<TextSpan>>();

                                foreach (ReferencedSymbol reference in references)
                                {
                                    foreach (ReferenceLocation location in reference.Locations)
                                    {
                                        SyntaxTree treeSymbol = location.Document.GetSyntaxTreeAsync().Result;
                                        List<TextSpan> value;
                                        if (!spansDictionary.TryGetValue(treeSymbol.FilePath.ToUpperInvariant(), out value))
                                        {
                                            value = new List<TextSpan>();
                                            spansDictionary.Add(tree.FilePath.ToUpperInvariant(), value);
                                        }
                                        value.Add(location.Location.SourceSpan);
                                    }
                                }

                                if (!referenceDictionary.ContainsKey(symbol.ToDisplayString()))
                                {
                                    referenceDictionary.Add(symbol.ToDisplayString(), spansDictionary);
                                }
                                else
                                {
                                    Console.WriteLine("The key alheady exist on dictionary.");
                                }
                            }
                        }
                    }
                }
            }
            return referenceDictionary;
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
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Solution solution = workspace.OpenSolutionAsync(solutionPath).Result;

            IEnumerable<ISymbol> sourceDeclarations = new List<ISymbol>();
            try
            {
                sourceDeclarations = SymbolFinder.FindSourceDeclarationsAsync(solution, name, false).Result;
            }
            catch (AggregateException)
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
            List<Tuple<string, string>> sourceFiles = GetSourcesFiles(projectName, solutionPath);
            return sourceFiles;
        }
    }
}




