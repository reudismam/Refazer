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
using TreeEdit.Spg.TreeEdit.Mapping;
using TreeEdit.Spg.TreeEdit.Script;
using TreeEdit.Spg.TreeEdit.Update;

namespace ProseSample.Substrings
{
    public static class WitnessFunctions
    {
        private static readonly Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> Edits = new Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken>();

        private static readonly Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> CurrentTrees = new Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken>();
        
        /// <summary>
        /// Literal witness function for parameter tree.
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Literal", 1)]
        public static DisjunctiveExamplesSpec WitnessTree(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var inpTree = CurrentTrees[(SyntaxNodeOrToken)input[rule.Body[0]]];
                var literalExamples = new List<object>();
                foreach (MatchResult matchResult in spec.DisjunctiveExamples[input])
                {
                    //compute the occurrences of matchResult on the input tree.
                    var matchedSyntaxNodeOrTokens = Matches(inpTree, matchResult);
                    var lobject = matchedSyntaxNodeOrTokens.Select(o => (object) o);

                    literalExamples.AddRange(lobject);
                }
                treeExamples[input] = literalExamples;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        /// <summary>
        /// Literal witness function for parameter tree.
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("CList", 0)]
        public static DisjunctiveExamplesSpec WitnessCList1(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (List<MatchResult> matchResult in spec.DisjunctiveExamples[input])
                {
                    if (!matchResult.Any()) return null;
                    if (matchResult.Count == 1) return null;

                    matches.Add(matchResult.First());
                }
                treeExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        /// <summary>
        /// Literal witness function for parameter tree.
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("CList", 1)]
        public static DisjunctiveExamplesSpec WitnessCList2(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (List<MatchResult> matchResult in spec.DisjunctiveExamples[input])
                {
                    if (!matchResult.Any()) return null;
                    if (matchResult.Count == 1) return null;

                    matchResult.RemoveAt(0);
                    matches.Add(matchResult);
                }
                treeExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        /// <summary>
        /// Literal witness function for parameter tree.
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("CS", 0)]
        public static DisjunctiveExamplesSpec WitnessCSChild1(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (List<MatchResult> matchResult in spec.DisjunctiveExamples[input])
                {
                    if (!matchResult.Any()) return null;
                    if (matchResult.Count != 1) return null;

                    matches.Add(matchResult.First());
                }
                treeExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        /// <summary>
        /// Literal witness function for parameter tree.
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Abstract", 1)]
        public static DisjunctiveExamplesSpec WitnessAbstractKind(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var mats = new List<object>();
                var inpTree = CurrentTrees[(SyntaxNodeOrToken)input[rule.Body[0]]];
                foreach (MatchResult matchResult in spec.DisjunctiveExamples[input])
                {
                    SyntaxNodeOrToken sot = matchResult.match.Item1;
                    var matches = MatchesAbstract(inpTree, sot.Kind());
              
                    foreach (var item in matches)
                    {
                        if (item.ToString().Equals(sot.ToString()))
                        {
                            mats.Add(item.Kind());
                            if (!mats.First().Equals(item.Kind())) return null;
                        }
                    }
                }
                treeExamples[input] = mats.GetRange(0, 1);

            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        /// <summary>
        /// Literal witness function for parameter tree.
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Abstract", 2, DependsOnParameters = new[] { 1 })]
        public static DisjunctiveExamplesSpec WitnessAbstractK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kindBinding)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var mats = new List<object>();
                var inpTree = CurrentTrees[(SyntaxNodeOrToken)input[rule.Body[0]]];
                foreach (MatchResult matchResult in spec.DisjunctiveExamples[input])
                {
                    SyntaxNodeOrToken sot = matchResult.match.Item1;

                    var kind = (SyntaxKind) kindBinding.Examples[input];
                    var matches = MatchesAbstract(inpTree, kind);

                    for (int i = 0; i < matches.Count; i++)
                    {
                        var item = matches[i];
                        if (item.ToString().Equals(sot.ToString()))
                        {
                            mats.Add(i + 1);
                        }
                    }          
                }
                treeExamples[input] = mats;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        /// <summary>
        /// Matches concrete
        /// </summary>
        /// <param name="inpTree">Input tree</param>
        /// <param name="matchResult">Match on code</param>
        /// <returns>Matched nodes</returns>
        private static List<SyntaxNodeOrToken> Matches(SyntaxNodeOrToken inpTree, MatchResult matchResult)
        {
            var sot = matchResult.match.Item1;
            var matches = from item in inpTree.AsNode().DescendantNodesAndTokens()
                          where item.IsKind(sot.Kind()) && item.ToString().Equals(sot.ToString())
                          select item;
            return matches.ToList();
        }

        /// <summary>
        /// Abstract match
        /// </summary>
        /// <param name="inpTree">Input tree</param>
        /// <param name="kind">Syntax node or token to be matched.</param>
        /// <returns>Abstract match</returns>
        private static List<SyntaxNodeOrToken> MatchesAbstract(SyntaxNodeOrToken inpTree, SyntaxKind kind)
        {
            var matches = from item in inpTree.AsNode().DescendantNodesAndTokens()
                          where item.IsKind(kind)
                          select item;
            return matches.ToList();
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
                    SyntaxNodeOrToken sot = mt.match.Item1;
                    if (sot.IsToken) return null;

                    syntaxKinds.Add(sot.Kind());
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
            return ConcatenationBase(spec);
        }

        /// <summary>
        /// Concatenation learner base
        /// </summary>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        private static DisjunctiveExamplesSpec ConcatenationBase(DisjunctiveExamplesSpec spec)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (MatchResult matchResult in spec.DisjunctiveExamples[input])
                {
                    var sot = matchResult.match.Item1;
                    if (sot.IsToken) return null;

                    var lsot = ExtractChildren(sot);

                    var childList = new List<MatchResult>();
                    foreach (var item in lsot)
                    {
                        var binding = matchResult.match.Item2;
                        binding.bindings.Add(item);
                        Tuple<SyntaxNodeOrToken, Bindings> t = Tuple.Create(item, binding);
                        MatchResult m = new MatchResult(t);
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
        private static List<SyntaxNodeOrToken> ExtractChildren(SyntaxNodeOrToken parent)
        {
            List<SyntaxNodeOrToken> lsot = new List<SyntaxNodeOrToken>();
            foreach (var item in parent.ChildNodesAndTokens())
            {
                SyntaxNodeOrToken st = item;
                if (st.IsNode)
                {
                    lsot.Add(st);
                }
                else if (st.IsToken && st.IsKind(SyntaxKind.IdentifierToken))
                {
                    lsot.Add(st);
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
        [WitnessFunction("Script1", 1)]
        public static DisjunctiveExamplesSpec WitnessFunctionScript1Edit(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<object>();
                var inpTree = (SyntaxNodeOrToken)input[rule.Body[0]];
                var outTree = (SyntaxNodeOrToken)spec.Examples[input];

                Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M;
                var script = Script(inpTree, outTree, out M);

                TreeUpdate treeUp = new TreeUpdate();
                treeUp.PreProcessTree(script, inpTree, M);

                //string text = script.Aggregate("", (current, item) => current + item.ToString());
                foreach (var item in script)
                {
                    if (!treeUp.Processed.ContainsKey(item))
                    {
                        treeUp.ProcessEditOperation(item);
                        var changedNodeList = treeUp.CurrentTree.AsNode().GetAnnotatedNodes(treeUp.Ann[item]).ToList();
                        var newNode = changedNodeList.First();
                        kMatches.Add(newNode);
                    }
                }

                if (kMatches.Count != 1) return null;
               
                kExamples[input] = kMatches;
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }

        /// <summary>
        /// Witness function for script rule
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Rule parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Script2", 1)]
        public static DisjunctiveExamplesSpec WitnessFunctionScript2Edit(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<object>();
                var inpTree = (SyntaxNodeOrToken)input[rule.Body[0]];
                CurrentTrees[inpTree] = inpTree;
                foreach (SyntaxNodeOrToken outTree in spec.DisjunctiveExamples[input])
                {
                    Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M;
                    var script = Script(inpTree, outTree, out M);

                    TreeUpdate treeUp = new TreeUpdate();
                    treeUp.PreProcessTree(script, inpTree, M);

                    foreach (var item in script)
                    {
                        if (!treeUp.Processed.ContainsKey(item))
                        {
                            //var oldTree = (SyntaxNode)outTree;
                            treeUp.ProcessEditOperation(item);
                            var newTree = treeUp.CurrentTree;
                            kMatches.Add(newTree);
                        }
                    }

                    if (kMatches.Count != 2) return null;

                    Edits[inpTree] = outTree;

                    kMatches = new List<object> { inpTree.AsNode() };
                    kExamples[input] = kMatches;
                }
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }

        /// <summary>
        /// Witness function for script rule
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Rule parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Script2", 2)]
        public static DisjunctiveExamplesSpec WitnessFunctionScript2Edit2(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<object>();
                foreach (SyntaxNodeOrToken outTree in spec.DisjunctiveExamples[input])
                {
                    var key = (SyntaxNodeOrToken) input[rule.Body[0]];
                    var inpTree = CurrentTrees[key];
                    Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M;
                    var script = Script(inpTree, outTree, out M);

                    TreeUpdate treeUp = new TreeUpdate();
                    treeUp.PreProcessTree(script, inpTree, M);

                    foreach (var item in script)
                    {
                        if (!treeUp.Processed.ContainsKey(item))
                        {
                            treeUp.ProcessEditOperation(item);
                            var newNode = (SyntaxNode) treeUp.CurrentTree;
                            kMatches.Add(newNode);
                        }
                    }

                    if (kMatches.Count != 1) return null;

                    Edits[inpTree] = outTree;

                    kMatches = new List<object> { inpTree.AsNode() };
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
        /// <param name="M">out Mapping</param>
        /// <returns>Computed edit script</returns>
        private static List<EditOperation> Script(SyntaxNodeOrToken inpTree, SyntaxNodeOrToken outTree, out Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M)
        {
            ITreeMapping mapping = new GumTreeMapping();
            M = mapping.Mapping(inpTree, outTree);

            var generator = new EditScriptGenerator();
            var script = generator.EditScript(inpTree, outTree, M);
            return script;
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
                var kMatches = new List<object>();
                var matches = new List<object>();
                //var inpTree = (SyntaxNodeOrToken)input[rule.Body[0]];

                foreach (SyntaxNode inpTree in spec.DisjunctiveExamples[input])
                {
                    var outTree = Edits[inpTree];
                    Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M;
                    var script = Script(inpTree, outTree, out M);

                    TreeUpdate treeUp = new TreeUpdate();
                    treeUp.PreProcessTree(script, inpTree, M);

                    foreach (var item in script)
                    {
                        if (!treeUp.Processed.ContainsKey(item))
                        {
                            treeUp.ProcessEditOperation(item);
                            kMatches.Add(item.K);
                        }
                    }

                    //if (kMatches.Count != 1) return null; //more than an edit operation
                    matches.AddRange(kMatches.GetRange(0, 1));
                }
                kExamples[input] = matches;
            }

            if (kExamples.Values.Any(o => !o.SequenceEqual(kExamples.Values.First()))) return null; //Unable to generate programs (distinct ks).

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
                var kMatches = new List<object>();
                var matches = new List<object>();
                foreach (SyntaxNode inpTree in spec.DisjunctiveExamples[input])
                {
                    var outTree = Edits[inpTree];
                    Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M;
                    var script = Script(inpTree, outTree, out M);

                    TreeUpdate treeUp = new TreeUpdate();
                    treeUp.PreProcessTree(script, inpTree, M);

                    foreach (var item in script)
                    {
                        if (!treeUp.Processed.ContainsKey(item))
                        {
                            treeUp.ProcessEditOperation(item);
                            var parent = item.Parent;  
                            var result = new MatchResult(Tuple.Create(parent, new Bindings(new List<SyntaxNodeOrToken> { parent })));
                            kMatches.Add(result);
                        }
                    }
                    matches.AddRange(kMatches.GetRange(0, 1));
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
        // ReSharper disable once InconsistentNaming
        public static DisjunctiveExamplesSpec WitnessFunctionInsertAST(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kBinding, ExampleSpec parentBinding)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<object>();
                var matches = new List<object>();
                foreach (SyntaxNode inpTree in spec.DisjunctiveExamples[input])
                {
                    var k = (int)kBinding.Examples[input];
                    var outTree = Edits[inpTree];
                    Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M;
                    var script = Script(inpTree, outTree, out M);

                    TreeUpdate treeUp = new TreeUpdate();
                    treeUp.PreProcessTree(script, inpTree, M);

                    var current = new List<SyntaxNodeOrToken>();
                    foreach (var item in script)
                    {
                        if (!treeUp.Processed.ContainsKey(item))
                        {
                            treeUp.ProcessEditOperation(item);
                            var parent = (SyntaxNodeOrToken)treeUp.CurrentTree.AsNode().GetAnnotatedNodes(treeUp.Ann[item]).First();
                            var el = (SyntaxNodeOrToken) parent.AsNode().ChildNodes().ElementAt(k - 1);
                            kMatches.Add(el);
                            current.Add(treeUp.CurrentTree);
                        }
                    }

                    var key = (SyntaxNodeOrToken) input[rule.Body[0]];
                    CurrentTrees[key] = current.First();
                    matches.AddRange(kMatches.GetRange(0, 1));
                }
                kExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }

        #region Node
        /// <summary>
        /// C witness function for expression parameter with one child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjuntive examples specification</returns>
        [WitnessFunction("Node1", 0)]
        public static DisjunctiveExamplesSpec WitnessFunctionNode1Kd(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<object>();
                foreach (SyntaxNodeOrToken sot in spec.DisjunctiveExamples[input])
                {
                    if (sot.IsToken) return null;

                    kMatches.Add(sot.Kind());
                }

                kExamples[input] = kMatches;
            }

            return DisjunctiveExamplesSpec.From(kExamples);
        }

        [WitnessFunction("Node2", 0)]
        public static DisjunctiveExamplesSpec WitnessFunctionNode2Kd(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return WitnessFunctionNode1Kd(rule, parameter, spec);
        }

        /// <summary>
        /// C witness function for expression parameter with one child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kind">Learned kind</param>
        /// <returns>Disjuntive examples specification</returns>
        [WitnessFunction("Node1", 1, DependsOnParameters = new[] { 0 })]
        // ReSharper disable once InconsistentNaming
        public static DisjunctiveExamplesSpec WitnessNode1AST1(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            return NodeBase(spec, 1, 1);
        }

        /// <summary>
        /// C witness functino for expression parameter with two child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kind">Learned kind</param>
        /// <returns>Disjuntive examples specification</returns>
        [WitnessFunction("Node2", 1, DependsOnParameters = new[] { 0 })]
        // ReSharper disable once InconsistentNaming
        public static DisjunctiveExamplesSpec WitnessNode2AST1(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            return NodeBase(spec, 2, 1);
        }

        /// <summary>
        /// C witness function for expression parameter with two child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kind">Learned kind</param>
        /// <param name="ast1">Learned examples for ast1</param>
        /// <returns>Disjuntive examples specification</returns>
        [WitnessFunction("Node2", 2, DependsOnParameters = new[] { 0, 1 })]
        // ReSharper disable once InconsistentNaming
        public static DisjunctiveExamplesSpec WitnessNode2AST2(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind, ExampleSpec ast1)
        {
            return NodeBase(spec, 2, 2);
        }


        /// <summary>
        /// C witness functino for expression parameter with two child
        /// </summary>
        /// <param name="spec">Example specification</param>
        /// <param name="desiredIndex">Max children number</param>
        /// <param name="returnIndex">ChildNumber learned in this expression</param>
        /// <returns>Disjuntive examples specification</returns>
        private static DisjunctiveExamplesSpec NodeBase(DisjunctiveExamplesSpec spec, int desiredIndex, int returnIndex)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();

                foreach (SyntaxNodeOrToken sot in spec.DisjunctiveExamples[input])
                {
                    if (sot.IsToken) return null;

                    List<SyntaxNodeOrToken> lsot = new List<SyntaxNodeOrToken>();
                    foreach (var item in sot.AsNode().ChildNodesAndTokens())
                    {
                        SyntaxNodeOrToken st = item;
                        if (st.IsNode)
                        {
                            lsot.Add(st);
                        }
                        else if (st.IsToken && st.IsKind(SyntaxKind.IdentifierToken))
                        {
                            lsot.Add(st);
                        }
                    }

                    if (lsot.Count != desiredIndex) return null;

                    matches.Add(lsot[returnIndex - 1]);
                }
                eExamples[input] = matches;
            }

            return DisjunctiveExamplesSpec.From(eExamples);
        }

        #endregion

        #region const
        /// <summary>
        /// Learn a constant node
        /// </summary>
        /// <param name="rule">Rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Const", 0)]
        public static DisjunctiveExamplesSpec WitnessFunctionConst(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();

            var mats = new List<object>();
            foreach (State input in spec.ProvidedInputs)
            {
                foreach (SyntaxNodeOrToken sot in spec.DisjunctiveExamples[input])
                {
                    mats.Add(sot);
                }

                treeExamples[input] = mats;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        #endregion

        [WitnessFunction("NodesMap", 1)]
        public static PrefixSpec WitnessNodesMap(GrammarRule rule, int parameter,
                                                 PrefixSpec spec)
        {
            var linesExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                //Todo refactor
                var nodePrefix = spec.Examples[input].Cast<SyntaxNodeOrToken>();
                var tuple = (SyntaxNodeOrToken)input.Bindings.First().Value;

                var inpMapping = GetPair(tuple, nodePrefix);

                var linesContainingSelection = inpMapping;

                linesExamples[input] = linesContainingSelection;
            }
            return new PrefixSpec(linesExamples);
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
    }
}


//[WitnessFunction("SubStr", 1)]
//public static DisjunctiveExamplesSpec WitnessPositionPair(GrammarRule rule, int parameter,
//                                                          ExampleSpec spec)
//{
//    var ppExamples = new Dictionary<State, IEnumerable<object>>();
//    foreach (State input in spec.ProvidedInputs)
//    {
//        var v = (StringRegion) input[rule.Body[0]];
//        var desiredOutput = (StringRegion) spec.Examples[input];
//        var occurrences = new List<object>();
//        for (int i = v.Value.IndexOf(desiredOutput.Value, StringComparison.Ordinal);
//             i >= 0;
//             i = v.Value.IndexOf(desiredOutput.Value, i + 1, StringComparison.Ordinal))
//        {
//            occurrences.Add(Tuple.Create(v.Start + (uint?) i, v.Start + (uint?) i + desiredOutput.Length));
//        }
//        ppExamples[input] = occurrences;
//    }
//    return DisjunctiveExamplesSpec.From(ppExamples);
//}

//[WitnessFunction("AbsPos", 1)]
//public static DisjunctiveExamplesSpec WitnessK(GrammarRule rule, int parameter,
//                                               DisjunctiveExamplesSpec spec)
//{
//    var kExamples = new Dictionary<State, IEnumerable<object>>();
//    foreach (State input in spec.ProvidedInputs)
//    {
//        var v = (StringRegion) input[rule.Body[0]];
//        var positions = new List<object>();
//        foreach (uint pos in spec.DisjunctiveExamples[input])
//        {
//            positions.Add((int) pos + 1 - (int) v.Start);
//            positions.Add((int) pos - (int) v.End - 1);
//        }
//        kExamples[input] = positions;
//    }
//    return DisjunctiveExamplesSpec.From(kExamples);
//}

//[WitnessFunction("RegPos", 1)]
//public static DisjunctiveExamplesSpec WitnessRegexPair(GrammarRule rule, int parameter,
//                                                       DisjunctiveExamplesSpec spec)
//{
//    var rrExamples = new Dictionary<State, IEnumerable<object>>();
//    foreach (State input in spec.ProvidedInputs)
//    {
//        var v = (StringRegion) input[rule.Body[0]];
//        var regexes = new List<Tuple<RegularExpression, RegularExpression>>();
//        foreach (uint pos in spec.DisjunctiveExamples[input])
//        {
//            Dictionary<Token, TokenMatch> rightMatches;
//            if (!v.Cache.TryGetAllMatchesStartingAt(pos, out rightMatches)) continue;
//            Dictionary<Token, TokenMatch> leftMatches;
//            if (!v.Cache.TryGetAllMatchesEndingAt(pos, out leftMatches)) continue;
//            var leftRegexes = leftMatches.Keys.Select(RegularExpression.Create).Append(Epsilon);
//            var rightRegexes = rightMatches.Keys.Select(RegularExpression.Create).Append(Epsilon);
//            var regexPairs = from l in leftRegexes from r in rightRegexes select Tuple.Create(l, r);
//            regexes.AddRange(regexPairs);
//        }
//        rrExamples[input] = regexes;
//    }
//    return DisjunctiveExamplesSpec.From(rrExamples);
//}

//[WitnessFunction("RegPos", 2, DependsOnParameters = new[] { 1 })]
//public static DisjunctiveExamplesSpec WitnessRegexCount(GrammarRule rule, int parameter,
//                                                        DisjunctiveExamplesSpec spec,
//                                                        ExampleSpec regexBinding)
//{
//    var kExamples = new Dictionary<State, IEnumerable<object>>();
//    foreach (State input in spec.ProvidedInputs)
//    {
//        var v = (StringRegion)input[rule.Body[0]];
//        var rr = (Tuple<RegularExpression, RegularExpression>)regexBinding.Examples[input];
//        var ks = new List<object>();
//        foreach (uint pos in spec.DisjunctiveExamples[input])
//        {
//            var ms = rr.Item1.Run(v).Where(m => rr.Item2.MatchesAt(v, m.Right)).ToArray();
//            int index = ms.BinarySearchBy(m => m.Right.CompareTo(pos));
//            if (index < 0) return null;
//            ks.Add(index + 1);
//            ks.Add(index - ms.Length);
//        }
//        kExamples[input] = ks;
//    }
//    return DisjunctiveExamplesSpec.From(kExamples);
//}