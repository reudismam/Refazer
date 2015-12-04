using System;
using System.Collections.Generic;
using System.Linq;
using DiGraph;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Digraph;
using Spg.ExampleRefactoring.Expression;
using Spg.ExampleRefactoring.Partition;
using Spg.ExampleRefactoring.Position;
using Spg.ExampleRefactoring.Setting;
using Spg.ExampleRefactoring.Tok;
using Spg.LocationRefactor.Predicate;
using Spg.LocationRefactoring.Tok;

namespace Spg.ExampleRefactoring.Synthesis
{
    /// <summary>
    /// Class to generate programs
    /// </summary>
    public class ASTProgram
    {
        /// <summary>
        /// Setting for AST synthesis computation
        /// </summary>
        /// <returns>Setting</returns>
        public SynthesizerSetting Setting { get; set; }

        /// <summary>
        /// Previous computed token sequences
        /// </summary>
        private readonly Dictionary<ListNode, List<TokenSeq>> _computed = new Dictionary<ListNode, List<TokenSeq>>();


        /// <summary>
        /// Dynamic tokens and occurrences
        /// </summary>
        /// <returns></returns>
        public Dictionary<DymToken, List<DymToken>> Dict { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ASTProgram()
        {
            Dict = new Dictionary<DymToken, List<DymToken>>();
            this.Setting = new SynthesizerSetting { Deviation = 2, ConsiderConstrStr = true };
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="setting">Configuration setting</param>
        /// <param name="examples">Example list</param>
        /// <exception cref="Exception">Argument exception</exception>
        public ASTProgram(SynthesizerSetting setting, List<Tuple<ListNode, ListNode>> examples)
        {
            if (examples == null || examples.Count == 0) { throw new ArgumentException("Examples cannot be null or empty"); }

            this.Setting = setting;
            Dict = new Dictionary<DymToken, List<DymToken>>();

            if (setting.DynamicTokens)
            {
                CreateDymTokens(examples, setting._getFullyQualifiedName);
            }
        }

        ///// <summary>
        ///// Generate synthesized programs
        ///// </summary>
        ///// <param name="examples">Example set</param>
        ///// <returns>Synthesized program list</returns>
        //public List<SynthesizedProgram> GenerateStringProgram(List<Tuple<ListNode, ListNode>> examples)
        //{
        //    if (examples == null || examples.Count == 0) { throw new ArgumentException("Examples cannot be null or empty"); }

        //    List<SynthesizedProgram> validated = new List<SynthesizedProgram>();
        //    if (OutputIsEmpty(examples))
        //    {
        //        return GetEmptyProgram();
        //    }

        //    List<Dag> dags = Dags(examples);

        //    IntersectManager intManager = new IntersectManager();

        //    Dag T = intManager.Intersect(dags);

        //    //remove
        //    PartitionManager pManager = new PartitionManager();
        //    pManager.GeneratePartition(dags);
        //    //remove

        //    if (T == null)
        //    {
        //        dags = Dags(examples, false);
        //        T = intManager.Intersect(dags);

        //        //remove
        //        pManager = new PartitionManager();
        //        pManager.GeneratePartition(dags);
        //        //remove
        //    }

        //    if (T == null)
        //    {
        //        Setting.Deviation = 1;
        //        Console.WriteLine("Cannot generate programs for the defaut deviation. Setting deviation for 1.");
        //        dags = Dags(examples);
        //        T = intManager.Intersect(dags);

        //        //remove
        //        pManager = new PartitionManager();
        //        pManager.GeneratePartition(dags);
        //        //remove
        //    }

        //    ExpressionManager expmanager = new ExpressionManager();
        //    expmanager.FilterExpressions(T, examples);

        //    Clear(T);

        //    BreadthFirstDirectedPaths bfs = new BreadthFirstDirectedPaths(T.dag, T.Init.Id);
        //    double dist = bfs.DistTo(T.End.Id);
        //    Console.WriteLine(dist);

        //    List<Vertex> solutions = new List<Vertex>();

        //    foreach (string s in bfs.PathTo(T.End.Id))
        //    {
        //        solutions.Add(T.Vertexes[s]);
        //    }

        //    SynthesisManager manager = new SynthesisManager(Setting);
        //    SynthesizedProgram valid = manager.FilterASTPrograms(T.Mapping, solutions, examples);
        //    Console.WriteLine(valid);

        //    validated.Add(valid);
        //    return validated;
        //}

        /// <summary>
        /// Generate synthesized programs
        /// </summary>
        /// <param name="examples">Example set</param>
        /// <param name="boundary">Determine if to generate the program the tool needs to use boundaries.</param>
        /// <returns>Synthesized program list</returns>
        public List<SynthesizedProgram> GenerateStringProgram(List<Tuple<ListNode, ListNode>> examples, bool boundary = true)
        {
            if (examples == null || examples.Count == 0) { throw new ArgumentException("Examples cannot be null or empty"); }

            List<SynthesizedProgram> validated = new List<SynthesizedProgram>();
            if (OutputIsEmpty(examples))
            {
                return GetEmptyProgram();
            }

            Dictionary<Dag, List<Tuple<ListNode, ListNode>>> dags = Dags(examples, boundary);
            PartitionManager pManager = new PartitionManager();
            Dictionary<Dag, List<Tuple<ListNode, ListNode>>> Ts = pManager.GeneratePartition(dags);

            if (Ts.Count == examples.Count && boundary)
            {
                //Setting.Deviation = 1;
                return GenerateStringProgram(examples, false);
            }

            List<Tuple<IPredicate, SynthesizedProgram>> S = new List<Tuple<IPredicate, SynthesizedProgram>>();
            foreach (KeyValuePair<Dag, List<Tuple<ListNode, ListNode>>> T in Ts)
            {
                SynthesizedProgram valid = CreateSynthesizedProgram(T.Key, T.Value);

                if (Ts.Count == 1) {
                    Console.WriteLine("Generated program\n" + valid);
                    return new List<SynthesizedProgram> { valid };
                }

                List<Tuple<ListNode, ListNode, bool>> ln = CreateConditionalExamples(T.Key, T.Value, Ts, pManager);
                List<IPredicate> predicates = pManager.LearnPredicates(ln);

                if (!predicates.Any())
                {
                    Setting.Deviation = 1;
                    return GenerateStringProgram(examples);
                }

                Tuple<IPredicate, SynthesizedProgram> tSol = Tuple.Create(predicates.First(), valid);
                S.Add(tSol);
            }

            Switch sSwitch = new Switch(S);
            validated.Add(sSwitch);
            Console.WriteLine("Generated program\n" + sSwitch);
            return validated;
        }

        /// <summary>
        /// Create a synthesized program
        /// </summary>
        /// <param name="dag">Dag to be analyzed</param>
        /// <param name="examples">Examples</param>
        /// <returns></returns>
        private SynthesizedProgram CreateSynthesizedProgram(Dag dag, List<Tuple<ListNode, ListNode>> examples)
        {
            ExpressionManager expmanager = new ExpressionManager();
            expmanager.FilterExpressions(dag, examples);
            Clear(dag);

            BreadthFirstDirectedPaths bfs = new BreadthFirstDirectedPaths(dag.dag, dag.Init.Id);
            List<Vertex> solutions = new List<Vertex>();

            foreach (string s in bfs.PathTo(dag.End.Id))
            {
                solutions.Add(dag.Vertexes[s]);
            }

            SynthesisManager manager = new SynthesisManager(Setting);
            SynthesizedProgram valid = manager.FilterASTPrograms(dag.Mapping, solutions, examples);
            return valid;
        }

        /// <summary>
        /// Create condictional examples
        /// </summary>
        /// <param name="d">Dag analyzed</param>
        /// <param name="examples">Examples associated with the dag</param>
        /// <param name="Ts">Other dags created</param>
        /// <param name="pManager">Partition manager</param>
        /// <returns></returns>
        private static List<Tuple<ListNode, ListNode, bool>> CreateConditionalExamples(Dag d, List<Tuple<ListNode, ListNode>> examples, Dictionary<Dag, List<Tuple<ListNode, ListNode>>> Ts, PartitionManager pManager)
        {
            KeyValuePair<Dag, List<Tuple<ListNode, ListNode>>> T;
            List<Tuple<ListNode, ListNode, bool>> ln = new List<Tuple<ListNode, ListNode, bool>>();

            foreach (Tuple<ListNode, ListNode> dItem in examples)
            {
                Tuple<ListNode, ListNode, bool> tuple = Tuple.Create(dItem.Item1, dItem.Item1, true);
                ln.Add(tuple);
            }

            foreach (KeyValuePair<Dag, List<Tuple<ListNode, ListNode>>> TC in Ts)
            {
                foreach (Tuple<ListNode, ListNode> dItem in TC.Value)
                {
                    if (!TC.Key.Equals(d))
                    {
                        Tuple<ListNode, ListNode, bool> tuple = Tuple.Create(dItem.Item1, dItem.Item1, false);
                        List<Tuple<ListNode, ListNode, bool>> l = new List<Tuple<ListNode, ListNode, bool>>(ln);
                        l.Add(tuple);
                        List<IPredicate> ps = pManager.LearnPredicates(l);
                        if (ps.Any())
                        {
                            ln.Add(tuple);
                        }
                    }
                }
            }
            return ln;
        }

        /// <summary>
        /// Get an empty program
        /// </summary>
        /// <returns></returns>
        public static List<SynthesizedProgram> GetEmptyProgram()
        {
            List<SynthesizedProgram> validated = new List<SynthesizedProgram>();
            IExpression expression = new ConstruStr(new ListNode(new List<SyntaxNodeOrToken>()));
            List<IExpression> expressions = new List<IExpression> { expression };
            SynthesizedProgram program = new SynthesizedProgram();
            validated.Add(program);
            program.Solutions = expressions;

            return validated;
        }

        /// <summary>
        /// Verify is output is empty
        /// </summary>
        /// <param name="examples">Example set</param>
        /// <returns>True is output is empty</returns>
        private bool OutputIsEmpty(List<Tuple<ListNode, ListNode>> examples)
        {
            bool isEmpty = true;
            foreach (var example in examples)
            {
                if (example.Item2.List.Any())
                {
                    isEmpty = false;
                    break;
                }
            }
            return isEmpty;
        }

        private Dictionary<Dag, List<Tuple<ListNode, ListNode>>> Dags(List<Tuple<ListNode, ListNode>> examples, bool boundary = true)
        {
            Dictionary<Dag, List<Tuple<ListNode, ListNode>>> dags = new Dictionary<Dag, List<Tuple<ListNode, ListNode>>>();
            foreach (var example in examples)
            {
                List<int> boundaryPoints = null;

                if (boundary) { boundaryPoints = SynthesisManager.CreateBoundaryPoints(example); }

                ListNode input = example.Item1;
                ListNode output = example.Item2;

                List<Tuple<ListNode, ListNode>> exs = new List<Tuple<ListNode, ListNode>> { example };
                Dag d = GenerateStringBoundary(input, output, boundaryPoints);
                dags.Add(d, exs);
            }
            return dags;
        }

        ///// <summary>
        ///// Create dynamic tokens
        ///// </summary>
        ///// <param name="examples">Examples</param>
        //private void CreateDymTokens(List<Tuple<ListNode, ListNode>> examples)
        //{
        //    if (examples == null) { throw new ArgumentNullException("examples"); }

        //    Dictionary<DymToken, int> temp;
        //    foreach (Tuple<ListNode, ListNode> t in examples)
        //    {
        //        temp = new Dictionary<DymToken, int>();
        //        for (int i = 0; i < t.Item1.List.Count; i++)
        //        {
        //            SyntaxNodeOrToken st = t.Item1.List[i];
        //            if (st.IsKind(SyntaxKind.IdentifierToken) || st.IsKind(SyntaxKind.StringLiteralToken) || st.IsKind(SyntaxKind.NumericLiteralToken))
        //            {
        //                bool dym = false;
        //                if (i < t.Item1.Length())
        //                {
        //                    dym = IsDym(st);
        //                }

        //                if (!dym) continue;

        //                DymToken dt = new DymToken(st, true);
        //                int v;
        //                if (!temp.TryGetValue(dt, out v))
        //                {
        //                    temp.Add(dt, 0);
        //                }

        //                RawDymToken rdt = new RawDymToken(st, true);
        //                if (!temp.TryGetValue(rdt, out v))
        //                {
        //                    temp.Add(rdt, 0);
        //                }
        //            }
        //        }

        //        foreach (DymToken dt in temp.Keys)
        //        {
        //            int v;
        //            if (!Dict.TryGetValue(dt, out v))
        //            {
        //                Dict.Add(dt, 0);
        //            }
        //            Dict[dt] = v + 1;
        //        }
        //    }

        //    temp = new Dictionary<DymToken, int>();
        //    foreach (DymToken dt in Dict.Keys)
        //    {
        //        if (Dict[dt] == examples.Count())
        //        {
        //            if (!(dt is RawDymToken))
        //            {
        //                temp.Add(dt, examples.Count());
        //            }
        //            else if (Dict[(new DymToken(dt.token, true))] != examples.Count)
        //            {
        //                temp.Add(dt, examples.Count());
        //            }
        //        }
        //    }

        //    Dict = temp;
        //}

        /// <summary>
        /// Create dynamic tokens
        /// </summary>
        /// <param name="examples">Examples</param>
        private void CreateDymTokens(List<Tuple<ListNode, ListNode>> examples, bool _getFullyQualifiedName)
        {
            //_getFullyQualifiedName = false;
            if (examples == null) { throw new ArgumentNullException("examples"); }

            foreach (Tuple<ListNode, ListNode> t in examples)
            {
                for (int i = 0; i < t.Item1.List.Count; i++)
                {
                    SyntaxNodeOrToken st = t.Item1.List[i];
                    if (st.IsKind(SyntaxKind.IdentifierToken) || st.IsKind(SyntaxKind.StringLiteralToken) || st.IsKind(SyntaxKind.NumericLiteralToken))
                    {
                        bool dym = false;
                        if (i < t.Item1.Length())
                        {
                            dym = IsDym(st);
                        }

                        if (!dym) continue;

                        DymToken dt = new DymToken(st, _getFullyQualifiedName);
                        List<DymToken> v;
                        if (!Dict.TryGetValue(dt, out v))
                        {
                            Dict.Add(dt, new List<DymToken>());
                        }
                        Dict[dt].Add(dt);

                        RawDymToken rdt = new RawDymToken(st);
                        if (!Dict.TryGetValue(rdt, out v))
                        {
                            Dict.Add(rdt, new List<DymToken>());
                        }
                        Dict[rdt].Add(dt);
                    }
                }
            }

            Dictionary<DymToken, List<DymToken>> temp = new Dictionary<DymToken, List<DymToken>>();
            foreach (DymToken dt in Dict.Keys)
            {
                if (Dict[dt].Count >= examples.Count())
                {
                    temp.Add(dt, Dict[dt]);
                }
            }

            temp = new Dictionary<DymToken, List<DymToken>>();
            foreach (DymToken dt in Dict.Keys)
            {
                if (Dict[dt].Count == examples.Count())
                {
                    if (!(dt is RawDymToken))
                    {
                        temp.Add(dt, Dict[dt]);
                    }
                    else if (Dict[(new DymToken(dt.token, true))].Count != examples.Count)
                    {
                        temp.Add(dt, Dict[dt]);
                    }
                }
            }

            Dict = temp;
            temp = new Dictionary<DymToken, List<DymToken>>();
            foreach (var entry in Dict)
            {
                if (_getFullyQualifiedName && !(entry.Key is RawDymToken))
                {
                    bool isFullName = true;
                    string fullName = entry.Value.First().dynType.fullName;
                    foreach (var dymToken in entry.Value)
                    {
                        if (dymToken.dynType.type.Equals(DynType.FULLNAME) && dymToken.dynType.fullName.Equals(fullName))
                        {
                            continue;
                        }
                        else
                        {
                            isFullName = false;
                            break;
                        }
                    }

                    if (isFullName)
                    {
                        isFullName = IsSameDeclaraction(entry.Value);
                    }

                    if (isFullName)
                    {
                        temp.Add(entry.Key, entry.Value);
                    }
                    else
                    {
                        entry.Key.dynType.type = DynType.STRING;
                        entry.Key.dynType.fullName = entry.Key.token.ToString();
                        temp.Add(entry.Key, new List<DymToken> { entry.Key });
                    }
                }
                else
                {
                    temp.Add(entry.Key, entry.Value);
                }
            }
            Dict = temp;
        }

        private bool IsSameDeclaraction(List<DymToken> value)
        {
            var sourceSpan = value.First().dynType.symbol.Locations.First().SourceSpan;
            for(int i = 1; i < value.Count; i++){
                var span = value[i].dynType.symbol.Locations.First().SourceSpan;

                if(!(sourceSpan.Start == span.Start && sourceSpan.Length == span.Length))
                {
                    return false;
                }

            }
            return true;
        }

        /// <summary>
        /// Is dynamic token
        /// </summary>
        /// <param name="st">Syntax token or node</param>
        /// <returns>True if is a dynamic token</returns>
        private static bool IsDym(SyntaxNodeOrToken st)
        {
            if (st == null) { throw new ArgumentNullException("st"); }

            if (st.IsKind(SyntaxKind.StringLiteralToken)) { return true; }

            if (st.IsKind(SyntaxKind.NumericLiteralToken)) { return true; }

            if (st.IsKind(SyntaxKind.IdentifierToken)) { return true; }

            //if (!st.IsKind(SyntaxKind.IdentifierToken)) { return false; }

            //SyntaxNodeOrToken parent = ASTManager.Parent(st);

            //if (parent.IsKind(SyntaxKind.VariableDeclaration)) { return true; }

            //if (parent.IsKind(SyntaxKind.ObjectCreationExpression)) { return true; }

            //if (parent.IsKind(SyntaxKind.AttributeList)) { return true; }

            //if (parent.IsKind(SyntaxKind.InvocationExpression)) { return true; }

            //if (parent.IsKind(SyntaxKind.QualifiedName)) { return true; }

            //if (parent.IsKind(SyntaxKind.IfStatement)) { return true; }

            //if (parent.IsKind(SyntaxKind.MethodDeclaration)) { return true; }

            //if (parent.IsKind(SyntaxKind.Parameter)) { return true; }

            //if (parent.IsKind(SyntaxKind.SimpleMemberAccessExpression)) { return true; }

            //if (parent.IsKind(SyntaxKind.TypeArgumentList)) { return true; }

            //if(parent.IsKind(SyntaxKind.VariableDeclarator)) { return true;  }

            //if (parent.IsKind(SyntaxKind.Attribute)) { return true; }

            return false;
        }

        /// <summary>
        /// Clear entry of the graph that does not contains expressions.
        /// </summary>
        /// <param name="dag">Dag</param>
        /// <exception cref="ArgumentNullException">Exception if dag is null</exception>
        public static void Clear(Dag dag)
        {
            if (dag == null) { throw new ArgumentNullException("dag"); }

            if (dag.Vertexes == null || dag.dag == null || dag.Mapping == null) { throw new ArgumentException("Some property of Dag is null"); }

            Dictionary<Tuple<Vertex, Vertex>, Dictionary<ExpressionKind, List<IExpression>>> dictionary = dag.Mapping;
            List<Tuple<Vertex, Vertex>> removes = new List<Tuple<Vertex, Vertex>>();
            foreach (KeyValuePair<Tuple<Vertex, Vertex>, Dictionary<ExpressionKind, List<IExpression>>> entry in dictionary)
            {
                bool rm = true;
                foreach (KeyValuePair<ExpressionKind, List<IExpression>> item in entry.Value)
                {
                    if (!(item.Value.Count == 0))
                    {
                        rm = false;
                    }
                }
                if (rm)
                {
                    removes.Add(entry.Key);
                }

            }

            foreach (Tuple<Vertex, Vertex> entry in removes)
            {
                dictionary.Remove(entry);
                if (dag.dag.HasEdge(entry.Item1.Id, entry.Item2.Id))
                {
                    dag.dag.RemoveEdge(entry.Item1.Id, entry.Item2.Id);
                }
            }
        }

        /// <summary>
        /// Return Dag of expressions
        /// </summary>
        /// <param name="input">Input nodes</param>
        /// <param name="output">Output nodes</param>
        /// <returns>Dag of expressions</returns>
        public Dag GenerateString2(ListNode input, ListNode output)
        {
            DirectedGraph dag = new DirectedGraph();
            Dictionary<String, Vertex> vertexes = new Dictionary<String, Vertex>();

            for (int i = 0; i <= output.Length(); i++)
            {
                Vertex v = new Vertex(i.ToString(), 0);
                vertexes.Add(i.ToString(), v);
                dag.AddVertex(v);
            }

            //List<int> n_sorces = new List<int>();
            //n_sorces.Add(0);

            Dictionary<Tuple<Vertex, Vertex>, Dictionary<ExpressionKind, List<IExpression>>> W = new Dictionary<Tuple<Vertex, Vertex>, Dictionary<ExpressionKind, List<IExpression>>>();
            Dictionary<int, List<IPosition>> kpositions = new Dictionary<int, List<IPosition>>();
            for (int i = 0; i <= output.Length(); i++)
            {
                for (int j = i + 1; j <= output.Length(); j++)
                {
                    Tuple<Vertex, Vertex> tuple = Tuple.Create(vertexes[i.ToString()], vertexes[j.ToString()]);

                    Dictionary<ExpressionKind, List<IExpression>> synthExpressions = new Dictionary<ExpressionKind, List<IExpression>>();

                    ListNode subNodes = ASTManager.SubNotes(output, i, (j - i));
                    if (Setting.ConsiderConstrStr)
                    {
                        List<IExpression> subStrExpressions = new List<IExpression>();
                        IExpression expression = new ConstruStr(subNodes);
                        subStrExpressions.Add(expression);
                        synthExpressions.Add(ExpressionKind.Consttrustr, subStrExpressions);

                        //if (subNodes.Length() == 1 && subNodes.List.First().IsKind(SyntaxKind.IdentifierToken))
                        //{
                        //    List<IExpression> fakeConstStrExps = new List<IExpression>();
                        //    IExpression fakeConstrStr = new FakeConstrStr(subNodes);
                        //    fakeConstStrExps.Add(fakeConstrStr);
                        //    synthExpressions.Add(ExpressionKind.FakeConstrStr, fakeConstStrExps);

                        //}
                    }

                    //List<IExpression> subStrns = new List<IExpression>();
                    List<IExpression> expressions = GenerateNodes(input, subNodes, kpositions);
                    expressions = MinimizeExpressions(expressions);

                    //subStrns.AddRange(expressions);
                    if (expressions.Any())
                    {
                        synthExpressions.Add(ExpressionKind.SubStr, expressions);
                    }

                    List<IExpression> idenExpr = GenerateIdentToStr(input, subNodes, kpositions);

                    if (idenExpr.Any())
                    {
                        synthExpressions.Add(ExpressionKind.Identostr, idenExpr);
                    }

                    W.Add(tuple, synthExpressions);
                }
            }

            Dag digraph = new Dag(dag, vertexes["0"], vertexes[output.Length().ToString()], W, vertexes);

            foreach (var m in W.Keys)
            {
                digraph.dag.AddEdge(m.Item1.ToString(), m.Item2.ToString());
            }

            return digraph;
        }


        /// <summary>
        /// Dag of expressions
        /// </summary>
        /// <param name="input">Input nodes</param>
        /// <param name="output">Output nodes</param>
        /// <param name="boundaryPoints">boundaryPoints</param>
        /// <returns>Dag of expressions</returns>
        public Dag GenerateStringBoundary(ListNode input, ListNode output, List<int> boundaryPoints)
        {
            if (boundaryPoints == null)
            {
                return GenerateString2(input, output);
            }
            DirectedGraph dag = new DirectedGraph();

            Dictionary<String, Vertex> vertexes = new Dictionary<String, Vertex>();

            for (int i = 0; i <= output.Length(); i++)
            {
                Vertex v = new Vertex(i.ToString(), 0);
                vertexes.Add(i.ToString(), v);
                dag.AddVertex(v);
            }

            //List<int> n_sorces = new List<int>();
            //n_sorces.Add(0);

            Dictionary<Tuple<Vertex, Vertex>, Dictionary<ExpressionKind, List<IExpression>>> W = new Dictionary<Tuple<Vertex, Vertex>, Dictionary<ExpressionKind, List<IExpression>>>();
            Dictionary<int, List<IPosition>> kpositions = new Dictionary<int, List<IPosition>>();
            for (int i = 0; i < boundaryPoints.Count; i++)
            {
                for (int j = i + 1; j < boundaryPoints.Count; j++)
                {
                    Tuple<Vertex, Vertex> tuple = Tuple.Create(vertexes[boundaryPoints[i].ToString()], vertexes[boundaryPoints[j].ToString()]);
                    Dictionary<ExpressionKind, List<IExpression>> synthExpressions = new Dictionary<ExpressionKind, List<IExpression>>();

                    ListNode subNodes = ASTManager.SubNotes(output, boundaryPoints[i], boundaryPoints[j] - boundaryPoints[i]);
                    if (Setting.ConsiderConstrStr)
                    {
                        List<IExpression> constStrExprs = new List<IExpression>();
                        IExpression expression = new ConstruStr(subNodes);
                        constStrExprs.Add(expression);
                        synthExpressions.Add(ExpressionKind.Consttrustr, constStrExprs);

                        //if (subNodes.Length() == 1 && subNodes.List.First().IsKind(SyntaxKind.IdentifierToken))
                        //{
                        //    List<IExpression> fakeConstStrExps = new List<IExpression>();
                        //    IExpression fakeConstrStr = new FakeConstrStr(subNodes);
                        //    fakeConstStrExps.Add(fakeConstrStr);
                        //    synthExpressions.Add(ExpressionKind.FakeConstrStr, fakeConstStrExps);
                        //}
                    }
                    List<IExpression> expressions = GenerateNodes(input, subNodes, kpositions);
                    expressions = MinimizeExpressions(expressions);
                    if (expressions.Any())
                    {
                        synthExpressions.Add(ExpressionKind.SubStr, expressions);
                    }

                    List<IExpression> idenExpr = GenerateIdentToStr(input, subNodes, kpositions);

                    if (idenExpr.Any())
                    {
                        synthExpressions.Add(ExpressionKind.Identostr, idenExpr);
                    }

                    if (!W.ContainsKey(tuple))
                    {
                        W.Add(tuple, synthExpressions);
                    }
                }
            }
            /*Tuple<Vertex, Vertex> zero = Tuple.Create(vertexes["0"], vertexes["0"]);
            if (!W.ContainsKey(zero))
            {
                IExpression expression = new ConstruStr(new ListNode(new List<SyntaxNodeOrToken>()));
                List<IExpression> expressions = new List<IExpression> {expression};
                W.Add(zero, expressions);
            }*/

            Dag digraph = new Dag(dag, vertexes["0"], vertexes[output.Length().ToString()], W, vertexes);

            foreach (var m in W.Keys)
            {
                digraph.dag.AddEdge(m.Item1.ToString(), m.Item2.ToString());
            }

            return digraph;
        }

        private List<IExpression> MinimizeExpressions(List<IExpression> expressions)
        {
            //var items = from expression in expressions
            //            orderby new SelectorManager(setting).Order(expression) descending
            //            select expression;

            //List<IExpression> topExpressions = new List<IExpression>(items).GetRange(0, Math.Min(100, expressions.Count()));
            //return topExpressions;


            //List<IExpression> topExpressions = expressions.GetRange(0, Math.Min(500, expressions.Count()));
            //return topExpressions;
            return expressions;
        }

        /// <summary>
        /// Generate a list of sequence or tokens given the information of the nodes 
        /// of the tree.
        /// </summary>
        /// <param name="subNodesLeft">Nodes of the tree that is being analyzed.</param>
        /// <returns>List of tokens seq</returns>
        public List<TokenSeq> GenerateTokenSeq(ListNode subNodesLeft)
        {
            List<TokenSeq> tokensSeqs;
            if (!_computed.TryGetValue(subNodesLeft, out tokensSeqs))
            {
                tokensSeqs = new List<TokenSeq>();
                List<Token> tSeq = TokenSeq.GetTokens(subNodesLeft);

                TokenSeq ts = new TokenSeq(tSeq);

                if (Setting.CreateTokenSeq)
                {
                    List<TokenSeq> parentNodes = CreateTokenSeq(ts);
                    tokensSeqs.AddRange(parentNodes);
                }

                if (Setting.DynamicTokens)
                {
                    List<Token> dTSeq = TokenSeq.DymTokens(subNodesLeft, this.Dict);
                    TokenSeq dtSeq = new TokenSeq(dTSeq);
                    tokensSeqs.Add(dtSeq);
                    //                   tokensSeqs.Add(AddIdenToStr(ts));
                }
                //else
                //{
                tokensSeqs.Add(ts);
                //}
                if (subNodesLeft.List.Count > 0)
                {
                    List<Token> emptySeq = TokenSeq.GetTokens(new ListNode());
                    TokenSeq emptyTokenSeq = new TokenSeq(emptySeq);
                    tokensSeqs.Add(emptyTokenSeq);
                }

                _computed.Add(subNodesLeft, tokensSeqs);
            }

            tokensSeqs = _computed[subNodesLeft];

            return tokensSeqs;
        }

        ///// <summary>
        ///// Create token sequence
        ///// </summary>
        ///// <param name="subNodes">Sub nodes</param>
        ///// <returns>Token sequence</returns>
        //public static List<ListNode> CreateTokenSeq(ListNode subNodes)
        //{
        //    Dictionary<SyntaxNodeOrToken, List<SyntaxNodeOrToken>> nodes = new Dictionary<SyntaxNodeOrToken, List<SyntaxNodeOrToken>>();
        //    foreach (SyntaxNodeOrToken st in subNodes.List)
        //    {
        //        List<SyntaxNodeOrToken> value;
        //        SyntaxNodeOrToken parent = ASTManager.Parent(st);
        //        if (!nodes.TryGetValue(parent, out value))
        //        {
        //            nodes.Add(parent, new List<SyntaxNodeOrToken>());
        //        }

        //        nodes[parent].Add(st);
        //    }

        //    List<SyntaxNodeOrToken> selected = new List<SyntaxNodeOrToken>(nodes.Keys);

        //    List<ListNode> list = new List<ListNode>();
        //    list.Add(subNodes);
        //    foreach (SyntaxNodeOrToken st in selected)
        //    {
        //        list = Substitute(list, st, nodes);
        //    }

        //    list.Remove(subNodes);

        //    return list;
        //}

        /// <summary>
        /// Create token sequence
        /// </summary>
        /// <param name="seq">Sub nodes</param>
        /// <returns>Token sequence</returns>
        public static List<TokenSeq> CreateTokenSeq(TokenSeq seq)
        {
            List<Token> tokens = new List<Token>();
            bool add = false;
            foreach (Token st in seq.Tokens)
            {
                Token ai = new ArrayInitializerElementToken(st.token);
                if (ai.Match(st.token))
                {
                    tokens.Add(ai);
                    add = true;
                }
                else
                {
                    tokens.Add(st);
                }
            }

            List<TokenSeq> sequences = new List<TokenSeq>();
            if (add)
            {
                TokenSeq sequence = new TokenSeq(tokens);
                sequences.Add(sequence);
            }

            //sequences.AddRange(AddArgument(seq));
            //sequences.Add(AddIdenToStr(seq));

            return sequences;
        }

        //private static IEnumerable<TokenSeq> AddArgument(TokenSeq seq)
        //{
        //    List<Token> argumentTokens = new List<Token>();
        //    bool addArgument = false;
        //    bool previousIsArgument = false;
        //    foreach (Token st in seq.Tokens)
        //    {
        //        Token at = new ArgumentToken(st.token);
        //        if (at.Match(st.token))
        //        {
        //            if (!previousIsArgument)
        //            {
        //                argumentTokens.Add(at);
        //            }
        //            previousIsArgument = true;
        //            addArgument = true;
        //        }
        //        else
        //        {
        //            argumentTokens.Add(st);
        //            previousIsArgument = false;
        //        }
        //    }
        //    List<TokenSeq> sequences = new List<TokenSeq>();
        //    if (addArgument)
        //    {
        //        TokenSeq sequence = new TokenSeq(argumentTokens);
        //        sequences.Add(sequence);
        //    }

        //    return sequences;
        //}

        ///// <summary>
        ///// Create sequence of token with IdentToStrToken inserted where applicable
        ///// </summary>
        ///// <param name="seq">Sequence of tokens</param>
        ///// <returns>sequence of token with IdentToStrToken inserted where applicable</returns>
        //private static TokenSeq AddIdenToStr(TokenSeq seq)
        //{
        //    List<Token> addIdentoToken = new List<Token>();

        //    foreach (Token st in seq.Tokens)
        //    {
        //        if (st.token.IsKind(SyntaxKind.IdentifierToken))
        //        {
        //            addIdentoToken.Add(new IdenToStrToken(st.token));
        //        }
        //        else
        //        {
        //            addIdentoToken.Add(st);
        //        }
        //    }

        //    TokenSeq sequence = new TokenSeq(addIdentoToken);


        //    return sequence;
        //}

        /// <summary>
        /// Substitute
        /// </summary>
        /// <param name="previousTransfosrmations">Previous transformation list.</param>
        /// <param name="parentNode">Parent node of elements in the list.</param>
        /// <param name="parentDictionary">Parent dictionary, the key are the parent and the value are the childrens</param>
        /// <returns>List of transformation after substitution of all children for parent</returns>
        public static List<ListNode> Substitute(List<ListNode> previousTransfosrmations, SyntaxNodeOrToken parentNode, Dictionary<SyntaxNodeOrToken, List<SyntaxNodeOrToken>> parentDictionary)
        {
            List<ListNode> lnodes = new List<ListNode>();

            foreach (ListNode ns in previousTransfosrmations)
            {
                ListNode parent = new ListNode(parentDictionary[parentNode]);
                List<int> matches = ASTManager.Matches(ns, parent, new NodeComparer());

                if (matches.Count > 0)
                {
                    List<SyntaxNodeOrToken> sts = new List<SyntaxNodeOrToken>();
                    for (int i = 0; i < matches[0]; i++)
                    {
                        sts.Add(ns.List.ElementAt(i));
                    }
                    sts.Add(parentNode);

                    for (int i = matches[0] + parentDictionary[parentNode].Count; i < ns.List.Count; i++)
                    {
                        sts.Add(ns.List.ElementAt(i));
                    }
                    lnodes.Add(new ListNode(sts));
                }
            }
            previousTransfosrmations.AddRange(lnodes);
            return previousTransfosrmations;
        }
        /*
        /// <summary>
        /// Substitute
        /// </summary>
        /// <param name="pTrans">pTransformation</param>
        /// <param name="selection">Selection</param>
        /// <param name="nodes">ListNode</param>
        /// <returns>ListNode after substitution</returns>
        public static List<ListNode> Substitute(List<ListNode> pTrans, SyntaxNodeOrToken selection, Dictionary<SyntaxNodeOrToken, List<SyntaxNodeOrToken>> nodes)
        {
            List<ListNode> lnodes = new List<ListNode>();
            foreach (ListNode ns in pTrans)
            {
                ListNode subNSelection = new ListNode(nodes[selection]);
                List<int> matches = ASTManager.Matches(ns, subNSelection, new NodeComparer());
                if (matches.Count > 0)
                {
                    List<SyntaxNodeOrToken> sts = new List<SyntaxNodeOrToken>();
                    for (int i = 0; i < matches[0]; i++)
                    {
                        sts.Add(ns.List.ElementAt(i));
                    }

                    sts.Add(selection);

                    for (int i = matches[0] + nodes[selection].Count; i < ns.List.Count; i++)
                    {
                        sts.Add(ns.List.ElementAt(i));
                    }

                    lnodes.Add(new ListNode(sts));
                }
            }
            pTrans.AddRange(lnodes);
            return pTrans;
        }*/

        /// <summary>
        /// String examples to ListNode examples
        /// </summary>
        /// <param name="examples">Examples</param>
        /// <returns>ListNode representation</returns>
        public static List<Tuple<ListNode, ListNode>> Examples(List<Tuple<string, string>> examples)
        {
            List<Tuple<ListNode, ListNode>> data = new List<Tuple<ListNode, ListNode>>();
            foreach (Tuple<string, string> example in examples)
            {
                Tuple<ListNode, ListNode> tuple = Example(example);

                data.Add(tuple);
            }

            return data;
        }

        /// <summary>
        /// String example to ListNode example
        /// </summary>
        /// <param name="example"></param>
        /// <returns></returns>
        public static Tuple<ListNode, ListNode> Example(Tuple<String, String> example)
        {
            SyntaxTree tree1 = CSharpSyntaxTree.ParseText(example.Item1);
            SyntaxTree tree2 = CSharpSyntaxTree.ParseText(example.Item2);

            SyntaxNode inputRoot = tree1.GetRoot();
            List<SyntaxNodeOrToken> inputList = new List<SyntaxNodeOrToken>();
            inputList = ASTManager.EnumerateSyntaxNodesAndTokens(inputRoot, inputList);

            ListNode input = new ListNode(inputList);
            input.OriginalText = example.Item1;

            SyntaxNode outputRoot = tree2.GetRoot();
            List<SyntaxNodeOrToken> outputList = new List<SyntaxNodeOrToken>();
            outputList = ASTManager.EnumerateSyntaxNodesAndTokens(outputRoot, outputList);

            ListNode output = new ListNode(outputList);
            output.OriginalText = example.Item2;

            Tuple<ListNode, ListNode> tuple = Tuple.Create(input, output);
            return tuple;
        }

        /// <summary>
        /// Convert syntax node example to ListNode example
        /// </summary>
        /// <param name="example">Syntax nodes example</param>
        /// <returns>ListNode example</returns>
        public static Tuple<ListNode, ListNode> Example(Tuple<SyntaxNode, SyntaxNode> example)
        {
            SyntaxNode inputRoot = example.Item1;
            List<SyntaxNodeOrToken> inputList = new List<SyntaxNodeOrToken>();
            inputList = ASTManager.EnumerateSyntaxNodesAndTokens(inputRoot, inputList);

            ListNode input = new ListNode(inputList);
            input.OriginalText = example.Item1.GetText().ToString();

            SyntaxNode outputRoot = example.Item2;
            List<SyntaxNodeOrToken> outputList = new List<SyntaxNodeOrToken>();
            outputList = ASTManager.EnumerateSyntaxNodesAndTokens(outputRoot, outputList);

            ListNode output = new ListNode(outputList);
            output.OriginalText = example.Item2.GetText().ToString();

            Tuple<ListNode, ListNode> tuple = Tuple.Create(input, output);
            return tuple;
        }

        /// <summary>
        /// Generate nodes
        /// </summary>
        /// <param name="inputTree">Input ListNode</param>
        /// <param name="subNodes">Substring ListNode</param>
        /// <param name="kpositions">K positions</param>
        /// <returns>Expression list</returns>
        private List<IExpression> GenerateNodes(ListNode inputTree, ListNode subNodes, Dictionary<int, List<IPosition>> kpositions)
        {
            List<IExpression> result = new List<IExpression>();

            List<int> positionsIndexs = GetIndexesSubNodes(inputTree, subNodes);

            foreach (int k in positionsIndexs)
            {
                List<IPosition> y1 = null;
                if (!kpositions.TryGetValue(k, out y1))
                {
                    y1 = GeneratePosition(inputTree, k);
                    kpositions.Add(k, y1);
                }

                List<IPosition> y2 = null;
                if (!kpositions.TryGetValue((k + subNodes.Length()), out y2))
                {
                    y2 = GeneratePosition(inputTree, k + subNodes.Length());
                    kpositions.Add((k + subNodes.Length()), y2);
                }

                foreach (Tuple<IPosition, IPosition> positions in ConstructCombinations(y1, y2))
                {
                    IExpression expression = new SubStr(positions.Item1, positions.Item2);
                    result.Add(expression);
                }
            }
            return result;
        }

        /// <summary>
        /// Generate nodes
        /// </summary>
        /// <param name="inputTree">Input ListNode</param>
        /// <param name="subNodes">Substring ListNode</param>
        /// <param name="kpositions">K positions</param>
        /// <returns>Expression list</returns>
        private List<IExpression> GenerateIdentToStr(ListNode inputTree, ListNode subNodes, Dictionary<int, List<IPosition>> kpositions)
        {
            List<IExpression> result = new List<IExpression>();

            if (!(subNodes.Length() == 1 && subNodes.List[0].IsKind(SyntaxKind.StringLiteralToken)))
            {
                return result;
            }

            List<int> positionsIndexs = GetIndexesSubNodesIdenToStr(inputTree, subNodes);

            foreach (int k in positionsIndexs)
            {
                List<IPosition> y1 = null;
                if (!kpositions.TryGetValue(k, out y1))
                {
                    y1 = GeneratePosition(inputTree, k);
                    kpositions.Add(k, y1);
                }

                List<IPosition> y2 = null;
                if (!kpositions.TryGetValue((k + subNodes.Length()), out y2))
                {
                    y2 = GeneratePosition(inputTree, k + subNodes.Length());
                    kpositions.Add((k + subNodes.Length()), y2);
                }

                foreach (Tuple<IPosition, IPosition> positions in ConstructCombinations(y1, y2))
                {
                    IExpression expression = new IdenToStr(positions.Item1, positions.Item2);
                    result.Add(expression);
                }
            }
            return result;
        }

        /// <summary>
        /// Generate positions
        /// </summary>
        /// <param name="input">Input region</param>
        /// <param name="k">An index on input region</param>
        /// <returns>Position list</returns>
        public List<IPosition> GeneratePosition(ListNode input, int k, bool neg = true) {
            
            List<IPosition> positions = new List<IPosition>();

            for(int i = 1; i < 3; i++)
            {
                Setting.Deviation = i;
                List<IPosition> positionsi = GeneratePositionFlashFill(input, k, neg);
                positions.AddRange(positionsi);
            }

            var noDupes = new HashSet<IPosition>(positions);

            return noDupes.ToList();
            
        }

        /// <summary>
        /// Genetate positions for a given input and position on this input
        /// </summary>
        /// <param name="input">Input region</param>
        /// <param name="k">Index in which the position will be generated.</param>
        /// <returns></returns>
        public List<IPosition> GeneratePositionFlashFill(ListNode input, int k, bool neg = true)
        {
            List<IPosition> result = new List<IPosition>();
            if (neg)
            {
                result.Add(CPos(k)); result.Add(CPos(-(input.Length() - k + 1)));
            }
            //int deviation = 2;

            int k1 = Math.Max(k - Setting.Deviation, 0);
            int k2 = Math.Min(k + Setting.Deviation, input.Length());

            int i = Math.Max(0, k1);
            int j = Math.Max(0, (k - k1));

            ListNode subNodesLeft = ASTManager.SubNotes(input, i, j);
            // Match string s[k1, k - 1] for some constant k1
            List<TokenSeq> r1 = GenerateTokenSeq(subNodesLeft);

            ListNode subNodesRight = ASTManager.SubNotes(input, Math.Max(0, k), Math.Min(k2 - k, input.Length()));

            //Match string s[k, k2] for some constant k2
            List<TokenSeq> r2 = GenerateTokenSeq(subNodesRight);
            foreach (TokenSeq r11 in r1)
            {
                foreach (TokenSeq r22 in r2)
                {
                    if (r11.Tokens.Any() || r22.Tokens.Any())
                    {
                        TokenSeq r12 = ConcatenateRegularExpression(r11, r22);//Equivalent to TokenSeq(T1, T2,...,Tn, T{'}1, T{'}2,...,T{'}m)
                        TokenSeq regex = r12;
                        //ListNode subNodesk1k2 = ASTManager.SubNotes(input, k1, k2 - k1);
                        int c = IndexOfMatchOfTheNodes(input, r11, r22, k1, k2);
                        int cline = new RegexComparer().Matches(input, regex).Count;//ASTManager.Matches(input, regex, new RegexComparer()).Count();
                        TokenSeq r1Line = r11; //maybe you will need the raw type to execute search on the tree.
                        TokenSeq r2Line = r22;

                        result.Add(Pos(r1Line, r2Line, c)); //This need to be refactored.

                        if (neg)
                        {
                            result.Add(Pos(r1Line, r2Line, -(cline - c + 1)));
                        }
                    }
                }
            }
            return result;
        }



        ///// <summary>
        ///// Generate a set of Pos position expressions
        ///// </summary>
        ///// <param name="input">Input nodes</param>
        ///// <param name="k">Position k on the input to be analyzed</param>
        ///// <returns>Set of Pos expressions</returns>
        //public List<IPosition> GeneratePos(ListNode input, int k)
        //{
        //    List<IPosition> result = new List<IPosition>();

        //    int k1 = Math.Max(k - Setting.Deviation, 0);
        //    int k2 = Math.Min(k + Setting.Deviation, input.Length());

        //    int i = Math.Max(0, k1);
        //    int j = Math.Max(0, (k - k1));

        //    ListNode subNodesLeft = ASTManager.SubNotes(input, i, j);
        //    // Match string s[k1, k - 1] for some constant k1
        //    List<TokenSeq> r1 = GenerateTokenSeq(subNodesLeft);

        //    ListNode subNodesRight = ASTManager.SubNotes(input, Math.Max(0, k), Math.Min(k2 - k, input.Length()));

        //    //Match string s[k, k2] for some constant k2
        //    List<TokenSeq> r2 = GenerateTokenSeq(subNodesRight);
        //    foreach (TokenSeq r11 in r1)
        //    {
        //        foreach (TokenSeq r22 in r2)
        //        {
        //            if (r11.Tokens.Any() || r22.Tokens.Any())
        //            {
        //                TokenSeq r1Line = r11; //maybe you will need the raw type to execute search on the tree.
        //                TokenSeq r2Line = r22;

        //                result.Add(Pos(r1Line, r2Line, 0)); //This need to be refactored.
        //            }
        //        }
        //    }
        //    return result;
        //}

        /// <summary>
        /// Index of matches
        /// </summary>
        /// <param name="input">Input list nodes</param>
        /// <param name="r1">Left regular expression</param>
        /// <param name="r2">Right regular expression</param>
        /// <param name="k1">Index of begin search</param>
        /// <param name="k2">Index of end search</param>
        /// <returns>Index of matches</returns>
        private static int IndexOfMatchOfTheNodes(ListNode input, TokenSeq r1, TokenSeq r2, int k1, int k2)
        {
            TokenSeq r12 = ConcatenateRegularExpression(r1, r2);//Equivalent to TokenSeq(T1, T2,...,Tn, T{'}1, T{'}2,...,T{'}m)
            TokenSeq regex = r12;
            var matches = new RegexComparer().Matches(input, regex);//ASTManager.Matches(input, regex, new RegexComparer());
            for (int i = 0; i < matches.Count; i++)
            {
                int positionIndex = Position.Pos.GetPositionIndex(input, r1, r2, i + 1);
                if (positionIndex >= k1 && positionIndex <= k2)
                {
                    return i + 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// Return the list of indexes that match the regular expression
        /// </summary>
        /// <param name="input">Input nodes</param>
        /// <param name="regex">Regular expressions nodes</param>
        /// <returns>List of index matches.</returns>
        private static List<int> GetIndexesSubNodes(ListNode input, ListNode regex)
        {
            //return ASTManager.Matches(input, regex, new RegexComparer());
            return new NodeComparer().Matches(input, regex);
        }

        /// <summary>
        /// Return the list of indexes that match the regular expression
        /// </summary>
        /// <param name="input">Input nodes</param>
        /// <param name="regex">Regular expressions nodes</param>
        /// <returns>List of index matches.</returns>
        private static List<int> GetIndexesSubNodesIdenToStr(ListNode input, ListNode regex)
        {
            //return ASTManager.Matches(input, regex, new RegexComparer());
            return new IdenToStrComparer().Matches(input, regex);
        }


        /// <summary>
        /// Construct all combination for y1 and y2
        /// </summary>
        /// <param name="y1">First position list</param>
        /// <param name="y2">Second position list</param>
        /// <returns>Combinations</returns>
        public static List<Tuple<IPosition, IPosition>> ConstructCombinations(IEnumerable<IPosition> y1, IEnumerable<IPosition> y2)
        {
            List<Tuple<IPosition, IPosition>> combinations = new List<Tuple<IPosition, IPosition>>();

            foreach (IPosition pos1 in y1)
            {
                foreach (IPosition pos2 in y2)
                {
                    Tuple<IPosition, IPosition> combination = Tuple.Create(pos1, pos2);
                    combinations.Add(combination);
                }
            }

            return combinations;
        }

        /// <summary>
        /// Concatenate regular expressions r1 and r2
        /// </summary>
        /// <param name="r1">First regular expression</param>
        /// <param name="r2">Second regular expression</param>
        /// <returns>Regular expression concatenation</returns>
        public static TokenSeq ConcatenateRegularExpression(TokenSeq r1, TokenSeq r2)
        {
            List<Token> tokens = new List<Token>(r1.Tokens);
            tokens.AddRange(r2.Tokens);

            TokenSeq r12 = new TokenSeq(tokens);

            return r12;
        }

        /// <summary>
        /// Return nodes that match the solution
        /// </summary>
        /// <param name="example">List of examples</param>
        /// <param name="solutions">Hypothesis</param>
        /// <returns>Nodes that match the hypothesis</returns>
        public static ListNode RetrieveNodes(Tuple<ListNode, ListNode> example, List<IExpression> solutions)
        {
            ListNode concatenation = new ListNode();
            foreach (IExpression expression in solutions)
            {
                if (expression.IsPresentOn(example))
                {
                    ListNode substring = expression.RetrieveSubNodes(example.Item1);
                    concatenation.List.AddRange(substring.List);
                }
                else // If expression is not present on the example return null.
                {
                    return null;
                }
            }

            if (concatenation.Length() == 0)
            { //This need to be refactored.
                return null;
            }

            return concatenation;
        }

        /// <summary>
        /// Extract ASTTransfomration using synthesized program
        /// </summary>
        /// <param name="example"></param>
        /// <param name="synthesizedProgram"></param>
        /// <returns>ASTTransformation extracted</returns>
        public static ASTTransformation TransformString(string example, SynthesizedProgram synthesizedProgram)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(example);

            return UpdateASTManager.UpdateASTTree(tree, synthesizedProgram);
        }

        ///// <summary>
        ///// Retrieve the string corresponding to the hypothesis passed as parameter.
        ///// </summary>
        ///// <param name="input">Input syntax node</param>
        ///// <param name="synthesizedProgram">Synthesized program</param>
        ///// <returns></returns>
        //public static ASTTransformation TransformString(ListNode input, SynthesizedProgram synthesizedProgram)
        //{
        //    return UpdateASTManager.UpdateASTTree(input, synthesizedProgram);
        //}

        ///// <summary>
        ///// Retrieve the string corresponding to the hypothesis passed as parameter.
        ///// </summary>
        ///// <param name="input">Input syntax node</param>
        ///// <param name="synthesizedProgram">Synthesized program</param>
        ///// <returns></returns>
        //public static ASTTransformation TransformString(SyntaxNode input, SynthesizedProgram synthesizedProgram)
        //{
        //    return UpdateASTManager.UpdateASTTree(input, synthesizedProgram);
        //}

        /// <summary>
        /// Return CPos a expression
        /// </summary>
        /// <param name="k">CPos index</param>
        /// <returns>CPos expression</returns>
        private static IPosition CPos(int k)
        {
            IPosition position = new CPos(k);
            return position;
        }

        /// <summary>
        /// Returns a Pos expression.
        /// </summary>
        /// <param name="r1">First regular expression</param>
        /// <param name="r2">Second regular expression</param>
        /// <param name="k">position</param>
        /// <returns>Pos expression.</returns>
        private static IPosition Pos(TokenSeq r1, TokenSeq r2, int k)
        {
            IPosition position = new Pos(r1, r2, k);
            return position;
        }
    }
}





