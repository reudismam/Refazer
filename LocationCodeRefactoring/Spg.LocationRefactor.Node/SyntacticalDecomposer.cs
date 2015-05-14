using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ExampleRefactoring.Spg.ExampleRefactoring.Workspace;
using Spg.LocationCodeRefactoring.Controller;
using LocationCodeRefactoring.Spg.LocationRefactor.Location;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace LocationCodeRefactoring.Spg.LocationRefactor.Node
{
    /// <summary>
    /// Syntatical decomposer
    /// </summary>
    public class SyntacticalDecomposer
    {
        /// <summary>
        /// Syntax nodes to be used on filtering
        /// </summary>
        /// <param name="tree">Source code tree</param>
        /// <param name="name">Identifier name</param>
        /// <returns>Syntax nodes to be used on filtering</returns>
        internal static IEnumerable<SyntaxNode> SyntaxNodesWithSemanticModel(SyntaxNode tree, string name)
        {
            //string name = GetIdentifierName();

            if (name == null) return null;//SyntaxNodesWithoutSemanticModel(tree);

            EditorController controller = EditorController.GetInstance();
            Dictionary<string, Dictionary<string, List<TextSpan>>> result = null;
            try
            {
                result = WorkspaceManager.GetInstance()
                    .GetDeclaredReferences(controller.ProjectInformation.ProjectPath,
                        controller.ProjectInformation.SolutionPath, name);
            }
            catch (AggregateException)
            {
                MessageBox.Show("Could not find references for: " + name);
            }

            var referencedSymbols = ReferenceManager.GroupReferenceBySelection(result, controller.SelectedLocations);

            List<SyntaxNode> nodesList = new List<SyntaxNode>();
            Dictionary<string, List<TextSpan>> dictionary;
            if (referencedSymbols.Count == 1)
            {
                dictionary = referencedSymbols.First().Value;
            }
            else
            {
                dictionary = new Dictionary<string, List<TextSpan>>();
                foreach (KeyValuePair<string, Dictionary<string, List<TextSpan>>> symbol in referencedSymbols)
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
                    var nodes = from node in fileTree.GetRoot().DescendantNodesAndSelf()
                        where WithinLcas(node) && WithinSpans(node, fileSpans.Value)
                        select node;
                    nodesList.AddRange(nodes);
                }
            //}
            //else
            //{
                //MessageBox.Show("More than one syntax reference");
            //}

            if (!result.Any()) return null;//return SyntaxNodesWithoutSemanticModel(tree);
            return nodesList;
        }

        /// <summary>
        /// Syntax nodes to be used on filtering
        /// </summary>
        /// <returns>Syntax nodes to be used on filtering</returns>
        internal static IEnumerable<SyntaxNode> SyntaxNodesWithSemanticModel(string name)
        {
            //string name = GetIdentifierName();

            if (name == null) return null;

            EditorController controller = EditorController.GetInstance();
            Dictionary<string, Dictionary<string, List<TextSpan>>> result = WorkspaceManager.GetInstance()
                .GetDeclaredReferences(controller.ProjectInformation.ProjectPath,
                    controller.ProjectInformation.SolutionPath, name);
            var referencedSymbols = ReferenceManager.GroupReferenceBySelection(result, controller.SelectedLocations);

            List<SyntaxNode> nodesList = new List<SyntaxNode>();
            if (referencedSymbols.Count == 1)
            {
                Dictionary<string, List<TextSpan>> dictionary = referencedSymbols.First().Value;
                //for each file with the list of text span reference to the source declaration.
                foreach (KeyValuePair<string, List<TextSpan>> fileSpans in dictionary)
                {
                    SyntaxTree fileTree = CSharpSyntaxTree.ParseFile(fileSpans.Key);
                    var nodes = from node in fileTree.GetRoot().DescendantNodesAndSelf()
                                where WithinLcas(node) && WithinSpans(node, fileSpans.Value)
                                select node;
                    nodesList.AddRange(nodes);
                }
            }

            if (!result.Any()) return null;//return SyntaxNodesWithoutSemanticModel(tree);
            return nodesList;
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
            if(!files.Any()) throw new ArgumentException("Source files cannot be empty.", "files");

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
