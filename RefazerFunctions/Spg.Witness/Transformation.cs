using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using DbscanImplementation;
using LCA.Spg.Manager;
using LongestCommonAncestor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using RefazerFunctions.Bean;
using RefazerFunctions.Substrings;
using RefazerFunctions.Spg.Config;
using TreeEdit.Spg.Clustering;
using TreeEdit.Spg.ConnectedComponents;
using TreeEdit.Spg.Print;
using TreeEdit.Spg.Script;
using TreeEdit.Spg.TreeEdit.Update;
using TreeElement.Spg.Mapping;
using TreeElement.Spg.Node;

namespace RefazerFunctions.Spg.Witness
{
    /// <summary>
    /// This class specifies back-propagation functions for Transformation operator.
    /// </summary>
    public class Transformation
    {
        /// <summary>
        /// Specifies the back-propagation function for the rule parameter of the transformation operator.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="spec">Example specification</param>
        [SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public static SubsequenceSpec TransformationRule(GrammarRule rule, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            var dictionaryConnectedComponents = new Dictionary<State, List<List<EditOperation<SyntaxNodeOrToken>>>>();
            var listConnectedComponents = new List<List<EditOperation<SyntaxNodeOrToken>>>();
            var transSizeList = new List<int>();
            foreach (State input in spec.ProvidedInputs)
            {
                var inpTreeNode = (Node) input[rule.Body[0]];
                var inpTree = inpTreeNode.Value.Value;
                foreach (SyntaxNodeOrToken outTree in spec.DisjunctiveExamples[input])
                {
                    var script = Script(inpTree, outTree);
                    transSizeList.Add(script.Count);                
                    var primaryEditions = ConnectedComponentManager<SyntaxNodeOrToken>.PrimaryEditions(script);
                    var connectedComponentsInput = ConnectedComponentManager<SyntaxNodeOrToken>.ConnectedComponents(primaryEditions, script, inpTreeNode.Value);
                    dictionaryConnectedComponents[input] = connectedComponentsInput;
                    listConnectedComponents.AddRange(connectedComponentsInput);
                }
            }
            var clusters = ClusterScript(listConnectedComponents);
            foreach (State input in spec.ProvidedInputs)
            {
                var inpTree = (Node)input[rule.Body[0]];
                SyntaxNodeOrToken outTree = (SyntaxNodeOrToken) spec.DisjunctiveExamples[input].SingleOrDefault();
                //Compacted scripts for this input
                var scriptsInput = new List<List<Script>>();
                foreach (var cluster in clusters)
                {
                    //Connected components for this input
                    var connectedComponentsInput = dictionaryConnectedComponents[input];
                    //Select from the cluster the connected components related to the input.
                    var connectedComponentsInputInCluster = cluster.Where(o => connectedComponentsInput.Any(e => IsEquals(o.Edits, e)));
                    scriptsInput.Add(connectedComponentsInputInCluster.ToList());
                }
                var compactedEditsInput = scriptsInput.Select(o => CompactScript(o, inpTree.Value, outTree)).ToList();
                kExamples[input] = new List<List<List<Edit<SyntaxNodeOrToken>>>> { compactedEditsInput };
            }
            if (SynthesisConfig.GetInstance().CreateLog) SaveToFile(transSizeList);
            var subsequence = new SubsequenceSpec(kExamples);
            return subsequence;
        }

        /// <summary>
        /// Logs the size of the transformation based on the number of edit operations computed by a distance tree algorithm
        /// </summary>
        /// <param name="transSizeList">Size of the transformation</param>
        private static void SaveToFile(List<int> transSizeList)
        {
            var expHome = Environment.GetEnvironmentVariable("EXP_HOME", EnvironmentVariableTarget.User);
            var filePath = expHome + "scriptsize.txt";
            var s = "";
            for(int i = 0; i < transSizeList.Count - 1; i++)
            {
                var trans = transSizeList[i];
                s += trans + "\n";
            }
            s += transSizeList.Last();
            StreamWriter file = new StreamWriter(filePath);
            file.Write(s);
            file.Close();
        }

        /// <summary>
        /// Segment the edit in nodes
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="spec">Example specification</param>
        public static SubsequenceSpec EditMapTNode(GrammarRule rule, SubsequenceSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<TreeNode<SyntaxNodeOrToken>>();
                for (int i = 0; i < spec.PositiveExamples[input].Count(); i++)
                {
                    var examples = (List<Edit<SyntaxNodeOrToken>>)spec.PositiveExamples[input];
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

        /// <summary>
        /// Determines whether two edit scripts are equal
        /// </summary>
        /// <param name="firstEditScript">First edit script</param>
        /// <param name="secondEditScript">Second edit script</param>
        private static bool IsEquals(List<Edit<SyntaxNodeOrToken>> firstEditScript, List<EditOperation<SyntaxNodeOrToken>> secondEditScript)
        {
            if (firstEditScript.Count != secondEditScript.Count) return false;
            for (int i = 0; i < firstEditScript.Count(); i++)
            {
                var editI = firstEditScript[i].EditOperation;
                var editj = secondEditScript[i];

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
        /// Clusters connected components
        /// </summary>
        /// <param name="connectedComponents">Connected components</param>
        [SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
        [SuppressMessage("ReSharper", "InvertIf")]
        public static List<List<Script>> ClusterScript(List<List<EditOperation<SyntaxNodeOrToken>>> connectedComponents)
        {
            var clusteredList = new List<List<Script>>();
            if (connectedComponents.Any())
            {
                var clusters = ClusterConnectedComponents(connectedComponents);
                foreach (var cluster in clusters)
                {
                    var listEdit = cluster.Select(clusterList => new Script(clusterList.Select(o => new Edit<SyntaxNodeOrToken>(o)).ToList())).ToList();
                    clusteredList.Add(listEdit);
                }
            }
            return clusteredList;
        }

        /// <summary>
        /// Compacts edit operations in single edit operations
        /// </summary>
        /// <param name="connectedComponents">Non-compacted edit operations</param>
        /// <param name="inpTree">Input tree</param>
        private static List<Edit<SyntaxNodeOrToken>> CompactScript(List<Script> connectedComponents, TreeNode<SyntaxNodeOrToken> inpTree, SyntaxNodeOrToken outTree)
        {
            var newEditOperations = new List<Edit<SyntaxNodeOrToken>>();
            foreach (var script in connectedComponents)
            {
                var edit = CompactScriptIntoASingleOperation(inpTree, script);
                if (edit == null) throw new Exception("Unable to create edit operation!!");
                if (!inpTree.IsLabel(new TLabel(outTree.Kind())) && edit.EditOperation.T1Node.Equals(inpTree))
                {
                    if (edit.EditOperation is Update<SyntaxNodeOrToken>)
                    {
                        var update = (Update<SyntaxNodeOrToken>) edit.EditOperation;
                        var to = update.To;
                        to.Value = outTree;
                        to.Label = new TLabel(outTree.Kind());
                    }
                }
                newEditOperations.Add(edit);
            }
            return newEditOperations;
        }

        private static Edit<SyntaxNodeOrToken> CompactScriptIntoASingleOperation(TreeNode<SyntaxNodeOrToken> inpTree, Script script)
        {
            //check base case
            if (script.Edits.Count == 1)
            {
                return script.Edits.First();
            }

            if (script.Edits.All(o => o.EditOperation is Delete<SyntaxNodeOrToken>)) return ProcessDeleteOperations(script, inpTree);

            //parent of the edit operations
            var parent = GetParent(script, inpTree);
            var parentCopy = ConverterHelper.MakeACopy(parent);
            //transformed version of the parent node.
            var transformed = ProcessScriptOnNode(script, parentCopy);
            var primaryEditions = PrimaryEditions(script);
            if (primaryEditions.Count > 1)
            {
                var editOp = ProcessRootNodeHasMoreThanOneChild(script, primaryEditions, parent, transformed);
                return editOp;
            }

            var rootOperation = script.Edits.Find(o => !(o.EditOperation is Delete<SyntaxNodeOrToken>)).EditOperation;
            if (rootOperation is Insert<SyntaxNodeOrToken>)
            {
                var insertedNode = TreeUpdate.FindNode(transformed, rootOperation.T1Node.Value);
                var newParentInsertedNode = ConverterHelper.ConvertCSharpToTreeNode(transformed.Value);
                var insert = new Insert<SyntaxNodeOrToken>(insertedNode, newParentInsertedNode, rootOperation.K);
                return new Edit<SyntaxNodeOrToken>(insert);
            }
            return null;
        }

        private static List<Edit<SyntaxNodeOrToken>> PrimaryEditions(Script script)
        {
            var edits = script.Edits.Select(o => o.EditOperation).ToList();
            var primaryEditions = ConnectedComponentManager<SyntaxNodeOrToken>.PrimaryEditions(edits);
            var children = primaryEditions.Select(o => new Edit<SyntaxNodeOrToken>(o)).ToList();
            return children;
        }

        private static Edit<SyntaxNodeOrToken> ProcessRootNodeHasMoreThanOneChild(Script script, List<Edit<SyntaxNodeOrToken>> children, TreeNode<SyntaxNodeOrToken> parent, TreeNode<SyntaxNodeOrToken> transformed)
        {
            if (IsDirectUpdate(children, parent, transformed))
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
                var toNode = transformed;
                var @from = ConverterHelper.ConvertCSharpToTreeNode(parent.Value);
                var oldParent = ConverterHelper.ConvertCSharpToTreeNode(parent.Value.Parent);
                var update = new Update<SyntaxNodeOrToken>(@from, toNode, oldParent);
                {
                    var operation = new Edit<SyntaxNodeOrToken>(update);
                    return operation;
                }
            }
        }

        private static bool IsDirectUpdate(List<Edit<SyntaxNodeOrToken>> children, TreeNode<SyntaxNodeOrToken> parent, TreeNode<SyntaxNodeOrToken> transformed)
        {
            if (children.Count != 2) return false;

            if (!(children.First().EditOperation is Insert<SyntaxNodeOrToken> &&
                  children.ElementAt(1).EditOperation is Delete<SyntaxNodeOrToken>)) return false;

            var indexInsert = transformed.Children.FindIndex(o => o.Equals(children.First().EditOperation.T1Node));
            var indexDelete = parent.Children.FindIndex(o => o.Equals(children.ElementAt(1).EditOperation.T1Node));
            return indexInsert == indexDelete;
        }

        private static TreeNode<SyntaxNodeOrToken> ProcessScriptOnNode(Script script, TreeNode<SyntaxNodeOrToken> parent)
        {
            var treeUpdate = new TreeUpdate(parent);
            foreach (var s in script.Edits)
            {
                treeUpdate.ProcessEditOperation(s.EditOperation);
                Debug.WriteLine("New Tree: \n\n");
                PrintUtil<SyntaxNodeOrToken>.PrintPrettyDebug(treeUpdate.CurrentTree, "", true);
            }
            return treeUpdate.CurrentTree;
        }

        private static Edit<SyntaxNodeOrToken> ProcessDeleteOperations(Script script, TreeNode<SyntaxNodeOrToken> inpTree)
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

            //parent of the edit operations
            var parent = GetParent(script, inpTree);

            var @from = ConverterHelper.ConvertCSharpToTreeNode(list.First().Parent.Value);
            var to = ConverterHelper.MakeACopy(transformed);
            var update = new Update<SyntaxNodeOrToken>(@from, to, parent);
            return new Edit<SyntaxNodeOrToken>(update);
        }

        /// <summary>
        /// Get the not in which the operations are being executed.
        /// </summary>
        /// <param name="script">Edit script</param>
        /// <param name="inpTree">Input tree</param>
        /// <returns></returns>
        private static TreeNode<SyntaxNodeOrToken> GetParent(Script script, TreeNode<SyntaxNodeOrToken> inpTree)
        {
            var listNodes = new List<SyntaxNodeOrToken>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var v in script.Edits)
            {
                var tocompare = (v.EditOperation is Update<SyntaxNodeOrToken> || v.EditOperation is Delete<SyntaxNodeOrToken>) ? v.EditOperation.T1Node : v.EditOperation.Parent;
                var t1Node = TreeUpdate.FindNode(inpTree, tocompare.Value);
                if (t1Node != null)
                {
                    listNodes.Add(tocompare.Value);
                }
            }
            var lca = LCAManager.GetInstance().LeastCommonAncestor(listNodes, inpTree.Value);
            var parent = ConverterHelper.ConvertCSharpToTreeNode(lca);
            return parent;
        }

        /// <summary>
        /// Compact similar edit operations in a single edit operation
        /// </summary>
        /// <param name="connectedComponents">Non-compacted edit operations</param>
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
            var xEditOperations = x.Operations.Where(o => !o.T1Node.Value.IsKind(SyntaxKind.IdentifierToken) || o is Update<SyntaxNodeOrToken>).ToList();
            var yEditOperations = y.Operations.Where(o => !o.T1Node.Value.IsKind(SyntaxKind.IdentifierToken) || o is Update<SyntaxNodeOrToken>).ToList();
            var common = (double)lcc.FindCommon(xEditOperations, yEditOperations).Count;
            var dist = 1.0 - (2 * common) / ((double) xEditOperations.Count + (double) yEditOperations.Count);
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
            if (edit.EditOperation is Delete<SyntaxNodeOrToken> || edit.EditOperation is Update<SyntaxNodeOrToken>)
            {
                inputNode = edit.EditOperation.T1Node;
            }
            var previousTree = new TreeUpdate(inputTree);
            Tuple<TreeNode<SyntaxNodeOrToken>, TreeNode<SyntaxNodeOrToken>> beforeAfterAnchorNode;
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
        /// <returns>Computed edit</returns>
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
