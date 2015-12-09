using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Bean;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Tok;
using Spg.ExampleRefactoring.Workspace;
using Spg.LocationRefactor.Controller;
using Spg.LocationRefactor.Learn;
using Spg.LocationRefactor.Location;
using Spg.LocationRefactor.Node;
using Spg.LocationRefactor.Predicate;
using Spg.LocationRefactor.TextRegion;
using Spg.LocationRefactoring.Tok;

namespace Spg.LocationRefactor.Operator.Filter
{
    /// <summary>
    /// Filter base
    /// </summary>
    public abstract class FilterBase : IOperator
    {
        /// <summary>
        /// Predicate
        /// </summary>
        /// <returns>Predicate</returns>
        public IPredicate Predicate { get; set; }

        /// <summary>
        /// Least Common ancestor of selected nodes
        /// </summary>
        public List<SyntaxNode> Lcas { get; set; }

        /// <summary>
        /// Region list
        /// </summary>
        public List<TRegion> List;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="list">List of selected regions</param>
        protected FilterBase(List<TRegion> list)
        {
            List = list;
        }

        /// <summary>
        /// Execute filter base
        /// </summary>
        /// <param name="input">Source code</param>
        /// <returns>Statements that follows the predicate</returns>
        public ListNode Execute(string input)
        {
            SyntaxTree tree1 = CSharpSyntaxTree.ParseText(input);
            return Execute(tree1.GetRoot());
        }

        /// <summary>
        /// Run the filter on input. Verify if input nodes match with the filter
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <returns>True if this filter match with the input</returns>
        public ListNode Execute(SyntaxNode syntaxNode)
        {
            FilterLearnerBase learn = GetFilterLearner(List);
            //TokenSeq regex = Predicate.r1;

            Tuple<SyntaxNode, SyntaxNode> tuple = Tuple.Create(syntaxNode, syntaxNode);
            ListNode listNode = ASTProgram.Example(tuple).Item1;

            bool indicator = learn.Indicator(Predicate, listNode, Predicate.regex);
            if (indicator) return listNode;

            return null;
        }

        /// <summary>
        /// Evalute if node match predicate
        /// </summary>
        /// <param name="syntaxNode">Syntax node</param>
        /// <returns>True, if node match predicate, false otherwise</returns>
        public bool IsMatch(SyntaxNode syntaxNode)
        {
            FilterLearnerBase learn = GetFilterLearner(List);
            //Pos regex = new TokenSeq(Predicate.Regex());

            Tuple<SyntaxNode, SyntaxNode> tuple = Tuple.Create(syntaxNode, syntaxNode);
            ListNode listNode = ASTProgram.Example(tuple).Item1;

            bool indicator = learn.Indicator(Predicate, listNode, Predicate.regex);

            return indicator;
        }

        /// <summary>
        /// Retrieve regions
        /// </summary>
        /// <returns>Retrieved regions</returns>
        public List<TRegion> RetrieveRegion()
        {
            Dictionary<string, List<TRegion>> result = RegionManager.GetInstance().GroupRegionBySourceFile(List);
            if (result.Count == 1)
            {
                throw new Exception("RetrieveRegion with only source code parameter does not accept single file transformation.");
            }

            IEnumerable<SyntaxNode> nodesForFiltering = SyntaxNodesDymTokens();
            if (nodesForFiltering == null)
            {
                EditorController controller = EditorController.GetInstance();
                List<Tuple<string, string>> files =
                    WorkspaceManager.GetInstance()
                        .GetSourcesFiles(controller.ProjectInformation.ProjectPath,
                            controller.ProjectInformation.SolutionPath);
                nodesForFiltering = Decomposer.SyntaxNodesWithoutSemanticModel(files, Lcas);
                return RetrieveRegionsBase(nodesForFiltering);
            }
            List<TRegion> tregions = RetrieveRegionsBase(nodesForFiltering);
            return tregions;
        }

        /// <summary>
        /// Retrieve region given a syntax node
        /// </summary>
        /// <param name="syntaxNode">Syntax node</param>
        /// <param name="sourceCode">Source code</param>
        /// <returns>List of regions</returns>
        public List<TRegion> RetrieveRegion(SyntaxNode syntaxNode, string sourceCode)
        {
            IEnumerable<SyntaxNode> nodesForFiltering = SyntaxNodesDymTokens();
            if (nodesForFiltering == null)
            {
                nodesForFiltering = Decomposer.SyntaxNodesWithoutSemanticModel(syntaxNode, Lcas);
                return RetrieveRegionsBase(nodesForFiltering);
            }
            nodesForFiltering = NodesForSingleFile(nodesForFiltering, syntaxNode);
            return RetrieveRegionsBase(nodesForFiltering);
        }

