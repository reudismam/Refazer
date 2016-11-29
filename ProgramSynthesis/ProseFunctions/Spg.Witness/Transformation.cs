using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using TreeEdit.Spg.Script;
using TreeEdit.Spg.TreeEdit.Mapping;
using TreeEdit.Spg.TreeEdit.Update;
using ProseSample.Substrings;
using TreeEdit.Spg.Print;
using TreeElement.Spg.Node;
using LCA.Spg.Manager;

namespace ProseSample.Substrings.Spg.Witness
{
    public class Transformation
    {
        /// <summary>
        /// Transformation witness function for parameter rule.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Grammar parameter</param>
        /// <param name="spec">Example specification</param>
        public static SubsequenceSpec TransformationRule(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            var dicCcs = new Dictionary<State, List<List<EditOperation<SyntaxNodeOrToken>>>>();
            var ccsList = new List<List<EditOperation<SyntaxNodeOrToken>>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var inpTreeNode = (Node)input[rule.Body[0]];
                var inpTree = inpTreeNode.Value.Value;
                foreach (SyntaxNodeOrToken outTree in spec.DisjunctiveExamples[input])
                {
                    var script = Script(inpTree, outTree);
                    var primaryEditions = ConnectedComponentMannager<SyntaxNodeOrToken>.ComputePrimaryEditions(script);
                    var ccs = ConnectedComponentMannager<SyntaxNodeOrToken>.ConnectedComponents(primaryEditions, script);
                    dicCcs[input] = ccs;
                    ccsList.AddRange(ccs);
                }
            }

