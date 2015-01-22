using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.TextRegion;
using LeastCommonAncestor;
using Spg.LocationCodeRefactoring.Controller;
using Spg.LocationRefactor.Location;
using Microsoft.CodeAnalysis.Text;

namespace LocationCodeRefactoring.Br.Spg.Location
{
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
                //foreach (SyntaxNode node in statements)
                //{
                //    TextSpan span = node.Span;
                //    int start = span.Start;
                //    int length = span.Length;
                //    foreach (TRegion re in list)
                //    {
                //        if (start <= re.Start && re.Start <= start + length)
                //        {
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
            //        }
            //    }
            //}
            return examples;
        }

        /// <summary>
        /// Choose corresponding syntax node for the region
        /// </summary>
        /// <param name="statements">Statement list</param>
        /// <param name="region">Region</param>
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
            if (sourceCode == null)
            {
                throw new Exception("source code cannot be null");
            }
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
            //return snode.DescendantNodes().ToList();

        }

        public abstract List<SyntaxNode> SyntaxNodes(string sourceCode, List<TRegion> list);

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

            //TextSpan span = list[0].Span;
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
        /// <param name="kind">Syntax kind</param>
        /// <returns>Elements with syntax kind in the source code</returns>
        public static List<SyntaxNode> SyntaxElements(string sourceCode, SyntaxNode root)
        {
            if (sourceCode == null)
            {
                throw new Exception("source code cannot be null");
            }

            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);

            List<SyntaxNode> nodes = new List<SyntaxNode>();
            var descendentNodes = from node in root.DescendantNodes()
                                  select node;

            foreach (SyntaxNode node in descendentNodes)
            {
                nodes.Add(node);
            }

            return nodes;
        }

        /// <summary>
        /// Elements with syntax kind in the source code
        /// </summary>
        /// <param name="sourceCode">Source code</param>
        /// <param name="kind">Syntax kind</param>
        /// <returns>Elements with syntax kind in the source code</returns>
        public static List<SyntaxNode> SyntaxElements(string sourceCode, List<TRegion> list)
        {
            if (sourceCode == null || list == null) { throw new Exception("source code and selection list cannot be null"); }
            if (list.Count() == 0) { throw new Exception("Selection list cannot be empty"); }

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

            return snode.DescendantNodes().ToList();
        }

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

        internal List<Tuple<SyntaxNode, SyntaxNode>> SyntaxNodesRegion(string sourceBefore, string sourceAfter, List<CodeLocation> locations)
        {
            if (sourceBefore == null || sourceAfter == null) { throw new Exception("source code cannot be null"); }

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

            //List<Tuple<int, SyntaxNode>> locationAndKing = CalculatePositionAndSyntaxKind(tree.GetRoot(), treeAfter.GetRoot(), nodes, globalNode);
            List<Tuple<SyntaxNode, SyntaxNode>> result = CalculatePositionAndSyntaxKind(tree.GetRoot(), treeAfter.GetRoot(), nodes, globalNode);
            //return nodes;
            return result;
        }

        /// <summary>
        /// Calculate selection position index and syntax node
        /// </summary>
        /// <param name="tree">Syntax tree root</param>
        /// <param name="nodes">Selected nodes</param>
        /// <returns>Index and syntax node</returns>
        private List<Tuple<int, SyntaxNode>> CalculatePositionAndSyntaxKind(SyntaxNode tree, List<SyntaxNode> nodes)
        {
            List<Tuple<int, SyntaxNode>> kinds = new List<Tuple<int, SyntaxNode>>();
            foreach (SyntaxNode node in nodes)
            {
                var descendents = from snode in tree.DescendantNodes()
                                  where snode.CSharpKind() == node.CSharpKind()
                                  select snode;

                int i = 0;
                foreach (SyntaxNode parent in descendents)
                {
                    if (parent.Equals(node))
                    {
                        Tuple<int, SyntaxNode> tuple = Tuple.Create(i, node);
                        kinds.Add(tuple);
                    }
                    i++;
                }
            }
            return kinds;
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

            Tuple<string, string> estring = Tuple.Create(treeBefore.GetText().ToString(), treeAfter.GetText().ToString());
            Tuple<ListNode, ListNode> elnode = ASTProgram.Example(estring);

            var changes = treeBefore.SyntaxTree.GetChanges(treeAfter.SyntaxTree);
            List<SyntaxNode> nodesList = new List<SyntaxNode>();
            foreach (TextChange change in changes)
            {
                var descendents = from snode in treeAfter.DescendantNodes()
                                  where change.Span.Start <= snode.SpanStart && snode.SpanStart <= change.Span.End
                                  select snode;
                nodesList.AddRange(descendents);
            }

            Dictionary<int, SyntaxNodeOrToken> dic = new Dictionary<int, SyntaxNodeOrToken>();
            List<Tuple<int, SyntaxNode>> kinds = new List<Tuple<int, SyntaxNode>>();

            int lcas = 0;
            SyntaxNodeOrToken lca = nodesList.First();
            dic[lcas] = lca;
            for (int i = 1; i < nodesList.Count; i++)
            {
                SyntaxNode node = nodesList[i];
                if (node.SpanStart <= lca.SpanStart && lca.Span.Length <= node.Span.Length)
                {
                    lca = node;
                }
                else
                {
                    lca = lcaCalculator.LeastCommonAncestor(tuplePair.First().Item2, lca, node);
                }

                if (lca.Equals(tuplePair.First().Item2))
                {
                    lca = node;
                    lcas++;
                }
                else
                {
                    dic[lcas] = lca;
                }
            }

            List<Tuple<SyntaxNode, SyntaxNode>> pairsMatches = new List<Tuple<SyntaxNode, SyntaxNode>>();
            foreach (KeyValuePair<int, SyntaxNodeOrToken> item in dic)
            {
                Tuple<SyntaxNode, SyntaxNode> titem = Tuple.Create(nodes[item.Key], item.Value.AsNode());
                pairsMatches.Add(titem);
            }

            pairsMatches = EquivaleInKind(pairsMatches, tuplePair.First().Item1, tuplePair.First().Item2);

            return pairsMatches;
        }

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

        private List<Tuple<SyntaxNode, SyntaxNode>> EquivaleInKind(List<Tuple<SyntaxNode, SyntaxNode>> pairsMatches, SyntaxNode item1, SyntaxNode item2)
        {
            List<Tuple<SyntaxNode, SyntaxNode>> tuples = new List<Tuple<SyntaxNode, SyntaxNode>>();
            foreach (Tuple<SyntaxNode, SyntaxNode> tuple in pairsMatches)
            {
                if (tuple.Item1.CSharpKind() == tuple.Item2.CSharpKind())
                {
                    tuples.Add(tuple); continue;
                }

                SyntaxNode parent1 = tuple.Item1;
                while (parent1.CSharpKind() != tuple.Item2.CSharpKind() && !parent1.Equals(item1))
                {
                    parent1 = parent1.Parent;
                }

                SyntaxNode parent2 = tuple.Item2;
                while (parent2.CSharpKind() != tuple.Item1.CSharpKind() && !parent2.Equals(item2))
                {
                    parent2 = parent2.Parent;
                }

                if (parent1.Equals(item1)) //then pair 1 is bigger than pair 2
                {
                    Tuple<SyntaxNode, SyntaxNode> t = Tuple.Create(tuple.Item1, parent2);
                    tuples.Add(t);
                }
                else
                {
                    Tuple<SyntaxNode, SyntaxNode> t = Tuple.Create(parent1, tuple.Item2);
                    tuples.Add(t);
                }
            }
            return tuples;
        }

        private int Position(SyntaxNode node, IEnumerable<SyntaxNode> descendents)
        {
            int i = 0;
            foreach (SyntaxNode parent in descendents)
            {
                if (parent.Equals(node))
                {
                    Tuple<int, SyntaxNode> tuple = Tuple.Create(i, node);
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

        internal SyntaxNode SyntaxNodesRegion(string sourceCode, TRegion region)
        {
            if (sourceCode == null)
            {
                throw new Exception("source code cannot be null");
            }
            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);

            //List<SyntaxNode> nodes = new List<SyntaxNode>();

            List<SyntaxNode> nodesSelection = new List<SyntaxNode>();
            var descedentsBegin = from node in tree.GetRoot().DescendantNodes()
                                  where node.SpanStart == region.Start
                                  select node;
            nodesSelection.AddRange(descedentsBegin);

            var descedentsEnd = from node in tree.GetRoot().DescendantNodes()
                                where node.SpanStart + node.Span.Length == region.Start + region.Length
                                select node;
            nodesSelection.AddRange(descedentsEnd);

            LCA<SyntaxNodeOrToken> lcaCalculator = new LCA<SyntaxNodeOrToken>();
            SyntaxNodeOrToken lca = nodesSelection[0];
            for (int i = 1; i < nodesSelection.Count; i++)
            {
                SyntaxNodeOrToken node = nodesSelection[i];
                lca = lcaCalculator.LeastCommonAncestor(tree.GetRoot(), lca, node);
            }
            SyntaxNode snode = lca.AsNode();
            //nodes.Add(snode);

            return snode;
            //return snode.DescendantNodes().ToList();
        }
    }
}







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