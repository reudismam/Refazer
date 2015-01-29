using System;
using System.Collections.Generic;
using System.Linq;
using LeastCommonAncestor;
using LocationCodeRefactoring.Spg.LocationCodeRefactoring.Controller;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Location;
using Spg.LocationRefactor.TextRegion;

namespace LocationCodeRefactoring.Spg.LocationRefactor.Location
{
    /// <summary>
    /// Strategy
    /// </summary>
    public abstract class Strategy
    {
        public List<Tuple<ListNode, ListNode>> Extract(List<TRegion> list)
        {
            List<Tuple<ListNode, ListNode>> examples = new List<Tuple<ListNode, ListNode>>();

            TRegion region = list[0];
            List<SyntaxNode> statements = SyntaxNodes(region.Parent.Text, list);
            Dictionary<SyntaxNode, Tuple<ListNode, ListNode>> methodsDic = new Dictionary<SyntaxNode, Tuple<ListNode, ListNode>>();
            Dictionary<TRegion, SyntaxNode> pairs = ChoosePairs(statements, list);
            foreach (KeyValuePair<TRegion, SyntaxNode> pair in pairs)
            {
                TRegion re = pair.Key;
                SyntaxNode node = pair.Value;
              
                Tuple<ListNode, ListNode> val = null;
                Tuple<ListNode, ListNode> te = Example(node, re);
                if (!methodsDic.TryGetValue(node, out val))
                {
                    examples.Add(te);
                    methodsDic.Add(node, te);
                }
                else
                {
                    val.Item2.List.AddRange(te.Item2.List);
                }
            }
            return examples;
        }

        /// <summary>
        /// Choose corresponding syntax node for the region
        /// </summary>
        /// <param name="statements">Statement list</param>
        /// <param name="regions">Region</param>
        /// <returns></returns>
        private Dictionary<TRegion, SyntaxNode> ChoosePairs(List<SyntaxNode> statements, List<TRegion> regions)
        {
            Dictionary<TRegion, SyntaxNode> dicRegions = new Dictionary<TRegion, SyntaxNode>();

            foreach (SyntaxNode statment in statements)
            {
                foreach (TRegion region in regions)
                {
                    string text = region.Text;
                    string pattern = System.Text.RegularExpressions.Regex.Escape(text);
                    string statmentText = statment.GetText().ToString();
                    bool contains = System.Text.RegularExpressions.Regex.IsMatch(statmentText, pattern);
                    if (contains)
                    {
                        if (statment.SpanStart <= region.Start && region.Start <= statment.SpanStart + statment.Span.Length)
                        {
                            if (!dicRegions.ContainsKey(region))
                            {
                                dicRegions.Add(region, statment);
                            }
                            else if (statment.GetText().Length < dicRegions[region].GetText().Length)
                            {
                                dicRegions[region] = statment;
                            }
                        }
                    }
                }
            }
            return dicRegions;
        }

        internal static List<SyntaxNode> SyntaxElements(string input, string sourceCode, List<TRegion> list)
        {
            if (sourceCode == null) { throw new ArgumentNullException("sourceCode"); }

            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);

            List<SyntaxNode> nodes = new List<SyntaxNode>();
            foreach (TRegion region in list)
            {
                var descendentNodes = from node in tree.GetRoot().DescendantNodes()
                                      where node.SpanStart == region.Start
                                      select node;
                nodes.AddRange(descendentNodes);
            }

            LCA<SyntaxNodeOrToken> lcaCalculator = new LCA<SyntaxNodeOrToken>();
            SyntaxNodeOrToken lca = nodes[0];
            for (int i = 1; i < nodes.Count; i++)
            {
                SyntaxNodeOrToken node = nodes[i];
                lca = lcaCalculator.LeastCommonAncestor(tree.GetRoot(), lca, node);
            }

            SyntaxNode snode = lca.AsNode();

            SyntaxTree inputTree = CSharpSyntaxTree.ParseText(input);
            var rootList = from node in inputTree.GetRoot().DescendantNodes()
                           where node.RawKind == snode.RawKind
                           select node;
            List<SyntaxNode> result = new List<SyntaxNode>();
            foreach (SyntaxNode sn in rootList)
            {
                result.AddRange(sn.DescendantNodes());
            }
            return result;

        }

        /// <summary>
        /// Covert the region on a method to an example ListNode
        /// </summary>
        /// <param name="me">Method</param>
        /// <param name="re">Region within the method</param>
        /// <returns>A example</returns>
        private Tuple<ListNode, ListNode> Example(SyntaxNode me, TRegion re)
        {
            List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();
            list = ASTManager.EnumerateSyntaxNodesAndTokens(me, list);
            ListNode listNode = new ListNode(list);
            listNode.OriginalText = me.GetText().ToString();

            SyntaxNodeOrToken node = list[0];

            int i = 0;
            while (re.Start > node.Span.Start)
            {
                node = list[i++];
            }

            int j = i;
            while (re.Start + re.Length >= node.Span.Start)
            {
                if (j == list.Count)
                    break;
                node = list[Math.Max(j++, 0)];
            }

            if (i == 0 && j == 0)
            {
                j = list.Count;
            }

            ListNode subNodes = ASTManager.SubNotes(listNode, Math.Max(i - 1, 0), ((j) - i));
            subNodes.OriginalText = re.Text;
            Tuple<ListNode, ListNode> t = Tuple.Create(listNode, subNodes);

            return t;
        }

        /// <summary>
        /// Elements with syntax kind in the source code
        /// </summary>
        /// <param name="sourceCode">Source code</param>
        /// <param name="list">Selected regions</param>
        /// <returns>Elements with syntax kind in the source code</returns>
        public static List<SyntaxNode> SyntaxElements(string sourceCode, List<TRegion> list)
        {
            if (sourceCode == null || list == null) { throw new ArgumentException("source code and selection list cannot be null"); }

            if (!list.Any()) { throw new ArgumentException("Selection list cannot be empty"); }

            SyntaxNode snode = LeastCommonAncestor(sourceCode, list);
            return snode.DescendantNodes().ToList();
        }

