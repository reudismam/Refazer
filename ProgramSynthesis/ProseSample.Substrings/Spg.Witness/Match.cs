using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using TreeEdit.Spg.Isomorphic;
using TreeEdit.Spg.Match;
using TreeElement;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Witness
{
    public class Match
    {
        public static DisjunctiveExamplesSpec TreeKindRef(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return spec;
        }

        public static ExampleSpec CKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            //var pattern = MatchPattern(rule, parameter, spec);
            var kdExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var syntaxKinds = new List<SyntaxKind>();
                foreach (ITreeNode<Token> node in spec.DisjunctiveExamples[input])
                {
                    var sot = node.Value;
                    syntaxKinds.Add(sot.Kind);
                    if (!sot.Kind.Equals(syntaxKinds.First())) return null;
                }
                kdExamples[input] = syntaxKinds.First();
            }
            return new ExampleSpec(kdExamples);
        }

        public static DisjunctiveExamplesSpec CChildren(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<List<ITreeNode<Token>>>();
                foreach (ITreeNode<Token> node in spec.DisjunctiveExamples[input])
                {
                    //var sot = node.Value;
                    //if (sot.Value.IsToken) return null;
                    if (!node.Children.Any()) return null;

                    var lsot = ExtractChildren(node);
                    //var childList = lsot.Select(item => new Node(item)).ToList();
                    //childList.ForEach(item => item.Value.SyntaxTree = sot.SyntaxTree);
                    matches.Add(lsot);
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
        private static List<ITreeNode<Token>> ExtractChildren(ITreeNode<Token> parent)
        {
            return parent.Children;
        }

        public static DisjunctiveExamplesSpec MatchPattern(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            var patterns = new List<ITreeNode<Token>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<ITreeNode<SyntaxNodeOrToken>>();
                foreach (Node node in spec.DisjunctiveExamples[input])
                {
                    var sot = node.Value;
                    matches.Add(sot);
                }
                var pattern = ConverterHelper.ConvertITreeNodeToToken(matches.First());
                patterns.Add(pattern);
            }

            var commonPattern = BuildPattern(patterns.First(), patterns.Last());
            foreach(State input in spec.ProvidedInputs)
            {
                eExamples[input] = new List<ITreeNode<Token>> {ConverterHelper.MakeACopy(commonPattern.Tree)};
            }        
            return DisjunctiveExamplesSpec.From(eExamples);
        }

        public static Pattern BuildPattern(ITreeNode<Token> t1, ITreeNode<Token> t2)
        {
            var token = new Token(t1.Value.Kind);
            var itreeNode = new TreeNode<Token>(token, new TLabel(token.Kind));
            Pattern pattern = new Pattern(itreeNode);
            if (t1.Value.Kind != t2.Value.Kind) return null;
            if (!t1.Children.Any() || !t2.Children.Any())
            {
                if (!t1.Children.Any() && !t2.Children.Any() && t1.Value is DynToken && t2.Value is DynToken)
                {
                    var node1 = ConverterHelper.ConvertCSharpToTreeNode((t1.Value as DynToken).Value);
                    var node2 = ConverterHelper.ConvertCSharpToTreeNode((t2.Value as DynToken).Value);
                    if (IsomorphicManager<SyntaxNodeOrToken>.IsIsomorphic(node1, node2))
                    {
                        var dtoken = new DynToken(t1.Value.Kind, node1.Value);
                        var ditreeNode = new TreeNode<Token>(dtoken, new TLabel(dtoken.Kind));
                        return new Pattern(ditreeNode);
                    }
                }
                //var ltoken = new LeafToken(t1.Value.Kind);
                //var litreenode = new TreeNode<Token>(ltoken, new TLabel(token.Kind));
                //return new Pattern(litreenode);
                return pattern;
            }


            if (t1.Children.Count == t2.Children.Count)
            {
                for (int j = 0; j < t1.Children.Count; j++)
                {
                    Pattern patternChild = BuildPattern(t1.Children.ElementAt(j), t2.Children.ElementAt(j));
                    if (patternChild == null) return null;
                    pattern.Tree.AddChild(patternChild.Tree, j);
                }
            }
            return pattern;
        }

        public static DisjunctiveExamplesSpec MatchK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, DisjunctiveExamplesSpec kind)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                
                var pattern = (ITreeNode<Token>) kind.DisjunctiveExamples[input].First();
                var mats = new List<object>();
                foreach (Node node in spec.DisjunctiveExamples[input])
                {
                    var currentTree = WitnessFunctions.GetCurrentTree(node.Value.SyntaxTree);
                    var matches = PatterMatches(currentTree, pattern);

                    for (int i = 0; i < matches.Count; i++)
                    {
                        var item = matches[i];
                        if (node.Value.Equals(item))
                        {
                            mats.Add(i + 1);
                        }
                    }
                }

                if (!mats.All(o => o.Equals(mats.First()))) return null;
                kExamples[input] = mats;
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }


        /// <summary>
        /// Abstract match
        /// </summary>
        /// <param name="inpTree">Input tree</param>
        /// <param name="pattern">Syntax node or token to be matched.</param>
        /// <returns>Abstract match</returns>
        public static List<ITreeNode<SyntaxNodeOrToken>> PatterMatches(ITreeNode<SyntaxNodeOrToken> inpTree, ITreeNode<Token> pattern)
        {
            //todo Refactor this method putting it on a adequate class.
            var nodes = from item in inpTree.DescendantNodes() where Semantics.IsValue(item, pattern) select item;
            return nodes.ToList();
        }
    }
}
