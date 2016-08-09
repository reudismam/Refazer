using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using ProseSample.Substrings.List;
using ProseSample.Substrings.Spg.Semantic;
using TreeEdit.Spg.Match;
using TreeEdit.Spg.TreeEdit.Update;
using TreeElement;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    public static class Semantics
    {
        //Before transformation mapping
        private static readonly Dictionary<Node, SyntaxNodeOrToken> BeforeAfterMapping = new Dictionary<Node, SyntaxNodeOrToken>();

        //After transformation mapping
        private static readonly Dictionary<Node, SyntaxNodeOrToken> MappingRegions = new Dictionary<Node, SyntaxNodeOrToken>();

        /// <summary>
        /// Matches the element on the tree with specified kind and child nodes.
        /// </summary>
        /// <param name="kind">Syntax kind</param>
        /// <param name="children">Children nodes</param>
        /// <returns>The element on the tree with specified kind and child nodes</returns>
        public static Pattern C(SyntaxKind kind, IEnumerable<Pattern> children)
        {
            return SemanticMatch.C(kind, children);
        }

        /// <summary>
        /// Splits node in elements of kind type.
        /// </summary>
        /// <param name="node">Source node</param>
        /// <param name="kind">Syntax kind</param>
        /// <returns>Elements of kind type</returns>
        public static List<ITreeNode<SyntaxNodeOrToken>> SplitToNodes(ITreeNode<SyntaxNodeOrToken> node, SyntaxKind kind)
        {
            TLabel label = new TLabel(kind);
            var descendantNodes = node.DescendantNodesAndSelf();

            var kinds = from k in descendantNodes
                where k.IsLabel(label)
                select k;
            return kinds.ToList();
        }

        /// <summary>
        /// Return the tree
        /// </summary>
        /// <param name="variable">Result of a match result</param>
        /// <returns></returns>
        public static Pattern Tree(Pattern variable)
        {
            return variable;
        }

        /// <summary>
        /// Searches a node with with kind and occurrence
        /// </summary>
        /// <param name="kind">Kind</param>
        /// <returns>Search result</returns>
        public static Pattern Variable(SyntaxKind kind)
        {
            return SemanticMatch.Variable(kind);
        }


        /// <summary>
        /// Searches a node with with kind and occurrence
        /// </summary>
        /// <param name="kind">Kind</param>
        /// <returns>Search result</returns>
        public static Pattern Leaf(SyntaxKind kind)
        {
            return SemanticMatch.Leaf(kind);
        }

        public static Pattern Parent(Pattern match, int k)
        {
            var patternP = new PatternP(match.Tree, k);
            return patternP;
            //var child = match.Value.Children.ElementAt(k - 1);
            //var result = new Node(child);
            //return result;
        }

        /// <summary>
        /// Literal
        /// </summary>
        /// <param name="tree">Value</param>
        /// <returns>Literal</returns>
        public static Pattern Literal(SyntaxNodeOrToken tree)
        {
            return SemanticMatch.Literal(tree);
        }

        /// <summary>
        /// Insert the newNode node as in the k position of the node in the matching result 
        /// </summary>
        /// <param name="target">Target node</param>
        /// <param name="node">Input data</param>
        /// <param name="k">Position in witch the node will be inserted.</param>
        /// <param name="newNode">Node that will be insert</param>
        /// <returns>New node with the newNode node inserted as the k child</returns>
        public static Node Insert(Node target, Node newNode, int k)
        {
            return SemanticEditOperation.Insert(target, newNode, k);
        }

        /// <summary>
        /// Insert the newNode node as in the k position of the node in the matching result 
        /// </summary>
        /// <param name="target">Target node</param>
        /// <param name="node">Input data</param>
        /// <param name="newNode">Node that will be insert</param>
        /// <returns>New node with the newNode node inserted as the k child</returns>
        public static Node InsertBefore(Node target, Node node, Node newNode)
        {
            return SemanticEditOperation.InsertBefore(target, node, newNode);
        }

        /// <summary>
        /// Update edit operation
        /// </summary>
        /// <param name="target">Target node</param>
        /// <param name="node">Input node</param>
        /// <param name="to">New value</param>
        public static Node Update(Node target, Node to)
        {
            return SemanticEditOperation.Update(target, to);
        }

        /// <summary>
        /// Delete edit operation
        /// </summary>
        /// <param name="target">target</param>
        /// <param name="node">Input node</param>
        /// <returns>Result of the edit opration</returns>
        public static Node Delete(Node target, Node node)
        {
            return SemanticEditOperation.Delete(target, node);
        }

        /// <summary>
        /// Script semantic function
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="patch">Edit operations</param>
        /// <returns>Transformed node.</returns>
        public static IEnumerable<Node> Apply(Node node, Patch patch)
        {
            var beforeFlorest = patch.Edits.Select(o => o.ToList());

            var resultList = new List<Node>();
            foreach (var edited in beforeFlorest)
            {
                resultList.AddRange(edited);
            }
            return resultList;
        }

        /// <summary>
        /// Script semantic function
        /// </summary>
        /// <param name="target">Node</param>
        /// <param name="edits">Edit operations</param>
        /// <returns>Transformed node.</returns>
        public static Node Script(Node target, IEnumerable<Node> edits)
        {
            ITreeNode<SyntaxNodeOrToken> current = edits.Last().Value;
            //if (edits.Last().Value.Value.IsKind(SyntaxKind.EmptyStatement))
            //{
            //    current = edits.Last().Value.Children.First();
            //}
            //else
            //{
            //    current = edits.Last().Value;
            //}
            if (edits.Last().LeftNode != null)
            {
                var leftnode = ReconstructTree(edits.First().LeftNode.Value);
            }

            if (edits.Last().RightNode != null)
            {
                var rightnode = ReconstructTree(edits.First().RightNode.Value);
            }

            var node = ReconstructTree(current);
            MappingRegions[target] = node;
            Console.WriteLine(node.ToString());
            //throw new ArgumentException("exception");
            return target;
        }

        /// <summary>
        /// Return a new node
        /// </summary>
        /// <param name="kind">Returned node SyntaxKind</param>
        /// <param name="childrenNodes">Children nodes</param>
        /// <returns>A new node with kind and child</returns>
        public static Node Node(SyntaxKind kind, IEnumerable<Node> childrenNodes)
        {
            var childrenList = (List<Node>) childrenNodes;
            if (!childrenList.Any()) return null;
            ITreeNode<SyntaxNodeOrToken> parent = new TreeNode<SyntaxNodeOrToken>(null, new TLabel(kind));
            SyntaxNodeOrToken nodevalue = null;
            for (int i = 0; i < childrenList.Count(); i++)
            {
                var child = childrenList.ElementAt(i).Value;
                parent.AddChild(child, i);
                if (child.Value.Parent.IsKind(kind))
                {
                    nodevalue = child.Value.Parent;
                }
            }
            if (nodevalue != null)
            {
                var copy = ConverterHelper.MakeACopy(ConverterHelper.ConvertCSharpToTreeNode(nodevalue));
                copy.Children = parent.Children;
                var node = new Node(copy);
                return node;
            }
            else
            {
                var node = new Node(parent);
                return node;
            }
        }

        /// <summary>
        /// Create a constant node
        /// </summary>
        /// <param name="cst">Constant</param>
        /// <returns>A new constant node.</returns>
        public static Node Const(SyntaxNodeOrToken cst)
        {
            var parent = new TreeNode<SyntaxNodeOrToken>(cst.Parent, new TLabel(cst.Parent.Kind()));
            var itreeNode = new TreeNode<SyntaxNodeOrToken>(cst, new TLabel(cst.Kind()));
            itreeNode.Parent = parent;
            var node = new Node(itreeNode);
            return node;
        }

        /// <summary>
        /// Reference semantic function
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="result">Result of the pattern</param>
        /// <returns>Result of the pattern</returns>
        public static Node Ref(Node node, Node result)
        {
            return result;
        }

        public static IEnumerable<Pattern> CList(Pattern child1, IEnumerable<Pattern> cList)
        {
            return GList<Pattern>.List(child1, cList);
        }

        public static IEnumerable<Pattern> SC(Pattern child)
        {
            return GList<Pattern>.Single(child);
        }

        public static IEnumerable<Pattern> PList(Pattern child1, IEnumerable<Pattern> cList)
        {
            return GList<Pattern>.List(child1, cList);
        }

        public static IEnumerable<Pattern> SP(Pattern child)
        {
            return GList<Pattern>.Single(child);
        }

        public static IEnumerable<Node> NList(Node child1, IEnumerable<Node> cList)
        {
            return GList<Node>.List(child1, cList);
        }

        public static IEnumerable<Node> SN(Node child)
        {
            return GList<Node>.Single(child);
        }

        public static Patch EList(IEnumerable<Node> child1, Patch cList)
        {
            var editList =  GList<IEnumerable<Node>>.List(child1, cList.Edits).ToList();
            var patch = new Patch(editList);
            return patch;
        }

        public static Patch SE(IEnumerable<Node> child)
        {
            var editList =  GList<IEnumerable<Node>>.Single(child).ToList();
            var list = editList.ToList();
            var patch = new Patch(editList);
            return patch;
        }


        public static IEnumerable<Node> SL(Node child1, IEnumerable<Node> cList)
        {
            return GList<Node>.List(child1, cList);
        }

        public static IEnumerable<Node> SO(Node child)
        {
            return GList<Node>.Single(child);
        }

        public static IEnumerable<SyntaxNodeOrToken> SplitNodes(SyntaxNodeOrToken n)
        {
            SyntaxKind targetKind = SyntaxKind.MethodDeclaration;
            SyntaxNode node = n.AsNode();
            var nodes = from snode in node.DescendantNodes()
                        where snode.IsKind(targetKind)
                        select snode;

            return nodes.Select(snot => (SyntaxNodeOrToken)snot).ToList();
        }

        public static bool FTrue()
        {
            return true;
        }

        public static SyntaxNodeOrToken Transformation(SyntaxNodeOrToken node, IEnumerable<Node> loop)
        {
            List<SyntaxNodeOrToken> afterNodeList;
            List<SyntaxNodeOrToken> beforeNodeList;
            FillBeforeAfterList(out afterNodeList, loop, out beforeNodeList);
            //traversal index of the nodes before the transformation
            var traversalIndices = PostOrderTraversalIndices(node, beforeNodeList);
            //Annotate edited nodes
            node = AnnotateNodeEditedNodes(node, traversalIndices);
            //Update annotated nodes
            node = UpdateAnnotatedNodes(node, traversalIndices, afterNodeList);
            var stringNode = node.ToFullString();
            return node;
        }

        private static SyntaxNodeOrToken UpdateAnnotatedNodes(SyntaxNodeOrToken node, List<int> traversalIndices, List<SyntaxNodeOrToken> afterNodeList)
        {
            for (int i = 0; i < traversalIndices.Count; i++)
            {
                var index = traversalIndices[i];
                var snode = node.AsNode().GetAnnotatedNodes($"ANN{index}").ToList();
                if (snode.Any())
                {
                    var rewriter = new UpdateTreeRewriter(snode.First(), afterNodeList.ElementAt(i).AsNode());
                    node = rewriter.Visit(node.AsNode());
                }
            }
            return node;
        }

        private static SyntaxNodeOrToken AnnotateNodeEditedNodes(SyntaxNodeOrToken node, List<int> traversalIndices)
        {
            foreach (var index in traversalIndices)
            {           
                var treeNode = ConverterHelper.ConvertCSharpToTreeNode(node);
                var traversalNodes = treeNode.DescendantNodesAndSelf();
                var ann = new AddAnnotationRewriter(traversalNodes.ElementAt(index).Value.AsNode(),
                    new List<SyntaxAnnotation> {new SyntaxAnnotation($"ANN{index}")});
                node = ann.Visit(node.AsNode());
            }
            return node;
        }

        private static void FillBeforeAfterList(out List<SyntaxNodeOrToken> afterNodeList, IEnumerable<Node> loop, out List<SyntaxNodeOrToken> beforeNodeList)
        {
            afterNodeList = new List<SyntaxNodeOrToken>();
            beforeNodeList = new List<SyntaxNodeOrToken>();
            var list = loop.ToList();
            foreach (var snode in list)
            {
                //afterNodeList.AddRange(MappingRegions[snode]);
                //beforeNodeList.AddRange(BeforeAfterMapping[snode]);
            }
        }

        private static List<int> PostOrderTraversalIndices(SyntaxNodeOrToken node, List<SyntaxNodeOrToken> beforeNodeList)
        {
            var treeNode = ConverterHelper.ConvertCSharpToTreeNode(node);
            var traversalNodes = treeNode.DescendantNodesAndSelf();
            var traversalIndices = new List<int>();

            for (int i = 0; i < traversalNodes.Count; i++)
            {
                var snode = traversalNodes[i];
                foreach (var v in beforeNodeList)
                {
                    if (snode.Value.Equals(v))
                    {
                        traversalIndices.Add(i);
                    }
                }
            }
            return traversalIndices;
        }

        public static bool NodeMatch(Node sx, Pattern template)
        {
            //if (!sx.Value.Value.IsKind(template.Tree.Value.Kind)) return false
            if (template is PatternP)
            {
                var patternP = (PatternP) template;
                var parent = sx.Value.Parent;
                if (parent == null) return false;
                var isValue = MatchManager.IsValueEachChild(parent, template.Tree);
                var isValid = isValue && parent.Children.FindIndex(o => o.Equals(sx.Value)) == patternP.K - 1;
                return isValid;
            }
            else
            {
                var isValue = MatchManager.IsValueEachChild(sx.Value, template.Tree);
                return isValue;
            }
        }

        public static Node Match(Node target, Pattern kmatch, int k)
        {
            if (kmatch is PatternP)
            {
                var patternP = (PatternP) kmatch;
                //var pattern = target.Value.Parent.Children.ElementAt(patternP.K - 1);
                var nodes = MatchManager.Matches(target.Value, kmatch.Tree);
                //nodes = nodes.Select(o => o.Children.ElementAt(patternP.K - 1)).ToList();
                return new Node(nodes.ElementAt(k - 1).Children.ElementAt(patternP.K - 1));
            }
            else
            {
                var nodes = MatchManager.Matches(target.Value, kmatch.Tree);
                return new Node(nodes.ElementAt(k - 1));      
            }
        }

        public static Pattern NMatch(Pattern kmatch, string id)
        {
            return kmatch;
        }

        public static IEnumerable<Node> Template(Node node, Pattern pattern)
        {
            var currentTree = node.Value;
            var res = new List<Node>();
            if (pattern.Tree.Value.Kind == SyntaxKind.EmptyStatement)
            {
                var list = FlorestByKind(pattern, currentTree);

                if (list.Any()) res = CreateRegions(list);
            }
            res = SingleLocations(res);
            return res;
        }

        public static IEnumerable<Node> Traversal(Node node, string type)
        {
            var traversal = new TreeTraversal<SyntaxNodeOrToken>();
            var itreenode = node.Value;
            var nodes = traversal.PostOrderTraversal(itreenode).ToList();
            var result = new List<Node>();
            foreach (var n in nodes)
            {
                //var emptyKind = SyntaxKind.EmptyStatement;
                result.Add(new Node(n));
                //var parentEmpty = new Node(new TreeNode<SyntaxNodeOrToken>(SyntaxFactory.EmptyStatement(), new TLabel(emptyKind), new List<ITreeNode<SyntaxNodeOrToken>> {n}));
                //result.Add(parentEmpty);
            }
            return result;
        }
         
        private static List<Node> SingleLocations(List<Node> res)
        {
            var list = new List<Node>();
            var candidates = new List<Tuple<Node, Node>> ();
            bool [] intersect = new bool[res.Count]; 

            for (int i = 0; i < res.Count; i++)
            {
                var v = res[i].Value.Value;
                for (int j =  i + 1; j < res.Count; j++)
                {
                    var c = res[j].Value.Value;
                    if (v.Span.Contains(c.Span) || c.Span.Contains(v.Span))
                    {
                        intersect[i] = true;
                        intersect[j] = true;
                        candidates.Add(Tuple.Create(res[i], res[j]));
                        break;
                    }
                }

                if (!intersect[i])
                {
                    list.Add(res[i]);
                }
            }

            list.AddRange(candidates.Select(v => v.Item1.Value.Value.Span.Contains(v.Item2.Value.Value.Span) ? v.Item2 : v.Item1));
            return list.OrderBy(o => o.Value.Value.SpanStart).ToList();
        }

        private static List<Node> CreateRegions(List<List<SyntaxNodeOrToken>> list)
        {
            var regions = new List<Node>();
            for (int j = 0; j < list.First().Count; j++)
            {
                ITreeNode<SyntaxNodeOrToken> iTree = new TreeNode<SyntaxNodeOrToken>(SyntaxFactory.EmptyStatement(), new TLabel(SyntaxKind.EmptyStatement));
                for (int i = 0; i < list.Count; i++)
                {
                    var child = list[i][j];
                    var newchild = ConverterHelper.ConvertCSharpToTreeNode(child);
                    iTree.AddChild(newchild, i);
                }

                var beforeFlorest = iTree.Children.Select(o => o.Value).ToList();
                var emptyStatement = SyntaxFactory.EmptyStatement();
                var newtree = new TreeNode<SyntaxNodeOrToken>(emptyStatement, new TLabel(SyntaxKind.EmptyStatement));
                newtree.AddChild(iTree.Children.First(), 0);
                var newNode = new Node(newtree);
                //BeforeAfterMapping[newNode] = beforeFlorest;

                regions.Add(newNode);
            }
            return regions;
        }

        private static List<List<SyntaxNodeOrToken>> FlorestByKind(Pattern match, ITreeNode<SyntaxNodeOrToken> currentTree)
        {
            var list = new List<List<SyntaxNodeOrToken>>();
            foreach (var child in match.Tree.Children)
            {
                var nodeList = SplitToNodes(currentTree, child.Value.Kind);
                var result = (from node in nodeList where MatchManager.IsValue(node, child) select node.Value).ToList();

                if (result.Any())
                {
                    list.Add(result);
                }
            }
            return list;
        }

        /// <summary>
        /// Syntax node factory. This method will be removed in future
        /// </summary>
        /// <param name="kind">SyntaxKind of the node that will be created.</param>
        /// <param name="children">Children nodes.</param>
        /// <param name="node">Node</param>
        /// <returns>A SyntaxNode with specific king and children</returns>
        private static SyntaxNodeOrToken GetSyntaxElement(SyntaxKind kind, List<SyntaxNodeOrToken> children, SyntaxNodeOrToken node = default(SyntaxNodeOrToken), List<SyntaxNodeOrToken> identifiers = null)
        {
            switch (kind)
            { 
                case SyntaxKind.CastExpression:
                {
                    var typeSyntax = (TypeSyntax) children[0];
                    var expressionSyntax = (ExpressionSyntax) children[1];
                    var castExpression = SyntaxFactory.CastExpression(typeSyntax, expressionSyntax);
                    return castExpression;
                }
                case SyntaxKind.SwitchSection:
                {
                    var labels =
                        children.Where(o => o.IsKind(SyntaxKind.CaseSwitchLabel))
                            .Select(o => (SwitchLabelSyntax) o)
                            .ToList();
                    var values =
                        children.Where(o => !o.IsKind(SyntaxKind.CaseSwitchLabel))
                            .Select(o => (StatementSyntax) o)
                            .ToList();
                    var labelList = SyntaxFactory.List(labels);
                    var valueList = SyntaxFactory.List(values);
                    var switchSection = SyntaxFactory.SwitchSection(labelList, valueList);
                    return switchSection;
                }
                case SyntaxKind.CaseSwitchLabel:
                {
                    var expressionSyntax = (ExpressionSyntax) children.First();
                    var caseSwitchLabel = SyntaxFactory.CaseSwitchLabel(expressionSyntax);
                    return caseSwitchLabel;
                }
                case SyntaxKind.NameColon:
                {
                    var identifier = (IdentifierNameSyntax) children[0];
                    var nameColon = SyntaxFactory.NameColon(identifier);
                    return nameColon;
                }
                case SyntaxKind.AddExpression:
                case SyntaxKind.GreaterThanOrEqualExpression:
                case SyntaxKind.LogicalAndExpression:
                {
                    var leftExpression = (ExpressionSyntax) children[0];
                    var rightExpresssion = (ExpressionSyntax) children[1];
                    var logicalAndExpression = SyntaxFactory.BinaryExpression(kind,
                        leftExpression, rightExpresssion);
                    return logicalAndExpression;
                }
                case SyntaxKind.LocalDeclarationStatement:
                {
                    var variableDeclation = (Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax) children[0];
                    var localDeclaration = SyntaxFactory.LocalDeclarationStatement(variableDeclation);
                    return localDeclaration;
                }
                case SyntaxKind.ForEachStatement:
                {
                    var foreachStt = (ForEachStatementSyntax) node;
                    var identifier = foreachStt.Identifier;
                    var typesyntax = (TypeSyntax) children[0];
                    var expressionSyntax = (ExpressionSyntax) children[1];
                    var statementSyntax = (StatementSyntax) children[2];
                    var foreachstatement = SyntaxFactory.ForEachStatement(typesyntax, identifier, expressionSyntax, statementSyntax);
                    return foreachstatement;
                }
                case SyntaxKind.VariableDeclaration:
                {
                    var typeSyntax = (TypeSyntax) children[0];
                    var listArguments = new List<VariableDeclaratorSyntax>();
                    for (int i = 1; i < children.Count; i++)
                    {
                        var variable = (VariableDeclaratorSyntax) children[i];
                        listArguments.Add(variable);
                    }
                    var spal = SyntaxFactory.SeparatedList(listArguments);
                    var variableDeclaration = SyntaxFactory.VariableDeclaration(typeSyntax, spal);
                    return variableDeclaration;
                }
                case SyntaxKind.VariableDeclarator:
                {
                    var property = (VariableDeclaratorSyntax) node;
                    var identifer = property.Identifier;
                    var equalsExpression = (EqualsValueClauseSyntax) children[0];
                    var variableDeclaration = SyntaxFactory.VariableDeclarator(identifer, null, equalsExpression);
                    return variableDeclaration;
                }
                case SyntaxKind.ExpressionStatement:
                {
                    ExpressionSyntax expression = (ExpressionSyntax) children.First();
                    ExpressionStatementSyntax expressionStatement = SyntaxFactory.ExpressionStatement(expression);
                    return expressionStatement;
                }
                case SyntaxKind.Block:
                {
                    var statetements = children.Select(child => (StatementSyntax) child).ToList();

                    var block = SyntaxFactory.Block(statetements);
                    return block;
                }
                case SyntaxKind.InvocationExpression:
                {
                    if (!identifiers.Any())
                    {
                        var expressionSyntax = (ExpressionSyntax) children[0];
                        ArgumentListSyntax argumentList = (ArgumentListSyntax) children[1];
                        var invocation = SyntaxFactory.InvocationExpression(expressionSyntax, argumentList);
                        return invocation;
                    }
                    else
                    {
                        var expressionSyntax = (ExpressionSyntax) GetSyntaxElement(SyntaxKind.IdentifierName, null, null, identifiers);
                        ArgumentListSyntax argumentList = (ArgumentListSyntax) children[0];
                        var invocation = SyntaxFactory.InvocationExpression(expressionSyntax, argumentList);
                        return invocation;
                    }

                }
                case SyntaxKind.SimpleMemberAccessExpression:
                {
                    if (!identifiers.Any())
                    {
                        var expressionSyntax = (ExpressionSyntax) children[0];
                        var syntaxName = (SimpleNameSyntax) children[1];
                        var simpleMemberExpression = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expressionSyntax, syntaxName);
                        return simpleMemberExpression;
                    }
                    else
                    {
                        var expressionSyntax = (ExpressionSyntax)children[0];
                        var syntaxName = (SimpleNameSyntax) GetSyntaxElement(SyntaxKind.IdentifierName, null, null, identifiers);
                        var simpleMemberExpression = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expressionSyntax, syntaxName);
                        return simpleMemberExpression;
                    }
                }
                case SyntaxKind.ElementAccessExpression:
                {
                    var expressionSyntax = (ExpressionSyntax) children[0];
                    var bracketArgumentList = (BracketedArgumentListSyntax) children[1];
                    var elementAccessExpression = SyntaxFactory.ElementAccessExpression(expressionSyntax, bracketArgumentList);
                    return elementAccessExpression;
                }
                case SyntaxKind.TypeOfExpression:
                {
                    var typeSyntax = (TypeSyntax) children[0];
                    var typeofExpression = SyntaxFactory.TypeOfExpression(typeSyntax);
                    return typeofExpression;
                }
                case SyntaxKind.ObjectCreationExpression:
                {
                    var typeSyntax = (TypeSyntax) children[0];
                    var argumentList = (ArgumentListSyntax) children[1];
                    var objectcreation = SyntaxFactory.ObjectCreationExpression(typeSyntax, argumentList, null);
                    objectcreation = objectcreation.WithAdditionalAnnotations(Formatter.Annotation);
                    return objectcreation;
                }
                case SyntaxKind.ParameterList:
                {
                    var parameter = (ParameterSyntax) children[0];
                    var spal = SyntaxFactory.SeparatedList(new[] {parameter});
                    var parameterList = SyntaxFactory.ParameterList(spal);
                    return parameterList;
                }
                case SyntaxKind.ArrayInitializerExpression:
                {
                    var expressionSyntaxs = children.Select(child => (ExpressionSyntax)child).ToList();
                    var spal = SyntaxFactory.SeparatedList(expressionSyntaxs);
                    var arrayInitializer = SyntaxFactory.InitializerExpression(kind, spal);
                    return arrayInitializer;
                }
                case SyntaxKind.Parameter:
                {
                    return children.First().Parent;
                }
                case SyntaxKind.BracketedArgumentList:
                {
                    var listArguments = children.Select(child => (ArgumentSyntax)child).ToList();
                    var spal = SyntaxFactory.SeparatedList(listArguments);
                    var bracketedArgumentList = SyntaxFactory.BracketedArgumentList(spal);
                    return bracketedArgumentList;
                }
                case SyntaxKind.ArgumentList:
                {
                    var listArguments = children.Select(child => (ArgumentSyntax) child).ToList();

                    var spal = SyntaxFactory.SeparatedList(listArguments);
                    var argumentList = SyntaxFactory.ArgumentList(spal);
                    return argumentList;
                }
                case SyntaxKind.Argument:
                    if (children.Count() == 1)
                    {
                        ExpressionSyntax s = (ExpressionSyntax) children.First();
                        var argument = SyntaxFactory.Argument(s);
                        return argument;
                    }
                    else
                    {
                        var ncolon = (NameColonSyntax) children[0];
                        var expression = (ExpressionSyntax) children[1];
                        var argument = SyntaxFactory.Argument(ncolon, default(SyntaxToken), expression);
                        return argument;
                    }
                case SyntaxKind.ParenthesizedExpression:
                {
                    var expressionSyntax = (ExpressionSyntax) children[0];
                    var parenthizedExpression = SyntaxFactory.ParenthesizedExpression(expressionSyntax);
                    return parenthizedExpression;
                }
                case SyntaxKind.SimpleLambdaExpression:
                {
                    var parameter = (ParameterSyntax)children[0];
                    var csharpbody = (CSharpSyntaxNode)children[1];
                    var simpleLambdaExpression = SyntaxFactory.SimpleLambdaExpression(parameter, csharpbody);
                    return simpleLambdaExpression;
                }
                case SyntaxKind.ParenthesizedLambdaExpression:
                {
                    var parameterList = (ParameterListSyntax) children[0];
                    var csharpbody = (CSharpSyntaxNode) children[1];
                    var parenthizedLambdaExpression = SyntaxFactory.ParenthesizedLambdaExpression(parameterList,
                        csharpbody);
                    return parenthizedLambdaExpression;
                }
                case SyntaxKind.EqualsExpression:
                {
                    var left = (ExpressionSyntax) children[0];
                    var right = (ExpressionSyntax) children[1];
                    var equalsExpression = SyntaxFactory.BinaryExpression(SyntaxKind.EqualsExpression, left, right);
                    return equalsExpression;
                }
                case SyntaxKind.EqualsValueClause:
                {
                    var expressionSyntax = (ExpressionSyntax) children[0];
                    var equalsValueClause = SyntaxFactory.EqualsValueClause(expressionSyntax);
                    return equalsValueClause;
                }
                case SyntaxKind.ConditionalExpression:
                {
                    var condition = (ExpressionSyntax) children[0];
                    var whenTrue = (ExpressionSyntax) children[1];
                    var whenFalse = (ExpressionSyntax) children[2];
                    var conditionalExpression = SyntaxFactory.ConditionalExpression(condition, whenTrue, whenFalse);
                    return conditionalExpression;
                }
                case SyntaxKind.IfStatement:
                {
                    var condition = (ExpressionSyntax) children[0];
                    var statementSyntax = (StatementSyntax) children[1];
                    var ifStatement = SyntaxFactory.IfStatement(condition, statementSyntax);
                    return ifStatement;
                }
                case SyntaxKind.LogicalNotExpression:
                case SyntaxKind.UnaryMinusExpression:
                {
                    ExpressionSyntax expression = (ExpressionSyntax) children[0];
                    var unary = SyntaxFactory.PrefixUnaryExpression(kind, expression);
                    return unary;
                }
                case SyntaxKind.YieldReturnStatement:
                {
                    var expression = (ExpressionSyntax) children[0];
                    var yieldReturn = SyntaxFactory.YieldStatement(kind, expression);
                    return yieldReturn;
                }
                case SyntaxKind.ReturnStatement:
                {
                    ExpressionSyntax expression = (ExpressionSyntax) children[0];
                    var returnStatement = SyntaxFactory.ReturnStatement(expression);
                    return returnStatement;
                }
                case SyntaxKind.ElseClause:
                {
                    var statatementSyntax = (StatementSyntax) children[0];
                    var elseClause = SyntaxFactory.ElseClause(statatementSyntax);
                    return elseClause;
                }
                case SyntaxKind.IdentifierName:
                {
                    SyntaxToken stoken = (SyntaxToken) identifiers.First();
                    var identifierName = SyntaxFactory.IdentifierName(stoken);
                    return identifierName;
                }
                case SyntaxKind.TypeArgumentList:
                {
                    var listType = children.Select(child => (TypeSyntax) child).ToList();

                    var typespal = SyntaxFactory.SeparatedList(listType);
                    var typeArgument = SyntaxFactory.TypeArgumentList(typespal);
                    return typeArgument;
                }
                case SyntaxKind.GenericName:
                {
                    var gName = (GenericNameSyntax) node;
                    var typeArg = (TypeArgumentListSyntax) children[0];
                    var genericName = SyntaxFactory.GenericName(gName.Identifier, typeArg);
                    return genericName;
                }
                case SyntaxKind.NotEqualsExpression:
                {
                    var leftExpression = (ExpressionSyntax) children[0];
                    var rightExpression = (ExpressionSyntax) children[1];
                    var notEqualsExpression = SyntaxFactory.BinaryExpression(SyntaxKind.NotEqualsExpression,
                        leftExpression, rightExpression);
                    return notEqualsExpression;
                }
            }
            return null;
        }

        /// <summary>
        /// Reconstruct the tree
        /// </summary>
        /// <param name="tree">Tree in another format</param>
        /// <returns>Reconstructed tree</returns>
        public static SyntaxNodeOrToken ReconstructTree(ITreeNode<SyntaxNodeOrToken> tree)
        {
            if (!tree.Children.Any())
            {
                return tree.Value;
            }
            List<SyntaxNodeOrToken> children = new List<SyntaxNodeOrToken>();
            List<SyntaxNodeOrToken> identifier = new List<SyntaxNodeOrToken>();
            foreach (var v in tree.Children)
            {
                if (v.Value.IsNode)
                {
                    var result = ReconstructTree(v);
                    children.Add(result);
                }
                else
                {
                    identifier.Add(v.Value);
                }
            }
            //children = tree.Children.Select(ReconstructTree).ToList();
            var node = GetSyntaxElement((SyntaxKind) tree.Label.Label, children, tree.Value, identifier);
            return node;
        }
    }
}