        /// <summary>
        /// Calculate least common ancestor of source code
        /// </summary>
        /// <param name="sourceCode">Source code</param>
        /// <param name="list">Region list</param>
        /// <returns>Least common ancestor</returns>
        public static SyntaxNode LeastCommonAncestor(string sourceCode, List<TRegion> list)
        {
            if (sourceCode == null || list == null) { throw new ArgumentException("source code and selection list cannot be null"); }

            if (!list.Any()) { throw new ArgumentException("Selection list cannot be empty"); }

            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);
            List<SyntaxNode> nodes = new List<SyntaxNode>();
            foreach (TRegion region in list)
            {
                var descendentNodes = from node in tree.GetRoot().DescendantNodes()
                                      where node.SpanStart == region.Start
                                      select node;
                nodes.AddRange(descendentNodes);
            }

            LCA<SyntaxNodeOrToken> lcaCalculator = new LCA<SyntaxNodeOrToken>();
            SyntaxNodeOrToken lca = nodes[0];
            for (int i = 1; i < nodes.Count; i++)
            {
                SyntaxNodeOrToken node = nodes[i];
                lca = lcaCalculator.LeastCommonAncestor(tree.GetRoot(), lca, node);
            }

            SyntaxNode snode = lca.AsNode();
            return snode;
        }

        internal List<Tuple<SyntaxNode, SyntaxNode>> SyntaxNodesRegion(string sourceBefore, string sourceAfter, List<CodeLocation> locations)
        {
            if (sourceBefore == null || sourceAfter == null) { throw new ArgumentNullException("source code cannot be null"); }

            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceBefore);
            SyntaxTree treeAfter = CSharpSyntaxTree.ParseText(EditorController.GetInstance().CurrentViewCodeAfter);
            List<SyntaxNode> nodes = new List<SyntaxNode>();

            SyntaxNodeOrToken globalLCA;
            SyntaxNode globalNode = null;
            LCA<SyntaxNodeOrToken> lcaCalculator = new LCA<SyntaxNodeOrToken>();
            foreach (CodeLocation location in locations)
            {
                if (sourceBefore.Equals(location.SourceCode))
                {
                    List<SyntaxNode> nodesSelection = new List<SyntaxNode>();
                    var descedentsBegin = from node in tree.GetRoot().DescendantNodes()
                                          where node.SpanStart == location.Region.Start
                                          select node;
                    nodesSelection.AddRange(descedentsBegin);

                    var descedentsEnd = from node in tree.GetRoot().DescendantNodes()
                                        where node.SpanStart + node.Span.Length == location.Region.Start + location.Region.Length
                                        select node;
                    nodesSelection.AddRange(descedentsEnd);

                    SyntaxNodeOrToken lca = nodesSelection[0];
                    for (int i = 1; i < nodesSelection.Count; i++)
                    {
                        SyntaxNodeOrToken node = nodesSelection[i];
                        lca = lcaCalculator.LeastCommonAncestor(tree.GetRoot(), lca, node);
                    }
                    SyntaxNode snode = lca.AsNode();
                    nodes.Add(snode);
                }
            }

            globalLCA = nodes[0];
            for (int i = 1; i < nodes.Count; i++)
            {
                SyntaxNodeOrToken node = nodes[i];
                globalLCA = lcaCalculator.LeastCommonAncestor(tree.GetRoot(), globalLCA, node);
            }
            globalNode = globalLCA.AsNode();

            List<Tuple<SyntaxNode, SyntaxNode>> result = CalculatePositionAndSyntaxKind(tree.GetRoot(), treeAfter.GetRoot(), nodes, globalNode);
            return result;
        }

        /// <summary>
        /// Calculate selection position index and syntax node
        /// </summary>
        /// <param name="treeBefore">Syntax tree root</param>
        /// <param name="nodes">Selected nodes</param>
        /// <returns>Index and syntax node</returns>
        private List<Tuple<SyntaxNode, SyntaxNode>> CalculatePositionAndSyntaxKind(SyntaxNode treeBefore, SyntaxNode treeAfter, List<SyntaxNode> nodes, SyntaxNode globalLCA)
        {
            var treeDescendents = from snode in treeBefore.DescendantNodes()
                                  where snode.CSharpKind() == globalLCA.CSharpKind()
                                  select snode;

            LCA<SyntaxNode> lcaCalculator = new LCA<SyntaxNode>();

            int position = Position(globalLCA, treeDescendents);

            List<Tuple<int, SyntaxNode>> pairsNodes = new List<Tuple<int, SyntaxNode>>();
            Tuple<int, SyntaxNode> t = Tuple.Create(position, globalLCA);
            pairsNodes.Add(t);

            List<Tuple<SyntaxNode, SyntaxNode>> tuplePair = CalculatePair(treeAfter, pairsNodes); //least common ancestor before and after

            List<SyntaxNode> update = new List<SyntaxNode>();

            foreach (CodeLocation location in EditorController.GetInstance().Locations)
            {
                SyntaxNode selection = location.Region.Node;

                var decedents = from snode in tuplePair.First().Item1.DescendantNodes()
                                where snode.Span.Start == selection.Span.Start && snode.Span.Length == selection.Span.Length && snode.CSharpKind() == selection.CSharpKind()
                                select snode;
                update.AddRange(decedents);
            }

            List<SyntaxNode> snodesList = tuplePair.First().Item1.ChildNodes().ToList();
            List<int> positions = new List<int>();

            for (int i = 0; i < snodesList.Count; i++)
            {
                foreach (SyntaxNode snode in update)
                {
                    if (snodesList[i].Equals(snode))
                    {
                        positions.Add(i);
                    }
                }
            }

            List<SyntaxNode> snodeList2 = tuplePair.First().Item2.ChildNodes().ToList();
            List<Tuple<SyntaxNode, SyntaxNode>> tuples = new List<Tuple<SyntaxNode, SyntaxNode>>();
            foreach (int i in positions)
            {
                Tuple<SyntaxNode, SyntaxNode> tuple = Tuple.Create(snodesList[i], snodeList2[i]);
                tuples.Add(tuple);
            }

            return tuples;
     }

        /// <summary>
        /// Topological position of a specific node in a node list
        /// </summary>
        /// <param name="node">Syntax node</param>
        /// <param name="descendants">List of descendant</param>
        /// <returns>Position of the node on the list</returns>
        private int Position(SyntaxNode node, IEnumerable<SyntaxNode> descendants)
        {
            int i = 0;
            foreach (SyntaxNode parent in descendants)
            {
                if (parent.Equals(node))
                {
                    return i;
                }
                i++;
            }
            return i - 1;
        }

        /// <summary>
        /// Calculate pair transformation
        /// </summary>
        /// <param name="tree">Syntax tree root</param>
        /// <param name="nodes">Index of nodes with respective positions</param>
        /// <returns>Transformation pair</returns>
        private List<Tuple<SyntaxNode, SyntaxNode>> CalculatePair(SyntaxNode tree, List<Tuple<int, SyntaxNode>> nodes)
        {
            List<Tuple<SyntaxNode, SyntaxNode>> kinds = new List<Tuple<SyntaxNode, SyntaxNode>>();
            foreach (Tuple<int, SyntaxNode> pair in nodes)
            {
                var descendents = from node in tree.DescendantNodes()
                                  where node.CSharpKind() == pair.Item2.CSharpKind()
                                  select node;

                Tuple<SyntaxNode, SyntaxNode> tuple = Tuple.Create(pair.Item2, descendents.ElementAt(pair.Item1));
                kinds.Add(tuple);
            }

            return kinds;
        }

        public abstract List<SyntaxNode> SyntaxNodes(string sourceCode, List<TRegion> list);
    }
}