            var clusters = ClusterScript(ccsList);
            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<List<Script>>();
                foreach (var cluster in clusters)
                {
                    var ccsInput = dicCcs[input];
                    var listItem = cluster.Where(item => ccsInput.Any(e => IsEquals(item.Edits, e))).ToList();
                    kMatches.Add(listItem);
                }
                var inpTreeNode = (Node)input[rule.Body[0]];
                var edits = kMatches.Select(o => CompactScript(o, inpTreeNode.Value)).ToList();
                kExamples[input] = new List<List<List<Edit<SyntaxNodeOrToken>>>> { edits };
            }
            var subsequence = new SubsequenceSpec(kExamples);
            return subsequence;
        }

        /// <summary>
        /// Segment the edit edit in nodes
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Rule parameter</param>
        /// <param name="spec">Example specification</param>
        public static SubsequenceSpec EditMapTNode(GrammarRule rule, int parameter, SubsequenceSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<TreeNode<SyntaxNodeOrToken>>();
                for (int i = 0; i < spec.Examples[input].Count(); i++)
                {
                    var examples = (List<Edit<SyntaxNodeOrToken>>)spec.Examples[input];
                    var edit = examples.ElementAt(i);
                    var editOperation = edit.EditOperation;
                    TreeNode<SyntaxNodeOrToken> node;
                    if (editOperation is Update<SyntaxNodeOrToken> || editOperation is Delete<SyntaxNodeOrToken>)
                    {
                        node = editOperation.T1Node;
                    }
                    else
                    {
                        node = editOperation.Parent;
                    }
                    kMatches.Add(node);
                    ConfigureContext(node, edit);
                }
                kExamples[input] = kMatches;
            }
            return new SubsequenceSpec(kExamples);
        }

        private static bool IsEquals(List<Edit<SyntaxNodeOrToken>> item1, List<EditOperation<SyntaxNodeOrToken>> item2)
        {
            if (item1.Count != item2.Count) return false;
            for (int i = 0; i < item1.Count(); i++)
            {
                var editI = item1[i].EditOperation;
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

        /// <summary>
        /// Cluster edit edit in regions
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
        /// <param name="inpTree">Input tree</param>
        private static List<Edit<SyntaxNodeOrToken>> CompactScript(List<Script> connectedComponents, TreeNode<SyntaxNodeOrToken> inpTree)
        {
            var newEditOperations = new List<Edit<SyntaxNodeOrToken>>();
            foreach (var script in connectedComponents)
            {
                var edit = CompactScriptIntoASingleOperation(inpTree, script);
                if (edit != null)
                {
                    newEditOperations.Add(edit);
                }
            }
            return newEditOperations;
        }

        private static Edit<SyntaxNodeOrToken> CompactScriptIntoASingleOperation(TreeNode<SyntaxNodeOrToken> inpTree, Script script)
        {
            //if the script has only an edit, the compacted edit is itself.
            if (script.Edits.Count == 1)
            {
                return script.Edits.First();
            }
            //parent of the edit operations
            var parent = GetParent(script, inpTree);
            //transformed version of the parent node.
            var transformed = ProcessScriptOnNode(script, parent);

            //TODO Refactor the code to does not convert a list in another list and back.
            var edits = script.Edits.Select(o => o.EditOperation).ToList();
            var primaryEditions = ConnectedComponentMannager<SyntaxNodeOrToken>.ComputePrimaryEditions(edits);
            var children = primaryEditions.Select(o => new Edit<SyntaxNodeOrToken>(o)).ToList();
           
            if (script.Edits.All(o => o.EditOperation is Delete<SyntaxNodeOrToken>)) return ProcessSequenceOfDeleteOperations(script, parent);
            if (children.Count > 1) return ProcessRootNodeHasMoreThanOneChild(script, children, parent, transformed);

            var firstOperation = script.Edits.Find(o => !(o.EditOperation is Delete<SyntaxNodeOrToken>)).EditOperation;
            if (firstOperation is Insert<SyntaxNodeOrToken>)
            {
                var inserted = TreeUpdate.FindNode(transformed, firstOperation.T1Node.Value);
                var parentcopy = ConverterHelper.ConvertCSharpToTreeNode(transformed.Value);
                var insert = new Insert<SyntaxNodeOrToken>(inserted, parentcopy, firstOperation.K);
                return new Edit<SyntaxNodeOrToken>(insert);
            }
            return null;
        }

        private static Edit<SyntaxNodeOrToken> ProcessRootNodeHasMoreThanOneChild(Script script, List<Edit<SyntaxNodeOrToken>> children, TreeNode<SyntaxNodeOrToken> parent, TreeNode<SyntaxNodeOrToken> transformed)
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
                {
                    var operation = new Edit<SyntaxNodeOrToken>(update);
                    return operation;
                }
            }
            else
            {
                //todo we need to work more here.
                var toNode = transformed;
                var @from = ConverterHelper.ConvertCSharpToTreeNode(parent.Value);
                var update = new Update<SyntaxNodeOrToken>(@from, toNode,
                    ConverterHelper.ConvertCSharpToTreeNode(parent.Value.Parent));
                {
                    var operation = new Edit<SyntaxNodeOrToken>(update);
                    return operation;
                }
            }
        }

        private static TreeNode<SyntaxNodeOrToken> ProcessScriptOnNode(Script script, TreeNode<SyntaxNodeOrToken> parent)
        {
            var treeUpdate = new TreeUpdate(parent);
            foreach (var s in script.Edits)
            {
                treeUpdate.ProcessEditOperation(s.EditOperation);
            }
            return treeUpdate.CurrentTree;
        }

        private static Edit<SyntaxNodeOrToken> ProcessSequenceOfDeleteOperations(Script script, TreeNode<SyntaxNodeOrToken> parent)
        {
            var edits = script.Edits.Select(o => o.EditOperation).ToList();
            var list = Compact(new List<List<EditOperation<SyntaxNodeOrToken>>> { edits });

            if (list.Count == 1)
            {
                var t1Node = list.Single();
                var delete = new Delete<SyntaxNodeOrToken>(t1Node.T1Node);
                delete.Parent = t1Node.Parent;
                return new Edit<SyntaxNodeOrToken>(delete);
            }

            var node = ConverterHelper.ConvertCSharpToTreeNode(list.First().Parent.Value);
            var transformed = ProcessScriptOnNode(script, node);

            var @from = ConverterHelper.ConvertCSharpToTreeNode(list.First().Parent.Value);
            var to = ConverterHelper.MakeACopy(transformed);
            var update = new Update<SyntaxNodeOrToken>(@from, to, parent);
            return new Edit<SyntaxNodeOrToken>(update);
        }

        /// <summary>
        /// Get the not in which the operations are being executed.
        /// The parent is computed based on the position of the nodes in the syntax tree.
        /// If a particular node occurs first in the tree than another node.
        /// Then, it is a candidate for the parent position, else the other node is a candidate for the parent position.
        /// If two nodes occur in the same position of the syntax tree,
        /// then the node that is an ancestor of another node is the candidate for the parent position.
        /// </summary>
        /// <param name="script">Edit script</param>
        /// <param name="inpTree">Input tree</param>
        /// <returns></returns>
        private static TreeNode<SyntaxNodeOrToken> GetParent(Script script, TreeNode<SyntaxNodeOrToken> inpTree)
        {
            var listNodes = new List<SyntaxNodeOrToken>();
            foreach (var v in script.Edits)
            {
                TreeNode<SyntaxNodeOrToken> tocompare = null;
                if (v.EditOperation is Update<SyntaxNodeOrToken> || v.EditOperation is Delete<SyntaxNodeOrToken>)
                {
                    tocompare = v.EditOperation.T1Node;
                }
                else
                {
                    tocompare = v.EditOperation.Parent;
                }

                listNodes.Add(tocompare.Value);       
            }
            var lca = LCAManager.GetInstance().LeastCommonAncestor(listNodes, inpTree.Value);
            var parent = ConverterHelper.ConvertCSharpToTreeNode(lca);
            return parent;
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

        private static void ConfigureContext(TreeNode<SyntaxNodeOrToken> anchor, Edit<SyntaxNodeOrToken> edit)
        {
            var treeUp = new TreeUpdate(anchor);
            ConfigureParentSyntaxTree(edit, anchor);
            if (!WitnessFunctions.TreeUpdateDictionary.ContainsKey(anchor))
            {
                WitnessFunctions.TreeUpdateDictionary.Add(anchor, treeUp);
            }
            WitnessFunctions.CurrentTrees[anchor] = anchor;
        }

        public static Tuple<TreeNode<SyntaxNodeOrToken>, TreeNode<SyntaxNodeOrToken>> ConfigContextBeforeAfterNode(Edit<SyntaxNodeOrToken> edit, TreeNode<SyntaxNodeOrToken> inputTree)
        {
            //Get a reference for the node that was modified on the T1 tree.
            TreeNode<SyntaxNodeOrToken> inputNode = null;
            //if (TreeUpdate.FindNode(inputTree, edit.EditOperation.T1Node.Value) != null)
            if (edit.EditOperation is Delete<SyntaxNodeOrToken> || edit.EditOperation is Update<SyntaxNodeOrToken>)
            {
                inputNode = edit.EditOperation.T1Node;
            }

            var previousTree = new TreeUpdate(inputTree);
            var copy = ConverterHelper.MakeACopy(inputTree);

            Tuple<TreeNode<SyntaxNodeOrToken>, TreeNode<SyntaxNodeOrToken>> beforeAfterAnchorNode;
            TreeNode<SyntaxNodeOrToken> treeNode = null;
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

        private static Tuple<TreeNode<SyntaxNodeOrToken>, TreeNode<SyntaxNodeOrToken>> GetBeforeAfterAnchorNode(TreeNode<SyntaxNodeOrToken> node)
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
            return Tuple.Create<TreeNode<SyntaxNodeOrToken>, TreeNode<SyntaxNodeOrToken>>(null, null);
        }

        private static void ConfigureParentSyntaxTree(Edit<SyntaxNodeOrToken> edit, TreeNode<SyntaxNodeOrToken> syntaxTree)
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

        /// <summary>
        /// Compute the edition edit
        /// </summary>
        /// <param name="inpTree">Input tree</param>
        /// <param name="outTree">Output tree</param>
        /// <returns>Computed edit edit</returns>
        private static List<EditOperation<SyntaxNodeOrToken>> Script(SyntaxNodeOrToken inpTree, SyntaxNodeOrToken outTree)
        {
            var gumTreeMapping = new GumTreeMapping<SyntaxNodeOrToken>();
            var inpNode = ConverterHelper.ConvertCSharpToTreeNode(inpTree);
            var outNode = ConverterHelper.ConvertCSharpToTreeNode(outTree);
            var mapping = gumTreeMapping.Mapping(inpNode, outNode);

            var generator = new EditScriptGenerator<SyntaxNodeOrToken>();
            var script = generator.EditScript(inpNode, outNode, mapping);
            return script;
        }
    }
}
