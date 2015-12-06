using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Spg.ExampleRefactoring.LCS;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Spg.LocationRefactoring.Tok;
using Microsoft.CodeAnalysis.CSharp;

namespace Spg.ExampleRefactoring.Workspace
{
    /// <summary>
    /// Manager the workspace
    /// </summary>
    public class WorkspaceManager
    {

        private static WorkspaceManager _instance;
        private readonly Dictionary<string, SemanticModel> _dictionary;

        private Solution solutionInstance;

        private WorkspaceManager()
        {
            _dictionary = new Dictionary<string, SemanticModel>();
        }

        /// <summary>
        /// Init method
        /// </summary>
        public static void Init()
        {
            _instance = null;
        }

        private Solution GetWorkSpace(string solutionPath)
        {
            Console.WriteLine("Opening solution: " + solutionPath);
            if (solutionInstance == null)
            {
                MSBuildWorkspace workspace = MSBuildWorkspace.Create();
                solutionInstance = workspace.OpenSolutionAsync(solutionPath).Result;
            }
            Console.WriteLine("Solution opened.");
            return solutionInstance;  
        }

        public void SetWorkSpace(Microsoft.CodeAnalysis.Workspace workspace)
        {
            solutionInstance = workspace.CurrentSolution;
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
            Solution solution = GetWorkSpace(solutionPath);

            foreach (ProjectId projectId in solution.ProjectIds)
            {
                Project project = solution.GetProject(projectId);

                if (!project.FilePath.ToUpperInvariant().EndsWith(".CSPROJ")) { continue; } // execute only if the project if C#

                if (/*project.Name.Equals(projectName)projectName.Contains(project.Name)*/true)
                {
                    foreach (DocumentId documentId in project.DocumentIds)
                    {
                        var document = solution.GetDocument(documentId);
                        if (document.FilePath.ToUpperInvariant().EndsWith(".CS"))
                        {
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
            }
            //foreach (var file in sourceFiles)
            //{
            //    if (file.Item2.ToUpperInvariant().Contains("FaultyAnalyzer.cs".ToUpperInvariant()))
            //    {
            //        MessageBox.Show("Reudismam");
            //    }
            //}
            return sourceFiles;
        }

        ///// <summary>
        ///// Get fully qualified name of a node
        ///// </summary>
        ///// <param name="projectName">Project name</param>
        ///// <param name="solutionPath">Solution path</param>
        ///// <param name="node">Node to be analyzed</param>
        ///// <param name="name">Name of the identifier</param>
        ///// <returns>Fully qualified name of the node</returns>
        //public string GetFullyQualifiedName(string projectName, string solutionPath, SyntaxNodeOrToken node, string name)
        //{
        //    //Tuple<LCAManager.Node, string> tuple =
        //    //    Tuple.Create(new LCAManager.Node(node.Span.Start, node.Span.End, node),
        //    //        node.SyntaxTree.GetText().ToString());
        //    //if (_dictionary.ContainsKey(tuple))
        //    //{
        //    //    return _dictionary[tuple];
        //    //}
        //    Solution solution = GetWorkSpace(solutionPath);

        //    foreach (ProjectId projectId in solution.ProjectIds)
        //    {
        //        Project project = solution.GetProject(projectId);
        //        //if (/*project.Name.Equals(projectName)*/ true)
        //        //{
        //        //    Compilation compilation = project.GetCompilationAsync().Result;
        //        //    foreach (DocumentId documentId in project.DocumentIds)
        //        //    {
        //        //        var document = solution.GetDocument(documentId);

        //        //        SyntaxTree tree;
        //        //        document.TryGetSyntaxTree(out tree);
        //        //        if (tree.GetText().ToString().Equals(node.SyntaxTree.GetText().ToString()))
        //        //        {
        //        //            document.TryGetSyntaxTree(out tree);
        //        //            SemanticModel model2 = compilation.GetSemanticModel(tree);
        //        //            foreach (ISymbol symbol in model2.LookupSymbols(node.SpanStart, null, name))
        //        //            {
        //        //                if (symbol.CanBeReferencedByName && symbol.Name.Contains(name))
        //        //                {
        //        //                    _dictionary.Add(tuple, symbol.ToDisplayString());
        //        //                    return symbol.ToDisplayString();
        //        //                }
        //        //            }
        //        //        }
        //        //    }
        //        //}
        //    }
        //    //_dictionary.Add(tuple, null);
        //    return null;
        //}

        ///// <summary>
        ///// Get fully qualified name of a node
        ///// </summary>
        ///// <param name="projectName">Project name</param>
        ///// <param name="solutionPath">Solution path</param>
        ///// <param name="node">Node to be analyzed</param>
        ///// <param name="name">Name of the identifier</param>
        ///// <returns>Fully qualified name of the node</returns>
        //public string GetFullyQualifiedName2(string projectName, string solutionPath, SyntaxNodeOrToken node, string name)
        //{
        //    try
        //    {
        //        Solution solution = GetWorkSpace(solutionPath);

        //        SemanticModel smodel = null;
        //        if (_dictionary.ContainsKey(node.SyntaxTree.GetText().ToString()))
        //        {
        //            smodel = _dictionary[node.SyntaxTree.GetText().ToString()];
        //        }
        //        else
        //        {
        //            foreach (ProjectId projectId in solution.ProjectIds)
        //            {
        //                Project project = solution.GetProject(projectId);
        //                try
        //                {
        //                    Compilation compilation = project.GetCompilationAsync().Result;

        //                    foreach (DocumentId documentId in project.DocumentIds)
        //                    {
        //                        var document = solution.GetDocument(documentId);
        //                        SyntaxTree tree;
        //                        document.TryGetSyntaxTree(out tree);
        //                        if (tree.GetText().ToString().Equals(node.SyntaxTree.GetText().ToString()))
        //                        {
        //                            document.TryGetSyntaxTree(out tree);
        //                            smodel = compilation.GetSemanticModel(tree);
        //                            _dictionary.Add(node.SyntaxTree.GetText().ToString(), smodel);
        //                        }
        //                    }
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e.Message);
        //                }
        //            }
        //        }

        //        foreach (ISymbol symbol in smodel.LookupSymbols(node.SpanStart, null, name))
        //        {
        //            if (symbol.CanBeReferencedByName && symbol.Name.Contains(name))
        //            {
        //                return symbol.ToDisplayString();
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }

        //    //_dictionary.Add(tuple, null);
        //    return null;
        //}

        /// <summary>
        /// Get fully qualified name of a node
        /// </summary>
        /// <param name="solutionPath">Solution path</param>
        /// <param name="node">Node to be analyzed</param>
        /// <returns>Fully qualified name of the node</returns>
        public DynType GetFullyQualifiedName(string solutionPath, SyntaxNodeOrToken token)
        {
            var referenceDictionary = new Dictionary<string, Dictionary<string, List<TextSpan>>>();
            Solution solution = GetWorkSpace(solutionPath);

            SemanticModel smodel = null;
            if (_dictionary.ContainsKey(token.SyntaxTree.FilePath.ToUpperInvariant()))
            {
                smodel = _dictionary[token.SyntaxTree.FilePath.ToUpperInvariant()];
            }
            else
            {
                foreach (ProjectId projectId in solution.ProjectIds)
                {
                    Project project = solution.GetProject(projectId);
                    Compilation compilation = null;

                    if (!project.FilePath.ToUpperInvariant().EndsWith(".CSPROJ")) { continue; } // execute only if the project if C#

                    try
                    {
                        compilation = project.GetCompilationAsync().Result;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Could not load project: " + project.Name);
                        continue;
                    }

                    foreach (DocumentId documentId in project.DocumentIds)
                    {
                        var document = solution.GetDocument(documentId);
                        SyntaxTree tree;
                        document.TryGetSyntaxTree(out tree);
                        if (tree.FilePath.ToUpperInvariant().Equals(token.SyntaxTree.FilePath.ToUpperInvariant()))
                        {
                            document.TryGetSyntaxTree(out tree);
                            smodel = compilation.GetSemanticModel(tree);
                            if (!_dictionary.ContainsKey((tree.FilePath.ToUpperInvariant()))){
                                _dictionary.Add(tree.FilePath.ToUpperInvariant(), smodel);
                            }
                        }
                    }
                }
            }

            DynType dynType = null;
            if (!token.IsKind(SyntaxKind.IdentifierToken))
            { 
                if (token.IsKind(SyntaxKind.StringLiteralToken))
                {
                    dynType = new DynType(token.ToFullString(), DynType.STRING);
                }
                else
                {
                    dynType = new DynType(token.ToFullString(), DynType.NUMBER);
                }
                return dynType;
            }

            SyntaxNode snode = null;
            if (smodel != null)
            {
                snode = smodel.SyntaxTree.GetRoot().FindNode(token.Parent.Span);
            }
            if (snode != null)
            {
                SymbolInfo symbolInfo = smodel.GetSymbolInfo(snode);
                if (symbolInfo.Symbol != null)
                {

                    string toDisplayString = symbolInfo.Symbol.ToDisplayString();
                    dynType = new DynType(toDisplayString, DynType.FULLNAME);
                    dynType.symbol = symbolInfo.Symbol;
                    return dynType;
                }
                else
                {
                    Console.WriteLine("Symbol was not found for node: " + snode.ToString());
                    dynType = new DynType(snode.ToString(), DynType.STRING);
                    return dynType;
                }
            }
            else
            {
                dynType = new DynType(token.ToString(), DynType.STRING);
                return dynType;
            }
        }

        /// <summary>
        /// Get fully qualified name of a node
        /// </summary>
        /// <param name="solutionPath">Solution path</param>
        /// <param name="dynToken">Node to be analyzed</param>
        /// <returns>Fully qualified name of the node</returns>
        public Dictionary<string, Dictionary<string, List<TextSpan>>> GetLocalReferences(string projectName, string solutionPath, DymToken dynToken)
        {
            SyntaxNode node = dynToken.token.Parent;
            var referenceDictionary = new Dictionary<string, Dictionary<string, List<TextSpan>>>();
            Solution solution = GetWorkSpace(solutionPath);

            SemanticModel smodel = null;
            if (_dictionary.ContainsKey(node.SyntaxTree.GetText().ToString()))
            {
                smodel = _dictionary[node.SyntaxTree.GetText().ToString()];
            }
            else
            {
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
                            smodel = compilation.GetSemanticModel(tree);
                            _dictionary.Add(tree.GetText().ToString(), smodel);
                        }

                    }
                }
            }

