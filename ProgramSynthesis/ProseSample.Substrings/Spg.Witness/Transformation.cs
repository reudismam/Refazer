using System;
using System.Collections.Generic;
using System.Linq;
using DbscanImplementation;
using LongestCommonSubsequence;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using ProseSample.Substrings.Spg.Bean;
using TreeEdit.Spg.Clustering;
using TreeEdit.Spg.ConnectedComponents;
using TreeEdit.Spg.Print;
using TreeEdit.Spg.Script;
using TreeEdit.Spg.TreeEdit.Mapping;
using TreeEdit.Spg.TreeEdit.Update;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Witness
{
    public class Transformation
    {
        /// <summary>
        /// Mapped nodes on the before after tree
        /// </summary>
        public static Dictionary<ITreeNode<SyntaxNodeOrToken>, ITreeNode<SyntaxNodeOrToken>> Mapping { get; set; }

        /// <summary>
        /// Witness function to segment the script in a list of edit operations
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Grammar parameter</param>
        /// <param name="spec">Example specification</param>
        public static ExampleSpec ScriptEdits(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var editsExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var script = (Script)spec.Examples[input];
                var edits = script.Edits;
                //edits = edits.GetRange(0, 1);
                editsExamples[input] = edits;
            }
            return new ExampleSpec(editsExamples);
        }

        /// <summary>
        /// Cluster edit operations and return list of clustered elements.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Grammar parameter</param>
        /// <param name="spec">Example specification</param>
        public static SubsequenceSpec ApplyPatch(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<List<Script>>();
                var inpTreeNode = (Node)input[rule.Body[0]];
                var inpTree = inpTreeNode.Value.Value;
                foreach (SyntaxNodeOrToken outTree in spec.DisjunctiveExamples[input])
                {
                    var script = Script(inpTree, outTree);
                    PrintScript(script);
                    var ccs = ConnectedComponentMannager<SyntaxNodeOrToken>.ConnectedComponents(script);
                    //ccs = CompactConnectedComponentsBasedOnAncestor(script, inpTreeNode.Value);

                    kMatches = ClusterScript(ccs);
                    kMatches.First().ForEach(o => PrintScript(o.Edits));
                    kMatches = kMatches.Select(o => CompactScript(o, inpTreeNode.Value)).ToList();
                    kMatches.First().ForEach(o => PrintScript(o.Edits));
                }
                //kMatches = kMatches.GetRange(0, 1);
                kExamples[input] = new List<List<List<Script>>> { kMatches };
            }
            var subsequence = new SubsequenceSpec(kExamples);
            return subsequence;
        }

        //private static List<List<EditOperation<SyntaxNodeOrToken>>> CompactConnectedComponentsBasedOnAncestor(List<EditOperation<SyntaxNodeOrToken>> script, ITreeNode<SyntaxNodeOrToken> inpTree)
        //{
        //    var tree = ConverterHelper.MakeACopy(inpTree);
        //    var treeUpdate = new TreeUpdate(tree);
        //    foreach (var s in script)
        //    {
        //        treeUpdate.ProcessEditOperation(s);
        //    }

        //    foreach(var v )
        //}


        public static void PrintScript(List<Edit<SyntaxNodeOrToken>> script)
        {
            string s = script.Aggregate("", (current, v) => current + (v + "\n"));
        }

        public static void PrintScript(List<EditOperation<SyntaxNodeOrToken>> script)
        {
            string s = script.Aggregate("", (current, v) => current + (v + "\n"));
        }

        //private static int Less(List<Script> scripts1, List<Script> scripts2)
        //{
        //    var s1First = scripts1.First().Edits.First().EditOperation;
        //    var s2First = scripts2.First().Edits.First().EditOperation;
        //    var aToComapre = s1First.Parent ?? s1First.T1Node;
        //    var bToCompare = s2First.Parent ?? s2First.T1Node;
        //    if (aToComapre.Value.SpanStart < bToCompare.Value.SpanStart) return -1;
        //    return aToComapre.Value.SpanStart > bToCompare.Value.SpanStart ? 1 : 0;
        //}

        ///// <summary>
        ///// Segment the edit script in nodes
        ///// </summary>
        ///// <param name="rule">Grammar rule</param>
        ///// <param name="parameter">Rule parameter</param>
        ///// <param name="spec">Example specification</param>
        //public static SubsequenceSpec EditMapTNode(GrammarRule rule, int parameter, SubsequenceSpec spec)
        //{
        //    var kExamples = new Dictionary<State, IEnumerable<object>>();
        //    foreach (State input in spec.ProvidedInputs)
        //    {
        //        var inputTree = (Node)input[rule.Grammar.InputSymbol];

        //        //var nodes = ContextNodes(spec, input, inputTree);
        //        var inputTreeCopy = ConverterHelper.MakeACopy(inputTree.Value);
        //        var nodes = GetAnchorNodes(spec, input, inputTree, inputTreeCopy);

        //        var kMatches = new List<Node>();
        //        ConfigureAnchorNodes(nodes);
        //        for (int i = 0; i < spec.Examples[input].Count(); i++)
        //        {
        //            var script = (Script)spec.Examples[input].ElementAt(i);
        //            var node = nodes[i];
        //            var parentNode = !node.Parent.Value.IsKind(SyntaxKind.Block) ? ConverterHelper.MakeACopy(node.Parent) : ConverterHelper.ConvertCSharpToTreeNode(SyntaxFactory.EmptyStatement());
        //            parentNode.Children.RemoveRange(0, parentNode.Children.Count());

        //            int j = 0;
        //            if (node.LeftNode != null)
        //            {
        //                parentNode.AddChild(node.LeftNode, j++);
        //                script.Edits.First().EditOperation.K = 2;
        //            }
        //            else
        //            {
        //                script.Edits.First().EditOperation.K = 1;
        //            }

        //            if (node.Value != null) parentNode.AddChild(node.Value, j++);
        //            if (node.RightNode != null) parentNode.AddChild(node.RightNode, j);

        //            //Todo refactor this code.
        //            script.Edits.First().EditOperation.Parent = ConverterHelper.MakeACopy(parentNode);

        //            var anode = new Node(parentNode);
        //            kMatches.Add(anode);
        //            ConfigureContext(parentNode, script);
        //        }
        //        kExamples[input] = kMatches;
        //    }
        //    return new SubsequenceSpec(kExamples);
        //}

        /// <summary>
        /// Segment the edit script in nodes
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Rule parameter</param>
        /// <param name="spec">Example specification</param>
        public static SubsequenceSpec EditMapTNode(GrammarRule rule, int parameter, SubsequenceSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<Node>();
                for (int i = 0; i < spec.Examples[input].Count(); i++)
                {
                    //input tree
                    var inputTree = (Node)input[rule.Grammar.InputSymbol];
                    var copyInput = ConverterHelper.MakeACopy(inputTree.Value);
                    var examples = (List<Script>) spec.Examples[input];
                    var script = examples.ElementAt(i);
                    var editOperation = script.Edits.Single().EditOperation;
                    Node node = null;
                    if (editOperation is Update<SyntaxNodeOrToken> || editOperation is Delete<SyntaxNodeOrToken>)
                    {
                        node = new Node(editOperation.T1Node);
                    }
                    else
                    {
                        //if (ConnectedComponentMannager<SyntaxNodeOrToken>.IsValidBlock(editOperation.Parent))
                        //{
                            node = new Node(editOperation.Parent);
                        //}
                        //else
                        //{
                        //    var beforeAfter = ConfigContextBeforeAfterNode(script.Edits.First(), copyInput);
                        //    var context = beforeAfter.Item2 ?? beforeAfter.Item1;
                        //    node = new Node(context);
                        //}
                    }
                    kMatches.Add(node);
                    ConfigureContext(node.Value, script);
                }
                kExamples[input] = kMatches;
            }
            return new SubsequenceSpec(kExamples);
        }


        //private static void ConfigureAnchorNodes(List<AnchorNode> nodes)
        //{
        //    if (nodes.Any(node => node.LeftNode == null)) nodes.ForEach(node => node.LeftNode = null);
        //    if (nodes.Any(node => node.RightNode == null)) nodes.ForEach(node => node.RightNode = null);
        //}

        //private static List<AnchorNode> GetAnchorNodes(SubsequenceSpec spec, State input, Node inputTree, ITreeNode<SyntaxNodeOrToken> inputTreeCopy)
        //{
        //    var nodes = new List<AnchorNode>();

        //    foreach (Script script in spec.Examples[input])
        //    {
        //        var parent = script.Edits.First().EditOperation.Parent;
        //        var children = script.Edits.Where(o => o.EditOperation.Parent.Value.Equals(parent.Value) /*&& !(o.EditOperation is Delete<SyntaxNodeOrToken>)*/).ToList();
        //        if (children.Count > 1)
        //        {
        //            var iTreeParent = ConverterHelper.ConvertCSharpToTreeNode(SyntaxFactory.EmptyStatement());
        //            var value = ConverterHelper.ConvertCSharpToTreeNode(parent.Value);
        //            var anchorParent = new AnchorNode(value);
        //            anchorParent.Parent = iTreeParent;
        //            //anchorParent.Parent = iTreeParent;

        //            nodes.Add(anchorParent);
        //        }
        //        else
        //        {
        //            var connectedComponents = ComputeConnectedComponents(script);
        //            var tree = BuildTree(connectedComponents, ConverterHelper.MakeACopy(inputTree.Value));
        //            var node = ConfigAnchorBeforeAfterNode(script.Edits.First(), tree, inputTreeCopy);
        //            nodes.Add(node);
        //        }
        //    }
        //    return nodes;
        //}

        //private static List<Node> ContextNodes(SubsequenceSpec spec, State input, Node inputTree)
        //{
        //    var nodes = new List<Node>();

        //    foreach (Script script in spec.Examples[input])
        //    {
        //        var parent = script.Edits.First().EditOperation.Parent;
        //        var children = script.Edits.Where(o => o.EditOperation.Parent.Value.Equals(parent.Value)).ToList();

        //        /*if more than one operation occurs if more than an edit operation in the parent.
        //        For instance, an insert and an move with the parent as target.
        //        */
        //        if (children.Count > 1)
        //        {
        //            var iTreeParent = ConverterHelper.ConvertCSharpToTreeNode(SyntaxFactory.EmptyStatement());
        //            var value = ConverterHelper.ConvertCSharpToTreeNode(parent.Value);
        //            var anchorParent = new AnchorNode(value);
        //            anchorParent.Parent = iTreeParent;
        //            //anchorParent.Parent = iTreeParent;

        //            nodes.Add(new Node(value));
        //        }
        //        else
        //        {
        //            var firstOperation = script.Edits.First();
        //            //In these cases, the edit node that the technique is putting is new on the context node.
        //            if (firstOperation.EditOperation is Insert<SyntaxNodeOrToken> ||
        //                firstOperation.EditOperation is Move<SyntaxNodeOrToken>)
        //            {
        //                var tuple = ConfigContextBeforeAfterNode(firstOperation, inputTree.Value);
        //                if (tuple.Item2 != null)
        //                {
        //                    nodes.Add(new Node(tuple.Item2));
        //                }
        //                else if (tuple.Item1 != null)
        //                {
        //                    nodes.Add(new Node(tuple.Item1));
        //                }
        //                else
        //                {
        //                    var value = ConverterHelper.ConvertCSharpToTreeNode(parent.Value);
        //                    nodes.Add(new Node(value));
        //                }
        //            }
        //            else
        //            {
        //                var node = TreeUpdate.FindNode(inputTree.Value, script.Edits.First().EditOperation.T1Node.Value);
        //                nodes.Add(new Node(node));
        //            }
        //        }
        //        /*else
        //        {
        //            var connectedComponents = ComputeConnectedComponents(script);
        //            var tree = BuildTree(connectedComponents, ConverterHelper.MakeACopy(inputTree.Value));
        //            var node = ConfigAnchorBeforeAfterNode(script.Edits.First(), tree, inputTreeCopy);
        //            nodes.Add(node);
        //        }*/
        //    }
        //    return nodes;
        //}

        /// <summary>
        /// Cluster edit script in regions
        /// </summary>
        /// <param name="clusteredEdits">Clustered edit operations</param>
        private static List<List<Script>> ClusterScript(List<List<EditOperation<SyntaxNodeOrToken>>> clusteredEdits)
        {
            var clusteredList = new List<List<Script>>();
            if (clusteredEdits.Any())
            {
                var clusters = ClusterConnectedComponents(clusteredEdits);
                foreach (var cluster in clusters)
                {
                    var listEdit = cluster.Select(clusterList => new Script(clusterList.Select(o => new Edit<SyntaxNodeOrToken>(o)).ToList())).ToList();
                    clusteredList.Add(listEdit);
                }
            }
            return clusteredList;
        }


        /// <summary>
        /// Compact edit operations with similar semantic in compacted edit operations
        /// </summary>
        /// <param name="connectedComponents">Uncompacted edit operations</param>
        private static List<Script> CompactScript(List<Script> connectedComponents, ITreeNode<SyntaxNodeOrToken> inpTree)
        {
            var newccs = new List<Script>();
            foreach (var script in connectedComponents)
            {
                var newScript = new Script(new List<Edit<SyntaxNodeOrToken>>());
                //var parent = ;
                var parent = ConverterHelper.ConvertCSharpToTreeNode(GetParent(script, inpTree).Value); //ConverterHelper.ConvertCSharpToTreeNode(script.Edits.First().EditOperation.Parent.Value);
                var children = script.Edits.Where(o => o.EditOperation.Parent.Value.Equals(parent.Value)).ToList();

                var treeUpdate = new TreeUpdate(parent);
                foreach (var s in script.Edits)
                {
                    treeUpdate.ProcessEditOperation(s.EditOperation);
                    string str = PrintUtil<SyntaxNodeOrToken>.PrettyPrintString(treeUpdate.CurrentTree, @"C:\Users\SPG-04\Desktop\out.txt");
                }

                if (script.Edits.All(o => o.EditOperation is Delete<SyntaxNodeOrToken>))
                {
                    //var t1node = treeUpdate.CurrentTree;
                    //var t1parent = ConverterHelper.ConvertCSharpToTreeNode(parent.Value.Parent);
                    var edits = script.Edits.Select(o => o.EditOperation).ToList();
                    var list = Compact(new List<List<EditOperation<SyntaxNodeOrToken>>> { edits });
                    var t1node = list.First();
                    var delete = new Delete<SyntaxNodeOrToken>(t1node.T1Node);
                    delete.Parent = t1node.Parent;
                    newScript.Edits.Add(new Edit<SyntaxNodeOrToken>(delete));
                    newccs.Add(newScript);
                    //return newccs;
                    continue;
                }

                var firstOperation = script.Edits.Find(o => !(o.EditOperation is Delete<SyntaxNodeOrToken>)).EditOperation;

                if (children.Count > 1)
                {
                    //todo correct the children.First children.second
                    if (children.Count == 2 && children.First().EditOperation is Insert<SyntaxNodeOrToken> && children.ElementAt(1).EditOperation is Delete<SyntaxNodeOrToken>)
                    {
                        var @from = ConverterHelper.ConvertCSharpToTreeNode(children.ElementAt(1).EditOperation.T1Node.Value);
                        var to = children.First().EditOperation.T1Node;
                        foreach (var v in @from.DescendantNodesAndSelf())
                        {
                            foreach (var edit in script.Edits)
                            {
                                if (edit.EditOperation is Update<SyntaxNodeOrToken>)
                                {
                                    var up = (Update<SyntaxNodeOrToken>)edit.EditOperation;
                                    if (v.Equals(up.To))
                                    {
                                        v.Value = up.T1Node.Value;
                                    }
                                }
                            }
                        }
                        var update = new Update<SyntaxNodeOrToken>(@from, to, parent);
                        newScript.Edits.Add(new Edit<SyntaxNodeOrToken>(update));
                        newccs.Add(newScript);
                        //return newccs;
                        continue;
                    }
                    else
                    {
                        //todo we need to work more here.
                        var toNode = treeUpdate.CurrentTree;
                        var @from = ConverterHelper.ConvertCSharpToTreeNode(parent.Value);
                        var update = new Update<SyntaxNodeOrToken>(@from, toNode, ConverterHelper.ConvertCSharpToTreeNode(parent.Value.Parent));
                        newScript.Edits.Add(new Edit<SyntaxNodeOrToken>(update));
                        newccs.Add(newScript);
                        //return newccs;
                        continue;
                    }
                }
                else if (firstOperation is Insert<SyntaxNodeOrToken>)
                {
                    var inserted = TreeUpdate.FindNode(treeUpdate.CurrentTree, firstOperation.T1Node.Value);
                    var parentcopy = ConverterHelper.ConvertCSharpToTreeNode(treeUpdate.CurrentTree.Value);
                    var insert = new Insert<SyntaxNodeOrToken>(inserted, parentcopy, firstOperation.K);
                    newScript.Edits.Add(new Edit<SyntaxNodeOrToken>(insert));
                    newccs.Add(newScript);
                    //return newccs;
                    continue;
                }
                else if (firstOperation is Update<SyntaxNodeOrToken>)
                {
                    newScript.Edits.Add(new Edit<SyntaxNodeOrToken>(firstOperation));
                    newccs.Add(newScript);
                    //return newccs;
                    continue;
                }
            }
            return newccs;
        }

        private static ITreeNode<SyntaxNodeOrToken> GetParent(Script script, ITreeNode<SyntaxNodeOrToken> inpTree)
        {
            ITreeNode<SyntaxNodeOrToken> root = null;
            foreach (var v in script.Edits)
            {
                ITreeNode<SyntaxNodeOrToken> tocompare = null;
                if (v.EditOperation is Update<SyntaxNodeOrToken> || v.EditOperation is Delete<SyntaxNodeOrToken>)
                {
                    tocompare = v.EditOperation.T1Node;
                }
                else
                {
                    tocompare = v.EditOperation.Parent;
                }

                if (root == null)
                {
                    root = tocompare;
                }
                else if (TreeUpdate.FindNode(inpTree, tocompare.Value) != null)
                {
                    if (root.Value.SpanStart > tocompare.Value.SpanStart)
                    {
                        root = tocompare;
                    }else if (root.Value.SpanStart == tocompare.Value.SpanStart)
                    {
                        if (root.Value.Span.End < tocompare.Value.Span.End)
                        {
                            root = tocompare;
                        }
                    }
                    
                }
            }
            return root;
        }

        ///// <summary>
        ///// Compact edit operations with similar semantic in compacted edit operations
        ///// </summary>
        ///// <param name="connectedComponents">Uncompacted edit operations</param>
        //private static List<Script> CompactScript(List<Script> connectedComponents)
        //{
        //    var newccs = new List<Script>();
        //    foreach (var cc in connectedComponents)
        //    {
        //        var editionConnected = ConnectedComponentMannager<SyntaxNodeOrToken>.EditConnectedComponents(cc.Edits.Select(o => o.EditOperation).ToList());
        //        var newScript = Compact(editionConnected);
        //        newccs.Add(new Script(newScript.Select(o => new Edit<SyntaxNodeOrToken>(o)).ToList()));
        //    }
        //    return newccs;
        //}

        /// <summary>
        /// Compact similar edit operations in a single edit operation
        /// </summary>
        /// <param name="connectedComponents">Uncompacted edit operations</param>
        private static List<EditOperation<SyntaxNodeOrToken>> Compact(List<List<EditOperation<SyntaxNodeOrToken>>> connectedComponents)
        {
            var newList = new List<EditOperation<SyntaxNodeOrToken>>();
            var removes = new List<EditOperation<SyntaxNodeOrToken>>();
            foreach (var editOperations in connectedComponents)
            {
                foreach (var editI in editOperations)
                {
                    foreach (var editJ in editOperations)
                    {
                        if (editI.Equals(editJ)) continue;
                        if (editI.T1Node.Equals(editJ.Parent))
                        {
                            if (!(editJ is Update<SyntaxNodeOrToken>))
                            {
                                editI.T1Node.AddChild(editJ.T1Node, editI.T1Node.Children.Count);
                            }
                            else
                            {
                                TreeUpdate update = new TreeUpdate(editI.T1Node);
                                update.ProcessEditOperation(editJ);
                            }
                            removes.Add(editJ);
                        }
                    }
                }
                newList.AddRange(editOperations);
            }
            newList.RemoveAll(editOperation => removes.Any(node => node == editOperation));
            return newList;
        }

        ///// <summary>
        ///// Compact similar edit operations in a single edit operation
        ///// </summary>
        ///// <param name="connectedComponents">Uncompacted edit operations</param>
        //private static List<EditOperation<SyntaxNodeOrToken>> Compact(List<List<EditOperation<SyntaxNodeOrToken>>> connectedComponents)
        //{
        //    var newList = new List<EditOperation<SyntaxNodeOrToken>>();
        //    var removes = new List<EditOperation<SyntaxNodeOrToken>>();
        //    foreach (var editOperations in connectedComponents)
        //    {
        //        foreach (var editI in editOperations)
        //        {
        //            foreach (var editJ in editOperations)
        //            {
        //                if (editI.Equals(editJ)) continue;
        //                if (editI.T1Node.Equals(editJ.Parent))
        //                {
        //                    if (!(editJ is Update<SyntaxNodeOrToken>))
        //                    {
        //                        editI.T1Node.AddChild(editJ.T1Node, editI.T1Node.Children.Count);
        //                    }
        //                    else
        //                    {
        //                        TreeUpdate update = new TreeUpdate(editI.T1Node);
        //                        update.ProcessEditOperation(editJ);
        //                    }
        //                    removes.Add(editJ);
        //                }            
        //            }
        //        }
        //        newList.AddRange(editOperations);
        //    }
        //    newList.RemoveAll(editOperation => removes.Any(node => node == editOperation));
        //    //var updateRemoves = new List<EditOperation<SyntaxNodeOrToken>>();
        //    //var newNodes = new List<Tuple<EditOperation<SyntaxNodeOrToken>, EditOperation<SyntaxNodeOrToken>>>();
        //    //foreach (var si in newList)
        //    //{
        //    //    foreach (var sj in newList)
        //    //    {
        //    //        if (si.Parent.Equals(sj.Parent))
        //    //        {
        //    //            updateRemoves.Add(si);
        //    //            updateRemoves.Add(sj);
        //    //            var newParent = ConverterHelper.ConvertCSharpToTreeNode(si.Parent.Value);
        //    //            var treeUpdate = new TreeUpdate(newParent);
        //    //            treeUpdate.ProcessEditOperation(si);
        //    //            treeUpdate.ProcessEditOperation(sj);
        //    //            var updateEdit = new Update<SyntaxNodeOrToken>(si.Parent, treeUpdate.CurrentTree, si.Parent.Parent);
        //    //        }
        //    //    }
        //    //}
        //    return newList;
        //}

        /// <summary>
        /// Cluster connected components
        /// </summary>
        /// <param name="connectedComponents">Connected components</param>
        private static List<List<List<EditOperation<SyntaxNodeOrToken>>>> ClusterConnectedComponents(List<List<EditOperation<SyntaxNodeOrToken>>> connectedComponents)
        {
            var clusters = Clusters(connectedComponents);
            var list = new List<List<List<EditOperation<SyntaxNodeOrToken>>>>();
            foreach (var cluster in clusters)
            {
                list.Add(cluster.Select(item => item.Operations).ToList());
            }
            return list;
        }

        /// <summary>
        /// Cluster connected components
        /// </summary>
        /// <param name="connectedComponents">Connected components</param>
        private static HashSet<EditOperationDatasetItem[]> Clusters(List<List<EditOperation<SyntaxNodeOrToken>>> connectedComponents)
        {
            HashSet<EditOperationDatasetItem[]> clusters;
            var lcc = new LongestCommonSubsequenceManager<EditOperation<SyntaxNodeOrToken>>();
            var featureData = connectedComponents.Select(x => new EditOperationDatasetItem(x)).ToArray();
            var dbs = new DbscanAlgorithm<EditOperationDatasetItem>((x, y) => 1.0 - (2 * (double)lcc.FindCommon(x.Operations, y.Operations).Count) / ((double)x.Operations.Count + (double)y.Operations.Count));
            dbs.ComputeClusterDbscan(allPoints: featureData, epsilon: 0.4, minPts: 1, clusters: out clusters);
            return clusters;
        }

        private static void ConfigureContext(ITreeNode<SyntaxNodeOrToken> anchor, Script script)
        {
            var treeUp = new TreeUpdate(anchor);
            ConfigureParentSyntaxTree(script, anchor);
            if (!WitnessFunctions.TreeUpdateDictionary.ContainsKey(anchor))
            {
                WitnessFunctions.TreeUpdateDictionary.Add(anchor, treeUp);
            }
            WitnessFunctions.CurrentTrees[anchor] = anchor;
        }

        public static Tuple<ITreeNode<SyntaxNodeOrToken>, ITreeNode<SyntaxNodeOrToken>> ConfigContextBeforeAfterNode(Edit<SyntaxNodeOrToken> edit, ITreeNode<SyntaxNodeOrToken> inputTree)
        {
            //Get a reference for the node that was modified on the T1 tree.
            ITreeNode<SyntaxNodeOrToken> inputNode = null;
            //if (TreeUpdate.FindNode(inputTree, edit.EditOperation.T1Node.Value) != null)
            if (edit.EditOperation is Delete<SyntaxNodeOrToken> || edit.EditOperation is Update<SyntaxNodeOrToken>)
            {
                inputNode = edit.EditOperation.T1Node;
            }

            var previousTree = new TreeUpdate(inputTree);
            var copy = ConverterHelper.MakeACopy(inputTree);

            Tuple<ITreeNode<SyntaxNodeOrToken>, ITreeNode<SyntaxNodeOrToken>> beforeAfterAnchorNode;
            ITreeNode<SyntaxNodeOrToken> treeNode = null;
            if (inputNode == null)
            {
                previousTree.ProcessEditOperation(edit.EditOperation);
                var from = TreeUpdate.FindNode(previousTree.CurrentTree, edit.EditOperation.T1Node.Value);
                beforeAfterAnchorNode = GetBeforeAfterAnchorNode(from);
            }
            else
            {
                var from = TreeUpdate.FindNode(previousTree.CurrentTree, edit.EditOperation.T1Node.Value);
                beforeAfterAnchorNode = GetBeforeAfterAnchorNode(from);
                treeNode = TreeUpdate.FindNode(inputTree, inputNode.Value);
                previousTree.ProcessEditOperation(edit.EditOperation);
            }

            return beforeAfterAnchorNode;
            //Get the nodes before and after the input node.
            //Location of the left, right, and after node.
            /*var leftNode = beforeAfterAnchorNode.Item1 != null ? TreeUpdate.FindNode(copy, beforeAfterAnchorNode.Item1.Value) : null;
            var rightNode = beforeAfterAnchorNode.Item2 != null ? TreeUpdate.FindNode(copy, beforeAfterAnchorNode.Item2.Value) : null;

            var node = new AnchorNode(treeNode, leftNode, rightNode);
            return node;*/
        }

        //private static AnchorNode ConfigAnchorBeforeAfterNode(Edit<SyntaxNodeOrToken> edit, ITreeNode<SyntaxNodeOrToken> anchorNode, ITreeNode<SyntaxNodeOrToken> inputTree)
        //{
        //    //Get a reference for the node that was modified on the T1 tree.
        //    ITreeNode<SyntaxNodeOrToken> inputNode = null;
        //    //if (TreeUpdate.FindNode(inputTree, edit.EditOperation.T1Node.Value) != null)
        //    if(edit.EditOperation is Delete<SyntaxNodeOrToken> || edit.EditOperation is Update<SyntaxNodeOrToken>)
        //    {
        //        inputNode = anchorNode;
        //    }

        //    var previousTree = new TreeUpdate(inputTree);
        //    var copy = ConverterHelper.MakeACopy(inputTree);

        //    Tuple<ITreeNode<SyntaxNodeOrToken>, ITreeNode<SyntaxNodeOrToken>> beforeAfterAnchorNode;
        //    ITreeNode<SyntaxNodeOrToken> treeNode = null;
        //    if (inputNode == null)
        //    {
        //        previousTree.ProcessEditOperation(edit.EditOperation);
        //        var from = TreeUpdate.FindNode(previousTree.CurrentTree, edit.EditOperation.T1Node.Value);
        //        beforeAfterAnchorNode = GetBeforeAfterAnchorNode(from);
        //    }
        //    else
        //    {
        //        var from = TreeUpdate.FindNode(previousTree.CurrentTree, edit.EditOperation.T1Node.Value);
        //        beforeAfterAnchorNode = GetBeforeAfterAnchorNode(from);
        //        treeNode = TreeUpdate.FindNode(inputTree, inputNode.Value);
        //        if (edit.EditOperation is Delete<SyntaxNodeOrToken>)
        //        {
        //            previousTree.ProcessEditOperation(edit.EditOperation);
        //        }
        //    }
        //    //Get the nodes before and after the input node.
        //    //Location of the left, right, and after node.
        //    var leftNode = beforeAfterAnchorNode.Item1 != null ? TreeUpdate.FindNode(copy, beforeAfterAnchorNode.Item1.Value) : null;
        //    var rightNode = beforeAfterAnchorNode.Item2 != null ? TreeUpdate.FindNode(copy, beforeAfterAnchorNode.Item2.Value) : null;

        //    var node = new AnchorNode(treeNode, leftNode, rightNode);
        //    return node;
        //}

        private static Tuple<ITreeNode<SyntaxNodeOrToken>, ITreeNode<SyntaxNodeOrToken>> GetBeforeAfterAnchorNode(ITreeNode<SyntaxNodeOrToken> node)
        {
            for (int i = 0; i < node.Parent.Children.Count; i++)
            {
                var child = node.Parent.Children[i];
                if (child.Equals(node))
                {
                    var left = i > 0 ? node.Parent.Children[i - 1] : null;
                    var right = node.Parent.Children.Count > i + 1 ? node.Parent.Children[i + 1] : null;

                    if (left != null || right != null)
                    {
                        var tuple = Tuple.Create(left, right);
                        return tuple;
                    }
                }
            }
            return Tuple.Create<ITreeNode<SyntaxNodeOrToken>, ITreeNode<SyntaxNodeOrToken>>(null, null);
        }

        private static void ConfigureParentSyntaxTree(Script script, ITreeNode<SyntaxNodeOrToken> syntaxTree)
        {
            foreach (var edit in script.Edits)
            {
                edit.EditOperation.T1Node.SyntaxTree = syntaxTree;
                if (edit.EditOperation.Parent != null)
                {
                    edit.EditOperation.Parent.SyntaxTree = syntaxTree;
                }

                if (edit.EditOperation is Update<SyntaxNodeOrToken>)
                {
                    var update = (Update<SyntaxNodeOrToken>)edit.EditOperation;
                    update.To.SyntaxTree = syntaxTree;
                }
            }
        }

        ///// <summary>
        ///// Compute connected components from the script
        ///// </summary>
        ///// <param name="script">Script</param>
        //private static List<Script> ComputeConnectedComponents(Script script)
        //{
        //    var editOperations = script.Edits.Select(o => o.EditOperation).ToList();
        //    var connectedComponents = ConnectedComponentMannager<SyntaxNodeOrToken>.DescendantsConnectedComponents(editOperations);
        //    connectedComponents = connectedComponents.OrderBy(o => o.First().T1Node.Value.SpanStart).ToList();
        //    var scripts = connectedComponents.Select(component => new Script(component.Select(o => new Edit<SyntaxNodeOrToken>(o)).ToList())).ToList();
        //    return scripts;
        //}

        /// <summary>
        /// Compute the edition script
        /// </summary>
        /// <param name="inpTree">Input tree</param>
        /// <param name="outTree">Output tree</param>
        /// <returns>Computed edit script</returns>
        private static List<EditOperation<SyntaxNodeOrToken>> Script(SyntaxNodeOrToken inpTree, SyntaxNodeOrToken outTree)
        {
            var gumTreeMapping = new GumTreeMapping<SyntaxNodeOrToken>();
            var inpNode = ConverterHelper.ConvertCSharpToTreeNode(inpTree);
            var outNode = ConverterHelper.ConvertCSharpToTreeNode(outTree);
            Mapping = gumTreeMapping.Mapping(inpNode, outNode);

            var generator = new EditScriptGenerator<SyntaxNodeOrToken>();
            var script = generator.EditScript(inpNode, outNode, Mapping);
            return script;
        }

        //private static ITreeNode<SyntaxNodeOrToken> BuildTree(List<Script> ccs, ITreeNode<SyntaxNodeOrToken> inpTree)
        //{
        //    return ccs.Select(cc => BuildTemplate(cc, inpTree)).Select(template => template.First()).OrderBy(o => o.Value.SpanStart).Single();
        //}

        //private static List<ITreeNode<SyntaxNodeOrToken>> BuildTemplate(Script script, ITreeNode<SyntaxNodeOrToken> tree)
        //{
        //    var nodes = BFSWalker<SyntaxNodeOrToken>.BreadFirstSearch(tree);
        //    var list = new List<ITreeNode<SyntaxNodeOrToken>>();
        //    var editNodes = EditNodes(script);
        //    foreach (var node in nodes)
        //    {
        //        foreach (var t1Node in editNodes)
        //        {
        //            if (node.Equals(t1Node))
        //            {
        //                if (!list.Contains(node))
        //                {
        //                    list.Add(node);
        //                }
        //            }
        //        }
        //    }
        //    var tcc = new TemplateConnectedComponents<SyntaxNodeOrToken>();
        //    var cnodes = tcc.ConnectedNodes(list);
        //    return cnodes;
        //}

        //private static List<ITreeNode<SyntaxNodeOrToken>> EditNodes(Script script)
        //{
        //    var nodes = new List<ITreeNode<SyntaxNodeOrToken>>();
        //    foreach (var edit in script.Edits)
        //    {
        //        nodes.AddRange(edit.EditOperation.T1Node.DescendantNodesAndSelf());

        //        if (!(edit.EditOperation is Delete<SyntaxNodeOrToken>))
        //        {
        //            nodes.AddRange(edit.EditOperation.Parent.DescendantNodesAndSelf());
        //        }
        //    }
        //    return nodes;
        //}
    }
}
