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

        public static Dictionary<ITreeNode<SyntaxNodeOrToken>, ITreeNode<SyntaxNodeOrToken>> mapping { get; set; }
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

        ///// <summary>
        ///// Compact edit operations with similar semantic in compacted edit operations
        ///// </summary>
        ///// <param name="connectedComponents">Uncompacted edit operations</param>
        //private static List<List<EditOperation<SyntaxNodeOrToken>>> CompactScript(List<List<EditOperation<SyntaxNodeOrToken>>> connectedComponents)
        //{
        //    var newccs = new List<List<EditOperation<SyntaxNodeOrToken>>>();
        //    foreach (var cc in connectedComponents)
        //    {
        //        var editionConnected = ConnectedComponentMannager<SyntaxNodeOrToken>.EditConnectedComponents(cc);
        //        var newScript = Compact(editionConnected);
        //        newccs.Add(newScript);
        //    }
        //    return newccs;
        //}


        /// <summary>
        /// Compact edit operations with similar semantic in compacted edit operations
        /// </summary>
        /// <param name="connectedComponents">Uncompacted edit operations</param>
        private static List<Script> CompactScript(List<Script> connectedComponents)
        {
            //TODO refactor this code.
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
            newList.RemoveAll(o => removes.Contains(o));
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
                var inpTreeNode = (Node)input[rule.Grammar.InputSymbol];
                var inpTree = inpTreeNode.Value.Value;
                var kMatches = new List<Node>();
                foreach (Script script in spec.Examples[input])
                {
                    var connectedComponents = ComputeConnectedComponents(script.Edits);
                    var trees = BuildTree(connectedComponents, inpTree);
                    trees = trees.Select(o => BuildPattern(o)).ToList(); //TODO refactor: trees has only a node, therefore, do not need to be a list.
                   
                    script.Edits.First().EditOperation.K = 1; //Todo Bug: this peace of code will genenate many falts.
                    if (trees.First().Value.IsKind(SyntaxKind.Block))
                    {
                        var keynode = script.Edits.First().EditOperation.T1Node;
                        var node = mapping.ToList().Find(o => o.Value.Equals(keynode)).Key;
                        var anchorNode = AnchorNode(node);
                        trees = new List<ITreeNode<SyntaxNodeOrToken>> {anchorNode};
                    }
                    else if (trees.First().Value.IsKind(SyntaxKind.EmptyStatement))
                    {
                        script.Edits.First().EditOperation.Parent = ConverterHelper.ConvertCSharpToTreeNode(SyntaxFactory.EmptyStatement());
                    }

                    foreach (var region in trees)
                    {
                        kMatches.Add(new Node(region));
                        var treeUp = new TreeUpdate(region);
                        ConfigureParentSyntaxTree(script, region);
                        WitnessFunctions.TreeUpdateDictionary.Add(region, treeUp);
                        WitnessFunctions.CurrentTrees[region] = region;
                    }
                    kExamples[input] = kMatches;
                }
            }
            return new SubsequenceSpec(kExamples);
        }

        private static ITreeNode<SyntaxNodeOrToken> AnchorNode(ITreeNode<SyntaxNodeOrToken> node)
        {
            for (int i = 0; i < node.Parent.Children.Count; i++)
            {
                var child = node.Parent.Children[i];
                if (child.Equals(node))
                {
                    var nnode = node.Parent.Children[i + 1];
                    return nnode; //Todo this piece of code will produces bugs.
                }
            }
            return null;
        }

        private static ITreeNode<SyntaxNodeOrToken> BuildPattern(ITreeNode<SyntaxNodeOrToken> inpTree)
        {
            
            if (inpTree.Children.Count() == 1 && ((SyntaxNode)inpTree.Value).ChildNodes().Count() > 1)
            {
                var emptyKind = SyntaxKind.EmptyStatement;
                var emptyStatement = SyntaxFactory.EmptyStatement();
                var emptyNode = new TreeNode<SyntaxNodeOrToken>(emptyStatement, new TLabel(emptyKind));
                emptyNode.AddChild(inpTree.Children.Single(), 0);
                return emptyNode;
            }else if (!inpTree.Children.Any() && !inpTree.Value.IsKind(SyntaxKind.Block))
            {
                var itreeNode = ConverterHelper.ConvertCSharpToTreeNode(inpTree.Value);
                return itreeNode;
            }
            else
            {
                return inpTree;
            }
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

        private static List<List<EditOperation<SyntaxNodeOrToken>>> ComputeConnectedComponents(List<Edit<SyntaxNodeOrToken>> cc)
        {
            var editOperations = cc.Select(o => o.EditOperation).ToList();
            var ccs = ConnectedComponentMannager<SyntaxNodeOrToken>.DescendantsConnectedComponents(editOperations);
            ccs = ccs.OrderBy(o => o.First().T1Node.Value.SpanStart).ToList();
            return ccs;
        }

        public static DisjunctiveExamplesSpec TemplateTemplate(GrammarRule rule, int parameter, SubsequenceSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = (from ITreeNode<SyntaxNodeOrToken> cc in spec.Examples[input] select new Node(cc)).ToList();
                kExamples[input] = kMatches;
            }

            return DisjunctiveExamplesSpec.From(kExamples);
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
            mapping = gumTreeMapping.Mapping(inpNode, outNode);

            var generator = new EditScriptGenerator<SyntaxNodeOrToken>();
            var script = generator.EditScript(inpNode, outNode, mapping);
            return script;
        }

        private static void PrintScript(List<Edit<SyntaxNodeOrToken>> script)
        {
            string s = script.Aggregate("", (current, v) => current + (v + "\n"));
        }

        private static void PrintScript(List<EditOperation<SyntaxNodeOrToken>> script)
        {
            string s = script.Aggregate("", (current, v) => current + (v + "\n"));
        }

        private static List<ITreeNode<SyntaxNodeOrToken>> BuildTree(List<List<EditOperation<SyntaxNodeOrToken>>> ccs, SyntaxNodeOrToken inpTree)
        {
            return ccs.Select(cc => BuildTemplate(cc, inpTree)).Select(template => template.First()).OrderBy(o => o.Value.SpanStart).ToList();
        }

        private static List<ITreeNode<SyntaxNodeOrToken>> BuildTemplate(List<EditOperation<SyntaxNodeOrToken>> cc, SyntaxNodeOrToken inpTree)
        {
            var input = ConverterHelper.ConvertCSharpToTreeNode(inpTree);
            var nodes = BFSWalker<SyntaxNodeOrToken>.BreadFirstSearch(input);

            var list = new List<ITreeNode<SyntaxNodeOrToken>>();

            var editNodes = EditNodes(cc);
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

        private static List<ITreeNode<SyntaxNodeOrToken>> EditNodes(List<EditOperation<SyntaxNodeOrToken>> cc)
        {
            var nodes = new List<ITreeNode<SyntaxNodeOrToken>>();
            foreach (var edit in cc)
            {
                nodes.AddRange(edit.T1Node.DescendantNodesAndSelf());

                if (!(edit is Delete<SyntaxNodeOrToken>))
                {
                    nodes.AddRange(edit.Parent.DescendantNodesAndSelf());
                }
            }
            return nodes;
        }
    }
}
