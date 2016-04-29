using System;
using System.Collections.Generic;
using System.Linq;
using LongestCommonAncestor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using ProseSample.Substrings.List;
using Spg.TreeEdit.Node;
using TreeEdit.Spg.TreeEdit.Mapping;
using TreeEdit.Spg.TreeEdit.Script;
using TreeEdit.Spg.TreeEdit.Update;

namespace ProseSample.Substrings
{
    public static class WitnessFunctions
    { 
        private static readonly Dictionary<SyntaxNodeOrToken, ITreeNode<SyntaxNodeOrToken>> CurrentTrees = new Dictionary<SyntaxNodeOrToken, ITreeNode<SyntaxNodeOrToken>>();

        private static readonly Dictionary<State, TreeUpdate> TreeUpdateDictionary = new Dictionary<State, TreeUpdate>(); 

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
                var inpTree = GetCurrentTree((SyntaxNodeOrToken)input[rule.Body[0]]);
                var literalExamples = new List<object>();
                foreach (MatchResult matchResult in spec.DisjunctiveExamples[input])
                {
                    //compute the occurrences of matchResult on the input tree.
                    var matchedSyntaxNodeOrTokens = Matches(inpTree, matchResult);
                    var lobject = matchedSyntaxNodeOrTokens.Select(o => (object) o);

                    if (!lobject.Any()) return null;

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
        [WitnessFunction("Abstract", 1)]
        public static DisjunctiveExamplesSpec WitnessAbstractKind(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var mats = new List<object>();
                var inpTree = GetCurrentTree((SyntaxNodeOrToken)input[rule.Body[0]]);
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

                    if (!mats.Any()) return null;
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
        /// <param name="kindBinding">kind binding</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Abstract", 2, DependsOnParameters = new[] { 1 })]
        public static DisjunctiveExamplesSpec WitnessAbstractK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kindBinding)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var mats = new List<object>();
                var inpTree = GetCurrentTree((SyntaxNodeOrToken)input[rule.Body[0]]);
                foreach (MatchResult matchResult in spec.DisjunctiveExamples[input])
                {
                    SyntaxNodeOrToken sot = matchResult.match.Item1;

                    var kind = (SyntaxKind)kindBinding.Examples[input];
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

        /// <summary>
        /// Matches concrete
        /// </summary>
        /// <param name="inpTree">Input tree</param>
        /// <param name="matchResult">Match on code</param>
        /// <returns>Matched nodes</returns>
        private static List<SyntaxNodeOrToken> Matches(ITreeNode<SyntaxNodeOrToken> inpTree, MatchResult matchResult)
        {
            var descendants = inpTree.DescendantNodes();
            var sot = matchResult.match.Item1;
            var matches = from item in descendants
                          where item.Value.IsKind(sot.Kind()) && item.ToString().Equals(sot.ToString())
                          select item.Value;
            return matches.ToList();
        }

        /// <summary>
        /// Abstract match
        /// </summary>
        /// <param name="inpTree">Input tree</param>
        /// <param name="kind">Syntax node or token to be matched.</param>
        /// <returns>Abstract match</returns>
        private static List<SyntaxNodeOrToken> MatchesAbstract(ITreeNode<SyntaxNodeOrToken> inpTree, SyntaxKind kind)
        {
            var matches = from item in inpTree.DescendantNodes()
                          where item.Value.IsKind(kind)
                          select item.Value;
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
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (MatchResult matchResult in spec.DisjunctiveExamples[input])
                {
                    var sot = matchResult.match.Item1;
                    if (sot.IsToken) return null;

                    var lsot = ExtractChildren(sot);

                    var currentTree = GetCurrentTree((SyntaxNodeOrToken)input[rule.Body[0]]);
                    if (TreeUpdate.FindNode(currentTree, sot) == null) return null;

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
        [WitnessFunction("Script", 1)]
        public static DisjunctiveExamplesSpec WitnessFunctionScript2Edit(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            var scrips = new List<List<EditOperation<SyntaxNodeOrToken>>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<object>();
                var inpTree = (SyntaxNodeOrToken)input[rule.Body[0]];
                foreach (SyntaxNodeOrToken outTree in spec.DisjunctiveExamples[input])
                {
                    Dictionary<ITreeNode<SyntaxNodeOrToken>, ITreeNode<SyntaxNodeOrToken>> m;
                    var script = Script(inpTree, outTree, out m);
                    scrips.Add(script);

                    TreeUpdate treeUp = new TreeUpdate();
                    treeUp.PreProcessTree(script, inpTree);

                    TreeUpdateDictionary.Add(input, treeUp);    

                    kMatches.Add(script);    
                }
                kExamples[input] = kMatches;
            }
            var lcs = new LongestCommonAncestorManager<EditOperation<SyntaxNodeOrToken>>();
            var lcsrresult = lcs.FindDifference(scrips[0], scrips[1]);
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
                    var result = new MatchResult(Tuple.Create(from.Value, new Bindings(new List<SyntaxNodeOrToken> { from.Value })));
                    matches.Add(result);

                    var key = (SyntaxNodeOrToken)input[rule.Body[0]];
                    var treeUp = TreeUpdateDictionary[input];
                    var previousTree = ConverterHelper.MakeACopy(treeUp.CurrentTree);
                    treeUp.ProcessEditOperation(editOperation);
                    CurrentTrees[key] = previousTree; 
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
                    var result = new MatchResult(Tuple.Create(from.Value, new Bindings(new List<SyntaxNodeOrToken> { from.Value })));
                    matches.Add(result);

                    var key = (SyntaxNodeOrToken)input[rule.Body[0]];
                    var treeUp = TreeUpdateDictionary[input];

                    var previousTree = ConverterHelper.MakeACopy(treeUp.CurrentTree);
                    treeUp.ProcessEditOperation(editOperation);
                    CurrentTrees[key] = previousTree;
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
        // ReSharper disable once InconsistentNaming
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
                    matches.Add(update.To.Value);
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

                    var key = (SyntaxNodeOrToken)input[rule.Body[0]];
                    var treeUp = TreeUpdateDictionary[input];
                    matches.Add(editOperation.K);

                    var previousTree = ConverterHelper.MakeACopy(treeUp.CurrentTree);
                    treeUp.ProcessEditOperation(editOperation);
                    CurrentTrees[key] = previousTree;
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
            return MoveBase(rule, parameter, spec);
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
        // ReSharper disable once InconsistentNaming
        public static DisjunctiveExamplesSpec WitnessFunctionMoveTo(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kBinding, ExampleSpec parentBinding)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (EditOperation<SyntaxNodeOrToken> editOperation in spec.DisjunctiveExamples[input])
                {
                    if (!(editOperation is Move<SyntaxNodeOrToken>)) return null;

                    var from = editOperation.T1Node;
                    var result = new MatchResult(Tuple.Create(from.Value, new Bindings(new List<SyntaxNodeOrToken> { from.Value })));
                    matches.Add(result);
                }
                kExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }

        public static DisjunctiveExamplesSpec MoveBase(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (EditOperation<SyntaxNodeOrToken> editOperation in spec.DisjunctiveExamples[input])
                {
                    if (!(editOperation is Move<SyntaxNodeOrToken>)) return null;

                    AddMatchesMove(matches, parameter, editOperation);
                }

                kExamples[input] = matches;
            }

            return DisjunctiveExamplesSpec.From(kExamples);
        }

        private static void AddMatchesMove(List<object> matches, int parameter, EditOperation<SyntaxNodeOrToken> editOperation)
        {
            if (parameter == 1)
            {
                matches.Add(editOperation.K);
            }

            if (parameter == 2)
            {
                var move = (Move<SyntaxNodeOrToken>) editOperation;
                var parent = move.Parent;
                var result = new MatchResult(Tuple.Create(parent.Value, new Bindings(new List<SyntaxNodeOrToken> { parent.Value })));

                matches.Add(result);
            }
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

                    var key = (SyntaxNodeOrToken)input[rule.Body[0]];
                    var treeUp = TreeUpdateDictionary[input];
                    matches.Add(editOperation.K);

                    var previousTree = ConverterHelper.MakeACopy(treeUp.CurrentTree);
                    treeUp.ProcessEditOperation(editOperation);
                    CurrentTrees[key] = previousTree;
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
            return InsertBase(rule, parameter, spec);
        }

        public static DisjunctiveExamplesSpec InsertBase(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (EditOperation<SyntaxNodeOrToken> editOperation in spec.DisjunctiveExamples[input])
                {
                    if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;

                    AddMatchesInsert(matches, parameter, editOperation);
                }

                kExamples[input] = matches;
            }

            return DisjunctiveExamplesSpec.From(kExamples);
        }

        private static void AddMatchesInsert(List<object> matches, int parameter, EditOperation<SyntaxNodeOrToken> editOperation)
        {
            if (parameter == 1)
            {
                matches.Add(editOperation.K);
            }

            if (parameter == 2)
            {
                var parent = editOperation.Parent;
                var result = new MatchResult(Tuple.Create(parent.Value, new Bindings(new List<SyntaxNodeOrToken> { parent.Value })));

                matches.Add(result);
            }
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
                var matches = new List<object>();
                foreach (EditOperation<SyntaxNodeOrToken> editOperation in spec.DisjunctiveExamples[input])
                {
                    if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;
            
                    matches.Add(editOperation.T1Node.Value);
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
                foreach (SyntaxNodeOrToken sot in spec.DisjunctiveExamples[input])
                {
                    if (sot.IsToken) return null;

                    kMatches.Add(sot.Kind());
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
        // ReSharper disable once InconsistentNaming
        public static DisjunctiveExamplesSpec WitnessNode1AST1(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            return NodeBase(spec);
        }

        /// <summary>
        /// C witness functino for expression parameter with two child
        /// </summary>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjuntive examples specification</returns>
        private static DisjunctiveExamplesSpec NodeBase(DisjunctiveExamplesSpec spec)
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


        [WitnessFunction("NodesMap", 1)]
        public static SubsequenceSpec WitnessNodesMap(GrammarRule rule, int parameter,
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

        private static ITreeNode<SyntaxNodeOrToken> GetCurrentTree(SyntaxNodeOrToken n)
        {
            if (!CurrentTrees.ContainsKey(n))
            {
                CurrentTrees[n] = ConverterHelper.ConvertCSharpToTreeNode(n);
            }

            ITreeNode<SyntaxNodeOrToken> node = CurrentTrees[n];

            return node;
        }
    }
}