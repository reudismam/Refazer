using System.Collections.Generic;
using System.Linq;
using DbscanImplementation;
using LongestCommonSubsequence;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
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
        public static ExampleSpec ScriptEdits(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var editsExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var script = (List<Edit<SyntaxNodeOrToken>>)spec.Examples[input];
                //script = script.GetRange(0, 3);
                editsExamples[input] = script;
            }
            return new ExampleSpec(editsExamples);
        }

        public static SubsequenceSpec TransformationLoop(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            //TODO Refactor the code TransformationLoop
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<List<List<Edit<SyntaxNodeOrToken>>>>();
                var inpTreeNode = (Node)input[rule.Body[0]];
                var inpTree = inpTreeNode.Value.Value;
                foreach (SyntaxNodeOrToken outTree in spec.DisjunctiveExamples[input])
                {
                    var script = Script(inpTree, outTree);

                    var ccs = ConnectedComponentMannager<SyntaxNodeOrToken>.ConnectedComponents(script);
                    ccs = ccs.OrderBy(o => o.First().T1Node.Value.SpanStart).ToList();

                    //PrintScript(ccs.First());

                    var newccs = new List<List<EditOperation<SyntaxNodeOrToken>>>();
                    foreach (var cc in ccs)
                    {
                        var editionConnected = ConnectedComponentMannager<SyntaxNodeOrToken>.EditConnectedComponents(cc);
                        var newScript = Compact(editionConnected);
                        newccs.Add(newScript);
                    }

                    if (ccs.Any())
                    {
                        var list = ClusterConnectedComponentsInRegions(newccs);
                        var listEdit = list.Select(cc => cc.Select(o => new Edit<SyntaxNodeOrToken>(o)).ToList()).ToList();
                        kMatches = new List<List<List<Edit<SyntaxNodeOrToken>>>> { listEdit };
                    }
                }
                kExamples[input] = new List<List<List<List<Edit<SyntaxNodeOrToken>>>>> { kMatches };
            }
            var subsequence = new SubsequenceSpec(kExamples);
            return subsequence;
        }

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

        private static List<List<EditOperation<SyntaxNodeOrToken>>> ClusterConnectedComponentsInRegions(List<List<EditOperation<SyntaxNodeOrToken>>> ccs)
        {
            var clusters = Clusters(ccs);

            var list = new List<List<EditOperation<SyntaxNodeOrToken>>>();
            for (int i = 0; i < clusters.First().Length; i++) list.Add(new List<EditOperation<SyntaxNodeOrToken>>());
            foreach (var v in clusters)
            {
                var editOperationDatasetItems = v.OrderBy(o => o.Operations.First().T1Node.Value.Span);
                for (int i = 0; i < editOperationDatasetItems.Count(); i++)
                {
                    list[i].AddRange(v.ElementAt(i).Operations);
                }
            }
            return list;
        }

        private static HashSet<EditOperationDatasetItem[]> Clusters(List<List<EditOperation<SyntaxNodeOrToken>>> ccs)
        {
            HashSet<EditOperationDatasetItem[]> clusters;
            var lcc = new LongestCommonSubsequenceManager<EditOperation<SyntaxNodeOrToken>>();
            var featureData = ccs.Select(x => new EditOperationDatasetItem(x)).ToArray();
            var dbs = new DbscanAlgorithm<EditOperationDatasetItem>((x, y) => 1.0 - (2 * (double)lcc.FindCommon(x.Operations, y.Operations).Count) / ((double)x.Operations.Count + (double)y.Operations.Count));
            dbs.ComputeClusterDbscan(allPoints: featureData, epsilon: 0.5, minPts: 1, clusters: out clusters);
            return clusters;
        }

        public static SubsequenceSpec Loop(GrammarRule rule, int parameter, SubsequenceSpec spec)
        {
            //todo REFACTOR loop method
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<Node>(); //todo refactor: put the correct referred object.
                var inpTreeNode = (Node)input[rule.Grammar.InputSymbol];
                var inpTree = inpTreeNode.Value.Value;
                var ocurrences = new List<Node>();

                foreach (List<Edit<SyntaxNodeOrToken>> cc in spec.Examples[input])
                {
                    var ccs = ComputeConnectedComponents(cc);

                    var regions = FindRegion(ccs, inpTree);
                    var tree = new TreeNode<SyntaxNodeOrToken>(SyntaxFactory.EmptyStatement(),
                        new TLabel(SyntaxKind.EmptyStatement));
                    cc.First().EditOperation.Parent = ConverterHelper.MakeACopy(tree);
                    tree.SyntaxTree = tree;

                    for (int i = 0; i < regions.Count; i++)
                        //todo BUG: correct this method to return the number of nodes corresponding to the number of regions.
                    {
                        var region = regions[i];
                        if (region.Children.Count == 1)
                        {
                            region = region.Children.First();
                        }
                        var r = ConverterHelper.ConvertCSharpToTreeNode(region.Value);
                        tree.AddChild(r, i);
                    }
                    ocurrences.Add(new Node(tree));

                    var copy = ConverterHelper.MakeACopy(tree);
                    TreeUpdate treeUp = new TreeUpdate(copy);

                    foreach (var v in cc)
                    {
                        v.EditOperation.T1Node.SyntaxTree = tree;
                        if (v.EditOperation.Parent != null)
                        {
                            v.EditOperation.Parent.SyntaxTree = tree;
                        }

                        if (v.EditOperation is Update<SyntaxNodeOrToken>)
                        {
                            var update = (Update<SyntaxNodeOrToken>)v.EditOperation;
                            update.To.SyntaxTree = tree;
                        }

                    }

                    WitnessFunctions.TreeUpdateDictionary.Add(tree, treeUp);
                    WitnessFunctions.CurrentTrees[tree] = tree;
                }

                if (ocurrences.Any())
                {
                    kMatches.AddRange(ocurrences);
                    kExamples[input] = kMatches;
                }
            }
            return new SubsequenceSpec(kExamples);
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

        private static List<ITreeNode<SyntaxNodeOrToken>> FindRegion(List<List<EditOperation<SyntaxNodeOrToken>>> ccs, SyntaxNodeOrToken inpTree)
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
