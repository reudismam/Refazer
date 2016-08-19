﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using ProseSample.Substrings.List;
using ProseSample.Substrings.Spg.Bean;
using TreeEdit.Spg.Script;
using TreeEdit.Spg.TreeEdit.Update;
using ProseSample.Substrings.Spg.Witness;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    public static class WitnessFunctions
    {
        /// <summary>
        /// Current trees.
        /// </summary>
        public static Dictionary<object, ITreeNode<SyntaxNodeOrToken>> CurrentTrees = new Dictionary<object, ITreeNode<SyntaxNodeOrToken>>();
        /// <summary>
        /// TreeUpdate mapping.
        /// </summary>
        public static Dictionary<object, TreeUpdate> TreeUpdateDictionary = new Dictionary<object, TreeUpdate>();

        /// <summary>
        /// Literal witness function for parameter tree.
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Literal", 0)]
        public static DisjunctiveExamplesSpec LiteralTree(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return Literal.LiteralTree(rule, parameter, spec);
        }

        /// <summary>
        /// KindRef witness function for parameter kind.
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Variable", 0)]
        public static ExampleSpec VariableKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return Variable.VariableKind(rule, parameter, spec);
        }

        /// <summary>
        /// KindRef witness function for parameter kind.
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Leaf", 0)]
        public static ExampleSpec LeafKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return Variable.LeafKind(rule, parameter, spec);
        }

        /// <summary>
        /// Parent witness function for parameter kindRef
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">parameter</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjuntive example specification</returns>
        [WitnessFunction("Parent", 0)]
        public static DisjunctiveExamplesSpec ParentVariable(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return new Parent().ParentVariable(rule, parameter, spec);
        }

        /// <summary>
        /// Parent witness function for parameter k
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">parameter</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kindBinding">kindRef binding</param>
        /// <returns>Disjuntive example specification</returns>
        [WitnessFunction("Parent", 1, DependsOnParameters = new[] { 0 })]
        public static DisjunctiveExamplesSpec ParentK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, DisjunctiveExamplesSpec kindBinding)
        {
            return new Parent().ParentK(rule, parameter, spec, kindBinding);
        }

        /// <summary>
        /// CList witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("CList", 0)]
        public static DisjunctiveExamplesSpec WitnessCList1(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return GList<ITreeNode<Token>>.List0(rule, parameter, spec);
        }

        /// <summary>
        /// CList witness function for parameter 1
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("CList", 1, DependsOnParameters = new[] { 0 })]
        public static DisjunctiveExamplesSpec WitnessNList2(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, DisjunctiveExamplesSpec matchSpec)
        {
            return GList<ITreeNode<Token>>.List1(rule, parameter, spec);
        }

        /// <summary>
        /// SC witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("SC", 0)]
        public static DisjunctiveExamplesSpec WitnessScChild1(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return GList<ITreeNode<Token>>.Single(rule, parameter, spec);
        }

        /// <summary>
        /// NList witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("SL", 0)]
        public static DisjunctiveExamplesSpec WitnessSl1(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return GList<Edit<SyntaxNodeOrToken>>.List0(rule, parameter, spec);
        }

        /// <summary>
        /// NList witness function for parameter 1
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("SL", 1)]
        public static DisjunctiveExamplesSpec WitnessSl2(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return GList<Edit<SyntaxNodeOrToken>>.List1(rule, parameter, spec);
        }

        /// <summary>
        /// SN witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("SO", 0)]
        public static DisjunctiveExamplesSpec WitnessSoChild1(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return GList<Edit<SyntaxNodeOrToken>>.Single(rule, parameter, spec);
        }

        /// <summary>
        /// NList witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("NList", 0)]
        public static DisjunctiveExamplesSpec WitnessNList1(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return GList<ITreeNode<SyntaxNodeOrToken>>.List0(rule, parameter, spec);
        }

        /// <summary>
        /// NList witness function for parameter 1
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("NList", 1)]
        public static DisjunctiveExamplesSpec WitnessCList2(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return GList<ITreeNode<SyntaxNodeOrToken>>.List1(rule, parameter, spec);
        }

        /// <summary>
        /// SN witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("SN", 0)]
        public static DisjunctiveExamplesSpec WitnessCnChild1(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return GList<ITreeNode<SyntaxNodeOrToken>>.Single(rule, parameter, spec);
        }

        /// <summary>
        /// NList witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("EList", 0)]
        public static SubsequenceSpec WitnessEList1(GrammarRule rule, int parameter, SubsequenceSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<Script>();
                foreach (List<List<Script>> editList in spec.Examples[input])
                {
                    if (!editList.Any()) return null;
                    if (editList.Count == 1) return null;

                    matches = editList.First();
                }
                if (matches.Any())
                {
                    treeExamples[input] = matches;
                }
            }
            return new SubsequenceSpec(treeExamples);
        }

        /// <summary>
        /// NList witness function for parameter 1
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("EList", 1)]
        public static SubsequenceSpec WitnessEList2(GrammarRule rule, int parameter, SubsequenceSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var newPatch = new List<List<Script>>();
                foreach (List<List<Script>> editList in spec.Examples[input])
                {
                    if (!editList.Any()) return null;
                    if (editList.Count == 1) return null;

                    editList.RemoveAt(0);
                    newPatch = editList;
                }
                treeExamples[input] = new List<List<List<Script>>> { newPatch };
            }
            return new SubsequenceSpec(treeExamples);
        }

        /// <summary>
        /// SN witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("SE", 0)]
        public static SubsequenceSpec WitnessSeChild1(GrammarRule rule, int parameter, SubsequenceSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<Script>();
                foreach (List<List<Script>> editList in spec.Examples[input])
                {
                    if (!editList.Any()) return null;
                    if (editList.Count != 1) return null;

                    matches = editList.First();
                }
                if (matches.Any())
                {
                    treeExamples[input] = matches;
                }
            }
            return new SubsequenceSpec(treeExamples);
        }

        /// <summary>
        /// C witness function for kind parameter with one child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="parameter">Parameter associated to C rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive examples specification with the kind of the nodes in the examples</returns>
        [WitnessFunction("C", 0)]
        public static DisjunctiveExamplesSpec CKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return Match.CKind(rule, parameter, spec);
        }

        /// <summary>
        /// C witness functino for expression parameter with one child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kind">Learned kind</param>
        /// <returns>Disjuntive examples specification</returns>
        [WitnessFunction("C", 1, DependsOnParameters = new[] { 0 })]
        public static DisjunctiveExamplesSpec CChildren(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            return Match.CChildren(rule, parameter, spec, kind);
        }

        [WitnessFunction("ReferenceNode", 1)]
        public static DisjunctiveExamplesSpec MatchPattern(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return Match.MatchPattern(rule, parameter, spec);
        }

        [WitnessFunction("ReferenceNode", 2, DependsOnParameters = new[] { 1 })]
        public static DisjunctiveExamplesSpec MatchK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            return Match.MatchK(rule, parameter, spec, kind);
        }

        [WitnessFunction("NMatch", 0)]
        public static DisjunctiveExamplesSpec NMatchPattern(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var mats = new List<ITreeNode<Token>>();
                foreach (Pattern node in spec.DisjunctiveExamples[input])
                {
                    if (node.GetType().IsSubclassOf(typeof(Pattern))) continue;
                    var target = node.Tree;//Target(node);
                    if (target == null) continue;
                    mats.Add(target);
                }
                if (!mats.Any()) return null;
                treeExamples[input] = mats;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        [WitnessFunction("NMatch", 1)]
        public static DisjunctiveExamplesSpec NMatchId(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                treeExamples[input] = new List<string> { "NMatch" };
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        /// <summary>
        /// Witness function for script rule
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Rule parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Script", 1)]
        public static DisjunctiveExamplesSpec Edit(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return Transformation.ScriptEdits(rule, parameter, spec);
        }

        [WitnessFunction("Apply", 1)]
        public static SubsequenceSpec TransformationLoop(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return Transformation.ApplyPatch(rule, parameter, spec);
        }

        [WitnessFunction("EditMap", 1)]
        public static SubsequenceSpec RegionMap(GrammarRule rule, int parameter, SubsequenceSpec spec)
        {
            return Transformation.EditMapTNode(rule, parameter, spec);
        }

        [WitnessFunction("Traversal", 1)]
        public static DisjunctiveExamplesSpec TemplateTraversal(GrammarRule rule, int parameter, SubsequenceSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                treeExamples[input] = new List<string> { "PostOrder" };
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction("Delete", 1)]
        public static ExampleSpec DeleteFrom(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return EditOperation.T1Learner<Delete<SyntaxNodeOrToken>>(rule, parameter, spec);
        }

        ///// <summary>
        ///// Witness function for parater k in the insert operator
        ///// </summary>
        ///// <param name="rule">Grammar rule</param>
        ///// <param name="parameter">Parameter</param>
        ///// <param name="spec">Examples specification</param>
        ///// <returns></returns>
        //[WitnessFunction("Update", 1)]
        //public static ExampleSpec UpdateFrom(GrammarRule rule, int parameter, ExampleSpec spec)
        //{
        //    return EditOperation.T1Learner<Update<SyntaxNodeOrToken>>(rule, parameter, spec);
        //}

        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction("Update", 1)]
        public static ExampleSpec UpdateTo(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return EditOperation.UpdateTo(rule, parameter, spec);
        }

        ///// <summary>
        ///// Witness function for parater k in the insert operator
        ///// </summary>
        ///// <param name="rule">Grammar rule</param>
        ///// <param name="parameter">Parameter</param>
        ///// <param name="spec">Examples specification</param>
        ///// <returns></returns>
        //[WitnessFunction("Move", 1)]
        //public static ExampleSpec MoveFrom(GrammarRule rule, int parameter, ExampleSpec spec)
        //{
        //    return EditOperation.ParentLearner<Move<SyntaxNodeOrToken>>(rule, parameter, spec);
        //}

        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction("Move", 1)]
        public static ExampleSpec MoveTo(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return EditOperation.T1Learner<Move<SyntaxNodeOrToken>>(rule, parameter, spec);
        }

        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction("Move", 2, DependsOnParameters = new[] { 1 })]
        public static ExampleSpec MoveK(GrammarRule rule, int parameter, ExampleSpec spec, ExampleSpec ToSpec)
        {
            return EditOperation.LearnK<Move<SyntaxNodeOrToken>>(rule, parameter, spec);
        }

        ///// <summary>
        ///// Witness function for parater k in the insert operator
        ///// </summary>
        ///// <param name="rule">Grammar rule</param>
        ///// <param name="parameter">Parameter</param>
        ///// <param name="spec">Examples specification</param>
        ///// <returns></returns>
        //[WitnessFunction("Insert", 1)]
        //public static ExampleSpec InsertParent(GrammarRule rule, int parameter, ExampleSpec spec)
        //{
        //    return EditOperation.InsertParentLearner(rule, parameter, spec);
        //}

        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction("Insert", 1)]
        public static ExampleSpec Insertast(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return EditOperation.Insertast(rule, parameter, spec);
        }


        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction("InsertBefore", 1)]
        public static ExampleSpec InsertBeforeParent(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return EditOperation.InsertBeforeParentLearner(rule, parameter, spec);
        }

        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction("InsertBefore", 2, DependsOnParameters = new[] { 1 })]
        public static ExampleSpec InsertBeforeast(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return EditOperation.Insertast(rule, parameter, spec);
        }

        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction("Insert", 2, DependsOnParameters = new[] { 1 })]
        public static ExampleSpec InsertK(GrammarRule rule, int parameter, ExampleSpec spec, ExampleSpec AstSpec)
        {
            return EditOperation.LearnK<Insert<SyntaxNodeOrToken>>(rule, parameter, spec);
        }

        /// <summary>
        /// Node witness function for expression parameter with one child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjuntive examples specification</returns>
        [WitnessFunction("Node", 0)]
        public static DisjunctiveExamplesSpec NodeKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return AST.NodeKind(rule, parameter, spec);
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
            return AST.NodeChildren(rule, parameter, spec, kind);
        }

        /// <summary>
        /// Learn a constant node
        /// </summary>
        /// <param name="rule">Rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Const", 0)]
        public static ExampleSpec Const(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return AST.Const(rule, parameter, spec);
        }

        [WitnessFunction("Ref", 1)]
        public static DisjunctiveExamplesSpec Ref(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return AST.Ref(rule, parameter, spec);
        }

        [WitnessFunction("NodeMatch", 1)]
        public static DisjunctiveExamplesSpec NodeMatch(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            var dic = new Dictionary<int, List<ITreeNode<SyntaxNodeOrToken>>>();
            foreach (State input in spec.ProvidedInputs)
            {
                //get parent
                var target = (Node)input[rule.Body[0]];
                var parent = target.Value.Value.AsNode();
                for (int i = 0; i < 3; i++)
                {
                    if (parent.IsKind(SyntaxKind.Block) || parent.DescendantNodesAndSelf().Count() > 100)
                    {
                        break;
                    }

                    if (!dic.ContainsKey(i))
                    {
                        dic[i] = new List<ITreeNode<SyntaxNodeOrToken>>();
                    }
                    dic[i].Add(ConverterHelper.ConvertCSharpToTreeNode(parent));
                    parent = parent.Parent;
                }
            }

            var dicPattern = new Dictionary<int, Pattern>();
            for (int i = 0; i < spec.ProvidedInputs.Count(); i++)
            {
                var input = spec.ProvidedInputs.ElementAt(i);
                var target = (Node)input[rule.Body[0]];
                foreach (var item in dic)
                {
                    if (item.Value.Count() == spec.ProvidedInputs.Count())
                    {
                        var patterns = item.Value.Select(ConverterHelper.ConvertITreeNodeToToken).ToList();
                        var commonPattern = Match.BuildPattern(patterns);

                        if (item.Key == 0)
                        {
                            var p = new Pattern(commonPattern.Tree);
                            dicPattern[item.Key] = p;
                        }
                        else
                        {
                            var targetNode = TreeUpdate.FindNode(item.Value[i], target.Value.Value);
                            var str1 = Match.GetPath(targetNode);
                            var p = new PatternP(commonPattern.Tree, str1);
                            dicPattern[item.Key] = p;
                        }
                    }
                }
            }

            foreach (var input in spec.ProvidedInputs)
            {
                var resultList = new List<Pattern>();
                var list = dicPattern.OrderByDescending(o => o.Key).Select(item => item.Value).ToList();
                resultList.Add(list.Last());
                var valids = ValidPatterns(list);
                if (valids.Any()) resultList.Add(valids.First());
                eExamples[input] = resultList;
            }
            //end get parent
            return DisjunctiveExamplesSpec.From(eExamples);
        }

        private static List<Pattern> ValidPatterns(List<Pattern> list)
        {
            var valids = new List<Pattern>();
            for (int i = 0; i < list.Count - 1; i++)
            {
                var patternP = list[i] as PatternP;
                var child = Semantics.FindChild(patternP.Tree, patternP.K);
                if (child != null) valids.Add(patternP);
            }
            return valids;
        }


        public static DisjunctiveExamplesSpec NodeMatchBasic(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            var patterns = new List<ITreeNode<Token>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var inpTree = (Node)input[rule.Body[0]];
                var hinpTree = TreeManager<SyntaxNodeOrToken>.GetNodeAtHeight(inpTree.Value, 3);
                var pattern = BuildPattern(hinpTree);
                patterns.Add(pattern);
            }
            var commonPattern = Match.BuildPattern(patterns);
            foreach (State input in spec.ProvidedInputs)
            {
                var list = new List<Pattern> { new Pattern(ConverterHelper.MakeACopy(commonPattern.Tree)) };
                //if (commonPattern.Tree.Children.Any())
                //{
                //    var copy = ConverterHelper.MakeACopy(commonPattern.Tree);
                //    var empty = new EmptyToken();
                //    ITreeNode<Token> itreeNodeToken = new TreeNode<Token>(empty, new TLabel(SyntaxKind.EmptyStatement));
                //    itreeNodeToken.Children = copy.Children;
                //    list.Add(new Pattern(itreeNodeToken));
                //}
                eExamples[input] = list;
            }
            return DisjunctiveExamplesSpec.From(eExamples);
        }

        private static ITreeNode<Token> BuildPattern(ITreeNode<SyntaxNodeOrToken> tree)
        {
            var pattern = ConverterHelper.ConvertITreeNodeToToken(tree);
            return pattern;
        }

        public static ITreeNode<SyntaxNodeOrToken> GetCurrentTree(object n)
        {
            var node = CurrentTrees[n];
            return node;
        }
    }
}