using System;
using System.Collections.Generic;
using System.Linq;
using DiGraph;
using Spg.ExampleRefactoring.LCS;
using LongestCommonAncestor;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.AST;
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
        public SynthesizerSetting Setting { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SynthesisManager()
        {
            this.Setting = new SynthesizerSetting();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="setting">Setting</param>
        public SynthesisManager(SynthesizerSetting setting)
        {
            this.Setting = setting;
        }

        /// <summary>
        /// Create new synthesized program combining current synthesized programs with expressions
        /// </summary>
        /// <param name="synthesizedProgramList">Synthesized programs list</param>
        /// <param name="expressionList">Expression list</param>
        /// <param name="examples">Examples</param>
        /// <returns>Synthesized programs list</returns>
        private List<SynthesizedProgram> CombSynthProgramExp2(List<SynthesizedProgram> synthesizedProgramList, List<IExpression> expressionList, List<Tuple<ListNode, ListNode>> examples)
        {
            List<SynthesizedProgram> synthesizedProgList = new List<SynthesizedProgram>();
            var items = from expression in expressionList
                        orderby new SelectorManager(Setting).Order(expression) descending
                        select expression;
            if (synthesizedProgramList.Count > 0)
            {
                foreach (SynthesizedProgram sd1 in synthesizedProgramList)
                {
                    List<IExpression> solutions = new List<IExpression>(sd1.Solutions);
                    SynthesizedProgram sp = new SynthesizedProgram();
                    sp.Solutions = solutions;
                    sp.Add(items.First());
                    bool isValid = ValidateSubExpression(sp.Solutions, examples);
                    if (isValid)
                    {
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
        /// Validate an expression in function of the examples
        /// </summary>
        /// <param name="expressions">Expression to be tested</param>
        /// <param name="examples">Set of examples</param>
        /// <returns>True if expression match the examples, false otherwise</returns>
        public bool ValidateSubExpression(List<IExpression> expressions, List<Tuple<ListNode, ListNode>> examples)
        {
            SynthesizedProgram syntheProg = new SynthesizedProgram();
            syntheProg.Solutions = expressions; ;

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

        /// <summary>
        /// Expression list that forms the synthesized program
        /// </summary>
        /// <param name="mapping">Expressions mapping</param>
        /// <param name="indexs">Synthesized program sub parts</param>
        /// <returns>Expression list that forms the synthesized program</returns>
        private static List<Dictionary<ExpressionKind, List<IExpression>>> SynthesizedPrograms(Dictionary<Tuple<Vertex, Vertex>, Dictionary<ExpressionKind, List<IExpression>>> mapping, List<Vertex> indexs)
        {
            List<Dictionary<ExpressionKind, List<IExpression>>> synthProgList = new List<Dictionary<ExpressionKind, List<IExpression>>>();
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
                if (expression is ConstTokens )
                {
                    Handle(features, FeatureType.CONSTSTR);
                }
                else
                {
                    SubTokens subStr = (SubTokens)expression;

                    IPosition p1 = subStr.P1;
                    IPosition p2 = subStr.P2;

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

        /// <summary>
        /// Evalute positioning expressions
        /// </summary>
        /// <param name="p1">Posisition expression</param>
        /// <param name="features">Features mapping</param>
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

                int position = cpos.Position;

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

        /// <summary>
        /// Verify the presence of dynamic token on relative position
        /// </summary>
        /// <param name="features">Mapping of features</param>
        /// <param name="pos">Relative position expression</param>
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
            for (int i = 0; i < pos.R1.Tokens.Count; i++)
            {
                if (pos.R1.Tokens[i] is DymToken)
                {
                    count++;
                }
            }

            for (int i = 0; i < pos.R2.Tokens.Count; i++)
            {
                if (pos.R2.Tokens[i] is DymToken)
                {
                    count++;
                }
            }
            features[key] += count;
        }

        /// <summary>
        /// Verify the presence of syntax elements on relative position expression
        /// </summary>
        /// <param name="features">Features mapping</param>
        /// <param name="pos">Relative position expression</param>
        private static void HandleSyntax(Dictionary<FeatureType, int> features, Pos pos)
        {
            int value;
            FeatureType key = FeatureType.SYNTAX;

            if (!features.TryGetValue(key, out value))
            {
                features.Add(key, 0);
            }

            int count = 0;
            for (int i = 0; i < pos.R1.Tokens.Count; i++)
            {
                if (pos.R1.Tokens[i].token.AsNode() != null)
                {
                    count++;
                }
            }

            for (int i = 0; i < pos.R2.Tokens.Count; i++)
            {
                if (pos.R2.Tokens[i].token.AsNode() != null)
                {
                    count++;
                }
            }
            features[key] += count;
        }

        /// <summary>
        /// Identify empty elements on relative position expression
        /// </summary>
        /// <param name="features">Features mapping</param>
        /// <param name="p1">Relative position expression</param>
        private static void HandleEmpty(Dictionary<FeatureType, int> features, Pos p1)
        {
            int value;
            FeatureType key = FeatureType.EMPTY;


            if (!features.TryGetValue(key, out value))
            {
                features.Add(key, 0);
                value = 0;
            }

            if (p1.R1.Length() == 0)
            {
                features[key] = value + 1;
            }

            if (p1.R2.Length() == 0)
            {
                features[key] = value + 1;
            }

        }

        /// <summary>
        /// Identify the number of times the feature appear on the synthesized program
        /// </summary>
        /// <param name="features">Dictionary of features</param>
        /// <param name="key">Analyzed feature</param>
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
        public SynthesizedProgram FilterASTPrograms(Dictionary<Tuple<Vertex, Vertex>, Dictionary<ExpressionKind, List<IExpression>>> mapping, List<Vertex> solutions, List<Tuple<ListNode, ListNode>> examples)
        {
            List<Dictionary<ExpressionKind, List<IExpression>>> expressions = SynthesizedPrograms(mapping, solutions);

            List<SynthesizedProgram> hypotheses = new List<SynthesizedProgram>();
            foreach (Dictionary<ExpressionKind, List<IExpression>> item in expressions)
            {
                List<IExpression> l = GetExpressions(item);         
                hypotheses = CombSynthProgramExp2(hypotheses, l, examples);
            }

            var sorted = from hypothesis in hypotheses
                         orderby (new SelectorManager(Setting)).Order(hypothesis.Solutions) descending
                         select hypothesis;

            int count = 0;
            foreach (SynthesizedProgram sp in sorted)
            {
                count++;

                bool isValid = ValidateSynthesizedProgram(sp.Solutions, examples);
                //Console.WriteLine("Processing ....");
                if (isValid)
                {
                        return sp;         
                }
            }
            return null;
        }

        /// <summary>
        /// /Get expression list
        /// </summary>
        /// <param name="mapping">Mapping of expression each kind</param>
        /// <returns>Expression list</returns>
        public List<IExpression> GetExpressions(Dictionary<ExpressionKind, List<IExpression>> mapping)
        {
            List<IExpression> expressions = new List<IExpression>();
            foreach (KeyValuePair<ExpressionKind, List<IExpression>> entry in mapping)
            {
                expressions.AddRange(entry.Value);
            }
            return expressions;
        }

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

        ///// <summary>
        ///// Calculate the difference point between input and output
        ///// </summary>
        ///// <param name="input">Input</param>
        ///// <param name="output">Output</param>
        ///// <returns>Index of the difference</returns>
        //public static List<int> Differ(ListNode input, ListNode output)
        //{
        //    List<int> indexes = new List<int>();
        //    List<ComparisonObject> tinput = DynTokens(input, ComparisonObject.INPUT);
        //    List<ComparisonObject> touput = DynTokens(output, ComparisonObject.OUTPUT);

        //    ListDiffer<ComparisonObject> differ = new ListDiffer<ComparisonObject>();
        //    List<ComparisonResult<ComparisonObject>> result = differ.FindDifference(tinput, touput);
        //    List<ComparisonResult<ComparisonObject>> result2 = differ.FindDifference(touput, tinput);
        //    List<ComparisonResult<ComparisonObject>> selecteds = new List<ComparisonResult<ComparisonObject>>();
        //    for (int i = 0; i < result.Count; i++)
        //    {
        //        ComparisonResult<ComparisonObject> r = result[i];
        //        if (!r.ModificationType.Equals(ModificationType.None))
        //        {

        //            //if (r.DataCompared.Index < output.Length())
        //            //{
        //                selecteds.Add(r);
        //            //}
        //        }
        //    }

        //    //List<ComparisonResult<ComparisonObject>> selecteds2 = new List<ComparisonResult<ComparisonObject>>();
        //    //for (int i = 0; i < result2.Count; i++)
        //    //{
        //    //    ComparisonResult<ComparisonObject> r = result2[i];
        //    //    if (r.ModificationType.Equals(ModificationType.None))
        //    //    {

        //    //        //if (r.DataCompared.Index < output.Length())
        //    //        //{
        //    //            selecteds2.Add(r);
        //    //        //}
        //    //    }
        //    //}

        //    indexes = FilterIndexes(selecteds, output);
        //    indexes.AddRange(FilterIndexes(result2, output));

        //    if (!indexes.Contains(0)) //insert first index if it was not found.
        //    {
        //        indexes.Insert(0, 0);
        //    }

        //    if (!indexes.Contains(output.Length())) //insert last index if it is not found.
        //    {
        //        indexes.Add(output.Length());
        //    }

        //    HashSet<int> nonDup = new HashSet<int>(indexes);

        //    return nonDup.ToList();
        //}

        /// <summary>
        /// Calculate the difference point between input and output
        /// </summary>
        /// <param name="input">Input</param>
        /// <param name="output">Output</param>
        /// <returns>Index of the difference</returns>
        public static List<int> Differ(ListNode input, ListNode output)
        {
            List<int> indexes = new List<int>();
            List<ComparisonObject> tinput = DynTokens(input, ComparisonObject.INPUT);
            List<ComparisonObject> touput = DynTokens(output, ComparisonObject.OUTPUT);

            ListDiffer<ComparisonObject> differ = new ListDiffer<ComparisonObject>();
            List<ComparisonResult<ComparisonObject>> result = differ.FindDifference(tinput, touput);
            List<ComparisonResult<ComparisonObject>> result2 = differ.FindDifference(touput, tinput);
            List<ComparisonResult<ComparisonObject>> selecteds = new List<ComparisonResult<ComparisonObject>>();
            for (int i = 0; i < result.Count; i++)
            {
                ComparisonResult<ComparisonObject> r = result[i];
                if (!r.ModificationType.Equals(ModificationType.None))
                {
                    selecteds.Add(r);
                }
            }

            indexes = FilterIndexes(selecteds, output);
            indexes.AddRange(FilterIndexes(result2, output));

            if (!indexes.Contains(0)) //insert first index if it was not found.
            {
                indexes.Insert(0, 0);
            }

            if (!indexes.Contains(output.Length())) //insert last index if it is not found.
            {
                indexes.Add(output.Length());
            }

            HashSet<int> nonDup = new HashSet<int>(indexes);

            return nonDup.ToList();
        }

        ///// <summary>
        ///// Calculate the difference point between input and output
        ///// </summary>
        ///// <param name = "input" > Input </ param >
        ///// < param name="output">Output</param>
        ///// <returns>Index of the difference</returns>
        //public static List<int> Differ(ListNode input, ListNode output)
        //{
        //    List<int> indexes = new List<int> { 0 };
        //    List<ComparisonObject> tinput = DynTokens(input, ComparisonObject.INPUT);
        //    List<ComparisonObject> touput = DynTokens(output, ComparisonObject.OUTPUT);

        //    ListDiffer<ComparisonObject> differ = new ListDiffer<ComparisonObject>();
        //    List<ComparisonResult<ComparisonObject>> result = differ.FindDifference(tinput, touput);
        //    List<ComparisonResult<ComparisonObject>> selecteds = new List<ComparisonResult<ComparisonObject>>();
        //    for (int i = 0; i < result.Count; i++)
        //    {
        //        ComparisonResult<ComparisonObject> r = result[i];
        //        if (!r.ModificationType.Equals(ModificationType.None))
        //        {
        //            Console.WriteLine(r.DataCompared);
        //            if (r.DataCompared.Index < output.Length() && !indexes.Contains(r.DataCompared.Index))
        //            {
        //                if (i - 1 >= 0 && (result[i - 1].DataCompared.Index == r.DataCompared.Index - 1 && result[i - 1].ModificationType.Equals(r.ModificationType)))
        //                {
        //                    continue;
        //                }
        //                indexes.Add(r.DataCompared.Index);
        //                selecteds.Add(r);
        //            }
        //        }

        //        if (r.ModificationType.Equals(ModificationType.Inserted))
        //        {
        //            if (r.DataCompared.Index < output.Length() && !indexes.Contains(Math.Min(output.Length(), r.DataCompared.Index + 1)))
        //            {
        //                indexes.Add(Math.Min(output.Length(), r.DataCompared.Index + 1));
        //            }
        //        }
        //    }


        //    if (!indexes.Contains(output.Length()))
        //    {
        //        indexes.Add(output.Length());
        //    }

        //    return indexes;
        //}

        //private static List<int> FilterIndexes(List<ComparisonResult<ComparisonObject>> selecteds, ListNode output)
        //{
        //    List<int> indexes = new List<int>();
        //    ModificationType lastEvaluated = ModificationType.None;
        //    int lastIndex = 0;
        //    if (selecteds.Any())
        //    {
        //        var element = selecteds.First();
        //        lastEvaluated = element.ModificationType;
        //        lastIndex = element.DataCompared.Index;
        //        indexes.Add(selecteds.First().DataCompared.Index);

        //        if (selecteds.First().ModificationType.Equals(ModificationType.Inserted))
        //        {
        //            if (selecteds.First().DataCompared.Index < output.Length() && !indexes.Contains(Math.Min(output.Length(), selecteds.First().DataCompared.Index + 1)))
        //            {
        //                indexes.Add(Math.Min(output.Length(), selecteds.First().DataCompared.Index + 1));
        //            }
        //        }
        //    }

        //    for(int i = 1; i < selecteds.Count - 1; i++)
        //    {
        //        var result = selecteds[i];

        //        if (result.ModificationType.Equals(lastEvaluated) )
        //        {
        //            if (lastIndex != result.DataCompared.Index - 1) //sequence of evaluated elements.
        //            {
        //                indexes.Add(result.DataCompared.Index);
        //            }
        //        }
        //        else
        //        {
        //            if (!indexes.Contains(lastIndex))
        //            {
        //                indexes.Add(lastIndex);
        //            }

        //            lastEvaluated = result.ModificationType;
        //            if (!indexes.Contains(lastIndex))
        //            {
        //                indexes.Add(result.DataCompared.Index);
        //            }
        //        }

        //        if (result.ModificationType.Equals(ModificationType.Inserted))
        //        {
        //            if (result.DataCompared.Index < output.Length() && !indexes.Contains(Math.Min(output.Length(), result.DataCompared.Index + 1)))
        //            {
        //                indexes.Add(Math.Min(output.Length(), result.DataCompared.Index + 1));
        //            }
        //        }

        //        lastIndex = result.DataCompared.Index; //Update last index
        //    }

        //    if (selecteds.Count() > 1 && !indexes.Contains(selecteds.Last().DataCompared.Index))
        //    {
        //        indexes.Add(selecteds.Last().DataCompared.Index); //last element must always be inserted.
        //    }

        //    return indexes;
        //}

        private static List<int> FilterIndexes(List<ComparisonResult<ComparisonObject>> selecteds, ListNode output)
        {
            List<List<ComparisonResult<ComparisonObject>>> sequences = GetSubSequences(selecteds);

            List<int> indexes = new List<int>();

            foreach(var sequence in sequences)
            {
                if (sequence.First().DataCompared.Indicator.Equals(ComparisonObject.OUTPUT)){
                    indexes.Add(sequence.First().DataCompared.Index);

                    if (sequence.First().ModificationType.Equals(ModificationType.Inserted))
                    {
                        indexes.Add(sequence.Last().DataCompared.Index);
                        indexes.Add(sequence.Last().DataCompared.Index + 1);
                    }
                }
            }
            indexes.Except(indexes.Where(c => c > output.Length())).ToList();
            return indexes;
        }

        //private static List<List<ComparisonResult<ComparisonObject>>> GetSubSequences(List<ComparisonResult<ComparisonObject>> selecteds)
        //{
        //    List<List<ComparisonResult<ComparisonObject>>> sequences = new List<List<ComparisonResult<ComparisonObject>>>();

        //    if (!selecteds.Any()) return sequences;

        //    List<ComparisonResult<ComparisonObject>> subSequence = new List<ComparisonResult<ComparisonObject>>();
        //    subSequence.Add(selecteds.First());
        //    for(int i = 1; i < selecteds.Count; i++)
        //    {
        //        var result = selecteds[i];
        //        //if (result.DataCompared.Indicator.Equals(ComparisonObject.OUTPUT))
        //        //{
        //            if (result.ModificationType.Equals(subSequence.Last().ModificationType) /*&& result.DataCompared.Indicator.Equals(subSequence.Last().DataCompared.Indicator)*/)
        //            {
        //                if (subSequence.Last().DataCompared.Index == result.DataCompared.Index - 1)
        //                {
        //                    subSequence.Add(result);
        //                }
        //                else
        //                {
        //                    List<ComparisonResult<ComparisonObject>> newList = new List<ComparisonResult<ComparisonObject>>(subSequence);
        //                    sequences.Add(newList);
        //                    subSequence = new List<ComparisonResult<ComparisonObject>>();
        //                    subSequence.Add(result);
        //                }
        //            }
        //            else
        //            {
        //                List<ComparisonResult<ComparisonObject>> newList = new List<ComparisonResult<ComparisonObject>>(subSequence);
        //                sequences.Add(newList);
        //                subSequence = new List<ComparisonResult<ComparisonObject>>();
        //                subSequence.Add(result);
        //            }
        //        }
        //    //}

        //    sequences.Add(subSequence);
        //    return sequences;
        //}

        //private static List<List<ComparisonResult<ComparisonObject>>> GetSubSequences(List<ComparisonResult<ComparisonObject>> selecteds)
        //{
        //    List<ComparisonResult<ComparisonObject>> inserted = new List<ComparisonResult<ComparisonObject>>();
        //    List<ComparisonResult<ComparisonObject>> removed = new List<ComparisonResult<ComparisonObject>>();

        //    foreach(var item in selecteds)
        //    {
        //        if (item.ModificationType.Equals(ModificationType.Inserted))
        //        {
        //            inserted.Add(item);
        //        }

        //        if (item.ModificationType.Equals(ModificationType.Deleted))
        //        {
        //            removed.Add(item);
        //        }
        //    }

        //    var subInserted = GetSubSequencesDataCompared(inserted);
        //    var subRemoved = GetSubSequencesDataCompared(removed);
        //    var result = new List<List<ComparisonResult<ComparisonObject>>>(subInserted);
        //    result.AddRange(subRemoved);

        //    return result;
        //}

        private static List<List<ComparisonResult<ComparisonObject>>> GetSubSequences(List<ComparisonResult<ComparisonObject>> selecteds)
        {
            List<List<ComparisonResult<ComparisonObject>>> sequences = new List<List<ComparisonResult<ComparisonObject>>>();

            if (!selecteds.Any()) return sequences;

            List<ComparisonResult<ComparisonObject>> subSequence = new List<ComparisonResult<ComparisonObject>>();
            subSequence.Add(selecteds.First());
            for (int i = 1; i < selecteds.Count; i++)
            {
                var result = selecteds[i];

                if (result.ModificationType.Equals(subSequence.Last().ModificationType))
                {
                    if (subSequence.Last().DataCompared.Index == result.DataCompared.Index - 1)
                    {
                        subSequence.Add(result);
                    }
                    else
                    {
                        List<ComparisonResult<ComparisonObject>> newList = new List<ComparisonResult<ComparisonObject>>(subSequence);
                        sequences.Add(newList);
                        subSequence = new List<ComparisonResult<ComparisonObject>>();
                        subSequence.Add(result);
                    }
                }
                else
                {
                    List<ComparisonResult<ComparisonObject>> newList = new List<ComparisonResult<ComparisonObject>>(subSequence);
                    sequences.Add(newList);
                    subSequence = new List<ComparisonResult<ComparisonObject>>();
                    subSequence.Add(result);
                }
            }
            //}

            sequences.Add(subSequence);
            return sequences;
        }



        /// <summary>
        /// Create comparison objects
        /// </summary>
        /// <param name="listNode">List of nodes</param>
        /// <param name="indicator">Indicator of input or output</param>
        /// <returns>Comparison objects</returns>
        private static List<ComparisonObject> DynTokens(ListNode listNode, int indicator)
        {
            List<ComparisonObject> comparisonObjects = new List<ComparisonObject>();
            for (int i = 0; i < listNode.List.Count; i++)
            {
                SyntaxNodeOrToken st = listNode.List[i];
                DymToken token = new DymToken(st, false);
                ComparisonObject obj = new ComparisonObject(token, i, indicator);
                comparisonObjects.Add(obj);
            }
            return comparisonObjects;
        }
    }
}