        /// <summary>
        /// Calculate syntax node for filtering using dynamic tokens
        /// </summary>
        /// <returns>Syntax node used on filtering process</returns>
        internal IEnumerable<SyntaxNode> SyntaxNodesDymTokens()
        {
            IEnumerable<DymToken> nameList = GetIdentifierNames().ToList();
            if (nameList.Any())
            {
                var referenceDictionary = new Dictionary<string, IEnumerable<SyntaxNode>>();
                foreach (DymToken nameDyn in nameList)
                {
                    IEnumerable<SyntaxNode> nodes = Decomposer.GetInstance().SyntaxNodesWithSemanticModel(nameDyn, Lcas);
                    if (nodes != null)
                    {
                        if (!referenceDictionary.ContainsKey(nameDyn.dynType.fullName))
                        {
                            referenceDictionary.Add(nameDyn.dynType.fullName, nodes);
                        }
                    }
                }

                if (referenceDictionary.Count() == 1)
                {
                    //if (MatchesSelectedLocations(referenceDictionary.First().Value.ToList(), EditorController.GetInstance().SelectedLocations))
                    //{
                    return referenceDictionary.First().Value;
                    //}
                    //return null;
                }

                if (referenceDictionary.Any())
                {
                    List<SyntaxNode> nodes = GetIntersection(referenceDictionary);
                    return nodes;
                }
            }
            return null;
        }

        ///// <summary>
        ///// Calculate the intersection of references and return it.
        ///// </summary>
        ///// <param name="referenceDictionary">Dictinary of references</param>
        ///// <returns>Intersection of referenced elements on dictionary</returns>
        //private List<SyntaxNode> GetIntersection(Dictionary<string, IEnumerable<SyntaxNode>> referenceDictionary)
        //{
        //    Dictionary<string, IEnumerable<Selection>> dicSelections = new Dictionary<string, IEnumerable<Selection>>();
        //    foreach (KeyValuePair<string, IEnumerable<SyntaxNode>> keypPair in referenceDictionary)
        //    {
        //        List<Selection> selections = new List<Selection>();
        //        foreach (SyntaxNode syntaxNode in keypPair.Value)
        //        {
        //            Selection selection = new Selection(syntaxNode.Span.Start, syntaxNode.Span.Length, syntaxNode.SyntaxTree.FilePath, syntaxNode.SyntaxTree.GetText().ToString(), syntaxNode.ToFullString());
        //            selections.Add(selection);
        //        }
        //        dicSelections.Add(keypPair.Key, selections);
        //    }

        //    IEnumerable<Selection> intersection = dicSelections.Values.First();
        //    for (int i = 1; i < dicSelections.Values.Count; i++)
        //    {
        //        intersection = intersection.Intersect(dicSelections.Values.ElementAt(i));
        //    }

        //    List<Selection> enumerable = intersection as List<Selection> ?? intersection.ToList();

        //    List<SyntaxNode> nodes = new List<SyntaxNode>();

        //    foreach (Selection selection in enumerable)
        //    {
        //        string strTree = FileUtil.ReadFile(selection.SourcePath);
        //        SyntaxTree root = CSharpSyntaxTree.ParseText(strTree, path: selection.SourcePath);
        //        nodes.Add(root.GetRoot().FindNode(new TextSpan(selection.Start, selection.Length)));
        //    }
        //    return nodes;
        //}

        /// <summary>
        /// Calculate the intersection of references and return it.
        /// </summary>
        /// <param name="referenceDictionary">Dictinary of references</param>
        /// <returns>Intersection of referenced elements on dictionary</returns>
        private List<SyntaxNode> GetIntersection(Dictionary<string, IEnumerable<SyntaxNode>> referenceDictionary)
        {
            Dictionary<string, IEnumerable<Selection>> dicSelections = new Dictionary<string, IEnumerable<Selection>>();
            Dictionary<Selection, SyntaxNode> dicIntersection = new Dictionary<Selection, SyntaxNode>();

            foreach (KeyValuePair<string, IEnumerable<SyntaxNode>> keypPair in referenceDictionary)
            {
                List<Selection> selections = new List<Selection>();
                foreach (SyntaxNode syntaxNode in keypPair.Value)
                {
                    Selection selection = new Selection(syntaxNode.Span.Start, syntaxNode.Span.Length, syntaxNode.SyntaxTree.FilePath, syntaxNode.SyntaxTree.GetText().ToString(), syntaxNode.ToFullString());
                    selections.Add(selection);

                    if (!dicIntersection.ContainsKey(selection))
                    {
                        dicIntersection.Add(selection, syntaxNode);
                    }
                }
                dicSelections.Add(keypPair.Key, selections);
            }


            IEnumerable<Selection> intersection = dicSelections.Values.First();
            for (int i = 1; i < dicSelections.Values.Count; i++)
            {
                intersection = intersection.Intersect(dicSelections.Values.ElementAt(i));
            }

            List<Selection> enumerable = intersection as List<Selection> ?? intersection.ToList();

            List<SyntaxNode> nodes = new List<SyntaxNode>();

            foreach(var item in enumerable)
            {
                nodes.Add(dicIntersection[item]);
            }
            
            return nodes;
        }

