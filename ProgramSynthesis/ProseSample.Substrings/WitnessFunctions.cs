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
using Spg.ExampleRefactoring.LCS;
using Spg.ExampleRefactoring.RegularExpression;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactoring.Tok;
using TreeEdit.Spg.TreeEdit.Mapping;
using TreeEdit.Spg.TreeEdit.Script;
using TreeEdit.Spg.TreeEdit.Update;

namespace ProseSample.Substrings
{
    public static class WitnessFunctions
    {
        public static Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> Edits = new Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken>();

        #region literal
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

            var mats = new List<object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var v = (Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>)input[rule.Body[0]];
                var desiredOutput = (MatchResult)spec.Examples[input];

                Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken> tuple = Tuple.Create(v.Item1, desiredOutput.match.Item1);
                Tuple<ListNode, ListNode> lnode = ASTProgram.Example(tuple);

                TokenSeq seq = DymTokens(lnode.Item2.List);
                var matches = Regex.Matches(lnode.Item1, seq);

                foreach (var item in matches)
                {
                    if (item.Item2.Length() == 1)
                    {
                        mats.Add(item.Item2.List.Single());
                    }
                    else
                    {
                        SyntaxNodeOrToken parent = LCAManager.GetInstance().LeastCommonAncestor(item.Item2.List, item.Item2.List.First().SyntaxTree).AsNode();
                        mats.Add(parent);
                    }
                }

                treeExamples[input] = mats;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        /// <summary>
        /// Witness function for identifier
        /// </summary>
        /// <param name="rule">Identifier rule</param>
        /// <param name="parameter">Parameter in the identifier rule</param>
        /// <param name="spec">Example spefication</param>
        /// <returns>Disjunctive example with the string kind</returns>
        [WitnessFunction("Identifier", 0)]
        public static DisjunctiveExamplesSpec WitnessId(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return LiteralParameterBase(rule, parameter, spec, new List<SyntaxKind> { SyntaxKind.IdentifierToken, SyntaxKind.IdentifierName });
        }

        /// <summary>
        /// Predefined type witness function
        /// </summary>
        /// <param name="rule">Predefined type rule</param>
        /// <param name="parameter">Parameter related to the predefined type rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example with string for the predefined type</returns>
        [WitnessFunction("PredefinedType", 0)]
        public static DisjunctiveExamplesSpec WitnessPredefinedTypeId(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return LiteralParameterBase(rule, parameter, spec, new List<SyntaxKind> { SyntaxKind.PredefinedType });
        }

        /// <summary>
        /// Numeric expression witness function
        /// </summary>
        /// <param name="rule">numeric expression witness function rule</param>
        /// <param name="parameter">Parameter associated to the numeric expression witness function</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example with string representtion of numeric literals</returns>
        [WitnessFunction("NumericLiteralExpression", 0)]
        public static DisjunctiveExamplesSpec WitnessNumericLiteralId(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return LiteralParameterBase(rule, parameter, spec, new List<SyntaxKind> { SyntaxKind.NumericLiteralToken, SyntaxKind.NumericLiteralExpression });
        }

        /// <summary>
        /// Witness function for string literal expression
        /// </summary>
        /// <param name="rule">String literal expression rule</param>
        /// <param name="parameter">Parameter of the string literal expression rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive examples specification</returns>
        [WitnessFunction("StringLiteralExpression", 0)]
        public static DisjunctiveExamplesSpec WitnessStringLiteralId(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return LiteralParameterBase(rule, parameter, spec, new List<SyntaxKind> { SyntaxKind.StringLiteralToken, SyntaxKind.StringLiteralExpression });

        }

        //TODO refactor this method
        /// <summary>
        /// Witness function for string literal expression
        /// </summary>
        /// <param name="rule">String literal expression rule</param>
        /// <param name="parameter">Parameter of the string literal expression rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive examples specification</returns>
        [WitnessFunction("Block", 0)]
        public static DisjunctiveExamplesSpec BlockLiteralId(GrammarRule rule, int parameter,
            DisjunctiveExamplesSpec spec)
        {
            var idExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var strings = new List<object>();
                foreach (SyntaxNodeOrToken sot in spec.DisjunctiveExamples[input])
                {
                    var kinds = new List<SyntaxKind> {SyntaxKind.Block};
                    if (IsKind(sot, kinds) && !((BlockSyntax) sot).ChildNodes().Any())
                    {
                        strings.Add(sot.ToString());
                    }
                    else
                    {
                        return null;
                    }
                }
                idExamples[input] = strings;
            }
            return DisjunctiveExamplesSpec.From(idExamples);
        }

