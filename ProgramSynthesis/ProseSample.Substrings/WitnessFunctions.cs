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
        /// Tree witness function for parameter tree.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Example specificaiton</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Tree", 0)]
        public static DisjunctiveExamplesSpec TreeKindRef(GrammarRule rule, int parameter, ExampleSpec spec)
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
        public static DisjunctiveExamplesSpec ParentVariable(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return Parent.ParentVariable(rule, parameter, spec);
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
            return Parent.ParentK(rule, parameter, spec, kindBinding);
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

        /// <summary>
        /// C witness function for kind parameter with one child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="parameter">Parameter associated to C rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive examples specification with the kind of the nodes in the examples</returns>
        [WitnessFunction("P", 0)]
        public static DisjunctiveExamplesSpec PKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return Template.PKind(rule, parameter, spec);
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
        public static DisjunctiveExamplesSpec PChildren(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            return Template.PChildren(rule, parameter, spec, kind);
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
            return Template.ConcreteTree(rule, parameter, spec);
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
            return Template.AbstractKind(rule, parameter, spec);
        }

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

            TreeUpdateDictionary = new Dictionary<object, TreeUpdate>();
            CurrentTrees = new Dictionary<object, ITreeNode<SyntaxNodeOrToken>>();

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
                    TreeUpdateDictionary.Add(cc, treeUp);
                    CurrentTrees[cc] = tree;
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
        public static DisjunctiveExamplesSpec DeleteFrom(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
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
        public static DisjunctiveExamplesSpec UpdateFrom(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return EditOperation.UpdateFrom(rule, parameter, spec);
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
        public static DisjunctiveExamplesSpec UpdateTo(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec fromBinding)
        {
            return EditOperation.UpdateTo(rule, parameter, spec, fromBinding);
        }

        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction("Move", 1)]
        public static DisjunctiveExamplesSpec MoveFrom(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return EditOperation.MoveFrom(rule, parameter, spec);
        }

        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction("Move", 2, DependsOnParameters = new[] { 1 })]
        public static DisjunctiveExamplesSpec MoveTo(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return EditOperation.MoveTo(rule, parameter, spec);
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
        public static DisjunctiveExamplesSpec MoveK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kBinding, ExampleSpec parentBinding)
        {
            return EditOperation.MoveK(rule, parameter, spec, kBinding, parentBinding);
        }

        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns></returns>
        [WitnessFunction("Insert", 1)]
        public static DisjunctiveExamplesSpec InsertParent(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return EditOperation.InsertParent(rule, parameter, spec);
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
        public static DisjunctiveExamplesSpec InsertAST(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kBinding)
        {
            return EditOperation.InsertAST(rule, parameter, spec, kBinding);
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
        public static DisjunctiveExamplesSpec InsertK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kBinding, ExampleSpec parentBinding)
        {
            return EditOperation.InsertK(rule, parameter, spec, kBinding, parentBinding);
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
            var node = CurrentTrees[n];
            return node;
        }

        private static void PrintScript(List<EditOperation<SyntaxNodeOrToken>> script)
        {
            string s = script.Aggregate("", (current, v) => current + (v + "\n"));
        }
    }
}