//internal List<Tuple<SyntaxNode, SyntaxNode>> SyntaxNodesRegion(string sourceBefore, string sourceAfter, List<CodeLocation> locations)
//{
//    if (sourceBefore == null || sourceAfter == null) { throw new ArgumentNullException("source code cannot be null"); }

//    SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceBefore);
//    SyntaxTree treeAfter = CSharpSyntaxTree.ParseText(EditorController.GetInstance().CurrentViewCodeAfter);
//    List<SyntaxNode> nodes = new List<SyntaxNode>();

//    SyntaxNodeOrToken globalLCA;
//    SyntaxNode globalNode = null;
//    LCA<SyntaxNodeOrToken> lcaCalculator = new LCA<SyntaxNodeOrToken>();
//    foreach (CodeLocation location in locations)
//    {
//        if (sourceBefore.Equals(location.SourceCode))
//        {
//            List<SyntaxNode> nodesSelection = new List<SyntaxNode>();
//            var descedentsBegin = from node in tree.GetRoot().DescendantNodes()
//                                  where node.SpanStart == location.Region.Start
//                                  select node;
//            nodesSelection.AddRange(descedentsBegin);

//            var descedentsEnd = from node in tree.GetRoot().DescendantNodes()
//                                where node.SpanStart + node.Span.Length == location.Region.Start + location.Region.Length
//                                select node;
//            nodesSelection.AddRange(descedentsEnd);

//            SyntaxNodeOrToken lca = nodesSelection[0];
//            for (int i = 1; i < nodesSelection.Count; i++)
//            {
//                SyntaxNodeOrToken node = nodesSelection[i];
//                lca = lcaCalculator.LeastCommonAncestor(tree.GetRoot(), lca, node);
//            }
//            SyntaxNode snode = lca.AsNode();
//            nodes.Add(snode);
//        }
//    }

//    globalLCA = nodes[0];
//    for (int i = 1; i < nodes.Count; i++)
//    {
//        SyntaxNodeOrToken node = nodes[i];
//        globalLCA = lcaCalculator.LeastCommonAncestor(tree.GetRoot(), globalLCA, node);
//    }
//    globalNode = globalLCA.AsNode();

//    //List<Tuple<int, SyntaxNode>> locationAndKing = CalculatePositionAndSyntaxKind(tree.GetRoot(), treeAfter.GetRoot(), nodes, globalNode);
//    //List<Tuple<SyntaxNode, SyntaxNode>> result = CalculatePair(treeAfter.GetRoot(), locationAndKing);
//    List<Tuple<SyntaxNode, SyntaxNode>> result = CalculatePositionAndSyntaxKind(tree.GetRoot(), treeAfter.GetRoot(), nodes, globalNode);
//    return result;
//}

///// <summary>
///// Calculate selection position index and syntax node
///// </summary>
///// <param name="treeBefore">Syntax tree root</param>
///// <param name="nodes">Selected nodes</param>
///// <returns>Index and syntax node</returns>
//private List<Tuple<SyntaxNode, SyntaxNode>> CalculatePositionAndSyntaxKind(SyntaxNode treeBefore, SyntaxNode treeAfter, List<SyntaxNode> nodes, SyntaxNode globalLCA)
//{
//    var treeDescendents = from snode in treeBefore.DescendantNodes()
//                          where snode.CSharpKind() == globalLCA.CSharpKind()
//                          select snode;

//    LCA<SyntaxNode> lcaCalculator = new LCA<SyntaxNode>();

//    int position = Position(globalLCA, treeDescendents);

//    List<Tuple<int, SyntaxNode>> pairsNodes = new List<Tuple<int, SyntaxNode>>();
//    Tuple<int, SyntaxNode> t = Tuple.Create(position, globalLCA);
//    pairsNodes.Add(t);

//    List<Tuple<SyntaxNode, SyntaxNode>> tuplePair = CalculatePair(treeAfter, pairsNodes); //least common ancestor before and after

//    Tuple<string, string> estring = Tuple.Create(treeBefore.GetText().ToString(), treeAfter.GetText().ToString());
//    Tuple<ListNode, ListNode> elnode = ASTProgram.Example(estring);

