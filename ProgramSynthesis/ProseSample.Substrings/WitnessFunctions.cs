using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using ProseSample.Substrings.List;
using TreeEdit.Spg.ConnectedComponents;
using TreeEdit.Spg.Print;
using TreeEdit.Spg.Isomorphic;
using TreeEdit.Spg.Match;
using TreeEdit.Spg.Script;
using TreeEdit.Spg.TreeEdit.Mapping;
using TreeEdit.Spg.TreeEdit.Update;
using TreeEdit.Spg.Walker;
using Tutor.Spg.Node;
using ProseSample.Substrings.Spg.Witness;

namespace ProseSample.Substrings
{
    public static class WitnessFunctions
    {
        /// <summary>
        /// Current trees.
        /// </summary>
        private static Dictionary<object, ITreeNode<SyntaxNodeOrToken>> _currentTrees = new Dictionary<object, ITreeNode<SyntaxNodeOrToken>>();

        /// <summary>
        /// TreeUpdate mapping.
        /// </summary>
        private static Dictionary<object, TreeUpdate> _treeUpdateDictionary = new Dictionary<object, TreeUpdate>();

        /// <summary>
        /// Literal witness function for parameter tree.
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Literal", 1)]
        public static DisjunctiveExamplesSpec LiteralTree(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return Literal.LiteralTree(rule, parameter, spec);
        }

        /// <summary>
        /// Literal witness function for parameter tree.
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <param name="treeBinding">TreeBinding</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Literal", 2, DependsOnParameters = new []{1})]
        public static DisjunctiveExamplesSpec LiteralK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec treeBinding)
        {
            return Literal.LiteralK(rule, parameter, spec, treeBinding);
        }

        /// <summary>
        /// Concrete witness function for parameter tree.
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Concrete", 0)]
        public static DisjunctiveExamplesSpec ConcreteTree(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (var input in spec.ProvidedInputs)
            {
                var literalExamples = new List<object>();
                foreach (List<ITreeNode<SyntaxNodeOrToken>> treeNodes in spec.DisjunctiveExamples[input])
                {
                    var tree = treeNodes.First();

                    foreach (var sot in treeNodes.Select(matchResult => matchResult))
                    {
                        if (sot.Value.IsToken || sot.Children.Any()) return null;

                        if (!IsomorphicManager<SyntaxNodeOrToken>.IsIsomorphic(tree, sot)) return null;
                    }
                    literalExamples.Add(tree.Value);
                }
                treeExamples[input] = literalExamples;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        /// <summary>
        /// Abstract witness function for parameter kind
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Rule parameter</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Abstract", 0)]
        public static DisjunctiveExamplesSpec AbstractKind(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kindExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (var input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (List<ITreeNode<SyntaxNodeOrToken>> treeNodes in spec.DisjunctiveExamples[input])
                {
                    var first = treeNodes.First().Value;
                    var kind = first.Kind();
                    if (treeNodes.Any(t => !t.Value.IsKind(kind) || t.Children.Any())) return null;

                    if (treeNodes.All(matchResult => matchResult.Value.ToString().Equals(first.ToString()))) return null;

                    matches.Add(kind);
                }
                kindExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(kindExamples);
        }

        /// <summary>
        /// Tree witness function for parameter tree.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Example specificaiton</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Tree", 0)]
        public static DisjunctiveExamplesSpec TreeKindRef(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (var input in spec.ProvidedInputs)
            {
                var matches = spec.DisjunctiveExamples[input].Cast<MatchResult>().Cast<object>().ToList();
                treeExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        /// <summary>
        /// KindRef witness function for parameter kind.
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Variable", 1)]
        public static DisjunctiveExamplesSpec VariableKind(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return Variable.VariableKind(rule, parameter, spec);
        }

        /// <summary>
        /// KindRef witness function for parameter k.
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kindBinding">kind binding</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Variable", 2, DependsOnParameters = new[] { 1 })]
        public static DisjunctiveExamplesSpec VariableK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kindBinding)
        {
            return Variable.VariableK(rule, parameter, spec, kindBinding);
        }

        /// <summary>
        /// Parent witness function for parameter kindRef
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">parameter</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjuntive example specification</returns>
        [WitnessFunction("Parent", 1)]
        public static DisjunctiveExamplesSpec ParentKindRef(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var mats = new List<object>();
                foreach (var sot in from MatchResult matchResult in spec.DisjunctiveExamples[input] select matchResult.Match.Item1)
                {
                    var parent = sot.Parent;
                    if (sot.Value.IsToken || parent == null) return null;

                    var result = new MatchResult(Tuple.Create(parent, new Bindings(new List<SyntaxNodeOrToken> { parent.Value })));
                    mats.Add(result);
                }
                treeExamples[input] = mats.GetRange(0, 1);
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        /// <summary>
        /// Parent witness function for parameter k
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">parameter</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kindBinding">kindRef binding</param>
        /// <returns>Disjuntive example specification</returns>
        [WitnessFunction("Parent", 2, DependsOnParameters = new[] { 1 })]
        public static DisjunctiveExamplesSpec ParentK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kindBinding)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (MatchResult matchResult in spec.DisjunctiveExamples[input])
                {
                    var sot = matchResult.Match.Item1;
                    var parent = sot.Parent;

                    if (sot.Value.IsToken || parent == null) return null;

                    var children = parent.Children;

                    for (int i = 0; i < children.Count; i++)
                    {
                        var item = children.ElementAt(i);
                        if (item.Equals(sot))
                        {
                            matches.Add(i + 1);
                        }
                    }
                }

                kExamples[input] = matches;
                var value = kExamples.Values;

                if (value.Any(sequence => !sequence.SequenceEqual(value.First()))) return null;

            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }

        /// <summary>
        /// PList witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("PList", 0)]
        public static DisjunctiveExamplesSpec PListTemplate(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return GList<List<ITreeNode<SyntaxNodeOrToken>>>.List0(rule, parameter, spec);
        }

        /// <summary>
        /// PList witness function for parameter 1
        /// </summary>
        /// <param name="rule">Literal rule</param>Pa
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("PList", 1)]
        public static DisjunctiveExamplesSpec PListChildren(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return GList<List<ITreeNode<SyntaxNodeOrToken>>>.List1(rule, parameter, spec);
        }

