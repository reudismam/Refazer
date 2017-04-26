using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using RefazerFunctions.Substrings;
using RefazerFunctions.List;
using RefazerFunctions.Spg.Witness;
using TreeEdit.Spg.Script;
using TreeEdit.Spg.TreeEdit.Update;
using TreeElement.Spg.Node;

namespace RefazerFunctions
{
    /// <summary>
    /// Witness functions for the grammar
    /// </summary>
    public class WitnessFunctions: DomainLearningLogic
    {
        public static Dictionary<State, Dictionary<string, string>> Bindings = new Dictionary<State, Dictionary<string, string>>();

        public WitnessFunctions(Grammar grammar) : base(grammar) { }

        /// <summary>
        /// Current trees.
        /// </summary>
        public static Dictionary<object, TreeNode<SyntaxNodeOrToken>> CurrentTrees = new Dictionary<object, TreeNode<SyntaxNodeOrToken>>();
        /// <summary>
        /// TreeUpdate mapping.
        /// </summary>
        public static Dictionary<object, TreeUpdate> TreeUpdateDictionary = new Dictionary<object, TreeUpdate>();

        /// <summary>
        /// Literal witness function for parameter tree.
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction(nameof(Semantics.Concrete), 0)]
        public  DisjunctiveExamplesSpec LiteralTree(GrammarRule rule, ExampleSpec spec)
        {
            return Literal.LiteralTree(rule, spec);
        }