//    List<SyntaxNodeOrToken> list = AnalizeDiff2(elnode);

//    List<SyntaxNode> nodesList = GetElements(list, treeBefore);
//    nodesList.AddRange(nodes);

//    List<SyntaxNode> parentNodes = ConnectedEdition(nodesList, tuplePair.First().Item1);
//    parentNodes = FilterParent(parentNodes, list);
//    List<Tuple<int, SyntaxNode>> indAndKind = CalculatePositionAndSyntaxKind(treeBefore, parentNodes);

//    //remove
//    Tuple<ListNode, ListNode> elnode2 = Tuple.Create(elnode.Item2, elnode.Item1);
//    List<SyntaxNodeOrToken> list2 = AnalizeDiff2(elnode2);
//    List<SyntaxNode> nodesList2 = GetElements(list2, treeAfter);

//    List<SyntaxNode> parentNodes2 = ConnectedEdition(nodesList2, tuplePair.First().Item2);

//    List<Tuple<SyntaxNode, SyntaxNode>> pts = new List<Tuple<SyntaxNode, SyntaxNode>>();
//    for (int i = 0; i < parentNodes.Count; i++)
//    {
//        Tuple<SyntaxNode, SyntaxNode> pt = Tuple.Create(parentNodes[i], parentNodes2[i]);
//        pts.Add(pt);

//    }
//    pts = EquivaleInKind(pts, tuplePair.First().Item1, tuplePair.First().Item2);
//    return pts;
//    //remove indAndKind
//    //return 
//}

///// <summary>
///// Filter parents
///// </summary>
///// <param name="parentNodes">Parent nodes</param>
///// <param name="list">List of edited regions</param>
///// <returns>Filtered nodes</returns>
//private List<SyntaxNode> FilterParent(List<SyntaxNode> parentNodes, List<SyntaxNodeOrToken> list)
//{
//    List<SyntaxNode> nodes = new List<SyntaxNode>();
//    foreach (SyntaxNode node in parentNodes)
//    {
//        bool edited = false;
//        foreach (SyntaxNodeOrToken st in list)
//        {
//            if (node.SpanStart <= st.SpanStart && st.SpanStart <= node.SpanStart + node.Span.Length)
//            {
//                edited = true;
//                break;
//            }
//        }

//        if (edited)
//        {
//            nodes.Add(node);
//        }
//    }

//    return nodes;
//}

///// <summary>
///// Get nodes of a region
///// </summary>
///// <param name="sourceCode">Source code</param>
///// <param name="region">Region</param>
///// <returns>Corresponding syntax node in the source code</returns>
//internal SyntaxNode SyntaxNodesRegion(string sourceCode, TRegion region)
//{
//    if (sourceCode == null) { throw new Exception("source code cannot be null"); }

//    SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);

//    List<SyntaxNode> nodesSelection = new List<SyntaxNode>();
//    var descedentsBegin = from node in tree.GetRoot().DescendantNodes()
//                          where node.SpanStart == region.Start
//                          select node;
//    nodesSelection.AddRange(descedentsBegin);

//    var descedentsEnd = from node in tree.GetRoot().DescendantNodes()
//                        where node.SpanStart + node.Span.Length == region.Start + region.Length
//                        select node;
//    nodesSelection.AddRange(descedentsEnd);

//    LCA<SyntaxNodeOrToken> lcaCalculator = new LCA<SyntaxNodeOrToken>();
//    SyntaxNodeOrToken lca = nodesSelection[0];
//    for (int i = 1; i < nodesSelection.Count; i++)
//    {
//        SyntaxNodeOrToken node = nodesSelection[i];
//        lca = lcaCalculator.LeastCommonAncestor(tree.GetRoot(), lca, node);
//    }
//    SyntaxNode snode = lca.AsNode();

//    return snode;
//}

///// <summary>
///// Get elements
///// </summary>
///// <param name="list">List of nodes</param>
///// <param name="tree">Syntax tree</param>
///// <returns>Corresponding nodes in the tree</returns>
//private List<SyntaxNode> GetElements(List<SyntaxNodeOrToken> list, SyntaxNode tree)
//{
//    List<SyntaxNode> nodesList = new List<SyntaxNode>();
//    foreach (SyntaxNodeOrToken st in list)
//    {
//        var descendents = from snode in tree.DescendantNodes()
//                          where st.Span.Start == snode.SpanStart
//                          select snode;
//        nodesList.AddRange(descendents);
//    }
//    return nodesList;
//}

//private List<SyntaxNodeOrToken> AnalizeDiff2(Tuple<ListNode, ListNode> elnode)
//{
//    //remove
//    List<ComparisonResult<ComparisonObject>> result = SynthesisManager.Differ2(elnode.Item1, elnode.Item2);
//    //List<ComparisonResult<ComparisonObject>> lint2 = SynthesisManager.Differ2(elnode.Item2, elnode.Item1);
//    //remove

//    List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();
//    for (int i = 0; i < result.Count; i++)
//    {
//        ComparisonResult<ComparisonObject> r = result[i];
//        if (r.ModificationType.Equals(ModificationType.Deleted))
//        {
//            list.Add(r.DataCompared.Token.token);
//        }
//    }

//    return list;
//}

//private List<SyntaxNode> ConnectedEdition(List<SyntaxNode> nodesList, SyntaxNode tree)
//{
//    LCA<SyntaxNode> lcaCalculator = new LCA<SyntaxNode>();
//    Dictionary<int, SyntaxNodeOrToken> dic = new Dictionary<int, SyntaxNodeOrToken>();
//    List<Tuple<int, SyntaxNode>> kinds = new List<Tuple<int, SyntaxNode>>();

//    List<int> ancestrors = new List<int>(new int[nodesList.Count]);

