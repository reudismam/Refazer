using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DiGraph;
using LCS2;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Expression;
using Spg.ExampleRefactoring.Position;
using Spg.ExampleRefactoring.Setting;
using Spg.LocationRefactoring.Tok;

namespace Spg.ExampleRefactoring.Synthesis
{
    /// <summary>
    /// Synthesis manager
    /// </summary>
    public class SynthesisManager
    {
        /// <summary>
        /// Settings
        /// </summary>
        /// <returns>Get or set synthesis setting</returns>
        public SynthesizerSetting setting { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SynthesisManager()
        {
            this.setting = new SynthesizerSetting();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="setting">Setting</param>
        public SynthesisManager(SynthesizerSetting setting)
        {
            this.setting = setting;
        }

        /// <summary>
        /// Create new synthesized program combining current synthesized programs with expressions
        /// </summary>
        /// <param name="synthesizedProgramList">Synthesized programs list</param>
        /// <param name="expressionList">Expression list</param>
        /// <returns>Synthesized programs list</returns>
        private static List<SynthesizedProgram> CombSynthProgramExp(List<SynthesizedProgram> synthesizedProgramList, List<IExpression> expressionList)
        {
            List<SynthesizedProgram> synthesizedProgList = new List<SynthesizedProgram>();
            if (synthesizedProgramList.Count > 0)
            {
                foreach (SynthesizedProgram sd1 in synthesizedProgramList)
                {
                    foreach (IExpression e2 in expressionList)
                    {
                        List<IExpression> solutions = new List<IExpression>(sd1.solutions);
                        SynthesizedProgram sp = new SynthesizedProgram();
                        sp.solutions = solutions;
                        sp.Add(e2);

                        synthesizedProgList.Add(sp);
                    }
                }
                return synthesizedProgList;
            }

            foreach (IExpression expression in expressionList)
            {
                SynthesizedProgram sp = new SynthesizedProgram();
                sp.Add(expression);
                synthesizedProgList.Add(sp);
            }
            return synthesizedProgList;
        }

        /// <summary>
        /// Create new synthesized program combining current synthesized programs with expressions
        /// </summary>
        /// <param name="synthesizedProgramList">Synthesized programs list</param>
        /// <param name="expressionList">Expression list</param>
        /// <param name="examples">Examples</param>
        /// <returns>Synthesized programs list</returns>
        private List<SynthesizedProgram> CombSynthProgramExp(List<SynthesizedProgram> synthesizedProgramList, List<IExpression> expressionList, List<Tuple<ListNode, ListNode>> examples)
        {
            List<SynthesizedProgram> synthesizedProgList = new List<SynthesizedProgram>();
            if (synthesizedProgramList.Count > 0)
            {
                foreach (SynthesizedProgram sd1 in synthesizedProgramList)
                {
                    foreach (IExpression e2 in expressionList)
                    {
                        List<IExpression> solutions = new List<IExpression>(sd1.solutions);
                        SynthesizedProgram sp = new SynthesizedProgram();
                        sp.solutions = solutions;
                        sp.Add(e2);

                        //ExpressionManager manager = new ExpressionManager();
                        //bool isValid = manager.ValidateExpression(sp, examples);
                        //if (isValid)
                        //{
                            synthesizedProgList.Add(sp);
                        //}
                    }
                }
                return synthesizedProgList;
            }

            foreach (IExpression expression in expressionList)
            {
                SynthesizedProgram sp = new SynthesizedProgram();
                sp.Add(expression);
                synthesizedProgList.Add(sp);
            }
            return synthesizedProgList;
        }

      

        /// <summary>
        /// Expression list that forms the synthesized program
        /// </summary>
        /// <param name="mapping">Expressions mapping</param>
        /// <param name="indexs">Synthesized program sub parts</param>
        /// <returns>Expression list that forms the synthesized program</returns>
        private static List<List<IExpression>> SynthesizedPrograms(Dictionary<Tuple<Vertex, Vertex>, List<IExpression>> mapping, List<Vertex> indexs)
        {
            List<List<IExpression>> synthProgList = new List<List<IExpression>>();
            for (int i = 1; i < indexs.Count; i++)
            {
                Tuple<Vertex, Vertex> tuple = Tuple.Create(indexs[i - 1], indexs[i]);
                synthProgList.Add(mapping[tuple]);
            }
            return synthProgList;
        }

        /// <summary>
        /// Validate synthesized program
        /// </summary>
        /// <param name="solutions">Synthesized program expressions list</param>
        /// <param name="examples">Examples</param>
        /// <returns>True if valid</returns>
        private bool ValidateSynthesizedProgram(List<IExpression> solutions, List<Tuple<ListNode, ListNode>> examples/*, StreamWriter file*/)
        {
            if (setting.considerConstrStr && solutions.Count == 1 && solutions[0] is ConstruStr)
            {
                return false;
            }

            foreach (Tuple<ListNode, ListNode> example in examples)
            {
                ListNode match = ASTProgram.RetrieveNodes(example, solutions);
                ListNode output = example.Item2;

                bool equals = new NodeComparer().SequenceEqual(match, output);
                if (!equals)
                {
                    return false;
                }
            }
            return true;
        }

        
        /// <summary>
        /// Create an instance on dataset using solution features
        /// </summary>
        /// <param name="solution">Synthesized program expression</param>
        /// <returns>
        /// Occurrence number of each feature
        /// </returns>
        public static Dictionary<FeatureType, int> CreateInstance(List<IExpression> solution)
        {
            Dictionary<FeatureType, int> features = new Dictionary<FeatureType, int>();
            InitFeatures(features);
            foreach (IExpression expression in solution)
            {
                if (expression is ConstruStr)
                {
                    Handle(features, FeatureType.CONSTSTR);
                }
                else
                {
                    SubStr subStr = (SubStr)expression;

                    IPosition p1 = subStr.p1;
                    IPosition p2 = subStr.p2;

                    EvaluatePos(p1, features);
                    EvaluatePos(p2, features);
                }
            }

            features.Add(FeatureType.SIZE, solution.Count());

            return features;

        }

        /// <summary>
        /// Start features to default values
        /// </summary>
        /// <param name="features">features</param>
        private static void InitFeatures(Dictionary<FeatureType, int> features)
        {
            features.Add(FeatureType.CONSTSTR, 0);
            features.Add(FeatureType.POS, 0);
            features.Add(FeatureType.CPOSBEGIN, 0);
            features.Add(FeatureType.CPOSEND, 0);
            features.Add(FeatureType.CPOS, 0);
            features.Add(FeatureType.EMPTY, 0);
            features.Add(FeatureType.SYNTAX, 0);
            //features.Add(SIZE, 0);
        }

        private static void EvaluatePos(IPosition p1, Dictionary<FeatureType, int> features)
        {
            if (p1 is Pos)
            {
                Handle(features, FeatureType.POS);
                Pos p = p1 as Pos;
                HandleEmpty(features, p);
                HandleSyntax(features, p);
                HandleDymToken(features, p);
            }
            else
            {
                CPos cpos = (CPos)p1;

                int position = cpos.position;

                switch (position)
                {
                    case 0:
                        Handle(features, FeatureType.CPOSBEGIN);
                        break;
                    case -1:
                        Handle(features, FeatureType.CPOSEND);
                        break;
                    default:
                        Handle(features, FeatureType.CPOS);
                        break;
                }
            }
        }

        private static void HandleDymToken(Dictionary<FeatureType, int> features, Pos pos)
        {
            int value;
            FeatureType key = FeatureType.DYMTOKEN;

            if (!features.TryGetValue(key, out value))
            {
                features.Add(key, 0);
                value = 0;
            }

            int count = 0;
            for (int i = 0; i < pos.r1.Tokens.Count; i++)
            {
                if (pos.r1.Tokens[i] is DymToken)
                {
                    count++;
                }
            }

            for (int i = 0; i < pos.r2.Tokens.Count; i++)
            {
                if (pos.r2.Tokens[i] is DymToken)
                {
                    count++;
                }
            }
            features[key] += count;
        }

        private static void HandleSyntax(Dictionary<FeatureType, int> features, Pos pos)
        {
            int value;
            FeatureType key = FeatureType.SYNTAX;

            if (!features.TryGetValue(key, out value))
            {
                features.Add(key, 0);
                value = 0;
            }

            int count = 0;
            for (int i = 0; i < pos.r1.Tokens.Count; i++)
            {
                if (pos.r1.Tokens[i].token.AsNode() != null)
                {
                    count++;
                }
            }

            for (int i = 0; i < pos.r2.Tokens.Count; i++)
            {
                if (pos.r2.Tokens[i].token.AsNode() != null)
                {
                    count++;
                }
            }
            features[key] += count;
        }

        private static void HandleEmpty(Dictionary<FeatureType, int> features, Pos p1)
        {

            int value;
            FeatureType key = FeatureType.EMPTY;


            if (!features.TryGetValue(key, out value))
            {
                features.Add(key, 0);
                value = 0;
            }

            if (p1.r1.Length() == 0)
            {
                features[key] = value + 1;
            }

            if (p1.r2.Length() == 0)
            {
                features[key] = value + 1;
            }

        }

        private static void Handle(Dictionary<FeatureType, int> features, FeatureType key)
        {
            int value;

            if (!features.TryGetValue(key, out value))
            {
                features.Add(key, 0);
                value = 0;
            }

            features[key] = value + 1;
        }

        /// <summary>
        /// Filter hypothesis
        /// </summary>
        /// <param name="mapping">Dag</param>
        /// <param name="solutions">Solution</param>
        /// <param name="examples">Examples</param>
        /// <returns>A valid hypothesis, if it exits, null otherwise</returns>
        public SynthesizedProgram FilterASTPrograms(Dictionary<Tuple<Vertex, Vertex>, List<IExpression>> mapping, List<Vertex> solutions, List<Tuple<ListNode, ListNode>> examples)
        {
            SynthesizedProgram selected = null;
            List<List<IExpression>> expressions = SynthesizedPrograms(mapping, solutions);

            List<SynthesizedProgram> hypotheses = new List<SynthesizedProgram>();
            foreach (List<IExpression> l in expressions)
            {
                hypotheses = CombSynthProgramExp(hypotheses, l, examples);
            }

            var sorted = from hypothesis in hypotheses
                         orderby (new SelectorManager(setting)).Order(hypothesis.solutions) descending
                         select hypothesis;

            int count = 0;
            foreach (SynthesizedProgram sp in sorted)
            {
                count++;

                bool isValid = ValidateSynthesizedProgram(sp.solutions, examples/*, file*/);
                Console.WriteLine("Processing ....");
                if (isValid)
                {
                    //file.Write(1 + "\n");
                    Console.WriteLine(count);
                    if (selected == null)
                    {
                        selected = sp;
                    }
                    return sp;
                }
                else
                {
                    //file.Write(0 + "\n");
                }
            }
            // }

            //file.Close();
            return selected;
        }


        /// <summary>
        /// Create boundary points
        ///// </summary>
        ///// <param name="input">Input string</param>
        ///// <param name="output">Output string</param>
        ///// <param name="data">ListNode</param>
        ///// <returns>Boundary points</returns>
        //[Obsolete]
        //public static List<int> CreateBoundaryPoints(string input, string output, Tuple<ListNode, ListNode> data)
        //{
        //    List<int> points = Differ(data.Item1, data.Item2);
        //    points.Sort();
        //    return points;
        //}

        /// <summary>
        /// Create boundary points
        /// </summary>
        /// <param name="data">ListNode</param>
        /// <returns>Boundary points</returns>
        public static List<int> CreateBoundaryPoints(Tuple<ListNode, ListNode> data)
        {
            List<int> points = Differ(data.Item1, data.Item2);
            points.Sort();
            return points;
        }

        private static List<int> Differ(ListNode input, ListNode output)
        {
            List<int> indexes = new List<int>();
            indexes.Add(0);
            List<ComparisonObject> tinput = new List<ComparisonObject>();
            for (int i = 0; i < input.List.Count; i++)
            {
                SyntaxNodeOrToken st = input.List[i];
                DymToken token = new DymToken(st);
                ComparisonObject obj = new ComparisonObject(token, i);
                tinput.Add(obj);
            }

            List<ComparisonObject> touput = new List<ComparisonObject>();
            for (int i = 0; i < output.List.Count; i++)
            {
                SyntaxNodeOrToken st = output.List[i];
                DymToken token = new DymToken(st);
                ComparisonObject obj = new ComparisonObject(token, i);
                touput.Add(obj);
            }


            ListDiffer<ComparisonObject> differ = new ListDiffer<ComparisonObject>();
            List<ComparisonResult<ComparisonObject>> result = differ.FindDifference(tinput, touput);
            for (int i = 0; i < result.Count; i++)
            {
                ComparisonResult<ComparisonObject> r = result[i];
                if (!r.ModificationType.Equals(ModificationType.None))
                {
                    Console.WriteLine(r.DataCompared);
                    if (r.DataCompared.Index < output.Length() && !indexes.Contains(r.DataCompared.Index))
                    {
                        if (i - 1 >= 0 && (result[i - 1].DataCompared.Index == r.DataCompared.Index - 1 && result[i - 1].ModificationType.Equals(r.ModificationType)))
                        {
                            continue;
                        }
                        indexes.Add(r.DataCompared.Index);
                    }
                }

                if (r.ModificationType.Equals(ModificationType.Inserted))
                {
                    if (r.DataCompared.Index < output.Length() && !indexes.Contains(Math.Min(output.Length(), r.DataCompared.Index + 1)))
                    {
                        indexes.Add(Math.Min(output.Length(), r.DataCompared.Index + 1));
                    }
                }
            }

            if (!indexes.Contains(output.Length()))
            {
                indexes.Add(output.Length());
            }

            return indexes;
        }


       
    }
}

/* /// <summary>
       /// Is a solution
       /// </summary>
       /// <param name="solutionCandidate">Solution candidate</param>
       /// <param name="length">Output length</param>
       /// <returns>True if candidate is a solution</returns>
       private static bool IsASolution(List<Tuple<int, int>> solutionCandidate, int length)
       {
           if (solutionCandidate.Count > 0)
           {
               int i = solutionCandidate.Last().Item1;
               int j = solutionCandidate.Last().Item2;

               return (i + j == length);
           }

           return false;
       }*/

/*/// <summary>
       /// Create boundary points
       /// </summary>
       /// <param name="input">Input data</param>
       /// <param name="output">Output data</param>
       /// <param name="original">Original data</param>
       /// <param name="data">ListNode data</param>
       /// <returns>Boundary points</returns>
       public static List<int> CreateBoundaryPoints2(string input, string output, string original, Tuple<ListNode, ListNode> data)
       {
           SyntaxTree st1 = CSharpSyntaxTree.ParseText(input);
           SyntaxTree st2 = CSharpSyntaxTree.ParseText(output);

           var changestext = st2.GetChanges(st1);

           String pattern = Regex.Escape(output);
           int start = Regex.Match(original, pattern).Index;

           List<int> boundary = new List<int>();
           boundary.Add(0);
           foreach (TextChange changed in changestext)
           {
               List<int> indexs = LookupIndex(changed, data.Item2);
               foreach (int i in indexs)
               {
                   if (i > 0 && boundary.Contains(i))
                   {
                       boundary.Add(i);
                   }
               }
           }

           boundary.Add(data.Item2.Length());
           return boundary;
       }

       /// <summary>
       /// Index points
       /// </summary>
       /// <param name="re">Text changed</param>
       /// <param name="listNode">ListNode</param>
       /// <returns>Index Points</returns>
       private static List<int> LookupIndex(TextChange re, ListNode listNode)
       {
           if (listNode == null)
           {
               throw new Exception("Changed text or list node cannot be null");
           }

           if (listNode.Length() == 0)
           {
               throw new Exception("List node must contains at least one element");
           }

           List<int> result = new List<int>();
           SyntaxNodeOrToken firstNode = listNode.List[0];

           List<SyntaxNodeOrToken> list = listNode.List;
           int i = 0;
           SyntaxNodeOrToken node = list[i];
           TextSpan span = node.Span;
           while ((firstNode.FullSpan.Start + re.Span.Start) > span.Start)
           {
               if (i >= list.Count())
               {
                   return result;
               }
               node = list[i++];
               span = node.Span;
           }

           int j = i;
           while ((firstNode.FullSpan.Start + re.Span.Start + re.NewText.Length) > span.Start)
           {
               if (j == list.Count)
                   break;
               node = list[Math.Max(j++, 0)];
               span = node.Span;
           }

           if (i == 0 && j == 0)
           {
               j = list.Count;
           }

           for (int z = i - 1; z < j; z++)
           {
               result.Add(z);
           }
           return result;
       }*/

/*private static int LookupIndex(int start, ListNode data)
{
    for (int i = 0; i < data.Length(); i++)
    {
        SyntaxNodeOrToken node = data.List[i];
        int spanStart = node.Span.Start;

        if (spanStart >= start)
        {
            return i;
        }
    }

    return -1;
}*/

/*
        private static int GetProduct(Dictionary<Tuple<int, int>, List<IExpression>> mapping, List<Tuple<int, int>> solution)
        {
            int sum = 1;
            foreach (Tuple<int, int> entry in solution)
            {
                sum *= mapping[entry].Count();
            }

            return sum;
        }
        */