        ///// <summary>
        ///// Verify if nodes match with developer selected locations
        ///// </summary>
        ///// <param name="nodes">List of nodes</param>
        ///// <param name="selectedLocations">Developer selected locations</param>
        ///// <returns>True if nodes match with developer selected locations</returns>
        //private bool MatchesSelectedLocations(IEnumerable<Selection> nodes, List<TRegion> selectedLocations)
        //{
        //    IList<Selection> selections = nodes as IList<Selection> ?? nodes.ToList();
        //    foreach (TRegion region in selectedLocations)
        //    {
        //        bool isIntersection = false;
        //        foreach (Selection selection in selections)
        //        {
        //            TRegion rSel = new TRegion();
        //            rSel.Start = selection.Start;
        //            rSel.Length = selection.Length;
        //            if (region.Path.ToUpperInvariant().Equals(selection.SourcePath.ToUpperInvariant()) && rSel.IntersectWith(region))
        //            {
        //                isIntersection = true;
        //                break;
        //            }
        //        }

        //        if (!isIntersection)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        /// <summary>
        /// Verify if nodes match with developer selected locations
        /// </summary>
        /// <param name="nodes">List of nodes</param>
        /// <param name="selectedLocations">Developer selected locations</param>
        /// <returns>True if nodes match with developer selected locations</returns>
        private bool MatchesSelectedLocations(IEnumerable<SyntaxNode> nodes, List<TRegion> selectedLocations)
        {
            Dictionary<string, List<SyntaxNode>> dicNodes = new Dictionary<string, List<SyntaxNode>>();
            IList<SyntaxNode> syntaxNodes = nodes as IList<SyntaxNode> ?? nodes.ToList();
            foreach (SyntaxNode node in syntaxNodes)
            {
                List<SyntaxNode> value;
                if (!dicNodes.TryGetValue(node.SyntaxTree.FilePath.ToUpperInvariant(), out value))
                {
                    dicNodes.Add(node.SyntaxTree.FilePath.ToUpperInvariant(), new List<SyntaxNode>());
                }
                dicNodes[node.SyntaxTree.FilePath.ToUpperInvariant()].Add(node);
            }
            foreach (TRegion region in selectedLocations)
            {
                bool isIntersectoin = false;
                if (dicNodes.ContainsKey(region.Path.ToUpperInvariant()))
                {
                    foreach (SyntaxNode selection in dicNodes[region.Path.ToUpperInvariant()])
                    {
                        TRegion rSel = new TRegion();
                        rSel.Start = selection.Span.Start;
                        rSel.Length = selection.Span.Length;
                        if (rSel.IntersectWith(region))
                        {
                            isIntersectoin = true;
                            break;
                        }
                    }
                }

                if (!isIntersectoin)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Select nodes for a single document
        /// </summary>
        /// <param name="nodes">Nodes</param>
        /// <param name="root">Node container of the selection</param>
        /// <returns>Nodes for single document</returns>
        private IEnumerable<SyntaxNode> NodesForSingleFile(IEnumerable<SyntaxNode> nodes, SyntaxNode root)
        {
            List<SyntaxNode> resultList = new List<SyntaxNode>();
            EditorController controller = EditorController.GetInstance();
            foreach (SyntaxNode node in nodes)
            {
                if (node.SyntaxTree.FilePath.ToUpperInvariant().Equals(Path.GetFullPath(controller.CurrentViewCodePath).ToUpperInvariant()))
                {
                    try
                    {
                        SyntaxNode childNode = root.FindNode(node.Span);
                        if (childNode != null)
                        {
                            resultList.Add(childNode);
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        //MessageBox.Show("This location is not present on root node.");
                    }
                }
            }
            return resultList;
        }

        /// <summary>
        /// Base processing for RetrieveRegion
        /// </summary>
        /// <param name="syntaxNodes">regions</param>
        /// <returns>List of regions</returns>
        private List<TRegion> RetrieveRegionsBase(IEnumerable<SyntaxNode> syntaxNodes)
        {
            List<TRegion> tRegions = new List<TRegion>();

            foreach (SyntaxNode node in syntaxNodes)
            {
                List<SyntaxNodeOrToken> tokens = new List<SyntaxNodeOrToken>();
                tokens = ASTManager.EnumerateSyntaxNodesAndTokens(node, tokens);
                ListNode lNode = new ListNode(tokens);

                if (Predicate.Evaluate(lNode))
                {
                    TRegion parent = new TRegion();
                    parent.Text = node.SyntaxTree.GetText().ToString();
                    TRegion tRegion = new TRegion();

                    TextSpan span = node.Span;
                    tRegion.Text = node.GetText().ToString();
                    tRegion.Start = span.Start;
                    tRegion.Length = span.Length;
                    tRegion.Node = node;
                    tRegion.Path = node.SyntaxTree.FilePath.ToUpperInvariant();
                    tRegion.Parent = parent;

                    tRegions.Add(tRegion);
                }
            }
            return tRegions;
        }

        ///// <summary>
        ///// Get the name used to search for references
        ///// </summary>
        ///// <returns>Name used to search for references</returns>
        //private IEnumerable<string> GetIdentifierNames()
        //{
        //    List<string> nameList = new List<string>();
        //    foreach (var token in Predicate.r1.Regex())
        //    {
        //        if (token is DymToken && !(Predicate is NotContains))
        //        {
        //            string name = token.token.ToString();
        //            WorkspaceManager manager = WorkspaceManager.GetInstance();
        //            var x = manager.GetLocalReferences(EditorController.GetInstance().ProjectInformation.ProjectPath.First(), EditorController.GetInstance().ProjectInformation.SolutionPath, token.token, name);

        //            nameList.Add(name);
        //        }
        //    }
        //    foreach (var token in Predicate.r2.Regex())
        //    {
        //        if (token is DymToken && !(Predicate is NotContains))
        //        {
        //            string name = token.token.ToString();
        //            WorkspaceManager manager = WorkspaceManager.GetInstance();
        //            var x = manager.GetLocalReferences(EditorController.GetInstance().ProjectInformation.ProjectPath.First(), EditorController.GetInstance().ProjectInformation.SolutionPath, token.token, name);
        //            nameList.Add(name);
        //        }
        //    }
        //    return nameList;
        //}

        /// <summary>
        /// Get the name used to search for references
        /// </summary>
        /// <returns>Name used to search for references</returns>
        private IEnumerable<DymToken> GetIdentifierNames()
        {
            List<DymToken> nameList = new List<DymToken>();
            if (!(Predicate is NotContains))
            {
                IEnumerable<DymToken> tokensR1 = LookUpForDymTokens(Predicate.regex.R1);
                IEnumerable<DymToken> tokensR2 = LookUpForDymTokens(Predicate.regex.R2);

                nameList.AddRange(tokensR1);
                nameList.AddRange(tokensR2);
            }

            return nameList;
        }

        /// <summary>
        /// Look up for dynamic tokens on predicate
        /// </summary>
        /// <param name="tokenSeq">Token sequence</param>
        /// <returns>Dynamic tokens</returns>
        private IEnumerable<DymToken> LookUpForDymTokens(TokenSeq tokenSeq)
        {
            List<DymToken> nameList = new List<DymToken>();
            foreach (Token token in tokenSeq.Tokens)
            {
                if (token is DymToken && token.token.IsKind(SyntaxKind.IdentifierToken))
                {
                    if (!(token is RawDymToken) && (token as DymToken).dynType.type.Equals(DynType.FULLNAME))
                    {
                        nameList.Add(token as DymToken);
                    }
                }
            }
            return nameList;
        }

        /// <summary>
        /// Filter learner
        /// </summary>
        /// <returns>Filter learn base</returns>
        protected abstract FilterLearnerBase GetFilterLearner(List<TRegion> list);

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns>String representation for this object.</returns>
        public override string ToString()
        {
            return "FilterBool(b, FilterNodeByType(R0, t))"
            +"\n\t\tb=" + Predicate.ToString()
            + "\n\t\tt=[" + GetTypes(Names(Lcas)) + "]";
        }

        private string GetTypes(List<string> lcas)
        {
            string s = "";
            s += lcas.First();
        
            for(int index = 1; index < lcas.Count; index++)
            {
                s += ", " + lcas[index].ToString();
            }

            return s;
        }

        private List<string> Names(List<SyntaxNode> lcas)
        {
            List<string> strs = new List<string>();

            foreach(var item in lcas)
            {
                strs.Add(KindText(item.Kind()));
            }

            HashSet<string> hash = new HashSet<string>(strs);
            return hash.ToList();
        }

        private string KindText(SyntaxKind kind)
        {
            var values = Enum.GetValues(typeof(SyntaxKind)).Cast<SyntaxKind>();

            foreach (SyntaxKind item in values)
            {
                if(item.Equals(kind))
                {
                    return Enum.GetName(typeof(SyntaxKind), item);
                }
            }
            return null;
        }
    }
}