//    int anc = 0;
//    for (int i = 0; i < nodesList.Count; i++)
//    {
//        if (ancestrors[i] == 0)
//        {
//            ancestrors[i] = ++anc;
//            dic[anc] = nodesList[i];
//            for (int j = i + 1; j < nodesList.Count; j++)
//            {
//                if (ancestrors[j] == 0)
//                {
//                    SyntaxNodeOrToken lca = lcaCalculator.LeastCommonAncestor(tree, dic[anc], nodesList[j]);
//                    if (!lca.Equals(tree))
//                    {
//                        ancestrors[j] = anc;
//                        dic[anc] = lca;
//                    }
//                }
//            }
//        }
//    }

//    List<SyntaxNode> parentNodes = new List<SyntaxNode>();
//    foreach (KeyValuePair<int, SyntaxNodeOrToken> item in dic)
//    {
//        parentNodes.Add(item.Value.AsNode());
//    }
//    return parentNodes;
//}


///// <summary>
///// Convert syntax nodes to its equivalent kinds
///// </summary>
///// <param name="pairOfMatches">Nodes</param>
///// <param name="firstRoot">First root</param>
///// <param name="secondRoot">Second root</param>
///// <returns></returns>
//private List<Tuple<SyntaxNode, SyntaxNode>> EquivaleInKind(List<Tuple<SyntaxNode, SyntaxNode>> pairOfMatches, SyntaxNode firstRoot, SyntaxNode secondRoot)
//{
//    List<Tuple<SyntaxNode, SyntaxNode>> tuples = new List<Tuple<SyntaxNode, SyntaxNode>>();
//    foreach (Tuple<SyntaxNode, SyntaxNode> tuple in pairOfMatches)
//    {
//        if (tuple.Item1.CSharpKind() == tuple.Item2.CSharpKind())
//        {
//            tuples.Add(tuple); continue;
//        }

//        SyntaxNode parent1 = tuple.Item1;
//        List<SyntaxNode> sn1 = new List<SyntaxNode>();
//        while (!parent1.Equals(firstRoot))
//        {
//            sn1.Add(parent1);
//            parent1 = parent1.Parent;
//        }

//        SyntaxNode parent2 = tuple.Item2;
//        List<SyntaxNode> sn2 = new List<SyntaxNode>();
//        while (!parent2.Equals(secondRoot))
//        {
//            sn2.Add(parent2);
//            parent2 = parent2.Parent;
//        }

//        foreach (SyntaxNode i in sn1)
//        {
//            bool found = false;
//            foreach (SyntaxNode j in sn2)
//            {
//                if (i.CSharpKind() == j.CSharpKind())
//                {
//                    Tuple<SyntaxNode, SyntaxNode> te = Tuple.Create(i, j);
//                    tuples.Add(te);
//                    found = true;
//                    break;
//                }
//            }
//            if (found) break;
//        }

//    }
//    return tuples;
//}

///// <summary>
///// Calculate selection position index and syntax node
///// </summary>
///// <param name="tree">Syntax tree root</param>
///// <param name="nodes">Selected nodes</param>
///// <returns>Index and syntax node</returns>
//private List<Tuple<int, SyntaxNode>> CalculatePositionAndSyntaxKind(SyntaxNode tree, List<SyntaxNode> nodes)
//{
//    List<Tuple<int, SyntaxNode>> kinds = new List<Tuple<int, SyntaxNode>>();
//    foreach (SyntaxNode node in nodes)
//    {
//        var descendents = from snode in tree.DescendantNodes()
//                          where snode.CSharpKind() == node.CSharpKind()
//                          select snode;

//        int i = 0;
//        descendents = descendents.ToList();
//        foreach (SyntaxNode parent in descendents)
//        {
//            if (parent.Equals(node))
//            {
//                Tuple<int, SyntaxNode> tuple = Tuple.Create(i, node);
//                kinds.Add(tuple);
//            }
//            i++;
//        }
//    }
//    return kinds;
//}

//private List<SyntaxNodeOrToken> AnalizeDiff(Tuple<ListNode, ListNode> elnode)
//{
//    //remove
//    List<ComparisonResult<ComparisonObject>> result = SynthesisManager.Differ2(elnode.Item1, elnode.Item2);
//    //List<ComparisonResult<ComparisonObject>> lint2 = SynthesisManager.Differ2(elnode.Item2, elnode.Item1);
//    //remove

//    List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();
//    for (int i = 0; i < result.Count; i++)
//    {
//        ComparisonResult<ComparisonObject> r = result[i];
//        if (r.ModificationType.Equals(ModificationType.Deleted))
//        {
//            list.Add(r.DataCompared.Token.token);
//        }

//        if (r.ModificationType.Equals(ModificationType.Inserted))
//        {
//            if (i  < result.Count + 1)
//            {
//                list.Add(result[i + 1].DataCompared.Token.token);
//            }
//        }
//    }

//    return list;
//}

///// <summary>
///// Elements with syntax kind in the source code
///// </summary>
///// <param name="sourceCode">Source code</param>
///// <param name="kind">Syntax kind</param>
///// <returns>Elements with syntax kind in the source code</returns>
//public static List<SyntaxNode> SyntaxElements(string sourceCode, SyntaxNode root)
//{
//    if (sourceCode == null)
//    {
//        throw new Exception("source code cannot be null");
//    }

//    SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);

//    List<SyntaxNode> nodes = new List<SyntaxNode>();
//    var descendentNodes = from node in root.DescendantNodes()
//                          select node;

//    foreach (SyntaxNode node in descendentNodes)
//    {
//        nodes.Add(node);
//    }

//    return nodes;
//}


//public List<Tuple<ListNode, ListNode>> Extract(List<TRegion> list)
//{
//    List<Tuple<ListNode, ListNode>> examples = new List<Tuple<ListNode, ListNode>>();

