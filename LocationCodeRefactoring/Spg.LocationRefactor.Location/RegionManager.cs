using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ExampleRefactoring.Spg.ExampleRefactoring.Bean;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Bean;
using Spg.ExampleRefactoring.LCS;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Controller;
using Spg.LocationRefactor.Node;
using Spg.LocationRefactor.TextRegion;
using Spg.LocationRefactor.Transform;

namespace Spg.LocationRefactor.Location
{
    /// <summary>
    /// Strategy
    /// </summary>
    public class RegionManager
    {
        private readonly Dictionary<SelectionInfo, List<SyntaxNode>> _computed;

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static RegionManager _instance;

        /// <summary>
        /// Return a new singleton instance
        /// </summary>
        private RegionManager()
        {
            _computed = new Dictionary<SelectionInfo, List<SyntaxNode>>();
        }

        /// <summary>
        /// Create a new instance
        /// </summary>
        public static void Init()
        {
            _instance = null;
        }

        /// <summary>
        /// Return the singleton instance
        /// </summary>
        /// <returns>Singleton instance</returns>
        public static RegionManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new RegionManager();
            }
            return _instance;
        }

        /// <summary>
        /// Group region by source file
        /// </summary>
        /// <param name="list">List of no grouped regions</param>
        /// <returns>Regions grouped by source file</returns>
        public Dictionary<string, List<TRegion>> GroupRegionBySourceFile(List<TRegion> list)
        {
            Dictionary<string, List<TRegion>> dic = new Dictionary<string, List<TRegion>>();
            foreach (var item in list)
            {
                List<TRegion> value;
                if (!dic.TryGetValue(item.Parent.Text, out value))
                {
                    value = new List<TRegion>();
                    dic[item.Parent.Text] = value;
                }

                dic[item.Parent.Text].Add(item);
            }

            return dic;
        }

        /// <summary>
        /// Group region by source file
        /// </summary>
        /// <param name="list">List of no grouped regions</param>
        /// <returns>Regions grouped by source file</returns>
        public Dictionary<string, List<ListNode>> GroupRegionBySourceFile(List<ListNode> list)
        {
            Dictionary<string, List<ListNode>> dic = new Dictionary<string, List<ListNode>>();
            foreach (var item in list)
            {
                List<ListNode> value;
                if (!dic.TryGetValue(item.List.First().SyntaxTree.GetText().ToString(), out value))
                {
                    value = new List<ListNode>();
                    dic[item.List.First().SyntaxTree.GetText().ToString()] = value;
                }

                dic[item.List.First().SyntaxTree.GetText().ToString()].Add(item);
            }

            return dic;
        }

        /// <summary>
        /// Group region by source file
        /// </summary>
        /// <param name="list">List of no grouped regions</param>
        /// <returns>Regions grouped by source file</returns>
        public Dictionary<string, List<TRegion>> GroupRegionBySourcePath(List<TRegion> list)
        {
            Dictionary<string, List<TRegion>> dic = new Dictionary<string, List<TRegion>>();
            foreach (var item in list)
            {
                List<TRegion> value;
                if (!dic.TryGetValue(item.Path, out value))
                {
                    value = new List<TRegion>();
                    dic[item.Path] = value;
                }

                dic[item.Path].Add(item);
            }

            return dic;
        }

        /// <summary>
        /// Group region by source file
        /// </summary>
        /// <param name="list">List of no grouped regions</param>
        /// <returns>Regions grouped by source file</returns>
        public Dictionary<string, List<CodeTransformation>> GroupTransformationsBySourcePath(List<CodeTransformation> list)
        {
            Dictionary<string, List<CodeTransformation>> dic = new Dictionary<string, List<CodeTransformation>>();
            foreach (var item in list)
            {
                List<CodeTransformation> value;
                if (!dic.TryGetValue(item.Location.SourceClass, out value))
                {
                    value = new List<CodeTransformation>();
                    dic[item.Location.SourceClass] = value;
                }

                dic[item.Location.SourceClass].Add(item);
            }

            return dic;
        }

        /// <summary>
        /// Group location by source file
        /// </summary>
        /// <param name="list">Location list</param>
        /// <returns>Locations by source file</returns>
        public Dictionary<string, List<CodeLocation>> GroupLocationsBySourceFile(List<CodeLocation> list)
        {
            Dictionary<string, List<CodeLocation>> dic = new Dictionary<string, List<CodeLocation>>();
            foreach (var item in list)
            {
                List<CodeLocation> value;
                if (!dic.TryGetValue(item.SourceClass.ToUpperInvariant(), out value))
                {
                    value = new List<CodeLocation>();
                    dic[item.SourceClass.ToUpperInvariant()] = value;
                }

                dic[item.SourceClass.ToUpperInvariant()].Add(item);
            }

            return dic;
        }

        ///// <summary>
        ///// Group location by source path
        ///// </summary>
        ///// <param name="list">Location list</param>
        ///// <returns>Locations by source path</returns>
        //public Dictionary<string, List<CodeLocation>> GroupLocationsBySourcePath(List<CodeLocation> list)
        //{
        //    Dictionary<string, List<CodeLocation>> dic = new Dictionary<string, List<CodeLocation>>();
        //    foreach (var item in list)
        //    {
        //        List<CodeLocation> value;
        //        if (!dic.TryGetValue(item.SourceClass, out value))
        //        {
        //            value = new List<CodeLocation>();
        //            dic[item.SourceClass] = value;
        //        }

        //        dic[item.SourceClass].Add(item);
        //    }

        //    return dic;
        //}

        ///// <summary>
        ///// Covert the region on a method to an example ListNode
        ///// </summary>
        ///// <param name="me">Method</param>
        ///// <param name="re">Region within the method</param>
        ///// <returns>A example</returns>
        //private Tuple<ListNode, ListNode> Example2(SyntaxNode me, TRegion re)
        //{
        //    List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();
        //    list = ASTManager.EnumerateSyntaxNodesAndTokens(me, list);
        //    ListNode listNode = new ListNode(list);
        //    listNode.OriginalText = me.GetText().ToString();

        //    SyntaxNodeOrToken node = list[0];

        //    int i = 0;
        //    while (re.Start > node.Span.Start)
        //    {
        //        node = list[i++];
        //    }

        //    int j = i;
        //    while (re.Start + re.Length >= node.Span.End)
        //    {
        //        if (j == list.Count)
        //            break;
        //        node = list[Math.Max(j++, 0)];
        //    }

        //    if (i == 0 && j == 0)
        //    {
        //        j = list.Count;
        //    }

        //    ListNode subNodes = ASTManager.SubNotes(listNode, Math.Max(i - 1, 0), ((j) - i));
        //    subNodes.OriginalText = re.Text;
        //    Tuple<ListNode, ListNode> t = Tuple.Create(listNode, subNodes);

        //    return t;
        //}



        /// <summary>
        /// Calculate least common ancestor of source code
        /// </summary>
        /// <param name="sourceCode">Source code</param>
        /// <param name="list">Region list</param>
        /// <returns>Least common ancestor</returns>
        public static SyntaxNode LeastCommonAncestor(string sourceCode, List<TRegion> list)
        {
            if (sourceCode == null) throw new ArgumentNullException("sourceCode");
            if (list == null) throw new ArgumentNullException("list");
            if (!list.Any()) { throw new ArgumentException("Selection list cannot be empty"); }

            string filePath = list.First().Path;
            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode, path: filePath);
            List<SyntaxNodeOrToken> nodes = new List<SyntaxNodeOrToken>();
            foreach (TRegion region in list)
            {
                var descendentNodes = ASTManager.NodesBetweenStartAndEndPosition(tree, region.Start, region.Start + region.Length);
                nodes.AddRange(descendentNodes);
            }

            SyntaxNodeOrToken lca = LCAManager.GetInstance().LeastCommonAncestor(nodes, tree);
            SyntaxNode snode = lca.AsNode();

            return snode;
        }

        /// <summary>
        /// Syntax nodes that will be used in the filtering phase.
        /// </summary>
        /// <param name="sourceCode">Source code</param>
        /// <param name="list">Selected regions to base syntax nodes selection</param>
        /// <returns>Elements to be used on the filtering process</returns>
        public static List<SyntaxNode> SyntaxNodesForFiltering(string sourceCode, List<TRegion> list)
        {
            if (sourceCode == null) throw new ArgumentNullException("sourceCode");
            if (list == null) throw new ArgumentNullException("list");
            if (!list.Any()) { throw new ArgumentException("Selection list cannot be empty"); }

            string filePath = list.First().Path;
            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode, path: filePath);
            var lcas = new List<SyntaxNode>();
            foreach (TRegion region in list)
            {
                if (region.Length != 0)
                {
                    var descendentNodes = ASTManager.NodesBetweenStartAndEndPosition(tree, region.Start, region.Start + region.Length);
                    SyntaxNodeOrToken lca = LCAManager.GetInstance().LeastCommonAncestor(descendentNodes, tree);
                    lcas.Add(lca.AsNode());
                }
            }
            Dictionary<SyntaxKind, IEnumerable<SyntaxNode>> dicSyntaxNodes = new Dictionary<SyntaxKind, IEnumerable<SyntaxNode>>();
            foreach (var sn in lcas)
            {
                IEnumerable<SyntaxNode> value;
                if (!dicSyntaxNodes.TryGetValue(sn.Kind(), out value))
                {
                    dicSyntaxNodes[sn.Kind()] = ASTManager.NodesWithTheSameSyntaxKind(tree.GetRoot(), sn.Kind());
                }
            }
            List<SyntaxNode> retr = new List<SyntaxNode>();
            foreach (var item in dicSyntaxNodes)
            {
                retr.AddRange(item.Value);
            }
            return retr;
        }

        /// <summary>
        /// Syntax nodes 
        /// </summary>
        /// <param name="sourceCode">Source code</param>
        /// <param name="list">List of selected regions</param>
        /// <returns>Syntax nodes</returns>
        public List<SyntaxNode> SyntaxNodes(string sourceCode, List<TRegion> list)
        {
            SelectionInfo info = new SelectionInfo(sourceCode, new List<TRegion>(list));
            List<SyntaxNode> nodes;
            if (!_computed.TryGetValue(info, out nodes))
            {
                nodes = SyntaxNodesForFiltering(sourceCode, list);
                _computed.Add(info, nodes);
            }
            //return nodes;
            return _computed[info];
        }

        ///// <summary>
        ///// Pair of syntax node before and after transformation
        ///// </summary>
        ///// <param name="locations">Selected locations</param>
        ///// <returns>Pair of syntax node before and after transformation</returns>
        //internal List<Tuple<SyntaxNode, SyntaxNode>> SyntaxNodesRegionBeforeAndAfterEditing(List<CodeLocation> locations)
        //{
        //    if (locations == null) throw new ArgumentNullException("locations");
        //    if (!locations.Any()) throw new Exception("Locations cannot be empty.");

        //    Dictionary<string, List<CodeLocation>> groupLocations = GroupLocationsBySourceFile(locations);

        //    EditorController controller = EditorController.GetInstance();
        //    var result = new List<Tuple<SyntaxNode, SyntaxNode>>();

        //    foreach (var item in groupLocations)
        //    {
        //        string sourceCode = item.Value.First().SourceCode;
        //        string sourceCodeAfter = GetDocumentAfterEdition(sourceCode, controller.DocumentsBeforeAndAfter);
        //        if (sourceCodeAfter != null)
        //        {
        //            SyntaxTree treeAfter = CSharpSyntaxTree.ParseText(sourceCodeAfter);
        //            List<SyntaxNode> aNodes = new List<SyntaxNode>();
        //            foreach (var span in controller.EditedLocations[item.Key])
        //            {
        //                MessageBox.Show(sourceCodeAfter.Substring(span.Start + 1, span.Length - 2));
        //                //var snode = LeastCommonAncestor(treeAfter, span.Start + 1, (span.Start + 1) + (span.Length - 2));
        //                SyntaxNode snode = LCAManager.LeastCommonAncestor(treeAfter, span.Start + 1, (span.Start + 1) + (span.Length - 2));
        //                aNodes.Add(snode);
        //            }

        //            for (int i = 0; i < item.Value.Count; i++)
        //            {
        //                Tuple<SyntaxNode, SyntaxNode> tuple = Tuple.Create(item.Value[i].Region.Node, aNodes[i]);
        //                result.Add(tuple);
        //            }
        //        }
        //    }
        //    return result;
        //}

        /// <summary>
        /// Pair of syntax node before and after transformation
        /// </summary>
        /// <param name="locations">Selected locations</param>
        /// <returns>Pair of syntax node before and after transformation</returns>
        internal List<Tuple<SyntaxNode, SyntaxNode>> SyntaxNodesRegionBeforeAndAfterEditing(List<CodeLocation> locations)
        {
            if (locations == null) throw new ArgumentNullException("locations");
            if (!locations.Any()) throw new Exception("Locations cannot be empty.");

            Dictionary<string, List<CodeLocation>> groupLocations = GroupLocationsBySourceFile(locations);

            EditorController controller = EditorController.GetInstance();
            IEnumerable<string> files = controller.OpenFiles();
            var result = new List<Tuple<SyntaxNode, SyntaxNode>>();

            foreach (string file in files)
            {
                List<CodeLocation> clocations = groupLocations[file];
                string sourceCode = clocations.First().SourceCode;
                string sourceCodeAfter = GetDocumentAfterEdition(sourceCode, controller.DocumentsBeforeAndAfter);
                //if (sourceCodeAfter != null)
                //{
                SyntaxTree treeAfter = CSharpSyntaxTree.ParseText(sourceCodeAfter, path: file);
                List<SyntaxNode> aNodes = new List<SyntaxNode>();
                foreach (Selection span in controller.EditedLocations[file])
                {
                    //MessageBox.Show(sourceCodeAfter.Substring(span.Start + 1, span.Length - 2));

                    SyntaxNode snode = LCAManager.LeastCommonAncestor(treeAfter, span.Start + 1,
                        (span.Start + 1) + (span.Length - 2));
                    aNodes.Add(snode);
                }

                for (int i = 0; i < clocations.Count; i++)
                {
                    Tuple<SyntaxNode, SyntaxNode> tuple = Tuple.Create(clocations[i].Region.Node, aNodes[i]);
                    result.Add(tuple);
                }
                // }
            }
            return result;
        }

        /// <summary>
        /// Pair of syntax node before and after transformation
        /// </summary>
        /// <param name="locations">Selected locations</param>
        /// <returns>Pair of syntax node before and after transformation</returns>
        internal List<Tuple<ListNode, ListNode>> ElementsSelectionBeforeAndAfterEditing(List<CodeLocation> locations)
        {
            if (locations == null) throw new ArgumentNullException("locations");
            if (!locations.Any()) throw new Exception("Locations cannot be empty.");

            Dictionary<string, List<CodeLocation>> groupLocations = GroupLocationsBySourceFile(locations);

            EditorController controller = EditorController.GetInstance();
            IEnumerable<string> files = controller.OpenFiles();

            List<TRegion> inputRegions = new List<TRegion>();
            List<TRegion> outputRegions = new List<TRegion>();

            foreach (string file in files)
            {
                string fileUpper = file.ToUpperInvariant();
                string sourceCode = groupLocations[fileUpper].First().SourceCode;
                string sourceCodeAfter = GetDocumentAfterEdition(sourceCode, controller.DocumentsBeforeAndAfter);
                if (sourceCodeAfter != null)
                {
                    List<CodeLocation> cLocations = groupLocations[fileUpper];

                    TRegion iparent = new TRegion { Text = sourceCode };
                    foreach (CodeLocation codeLocation in cLocations)
                    {
                        codeLocation.Region.Parent = iparent;
                        inputRegions.Add(codeLocation.Region);
                    }


                    TRegion parent = new TRegion { Text = sourceCodeAfter };
                    foreach (var span in controller.EditedLocations[fileUpper])
                    {
                        TRegion tregion = new TRegion
                        {
                            Start = span.Start + 1,
                            Length = span.Length - 2,
                            Parent = parent,
                            Text = sourceCodeAfter.Substring(span.Start + 1, span.Length - 2),
                            Path = fileUpper
                        };
                        //MessageBox.Show(span.Start + tregion.Text + span.Length);
                        outputRegions.Add(tregion);
                    }
                }
            }

            List<Tuple<ListNode, ListNode>> inputSelection = Decomposer.GetInstance().DecomposeToOutput(inputRegions);
            List<Tuple<ListNode, ListNode>> ouputSelection = Decomposer.GetInstance().DecomposeToOutput(outputRegions);

            List<Tuple<ListNode, ListNode>> examples = new List<Tuple<ListNode, ListNode>>();
            for (int index = 0; index < inputSelection.Count; index++)
            {
                ListNode input = inputSelection[index].Item2;
                ListNode output = ouputSelection[index].Item2;
                Tuple<ListNode, ListNode> tuple = Tuple.Create(input, output);
                examples.Add(tuple);
            }

            return examples;
        }

        /// <summary>
        /// Get document after edition
        /// </summary>
        /// <param name="documentBeforeEdition">Document before edition</param>
        /// <param name="documents">All documents</param>
        /// <returns>Document after edition</returns>
        private string GetDocumentAfterEdition(string documentBeforeEdition, List<Tuple<string, string>> documents)
        {
            foreach (var document in documents)
            {
                if (document.Item1.Equals(documentBeforeEdition)) return document.Item2;
            }
            return null;
        }

        /// <summary>
        /// Least common ancestor
        /// </summary>
        /// <param name="sourceCode">Source code</param>
        /// <param name="region">Region</param>
        /// <returns>Least common ancestor of region</returns>
        private static SyntaxNode LeastCommonAncestor(string sourceCode, TRegion region)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode, path: region.Path);
            List<SyntaxNodeOrToken> nodesSelection = ASTManager.NodesBetweenStartAndEndPosition(tree, region.Start, region.Start + region.Length);

            SyntaxNodeOrToken lca = LCAManager.GetInstance().LeastCommonAncestor(nodesSelection, tree);
            SyntaxNode snode = lca.AsNode();

            return snode;
        }

        /// <summary>
        /// Least common ancestor
        /// </summary>
        /// <param name="sourceCode">Source code</param>
        /// <param name="region">Region</param>
        /// <returns>Least common ancestor of region</returns>
        private static SyntaxNode LeastCommonAncestor(ListNode lnode)
        { 
            SyntaxNodeOrToken lca = LCAManager.GetInstance().LeastCommonAncestor(lnode.List, lnode.List.First().SyntaxTree);
            SyntaxNode snode = lca.AsNode();

            return snode;
        }

        ///// <summary>
        ///// Least common ancestors
        ///// </summary>
        ///// <param name="sourceCode">Source code</param>
        ///// <param name="regions">Region list</param>
        ///// <returns>Least common ancestor of each region</returns>
        //public static List<SyntaxNode> LeastCommonAncestors(string sourceCode, List<TRegion> regions)
        //{
        //    List<SyntaxNode> slist = new List<SyntaxNode>();
        //    foreach (var region in regions)
        //    {
        //        slist.Add(LeastCommonAncestor(sourceCode, region));
        //    }
        //    return slist;
        //}

        /// <summary>
        /// Least common ancestors
        /// </summary>
        /// <param name="regions">Region list</param>
        /// <returns>Least common ancestor of each region</returns>
        public static List<SyntaxNode> LeastCommonAncestors(List<TRegion> regions)
        {
            var sourceFiles = GetInstance().GroupRegionBySourceFile(regions);
            List<SyntaxNode> slist = new List<SyntaxNode>();
            foreach (var sourceCode in sourceFiles)
            {
                foreach (var region in sourceCode.Value)
                {
                    slist.Add(LeastCommonAncestor(sourceCode.Key, region));
                }
            }
            return slist;
        }

        /// Least common ancestors
        /// </summary>
        /// <param name="regions">Region list</param>
        /// <returns>Least common ancestor of each region</returns>
        public static List<SyntaxNode> LeastCommonAncestors(List<ListNode> regions)
        {
            var sourceFiles = GetInstance().GroupRegionBySourceFile(regions);
            List<SyntaxNode> slist = new List<SyntaxNode>();
            foreach (var sourceCode in sourceFiles)
            {
                foreach (var region in sourceCode.Value)
                {
                    slist.Add(LeastCommonAncestor(region));
                }
            }
            return slist;
        }
    }
}


