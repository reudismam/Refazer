using System;
using System.Collections.Generic;
using System.Linq;
using ExampleRefactoring.Spg.ExampleRefactoring.Workspace;
using LocationCodeRefactoring.Spg.LocationCodeRefactoring.Controller;
using LocationCodeRefactoring.Spg.LocationRefactor.Location;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Spg.LocationRefactor.Node
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
        /// <returns>Syntax nodes to be used on filtering</returns>
        internal static IEnumerable<SyntaxNode> SyntaxNodes(SyntaxNode tree, string name)
        {
            //string name = GetIdentifierName();

            if (name == null) return SyntaxNodesWithoutSemanticModel(tree);

            EditorController controller = EditorController.GetInstance();
            Dictionary<string, Dictionary<string, List<TextSpan>>> result = WorkspaceManager.GetInstance()
                .GetDeclaredReferences(controller.ProjectInformation.ProjectPath,
                    controller.ProjectInformation.SolutionPath, name);
            var referencedSymbols = RegionManager.GroupReferenceBySelection(result, controller.SelectedLocations);

            List<SyntaxNode> nodesList = new List<SyntaxNode>();
            if (referencedSymbols.Count == 1)
            {
                Dictionary<string, List<TextSpan>> dictionary = referencedSymbols.First().Value;
                //for each file
                foreach (var fileSpans in dictionary)
                {
                    SyntaxTree fileTree = CSharpSyntaxTree.ParseFile(fileSpans.Key);
                    var nodes = from node in fileTree.GetRoot().DescendantNodesAndSelf()
                                where WithinLcas(node) && WithinSpans(node, fileSpans.Value)
                                select node;
                    nodesList.AddRange(nodes);
                }
            }

            if (!result.Any()) return SyntaxNodesWithoutSemanticModel(tree);
            return nodesList;
        }


        /// <summary>
        /// Syntax nodes to be used on filtering
        /// </summary>
        /// <returns>Syntax nodes to be used on filtering</returns>
        internal static IEnumerable<SyntaxNode> SyntaxNodes(string name)
        {
            //string name = GetIdentifierName();

            //if (name == null) return SyntaxNodesWithoutSemanticModel(tree);

            EditorController controller = EditorController.GetInstance();
            Dictionary<string, Dictionary<string, List<TextSpan>>> result = WorkspaceManager.GetInstance()
                .GetDeclaredReferences(controller.ProjectInformation.ProjectPath,
                    controller.ProjectInformation.SolutionPath, name);
            var referencedSymbols = RegionManager.GroupReferenceBySelection(result, controller.SelectedLocations);

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

            //if (!result.Any()) return SyntaxNodesWithoutSemanticModel(tree);
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