//    TRegion region = list[0];
//    List<SyntaxNode> statements = SyntaxNodes(region.Parent.Text, list);
//    Dictionary<SyntaxNode, Tuple<ListNode, ListNode>> methodsDic = new Dictionary<SyntaxNode, Tuple<ListNode, ListNode>>();
//    Dictionary<TRegion, SyntaxNode> pairs = ChoosePairs(statements, list);
//    foreach (KeyValuePair<TRegion, SyntaxNode> pair in pairs)
//    {
//        TRegion re = pair.Key;
//        SyntaxNode node = pair.Value;
//        //foreach (SyntaxNode node in statements)
//        //{
//        //    TextSpan span = node.Span;
//        //    int start = span.Start;
//        //    int length = span.Length;
//        //    foreach (TRegion re in list)
//        //    {
//        //        if (start <= re.Start && re.Start <= start + length)
//        //        {
//        Tuple<ListNode, ListNode> val = null;
//        Tuple<ListNode, ListNode> te = Example(node, re);
//        if (!methodsDic.TryGetValue(node, out val))
//        {
//            examples.Add(te);
//            methodsDic.Add(node, te);
//        }
//        else
//        {
//            val.Item2.List.AddRange(te.Item2.List);
//        }
//    }
//    //        }
//    //    }
//    //}
//    return examples;
//}

//private List<Tuple<SyntaxNode, SyntaxNode>> EquivaleInKind(List<Tuple<SyntaxNode, SyntaxNode>> pairsMatches, SyntaxNode item1, SyntaxNode item2)
//{
//    List<Tuple<SyntaxNode, SyntaxNode>> tuples = new List<Tuple<SyntaxNode, SyntaxNode>>();
//    foreach (Tuple<SyntaxNode, SyntaxNode> tuple in pairsMatches)
//    {
//        if (tuple.Item1.CSharpKind() == tuple.Item2.CSharpKind())
//        {
//            tuples.Add(tuple); continue;
//        }

//        SyntaxNode parent1 = tuple.Item1;
//        while (parent1.CSharpKind() != tuple.Item2.CSharpKind() && !parent1.Equals(item1))
//        {
//            parent1 = parent1.Parent;
//        }

//        SyntaxNode parent2 = tuple.Item2;
//        while (parent2.CSharpKind() != tuple.Item1.CSharpKind() && !parent2.Equals(item2))
//        {
//            parent2 = parent2.Parent;
//        }

//        if (parent1.Equals(item1)) //then pair 1 is bigger than pair 2
//        {
//            Tuple<SyntaxNode, SyntaxNode> t = Tuple.Create(tuple.Item1, parent2);
//            tuples.Add(t);
//        }
//        else
//        {
//            Tuple<SyntaxNode, SyntaxNode> t = Tuple.Create(parent1, tuple.Item2);
//            tuples.Add(t);
//        }
//    }
//    return tuples;
//}

///// <summary>
///// Calculate selection position index and syntax node
///// </summary>
///// <param name="treeBefore">Syntax tree root</param>
///// <param name="nodes">Selected nodes</param>
///// <returns>Index and syntax node</returns>
//private List<Tuple<int, SyntaxNode>> CalculatePositionAndSyntaxKind(SyntaxNode treeBefore, SyntaxNode treeAfter, List<SyntaxNode> nodes, SyntaxNode globalLCA)
//{
//    var treeDescendents = from snode in treeBefore.DescendantNodes()
//                          where snode.CSharpKind() == globalLCA.CSharpKind()
//                          select snode;

//    LCA<SyntaxNode> lcaCalculator = new LCA<SyntaxNode>();

//    int position = Position(globalLCA, treeDescendents);

//    List<Tuple<int, SyntaxNode>> pairsNodes = new List<Tuple<int, SyntaxNode>>();
//    Tuple<int, SyntaxNode> t = Tuple.Create(position, globalLCA);
//    pairsNodes.Add(t);

//    List<Tuple<SyntaxNode, SyntaxNode>> tuplePair = CalculatePair(treeAfter, pairsNodes); //least common ancestor before and after

//    Tuple<string, string> estring = Tuple.Create(treeBefore.GetText().ToString(), treeAfter.GetText().ToString());
//    Tuple<ListNode, ListNode> elnode = ASTProgram.Example(estring);

//    List<SyntaxNodeOrToken> list = AnalizeDiff(elnode);

//    List<SyntaxNode> nodesList = GetElements(list, treeBefore);
//    nodesList.AddRange(nodes);

//    List<SyntaxNode> parentNodes = ConnectedEdition(nodesList, tuplePair);
//    List<Tuple<int, SyntaxNode>> indAndKind = CalculatePositionAndSyntaxKind(treeBefore, parentNodes);

//    return indAndKind;
//}


//important
///// <summary>
///// Calculate selection position index and syntax node
///// </summary>
///// <param name="treeBefore">Syntax tree root</param>
///// <param name="nodes">Selected nodes</param>
///// <returns>Index and syntax node</returns>
//private List<Tuple<int, SyntaxNode>> CalculatePositionAndSyntaxKind(SyntaxNode treeBefore, SyntaxNode treeAfter, List<SyntaxNode> nodes, SyntaxNode globalLCA)
//{
//    var treeDescendents = from snode in treeBefore.DescendantNodes()
//                          where snode.CSharpKind() == globalLCA.CSharpKind()
//                          select snode;

//    LCA<SyntaxNode> lcaCalculator = new LCA<SyntaxNode>();

//    int position = Position(globalLCA, treeDescendents);

//    List<Tuple<int, SyntaxNode>> pairsNodes = new List<Tuple<int, SyntaxNode>>();
//    Tuple<int, SyntaxNode> t = Tuple.Create(position, globalLCA);
//    pairsNodes.Add(t);

//    List<Tuple<SyntaxNode, SyntaxNode>> tuplePair = CalculatePair(treeAfter, pairsNodes); //least common ancestor before and after

//    Tuple<string, string> estring = Tuple.Create(treeBefore.GetText().ToString(), treeAfter.GetText().ToString());
//    Tuple<ListNode, ListNode> elnode = ASTProgram.Example(estring);

//    List<SyntaxNodeOrToken> list = AnalizeDiff(elnode);

