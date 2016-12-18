using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using ProseFunctions.List;
using TreeEdit.Spg.Script;
using TreeEdit.Spg.TreeEdit.Update;
using ProseSample.Substrings.Spg.Witness;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    /// <summary>
    /// Witness functions for the grammar
    /// </summary>
    public static class WitnessFunctions
    {
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
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Concrete", 0)]
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
        [WitnessFunction("Abstract", 0)]
        public static DisjunctiveExamplesSpec VariableKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return Variable.VariableKind(rule, parameter, spec);
        }

        /// <summary>
        /// Parent witness function for parameter kindRef
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">parameter</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjuntive example specification</returns>
        [WitnessFunction("Context", 0)]
        public static DisjunctiveExamplesSpec ParentVariable(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return new Context().ParentVariable(rule, parameter, spec);
        }

        /// <summary>
        /// Parent witness function for parameter k
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">parameter</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kindBinding">kindRef binding</param>
        /// <returns>Disjuntive example specification</returns>
        [WitnessFunction("Context", 1, DependsOnParameters = new[] { 0 })]
        public static DisjunctiveExamplesSpec ParentK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kindBinding)
        {
            return new Context().ParentK(rule, parameter, spec, kindBinding);
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
            return GList<TreeNode<SyntaxNodeOrToken>>.List0(rule, parameter, spec);
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
            return GList<TreeNode<SyntaxNodeOrToken>>.List1(rule, parameter, spec);
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
            return GList<TreeNode<SyntaxNodeOrToken>>.Single(rule, parameter, spec);
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
            return GList<TreeNode<SyntaxNodeOrToken>>.List0(rule, parameter, spec);
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
            return GList<TreeNode<SyntaxNodeOrToken>>.List1(rule, parameter, spec);
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
            return GList<TreeNode<SyntaxNodeOrToken>>.Single(rule, parameter, spec);
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
                var matches = new List<Edit<SyntaxNodeOrToken>>();
                foreach (List<List<Edit<SyntaxNodeOrToken>>> editList in spec.Examples[input])
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
                var newPatch = new List<List<Edit<SyntaxNodeOrToken>>>();
                foreach (List<List<Edit<SyntaxNodeOrToken>>> editList in spec.Examples[input])
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
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("SE", 0)]
        public static SubsequenceSpec WitnessSeChild1(GrammarRule rule, int parameter, SubsequenceSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<Edit<SyntaxNodeOrToken>>();
                foreach (List<List<Edit<SyntaxNodeOrToken>>> editList in spec.Examples[input])
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
        [WitnessFunction("Pattern", 0)]
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
        [WitnessFunction("Pattern", 1, DependsOnParameters = new[] { 0 })]
        public static DisjunctiveExamplesSpec CChildren(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            return Match.CChildren(rule, parameter, spec, kind);
        }

        [WitnessFunction("Reference", 1)]
        public static DisjunctiveExamplesSpec MatchPattern(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return Match.MatchPattern(rule, parameter, spec);
        }

        [WitnessFunction("Reference", 2, DependsOnParameters = new[] { 1 })]
        public static DisjunctiveExamplesSpec MatchK(GrammarRule rule, int parameter, ExampleSpec spec, ExampleSpec kind)
        {
            return Match.MatchK(rule, parameter, spec, kind);
        }

        [WitnessFunction("Transformation", 1)]
        public static SubsequenceSpec TransformationLoop(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return Transformation.TransformationRule(rule, parameter, spec);
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
        /// </summary>N
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
        [WitnessFunction("ConstNode", 0)]
        public static ExampleSpec Const(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return AST.Const(rule, parameter, spec);
        }

        [WitnessFunction("Match", 1)]
        public static DisjunctiveExamplesSpec NodeMatch(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<TreeNode<SyntaxNodeOrToken>>();
                var target = (TreeNode<SyntaxNodeOrToken>)input[rule.Body[0]];
                if (target.Value.Parent.Parent.DescendantNodesAndSelf().Count() < 100)
                {
                    var parent = ConverterHelper.ConvertCSharpToTreeNode(target.Value.Parent.Parent);
                    target = TreeUpdate.FindNode(parent, target.Value);
                }
                else if (target.Value.Parent.DescendantNodesAndSelf().Count() < 100)
                {
                    var parent = ConverterHelper.ConvertCSharpToTreeNode(target.Value.Parent);
                    target = TreeUpdate.FindNode(parent, target.Value);
                }

                kMatches.Add(target);
                eExamples[input] = kMatches;
            }
            return DisjunctiveExamplesSpec.From(eExamples);
        }

        public static TreeNode<SyntaxNodeOrToken> GetCurrentTree(object n)
        {
            var node = CurrentTrees[n];
            return node;
        }
    }
}