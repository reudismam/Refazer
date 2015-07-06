using System;
using System.Collections.Generic;
using System.Linq;
using DiGraph;
using ExampleRefactoring.Spg.ExampleRefactoring.AST;
using ExampleRefactoring.Spg.ExampleRefactoring.Digraph;
using ExampleRefactoring.Spg.ExampleRefactoring.Expression;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
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

            Dictionary<Tuple<Vertex, Vertex>, Dictionary<string, List<IExpression>>> expressions = dag.Mapping;

            foreach (KeyValuePair<Tuple<Vertex, Vertex>, Dictionary<string, List<IExpression>>> entry in expressions)
            {
                Dictionary<string, List<IExpression>> removes = new Dictionary<string, List<IExpression>>();
                int i = 0;
                foreach (KeyValuePair<string, List<IExpression>> item in entry.Value)
                {
                    foreach (IExpression expression in item.Value)
                    {
                        //bool isValid = ValidateExpression(expression, examples);
                        bool isValid = ValidateExpression(entry.Key, expression, examples);

                        if (!isValid)
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

                foreach (KeyValuePair<string, List<IExpression>> item in removes)
                {
                    foreach (IExpression expression in item.Value)
                    {
                        entry.Value[item.Key].Remove(expression);
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