//    List<SyntaxNode> nodesList = new List<SyntaxNode>();
//    foreach (SyntaxNodeOrToken st in list)
//    {
//        var descendents = from snode in treeBefore.DescendantNodes()
//                          where st.Span.Start == snode.SpanStart
//                          select snode;
//        nodesList.AddRange(descendents);
//    }

//    nodesList.AddRange(nodes);

//    //Dictionary<int, SyntaxNodeOrToken> dic = new Dictionary<int, SyntaxNodeOrToken>();
//    //List<Tuple<int, SyntaxNode>> kinds = new List<Tuple<int, SyntaxNode>>();

//    //List<int> ancestrors = new List<int>(new int [nodesList.Count]);

//    //int anc = 0;
//    //for (int i = 0; i < nodesList.Count; i++)
//    //{
//    //    if (ancestrors[i] == 0)
//    //    {
//    //        ancestrors[i] = ++anc;
//    //        dic[anc] = nodesList[i];
//    //        for (int j = i + 1; j < nodesList.Count; j++)
//    //        {
//    //            if (ancestrors[j] == 0)
//    //            {
//    //                SyntaxNodeOrToken lca = lcaCalculator.LeastCommonAncestor(tuplePair.First().Item1, dic[anc], nodesList[j]);
//    //                if (!lca.Equals(tuplePair.First().Item1))
//    //                {
//    //                    ancestrors[j] = anc;
//    //                    dic[anc] = lca;
//    //                }
//    //            }
//    //        }
//    //    }
//    //}

//    //List<SyntaxNode> parentNodes = new List<SyntaxNode>();
//    //foreach (KeyValuePair<int, SyntaxNodeOrToken> item in dic)
//    //{
//    //    parentNodes.Add(item.Value.AsNode());
//    //}
//    List<SyntaxNode> parentNodes = ConnectedEdition(nodesList, tuplePair);
//    List<Tuple<int, SyntaxNode>> indAndKind = CalculatePositionAndSyntaxKind(treeBefore, parentNodes);

//    return indAndKind;
//}

//private List<Tuple<SyntaxNode, SyntaxNode>> EquivaleInKind(List<Tuple<SyntaxNode, SyntaxNode>> pairsMatches, SyntaxNode item1, SyntaxNode item2)
//{
//    List<Tuple<SyntaxNode, SyntaxNode>> tuples = new List<Tuple<SyntaxNode, SyntaxNode>>();
//    foreach (Tuple<SyntaxNode, SyntaxNode> tuple in pairsMatches)
//    {
//        if (tuple.Item1.CSharpKind() == tuple.Item2.CSharpKind())
//        {
//            tuples.Add(tuple); continue;
//        }

//        SyntaxNode parent1 = tuple.Item1;
//        while (parent1.CSharpKind() != tuple.Item2.CSharpKind() && !parent1.Equals(item1))
//        {
//            parent1 = parent1.Parent;
//        }

//        SyntaxNode parent2 = tuple.Item2;
//        while (parent2.CSharpKind() != tuple.Item1.CSharpKind() && !parent2.Equals(item2))
//        {
//            parent2 = parent2.Parent;
//        }

//        if (parent1.Equals(item1)) //then pair 1 is bigger than pair 2
//        {
//            Tuple<SyntaxNode, SyntaxNode> t = Tuple.Create(tuple.Item1, parent2);
//            tuples.Add(t);
//        }
//        else
//        {
//            Tuple<SyntaxNode, SyntaxNode> t = Tuple.Create(parent1, tuple.Item2);
//            tuples.Add(t);
//        }
//    }
//    return tuples;
//}

///// <summary>
///// Calculate selection position index and syntax node
///// </summary>
///// <param name="treeBefore">Syntax tree root</param>
///// <param name="nodes">Selected nodes</param>
///// <returns>Index and syntax node</returns>
//private List<Tuple<SyntaxNode, SyntaxNode>> CalculatePositionAndSyntaxKind(SyntaxNode treeBefore, SyntaxNode treeAfter, List<SyntaxNode> nodes, SyntaxNode globalLCA)
//{
//    var treeDescendents = from snode in treeBefore.DescendantNodes()
//                          where snode.CSharpKind() == globalLCA.CSharpKind()
//                          select snode;

//    LCA<SyntaxNode> lcaCalculator = new LCA<SyntaxNode>();

//    int position = Position(globalLCA, treeDescendents);

//    List<Tuple<int, SyntaxNode>> pairsNodes = new List<Tuple<int, SyntaxNode>>();
//    Tuple<int, SyntaxNode> t = Tuple.Create(position, globalLCA);
//    pairsNodes.Add(t);

//    List<Tuple<SyntaxNode, SyntaxNode>> tuplePair = CalculatePair(treeAfter, pairsNodes); //least common ancestor before and after

//    var childrens01 = tuplePair.First().Item1.ChildNodes();

//    var childrens02 = tuplePair.First().Item2.ChildNodes();

//    List<Tuple<SyntaxNode, SyntaxNode>> pairsMatches = new List<Tuple<SyntaxNode, SyntaxNode>>();

//    for (int i = 0; i < childrens01.Count(); i++)
//    {
//        Tuple<SyntaxNode, SyntaxNode> tsnode = Tuple.Create(childrens01.ElementAt(i), childrens02.ElementAt(i));
//        pairsMatches.Add(tsnode);
//    }

//    return pairsMatches;

//}



//internal List<Tuple<SyntaxNode, SyntaxNode>> SyntaxNodesRegion(string sourceBefore, string sourceAfter, List<CodeLocation> locations)
//{
//    if (sourceBefore == null || sourceAfter == null) { throw new Exception("source code cannot be null"); }

//    SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceBefore);
//    SyntaxTree treeAfter = CSharpSyntaxTree.ParseText(EditorController.GetInstance().CurrentViewCodeAfter);
//    List<SyntaxNode> nodes = new List<SyntaxNode>();

