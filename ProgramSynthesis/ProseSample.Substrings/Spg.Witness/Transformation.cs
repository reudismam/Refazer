using System;
using System.Collections.Generic;
using System.Linq;
using DbscanImplementation;
using LongestCommonSubsequence;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using ProseSample.Substrings.Spg.Bean;
using TreeEdit.Spg.Clustering;
using TreeEdit.Spg.ConnectedComponents;
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

        public static Dictionary<List<EditOperation<SyntaxNodeOrToken>>, ITreeNode<SyntaxNodeOrToken>> CurrentTrees = new Dictionary<List<EditOperation<SyntaxNodeOrToken>>, ITreeNode<SyntaxNodeOrToken>>();

        public static Dictionary<List<EditOperation<SyntaxNodeOrToken>>, EditOperation<SyntaxNodeOrToken>> Operations =
            new Dictionary<List<EditOperation<SyntaxNodeOrToken>>, EditOperation<SyntaxNodeOrToken>>();

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
                edits = script.Edits.GetRange(0, 1);
                editsExamples[input] = edits;
            }
            return new ExampleSpec(editsExamples);
        }

        /// <summary>
        /// Transformation witness function for parameter rule.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Grammar parameter</param>
        /// <param name="spec">Example specification</param>
        public static SubsequenceSpec TransformationRule(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            var dicCluster = new Dictionary<State, List<List<EditOperation<SyntaxNodeOrToken>>>>();
            var ccsList = new List<List<EditOperation<SyntaxNodeOrToken>>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var inpTreeNode = (Node)input[rule.Body[0]];
                var inpTree = inpTreeNode.Value.Value;
                foreach (SyntaxNodeOrToken outTree in spec.DisjunctiveExamples[input])
                {
                    var script = Script(inpTree, outTree);
                    PrintScript(script);
                    var primaryEditions = ConnectedComponentMannager<SyntaxNodeOrToken>.ComputePrimaryEditions(script);
                    var ccs = ConnectedComponentMannager<SyntaxNodeOrToken>.ConnectedComponents(primaryEditions, script);
                    //todo this need to be refactored.
                    ccs.ForEach(o => CurrentTrees.Add(o, inpTreeNode.Value));
                    dicCluster[input] = ccs;
                    ccsList.AddRange(ccs);
                }
            }

            var clusters = ClusterScript(ccsList);
            foreach (State input in spec.ProvidedInputs)
            {
                var inpTreeNode = (Node)input[rule.Body[0]];
                var kMatches = new List<List<Script>>();
                foreach (var cluster in clusters)
                {
                    var listItem = new List<Script>();
                    foreach (var item in cluster)
                    {
                        if (dicCluster[input].Any(e => IsEquals(item.Edits.Select(o => o.EditOperation).ToList(), e)))
                        {
                            listItem.Add(item);
                        }
                    }
                    kMatches.Add(listItem);
                }
                kMatches = kMatches.Select(o => CompactScript(o, inpTreeNode.Value)).ToList();
                kExamples[input] = new List<List<List<Script>>> { kMatches };
            }
            var subsequence = new SubsequenceSpec(kExamples);
            return subsequence;
        }

        private static bool IsEquals(List<EditOperation<SyntaxNodeOrToken>> item1, List<EditOperation<SyntaxNodeOrToken>> item2)
        {
            if (item1.Count() != item2.Count()) return false;

            for (int i = 0; i < item1.Count(); i++)
            {
                var editI = item1[i];
                var editj = item2[i];

                if (!(editI.GetType() == editj.GetType())) return false;
                if (!editI.T1Node.Value.Equals(editj.T1Node.Value)) return false;
                if (!editI.Parent.Value.Equals(editj.Parent.Value)) return false;
                if (editI.K != editj.K) return false;

                if (editI is Update<SyntaxNodeOrToken>)
                {
                    var upi = (Update<SyntaxNodeOrToken>)editI;
                    var upj = (Update<SyntaxNodeOrToken>)editj;
                    if (!upi.To.Value.Equals(upj.To.Value)) return false;
                }
            }
            return true;
        }

        public static void PrintScript(List<Edit<SyntaxNodeOrToken>> script)
        {
            string s = script.Aggregate("", (current, v) => current + (v + "\n"));
        }

        public static void PrintScript(List<EditOperation<SyntaxNodeOrToken>> script)
        {
            string s = script.Aggregate("", (current, v) => current + (v + "\n"));
        }

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
                    var examples = (List<Script>)spec.Examples[input];
                    var script = examples.ElementAt(i);
                    var editOperation = script.Edits.Single().EditOperation;
                    Node node = null;
                    if (editOperation is Update<SyntaxNodeOrToken> || editOperation is Delete<SyntaxNodeOrToken>)
                    {
                        node = new Node(editOperation.T1Node);
                    }
                    else
                    {
                        node = new Node(editOperation.Parent);
                    }
                    kMatches.Add(node);
                    ConfigureContext(node.Value, script);
                }
                kExamples[input] = kMatches;
            }
            return new SubsequenceSpec(kExamples);
        }

        /// <summary>
        /// Cluster edit script in regions
        /// </summary>
        /// <param name="clusteredEdits">Clustered edit operations</param>
        public static List<List<Script>> ClusterScript(List<List<EditOperation<SyntaxNodeOrToken>>> clusteredEdits)
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
                var parent = ConverterHelper.ConvertCSharpToTreeNode(GetParent(script, inpTree).Value);
                var children = script.Edits.Where(o => o.EditOperation.Parent.Value.Equals(parent.Value)).ToList();

                var treeUpdate = new TreeUpdate(parent);
                foreach (var s in script.Edits)
                {
                    treeUpdate.ProcessEditOperation(s.EditOperation);
                }

                if (script.Edits.All(o => o.EditOperation is Delete<SyntaxNodeOrToken>))
                {
                    var edits = script.Edits.Select(o => o.EditOperation).ToList();
                    var list = Compact(new List<List<EditOperation<SyntaxNodeOrToken>>> { edits });

                    if (list.Count == 1)
                    {
                        var t1node = list.Single();
                        var delete = new Delete<SyntaxNodeOrToken>(t1node.T1Node);
                        delete.Parent = t1node.Parent;
                        newScript.Edits.Add(new Edit<SyntaxNodeOrToken>(delete));
                        newccs.Add(newScript);
                        continue;
                    }

                    treeUpdate = new TreeUpdate(ConverterHelper.ConvertCSharpToTreeNode(list.First().Parent.Value));
                    foreach (var s in script.Edits)
                    {
                        treeUpdate.ProcessEditOperation(s.EditOperation);
                    }

                    var @from = ConverterHelper.ConvertCSharpToTreeNode(list.First().Parent.Value);
                    var to = ConverterHelper.MakeACopy(treeUpdate.CurrentTree);
                    var update = new Update<SyntaxNodeOrToken>(@from, to, parent);
                    newScript.Edits.Add(new Edit<SyntaxNodeOrToken>(update));
                    newccs.Add(newScript);
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
                    }
                    else if (root.Value.SpanStart == tocompare.Value.SpanStart)
                    {
                        if (root.Value.Span.End < tocompare.Value.Span.End)
                        {
                            root = tocompare;
                        }
                        else if (root.Value.Span.End == tocompare.Value.Span.End)
                        {
                            if (root.IsLabel(new TLabel(SyntaxKind.IdentifierName)))
                            {
                                root = tocompare;
                            }
                        }
                    }

                }
            }
            return root;
        }

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
            var dbs = new DbscanAlgorithm<EditOperationDatasetItem>((x, y) => Distance(lcc, x, y));
            dbs.ComputeClusterDbscan(allPoints: featureData, epsilon: 0.38, minPts: 1, clusters: out clusters);
            return clusters;
        }

        private static double Distance(LongestCommonSubsequenceManager<EditOperation<SyntaxNodeOrToken>> lcc, EditOperationDatasetItem x, EditOperationDatasetItem y)
        {
            var common = (double)lcc.FindCommon(x.Operations, y.Operations).Count;
            //var tuple = Tuple.Create(common / (double)x.Operations.Count, common / (double)y.Operations.Count);
            var dist = 1.0 - (2 * common) / ((double)x.Operations.Count + (double)y.Operations.Count);
            //var squares = (tuple.Item1 * tuple.Item1 + tuple.Item2 * tuple.Item2) / 2;
            //var dist = 1.0 - Math.Sqrt(squares);
            //if (!Operations.ContainsKey(x.Operations))
            //{
            //    var xtree = CurrentTrees[x.Operations];
            //    var editsx = new Script(x.Operations.Select(o => new Edit<SyntaxNodeOrToken>(o)).ToList());
            //    var compEditx = CompactScript(new List<Script> {editsx}, ConverterHelper.MakeACopy(xtree));
            //    Operations[x.Operations] = compEditx.First().Edits.Single().EditOperation;
            //}

            //if (!Operations.ContainsKey(y.Operations))
            //{
            //    var ytree = CurrentTrees[y.Operations];
            //    var editsy = new Script(y.Operations.Select(o => new Edit<SyntaxNodeOrToken>(o)).ToList());
            //    var compEdity = CompactScript(new List<Script> {editsy}, ConverterHelper.MakeACopy(ytree));
            //    Operations[y.Operations] = compEdity.First().Edits.Single().EditOperation;
            //}


            //var xeditOperation = Operations[x.Operations];
            //var yeditOperation = Operations[y.Operations];


            ////todo refactor this method 
            //if (xeditOperation.GetType() != yeditOperation.GetType()) return 1;

            return dist;
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
        }

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
    }
}