        /// <summary>
        /// SP witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("SP", 0)]
        public static DisjunctiveExamplesSpec SpTemplate(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return GList<List<ITreeNode<SyntaxNodeOrToken>>>.Single(rule, parameter, spec);
        }

        /// <summary>
        /// CList witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("CList", 0)]
        public static DisjunctiveExamplesSpec WitnessCList1(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return GList<MatchResult>.List0(rule, parameter, spec);
        }

        /// <summary>
        /// CList witness function for parameter 1
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("CList", 1)]
        public static DisjunctiveExamplesSpec WitnessNList2(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return GList<MatchResult>.List1(rule, parameter, spec);
        }

        /// <summary>
        /// SC witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("SC", 0)]
        public static DisjunctiveExamplesSpec WitnessScChild1(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return GList<MatchResult>.Single(rule, parameter, spec);
        }

        /// <summary>
        /// NList witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("NList", 0)]
        public static DisjunctiveExamplesSpec WitnessNList1(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return GList<SyntaxNodeOrToken>.List0(rule, parameter, spec);
        }

        /// <summary>
        /// NList witness function for parameter 1
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("NList", 1)]
        public static DisjunctiveExamplesSpec WitnessCList2(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return GList<SyntaxNodeOrToken>.List1(rule, parameter, spec);
        }

        /// <summary>
        /// SN witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("SN", 0)]
        public static DisjunctiveExamplesSpec WitnessCnChild1(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return GList<SyntaxNodeOrToken>.Single(rule, parameter, spec);
        }

        /// <summary>
        /// NList witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("EList", 0)]
        public static DisjunctiveExamplesSpec WitnessEList1(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return GList<EditOperation<SyntaxNodeOrToken>>.List0(rule, parameter, spec);
        }

        /// <summary>
        /// NList witness function for parameter 1
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("EList", 1)]
        public static DisjunctiveExamplesSpec WitnessEList2(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return GList<EditOperation<SyntaxNodeOrToken>>.List1(rule, parameter, spec);
        }

        /// <summary>
        /// SN witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("SE", 0)]
        public static DisjunctiveExamplesSpec WitnessSeChild1(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return GList<EditOperation<SyntaxNodeOrToken>>.Single(rule, parameter, spec);
        }

        ///// <summary>
        ///// Matches concrete
        ///// </summary>
        ///// <param name="inpTree">Input tree</param>
        ///// <param name="matchResult">Match on code</param>
        ///// <returns>Matched nodes</returns>
        //private static List<ITreeNode<SyntaxNodeOrToken>> ConcreteTreeMatches(ITreeNode<SyntaxNodeOrToken> inpTree, MatchResult matchResult)
        //{
        //    var descendants = inpTree.DescendantNodes();
        //    var sot = matchResult.Match.Item1;
        //    var matches = from item in descendants
        //                  where item.Value.IsKind(sot.Value.Kind()) && item.ToString().Equals(sot.ToString())
        //                  select item;
        //    return matches.ToList();
        //}

        /// <summary>
        /// C witness function for kind parameter with one child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="parameter">Parameter associated to C rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive examples specification with the kind of the nodes in the examples</returns>
        [WitnessFunction("P", 0)]
        public static DisjunctiveExamplesSpec WitnessPKd(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kdExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var syntaxKinds = new List<object>();
                foreach (List<ITreeNode<SyntaxNodeOrToken>> mt in spec.DisjunctiveExamples[input])
                {
                    syntaxKinds.Add(mt.First().Value.Kind());
                }
                kdExamples[input] = syntaxKinds.GetRange(0, 1);
            }
            return DisjunctiveExamplesSpec.From(kdExamples);
        }

        /// <summary>
        /// C witness functino for expression parameter with one child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kind">Learned kind</param>
        /// <returns>Disjuntive examples specification</returns>
        [WitnessFunction("P", 1, DependsOnParameters = new[] { 0 })]
        public static DisjunctiveExamplesSpec WitnessPExpression1(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();

                foreach (List<ITreeNode<SyntaxNodeOrToken>> matchResultList in spec.DisjunctiveExamples[input])
                {
                    var firstExample = matchResultList.First();

                    var children = firstExample.Children.Select(v => new List<ITreeNode<SyntaxNodeOrToken>>()).ToList();

                    foreach (var matchResult in matchResultList)
                    {
                        var sot = matchResult;
                        if (sot.Value.IsToken || !sot.Children.Any()) return null;

                        for (int i = 0; i < sot.Children.Count; i++)
                        {
                            var item = sot.Children[i];
                            var m = item;
                            children.ElementAt(i).Add(m);
                        }
                    }

                    matches.Add(children);
                }

                eExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(eExamples);
        }

        /// <summary>
        /// C witness function for kind parameter with one child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="parameter">Parameter associated to C rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive examples specification with the kind of the nodes in the examples</returns>
        [WitnessFunction("C", 1)]
        public static DisjunctiveExamplesSpec WitnessC1Kd(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kdExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var syntaxKinds = new List<object>();
                foreach (MatchResult mt in spec.DisjunctiveExamples[input])
                {
                    var sot = mt.Match.Item1;
                    if (sot.Value.IsToken) return null;

                    syntaxKinds.Add(sot.Value.Kind());
                }
                kdExamples[input] = syntaxKinds;
            }
            return DisjunctiveExamplesSpec.From(kdExamples);
        }

        /// <summary>
        /// C witness functino for expression parameter with one child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kind">Learned kind</param>
        /// <returns>Disjuntive examples specification</returns>
        [WitnessFunction("C", 2, DependsOnParameters = new[] { 1 })]
        public static DisjunctiveExamplesSpec WitnessC1Expression1(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (MatchResult matchResult in spec.DisjunctiveExamples[input])
                {
                    var sot = matchResult.Match.Item1;
                    if (sot.Value.IsToken) return null;

                    var key = input[rule.Body[0]];
                    var currentTree = GetCurrentTree(key);
                    var snode = TreeUpdate.FindNode(currentTree, sot.Value);
                    if (snode == null || !snode.Children.Any()) return null;

                    var lsot = ExtractChildren(snode);

                    var childList = new List<MatchResult>();
                    foreach (var item in lsot)
                    {
                        var binding = matchResult.Match.Item2;
                        binding.bindings.Add(item.Value);

                        MatchResult m = new MatchResult(Tuple.Create(item, binding));
                        childList.Add(m);
                    }

                    matches.Add(childList);
                }
                eExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(eExamples);
        }

        /// <summary>
        /// Extract relevant child
        /// </summary>
        /// <param name="parent">Parent</param>
        /// <returns>Relevant child</returns>
        private static List<ITreeNode<SyntaxNodeOrToken>> ExtractChildren(ITreeNode<SyntaxNodeOrToken> parent)
        {
            var lsot = new List<ITreeNode<SyntaxNodeOrToken>>();
            foreach (var item in parent.Children)
            {
                ITreeNode<SyntaxNodeOrToken> st = item;
                if (st.Value.IsNode)
                {
                    lsot.Add(st);
                }
                else if (st.Value.IsToken && st.Value.IsKind(SyntaxKind.IdentifierToken))
                {
                    lsot.Add(st);
                }
            }
            return lsot;
        }

        /// <summary>
        /// Extract relevant child
        /// </summary>
        /// <param name="parent">Parent</param>
        /// <returns>Relevant child</returns>
        private static List<SyntaxNodeOrToken> ExtractChildren(SyntaxNodeOrToken parent)
        {
            List<SyntaxNodeOrToken> lsot = new List<SyntaxNodeOrToken>();
            foreach (var child in parent.ChildNodesAndTokens())
            {
                if (child.IsNode)
                {
                    lsot.Add(child);
                }
                else if (child.IsToken && child.IsKind(SyntaxKind.IdentifierToken))
                {
                    lsot.Add(child);
                }
            }
            return lsot;
        }

        /// <summary>
        /// Witness function for script rule
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Rule parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Script", 1)]
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

        [WitnessFunction("ManyTrans", 1)]
        public static SubsequenceSpec WitnessFunctionManyTransLoop(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            var scrips = new List<List<EditOperation<SyntaxNodeOrToken>>>();

            _treeUpdateDictionary = new Dictionary<object, TreeUpdate>();
            _currentTrees = new Dictionary<object, ITreeNode<SyntaxNodeOrToken>>();

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
                    }

                    //var regions = FindRegion(ccs, inpTree);

                    //var tree = new TreeNode<SyntaxNodeOrToken>(SyntaxFactory.EmptyStatement(), new TLabel(SyntaxKind.EmptyStatement));

                    //for (int i = 0; i < regions.Count; i++)
                    //{
                    //    var r = regions[i];
                    //    tree.AddChild(r, i);
                    //}


                    kMatches.Add(cscripts);
                }
                kExamples[input] = kMatches;
            }

            var subsequenceSpec = new SubsequenceSpec(kExamples);
            return subsequenceSpec;
        }

        private static List<ITreeNode<SyntaxNodeOrToken>> FindRegion(List<List<EditOperation<SyntaxNodeOrToken>>> ccs, SyntaxNodeOrToken inpTree)
        {
            var l = new List<ITreeNode<SyntaxNodeOrToken>>();
            foreach (var cc in ccs)
            {
                var template = BuildTemplate(cc, inpTree);
                l.Add(template.First());
            }
            return l;
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

        [WitnessFunction("Loop", 1)]
        public static SubsequenceSpec WitnessFunctionLoop(GrammarRule rule, int parameter, SubsequenceSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = spec.Examples[input].Cast<List<EditOperation<SyntaxNodeOrToken>>>().Cast<object>().ToList();

                kExamples[input] = kMatches;
            }
            return new SubsequenceSpec(kExamples);
        }

        [WitnessFunction("Template", 1)]
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

                    for (int i = 0; i < regions.Count; i++)
                    {
                        var r = ConverterHelper.ConvertCSharpToTreeNode(regions[i].Value);
                        tree.AddChild(r, i);
                    }

                    ocurrences.Add(tree);
                    TreeUpdate treeUp = new TreeUpdate(tree);
                    _treeUpdateDictionary.Add(cc, treeUp);
                    _currentTrees[cc] = tree;
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
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction("Delete", 1)]
        public static DisjunctiveExamplesSpec WitnessFunctionDeleteFrom(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (EditOperation<SyntaxNodeOrToken> editOperation in spec.DisjunctiveExamples[input])
                {
                    if (!(editOperation is Delete<SyntaxNodeOrToken>)) return null;

                    var from = editOperation.T1Node;
                    var result = new MatchResult(Tuple.Create(from, new Bindings(new List<SyntaxNodeOrToken> { from.Value })));
                    matches.Add(result);

                    var key = input[rule.Body[0]];
                    var treeUp = _treeUpdateDictionary[key];
                    var previousTree = ConverterHelper.MakeACopy(treeUp.CurrentTree);
                    treeUp.ProcessEditOperation(editOperation);
                    _currentTrees[key] = previousTree;

                    Console.WriteLine("PREVIOUS TREE\n\n");
                    PrintUtil<SyntaxNodeOrToken>.PrintPretty(previousTree, "", true);
                    Console.WriteLine("UPDATED TREE\n\n");
                    PrintUtil<SyntaxNodeOrToken>.PrintPretty(treeUp.CurrentTree, "", true);
                }

                kExamples[input] = matches;
            }

            return DisjunctiveExamplesSpec.From(kExamples);
        }

        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction("Update", 1)]
        public static DisjunctiveExamplesSpec WitnessFunctionUpdateFrom(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (EditOperation<SyntaxNodeOrToken> editOperation in spec.DisjunctiveExamples[input])
                {
                    if (!(editOperation is Update<SyntaxNodeOrToken>)) return null;

                    var from = editOperation.T1Node;
                    var result = new MatchResult(Tuple.Create(from, new Bindings(new List<SyntaxNodeOrToken> { from.Value })));
                    matches.Add(result);

                    var key = input[rule.Body[0]];
                    var treeUp = _treeUpdateDictionary[key];

                    var previousTree = ConverterHelper.MakeACopy(treeUp.CurrentTree);
                    treeUp.ProcessEditOperation(editOperation);
                    _currentTrees[key] = previousTree;
                }

                kExamples[input] = matches;
            }

            return DisjunctiveExamplesSpec.From(kExamples);
        }

        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <param name="fromBinding">Learned examples for k</param>
        /// <returns></returns>
        [WitnessFunction("Update", 2, DependsOnParameters = new[] { 1 })]
        public static DisjunctiveExamplesSpec WitnessFunctionUpdateTo(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec fromBinding)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (EditOperation<SyntaxNodeOrToken> editOperation in spec.DisjunctiveExamples[input])
                {
                    if (!(editOperation is Update<SyntaxNodeOrToken>)) return null;

                    var update = (Update<SyntaxNodeOrToken>)editOperation;
                    matches.Add(update.To);
                }
                kExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }


        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction("Move", 1)]
        public static DisjunctiveExamplesSpec WitnessFunctionMoveK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (EditOperation<SyntaxNodeOrToken> editOperation in spec.DisjunctiveExamples[input])
                {
                    if (!(editOperation is Move<SyntaxNodeOrToken>)) return null;

                    var key = input[rule.Body[0]];
                    var treeUp = _treeUpdateDictionary[key];

                    var move = (Move<SyntaxNodeOrToken>)editOperation;
                    var parent = move.Parent;
                    var result = new MatchResult(Tuple.Create(parent, new Bindings(new List<SyntaxNodeOrToken> { parent.Value })));

                    matches.Add(result);

                    var previousTree = ConverterHelper.MakeACopy(treeUp.CurrentTree);
                    treeUp.ProcessEditOperation(editOperation);
                    _currentTrees[key] = previousTree;
                }

                kExamples[input] = matches;
            }

            return DisjunctiveExamplesSpec.From(kExamples);
        }

        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction("Move", 2, DependsOnParameters = new[] { 1 })]
        public static DisjunctiveExamplesSpec WitnessFunctionMoveFrom(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (EditOperation<SyntaxNodeOrToken> editOperation in spec.DisjunctiveExamples[input])
                {
                    if (!(editOperation is Move<SyntaxNodeOrToken>)) return null;

                    var from = editOperation.T1Node;
                    var result = new MatchResult(Tuple.Create(from, new Bindings(new List<SyntaxNodeOrToken> { from.Value })));
                    matches.Add(result);
                }

                kExamples[input] = matches;
            }

            return DisjunctiveExamplesSpec.From(kExamples);
        }

        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <param name="kBinding">Learned examples for k</param>
        /// <param name="parentBinding">Learned examples for parent</param>
        /// <returns></returns>
        [WitnessFunction("Move", 3, DependsOnParameters = new[] { 1, 2 })]
        public static DisjunctiveExamplesSpec WitnessFunctionMoveTo(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kBinding, ExampleSpec parentBinding)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (EditOperation<SyntaxNodeOrToken> editOperation in spec.DisjunctiveExamples[input])
                {
                    if (!(editOperation is Move<SyntaxNodeOrToken>)) return null;

                    matches.Add(editOperation.K);
                }
                kExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }

        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction("Insert", 1)]
        public static DisjunctiveExamplesSpec WitnessFunctionInsertK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (EditOperation<SyntaxNodeOrToken> editOperation in spec.DisjunctiveExamples[input])
                {
                    if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;

                    var key = input[rule.Body[0]];
                    var treeUp = _treeUpdateDictionary[key];

                    var parent = editOperation.Parent;
                    var result = new MatchResult(Tuple.Create(parent, new Bindings(new List<SyntaxNodeOrToken> { parent.Value })));
                    matches.Add(result);

                    var previousTree = ConverterHelper.MakeACopy(treeUp.CurrentTree);
                    treeUp.ProcessEditOperation(editOperation);
                    _currentTrees[key] = previousTree;
                    Console.WriteLine("PREVIOUS TREE\n\n");
                    PrintUtil<SyntaxNodeOrToken>.PrintPretty(previousTree, "", true);
                    Console.WriteLine("UPDATED TREE\n\n");
                    PrintUtil<SyntaxNodeOrToken>.PrintPretty(treeUp.CurrentTree, "", true);
                }

                kExamples[input] = matches;
            }

            return DisjunctiveExamplesSpec.From(kExamples);
        }

        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <param name="kBinding">kBinding</param>
        /// <returns></returns>
        [WitnessFunction("Insert", 2, DependsOnParameters = new[] { 1 })]
        public static DisjunctiveExamplesSpec WitnessFunctionInsertParent(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kBinding)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (EditOperation<SyntaxNodeOrToken> editOperation in spec.DisjunctiveExamples[input])
                {
                    if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;

                    editOperation.T1Node.Children = new List<ITreeNode<SyntaxNodeOrToken>>();
                    matches.Add(editOperation.T1Node);
                }

                kExamples[input] = matches;
            }

            return DisjunctiveExamplesSpec.From(kExamples);
        }

        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <param name="kBinding">Learned examples for k</param>
        /// <param name="parentBinding">Learned examples for parent</param>
        /// <returns></returns>
        [WitnessFunction("Insert", 3, DependsOnParameters = new[] { 1, 2 })]
        public static DisjunctiveExamplesSpec WitnessFunctionInsertAST(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kBinding, ExampleSpec parentBinding)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (EditOperation<SyntaxNodeOrToken> editOperation in spec.DisjunctiveExamples[input])
                {
                    if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;

                    matches.Add(editOperation.K);
                }
                kExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }

        /// <summary>
        /// Node witness function for expression parameter with one child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjuntive examples specification</returns>
        [WitnessFunction("Node", 0)]
        public static DisjunctiveExamplesSpec WitnessFunctionNode1Kd(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<object>();
                foreach (ITreeNode<SyntaxNodeOrToken> sot in spec.DisjunctiveExamples[input])
                {
                    if (sot.Value.IsToken) return null;
                    if (!sot.Children.Any()) return null;

                    kMatches.Add(sot.Value.Kind());
                }
                kExamples[input] = kMatches;
            }

            return DisjunctiveExamplesSpec.From(kExamples);
        }

        /// <summary>
        /// C witness function for expression parameter with one child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kind">Learned kind</param>
        /// <returns>Disjuntive examples specification</returns>
        [WitnessFunction("Node", 1, DependsOnParameters = new[] { 0 })]
        public static DisjunctiveExamplesSpec NodeChildren(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();

                foreach (SyntaxNodeOrToken sot in spec.DisjunctiveExamples[input])
                {
                    if (sot.IsToken) return null;

                    var lsot = ExtractChildren(sot);

                    matches.Add(lsot);
                }
                eExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(eExamples);
        }

        /// <summary>
        /// Learn a constant node
        /// </summary>
        /// <param name="rule">Rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Const", 0)]
        public static DisjunctiveExamplesSpec Const(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();

            var mats = new List<object>();
            foreach (State input in spec.ProvidedInputs)
            {
                foreach (ITreeNode<SyntaxNodeOrToken> sot in spec.DisjunctiveExamples[input])
                {
                    if (sot.Children.Any()) return null;
                    mats.Add(sot.Value);
                }

                treeExamples[input] = mats.GetRange(0, 1); //We do not need to pass more than a constant.
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        [WitnessFunction("Ref", 1)]
        public static DisjunctiveExamplesSpec Ref(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var key = input[rule.Body[0]];
                var inpTree = GetCurrentTree(key);
                var mats = new List<object>();
                foreach (ITreeNode<SyntaxNodeOrToken> sot in spec.DisjunctiveExamples[input])
                {
                    if (!sot.Value.IsNode) return null;

                    var subTree = ConverterHelper.ConvertCSharpToTreeNode(sot.Value);
                    var node = IsomorphicManager<SyntaxNodeOrToken>.FindIsomorphicSubTree(inpTree, subTree);

                    if (node == null) return null;

                    var result = new MatchResult(Tuple.Create(sot, new Bindings(new List<SyntaxNodeOrToken> { sot.Value })));

                    mats.Add(result);
                }

                treeExamples[input] = mats;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        [WitnessFunction("NodesMap", 1)]
        public static SubsequenceSpec NodesMap(GrammarRule rule, int parameter,
                                                 SubsequenceSpec spec)
        {
            var linesExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var nodePrefix = spec.Examples[input].Cast<SyntaxNodeOrToken>();
                var tuple = (SyntaxNodeOrToken)input.Bindings.First().Value;

                var inpMapping = GetPair(tuple, nodePrefix);

                var linesContainingSelection = inpMapping;

                linesExamples[input] = linesContainingSelection;
            }
            return new SubsequenceSpec(linesExamples);
        }

        /// <summary>
        /// Get the previous version of the transformed node on the input
        /// </summary>
        /// <param name="input">The source code before the transformation</param>
        /// <param name="nodePrefix">Transformed code fragments</param>
        /// <returns>Return the previous version of the transformed node on the input</returns>
        private static List<object> GetPair(SyntaxNodeOrToken input, IEnumerable<SyntaxNodeOrToken> nodePrefix)
        {
            return nodePrefix.Select(item => GetPair(input, item)).Cast<object>().ToList();
        }

        /// <summary>
        /// Get the corresponding pair of outTree in input tree
        /// </summary>
        /// <param name="inputTree">Input tree</param>
        /// <param name="outTree">output subTree</param>
        /// <returns>Corresponding pair</returns>
        private static SyntaxNodeOrToken GetPair(SyntaxNodeOrToken inputTree, SyntaxNodeOrToken outTree)
        {
            SyntaxNode node = inputTree.AsNode();

            var l = from nm in node.DescendantNodes()
                    where nm.IsKind(outTree.Kind())
                    select nm;

            MethodDeclarationSyntax m = (MethodDeclarationSyntax)outTree;
            return l.Cast<MethodDeclarationSyntax>().FirstOrDefault(mItem => m.Identifier.ToString().Equals(mItem.Identifier.ToString()));
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

        public static ITreeNode<SyntaxNodeOrToken> GetCurrentTree(object n)
        {
            var node = _currentTrees[n];
            return node;
        }

        private static void PrintScript(List<EditOperation<SyntaxNodeOrToken>> script)
        {
            string s = script.Aggregate("", (current, v) => current + (v + "\n"));
        }
    }
}