//    foreach (CodeLocation location in locations)
//    {
//        if (sourceBefore.Equals(location.SourceCode))
//        {
//            List<SyntaxNode> nodesSelection = new List<SyntaxNode>();
//            var descedentsBegin = from node in tree.GetRoot().DescendantNodes()
//                                  where node.SpanStart == location.Region.Start
//                                  select node;
//            nodesSelection.AddRange(descedentsBegin);

//            var descedentsEnd = from node in tree.GetRoot().DescendantNodes()
//                                where node.SpanStart + node.Span.Length == location.Region.Start + location.Region.Length
//                                select node;
//            nodesSelection.AddRange(descedentsEnd);

//            LCA<SyntaxNodeOrToken> lcaCalculator = new LCA<SyntaxNodeOrToken>();
//            SyntaxNodeOrToken lca = nodesSelection[0];
//            for (int i = 1; i < nodesSelection.Count; i++)
//            {
//                SyntaxNodeOrToken node = nodesSelection[i];
//                lca = lcaCalculator.LeastCommonAncestor(tree.GetRoot(), lca, node);
//            }
//            SyntaxNode snode = lca.AsNode();
//            nodes.Add(snode);
//        }
//    }

//    List<Tuple<int, SyntaxNode>> locationAndKing = CalculatePositionAndSyntaxKind(tree.GetRoot(), nodes);
//    List<Tuple<SyntaxNode, SyntaxNode>> result = CalculatePair(treeAfter.GetRoot(), locationAndKing);
//    //return nodes;
//    return result;
//}
//important





//internal List<SyntaxNode> SyntaxNodesRegion(string sourceCode, List<TRegion> list)
//{
//    if (sourceCode == null)
//    {
//        throw new Exception("source code cannot be null");
//    }
//    SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);

//    //remove
//    EditorController controller = EditorController.GetInstance();
//    SyntaxTree treeAfter = CSharpSyntaxTree.ParseText(controller.CodeAfter);
//    SyntaxNodesRegion(sourceCode, controller.CodeAfter, controller.locations);
//    //remove

//    List<SyntaxNode> nodes = new List<SyntaxNode>();

//    foreach (TRegion region in list)
//    {
//        List<SyntaxNode> nodesSelection = new List<SyntaxNode>();
//        var descedentsBegin = from node in tree.GetRoot().DescendantNodes()
//                              where node.SpanStart == region.Start
//                              select node;
//        nodesSelection.AddRange(descedentsBegin);

//        var descedentsEnd = from node in tree.GetRoot().DescendantNodes()
//                            where node.SpanStart + node.Span.Length == region.Start + region.Length
//                            select node;
//        nodesSelection.AddRange(descedentsEnd);

//        LCA<SyntaxNodeOrToken> lcaCalculator = new LCA<SyntaxNodeOrToken>();
//        SyntaxNodeOrToken lca = nodesSelection[0];
//        for (int i = 1; i < nodesSelection.Count; i++)
//        {
//            SyntaxNodeOrToken node = nodesSelection[i];
//            lca = lcaCalculator.LeastCommonAncestor(tree.GetRoot(), lca, node);
//        }
//        SyntaxNode snode = lca.AsNode();
//        nodes.Add(snode);
//    }

//    //var result = treeAfter.GetRoot().TrackNodes(nodes);
//    //foreach (SyntaxNode node in result.GetCurrentNodes(nodes[0]))
//    //{
//    //    Console.WriteLine();
//    //}

//    return nodes;
//    //return snode.DescendantNodes().ToList();
//}

/*/// <summary>
       /// Elements with syntax kind in the source code
       /// </summary>
       /// <param name="sourceCode">Source code</param>
       /// <param name="kind">Syntax kind</param>
       /// <returns>Elements with syntax kind in the source code</returns>
       public static List<SyntaxNode> SyntaxElements(string sourceCode, SyntaxKind kind)
       {
           if (sourceCode == null)
           {
               throw new Exception("source code cannot be null");
           }

           SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);

           List<SyntaxNode> nodes = new List<SyntaxNode>();
           var descendentNodes = from node in tree.GetRoot().DescendantNodes()
                                 where node.IsKind(kind)
                                 select node;

           foreach (SyntaxNode node in descendentNodes)
           {
               nodes.Add(node);
           }

           //remove
           //LCA <SyntaxNodeOrToken> lca = new LCA<SyntaxNodeOrToken>();
           //var result = lca.LeastCommonAncestor(tree.GetRoot(), nodes[0], nodes[1]);
           //remove

           //nodes = ASTManager.Nodes(tree.GetRoot(), nodes, kind);
           return nodes;
       }*/


//public List<Tuple<String, String>> ExtractText(List<TRegion> list)
//{
//    List<Tuple<String, String>> examples = new List<Tuple<String, String>>();
//    TRegion region = list[0];
//    List<SyntaxNode> statements = SyntaxNodes(region.Parent.Text, list);
//    Dictionary<String, String> methodsDic = new Dictionary<String, String>();
//    Dictionary<TRegion, SyntaxNode> pairs = ChoosePairs(statements, list);
//    foreach (KeyValuePair<TRegion, SyntaxNode> pair in pairs)
//    {
//        TRegion re = pair.Key;
//        SyntaxNode statment = pair.Value;

//        //foreach (SyntaxNode statement in statements)
//        //{
//        //    TextSpan span = statement.Span;
//        //    int start = span.Start;
//        //    int length = span.Length;
//        //    foreach (TRegion re in list)
//        //    {
//        //        if (start <= re.Start && re.Start <= start + length)
//        //        {
//        String value = null;
//        if (!methodsDic.TryGetValue(statment.GetText().ToString(), out value))
//        {
//            Tuple<String, String> tuple = Tuple.Create(re.Text, statment.GetText().ToString());
//            examples.Add(tuple);
//            methodsDic.Add(statment.GetText().ToString(), value);
//        }
//        //}
//        //    }
//        //}
//    }
//    return examples;
//}