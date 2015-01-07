using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Change;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Comparator;
using System.Text.RegularExpressions;
using Spg.ExampleRefactoring.Position;
using Spg.ExampleRefactoring.Tok;
using DiGraph;
using Spg.ExampleRefactoring.Digraph;

namespace Spg.ExampleRefactoring.Expression
{
    /// <summary>
    /// Expression manager
    /// </summary>
    public class ExpressionManager
    {

        /* /// <summary>
         /// Filter expression
         /// </summary>
         /// <param name="expressions">Synthesized expressions</param>
         /// <param name="examples">Examples</param>
         public void FilterExpressions(Dictionary<Tuple<int, int>, List<IExpression>> expressions, List<Tuple<ListNode, ListNode>> examples)
         {
             foreach (KeyValuePair<Tuple<int, int>, List<IExpression>> entry in expressions)
             {
                 List<IExpression> removes = new List<IExpression>();
                 int i = 0;
                 foreach (IExpression expression in entry.Value)
                 {
                     SynthesizedProgram synthesizedProg = new SynthesizedProgram();
                     synthesizedProg.Add(expression);


                     Boolean isValid = ValidateExpression(synthesizedProg, examples);

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
         }*/

        /// <summary>
        /// Filter expression
        /// </summary>
        /// <param name="dag">Synthesized expressions</param>
        /// <param name="examples">Examples</param>
        public void FilterExpressions(Dag dag, List<Tuple<ListNode, ListNode>> examples)
        {
            Dictionary<Tuple<Vertex, Vertex>, List<IExpression>> expressions = dag.mapping;

            foreach (KeyValuePair<Tuple<Vertex, Vertex>, List<IExpression>> entry in expressions)
            {
                List<IExpression> removes = new List<IExpression>();
                int i = 0;
                foreach (IExpression expression in entry.Value)
                {
                    SynthesizedProgram synthesizedProg = new SynthesizedProgram();
                    synthesizedProg.Add(expression);

                    Boolean isValid = ValidateExpression(synthesizedProg, examples);

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
        /// <param name="hypothese">Expression to be tested</param>
        /// <param name="examples">Set of examples</param>
        /// <returns>True if expression match the examples, false otherwise</returns>
        public Boolean ValidateExpression(SynthesizedProgram hypothese, List<Tuple<ListNode, ListNode>> examples)
        {
            Boolean isValid = false;
            foreach (Tuple<ListNode, ListNode> example in examples)
            {
                //ASTProgram program = new ASTProgram();
                ListNode solution = ASTProgram.RetrieveNodes(example, hypothese.solutions);

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