        /// <summary>
        /// Literal parameter base learner
        /// </summary>
        /// <param name="rule">Rule in the grammar</param>
        /// <param name="parameter">Rule parameter</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kinds">SyntaxKinds to be analized</param>
        /// <returns>Disjunctive example specification</returns>
        public static DisjunctiveExamplesSpec LiteralParameterBase(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, List<SyntaxKind> kinds)
        {
            var idExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var strings = new List<object>();
                foreach (SyntaxNodeOrToken sot in spec.DisjunctiveExamples[input])
                {
                    if (IsKind(sot, kinds))
                    {
                        strings.Add(sot.ToString());
                    }
                    else
                    {
                        return null;
                    }
                }
                idExamples[input] = strings;
            }

            return DisjunctiveExamplesSpec.From(idExamples);
        }

        /// <summary>
        /// Verify if toCompater is of one of the specified kinds.
        /// </summary>
        /// <param name="toCompater">Node to be compared</param>
        /// <param name="kinds">SyntaxKinds evaluated</param>
        /// <returns>True if toCompater is of one of the specified kinds.</returns>
        public static bool IsKind(SyntaxNodeOrToken toCompater, List<SyntaxKind> kinds)
        {
            foreach (var kind in kinds)
            {
                if (toCompater.IsKind(kind)) return true;
            }
            return false;
        }
        #endregion

        #region Concatenation
        /// <summary>
        /// C witness function for kind parameter with one child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="parameter">Parameter associated to C rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive examples specification with the kind of the nodes in the examples</returns>
        [WitnessFunction("C1", 1)]
        public static DisjunctiveExamplesSpec WitnessC1Kd(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kdExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var strings = new List<object>();
                foreach (MatchResult mt in spec.DisjunctiveExamples[input])
                {
                    SyntaxNodeOrToken sot = mt.match.Item1;

                    if (sot.IsToken) return null;

                    strings.Add(sot.Kind().ToString());
                }
                kdExamples[input] = strings;
            }

            return DisjunctiveExamplesSpec.From(kdExamples);
        }

        /// <summary>
        /// C witness function for kind parameter with two child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="parameter">Parameter associated to C rule</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjuntive examples specification with the kind of the nodes in the examples</returns>
        [WitnessFunction("C2", 1)]
        public static DisjunctiveExamplesSpec WitnessC2Kd(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return WitnessC1Kd(rule, parameter, spec);
        }

        /// <summary>
        /// C witness functino for expression parameter with one child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kind">Learned kind</param>
        /// <returns>Disjuntive examples specification</returns>
        [WitnessFunction("C1", 2, DependsOnParameters = new[] { 1 })]
        public static DisjunctiveExamplesSpec WitnessC1Expression1(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            return ConcatenationBase(spec, 1, 1);
        }

        /// <summary>
        /// C witness functino for expression parameter with two child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kind">Learned kind</param>
        /// <returns>Disjuntive examples specification</returns>
        [WitnessFunction("C2", 2, DependsOnParameters = new[] { 1 })]
        public static DisjunctiveExamplesSpec WitnessC2Expression1(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            return ConcatenationBase(spec, 2, 1);
        }

        /// <summary>
        /// C witness functino for expression parameter with two child
        /// </summary>
        /// <param name="rule">C rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kind">Learned kind</param>
        /// <param name="expression1">Specifiation for expression1</param>
        /// <returns>Disjuntive examples specification</returns>
        [WitnessFunction("C2", 3, DependsOnParameters = new[] { 1, 2 })]
        public static DisjunctiveExamplesSpec WitnessC2Expression2(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind, ExampleSpec expression1)
        {
            return ConcatenationBase(spec, 2, 2);
        }

        /// <summary>
        /// Concatenation learner base
        /// </summary>
        /// <param name="spec">Example specification</param>
        /// <param name="desiredIndex">Number of children considered</param>
        /// <param name="returnIndex">Target child</param>
        /// <returns>Disjunctive example specification</returns>
        private static DisjunctiveExamplesSpec ConcatenationBase(DisjunctiveExamplesSpec spec, int desiredIndex, int returnIndex)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();

                foreach (MatchResult ma in spec.DisjunctiveExamples[input])
                {
                    SyntaxNodeOrToken sot = ma.match.Item1;

                    if (sot.IsToken) return null;

                    var lsot = ExtractChildren(sot);

                    if (lsot.Count != desiredIndex) return null;

                    Tuple<SyntaxNodeOrToken, Bindings> t = Tuple.Create(lsot[returnIndex - 1], new Bindings());
                    MatchResult m = new MatchResult(t);

                    matches.Add(m);
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
        #endregion

        #region script

        //TODO refactor script parameter methods
        /// <summary>
        /// Witness function for script rule
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Rule parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Script1", 1)]
        public static DisjunctiveExamplesSpec WitnessFunctionScript1Kd(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            var kMatches = new List<object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var inp = (Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>) input[rule.Body[0]];
                var outTree = (SyntaxNodeOrToken)spec.Examples[input];
                var inpTree = GetPair(inp.Item1, outTree);

                ITreeMapping mapping = new GumTreeMapping();
                var M = mapping.Mapping(inpTree, outTree);

                var generator = new EditScriptGenerator();
                var script = generator.EditScript(inpTree, outTree, M);

                TreeUpdate treeUp = new TreeUpdate();
                treeUp.PreProcessTree(script, inpTree, M);

                foreach (var item in script)
                {
                    if (!treeUp._processed.ContainsKey(item))
                    {
                        //var changedNodeList = treeUp.CurrentTree.AsNode().GetAnnotatedNodes(treeUp._ann[item]).ToList();
                        //var oldNode = changedNodeList.First();
                        treeUp.ProcessEditOperation(item);
                        var changedNodeList = treeUp.CurrentTree.AsNode().GetAnnotatedNodes(treeUp._ann[item]).ToList();
                        var newNode = changedNodeList.First();
                        kMatches.Add(newNode.Kind().ToString());
                        //Edits.Add(oldNode, newNode);
                    }
                }

                if (kMatches.Count == 1)
                {
                    kExamples[input] = kMatches;
                }
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }

        private static SyntaxNodeOrToken GetPair(SyntaxNodeOrToken item1, SyntaxNodeOrToken outTree)
        {
            //Todo refactor this function
            SyntaxNode node = item1.AsNode();

            var l = from nm in node.DescendantNodes()
                where nm.IsKind(outTree.Kind())
                select nm;

            MethodDeclarationSyntax m = (MethodDeclarationSyntax) outTree;
            foreach (var item in l)
            {
                MethodDeclarationSyntax mItem = (MethodDeclarationSyntax) item;
                if (m.Identifier.ToString().Equals(mItem.Identifier.ToString()))
                {
                    return mItem;
                }
            }

            return null;
        }

        /// <summary>
        /// Witness function for script rule
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Rule parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <param name="kind">Kind disjunctive examples</param>
        /// <returns>Disjunctive example specification</returns>
        [WitnessFunction("Script1", 2, DependsOnParameters = new[] {1})]
        public static DisjunctiveExamplesSpec WitnessFunctionScript1Edit(GrammarRule rule, int parameter, ExampleSpec spec,  ExampleSpec kind)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            var kMatches = new List<object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var inp = (Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>)input[rule.Body[0]];
                var outTree = (SyntaxNodeOrToken)spec.Examples[input];
                var inpTree = GetPair(inp.Item1, outTree);

                ITreeMapping mapping = new GumTreeMapping();
                var M = mapping.Mapping(inpTree, outTree);

                var generator = new EditScriptGenerator();
                var script = generator.EditScript(inpTree, outTree, M);

                TreeUpdate treeUp = new TreeUpdate();
                treeUp.PreProcessTree(script, inpTree, M);

                foreach (var item in script)
                {
                    if (!treeUp._processed.ContainsKey(item))
                    {
                        var changedNodeList = treeUp.CurrentTree.AsNode().GetAnnotatedNodes(treeUp._ann[item]).ToList();
                        var oldNode = changedNodeList.First();
                        treeUp.ProcessEditOperation(item);
                        changedNodeList = treeUp.CurrentTree.AsNode().GetAnnotatedNodes(treeUp._ann[item]).ToList();
                        var newNode = changedNodeList.First();
                        kMatches.Add(newNode);
                        Edits.Add(newNode, oldNode);
                    }
                }

                if (kMatches.Count() == 1)
                {
                    kExamples[input] = kMatches;
                }
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }
        #endregion

        #region insert

        //TODO refactor insert parameter methods (tip: dynamic programmig)
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
            var kMatches = new List<object>();
            foreach (State input in spec.ProvidedInputs)
            {
                //TODO refactor this consider more examples
                var inp = (Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>)input[rule.Body[0]];
                var outTree = (SyntaxNode)spec.DisjunctiveExamples[input].First();
                var inpTree = Edits[outTree];

                ITreeMapping mapping = new GumTreeMapping();
                var M = mapping.Mapping(inpTree, outTree);

                var generator = new EditScriptGenerator();
                var script = generator.EditScript(inpTree, outTree, M);

                TreeUpdate treeUp = new TreeUpdate();
                treeUp.PreProcessTree(script, inpTree, M);

                foreach (var item in script)
                {
                    if (!treeUp._processed.ContainsKey(item))
                    {
                        //var changedNodeList = treeUp.CurrentTree.AsNode().GetAnnotatedNodes(treeUp._ann[item]).ToList();
                        //var oldNode = changedNodeList.First();
                        treeUp.ProcessEditOperation(item);
                        //changedNodeList = treeUp.CurrentTree.AsNode().GetAnnotatedNodes(treeUp._ann[item]).ToList();
                        //var newNode = changedNodeList.First();
                        kMatches.Add(item.K);
                    }
                }

                if (kMatches.Count == 1)
                {
                    kExamples[input] = kMatches;
                }
            }
            return DisjunctiveExamplesSpec.From(kExamples);
            //var kExamples = new Dictionary<State, IEnumerable<object>>();

            //foreach (State input in spec.ProvidedInputs)
            //{
            //    var kMatches = new List<object>();
            //    foreach (var item in spec.DisjunctiveExamples[input])
            //    {
            //        string type = item.GetType().ToString();
            //        SyntaxNode sot = (SyntaxNode) item;
            //        //if (sot.IsToken) return null;

            //        SyntaxNodeOrToken inp = (SyntaxNodeOrToken)input[rule.Body[0]];

            //        SyntaxNode nodeIn = inp.AsNode();
            //        SyntaxNode nodeOut = sot;//.AsNode();

            //        var childrenIn = nodeIn.ChildNodes().ToList();
            //        var childrenOut = nodeOut.ChildNodes().ToList();

            //        var dic = new List<List<int>>();

            //        for (int i = 0; i < childrenOut.Count(); i++)
            //        {
            //            dic.Add(new List<int>());
            //            var outi = childrenOut.ElementAt(i);

            //            for (int j = 0; j < childrenIn.Count(); j++)
            //            {
            //                var inj = childrenIn.ElementAt(j);
            //                if (IsomorphicManager.IsIsomorphic(outi, inj))
            //                {
            //                    dic[i].Add(j);
            //                }
            //            }
            //        }

            //        int cco = 1;
            //        foreach (var i in dic)
            //        {
            //            if (!i.Any())
            //            {
            //                kMatches.Add(cco);
            //            }
            //            //TODO: what happens a single element from the output matches more than one element in the input.
            //            cco++;
            //        }
            //    }
            //    kExamples[input] = kMatches;
            //}

            //return DisjunctiveExamplesSpec.From(kExamples);
        }


        /// <summary>
        /// Witness function for parater k in the insert operator
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Examples specification</param>
        /// <param name="kBiding">kBinding</param>
        /// <returns></returns>
        [WitnessFunction("Insert", 2, DependsOnParameters = new[] { 1 })]
        public static DisjunctiveExamplesSpec WitnessFunctionInsertParent(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kBiding)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            var kMatches = new List<object>();
            foreach (State input in spec.ProvidedInputs)
            {
                //TODO refactor this consider more examples
                var inp = (Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>)input[rule.Body[0]];
                var outTree = (SyntaxNode)spec.DisjunctiveExamples[input].First();
                var inpTree = Edits[outTree];

                ITreeMapping mapping = new GumTreeMapping();
                var M = mapping.Mapping(inpTree, outTree);

                var generator = new EditScriptGenerator();
                var script = generator.EditScript(inpTree, outTree, M);

                TreeUpdate treeUp = new TreeUpdate();
                treeUp.PreProcessTree(script, inpTree, M);

                foreach (var item in script)
                {
                    if (!treeUp._processed.ContainsKey(item))
                    {
                        var changedNodeList = treeUp.CurrentTree.AsNode().GetAnnotatedNodes(treeUp._ann[item]).ToList();
                        SyntaxNodeOrToken oldNode = changedNodeList.First();
                        treeUp.ProcessEditOperation(item);
                        MatchResult result = new MatchResult(Tuple.Create(oldNode, new Bindings()));
                        kMatches.Add(result);
                    }
                }

                if (kMatches.Count() == 1)
                {
                    kExamples[input] = kMatches;
                }
            }
            return DisjunctiveExamplesSpec.From(kExamples);
            //var kExamples = new Dictionary<State, IEnumerable<object>>();

            //foreach (State input in spec.ProvidedInputs)
            //{
            //    var kMatches = new List<object>();
            //    foreach (SyntaxNodeOrToken sot in spec.DisjunctiveExamples[input])
            //    {

            //        if (sot.IsToken) return null;

            //        SyntaxNodeOrToken inp = (SyntaxNodeOrToken)input[rule.Body[0]];

            //        Tuple<SyntaxNodeOrToken, Bindings> t = Tuple.Create(inp, new Bindings());
            //        MatchResult match = new MatchResult(t);
            //        kMatches.Add(match);
            //    }

            //    kExamples[input] = kMatches;
            //}

            //return DisjunctiveExamplesSpec.From(kExamples);
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
            var kMatches = new List<object>();
            foreach (State input in spec.ProvidedInputs)
            {
                //TODO refactor this consider more examples
                var inp = (Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>)input[rule.Body[0]];
                var outTree = (SyntaxNode)spec.DisjunctiveExamples[input].First();
                var inpTree = Edits[outTree];

                ITreeMapping mapping = new GumTreeMapping();
                var M = mapping.Mapping(inpTree, outTree);

                var generator = new EditScriptGenerator();
                var script = generator.EditScript(inpTree, outTree, M);

                TreeUpdate treeUp = new TreeUpdate();
                treeUp.PreProcessTree(script, inpTree, M);

                foreach (var item in script)
                {
                    if (!treeUp._processed.ContainsKey(item))
                    {
                        treeUp.ProcessEditOperation(item);
                        var changedNodeList = treeUp.CurrentTree.AsNode().GetAnnotatedNodes(treeUp._ann[item]).ToList();
                        var newNode = changedNodeList.First();
                        SyntaxNodeOrToken result = newNode.ChildNodes().ElementAt(item.K - 1);
                        kMatches.Add(result);
                    }
                }

                if (kMatches.Count() == 1)
                {
                    kExamples[input] = kMatches;
                }
            }
            return DisjunctiveExamplesSpec.From(kExamples);
            //var kExamples = new Dictionary<State, IEnumerable<object>>();

            //foreach (State input in spec.ProvidedInputs)
            //{
            //    var kMatches = new List<object>();
            //    var k = (int)kBinding.Examples[input];

            //    var mat = (MatchResult)parentBinding.Examples[input];

            //    var parent = mat.match.Item1;

            //    SyntaxNodeOrToken match = parent.AsNode().ChildNodes().ElementAt(k - 1);

            //    kMatches.Add(match);

            //    kExamples[input] = kMatches;
            //}

            //return DisjunctiveExamplesSpec.From(kExamples);
        }
        #endregion

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

                    SyntaxKind kind = sot.Kind();
                    var match = kind.ToString();

                    kMatches.Add(match);
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
                var sot = (SyntaxNodeOrToken)spec.Examples[input];

                mats.Add(sot);

                treeExamples[input] = mats;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        #endregion

        [WitnessFunction("NodesMap", 1)]
        public static PrefixSpec WitnessLinesMap(GrammarRule rule, int parameter,
                                                 PrefixSpec spec)
        {
            var linesExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                //Todo refactor
                //var source = ((SyntaxNodeOrToken)input[rule.Grammar.InputSymbol]);
                var nodePrefix = spec.Examples[input].Cast<SyntaxNodeOrToken>();
                var tuple = (Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>) input.Bindings.First().Value;

                var inpMapping = GetPair(tuple.Item1, nodePrefix);

                var linesContainingSelection = inpMapping;

                linesExamples[input] = linesContainingSelection;
            }
            return new PrefixSpec(linesExamples);
        }

        private static List<object> GetPair(SyntaxNodeOrToken item1, IEnumerable<SyntaxNodeOrToken> nodePrefix)
        {
            var result = new List<object>();

            foreach (var item in nodePrefix)
            {
                var node = GetPair(item1, item);
                result.Add(node);
            }

            return result;
        }

        #region Utilities

        //TODO remove dynamic token method
        /// <summary>
        /// Convert list in dynamic tokens
        /// </summary>
        /// <param name="list">List of tokens</param>
        /// <returns>Dynamic tokens</returns>
        private static TokenSeq DymTokens(List<SyntaxNodeOrToken> list)
        {
            var tokens = new List<Spg.ExampleRefactoring.Tok.Token>();
            foreach (var item in list)
            {
                RawDymToken t = new RawDymToken(item);
                tokens.Add(t);
            }

            TokenSeq seq = new TokenSeq(tokens);
            return seq;
        }

        #endregion
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