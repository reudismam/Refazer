using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ExampleRefactoring.Spg.ExampleRefactoring.AST;
using ExampleRefactoring.Spg.ExampleRefactoring.Bean;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using ExampleRefactoring.Spg.ExampleRefactoring.Workspace;
using ExampleRefactoring.Spg.LocationRefactoring.Tok;
using LocationCodeRefactoring.Spg.LocationRefactor.Location;
using LocationCodeRefactoring.Spg.LocationRefactor.Node;
using LocationCodeRefactoring.Spg.LocationRefactor.Operator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Spg.LocationCodeRefactoring.Controller;
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
        /// <param name="list">List of selected regions</param>
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

        /// <summary>
        /// Evalute if node match predicate
        /// </summary>
        /// <param name="syntaxNode">Syntax node</param>
        /// <returns>True, if node match predicate, false otherwise</returns>
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
        /// <returns>Retrieved regions</returns>
        public List<TRegion> RetrieveRegion()
        {
            Dictionary<string, List<TRegion>> result = RegionManager.GetInstance().GroupRegionBySourceFile(List);
            if (result.Count == 1)
            {
                throw new Exception("RetrieveRegion with only source code parameter does not accept single file transformation.");
            }

            //string name = GetIdentifierName();
            IEnumerable<SyntaxNode> //nodesForFiltering = SyntacticalDecomposer.SyntaxNodesWithSemanticModel(name);

                // if (nodesForFiltering == null)
                // {
                nodesForFiltering = SyntaxNodesDymTokens();
            if (nodesForFiltering == null)
            {
                EditorController controller = EditorController.GetInstance();
                List<Tuple<string, string>> files =
                    WorkspaceManager.GetInstance()
                        .GetSourcesFiles(controller.ProjectInformation.ProjectPath,
                            controller.ProjectInformation.SolutionPath);
                nodesForFiltering = Decomposer.SyntaxNodesWithoutSemanticModel(files);
                return RetrieveRegionsBase(nodesForFiltering);
            }
            // }
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
            //string name = GetIdentifierName();
            IEnumerable<SyntaxNode> //nodesForFiltering = null;// SyntacticalDecomposer.SyntaxNodesWithSemanticModel(name);

            //if (nodesForFiltering == null)
            //{
                nodesForFiltering = SyntaxNodesDymTokens();
            if (nodesForFiltering == null)
            {
                nodesForFiltering = Decomposer.SyntaxNodesWithoutSemanticModel(syntaxNode);
                return RetrieveRegionsBase(nodesForFiltering);
            }
            //}
            //IEnumerable<SyntaxNode> nodesForFiltering =
            //    SyntacticalDecomposer.SyntaxNodesWithoutSemanticModel(syntaxNode.Parent);
            nodesForFiltering = NodesForSingleFile(nodesForFiltering, syntaxNode);
            return RetrieveRegionsBase(nodesForFiltering);
        }

        /// <summary>
        /// Calculate syntax node for filtering using dynamic tokens
        /// </summary>
        /// <returns>Syntax node used on filtering process</returns>
        internal IEnumerable<SyntaxNode> SyntaxNodesDymTokens()
        {
            IEnumerable<string> nameList = GetIdentifierNames().ToList();
            if (nameList.Any())
            {
                var referenceDictionary = new Dictionary<string, IEnumerable<SyntaxNode>>();
                foreach (string nameDyn in nameList)
                {
                    IEnumerable<SyntaxNode> nodes = Decomposer.GetInstance().SyntaxNodesWithSemanticModel(nameDyn);
                    if (nodes != null)
                    {
                        if (!referenceDictionary.ContainsKey(nameDyn))
                        {
                            referenceDictionary.Add(nameDyn, nodes);
                        }
                    }
                }

                if (referenceDictionary.Count() == 1)
                {
                    return referenceDictionary.First().Value;
                }

                if (referenceDictionary.Any())
                {
                    List<SyntaxNode> nodes = GetIntersection(referenceDictionary);
                    return nodes;
                }
            }
            return null;
        }

        /// <summary>
        /// Calculate the intersection of references and return it.
        /// </summary>
        /// <param name="referenceDictionary">Dictinary of references</param>
        /// <returns>Intersection of referenced elements on dictionary</returns>
        private List<SyntaxNode> GetIntersection(Dictionary<string, IEnumerable<SyntaxNode>> referenceDictionary)
        {
            Dictionary<string, IEnumerable<Selection>> dicSelections = new Dictionary<string, IEnumerable<Selection>>();
            foreach (KeyValuePair<string, IEnumerable<SyntaxNode>> keypPair in referenceDictionary)
            {
                List<Selection> selections = new List<Selection>();
                foreach (SyntaxNode syntaxNode in keypPair.Value)
                {
                    Selection selection = new Selection(syntaxNode.Span.Start, syntaxNode.Span.Length, syntaxNode.SyntaxTree.FilePath, syntaxNode.SyntaxTree.GetText().ToString());
                    selections.Add(selection);
                }
                dicSelections.Add(keypPair.Key, selections);
            }

            IEnumerable<Selection> insersected = dicSelections.Values.First();
            for(int i = 1; i < dicSelections.Values.Count; i++)
            {
                 insersected = insersected.Intersect(dicSelections.Values.ElementAt(i));
            }

            List<SyntaxNode> nodes = new List<SyntaxNode>();
            foreach (Selection selection in insersected)
            {
                SyntaxTree root = CSharpSyntaxTree.ParseFile(selection.SourcePath);
                nodes.Add(root.GetRoot().FindNode(new TextSpan(selection.Start, selection.Length)));
            }
            return nodes;
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
                //MessageBox.Show("Node: " + childNode.CSharpKind());
                //Console.WriteLine(node);
            }
            return resultList;
        }

        /// <summary>
        /// Base processing for RetrieveRegion
        /// </summary>
        /// <param name="regions">regions</param>
        /// <returns>List of regions</returns>
        private List<TRegion> RetrieveRegionsBase(IEnumerable<SyntaxNode> regions)
        {
            List<TRegion> tRegions = new List<TRegion>();
            //IEnumerable<SyntaxNode> regions = SyntaxNodesWithSemanticModel(syntaxNode, List);

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
                    tRegion.Path = node.SyntaxTree.FilePath.ToUpperInvariant();
                    tRegion.Parent = parent;

                    tRegions.Add(tRegion);
                }
            }
            return tRegions;
        }

        /// <summary>
        /// Get the name used to search for references
        /// </summary>
        /// <returns>Name used to search for references</returns>
        private IEnumerable<string> GetIdentifierNames()
        {
            List<string> nameList = new List<string>();
            foreach (var token in Predicate.r1.Regex())
            {
                if (token is DymToken /*|| token.token.IsKind(SyntaxKind.IdentifierToken)*/)
                {
                    string name = token.token.ToString();
                    nameList.Add(name);
                }
            }
            foreach (var token in Predicate.r2.Regex())
            {
                if (token is DymToken /*|| token.token.IsKind(SyntaxKind.IdentifierToken)*/)
                {
                    string name = token.token.ToString();
                    nameList.Add(name);
                }
            }
            return nameList;
        }

        ///// <summary>
        ///// Syntax nodes
        ///// </summary>
        ///// <param name="input">Source code</param>
        ///// <returns>Syntax nodes</returns>
        //protected abstract IEnumerable<SyntaxNode> SyntaxNodesWithSemanticModel(string input);

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
///// <param name="sourceClass">Source code</param>
///// <returns>Retrieved regions</returns>
//public List<TRegion> RetrieveRegion(string sourceClass)
//{
//    Dictionary<string, List<TRegion>> result = RegionManager.GetInstance().GroupRegionBySourceFile(List);
//    //if (result.Count == 1)
//    //{
//        throw new Exception("RetrieveRegion with only source code parameter does not accept single file transformation.");
//    //}

