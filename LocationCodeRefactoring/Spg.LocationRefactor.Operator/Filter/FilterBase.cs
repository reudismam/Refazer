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
        public IPredicate Predicate { get; set; }

        public List<TRegion> List;

        protected FilterBase(List<TRegion> list)
        {
            this.List = list;
        }

        public override string ToString()
        {
            return Predicate.ToString();
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
            var nodes = from node in tree.DescendantNodesAndSelf()
                        where WithinLcas(node)
                        select node;
            return nodes;
            //string name = null;
            //foreach (var token in Predicate.r1.Regex())
            //{
            //    if (token is DymToken)
            //    {
            //        name = token.token.ToString();
            //        break;
            //    }
            //}

            //if (name == null)
            //{
            //    foreach (var token in Predicate.r2.Regex())
            //    {
            //        if (token is DymToken)
            //        {
            //            name = token.token.ToString();
            //            break;
            //        }
            //    }
            //}

            //EditorController controller = EditorController.GetInstance();
            //WorkspaceManager.GetInstance()
            //    .GetDeclaredReferences(controller.ProjectInformation.ProjectPath,
            //        controller.ProjectInformation.SolutionPath, tree, name);


            //return null;
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

                if(Predicate.Evaluate(lNode))
                {
                    TRegion tRegion = new TRegion();

                    TextSpan span = node.Span;
                    tRegion.Text = node.GetText().ToString();
                    tRegion.Start = span.Start;
                    tRegion.Length = span.Length;
                    tRegion.Node = node;

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

        /// <summary>
        /// Syntax nodes
        /// </summary>
        /// <param name="input">Source code</param>
        /// <returns>Syntax nodes</returns>
        protected abstract IEnumerable<SyntaxNode> SyntaxNodes(string input);

        /// <summary>
        /// Filter learner
        /// </summary>
        /// <returns>Filter learn base</returns>
        protected abstract FilterLearnerBase GetFilterLearner(List<TRegion> list);

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