using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using ProseSample.Substrings.List;
using TreeEdit.Spg.Script;
using TreeEdit.Spg.TreeEdit.Update;
using ProseSample.Substrings.Spg.Witness;

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
        public static ExampleSpec VariableKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
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
        [WitnessFunction("Context", 1, DependsOnParameters = new[] { 0 })]
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
            return GList<TreeNode<Token>>.List0(rule, parameter, spec);
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
            return GList<TreeNode<Token>>.List1(rule, parameter, spec);
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
            return GList<TreeNode<Token>>.Single(rule, parameter, spec);
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
        public static DisjunctiveExamplesSpec MatchPattern(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return Match.MatchPattern(rule, parameter, spec);
        }

        [WitnessFunction("Reference", 2, DependsOnParameters = new[] { 1 })]
        public static DisjunctiveExamplesSpec MatchK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            return Match.MatchK(rule, parameter, spec, kind);
        }

        [WitnessFunction("NodeMatch", 0)]
        public static DisjunctiveExamplesSpec NMatchPattern(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var mats = new List<TreeNode<Token>>();
                foreach (Pattern node in spec.DisjunctiveExamples[input])
                {
                    if (node.GetType().IsSubclassOf(typeof(Pattern))) continue;
                    var target = node.Tree;
                    if (target == null) continue;
                    mats.Add(target);
                }
                if (!mats.Any()) return null;
                treeExamples[input] = mats;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
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
        [WitnessFunction("ConstNode", 0)]
        public static ExampleSpec Const(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            return AST.Const(rule, parameter, spec);
        }

        [WitnessFunction("Match", 1)]
        public static DisjunctiveExamplesSpec NodeMatch(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            var dic = new Dictionary<int, List<TreeNode<SyntaxNodeOrToken>>>();
            foreach (State input in spec.ProvidedInputs)
            {
                //get parent
                var target = (TreeNode<SyntaxNodeOrToken>)input[rule.Body[0]];
                var parent = target.Value.AsNode();
                for (int i = 0; i < 3; i++)
                {
                    if (parent.IsKind(SyntaxKind.Block) || parent.DescendantNodesAndSelf().Count() > 100)
                    {
                        if(i != 0) break;
                    }

                    if (!dic.ContainsKey(i))
                    {
                        dic[i] = new List<TreeNode<SyntaxNodeOrToken>>();
                    }
                    dic[i].Add(ConverterHelper.ConvertCSharpToTreeNode(parent));
                    parent = parent.Parent;
                }
            }

            var dicPattern = new Dictionary<int, List<Pattern>>();
            foreach (var item in dic)
            {
                dicPattern[item.Key] = new List<Pattern>();
            }

            for (int i = 0; i < spec.ProvidedInputs.Count(); i++)
            {
                var input = spec.ProvidedInputs.ElementAt(i);
                var target = (TreeNode<SyntaxNodeOrToken>)input[rule.Body[0]];
                foreach (var item in dic)
                {
                    if (item.Value.Count() == spec.ProvidedInputs.Count())
                    {
                        var patterns = item.Value.Select(ConverterHelper.ConvertITreeNodeToToken).ToList();
                        var commonPattern = Match.BuildPattern(patterns);

                        if (item.Key == 0)
                        {
                            var p = new Pattern(commonPattern.Tree);
                            dicPattern[item.Key].Add(p);
                        }
                        else
                        {
                            var targetNode = TreeUpdate.FindNode(item.Value[i], target.Value);
                            var str1 = Match.GetPath(targetNode);
                            var p = new PatternP(commonPattern.Tree, str1);
                            dicPattern[item.Key].Add(p);
                        }
                    }
                }
            }

            foreach (var input in spec.ProvidedInputs)
            {
                var resultList = new List<Pattern>();
                var list = dicPattern.OrderByDescending(o => o.Key).Select(item => item.Value).ToList();
                resultList.Add(list.Last().First());
                var valids = ValidPatterns(list);
                if (valids.Any()) resultList.Add(valids.First());
                eExamples[input] = resultList;
            }
            //end get parent
            return DisjunctiveExamplesSpec.From(eExamples);
        }

        public static List<Pattern> ValidPatterns(List<List<Pattern>> list)
        {
            var valids = new List<Pattern>();
            for (int i = 0; i < list.Count - 1; i++)
            {
                var patternPList = list[i].Select(o => (PatternP) o).ToList();
                if (!patternPList.Any()) continue;
                if (patternPList.Any(o => !o.K.Equals(patternPList.First().K))) continue;

                var patternP = patternPList.First();
                var child = Semantics.FindChild(patternP.Tree, patternP.K);
                if (child == null) continue;
                if (patternP.Tree.DescendantNodesAndSelf().Any(o => o.Value.Kind != SyntaxKind.EmptyStatement))
                    valids.Add(patternP);
            }
            return valids;
        }

        public static TreeNode<SyntaxNodeOrToken> GetCurrentTree(object n)
        {
            var node = CurrentTrees[n];
            return node;
        }
    }
}