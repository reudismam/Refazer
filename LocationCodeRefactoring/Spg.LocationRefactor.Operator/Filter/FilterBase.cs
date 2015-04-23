using System;
using System.Collections.Generic;
using System.Linq;
using ExampleRefactoring.Spg.ExampleRefactoring.AST;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using ExampleRefactoring.Spg.ExampleRefactoring.Workspace;
using ExampleRefactoring.Spg.LocationRefactoring.Tok;
using LocationCodeRefactoring.Spg.LocationCodeRefactoring.Controller;
using LocationCodeRefactoring.Spg.LocationRefactor.Location;
using LocationCodeRefactoring.Spg.LocationRefactor.Operator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Spg.LocationRefactor.Learn;
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
        /// Region list
        /// </summary>
        public List<TRegion> List;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="list"></param>
        protected FilterBase(List<TRegion> list)
        {
            this.List = list;
        }

        /// <summary>
        /// Execute filter base
        /// </summary>
        /// <param name="input">Source code</param>
        /// <returns>Statements that follows the predicate</returns>
        public ListNode Execute(String input)
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
            TokenSeq regex = Predicate.r1;

            Tuple<SyntaxNode, SyntaxNode> tuple = Tuple.Create(syntaxNode, syntaxNode);
            ListNode listNode = ASTProgram.Example(tuple).Item1;

            bool indicator = learn.Indicator(Predicate, listNode, regex);
            if (indicator) return listNode;

            return null;
        }

        public bool IsMatch(SyntaxNode syntaxNode)
        {
            FilterLearnerBase learn = GetFilterLearner(List);
            TokenSeq regex = new TokenSeq(Predicate.Regex());

            Tuple<SyntaxNode, SyntaxNode> tuple = Tuple.Create(syntaxNode, syntaxNode);
            ListNode listNode = ASTProgram.Example(tuple).Item1;

            bool indicator = learn.Indicator(Predicate, listNode, regex);

            return indicator;
        }

        /// <summary>
        /// Retrieve regions
        /// </summary>
        /// <param name="sourceClass">Source code</param>
        /// <returns>Retrieved regions</returns>
        public List<TRegion> RetrieveRegion(string sourceClass)
        {
            Dictionary<string, List<TRegion>> result = RegionManager.GetInstance().GroupRegionBySourceFile(List);
            if (result.Count == 1) throw new Exception("RetrieveRegion with only source code parameter does not accept single file transformation.");

            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceClass);
            IEnumerable<SyntaxNode> descedants = SyntaxNodes(tree.GetRoot());
            List<TRegion> tregions = RetrieveRegionsBase(descedants);
            return tregions;
        }

        /// <summary>
        /// Retrieve regions
        /// </summary>
        /// <returns>Retrieved regions</returns>
        public List<TRegion> RetrieveRegion()
        {
            Dictionary<string, List<TRegion>> result = RegionManager.GetInstance().GroupRegionBySourceFile(List);
            if (result.Count == 1) throw new Exception("RetrieveRegion with only source code parameter does not accept single file transformation.");

            //SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceClass);
            IEnumerable<SyntaxNode> descedants = SyntaxNodes();
            List<TRegion> tregions = RetrieveRegionsBase(descedants);
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
            IEnumerable<SyntaxNode> nodesForFiltering = SyntaxNodes(syntaxNode.Parent);
            return RetrieveRegionsBase(nodesForFiltering);
        }

        ///// <summary>
        ///// Syntax nodes to be used on filtering
        ///// </summary>
        ///// <param name="tree">Source code tree</param>
        ///// <returns>Syntax nodes to be used on filtering</returns>
        //private IEnumerable<SyntaxNode> SyntaxNodes(SyntaxNode tree)
        //{
        //    var nodes = from node in tree.DescendantNodesAndSelf()
        //                          where WithinLcas(node)
        //                          select node;
        //    return nodes;
        //}

        /// <summary>
        /// Syntax nodes to be used on filtering
        /// </summary>
        /// <param name="tree">Source code tree</param>
        /// <returns>Syntax nodes to be used on filtering</returns>
        private IEnumerable<SyntaxNode> SyntaxNodes(SyntaxNode tree)
        { 
            string name = GetIdentifierName();

            if (name == null) return SyntaxNodesWithoutSemanticModel(tree);

            EditorController controller = EditorController.GetInstance();
            Dictionary<string, Dictionary<string, List<TextSpan>>> result = WorkspaceManager.GetInstance()
                .GetDeclaredReferences(controller.ProjectInformation.ProjectPath,
                    controller.ProjectInformation.SolutionPath, name);
            var referencedSymbols = RegionManager.GroupReferenceBySelection(result, controller.SelectedLocations);

            List<SyntaxNode> nodesList = new List<SyntaxNode>();
            if (referencedSymbols.Count == 1)
            {
                Dictionary<string, List<TextSpan>> dictionary = referencedSymbols.First().Value;
                //for each file
                foreach (var fileSpans in dictionary)
                {
                    SyntaxTree fileTree = CSharpSyntaxTree.ParseFile(fileSpans.Key);
                    var nodes = from node in fileTree.GetRoot().DescendantNodesAndSelf()
                                where WithinLcas(node) && WithinSpans(node, fileSpans.Value)
                                select node;
                    nodesList.AddRange(nodes);
                }
            }

            if(!result.Any()) return SyntaxNodesWithoutSemanticModel(tree);
            return nodesList;

        }

        /// <summary>
        /// Syntax nodes to be used on filtering
        /// </summary>
        /// <returns>Syntax nodes to be used on filtering</returns>
        private IEnumerable<SyntaxNode> SyntaxNodes()
        {
            string name = GetIdentifierName();

            //if (name == null) return SyntaxNodesWithoutSemanticModel(tree);

            EditorController controller = EditorController.GetInstance();
            Dictionary<string, Dictionary<string, List<TextSpan>>> result = WorkspaceManager.GetInstance()
                .GetDeclaredReferences(controller.ProjectInformation.ProjectPath,
                    controller.ProjectInformation.SolutionPath, name);
            var referencedSymbols = RegionManager.GroupReferenceBySelection(result, controller.SelectedLocations);

            List<SyntaxNode> nodesList = new List<SyntaxNode>();
            if (referencedSymbols.Count == 1)
            {
                Dictionary<string, List<TextSpan>> dictionary = referencedSymbols.First().Value;
                //for each file with the list of text span reference to the source declaration.
                foreach (KeyValuePair<string, List<TextSpan>> fileSpans in dictionary)
                {
                    SyntaxTree fileTree = CSharpSyntaxTree.ParseFile(fileSpans.Key);
                    var nodes = from node in fileTree.GetRoot().DescendantNodesAndSelf()
                                where WithinLcas(node) && WithinSpans(node, fileSpans.Value)
                                select node;
                    nodesList.AddRange(nodes);
                }
            }

            //if (!result.Any()) return SyntaxNodesWithoutSemanticModel(tree);
            return nodesList;
        }

        private IEnumerable<SyntaxNode> SyntaxNodesWithoutSemanticModel(SyntaxNode tree)
        {
            var nodes = from node in tree.DescendantNodesAndSelf()
                        where WithinLcas(node)
                        select node;
            return nodes;
        }

        private bool WithinSpans(SyntaxNode node, List<TextSpan> value)
        {
            foreach (var entry in value)
            {
                if (node.Span.IntersectsWith(entry)) {return true;}
            }
            return false;
        }

        private string GetIdentifierName()
        {
            string name = null;
            foreach (var token in Predicate.r1.Regex())
            {
                if (token is DymToken || token.token.IsKind(SyntaxKind.IdentifierToken))
                {
                    name = token.token.ToString();
                    break;
                }
            }

            if (name == null)
            {
                foreach (var token in Predicate.r2.Regex())
                {
                    if (token is DymToken || token.token.IsKind(SyntaxKind.IdentifierToken))
                    {
                        name = token.token.ToString();
                        break;
                    }
                }
            }
            return name;
        }

        private bool WithinLcas(SyntaxNode node)
        {
            foreach (var lca in EditorController.GetInstance().Lcas)
            {
                if (node.IsKind(lca.CSharpKind()))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Base processing for RetrieveRegion
        /// </summary>
        /// <param name="regions">regions</param>
        /// <returns>List of regions</returns>
        private List<TRegion> RetrieveRegionsBase(IEnumerable<SyntaxNode> regions)
        {
            List<TRegion> tRegions = new List<TRegion>();
            //IEnumerable<SyntaxNode> regions = SyntaxNodes(syntaxNode, List);

            foreach (SyntaxNode node in regions)
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
                    tRegion.Path = node.SyntaxTree.FilePath;
                    tRegion.Parent = parent;

                    tRegions.Add(tRegion);
                }
            }
            return tRegions;
        }

        ///// <summary>
        ///// Syntax nodes correspondents to selection
        ///// </summary>
        ///// <param name="sourceCode">Source code.</param>
        ///// <param name="list">Selection location on source code.</param>
        ///// <returns>Syntax nodes correspondents to selection on source code</returns>
        //private IEnumerable<SyntaxNode> SyntaxNodesForFiltering(string sourceCode, List<TRegion> list)
        //{
        //    List<SyntaxNode> nodes = RegionManager.SyntaxNodesForFiltering(sourceCode, list);
        //    return nodes;
        //}

        ///// <summary>
        ///// Syntax nodes
        ///// </summary>
        ///// <param name="input">Source code</param>
        ///// <returns>Syntax nodes</returns>
        //protected abstract IEnumerable<SyntaxNode> SyntaxNodes(string input);

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
            return Predicate.ToString();
        }

    }
}

