using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using ProseSample.Substrings.List;
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
        /// Tree witness function for parameter tree.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Example specificaiton</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Tree", 0)]
        public static DisjunctiveExamplesSpec TreeKindRef(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return Match.TreeKindRef(rule, parameter, spec);
        }

        /// <summary>
        /// KindRef witness function for parameter kind.
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Variable", 0)]
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
        /// Parent witness function for parameter kindRef
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">parameter</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjuntive example specification</returns>
        [WitnessFunction("RightChild", 0)]
        public static DisjunctiveExamplesSpec RightChildVariable(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return new RightChild().ParentVariable(rule, parameter, spec);
        }

        /// <summary>
        /// Parent witness function for parameter kindRef
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">parameter</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjuntive example specification</returns>
        [WitnessFunction("Child", 0)]
        public static DisjunctiveExamplesSpec ChildVariable(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return Child.ParentVariable(rule, parameter, spec);
        }

        /// <summary>
        /// Parent witness function for parameter k
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">parameter</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kindBinding">kindRef binding</param>
        /// <returns>Disjuntive example specification</returns>
        [WitnessFunction("RightChild", 1, DependsOnParameters = new[] { 0 })]
        public static DisjunctiveExamplesSpec RightChildK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kindBinding)
        {
            return new RightChild().ParentK(rule, parameter, spec, kindBinding);
        }

        ///// <summary>
        ///// PList witness function for parameter 0
        ///// </summary>
        ///// <param name="rule">Literal rule</param>
        ///// <param name="parameter">Parameter number</param>
        ///// <param name="spec">Example specification</param>
        ///// <returns>Disjunctive example specification</returns>
        //[WitnessFunction("PList", 0)]
        //public static DisjunctiveExamplesSpec PListTemplate(GrammarRule rule, int parameter, ExampleSpec spec)
        //{
        //    return GList<List<ITreeNode<SyntaxNodeOrToken>>>.List0(rule, parameter, spec);
        //}

        ///// <summary>
        ///// PList witness function for parameter 1
        ///// </summary>
        ///// <param name="rule">Literal rule</param>Pa
        ///// <param name="parameter">Parameter number</param>
        ///// <param name="spec">Example specification</param>
        ///// <returns>Disjunctive example specification</returns>
        //[WitnessFunction("PList", 1)]
        //public static DisjunctiveExamplesSpec PListChildren(GrammarRule rule, int parameter, ExampleSpec spec)
        //{
        //    return GList<List<ITreeNode<SyntaxNodeOrToken>>>.List1(rule, parameter, spec);
        //}

        ///// <summary>
        ///// SP witness function for parameter 0
        ///// </summary>
        ///// <param name="rule">Literal rule</param>
        ///// <param name="parameter">Parameter number</param>
        ///// <param name="spec">Example specification</param>
        ///// <returns>Disjunctive example specification</returns>
        //[WitnessFunction("SP", 0)]
        //public static DisjunctiveExamplesSpec SpTemplate(GrammarRule rule, int parameter, ExampleSpec spec)
        //{
        //    return GList<List<ITreeNode<SyntaxNodeOrToken>>>.Single(rule, parameter, spec);
        //}

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
            return GList<Node>.List0(rule, parameter, spec);
        }

        /// <summary>
        /// CList witness function for parameter 1
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("CList", 1)]
        public static DisjunctiveExamplesSpec WitnessNList2(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return GList<Node>.List1(rule, parameter, spec);
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
            return GList<Node>.Single(rule, parameter, spec);
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
        public static SubsequenceSpec WitnessEList1(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<Edit<SyntaxNodeOrToken>>();
                foreach (Patch patch in spec.DisjunctiveExamples[input])
                {
                    var matchResult = patch.Edits;
                    if (!matchResult.Any()) return null;
                    if (matchResult.Count == 1) return null;

                    matches = matchResult.First();
                }
                treeExamples[input] = matches;
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
        public static ExampleSpec WitnessEList2(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var newPatch = new Patch();
                foreach (Patch patch in spec.DisjunctiveExamples[input])
                {
                    var matchResult = patch.Edits;
                    if (!matchResult.Any()) return null;
                    if (matchResult.Count == 1) return null;

                    matchResult.RemoveAt(0);
                    newPatch.Edits = matchResult;
                }
                treeExamples[input] = newPatch;
            }
            return new ExampleSpec(treeExamples);
        }

        /// <summary>
        /// SN witness function for parameter 0
        /// </summary>
        /// <param name="rule">Literal rule</param>
        /// <param name="parameter">Parameter number</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("SE", 0)]
        public static SubsequenceSpec WitnessSeChild1(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<Edit<SyntaxNodeOrToken>>();
                foreach (Patch patch in spec.DisjunctiveExamples[input])
                {
                    var matchResult = patch.Edits;
                    if (!matchResult.Any()) return null;
                    if (matchResult.Count != 1) return null;

                    matches = matchResult.First();
                }
                treeExamples[input] = matches;
            }
            return new SubsequenceSpec(treeExamples);
        }

        ///// <summary>
        ///// C witness function for kind parameter with one child
        ///// </summary>
        ///// <param name="rule">C rule</param>
        ///// <param name="parameter">Parameter associated to C rule</param>
        ///// <param name="spec">Example specification</param>
        ///// <returns>Disjunctive examples specification with the kind of the nodes in the examples</returns>
        //[WitnessFunction("P", 0)]
        //public static DisjunctiveExamplesSpec PKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        //{
        //    return Template.PKind(rule, parameter, spec);
        //}

        ///// <summary>
        ///// C witness functino for expression parameter with one child
        ///// </summary>
        ///// <param name="rule">C rule</param>
        ///// <param name="parameter">Parameter</param>
        ///// <param name="spec">Example specification</param>
        ///// <param name="kind">Learned kind</param>
        ///// <returns>Disjuntive examples specification</returns>
        //[WitnessFunction("P", 1, DependsOnParameters = new[] { 0 })]
        //public static DisjunctiveExamplesSpec PChildren(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        //{
        //    return Template.PChildren(rule, parameter, spec, kind);
        //}

        ///// <summary>
        ///// Concrete witness function for parameter tree.
        ///// </summary>
        ///// <param name="rule">Literal rule</param>
        ///// <param name="parameter">Parameter number</param>
        ///// <param name="spec">Example specification</param>
        ///// <returns>Disjunctive example specification</returns>
        //[WitnessFunction("Concrete", 0)]
        //public static DisjunctiveExamplesSpec ConcreteTree(GrammarRule rule, int parameter, ExampleSpec spec)
        //{
        //    return Template.ConcreteTree(rule, parameter, spec);
        //}

        ///// <summary>
        ///// Abstract witness function for parameter kind
        ///// </summary>
        ///// <param name="rule">Grammar rule</param>
        ///// <param name="parameter">Rule parameter</param>
        ///// <param name="spec">Example specification</param>
        ///// <returns>Disjunctive example specification</returns>
        //[WitnessFunction("Abstract", 0)]
        //public static DisjunctiveExamplesSpec AbstractKind(GrammarRule rule, int parameter, ExampleSpec spec)
        //{
        //    return Template.AbstractKind(rule, parameter, spec);
        //}

        /// <summary>
        /// C witness function for kind parameter with one child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="parameter">Parameter associated to C rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive examples specification with the kind of the nodes in the examples</returns>
        [WitnessFunction("C", 1)]
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
        [WitnessFunction("C", 2, DependsOnParameters = new[] { 1 })]
        public static DisjunctiveExamplesSpec CChildren(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            return Match.CChildren(rule, parameter, spec, kind);
        }

        [WitnessFunction("Match", 1)]
        public static DisjunctiveExamplesSpec MatchPattern(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return Match.MatchPattern(rule, parameter, spec);
        }

        [WitnessFunction("Match", 2, DependsOnParameters = new[] { 1 })]
        public static DisjunctiveExamplesSpec MatchK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            return Match.MatchK(rule, parameter, spec, kind);
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
            return Transformation.ScriptEdits(rule, parameter, spec);
        }

        [WitnessFunction("Transformation", 1)]
        public static SubsequenceSpec TransformationLoop(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return Transformation.TransformationLoop(rule, parameter, spec);
        }

        [WitnessFunction("RegionMap", 1)]
        public static SubsequenceSpec RegionMap(GrammarRule rule, int parameter, SubsequenceSpec spec)
        {
            return Transformation.Loop(rule, parameter, spec);
        }

        [WitnessFunction("Template", 1)]
        public static DisjunctiveExamplesSpec TemplateTemplate(GrammarRule rule, int parameter, SubsequenceSpec spec)
        {
            return Transformation.TemplateTemplate(rule, parameter, spec);
        }

        [WitnessFunction("Traversal", 1)]
        public static DisjunctiveExamplesSpec TemplateTraversal(GrammarRule rule, int parameter, SubsequenceSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                treeExamples[input] = new List<string> {"PostOrder"};
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
            return EditOperation.DeleteFrom(rule, parameter, spec);
        }

        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction("Update", 1)]
        public static ExampleSpec UpdateFrom(GrammarRule rule, int parameter, ExampleSpec spec)
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
        public static ExampleSpec MoveFrom(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return EditOperation.MoveTo(rule, parameter, spec);
        }

        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction("Move", 2, DependsOnParameters = new[] { 1 })]
        public static ExampleSpec MoveTo(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return EditOperation.MoveK(rule, parameter, spec);
        }

        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction("Insert", 1)]
        public static ExampleSpec InsertParent(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return EditOperation.InsertAST(rule, parameter, spec);
        }

        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction("Insert", 2, DependsOnParameters = new[] { 1 })]
        public static ExampleSpec InsertAST(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return EditOperation.InsertK(rule, parameter, spec);
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
        public static DisjunctiveExamplesSpec Const(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return AST.Const(rule, parameter, spec);
        }

        [WitnessFunction("Ref", 1)]
        public static DisjunctiveExamplesSpec Ref(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return AST.Ref(rule, parameter, spec);
        } 

        [WitnessFunction("EditMap", 1)]
        public static SubsequenceSpec EditMap(GrammarRule rule, int parameter, SubsequenceSpec spec)
        {
            return Map.EditMap(rule, parameter, spec);
        }

        [WitnessFunction("MM", 1)]
        public static ExampleSpec MMMatch(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var editExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var inpTree = (Edit<SyntaxNodeOrToken>)input[rule.Body[0]];
                editExamples[input] = new Node(inpTree.EditOperation.Parent);
            }

            return new ExampleSpec(editExamples);
        }

        public static ITreeNode<SyntaxNodeOrToken> GetCurrentTree(object n)
        {
            var node = CurrentTrees[n];
            return node;
        }
    }
}