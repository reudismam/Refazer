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
                    var newccs = CompactScript(ccs);
                    kMatches = ClusterScript(newccs);
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
            var kMatches = new List<List<Script>>();
            if (clusteredEdits.Any())
            {
                var list = ClusterConnectedComponents(clusteredEdits);
                var listEdit = list.Select(cc => new Script(cc.Select(o => new Edit<SyntaxNodeOrToken>(o)).ToList())).ToList();
                kMatches = new List<List<Script>> { listEdit };
            }
            return kMatches;
        }

        /// <summary>
        /// Compact edit operations with similar semantic in compacted edit operations
        /// </summary>
        /// <param name="connectedComponents">Uncompacted edit operations</param>
        private static List<List<EditOperation<SyntaxNodeOrToken>>> CompactScript(List<List<EditOperation<SyntaxNodeOrToken>>> connectedComponents)
        {
            var newccs = new List<List<EditOperation<SyntaxNodeOrToken>>>();
            foreach (var cc in connectedComponents)
            {
                var editionConnected = ConnectedComponentMannager<SyntaxNodeOrToken>.EditConnectedComponents(cc);
                var newScript = Compact(editionConnected);
                newccs.Add(newScript);
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
        private static List<List<EditOperation<SyntaxNodeOrToken>>> ClusterConnectedComponents(List<List<EditOperation<SyntaxNodeOrToken>>> connectedComponents)
        {
            var clusters = Clusters(connectedComponents);
            var list = new List<List<EditOperation<SyntaxNodeOrToken>>>();
            foreach (var cluster in clusters)
            {
                list.AddRange(cluster.Select(item => item.Operations));
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
            var ccs = ConnectedComponentMannager<SyntaxNodeOrToken>.ConnectedComponents(editOperations);
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
            var mapping = gumTreeMapping.Mapping(inpNode, outNode);

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
