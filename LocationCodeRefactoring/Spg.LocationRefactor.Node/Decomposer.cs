using System;
using System.Collections.Generic;
using System.Linq;
using ExampleRefactoring.Spg.ExampleRefactoring.Projects;
using ExampleRefactoring.Spg.ExampleRefactoring.Workspace;
using LocationCodeRefactoring.Spg.LocationRefactor.Location;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Spg.LocationCodeRefactoring.Controller;
using Spg.LocationRefactor.TextRegion;

namespace LocationCodeRefactoring.Spg.LocationRefactor.Node
{
    /// <summary>
    /// Decompose element to be used on the filtering estage.
    /// The decomposition can be syntactical or semantical.
    /// </summary>
    public class Decomposer
    {
        /// <summary>
        /// Controler instance
        /// </summary>
        private static readonly EditorController Ctl = EditorController.GetInstance();

        /// <summary>
        /// Reference dictionary
        /// </summary>
        private readonly Dictionary<string, IEnumerable<SyntaxNode>> _dicReferences = new Dictionary<string, IEnumerable<SyntaxNode>>();

        /// <summary>
        /// Singletion instance
        /// </summary>
        private static Decomposer _instance;

        /// <summary>
        /// Constructor
        /// </summary>
        private Decomposer()
        {
        }

