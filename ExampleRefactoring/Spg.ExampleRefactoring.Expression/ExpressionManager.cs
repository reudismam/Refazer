using System;
using System.Collections.Generic;
using System.Linq;
using DiGraph;
using ExampleRefactoring.Spg.ExampleRefactoring.AST;
using ExampleRefactoring.Spg.ExampleRefactoring.Digraph;
using ExampleRefactoring.Spg.ExampleRefactoring.Expression;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Digraph;
using Spg.ExampleRefactoring.Synthesis;

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

            Dictionary<Tuple<Vertex, Vertex>, List<IExpression>> expressions = dag.Mapping;

            foreach (KeyValuePair<Tuple<Vertex, Vertex>, List<IExpression>> entry in expressions)
            {
                List<IExpression> removes = new List<IExpression>();
                int i = 0;
                foreach (IExpression expression in entry.Value)
                {
                    bool isValid = ValidateExpression(expression, examples);

                    if (!isValid)
                    {
                        removes.Add(expression);
                    }
                    i++;
                }

                foreach (IExpression expression in removes)
                {
                    entry.Value.Remove(expression);
                }
            }
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


