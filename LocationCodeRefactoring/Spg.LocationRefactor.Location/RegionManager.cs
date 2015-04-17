using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ExampleRefactoring.Spg.ExampleRefactoring.AST;
using ExampleRefactoring.Spg.ExampleRefactoring.LCS;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using LocationCodeRefactoring.Spg.LocationCodeRefactoring.Controller;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.LocationRefactor.Location;
using Spg.LocationRefactor.TextRegion;

namespace LocationCodeRefactoring.Spg.LocationRefactor.Location
{
    /// <summary>
    /// Strategy
    /// </summary>
    public class RegionManager
    {
        private readonly Dictionary<string, List<SyntaxNode>> _computed;

        private static RegionManager _instance;

        private RegionManager()
        {
            _computed = new Dictionary<string, List<SyntaxNode>>();
        }

        public static void Init()
        {
            _instance = null;
        }

        public static RegionManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new RegionManager();
            }
            return _instance;
        }

        /// <summary>
        /// Choose corresponding syntax node for the region
        /// </summary>
        /// <param name="statements">Statement list</param>
        /// <param name="regions">Region</param>
        /// <returns>Pair of region with the respectively syntax node</returns>
        private Dictionary<TRegion, SyntaxNode> ChoosePairs(List<SyntaxNode> statements, List<TRegion> regions)
        {
            Dictionary<TRegion, SyntaxNode> dicRegions = new Dictionary<TRegion, SyntaxNode>();


            foreach (SyntaxNode statement in statements)
            {
                foreach (TRegion region in regions)
                {
                    if (region.Length == 0)
                    {
                        dicRegions[region] = null;
                        continue;
                    }

                    string text = region.Text;
                    string pattern = Regex.Escape(text);
                    string statmentText = statement.GetText().ToString();
                    bool contains = Regex.IsMatch(statmentText, pattern);
                    if (contains)
                    {
                        if (statement.SpanStart <= region.Start && region.Start <= statement.Span.End)
                        {
                            if (!dicRegions.ContainsKey(region))
                            {
                                dicRegions.Add(region, statement);
                            }
                            else if (statement.GetText().Length < dicRegions[region].GetText().Length)
                            {
                                dicRegions[region] = statement;
                            }
                        }
                  }
                }
            }
            return dicRegions;
        }

        /// <summary>
        /// Decompose set of selected region to list of examples
        /// </summary>
        /// <param name="list">List of selected regions</param>
        /// <returns>Decomposed examples</returns>
        public List<Tuple<ListNode, ListNode>> Decompose(List<TRegion> list)
        {
            List<Tuple<ListNode, ListNode>> examples = new List<Tuple<ListNode, ListNode>>();

            Dictionary<string, List<TRegion>> dicRegion = GroupRegionBySourceFile(list);
            foreach (var entry in dicRegion)
            {
                List<SyntaxNode> statements = SyntaxNodes(entry.Key, entry.Value);
                //var methodsDic = new Dictionary<SyntaxNode, Tuple<ListNode, ListNode>>();
                Dictionary<TRegion, SyntaxNode> pairs = ChoosePairs(statements, entry.Value);
                foreach (KeyValuePair<TRegion, SyntaxNode> pair in pairs)
                {
                    TRegion re = pair.Key;
                    SyntaxNode node = pair.Value;

                    //Tuple<ListNode, ListNode> val;
                    Tuple<ListNode, ListNode> te;
                    if (node != null)
                    {
                        te = Example(node, re);
                    }
                    else
                    {
                        ListNode emptyNode = new ListNode(new List<SyntaxNodeOrToken>());
                        te = Tuple.Create(emptyNode, emptyNode);
                    }
                    //if (!methodsDic.TryGetValue(node, out val))
                    //{
                        examples.Add(te);
                    //    methodsDic.Add(node, te);
                    //}
                    //else
                    //{
                    //    val.Item2.List.AddRange(te.Item2.List);
                    //}
                }
            }
            return examples;
        }

        /// <summary>
        /// Decompose set of selected region to list of examples
        /// </summary>
        /// <param name="list">List of selected regions</param>
        /// <returns>Decomposed examples</returns>
        public List<Tuple<ListNode, ListNode>> DecomposeToOutput(List<TRegion> list)
        {
            List<Tuple<ListNode, ListNode>> examples = new List<Tuple<ListNode, ListNode>>();

            Dictionary<string, List<TRegion>> dicRegion = GroupRegionBySourceFile(list);
            foreach (var entry in dicRegion)
            {
                //List<SyntaxNode> statements = SyntaxNodes(entry.Key, entry.Value);
                //var methodsDic = new Dictionary<SyntaxNode, Tuple<ListNode, ListNode>>();
                //Dictionary<TRegion, SyntaxNode> pairs = ChoosePairs(statements, entry.Value);
                SyntaxTree tree = CSharpSyntaxTree.ParseText(entry.Key);
                foreach (TRegion re in entry.Value)
                {
                    //TRegion re = pair.Key;
                    //SyntaxNode node = pair.Value;
                    SyntaxNode node = tree.GetRoot();

                    //Tuple<ListNode, ListNode> val;
                    Tuple<ListNode, ListNode> te;
                    if (re.Length != 0)
                    {
                        te = Example(node, re);
                    }
                    else
                    {
                        ListNode emptyNode = new ListNode(new List<SyntaxNodeOrToken>());
                        te = Tuple.Create(emptyNode, emptyNode);
                    }
                    //if (!methodsDic.TryGetValue(node, out val))
                    //{
                    examples.Add(te);
                    //    methodsDic.Add(node, te);
                    //}
                    //else
                    //{
                    //    val.Item2.List.AddRange(te.Item2.List);
                    //}
                }
            }
            return examples;
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
                if (!dic.TryGetValue(item.SourceClass, out value))
                {
                    value = new List<CodeLocation>();
                    dic[item.SourceClass] = value;
                }

                dic[item.SourceClass].Add(item);
            }

            return dic;
        }

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
        /// Covert the region on a method to an example ListNode
        /// </summary>
        /// <param name="syntaxNode">Syntax node</param>
        /// <param name="re">Region within the method</param>
        /// <param name="compact">Needs to be compacted</param>
        /// <returns>An example</returns>
        private Tuple<ListNode, ListNode> Example(SyntaxNode syntaxNode, TRegion re, bool compact = false)
        {
            List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();
            if (compact)
            {
                list = ASTManager.EnumerateSyntaxNodesAndTokens2(syntaxNode, list);
            }
            else
            {
                list = ASTManager.EnumerateSyntaxNodesAndTokens(syntaxNode, list);
            }
            ListNode listNode = new ListNode(list);
            listNode.OriginalText = syntaxNode.GetText().ToString();

            SyntaxNodeOrToken node = list[0];

            int i = 0;
            while (re.Start > node.Span.Start)
            {
                node = list[i++];
            }

            int j = i;
            bool last = false;
            while (re.Start + re.Length >= node.Span.End)
            {
                if (node.Equals(list.Last()))
                {
                    last = true; break; //j reached the last element.
                }
                node = list[j++];
            }

            ListNode subNodes;
            int iIndex = Math.Max(i - 1, 0);
            if (last)
            {
                subNodes = ASTManager.SubNotes(listNode, iIndex, list.Count - iIndex);
            }
            else
            {
                List<SyntaxNodeOrToken> snort = new List<SyntaxNodeOrToken>();
                for (int k = iIndex; !list[k].Equals(node); k++)
                {
                    snort.Add(list[k]);
                }
                subNodes = new ListNode(snort);
            }

            //subNodes = ASTManager.SubNotes(listNode, Math.Max(i - 1, 0), ((j) - i));
            subNodes.OriginalText = re.Text;
            Tuple<ListNode, ListNode> t = Tuple.Create(listNode, subNodes);

            return t;
        }


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

            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);
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

            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);
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
                if (!dicSyntaxNodes.TryGetValue(sn.CSharpKind(), out value))
                {
                    dicSyntaxNodes[sn.CSharpKind()] = ASTManager.NodesWithTheSameSyntaxKind(tree.GetRoot(), sn.CSharpKind());
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
            List<SyntaxNode> nodes;
            if (!_computed.TryGetValue(sourceCode, out nodes))
            {
                nodes = SyntaxNodesForFiltering(sourceCode, list);
                _computed.Add(sourceCode, nodes);
            }

            return _computed[sourceCode];
        }

        /// <summary>
        /// Pair of syntax node before and after transformation
        /// </summary>
        /// <param name="locations">Selected locations</param>
        /// <returns>Pair of syntax node before and after transformation</returns>
        internal List<Tuple<SyntaxNode, SyntaxNode>> SyntaxNodesRegionBeforeAndAfterEditing(List<CodeLocation> locations)
        {
            if (locations == null) throw new ArgumentNullException("locations");
            if (!locations.Any()) throw new Exception("Locations cannot be null.");

            Dictionary<string, List<CodeLocation>> groupLocations = GroupLocationsBySourceFile(locations);

            EditorController controller = EditorController.GetInstance();
            var result = new List<Tuple<SyntaxNode, SyntaxNode>>();

            foreach (var item in groupLocations)
            {
                string sourceCode = item.Value.First().SourceCode;
                string sourceCodeAfter = GetDocumentAfterEdition(sourceCode, controller.DocumentsBeforeAndAfter);
                if (sourceCodeAfter != null)
                {
                    SyntaxTree treeAfter = CSharpSyntaxTree.ParseText(sourceCodeAfter);
                    List<SyntaxNode> aNodes = new List<SyntaxNode>();
                    foreach (var span in controller.EditedLocations[item.Key])
                    {
                        //MessageBox.Show(sourceCodeAfter.Substring(span.Start + 1, span.Length - 2));
                        var snode = LeastCommonAncestor(treeAfter, span.Start + 1, (span.Start + 1) + (span.Length - 2));
                        //SyntaxNode snode = LCAManager.LeastCommonAncestor(treeAfter, span.Start + 1, (span.Start + 1) + (span.Length - 2));
                        aNodes.Add(snode);
                    }

                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        Tuple<SyntaxNode, SyntaxNode> tuple = Tuple.Create(item.Value[i].Region.Node, aNodes[i]);
                        result.Add(tuple);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Pair of syntax node before and after transformation
        /// </summary>
        /// <param name="locations">Selected locations</param>
        /// <returns>Pair of syntax node before and after transformation</returns>
        internal List<Tuple<ListNode, ListNode>> ElementsSelectionBeforeAndAfterEditing(List<CodeLocation> locations )
        {
            if (locations == null) throw new ArgumentNullException("locations");
            if (!locations.Any()) throw new Exception("Locations cannot be null.");

            Dictionary<string, List<CodeLocation>> groupLocations = GroupLocationsBySourceFile(locations);

            EditorController controller = EditorController.GetInstance();
           
            List<TRegion> inputRegions = new List<TRegion>();
            List<TRegion> outputRegions = new List<TRegion>();
            
            foreach (var item in groupLocations)
            {
                string sourceCode = item.Value.First().SourceCode;

                TRegion iparent = new TRegion { Text = sourceCode };
                foreach (var codeLocation in item.Value)
                {
                    codeLocation.Region.Parent = iparent;
                    if (controller.FilesOpened.ContainsKey(item.Key))
                    {
                        inputRegions.Add(codeLocation.Region);
                    }
                }

                string sourceCodeAfter = GetDocumentAfterEdition(sourceCode, controller.DocumentsBeforeAndAfter);
                if (sourceCodeAfter != null)
                {
                    TRegion parent = new TRegion { Text = sourceCodeAfter };
                    foreach (var span in controller.EditedLocations[item.Key])
                    {
                        TRegion tregion = new TRegion
                        {
                            Start = span.Start + 1,
                            Length = span.Length - 2,
                            Parent = parent,
                            Text = sourceCodeAfter.Substring(span.Start + 1, span.Length - 2)
                        };
                        //MessageBox.Show(tregion.Text + span.Start + " " + span.Length);

                        outputRegions.Add(tregion);
                    }
                }
            }

            List<Tuple<ListNode, ListNode>> inputSelection = DecomposeToOutput(inputRegions);
            List<Tuple<ListNode, ListNode>> ouputSelection = DecomposeToOutput(outputRegions);

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

        private string GetDocumentAfterEdition(string documentBeforeEdition, List<Tuple<string, string>> documents)
        {
            foreach (var document in documents)
            {
                if (document.Item1.Equals(documentBeforeEdition)) return document.Item2;
            }
            return null;
        }

        private static SyntaxNode LeastCommonAncestor(string sourceCode, TRegion region)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);
            List<SyntaxNodeOrToken> nodesSelection = ASTManager.NodesBetweenStartAndEndPosition(tree, region.Start, region.Start + region.Length);

            SyntaxNodeOrToken lca = LCAManager.GetInstance().LeastCommonAncestor(nodesSelection, tree);
            SyntaxNode snode = lca.AsNode();

            return snode;
        }

        private static SyntaxNode LeastCommonAncestor(SyntaxTree tree, int start, int end)
        {
            List<SyntaxNodeOrToken> nodesSelection = ASTManager.NodesBetweenStartAndEndPosition(tree, start, end);

            SyntaxNodeOrToken lca = LCAManager.GetInstance().LeastCommonAncestor(nodesSelection, tree);
            SyntaxNode snode = lca.AsNode();

            return snode;
        }

        public static List<SyntaxNode> LeastCommonAncestors(string sourceCode, List<TRegion> regions)
        {
            List<SyntaxNode> slist = new List<SyntaxNode>();
            foreach (var region in regions)
            {
                slist.Add(LeastCommonAncestor(sourceCode, region));
            }
            return slist;
        }

        public static List<TRegion> GroupRegionByStartAndEndPosition(List<TRegion> tregions)
        {
            Dictionary<Tuple<int, int>, TRegion> dic = new Dictionary<Tuple<int, int>, TRegion>();

            foreach (var entry in tregions)
            {
                TRegion value;
                Tuple<int, int> key = Tuple.Create(entry.Start, entry.Length);
                if (!dic.TryGetValue(key, out value))
                {
                    dic[key] = entry;
                }
                else if (value.Node.GetText().Length > entry.Node.GetText().Length)
                {
                    dic[key] = entry;
                }
            }
            return dic.Values.ToList();
        }

        /// <summary>
        /// Return regions that have at least one of the syntax kind listed on syntaxKinds
        /// </summary>
        /// <param name="regions">Collection of regions</param>
        /// <param name="syntaxKinds">Syntax kinds</param>
        /// <returns>Regions that have at least one of the syntax kind listed on syntaxKinds</returns>
        internal static List<TRegion> RegionsThatHaveOneOfTheSyntaxKind(List<TRegion> regions, List<SyntaxNode> syntaxKinds)
        {
            List<TRegion> list = new List<TRegion>();
            foreach (var region in regions)
            {
                foreach (var node in syntaxKinds)
                {
                    if (region.Node.IsKind(node.CSharpKind()))
                    {
                        list.Add(region);
                        break;
                    }
                }
            }
            list = NonDuplicateRegions(list);
            return list;
       }

        private static List<TRegion> NonDuplicateRegions(List<TRegion> regions)
        {
            List<TRegion> nonDuplicationRegions = new List<TRegion>();
            bool[] analized = Enumerable.Repeat(false, regions.Count).ToArray();
            for (int i = 0; i < regions.Count; i++)
            {
                if (!analized[i])
                {
                    for (int j = i + 1; j < regions.Count; j++)
                    {
                        if (regions[i].IntersectWith(regions[j]))
                        {
                            if (regions[i].Length > regions[j].Length)
                            {
                                nonDuplicationRegions.Add(regions[i]);
                                analized[i] = true;
                            }
                            else
                            {
                                nonDuplicationRegions.Add(regions[j]);
                                analized[j] = true;
                            }
                        }
                    }
                }
                if (analized[i] == false)
                {
                    nonDuplicationRegions.Add(regions[i]);
                }
            }
            return nonDuplicationRegions;
        } 
    }
}