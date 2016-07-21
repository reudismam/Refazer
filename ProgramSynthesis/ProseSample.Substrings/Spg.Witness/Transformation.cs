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
using TreeElement.Spg.Walker;

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
                //edits = edits.GetRange(0, 2);
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
                    var ccs = ConnectedComponentMannager<SyntaxNodeOrToken>.ConnectedComponents(script);
                    kMatches = ClusterScript(ccs);
                    //var newccs = CompactScript(ccs);
                    kMatches = kMatches.Select(CompactScript).ToList();
                }
                kExamples[input] = new List<List<List<Script>>> { kMatches };
            }
            var subsequence = new SubsequenceSpec(kExamples);
            return subsequence;
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
                var inputTree = (Node)input[rule.Grammar.InputSymbol];
                var inputTreeCopy = ConverterHelper.MakeACopy(inputTree.Value);
                var nodes = new List<Node>();

                foreach (Script script in spec.Examples[input])
                {
                    var connectedComponents = ComputeConnectedComponents(script);
                    var tree = BuildTree(connectedComponents, ConverterHelper.MakeACopy(inputTree.Value));
                    var centroid = GetCentroid(tree);

                    var node = ConfigAnchorBeforeAfterNode(script.Edits.First(), centroid, inputTreeCopy);
                    nodes.Add(node);
                }

                var kMatches = new List<Node>();
                if (nodes.Any(node => node.LeftNode == null)) nodes.ForEach(node => node.LeftNode = null);
                if (nodes.Any(node => node.RightNode == null)) nodes.ForEach(node => node.RightNode = null);
               
                for (int i = 0; i < spec.Examples[input].Count(); i++)
                {
                    var script = (Script)spec.Examples[input].ElementAt(i);
                    var node = nodes[i];
                    var emptyNode = new TreeNode<SyntaxNodeOrToken>(SyntaxFactory.EmptyStatement(), new TLabel(SyntaxKind.EmptyStatement));

                    int j = 0;
                    if (node.LeftNode != null)
                    {
                        emptyNode.AddChild(node.LeftNode, j++);
                        script.Edits.First().EditOperation.K = 2;
                    }
                    else
                    {
                        script.Edits.First().EditOperation.K = 1;
                    }

                    if (node.Value != null) emptyNode.AddChild(node.Value, j++);
                    if (node.RightNode != null) emptyNode.AddChild(node.RightNode, j);

                    //Todo refactor this code.
                    script.Edits.First().EditOperation.Parent = ConverterHelper.ConvertCSharpToTreeNode(SyntaxFactory.EmptyStatement());
                    
                    kMatches.Add(new Node(emptyNode));
                    ConfigureContext(emptyNode, script);
                }
                kExamples[input] = kMatches;
            }
            return new SubsequenceSpec(kExamples);
        }


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
        private static List<Script> CompactScript(List<Script> connectedComponents)
        {
            var newccs = new List<Script>();
            foreach (var cc in connectedComponents)
            {
                var editionConnected = ConnectedComponentMannager<SyntaxNodeOrToken>.EditConnectedComponents(cc.Edits.Select(o => o.EditOperation).ToList());
                var newScript = Compact(editionConnected);
                newccs.Add(new Script(newScript.Select(o => new Edit<SyntaxNodeOrToken>(o)).ToList()));
            }
            return newccs;
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
                            editI.T1Node.AddChild(editJ.T1Node, editI.T1Node.Children.Count);
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
            var dbs = new DbscanAlgorithm<EditOperationDatasetItem>((x, y) => 1.0 - (2 * (double)lcc.FindCommon(x.Operations, y.Operations).Count) / ((double)x.Operations.Count + (double)y.Operations.Count));
            dbs.ComputeClusterDbscan(allPoints: featureData, epsilon: 0.5, minPts: 1, clusters: out clusters);
            return clusters;
        }

        private static void ConfigureContext(ITreeNode<SyntaxNodeOrToken> anchor, Script script)
        {
            //var newAnchor = new TreeNode<SyntaxNodeOrToken>(anchor.Parent.Value, new TLabel(anchor.Parent.Label), new List<ITreeNode<SyntaxNodeOrToken>> {anchor});
            //anchor.SyntaxTree = newAnchor;
            var treeUp = new TreeUpdate(anchor);
            ConfigureParentSyntaxTree(script, anchor);
            WitnessFunctions.TreeUpdateDictionary.Add(anchor, treeUp);
            WitnessFunctions.CurrentTrees[anchor] = anchor;
        }

        private static Node ConfigAnchorBeforeAfterNode(Edit<SyntaxNodeOrToken> edit, ITreeNode<SyntaxNodeOrToken> anchorNode, ITreeNode<SyntaxNodeOrToken> inputTree)
        {
            //Get a reference for the node that was modified on the T1 tree.
            ITreeNode<SyntaxNodeOrToken> inputNode = null;
            if (TreeUpdate.FindNode(inputTree, edit.EditOperation.T1Node.Value) != null)
            {
                inputNode = anchorNode;
            }

            var previousTree = new TreeUpdate(inputTree);

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

            //Get the nodes before and after the input node.
            //Location of the left, right, and after node.
            var leftNode = beforeAfterAnchorNode.Item1 != null ? TreeUpdate.FindNode(inputTree, beforeAfterAnchorNode.Item1.Value) : null;
            var rightNode = beforeAfterAnchorNode.Item2 != null ? TreeUpdate.FindNode(inputTree, beforeAfterAnchorNode.Item2.Value) : null;
            var emptyNode = ConverterHelper.ConvertCSharpToTreeNode(SyntaxFactory.EmptyStatement());

            //Configure the parent node.
            int i = 0;
            if (leftNode != null) emptyNode.AddChild(leftNode, i++);
            if (treeNode != null) emptyNode.AddChild(treeNode, i++);
            if (rightNode != null) emptyNode.AddChild(rightNode, i);

            var node = new Node(treeNode, leftNode, rightNode);
            return node;
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
            return null;
        }

        private static ITreeNode<SyntaxNodeOrToken> GetCentroid(ITreeNode<SyntaxNodeOrToken> inpTree)
        {
            if (inpTree.Children.Count == 1 && ((SyntaxNode)inpTree.Value).ChildNodes().Count() > 1)
            {
                return inpTree.Children.Single();
            }
            if (!inpTree.Children.Any() && !inpTree.Value.IsKind(SyntaxKind.Block))
            {
                var itreeNode = ConverterHelper.ConvertCSharpToTreeNode(inpTree.Value);
                return itreeNode;
            }
            return inpTree;
        }

        private static void ConfigureParentSyntaxTree(Script script, ITreeNode<SyntaxNodeOrToken> region)
        {
            foreach (var edit in script.Edits)
            {
                edit.EditOperation.T1Node.SyntaxTree = region;
                if (edit.EditOperation.Parent != null)
                {
                    edit.EditOperation.Parent.SyntaxTree = region;
                }

                if (edit.EditOperation is Update<SyntaxNodeOrToken>)
                {
                    var update = (Update<SyntaxNodeOrToken>)edit.EditOperation;
                    update.To.SyntaxTree = region;
                }
            }
        }

        /// <summary>
        /// Compute connected components from the script
        /// </summary>
        /// <param name="script">Script</param>
        private static List<Script> ComputeConnectedComponents(Script script)
        {
            var editOperations = script.Edits.Select(o => o.EditOperation).ToList();
            var connectedComponents = ConnectedComponentMannager<SyntaxNodeOrToken>.DescendantsConnectedComponents(editOperations);
            connectedComponents = connectedComponents.OrderBy(o => o.First().T1Node.Value.SpanStart).ToList();
            var scripts = connectedComponents.Select(component => new Script(component.Select(o => new Edit<SyntaxNodeOrToken>(o)).ToList())).ToList();
            return scripts;
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

        private static ITreeNode<SyntaxNodeOrToken> BuildTree(List<Script> ccs, ITreeNode<SyntaxNodeOrToken> inpTree)
        {
            return ccs.Select(cc => BuildTemplate(cc, inpTree)).Select(template => template.First()).OrderBy(o => o.Value.SpanStart).Single();
        }

        private static List<ITreeNode<SyntaxNodeOrToken>> BuildTemplate(Script script, ITreeNode<SyntaxNodeOrToken> tree)
        {
            var nodes = BFSWalker<SyntaxNodeOrToken>.BreadFirstSearch(tree);
            var list = new List<ITreeNode<SyntaxNodeOrToken>>();
            var editNodes = EditNodes(script);
            foreach (var node in nodes)
            {
                foreach (var t1Node in editNodes)
                {
                    if (node.Equals(t1Node))
                    {
                        if (!list.Contains(node))
                        {
                            list.Add(node);
                        }
                    }
                }
            }
            var tcc = new TemplateConnectedComponents<SyntaxNodeOrToken>();
            var cnodes = tcc.ConnectedNodes(list);
            return cnodes;
        }

        private static List<ITreeNode<SyntaxNodeOrToken>> EditNodes(Script script)
        {
            var nodes = new List<ITreeNode<SyntaxNodeOrToken>>();
            foreach (var edit in script.Edits)
            {
                nodes.AddRange(edit.EditOperation.T1Node.DescendantNodesAndSelf());

                if (!(edit.EditOperation is Delete<SyntaxNodeOrToken>))
                {
                    nodes.AddRange(edit.EditOperation.Parent.DescendantNodesAndSelf());
                }
            }
            return nodes;
        }
    }
}