        /// <summary>
        /// Return singleton instance
        /// </summary>
        /// <returns>Singleton instance</returns>
        public static Decomposer GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Decomposer();
            }
            return _instance;
        }

        /// <summary>
        /// Initialize singleton instance
        /// </summary>
        public static void Init()
        {
            _instance = null;
        }


        ///// <summary>
        /////   Syntax nodes to be used on filtering
        ///// </summary>
        ///// <param name="name">Identifier name</param>
        ///// <returns>Syntax nodes to be used on filtering</returns>
        //internal IEnumerable<SyntaxNode> SyntaxNodesWithSemanticModel(string name)
        //{
        //    if (name == null) return null;

        //    IEnumerable<SyntaxNode> output;
        //    if (!_dicReferences.TryGetValue(name, out output))
        //    {
        //        Dictionary<string, Dictionary<string, List<TextSpan>>> result = GetReferences(name);

        //        Dictionary<string, Dictionary<string, List<TextSpan>>> referencedSymbols =
        //            ReferenceManager.GroupReferencesBySelection(result, Ctl.SelectedLocations);
        //        List<SyntaxNode> nodesList = new List<SyntaxNode>();
        //        Dictionary<string, List<TextSpan>> dictionary;

        //        if (referencedSymbols.Count == 1)
        //        {
        //            dictionary = referencedSymbols.First().Value;
        //        }
        //        else
        //        {
        //            dictionary = new Dictionary<string, List<TextSpan>>();
        //            foreach (KeyValuePair<string, Dictionary<string, List<TextSpan>>> symbol in result)
        //            {
        //                foreach (KeyValuePair<string, List<TextSpan>> dic in symbol.Value)
        //                {
        //                    if (!dictionary.ContainsKey(dic.Key))
        //                    {
        //                        dictionary.Add(dic.Key, dic.Value);
        //                    }
        //                    dictionary[dic.Key].AddRange(dic.Value);
        //                }
        //            }
        //        }
        //        //for each file
        //        foreach (var fileSpans in dictionary)
        //        {
        //            SyntaxTree fileTree = CSharpSyntaxTree.ParseFile(fileSpans.Key);
        //            var nodes = from node in fileTree.GetRoot().DescendantNodesAndSelf()
        //                        where WithinLcas(node) && WithinSpans(node, fileSpans.Value)
        //                        select node;
        //            nodesList.AddRange(nodes);
        //        }
        //        //}
        //        //else
        //        //{
        //        //MessageBox.Show("More than one syntax reference");
        //        //}

        //        if (!result.Any() || !nodesList.Any())
        //        {
        //            //return null; //return SyntaxNodesWithoutSemanticModel(tree);
        //            _dicReferences.Add(name, null);
        //        }
        //        //return nodesList;
        //        else
        //        {
        //            _dicReferences.Add(name, nodesList);
        //        }
        //    }
        //    return _dicReferences[name];
        //}

        /// <summary>
        ///   Syntax nodes to be used on filtering
        /// </summary>
        /// <param name="name">Identifier name</param>
        /// <returns>Syntax nodes to be used on filtering</returns>
        internal IEnumerable<SyntaxNode> SyntaxNodesWithSemanticModel(Tuple<string, SyntaxNodeOrToken> name)
        {
            if (name == null) return null;

            IEnumerable<SyntaxNode> output;
            if (!_dicReferences.TryGetValue(name.Item1, out output))
            {
                Dictionary<string, Dictionary<string, List<TextSpan>>> result = GetReferences(name);

                Dictionary<string, Dictionary<string, List<TextSpan>>> referencedSymbols =
                    ReferenceManager.GroupReferencesBySelection(result, Ctl.SelectedLocations);
                List<SyntaxNode> nodesList = new List<SyntaxNode>();
                Dictionary<string, List<TextSpan>> dictionary;

                if (referencedSymbols.Count == 1)
                {
                    dictionary = referencedSymbols.First().Value;
                }
                else
                {
                    dictionary = new Dictionary<string, List<TextSpan>>();
                    foreach (KeyValuePair<string, Dictionary<string, List<TextSpan>>> symbol in result)
                    {
                        foreach (KeyValuePair<string, List<TextSpan>> dic in symbol.Value)
                        {
                            if (!dictionary.ContainsKey(dic.Key))
                            {
                                dictionary.Add(dic.Key, dic.Value);
                            }
                            dictionary[dic.Key].AddRange(dic.Value);
                        }
                    }
                }
                //for each file
                foreach (var fileSpans in dictionary)
                {
                    SyntaxTree fileTree = CSharpSyntaxTree.ParseFile(fileSpans.Key);
                    var spans = fileSpans;
                    var nodes = from node in fileTree.GetRoot().DescendantNodesAndSelf()
                                where WithinLcas(node) && WithinSpans(node, spans.Value)
                                select node;
                    nodesList.AddRange(nodes);
                }
                //}
                //else
                //{
                //MessageBox.Show("More than one syntax reference");
                //}

                if (!result.Any() || !nodesList.Any())
                {
                    //return null; //return SyntaxNodesWithoutSemanticModel(tree);
                    _dicReferences.Add(name.Item1, null);
                }
                //return nodesList;
                else
                {
                    _dicReferences.Add(name.Item1, nodesList);
                }
            }
            return _dicReferences[name.Item1];
        }

        ///// <summary>
        ///// Generates references on the format: key referenced type, and values dictionary of referencee file and list of referecees.
        ///// </summary>
        ///// <param name="name">Name to perform look up</param>
        ///// <returns>Generates references on the format: key referenced type, and values dictionary of referencee file and list of referecees.</returns>
        //internal static Dictionary<string, Dictionary<string, List<TextSpan>>> GetReferences(string name)
        //{
        //    try
        //    {
        //        WorkspaceManager wsManager = WorkspaceManager.GetInstance();
        //        ProjectInformation pjInfo = Ctl.ProjectInformation;
        //        Dictionary<string, Dictionary<string, List<TextSpan>>> result = wsManager.GetDeclaredReferences(pjInfo.ProjectPath, pjInfo.SolutionPath, name);
        //        return result;
        //    }
        //    catch (AggregateException)
        //    {
        //        Console.WriteLine("Could not find references for: " + name);
        //    }

        //    return new Dictionary<string, Dictionary<string, List<TextSpan>>>();
        //}


        /// <summary>
        /// Generates references on the format: key referenced type, and values dictionary of referencee file and list of referecees.
        /// </summary>
        /// <param name="name">Name to perform look up</param>
        /// <returns>Generates references on the format: key referenced type, and values dictionary of referencee file and list of referecees.</returns>
        internal static Dictionary<string, Dictionary<string, List<TextSpan>>> GetReferences(Tuple<string, SyntaxNodeOrToken> name)
        {
            try
            {
                WorkspaceManager wsManager = WorkspaceManager.GetInstance();
                ProjectInformation pjInfo = Ctl.ProjectInformation;
                EditorController controller = EditorController.GetInstance();
           
                Dictionary<string, List<TRegion>> files = RegionManager.GetInstance().GroupRegionBySourceFile(controller.SelectedLocations);
                Dictionary<string, Dictionary<string, List<TextSpan>>> result = new Dictionary<string, Dictionary<string, List<TextSpan>>>();
                if (files.Count == 1)
                {
                    result = wsManager.GetLocalReferences(pjInfo.ProjectPath.First(), pjInfo.SolutionPath, name.Item2, name.Item1);
                }

                if (!result.Any())
                {
                    result = wsManager.GetDeclaredReferences(pjInfo.ProjectPath, pjInfo.SolutionPath, name.Item1);
                }
                return result;
            }
            catch (AggregateException)
            {
                Console.WriteLine("Could not find references for: " + name);
            }

            return new Dictionary<string, Dictionary<string, List<TextSpan>>>();
        }

        /// <summary>
        /// Syntax nodes without semantical model
        /// </summary>
        /// <param name="tree">Syntax node root</param>
        /// <returns>Syntax nodes without semantical model</returns>
        internal static IEnumerable<SyntaxNode> SyntaxNodesWithoutSemanticModel(SyntaxNode tree)
        {
            if (tree == null) throw new ArgumentNullException("tree");

            var nodes = from node in tree.DescendantNodesAndSelf()
                        where WithinLcas(node)
                        select node;
            return nodes;
        }

        /// <summary>
        /// Syntax nodes without semantical model
        /// </summary>
        /// <param name="files">List of source files on the format (sourceCode, sourcePath)</param>
        /// <returns>Syntax nodes without semantical model</returns>
        internal static IEnumerable<SyntaxNode> SyntaxNodesWithoutSemanticModel(List<Tuple<string, string>> files)
        {
            if (files == null) throw new ArgumentNullException("files");
            if (!files.Any()) throw new ArgumentException("Source files cannot be empty.", "files");

            List<SyntaxNode> listNodes = new List<SyntaxNode>();
            foreach (Tuple<string, string> fileTuple in files)
            {
                SyntaxTree tree = CSharpSyntaxTree.ParseFile(fileTuple.Item2);

                var nodes = from node in tree.GetRoot().DescendantNodesAndSelf()
                            where WithinLcas(node)
                            select node;
                listNodes.AddRange(nodes);
            }
            return listNodes;
        }

        /// <summary>
        /// True if node intersects one of the spans
        /// </summary>
        /// <param name="node">Node to be analyzed</param>
        /// <param name="spans">List of spans</param>
        /// <returns>True is node intersects one of the spans</returns>
        internal static bool WithinSpans(SyntaxNode node, List<TextSpan> spans)
        {
            if (node == null) throw new ArgumentNullException("node");
            if (spans == null) throw new ArgumentNullException("spans");
            if (!spans.Any()) throw new ArgumentException("Spans cannot by empty", "spans");

            foreach (var entry in spans)
            {
                if (node.Span.IntersectsWith(entry)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// True if node is of the same type of one of the lca of the region selection by the user
        /// </summary>
        /// <param name="node">Syntax node to be analyzed</param>
        /// <returns>True if node is of the same type of one of the lca of the region selection by the user</returns>
        internal static bool WithinLcas(SyntaxNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            foreach (var lca in EditorController.GetInstance().Lcas)
            {
                if (node.IsKind(lca.CSharpKind()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