///// <summary>
///// Retrieve regions
///// </summary>
///// <param name="input">Source code</param>
///// <returns>Retrieved regions</returns>
//public List<TRegion> RetrieveRegion(string input)
//{
//    IEnumerable<SyntaxNode> regions = SyntaxNodes(input, List);
//    return RetrieveRegionsBase(regions);
//    //List<TRegion> tRegions = new List<TRegion>();
//    //IEnumerable<SyntaxNode> regions = SyntaxNodes(input, List);

//    //foreach (SyntaxNode node in regions)
//    //{
//    //    List<SyntaxNodeOrToken> tokens = new List<SyntaxNodeOrToken>();
//    //    tokens = ASTManager.EnumerateSyntaxNodesAndTokens(node, tokens);
//    //    ListNode lNode = new ListNode(tokens);

//    //    TokenSeq regexs = ASTProgram.ConcatenateRegularExpression(Predicate.r1, Predicate.r2);
//    //    TokenSeq regex = regexs;

//    //    if (ASTManager.IsMatch(lNode, regex))
//    //    {
//    //        TRegion tRegion = new TRegion();

//    //        TextSpan span = node.Span;
//    //        tRegion.Text = node.GetText().ToString();
//    //        tRegion.Start = span.Start;
//    //        tRegion.Length = span.Length;
//    //        tRegion.Node = node;

//    //        tRegions.Add(tRegion);
//    //    }
//    //}
//    //return tRegions;
//}