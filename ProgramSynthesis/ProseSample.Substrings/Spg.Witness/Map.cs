using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using TreeEdit.Spg.Print;
using TreeEdit.Spg.Script;
using TreeEdit.Spg.TreeEdit.Update;
using TreeElement.Spg.Node;
using TreeElement.Spg.Walker;

namespace ProseSample.Substrings.Spg.Witness
{
    public class Map
    {
        //public static SubsequenceSpec NodesMap(GrammarRule rule, int parameter, SubsequenceSpec spec)
        //{
        //    var linesExamples = new Dictionary<State, IEnumerable<object>>();
        //    foreach (State input in spec.ProvidedInputs)
        //    {
        //        var nodePrefix = spec.Examples[input].Cast<SyntaxNodeOrToken>();
        //        var tuple = (SyntaxNodeOrToken)input.Bindings.First().Value;

        //        var inpMapping = GetPair(tuple, nodePrefix);

        //        var linesContainingSelection = inpMapping;

        //        linesExamples[input] = linesContainingSelection;
        //    }
        //    return new SubsequenceSpec(linesExamples);
        //}


        public static SubsequenceSpec EditMap(GrammarRule rule, int parameter, SubsequenceSpec spec)
        {
            var editExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var edits = (List<Edit<SyntaxNodeOrToken>>) spec.Examples[input];
                var matches = new List<Node>();
                foreach (var v in edits)
                {
                    ITreeNode<SyntaxNodeOrToken> target;
                    if (v.EditOperation is Insert<SyntaxNodeOrToken> || v.EditOperation is Move<SyntaxNodeOrToken>)
                    {
                        //matches.Add(new Node(v.EditOperation.Parent));
                        target = v.EditOperation.Parent;
                    }
                    else
                    {
                        //matches.Add(new Node(v.EditOperation.T1Node));
                        target = v.EditOperation.T1Node;
                    }

                    var currentTree = WitnessFunctions.TreeUpdateDictionary[target.SyntaxTree].CurrentTree;
                    var targetNode = TreeUpdate.FindNode(currentTree, target.Value);
                    var dist = BFSWalker<SyntaxNodeOrToken>.Dist(targetNode);
                    var targetNodeHeight = ConverterHelper.TreeAtHeight(targetNode, dist, 1);
                    targetNodeHeight.SyntaxTree = target.SyntaxTree;
                    matches.Add(new Node(targetNodeHeight));
                }
                //var matches = edits.Select(e => new Node(e.EditOperation.Parent)).ToList();

                foreach (var edit in edits)
                {
                    //var key = input[rule.Body[0]];
                    var treeUp = WitnessFunctions.TreeUpdateDictionary[edit.EditOperation.Parent.SyntaxTree];
                    var previousTree = ConverterHelper.MakeACopy(treeUp.CurrentTree);
                    treeUp.ProcessEditOperation(edit.EditOperation);
                    

                    var modifieds = WitnessFunctions.TreeUpdateDictionary.Where(o => o.Value == treeUp);
                    foreach (var v in modifieds)
                    {
                        WitnessFunctions.CurrentTrees[v.Key] = previousTree;
                    }

                    Console.WriteLine("PREVIOUS TREE\n\n");
                    PrintUtil<SyntaxNodeOrToken>.PrintPretty(previousTree, "", true);
                    Console.WriteLine("UPDATED TREE\n\n");
                    PrintUtil<SyntaxNodeOrToken>.PrintPretty(treeUp.CurrentTree, "", true);
                }

                //foreach (var v in matches)
                //{
                //    v.SyntaxTree = WitnessFunctions.CurrentTrees[v.Value];
                //}

                editExamples[input] = matches;
            }
            return new SubsequenceSpec(editExamples);
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
