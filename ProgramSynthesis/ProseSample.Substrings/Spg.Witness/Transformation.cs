using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
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
        public static DisjunctiveExamplesSpec ScriptEdits(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var editsExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<object>();
                foreach (List<EditOperation<SyntaxNodeOrToken>> script in spec.DisjunctiveExamples[input])
                {
                    kMatches.Add(script);
                }

                editsExamples[input] = kMatches;
            }
            return DisjunctiveExamplesSpec.From(editsExamples);
        }

        public static SubsequenceSpec ManyTransLoop(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            var scrips = new List<List<EditOperation<SyntaxNodeOrToken>>>();

            WitnessFunctions.TreeUpdateDictionary = new Dictionary<object, TreeUpdate>();
            WitnessFunctions.CurrentTrees = new Dictionary<object, ITreeNode<SyntaxNodeOrToken>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<object>();
                var inpTree = (SyntaxNodeOrToken)input[rule.Body[0]];
                foreach (SyntaxNodeOrToken outTree in spec.DisjunctiveExamples[input])
                {
                    Dictionary<ITreeNode<SyntaxNodeOrToken>, ITreeNode<SyntaxNodeOrToken>> m;
                    var script = Script(inpTree, outTree, out m);
                    scrips.Add(script);

                    PrintScript(script);

                    var ccs = TreeConnectedComponents<SyntaxNodeOrToken>.ConnectedComponents(script);
                    ccs = ccs.OrderBy(o => o.First().T1Node.Value.SpanStart).ToList();

                    var cscripts = new List<EditOperation<SyntaxNodeOrToken>>();
                    foreach (var cc in ccs)
                    {
                       cscripts.AddRange(cc);
                        kMatches.Add(cc);
                        PrintScript(cc);
                    }
                    //var regions = FindRegion(ccs, inpTree);

                    //var tree = new TreeNode<SyntaxNodeOrToken>(SyntaxFactory.EmptyStatement(), new TLabel(SyntaxKind.EmptyStatement));

                    //for (int i = 0; i < regions.Count; i++)
                    //{
                    //    var r = regions[i];
                    //    tree.AddChild(r, i);
                    //}


                    //kMatches.Add(cscripts);
                }
                kExamples[input] = kMatches;
            }

            var subsequenceSpec = new SubsequenceSpec(kExamples);
            return subsequenceSpec;
        }

        public static SubsequenceSpec Loop(GrammarRule rule, int parameter, SubsequenceSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = spec.Examples[input].Cast<List<EditOperation<SyntaxNodeOrToken>>>().Cast<object>().ToList();

                kExamples[input] = kMatches;
            }
            return new SubsequenceSpec(kExamples);
        }

        public static DisjunctiveExamplesSpec TemplateTemplate(GrammarRule rule, int parameter, SubsequenceSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<object>();
                var inpTree = (SyntaxNodeOrToken)input[rule.Body[0]];
                var ocurrences = new List<ITreeNode<SyntaxNodeOrToken>>();
                foreach (List<EditOperation<SyntaxNodeOrToken>> cc in spec.Examples[input])
                {
                    var ccs = TreeConnectedComponents<SyntaxNodeOrToken>.ConnectedComponents(cc);
                    ccs = ccs.OrderBy(o => o.First().T1Node.Value.SpanStart).ToList();

                    var regions = FindRegion(ccs, inpTree);

                    var tree = new TreeNode<SyntaxNodeOrToken>(SyntaxFactory.EmptyStatement(), new TLabel(SyntaxKind.EmptyStatement));
                    var treePattern = new TreeNode<SyntaxNodeOrToken>(SyntaxFactory.EmptyStatement(), new TLabel(SyntaxKind.EmptyStatement));

                    for (int i = 0; i < regions.Count; i++)
                    {
                        var r = ConverterHelper.ConvertCSharpToTreeNode(regions[i].Value);
                        tree.AddChild(r, i);
                        treePattern.AddChild(regions[i], i);
                    }

                    TreeUpdate treeUp = new TreeUpdate(tree);
                    WitnessFunctions.TreeUpdateDictionary.Add(cc, treeUp);
                    WitnessFunctions.CurrentTrees[cc] = tree;

                    ocurrences.Add(treePattern);
                }

                if (ocurrences.Any())
                {
                    kMatches.Add(ocurrences);
                    kExamples[input] = kMatches;
                }
            }

            return DisjunctiveExamplesSpec.From(kExamples);
        }

        /// <summary>
        /// Compute the edition script
        /// </summary>
        /// <param name="inpTree">Input tree</param>
        /// <param name="outTree">Output tree</param>
        /// <param name="m">out Mapping</param>
        /// <returns>Computed edit script</returns>
        private static List<EditOperation<SyntaxNodeOrToken>> Script(SyntaxNodeOrToken inpTree, SyntaxNodeOrToken outTree, out Dictionary<ITreeNode<SyntaxNodeOrToken>, ITreeNode<SyntaxNodeOrToken>> m)
        {
            var mapping = new GumTreeMapping<SyntaxNodeOrToken>();

            var inpNode = ConverterHelper.ConvertCSharpToTreeNode(inpTree);
            var outNode = ConverterHelper.ConvertCSharpToTreeNode(outTree);
            m = mapping.Mapping(inpNode, outNode);

            var generator = new EditScriptGenerator<SyntaxNodeOrToken>();
            var script = generator.EditScript(inpNode, outNode, m);
            return script;
        }

        private static void PrintScript(List<EditOperation<SyntaxNodeOrToken>> script)
        {
            string s = script.Aggregate("", (current, v) => current + (v + "\n"));
        }

        private static List<ITreeNode<SyntaxNodeOrToken>> FindRegion(List<List<EditOperation<SyntaxNodeOrToken>>> ccs, SyntaxNodeOrToken inpTree)
        {
            return ccs.Select(cc => BuildTemplate(cc, inpTree)).Select(template => template.First()).ToList();
        }

        private static List<ITreeNode<SyntaxNodeOrToken>> BuildTemplate(List<EditOperation<SyntaxNodeOrToken>> cc, SyntaxNodeOrToken inpTree)
        {
            var input = ConverterHelper.ConvertCSharpToTreeNode(inpTree);
            var nodes = BFSWalker<SyntaxNodeOrToken>.BreadFirstSearch(input);

            var list = new List<ITreeNode<SyntaxNodeOrToken>>();

            foreach (var node in nodes)
            {
                foreach (var edit in cc)
                {
                    if (node.Equals(edit.T1Node))
                    {
                        if (!list.Contains(node))
                        {
                            list.Add(node);
                        }
                    }

                    if (!(edit is Delete<SyntaxNodeOrToken>) && node.Equals(edit.T1Node.Parent))
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
    }
}