        /// <summary>
        /// Literal witness function for parameter tree.
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction(nameof(Semantics.Concrete), 0)]
        public DisjunctiveExamplesSpec LiteralTree(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            return Literal.LiteralTreeDisjunctive(rule, spec);
        }

        /// <summary>
        /// KindRef witness function for parameter kind.
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction(nameof(Semantics.Abstract), 0)]
        public DisjunctiveExamplesSpec VariableKind(GrammarRule rule, ExampleSpec spec)
        {
            return Variable.VariableKind(rule, spec);
        }

        /// <summary>
        /// KindRef witness function for parameter kind.
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction(nameof(Semantics.Variable), 0)]
        public ExampleSpec AbstractId(GrammarRule rule, ExampleSpec spec)
        {
            return Variable.VariableID(rule, spec);
        }

        /// <summary>
        /// Parent witness function for parameter kindRef
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction(nameof(Semantics.Context), 0)]
        public DisjunctiveExamplesSpec ContextMatch(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            return new Context().ContextPattern(rule, spec);
        }

        /// <summary>
        /// Parent witness function for parameter k
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kindBinding">kindRef binding</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction(nameof(Semantics.Context), 1, DependsOnParameters = new[] { 0 })]
        public DisjunctiveExamplesSpec ContextXPath(GrammarRule rule, DisjunctiveExamplesSpec spec, ExampleSpec kindBinding)
        {
            return new Context().ContextXPath(rule, spec, kindBinding);
        }

        /// <summary>
        /// CList witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction(nameof(Semantics.CList), 0)]
        public DisjunctiveExamplesSpec WitnessCList1(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            return GList<Tuple<TreeNode<SyntaxNodeOrToken>, int>>.List0(rule, spec);
        }

        /// <summary>
        /// CList witness function for parameter 1
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction(nameof(Semantics.CList), 1, DependsOnParameters = new[] { 0 })]
        public DisjunctiveExamplesSpec WitnessNList2(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            return GList<Tuple<TreeNode<SyntaxNodeOrToken>, int>>.List1(rule, spec);
        }

        /// <summary>
        /// SC witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction(nameof(Semantics.SC), 0)]
        public DisjunctiveExamplesSpec WitnessScChild1(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            return GList<Tuple<TreeNode<SyntaxNodeOrToken>, int>>.Single(rule, spec);
        }

        /// <summary>
        /// NList witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction(nameof(Semantics.NList), 0)]
        public DisjunctiveExamplesSpec WitnessNList1(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            return GList<TreeNode<SyntaxNodeOrToken>>.List0(rule, spec);
        }

        /// <summary>
        /// NList witness function for parameter 1
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction(nameof(Semantics.NList), 1)]
        public DisjunctiveExamplesSpec WitnessCList2(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            return GList<TreeNode<SyntaxNodeOrToken>>.List1(rule, spec);
        }

        /// <summary>
        /// SN witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction(nameof(Semantics.SN), 0)]
        public DisjunctiveExamplesSpec WitnessCnChild1(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            return GList<TreeNode<SyntaxNodeOrToken>>.Single(rule, spec);
        }

        /// <summary>
        /// NList witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction(nameof(Semantics.EList), 0)]
        public SubsequenceSpec WitnessEList1(GrammarRule rule, SubsequenceSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<Edit<SyntaxNodeOrToken>>();
                foreach (List<List<Edit<SyntaxNodeOrToken>>> editList in spec.PositiveExamples[input])
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
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction(nameof(Semantics.EList), 1)]
        public SubsequenceSpec WitnessEList2(GrammarRule rule, SubsequenceSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var newPatch = new List<List<Edit<SyntaxNodeOrToken>>>();
                foreach (List<List<Edit<SyntaxNodeOrToken>>> editList in spec.PositiveExamples[input])
                {
                    if (!editList.Any()) return null;
                    if (editList.Count == 1) return null;

                    editList.RemoveAt(0);
                    newPatch = editList;
                }
                treeExamples[input] = new List<List<List<Edit<SyntaxNodeOrToken>>>> { newPatch };
            }
            return new SubsequenceSpec(treeExamples);
        }

        /// <summary>
        /// SN witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction(nameof(Semantics.SE), 0)]
        public SubsequenceSpec WitnessSeChild1(GrammarRule rule, SubsequenceSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<Edit<SyntaxNodeOrToken>>();
                foreach (List<List<Edit<SyntaxNodeOrToken>>> editList in spec.PositiveExamples[input])
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
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive examples specification with the kind of the nodes in the examples</returns>
        [WitnessFunction(nameof(Semantics.Pattern), 0)]
        public DisjunctiveExamplesSpec CKind(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            return Match.CKind(rule, spec);
        }

        /// <summary>
        /// C witness function for expression parameter with one child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kind">Learned kind</param>
        /// <returns>Disjunctive examples specification</returns>
        [WitnessFunction(nameof(Semantics.Pattern), 1, DependsOnParameters = new[] { 0 })]
        public DisjunctiveExamplesSpec CChildren(GrammarRule rule, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            return Match.CChildren(rule, spec, kind);
        }

        [WitnessFunction(nameof(Semantics.Reference), 1)]
        public DisjunctiveExamplesSpec MatchPattern(GrammarRule rule, ExampleSpec spec)
        {
            return Match.MatchPattern(rule, spec);
        }

        [WitnessFunction(nameof(Semantics.Reference), 2, DependsOnParameters = new[] { 1 })]
        public DisjunctiveExamplesSpec MatchK(GrammarRule rule, ExampleSpec spec, ExampleSpec kind)
        {
            return Match.MatchK(rule, spec, kind);
        }

        [WitnessFunction(nameof(Semantics.Transformation), 1)]
        public SubsequenceSpec TransformationLoop(GrammarRule rule, ExampleSpec spec)
        {
            return Transformation.TransformationRule(rule, spec);
        }

        [WitnessFunction("EditMap", 1)]
        public SubsequenceSpec RegionMap(GrammarRule rule, SubsequenceSpec spec)
        {
            return Transformation.EditMapTNode(rule, spec);
        }

        [WitnessFunction(nameof(Semantics.AllNodes), 1)]
        public DisjunctiveExamplesSpec TemplateTraversal(GrammarRule rule, SubsequenceSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                treeExamples[input] = new List<string> { "PostOrder" };
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        /// <summary>
        /// Witness function for parameter k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction(nameof(Semantics.Delete), 1)]
        public ExampleSpec DeleteFrom(GrammarRule rule, ExampleSpec spec)
        {
            return EditOperation.T1Learner<Delete<SyntaxNodeOrToken>>(rule, spec);
        }

        /// <summary>
        /// Witness function for parameter k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction(nameof(Semantics.Update), 1)]
        public ExampleSpec UpdateTo(GrammarRule rule, ExampleSpec spec)
        {
            return EditOperation.UpdateTo(rule, spec);
        } 

        /// <summary>
        /// Witness function for parameter k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction(nameof(Semantics.Insert), 1)]
        public ExampleSpec Insertast(GrammarRule rule, ExampleSpec spec)
        {
            return EditOperation.Insertast(rule, spec);
        }

        /// <summary>
        /// Witness function for parameter k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction(nameof(Semantics.Insert), 2, DependsOnParameters = new[] { 1 })]
        public ExampleSpec InsertK(GrammarRule rule, ExampleSpec spec, ExampleSpec AstSpec)
        {
            return EditOperation.LearnK<Insert<SyntaxNodeOrToken>>(rule, spec);
        }

        /// <summary>
        /// Witness function for parameter k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction(nameof(Semantics.InsertBefore), 1)]
        public ExampleSpec InsertBeforeParent(GrammarRule rule, ExampleSpec spec)
        {
            return EditOperation.InsertBeforeParentLearner(rule, spec);
        }

        /// <summary>
        /// Witness function for parameter k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction(nameof(Semantics.InsertBefore), 2, DependsOnParameters = new[] { 1 })]
        public ExampleSpec InsertBeforeast(GrammarRule rule, ExampleSpec spec)
        {
            return EditOperation.Insertast(rule, spec);
        }

        /// <summary>
        /// Node witness function for expression parameter with one child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive examples specification</returns>
        [WitnessFunction(nameof(Semantics.Node), 0)]
        public DisjunctiveExamplesSpec NodeKind(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            return AST.NodeKind(rule, spec);
        }

        /// <summary>
        /// C witness function for expression parameter with one child
        /// </summary>N
        /// <param name="rule">C rule</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kind">Learned kind</param>
        /// <returns>Disjunctive examples specification</returns>
        [WitnessFunction(nameof(Semantics.Node), 1, DependsOnParameters = new[] { 0 })]
        public DisjunctiveExamplesSpec NodeChildren(GrammarRule rule, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            return AST.NodeChildren(rule, spec, kind);
        }

        /// <summary>
        /// Learn a constant node
        /// </summary>
        /// <param name="rule">Rule</param>
        /// <param name="rule">Rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction(nameof(Semantics.ConstNode), 0)]
        public ExampleSpec Const(GrammarRule rule, ExampleSpec spec)
        {
            return AST.Const(rule, spec);
        }

        [WitnessFunction(nameof(Semantics.Match), 1)]
        public ExampleSpec NodeMatch(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            var eExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var target = (TreeNode<SyntaxNodeOrToken>)input[rule.Body[0]];
                var parent = ConverterHelper.ConvertCSharpToTreeNode(target.Value.Parent.Parent);
                target = TreeUpdate.FindNode(parent, target.Value);
                eExamples[input] = target;
            }
            return new ExampleSpec(eExamples);
        }

        public static TreeNode<SyntaxNodeOrToken> GetCurrentTree(object n)
        {
            var node = CurrentTrees[n];
            return node;
        }
    }
}