            SyntaxNode snode = null;
            if (smodel != null)
            {
                snode = smodel.SyntaxTree.GetRoot().FindNode(node.Span);
            }

            if (snode != null && snode is IdentifierNameSyntax)
            {
                SymbolInfo symbolInfo = smodel.GetSymbolInfo(snode);
                var references = SymbolFinder.FindReferencesAsync(symbolInfo.Symbol, solution).Result;

                var spansDictionary = new Dictionary<string, List<TextSpan>>();
                foreach (ReferencedSymbol reference in references)
                {
                    foreach (ReferenceLocation location in reference.Locations)
                    {
                        SyntaxTree treeSymbol = location.Document.GetSyntaxTreeAsync().Result;
                        List<TextSpan> values;
                        if (!spansDictionary.TryGetValue(treeSymbol.FilePath.ToUpperInvariant(), out values))
                        {
                            values = new List<TextSpan>();
                            spansDictionary.Add(treeSymbol.FilePath.ToUpperInvariant(), values);
                        }
                        values.Add(location.Location.SourceSpan);
                    }

                    if (!referenceDictionary.ContainsKey(dynToken.dynType.fullName))
                    {
                        referenceDictionary.Add(dynToken.dynType.fullName, spansDictionary);
                    }
                    else
                    {
                        Console.WriteLine("The key alheady exist on dictionary.");
                    }
                }
            }
            return referenceDictionary;
        }

        ///// <summary>
        ///// Get fully qualified name of a node
        ///// </summary>
        ///// <param name="projectName">Project name</param>
        ///// <param name="solutionPath">Solution path</param>
        ///// <param name="node">Node to be analyzed</param>
        ///// <param name="dymToken">Name of the identifier</param>
        ///// <returns>Fully qualified name of the node</returns>
        //public Dictionary<string, Dictionary<string, List<TextSpan>>> GetLocalReferences(string projectName, string solutionPath, DymToken dymToken)
        //{
        //    var referenceDictionary = new Dictionary<string, Dictionary<string, List<TextSpan>>>();
        //    Solution solution = GetWorkSpace(solutionPath);

        //    //SemanticModel smodel = null;
        //    //if (_dictionary.ContainsKey(dymToken.token.SyntaxTree.GetText().ToString()))
        //    //{
        //    //    smodel = _dictionary[dymToken.token.SyntaxTree.GetText().ToString()];
        //    //}
        //    //else
        //    //{

        //    foreach (ProjectId projectId in solution.ProjectIds)
        //    {
        //        Project project = solution.GetProject(projectId);
        //        if (project.Name.Equals(projectName))
        //        {

        //            Compilation compilation = project.GetCompilationAsync().Result;

        //            ISymbol typeOfInterest = compilation.GetTypeByMetadataName(dymToken.dynType.fullName);
        //            var references = SymbolFinder.FindReferencesAsync(typeOfInterest, solution).Result;

        //            var spansDictionary = new Dictionary<string, List<TextSpan>>();
        //            foreach (ReferencedSymbol reference in references)
        //            {
        //                foreach (ReferenceLocation location in reference.Locations)
        //                {
        //                    SyntaxTree treeSymbol = location.Document.GetSyntaxTreeAsync().Result;
        //                    List<TextSpan> values;
        //                    if (!spansDictionary.TryGetValue(treeSymbol.FilePath.ToUpperInvariant(), out values))
        //                    {
        //                        values = new List<TextSpan>();
        //                        spansDictionary.Add(treeSymbol.FilePath.ToUpperInvariant(), values);
        //                    }
        //                    values.Add(location.Location.SourceSpan);
        //                }

        //                if (!referenceDictionary.ContainsKey(dymToken.dynType.fullName))
        //                {
        //                    referenceDictionary.Add(dymToken.dynType.fullName, spansDictionary);
        //                }
        //                else
        //                {
        //                    Console.WriteLine("The key alheady exist on dictionary.");
        //                }
        //            }
        //        }

        //        //foreach (DocumentId documentId in project.DocumentIds)
        //        //{
        //        //    var document = solution.GetDocument(documentId);
        //        //    SyntaxTree tree;
        //        //    document.TryGetSyntaxTree(out tree);
        //        //    if (tree.GetText().ToString().Equals(dymToken.token.SyntaxTree.GetText().ToString()))
        //        //    {
        //        //        document.TryGetSyntaxTree(out tree);
        //        //        SemanticModel model2 = compilation.GetSemanticModel(tree);

        //        //        foreach (ISymbol symbol in model2.LookupSymbols(node.SpanStart, null, dymToken))
        //        //        {
        //        //            if (symbol.CanBeReferencedByName && symbol.Name.Contains(dymToken))
        //        //            {
        //        //                IEnumerable<ReferencedSymbol> references = SymbolFinder.FindReferencesAsync(symbol, solution).Result;
        //        //                var spansDictionary = new Dictionary<string, List<TextSpan>>();

        //        //                foreach (ReferencedSymbol reference in references)
        //        //                {
        //        //                    foreach (ReferenceLocation location in reference.Locations)
        //        //                    {
        //        //                        SyntaxTree treeSymbol = location.Document.GetSyntaxTreeAsync().Result;
        //        //                        List<TextSpan> value;
        //        //                        if (!spansDictionary.TryGetValue(treeSymbol.FilePath.ToUpperInvariant(), out value))
        //        //                        {
        //        //                            value = new List<TextSpan>();
        //        //                            spansDictionary.Add(treeSymbol.FilePath.ToUpperInvariant(), value);
        //        //                        }
        //        //                        value.Add(location.Location.SourceSpan);
        //        //                    }
        //        //                }

        //        //                if (!referenceDictionary.ContainsKey(symbol.ToDisplayString()))
        //        //                {
        //        //                    referenceDictionary.Add(symbol.ToDisplayString(), spansDictionary);
        //        //                }
        //        //                else
        //        //                {
        //        //                    Console.WriteLine("The key alheady exist on dictionary.");
        //        //                }
        //        //            }
        //        //        }
        //        //    }
        //        //}
        //        //}
        //    }
        //    return referenceDictionary;
        //}

        /// <summary>
        /// Get fully qualified name of a node
        /// </summary>
        /// <param name="projectName">Project name</param>
        /// <param name="solutionPath">Solution path</param>
        /// <param name="node">Node to be analyzed</param>
        /// <param name="name">Name of the identifier</param>
        /// <returns>Fully qualified name of the node</returns>
        public Dictionary<string, List<TextSpan>> GetErrorSpans(List<string> projectName, string solutionPath, List<string> docPaths)
        {
            Solution solution = GetWorkSpace(solutionPath);

            Dictionary<string, List<TextSpan>> dictionary = new Dictionary<string, List<TextSpan>>();
            foreach (ProjectId projectId in solution.ProjectIds)
            {
                Project project = solution.GetProject(projectId);
                Compilation compilation = project.GetCompilationAsync().Result;

                foreach (DocumentId documentId in project.DocumentIds)
                {
                    var document = solution.GetDocument(documentId);
                    SyntaxTree tree;
                    document.TryGetSyntaxTree(out tree);
                    SemanticModel smodel = compilation.GetSemanticModel(tree);
                    if (docPaths.Contains(document.FilePath.ToUpperInvariant()))
                    {
                        document.TryGetSyntaxTree(out tree);

                        if (tree == null)
                        {
                            throw new Exception("Document not found on this project");
                        }

                        List<TextSpan> spans = GetErrorSpans(smodel);
                        dictionary.Add(document.FilePath.ToUpperInvariant(), spans);
                    }
                }
            }
            return dictionary;
        }


        protected List<TextSpan> GetErrorSpans(SemanticModel tree)
        {
            if (tree == null) throw new ArgumentNullException(nameof(tree));

            List<TextSpan> spans = new List<TextSpan>();
            string strDetail = "";

            if (!tree.GetDiagnostics().Any())
            {
                strDetail += "Diagnostic test passed!!!";
            }

            Diagnostic obj = null;
            for (int i = 0; i < tree.GetDiagnostics().Count(); i++)
            {
                obj = tree.GetDiagnostics().ElementAt(i);
                strDetail += "<b>" + (i + 1) + ". Info: </b>" + obj;
                strDetail += " <b>Warning Level: </b>" + obj.WarningLevel;
                strDetail += " <b>Severity Level: </b>" + obj.Severity + "<br/>";
                strDetail += " <b>Location: </b>" + obj.Location.Kind;
                strDetail += " <b>Character at: </b>" + obj.Location.GetLineSpan().StartLinePosition.Character;
                strDetail += " <b>On Line: </b>" + obj.Location.GetLineSpan().StartLinePosition.Line;
                strDetail += "</br>";
                spans.Add(obj.Location.SourceSpan);
            }

            Console.WriteLine(strDetail);
            return spans;
        }

        /// <summary>
        /// Get fully qualified name of a node
        /// </summary>
        /// <param name="projectName">Project name</param>
        /// <param name="solutionPath">Solution path</param>
        /// <param name="name">Name of the identifier</param>
        /// <returns>Fully qualified name of the node</returns>
        public Dictionary<string, Dictionary<string, List<TextSpan>>> GetDeclaredReferences(List<string> projectName, string solutionPath, DymToken name)
        {
            var referenceDictionary = new Dictionary<string, Dictionary<string, List<TextSpan>>>();
            Solution solution = GetWorkSpace(solutionPath);

            IEnumerable<ISymbol> sourceDeclarations = new List<ISymbol>();
            try
            {
                sourceDeclarations = SymbolFinder.FindSourceDeclarationsAsync(solution, name.dynType.fullName, false).Result;
            }
            catch (AggregateException e)
            {
                Console.WriteLine("Error: " + e.GetBaseException().Message);
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






