using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using DiGraph;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Digraph;
using Spg.ExampleRefactoring.Expression;
using Spg.ExampleRefactoring.Synthesis;
using LeastCommonAncestor;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Comparator;

namespace Spg.ExampleRefactoring.Expression
{
    /// <summary>
    /// Expression manager
    /// </summary>
    public class ExpressionManager
    {
        /// <summary>
        /// Filter expression
        /// </summary>
        /// <param name="dag">Synthesized expressions</param>
        /// <param name="examples">Examples</param>
        public void FilterExpressions(Dag dag, List<Tuple<ListNode, ListNode>> examples)
        {
            if (dag == null) throw new ArgumentNullException("dag");
            if (examples == null) throw new ArgumentNullException("examples");
            if(!examples.Any()) throw new ArgumentException("Examples cannot be null");

            Dictionary<Tuple<Vertex, Vertex>, Dictionary<ExpressionKind, List<IExpression>>> expressions = dag.Mapping;

            foreach (KeyValuePair<Tuple<Vertex, Vertex>, Dictionary<ExpressionKind, List<IExpression>>> entry in expressions)
            {
                Dictionary<ExpressionKind, List<IExpression>> removes = new Dictionary<ExpressionKind, List<IExpression>>();
                int i = 0;
                foreach (KeyValuePair<ExpressionKind, List<IExpression>> item in entry.Value)
                {
                    foreach (IExpression expression in item.Value)
                    {
                        bool isValid = ValidateExpression(entry.Key, expression, examples);

                        if (!isValid && !item.Key.Equals(ExpressionKind.FakeConstrStr))
                        {
                            List<IExpression> value;
                            if (!removes.TryGetValue(item.Key, out value))
                            {
                                removes.Add(item.Key, new List<IExpression>());
                            }
                            removes[item.Key].Add(expression);
                        }
                        i++;
                    }
                }

                if (entry.Value.Count != 1)
                {
                    if (entry.Value.ContainsKey(ExpressionKind.FakeConstrStr))
                    {
                        List<IExpression> value;
                        if (!removes.TryGetValue(ExpressionKind.FakeConstrStr, out value))
                        {
                            removes.Add(ExpressionKind.FakeConstrStr, new List<IExpression>());
                        }
                        removes[ExpressionKind.FakeConstrStr].Add(entry.Value[ExpressionKind.FakeConstrStr].First());
                    }
                }

                foreach (KeyValuePair<ExpressionKind, List<IExpression>> item in removes)
                {
                    foreach (IExpression expression in item.Value)
                    {
                        entry.Value[item.Key].Remove(expression);
                        if (entry.Value[item.Key].Count == 0)
                        {
                            expressions[entry.Key].Remove(item.Key);
                        }
                    }
                }               
            }
        }

        /// <summary>
        /// Validate an expression in function of the examples
        /// </summary>
        /// <param name="expression">Expression to be tested</param>
        /// <param name="examples">Set of examples</param>
        /// <returns>True if expression match the examples, false otherwise</returns>
        public bool ValidateExpression(Tuple<Vertex, Vertex> edge, IExpression expression, List<Tuple<ListNode, ListNode>> examples)
        {
//            if (expression is FakeConstrStr) return true;//FakeConstruStr is not present on al output entries
            var firstVertex = edge.Item1.Id.Split(':');
            var secondVertex = edge.Item2.Id.Split(':');

            SynthesizedProgram syntheProg = new SynthesizedProgram();
            syntheProg.Add(expression);

            bool isValid = false;
            for(int i = 0; i < examples.Count; i++)
            {
                Tuple<ListNode, ListNode> example = examples[i];
                ListNode solution = ASTProgram.RetrieveNodes(example, syntheProg.Solutions);
                int position1 = Convert.ToInt32(firstVertex[i]);
                int position2 = Convert.ToInt32(secondVertex[i]);
                ListNode sot = ASTManager.SubNotes(example.Item2, position1, (position2 - position1));

                if (solution != null && new NodeComparer().SequenceEqual(sot, solution))
                {
                    isValid = true;
                }
                else
                {
                    return false;
                }
            }
            return isValid;
        }

        /// <summary>
        /// Validate an expression in function of the examples
        /// </summary>
        /// <param name="expression">Expression to be tested</param>
        /// <param name="examples">Set of examples</param>
        /// <returns>True if expression match the examples, false otherwise</returns>
        public bool ValidateExpression(IExpression expression, List<Tuple<ListNode, ListNode>> examples)
        {
            SynthesizedProgram syntheProg = new SynthesizedProgram();
            syntheProg.Add(expression);

            bool isValid = false;
            foreach (Tuple<ListNode, ListNode> example in examples)
            {
                ListNode solution = ASTProgram.RetrieveNodes(example, syntheProg.Solutions);

                if (solution != null && ASTManager.Matches(example.Item2, solution, new NodeComparer()).Count > 0)
                {
                    isValid = true;
                }
                else
                {
                    return false;
                }
            }
            return isValid;
        }
    }
}