//    string name = GetIdentifierName();
//    IEnumerable<SyntaxNode> nodesForFiltering = SyntacticalDecomposer.SyntaxNodesWithSemanticModel(name);

//    if (nodesForFiltering == null)
//    {
//        SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceClass);
//        nodesForFiltering = SyntacticalDecomposer.SyntaxNodesWithoutSemanticModel(tree.GetRoot());
//        return RetrieveRegionsBase(nodesForFiltering);
//    }

//    List<TRegion> tregions = RetrieveRegionsBase(nodesForFiltering);
//    return tregions;
//}


///// <summary>
///// Get the name used to search for references
///// </summary>
///// <returns>Name used to search for references</returns>
//private string GetIdentifierName()
//{
//    string name = null;
//    EditorController controller = EditorController.GetInstance();
//    bool isCommonName = true;
//    foreach (SyntaxNode lca in controller.Lcas)
//    {
//        switch (lca.CSharpKind())
//        {
//            case SyntaxKind.InvocationExpression:
//                name = GetName(lca as InvocationExpressionSyntax, name);
//                if (name == null)
//                {
//                    isCommonName = false;
//                }
//                break;
//            case SyntaxKind.ObjectCreationExpression:
//                ObjectCreationExpressionSyntax objectCreation = (ObjectCreationExpressionSyntax)lca;
//                SyntaxToken identifierToken = objectCreation.NewKeyword.GetNextToken();

//                string nameToken = identifierToken.ToFullString();

//                if (name == null)
//                {
//                    name = nameToken;
//                }
//                else if (!name.Equals(nameToken))
//                {
//                    isCommonName = false;
//                }
//                //objectCreation.
//                break;
//            case SyntaxKind.QualifiedName:
//                QualifiedNameSyntax qualifiedName = (QualifiedNameSyntax)lca;
//                if (name == null)
//                {
//                    name = qualifiedName.Right.ToFullString();
//                }
//                else if (!name.Equals(qualifiedName.Right.ToFullString()))
//                {
//                    isCommonName = false;
//                }
//                break;
//            case SyntaxKind.Argument:
//                ArgumentSyntax argumentSyntax = (ArgumentSyntax)lca;
//                ExpressionSyntax expressionSyntax = argumentSyntax.Expression;
//                if (expressionSyntax is InvocationExpressionSyntax)
//                {
//                    name = GetName(expressionSyntax as InvocationExpressionSyntax, name);
//                    if (name == null)
//                    {
//                        isCommonName = false;
//                    }
//                }
//                else if (expressionSyntax is ObjectCreationExpressionSyntax)
//                {
//                    ObjectCreationExpressionSyntax objectArgument = (ObjectCreationExpressionSyntax)expressionSyntax;
//                    SyntaxToken identifierArgument = objectArgument.NewKeyword.GetNextToken();

//                    string nameArgument = identifierArgument.ToFullString();

//                    if (name == null)
//                    {
//                        name = nameArgument;
//                    }
//                    else if (!name.Equals(nameArgument))
//                    {
//                        isCommonName = false;
//                    }
//                }
//                break;
//            case SyntaxKind.ExpressionStatement:
//                ExpressionStatementSyntax expressionStatement = (ExpressionStatementSyntax)lca;
//                ExpressionSyntax expressionStatementSyntax = expressionStatement.Expression;
//                //MessageBox.Show(expressionStatementSyntax.CSharpKind() + "");
//                if (expressionStatementSyntax is MemberAccessExpressionSyntax)
//                {
//                    MemberAccessExpressionSyntax memberAccess = (MemberAccessExpressionSyntax)lca;

//                    if (name == null)
//                    {
//                        name = memberAccess.Name.ToFullString();
//                    }
//                    else if (!name.Equals(memberAccess.Name.ToFullString()))
//                    {
//                        isCommonName = false;
//                    }
//                }
//                else if (expressionStatementSyntax is InvocationExpressionSyntax)
//                {
//                    name = GetName(expressionStatementSyntax as InvocationExpressionSyntax, name);
//                    if (name == null)
//                    {
//                        isCommonName = false;
//                    }
//                }
//                break;
//            case SyntaxKind.LocalDeclarationStatement:
//                //LocalDeclarationStatementSyntax localDeclaration = (LocalDeclarationStatementSyntax) lca;
//                //VariableDeclarationSyntax variableDeclaration = localDeclaration.Declaration;
//                //variableDeclaration.
//                break;
//            default:
//                if (lca is MemberAccessExpressionSyntax)
//                {
//                    MemberAccessExpressionSyntax memberAccess = (MemberAccessExpressionSyntax)lca;

//                    if (name == null)
//                    {
//                        name = memberAccess.Name.ToFullString();
//                    }
//                    else if (!name.Equals(memberAccess.Name.ToFullString()))
//                    {
//                        isCommonName = false;
//                    }
//                }
//                //else
//                //{
//                //    //return GetIdentifierName2();
//                //}
//                break;
//        }
//    }
//    if (name != null && isCommonName)
//    {
//        return name;
//    }

//    //return GetIdentifierName2();
//    return null;
//}

//private static string GetName(InvocationExpressionSyntax lca, string name)
//{
//    InvocationExpressionSyntax syntax = lca;
//    ExpressionSyntax expression = syntax.Expression;
//    if (expression is IdentifierNameSyntax)
//    {
//        IdentifierNameSyntax nameSyntax = (IdentifierNameSyntax)expression;
//        if (name == null)
//        {
//            name = nameSyntax.Identifier.ToString();
//        }
//        else if (!name.Equals(nameSyntax.Identifier.ToString()))
//        {
//            return null;
//        }
//    }
//    else if (expression is MemberAccessExpressionSyntax)
//    {
//        MemberAccessExpressionSyntax member = (MemberAccessExpressionSyntax)expression;
//        if (name == null)
//        {
//            name = member.Name.ToString();
//        }
//        else if (!name.Equals(member.Name.ToString()))
//        {
//            return null;
//        }

//    }
//    return name;
//}

//private string GetIdentifierName()
//{
//    string name = null;

//    EditorController controller = EditorController.GetInstance();
//    foreach (SyntaxNode node in controller.Lcas)
//    {
//        node.
//    }
//}

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
///// Retrieve regions
///// </summary>
///// <returns>Retrieved regions</returns>
//public List<TRegion> RetrieveRegion()
//{
//    Dictionary<string, List<TRegion>> result = RegionManager.GetInstance().GroupRegionBySourceFile(List);
//    if (result.Count == 1)
//        throw new Exception(
//            "RetrieveRegion with only source code parameter does not accept single file transformation.");

//    string name = GetIdentifierName();
//    IEnumerable<SyntaxNode> nodesForFiltering = null;
//    //SyntacticalDecomposer.SyntaxNodesWithSemanticModel(name);

//    if (nodesForFiltering == null)
//    {
//        nodesForFiltering = SyntaxNodesDymTokens();
//        //IEnumerable<string> nameList = GetIdentifierNames();
//        //if (nameList.Any())
//        //{
//        //    Dictionary<string, IEnumerable<SyntaxNode>> referenceDictionary =
//        //        new Dictionary<string, IEnumerable<SyntaxNode>>();
//        //    foreach (string nameDyn in nameList)
//        //    {
//        //        IEnumerable<SyntaxNode> nodes = SyntacticalDecomposer.SyntaxNodesWithSemanticModel(nameDyn);
//        //        if (nodes != null)
//        //        {
//        //            referenceDictionary.Add(nameDyn, nodes);
//        //        }
//        //    }

//        //    if (referenceDictionary.Count() == 1)
//        //    {
//        //        return RetrieveRegionsBase(referenceDictionary.First().Value);
//        //    }

//        //    if (referenceDictionary.Any())
//        //    {
//        //        MessageBox.Show("Make merge.");
//        //    }
//        //}
//    }

//    if (nodesForFiltering == null)
//    {
//        EditorController controller = EditorController.GetInstance();
//        List<Tuple<string, string>> files =
//            WorkspaceManager.GetInstance()
//                .GetSourcesFiles(controller.ProjectInformation.ProjectPath,
//                    controller.ProjectInformation.SolutionPath);
//        nodesForFiltering = SyntacticalDecomposer.SyntaxNodesWithoutSemanticModel(files);
//        return RetrieveRegionsBase(nodesForFiltering);
//    }

//    List<TRegion> tregions = RetrieveRegionsBase(nodesForFiltering);
//    return tregions;
//}

///// <summary>
///// Syntax nodes to be used on filtering
///// </summary>
///// <param name="tree">Source code tree</param>
///// <returns>Syntax nodes to be used on filtering</returns>
//private IEnumerable<SyntaxNode> SyntaxNodesWithSemanticModel(SyntaxNode tree)
//{
//    var nodes = from node in tree.DescendantNodesAndSelf()
//                          where WithinLcas(node)
//                          select node;
//    return nodes;
//}


///// <summary>
///// Retrieve regions
///// </summary>
///// <param name="input">Source code</param>
///// <returns>Retrieved regions</returns>
//public List<TRegion> RetrieveRegion(string input)
//{
//    IEnumerable<SyntaxNode> regions = SyntaxNodesWithSemanticModel(input, List);
//    return RetrieveRegionsBase(regions);
//    //List<TRegion> tRegions = new List<TRegion>();
//    //IEnumerable<SyntaxNode> regions = SyntaxNodesWithSemanticModel(input, List);

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