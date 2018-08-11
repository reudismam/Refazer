using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TreeElement.Spg.Node;

namespace TreeEdit.Spg.Builder
{
    // ReSharper disable once InconsistentNaming
    public class ASTBuilder
    {
        /// <summary>
        /// Reconstructs the tree
        /// </summary>
        /// <param name="target">Target node</param>
        /// <param name="tree">Tree in another format</param>
        /// <returns>Reconstructed tree</returns>
        public static SyntaxNodeOrToken ReconstructTree(SyntaxNodeOrToken target, TreeNode<SyntaxNodeOrToken> tree)
        {
            SyntaxNodeOrToken newNode = ReconstructTree(tree);
            if (newNode.IsKind(SyntaxKind.None)) return tree.Value;
            //newNode = newNode.AsNode().NormalizeWhitespace();
            //newNode = newNode.WithLeadingTrivia(target.GetLeadingTrivia());
            //newNode = newNode.WithTrailingTrivia(target.GetTrailingTrivia());
            return newNode;
        }

        /// <summary>
        /// Reconstructs the tree
        /// </summary>
        /// <param name="tree">Tree in another format</param>
        /// <returns>Reconstructed tree</returns>
        public static SyntaxNodeOrToken ReconstructTree(TreeNode<SyntaxNodeOrToken> tree)
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
            var node = GetSyntaxElement((SyntaxKind)tree.Label.Label, children, tree.Value, identifier);
            return node;
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
            case SyntaxKind.ArrayCreationExpression:
                {
                    var arrayType = (ArrayTypeSyntax)children[0];
                    var newToken = SyntaxFactory.Token(SyntaxKind.NewKeyword).WithTrailingTrivia(new List<SyntaxTrivia> { SyntaxFactory.Space });
                    var arrayCreation = SyntaxFactory.ArrayCreationExpression(newToken, arrayType, null);
                    return arrayCreation;
                }
            case SyntaxKind.ImplicitArrayCreationExpression:
                {
                    var initializerExpression = (InitializerExpressionSyntax)children[0];
                    var arrayCreation = SyntaxFactory.ImplicitArrayCreationExpression(initializerExpression);
                    return arrayCreation;
                }
            case SyntaxKind.ArrayRankSpecifier:
                {
                    var expressions = children.Select(o => (ExpressionSyntax)o);
                    var spal = SyntaxFactory.SeparatedList<ExpressionSyntax>(expressions);
                    var arrayRank = SyntaxFactory.ArrayRankSpecifier(spal);
                    return arrayRank;
                }
            case SyntaxKind.ArrayType:
                {
                    var typeSyntax = (TypeSyntax)children[0];
                    var arrayRankList =
                        children.Where(o => o.IsKind(SyntaxKind.ArrayRankSpecifier))
                            .Select(o => (ArrayRankSpecifierSyntax)o);
                    var syntaxList = new SyntaxList<ArrayRankSpecifierSyntax>();
                    syntaxList.AddRange(arrayRankList);
                    var arrayType = SyntaxFactory.ArrayType(typeSyntax, syntaxList);
                    return arrayType;
                }
            case SyntaxKind.CatchDeclaration:
                {
                    var typesyntax = (TypeSyntax)children[0];
                    var catchDeclaration = SyntaxFactory.CatchDeclaration(typesyntax);
                    return catchDeclaration;
                }
            case SyntaxKind.CatchClause:
                {
                    var catchDeclaration =
                        (CatchDeclarationSyntax)children.SingleOrDefault(o => o.IsKind(SyntaxKind.CatchDeclaration));
                    var catchFilter =
                        (CatchFilterClauseSyntax)children.SingleOrDefault(o => o.IsKind(SyntaxKind.CatchFilterClause));
                    var body = (BlockSyntax)children.SingleOrDefault(o => o.IsKind(SyntaxKind.Block));

                    var catchClause = SyntaxFactory.CatchClause(catchDeclaration, catchFilter, body);
                    return catchClause;
                }
            case SyntaxKind.TryStatement:
                {
                    var body = (BlockSyntax)children[0];
                    var catches =
                        children.Where(o => o.IsKind(SyntaxKind.CatchClause))
                            .Select(o => (CatchClauseSyntax)o)
                            .ToList();
                    var spal = new SyntaxList<CatchClauseSyntax>();
                    spal.AddRange(catches);
                    var finallyClause =
                        children.Where(o => o.IsKind(SyntaxKind.FinallyClause))
                            .Select(o => (FinallyClauseSyntax)o)
                            .SingleOrDefault();
                    var tryStatement = SyntaxFactory.TryStatement(body, spal, finallyClause);
                    return tryStatement;
                }
            case SyntaxKind.FinallyClause:
                {
                    var blockSyntax = (BlockSyntax)children[0];
                    var finallyClause = SyntaxFactory.FinallyClause(blockSyntax);
                    return finallyClause;
                }
            case SyntaxKind.ThrowStatement:
                {
                    var expressionSyntax = (ExpressionSyntax)children[0];
                    var throwStatement = SyntaxFactory.ThrowStatement(expressionSyntax);
                    return throwStatement;
                }
            case SyntaxKind.MethodDeclaration:
                {
                    var method = (MethodDeclarationSyntax)node;
                    if (identifiers != null && identifiers.Any(o => o.IsKind(SyntaxKind.IdentifierToken)))
                    {
                        var index = identifiers.FindIndex(o => o.IsKind(SyntaxKind.IdentifierToken));
                        var name = (SyntaxToken)identifiers[index];
                        method = method.WithIdentifier(name);
                    }

                    if (identifiers != null)
                    {
                        var modifiers = new List<SyntaxToken>();
                        if (identifiers.Any(ConverterHelper.IsAcessModifier))
                        {
                            var index = identifiers.FindIndex(ConverterHelper.IsAcessModifier);
                            modifiers.Add((SyntaxToken)identifiers[index]);
                        }

                        if (identifiers.Any(o => o.IsKind(SyntaxKind.SealedKeyword)))
                        {
                            var index = identifiers.FindIndex(o => o.IsKind(SyntaxKind.SealedKeyword));
                            modifiers.Add((SyntaxToken)identifiers[index]);
                        }

                        if (identifiers.Any(o => o.IsKind(SyntaxKind.StaticKeyword)))
                        {
                            var index = identifiers.FindIndex(o => o.IsKind(SyntaxKind.StaticKeyword));
                            modifiers.Add((SyntaxToken)identifiers[index]);
                        }

                        if (identifiers.Any(o => o.IsKind(SyntaxKind.OverrideKeyword)))
                        {
                            var index = identifiers.FindIndex(o => o.IsKind(SyntaxKind.OverrideKeyword));
                            modifiers.Add((SyntaxToken)identifiers[index]);
                        }

                        if (modifiers.Any())
                        {
                            method = method.AddModifiers(modifiers.ToArray());
                        }
                    }

                    if (children.Any(o => o.IsKind(SyntaxKind.ArrowExpressionClause)))
                    {
                        var index = children.FindIndex(o => o.IsKind(SyntaxKind.PredefinedType));
                        method = method.WithExpressionBody((ArrowExpressionClauseSyntax)children[index]);
                    }

                    if (children.Any(o => o.IsKind(SyntaxKind.AttributeList)))
                    {
                        var index = children.FindIndex(o => o.IsKind(SyntaxKind.AttributeList));
                        var syntaList = new SyntaxList<AttributeListSyntax>();
                        var attributeListSyntax = (AttributeListSyntax)children[index];
                        method = method.WithAttributeLists(syntaList);
                        method = method.AddAttributeLists(attributeListSyntax);
                    }

                    if (children.Any(o => o.IsKind(SyntaxKind.PredefinedType)))
                    {
                        var index = children.FindIndex(o => o.IsKind(SyntaxKind.PredefinedType));
                        method = method.WithReturnType((TypeSyntax)children[index]);
                    }

                    if (children.Any(o => o.IsKind(SyntaxKind.ParameterList)))
                    {
                        var index = children.FindIndex(o => o.IsKind(SyntaxKind.ParameterList));
                        method = method.WithParameterList((ParameterListSyntax)children[index]);
                    }

                    if (children.Any(o => o.IsKind(SyntaxKind.Block)))
                    {
                        var index = children.FindIndex(o => o.IsKind(SyntaxKind.Block));
                        method = method.WithBody((BlockSyntax)children[index]);
                    }

                    return method;
                }

            case SyntaxKind.PropertyDeclaration:
                {
                    var type = (TypeSyntax)children[0];
                    string name = null;

                    if (identifiers != null && identifiers.Any(o => o.IsKind(SyntaxKind.IdentifierToken)))
                    {
                        var index = identifiers.FindIndex(o => o.IsKind(SyntaxKind.IdentifierToken));
                        name = identifiers[index].ToString();
                    }

                    var property = SyntaxFactory.PropertyDeclaration(type, name);

                    if (identifiers != null)
                    {
                        var modifiers = new List<SyntaxToken>();
                        if (identifiers.Any(ConverterHelper.IsAcessModifier))
                        {
                            var index = identifiers.FindIndex(ConverterHelper.IsAcessModifier);
                            modifiers.Add((SyntaxToken)identifiers[index]);
                        }

                        if (identifiers.Any(o => o.IsKind(SyntaxKind.SealedKeyword)))
                        {
                            var index = identifiers.FindIndex(o => o.IsKind(SyntaxKind.SealedKeyword));
                            modifiers.Add((SyntaxToken)identifiers[index]);
                        }

                        if (identifiers.Any(o => o.IsKind(SyntaxKind.StaticKeyword)))
                        {
                            var index = identifiers.FindIndex(o => o.IsKind(SyntaxKind.StaticKeyword));
                            modifiers.Add((SyntaxToken)identifiers[index]);
                        }

                        if (identifiers.Any(o => o.IsKind(SyntaxKind.OverrideKeyword)))
                        {
                            var index = identifiers.FindIndex(o => o.IsKind(SyntaxKind.OverrideKeyword));
                            modifiers.Add((SyntaxToken)identifiers[index]);
                        }

                        if (modifiers.Any())
                        {
                            property = property.AddModifiers(modifiers.ToArray());
                        }
                    }

                    if (children.Any(o => o.IsKind(SyntaxKind.ArrowExpressionClause)))
                    {
                        var index = children.FindIndex(o => o.IsKind(SyntaxKind.PredefinedType));
                        property = property.WithExpressionBody((ArrowExpressionClauseSyntax)children[index]);
                    }

                    if (children.Any(o => o.IsKind(SyntaxKind.AccessorList)))
                    {
                        var index = children.FindIndex(o => o.IsKind(SyntaxKind.AccessorList));
                        property = property.WithAccessorList((AccessorListSyntax)children[index]);
                    }

                    return property;
                }
            case SyntaxKind.CastExpression:
                {
                    var typeSyntax = (TypeSyntax)children[0];
                    var expressionSyntax = (ExpressionSyntax)children[1];
                    var castExpression = SyntaxFactory.CastExpression(typeSyntax, expressionSyntax);
                    return castExpression;
                }
            case SyntaxKind.SwitchSection:
                {
                    var labels =
                        children.Where(o => o.IsKind(SyntaxKind.CaseSwitchLabel) || o.IsKind(SyntaxKind.DefaultSwitchLabel))
                            .Select(o => (SwitchLabelSyntax)o)
                            .ToList();
                    var values =
                        children.Where(o => !(o.IsKind(SyntaxKind.CaseSwitchLabel) || o.IsKind(SyntaxKind.DefaultSwitchLabel)))
                            .Select(o => (StatementSyntax)o)
                            .ToList();
                    var labelList = SyntaxFactory.List(labels);
                    var valueList = SyntaxFactory.List(values);
                    var switchSection = SyntaxFactory.SwitchSection(labelList, valueList);
                    return switchSection;
                }
            case SyntaxKind.CaseSwitchLabel:
                {
                    var expressionSyntax = (ExpressionSyntax)children.First();
                    var caseSwitchLabel = SyntaxFactory.CaseSwitchLabel(expressionSyntax);
                    return caseSwitchLabel;
                }
            case SyntaxKind.SwitchStatement:
                {
                    if (children.Count > 1)
                    {
                        var expressionSyntax = (ExpressionSyntax)children.First();
                        var switchSections = new List<SwitchSectionSyntax>();
                        for (int i = 1; i < children.Count; i++)
                        {
                            var switchSection = (SwitchSectionSyntax)children[i];
                            switchSections.Add(switchSection);
                        }
                        var syntaxList = SyntaxFactory.List(switchSections);
                        var switchStatement = SyntaxFactory.SwitchStatement(expressionSyntax, syntaxList);
                        return switchStatement;
                    }
                    else
                    {
                        var expression = (ExpressionSyntax)children.First();
                        var switchStatement = SyntaxFactory.SwitchStatement(expression);
                        return switchStatement;
                    }
                }
            case SyntaxKind.QualifiedName:
                {
                    var leftSyntax = (NameSyntax)children[0];
                    var rightSyntax = (SimpleNameSyntax)children[1];
                    var qualifiedName = SyntaxFactory.QualifiedName(leftSyntax, rightSyntax);
                    return qualifiedName;
                }
            case SyntaxKind.NameEquals:
                {
                    var identifierNameSyntax = (IdentifierNameSyntax)children[0];
                    var nameEquals = SyntaxFactory.NameEquals(identifierNameSyntax);
                    return nameEquals;
                }
            case SyntaxKind.NameColon:
                {
                    var identifier = (IdentifierNameSyntax)children[0];
                    var nameColon = SyntaxFactory.NameColon(identifier);
                    return nameColon;
                }
            case SyntaxKind.CoalesceExpression:
            case SyntaxKind.GreaterThanExpression:
            case SyntaxKind.LessThanExpression:
            case SyntaxKind.LessThanOrEqualExpression:
            case SyntaxKind.DivideExpression:
            case SyntaxKind.MultiplyExpression:
            case SyntaxKind.BitwiseAndExpression:
            case SyntaxKind.BitwiseOrExpression:
            case SyntaxKind.AsExpression:
            case SyntaxKind.AddExpression:
            case SyntaxKind.SubtractExpression:
            case SyntaxKind.GreaterThanOrEqualExpression:
            case SyntaxKind.LogicalOrExpression:
            case SyntaxKind.LogicalAndExpression:
                {
                    var leftExpression = (ExpressionSyntax)children[0];
                    var rightExpresssion = (ExpressionSyntax)children[1];
                    var logicalAndExpression = SyntaxFactory.BinaryExpression(kind,
                        leftExpression, rightExpresssion);
                    return logicalAndExpression;
                }
            case SyntaxKind.SubtractAssignmentExpression:
            case SyntaxKind.DivideAssignmentExpression:
            case SyntaxKind.MultiplyAssignmentExpression:
            case SyntaxKind.AddAssignmentExpression:
            case SyntaxKind.SimpleAssignmentExpression:
                {
                    var leftExpression = (ExpressionSyntax)children[0];
                    var rightExpression = (ExpressionSyntax)children[1];
                    var simpleAssignment = SyntaxFactory.AssignmentExpression(kind, leftExpression, rightExpression);
                    return simpleAssignment;
                }
            case SyntaxKind.LocalDeclarationStatement:
                {
                    var variableDeclation = (Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax)children[0];
                    var localDeclaration = SyntaxFactory.LocalDeclarationStatement(variableDeclation);
                    return localDeclaration;
                }
            case SyntaxKind.ForEachStatement:
                {
                    var foreachStt = (ForEachStatementSyntax)node;
                    var identifier = foreachStt.Identifier;
                    var typesyntax = (TypeSyntax)children[0];
                    var expressionSyntax = (ExpressionSyntax)children[1];
                    var statementSyntax = (StatementSyntax)children[2];
                    var foreachstatement = SyntaxFactory.ForEachStatement(typesyntax, identifier, expressionSyntax,
                        statementSyntax);
                    return foreachstatement;
                }
            case SyntaxKind.UsingStatement:
                {

                    VariableDeclarationSyntax variableDeclaration = null;
                    ExpressionSyntax expression = null;
                    if (children[0].IsKind(SyntaxKind.VariableDeclaration))
                    {
                        variableDeclaration = (VariableDeclarationSyntax)children[0];
                    }
                    else
                    {
                        expression = (ExpressionSyntax)children[0];
                    }
                    var statementSyntax = (StatementSyntax)children[1];
                    var usingStatement = SyntaxFactory.UsingStatement(variableDeclaration, expression, statementSyntax);
                    return usingStatement;
                }
            case SyntaxKind.VariableDeclaration:
                {
                    var typeSyntax = (TypeSyntax)children[0];
                    var listArguments = new List<VariableDeclaratorSyntax>();
                    for (int i = 1; i < children.Count; i++)
                    {
                        var variable = (VariableDeclaratorSyntax)children[i];
                        listArguments.Add(variable);
                    }
                    var spal = SyntaxFactory.SeparatedList(listArguments);
                    var variableDeclaration = SyntaxFactory.VariableDeclaration(typeSyntax, spal);
                    return variableDeclaration;
                }
            case SyntaxKind.VariableDeclarator:
                {
                    var property = (VariableDeclaratorSyntax)node;
                    SyntaxToken identifier = default(SyntaxToken);
                    if (identifiers.Any(o => o.IsKind(SyntaxKind.IdentifierToken)))
                    {
                        var index = identifiers.FindIndex(o => o.IsKind(SyntaxKind.IdentifierToken));
                        identifier = (SyntaxToken)identifiers[index];
                    }
                    else
                    {
                        identifier = property.Identifier;
                    }

                    EqualsValueClauseSyntax equalsExpression = null;
                    if (children.Any(o => o.IsKind(SyntaxKind.EqualsValueClause)))
                    {
                        var index = children.FindIndex(o => o.IsKind(SyntaxKind.EqualsValueClause));
                        equalsExpression = (EqualsValueClauseSyntax)children[index];
                    }
                    var variableDeclaration = SyntaxFactory.VariableDeclarator(identifier, null, equalsExpression);
                    return variableDeclaration;
                }
            case SyntaxKind.ExpressionStatement:
                {
                    ExpressionSyntax expression = (ExpressionSyntax)children.First();
                    ExpressionStatementSyntax expressionStatement = SyntaxFactory.ExpressionStatement(expression);
                    return expressionStatement;
                }
            case SyntaxKind.Block:
                {
                    var statetements = children.Select(child => (StatementSyntax)child).ToList();

                    var block = SyntaxFactory.Block(statetements);
                    return block;
                }
            case SyntaxKind.InvocationExpression:
                {
                    Debug.Assert(identifiers != null, "identifiers != null");
                    if (!identifiers.Any())
                    {
                        var expressionSyntax = (ExpressionSyntax)children[0];
                        ArgumentListSyntax argumentList = (ArgumentListSyntax)children[1];
                        var invocation = SyntaxFactory.InvocationExpression(expressionSyntax, argumentList);
                        return invocation;
                    }
                    else
                    {
                        var expressionSyntax =
                            (ExpressionSyntax)GetSyntaxElement(SyntaxKind.IdentifierName, null, null, identifiers);
                        ArgumentListSyntax argumentList = (ArgumentListSyntax)children[0];
                        var invocation = SyntaxFactory.InvocationExpression(expressionSyntax, argumentList);
                        return invocation;
                    }
                }
            case SyntaxKind.AwaitExpression:
                {
                    Debug.Assert(identifiers != null, "identifiers != null");
                    if (identifiers.Any())
                    {
                        var awaitToken = (SyntaxToken)identifiers.First();
                        var expressionSyntax = (ExpressionSyntax)children.First();
                        var awaitExpression = SyntaxFactory.AwaitExpression(awaitToken, expressionSyntax);
                        return awaitExpression;
                    }
                    else
                    {
                        var expressionSyntax = (ExpressionSyntax)children.First();
                        var awaitExpresion = SyntaxFactory.AwaitExpression(expressionSyntax);
                        return awaitExpresion;
                    }
                }
            case SyntaxKind.SimpleMemberAccessExpression:
                {
                    if (!identifiers.Any())
                    {
                        var expressionSyntax = (ExpressionSyntax)children[0];
                        var syntaxName = (SimpleNameSyntax)children[1];
                        var simpleMemberExpression =
                            SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                expressionSyntax, syntaxName);
                        return simpleMemberExpression;
                    }
                    else
                    {
                        var expressionSyntax = (ExpressionSyntax)children[0];
                        var syntaxName =
                            (SimpleNameSyntax)GetSyntaxElement(SyntaxKind.IdentifierName, null, null, identifiers);
                        var simpleMemberExpression =
                            SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                expressionSyntax, syntaxName);
                        return simpleMemberExpression;
                    }
                }
            case SyntaxKind.ElementAccessExpression:
                {
                    var expressionSyntax = (ExpressionSyntax)children[0];
                    var bracketArgumentList = (BracketedArgumentListSyntax)children[1];
                    var elementAccessExpression = SyntaxFactory.ElementAccessExpression(expressionSyntax,
                        bracketArgumentList);
                    return elementAccessExpression;
                }
            case SyntaxKind.TypeOfExpression:
                {
                    var typeSyntax = (TypeSyntax)children[0];
                    var typeofExpression = SyntaxFactory.TypeOfExpression(typeSyntax);
                    return typeofExpression;
                }
            case SyntaxKind.ObjectCreationExpression:
                {
                    var typeSyntax = (TypeSyntax)children[0];
                    ArgumentListSyntax argumentList = null;
                    InitializerExpressionSyntax initializer = null;
                    if (children[1].IsKind(SyntaxKind.ArgumentList))
                    {
                        argumentList = (ArgumentListSyntax)children[1];
                    }
                    else
                    {
                        initializer = (InitializerExpressionSyntax)children[1];
                    }
                    var newToken =
                        SyntaxFactory.Token(SyntaxKind.NewKeyword)
                            .WithTrailingTrivia(new List<SyntaxTrivia> { SyntaxFactory.Space });
                    var objectcreation = SyntaxFactory.ObjectCreationExpression(newToken, typeSyntax, argumentList,
                        initializer);
                    return objectcreation;
                }
            case SyntaxKind.AnonymousObjectCreationExpression:
                {
                    Debug.Assert(identifiers != null, "identifiers != null");
                    if (identifiers.Any())
                    {
                        Debug.Assert(identifiers != null, "identifiers != null");
                        SyntaxToken newToken = (SyntaxToken)identifiers.First();
                        var anonymousObjectMemberDeclaratorSyntaxs =
                            children.Select(child => (AnonymousObjectMemberDeclaratorSyntax)child).ToList();
                        var spal = SyntaxFactory.SeparatedList(anonymousObjectMemberDeclaratorSyntaxs);
                        var anonymousObjectCreation = SyntaxFactory.AnonymousObjectCreationExpression(newToken,
                            SyntaxFactory.Token(SyntaxKind.OpenBraceToken), spal,
                            SyntaxFactory.Token(SyntaxKind.CloseBraceToken));
                        return anonymousObjectCreation;
                    }
                    else
                    {
                        var anonymousObjectMemberDeclaratorSyntaxs =
                            children.Select(child => (AnonymousObjectMemberDeclaratorSyntax)child).ToList();
                        var spal = SyntaxFactory.SeparatedList(anonymousObjectMemberDeclaratorSyntaxs);
                        var anonymousObjectCreation = SyntaxFactory.AnonymousObjectCreationExpression(spal);
                        return anonymousObjectCreation;
                    }
                }
            case SyntaxKind.AnonymousObjectMemberDeclarator:
                {
                    if (children.Count == 2)
                    {
                        var nameEqualsSyntax = (NameEqualsSyntax)children.First();
                        var expressionSyntax = (ExpressionSyntax)children[1];
                        var anonymousDeclarator = SyntaxFactory.AnonymousObjectMemberDeclarator(nameEqualsSyntax, expressionSyntax);
                        return anonymousDeclarator;
                    }
                    else
                    {
                        var expressionSyntax = (ExpressionSyntax)children.First();
                        var anonymousDeclarator = SyntaxFactory.AnonymousObjectMemberDeclarator(expressionSyntax);
                        return anonymousDeclarator;
                    }
                }
            case SyntaxKind.ParameterList:
                {
                    var parameterSyntaxList = new List<ParameterSyntax>();
                    children.ForEach(o => parameterSyntaxList.Add((ParameterSyntax)o));
                    var spal = SyntaxFactory.SeparatedList(parameterSyntaxList);
                    var parameterList = SyntaxFactory.ParameterList(spal);
                    return parameterList;
                }
            case SyntaxKind.ComplexElementInitializerExpression:
            case SyntaxKind.CollectionInitializerExpression:
            case SyntaxKind.ObjectInitializerExpression:
            case SyntaxKind.ArrayInitializerExpression:
                {
                    var expressionSyntaxs = children.Select(child => (ExpressionSyntax)child).ToList();
                    var spal = SyntaxFactory.SeparatedList(expressionSyntaxs);
                    var arrayInitializer = SyntaxFactory.InitializerExpression(kind, spal);
                    return arrayInitializer;
                }
            case SyntaxKind.DefaultExpression:
                {
                    var typeSyntax = (TypeSyntax)children.First();
                    var defaultExpression = SyntaxFactory.DefaultExpression(typeSyntax);
                    return defaultExpression;
                }
            case SyntaxKind.Parameter:
                {
                    ParameterSyntax parameter;
                    if (node == null && children[0].IsKind(SyntaxKind.IdentifierName))
                    {
                        var name = (IdentifierNameSyntax)children[0];
                        parameter = SyntaxFactory.Parameter(name.Identifier);
                    }
                    else
                    {
                        parameter = (ParameterSyntax)node;
                    }

                    foreach (var c in children)
                    {
                        if (c.IsKind(SyntaxKind.GenericName))
                        {
                            var type = (TypeSyntax)c;
                            parameter = parameter.WithType(type);
                        }
                        if (c.IsKind(SyntaxKind.EqualsValueClause))
                        {
                            var equalsValue = (EqualsValueClauseSyntax)c;
                            parameter = parameter.WithDefault(equalsValue);
                        }
                    }
                    return parameter;
                }
            case SyntaxKind.BracketedArgumentList:
                {
                    var listArguments = children.Select(child => (ArgumentSyntax)child).ToList();
                    var spal = SyntaxFactory.SeparatedList(listArguments);
                    var bracketedArgumentList = SyntaxFactory.BracketedArgumentList(spal);
                    return bracketedArgumentList;
                }
            case SyntaxKind.Attribute:
                {
                    var name = (NameSyntax)children[0];
                    var atributeListSyntax = new List<AttributeArgumentSyntax>();
                    for (int i = 1; i < children.Count; i++)
                    {
                        try
                        {
                            atributeListSyntax.Add((AttributeArgumentSyntax)children[i]);
                        }
                        catch (Exception e)
                        {
                            var attributeListArgument = (AttributeArgumentListSyntax)children[1];
                            var att = SyntaxFactory.Attribute(name, attributeListArgument);
                            return att;
                        }
                    }
                    var spal = SyntaxFactory.SeparatedList(atributeListSyntax);
                    var atributeListArgument = SyntaxFactory.AttributeArgumentList(spal);
                    var attribute = SyntaxFactory.Attribute(name, atributeListArgument);
                    return attribute;
                }
            case SyntaxKind.AttributeArgument:
                {
                    if (children[0].IsKind(SyntaxKind.NameEquals))
                    {
                        var nameEqualsSyntax = (NameEqualsSyntax)children[0];
                        var expressionSyntax = (ExpressionSyntax)children[1];
                        var attributeArgument = SyntaxFactory.AttributeArgument(nameEqualsSyntax, null, expressionSyntax);
                        return attributeArgument;
                    }
                    else
                    {
                        var expressionSyntax = (ExpressionSyntax)children[0];
                        var attributeArgument = SyntaxFactory.AttributeArgument(null, null, expressionSyntax);
                        return attributeArgument;
                    }
                }
            case SyntaxKind.AttributeArgumentList:
                {
                    var atributeListSyntax = new List<AttributeArgumentSyntax>();
                    for (int i = 0; i < children.Count; i++)
                    {
                        atributeListSyntax.Add((AttributeArgumentSyntax)children[i]);
                    }
                    var spal = SyntaxFactory.SeparatedList(atributeListSyntax);
                    var atributeListArgument = SyntaxFactory.AttributeArgumentList(spal);
                    return atributeListArgument;
                }
            case SyntaxKind.AttributeList:
                {
                    var attributeSyntaxList = new List<AttributeSyntax>();
                    foreach (var v in children)
                    {
                        attributeSyntaxList.Add((AttributeSyntax)v);
                    }
                    var spal = SyntaxFactory.SeparatedList(attributeSyntaxList);
                    var attibuteList = SyntaxFactory.AttributeList(spal);
                    return attibuteList;
                }
            case SyntaxKind.ArgumentList:
                {
                    var listArguments = children.Select(child => (ArgumentSyntax)child).ToList();

                    var spal = SyntaxFactory.SeparatedList(listArguments);
                    var argumentList = SyntaxFactory.ArgumentList(spal);
                    return argumentList;
                }
            case SyntaxKind.Argument:
                if (children.Count() == 1)
                {
                    ExpressionSyntax s = (ExpressionSyntax)children.First();
                    var argument = SyntaxFactory.Argument(s);
                    return argument;
                }
                else
                {
                    var ncolon = (NameColonSyntax)children[0];
                    var expression = (ExpressionSyntax)children[1];
                    var argument = SyntaxFactory.Argument(ncolon, default(SyntaxToken), expression);
                    return argument;
                }
            case SyntaxKind.ParenthesizedExpression:
                {
                    var expressionSyntax = (ExpressionSyntax)children[0];
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
                    var parameterList = (ParameterListSyntax)children[0];
                    var csharpbody = (CSharpSyntaxNode)children[1];
                    var parenthizedLambdaExpression = SyntaxFactory.ParenthesizedLambdaExpression(parameterList,
                        csharpbody);
                    return parenthizedLambdaExpression;
                }
            case SyntaxKind.EqualsExpression:
                {
                    var left = (ExpressionSyntax)children[0];
                    var right = (ExpressionSyntax)children[1];
                    var equalsExpression = SyntaxFactory.BinaryExpression(SyntaxKind.EqualsExpression, left, right);
                    return equalsExpression;
                }
            case SyntaxKind.EqualsValueClause:
                {
                    var expressionSyntax = (ExpressionSyntax)children[0];
                    var equalsValueClause = SyntaxFactory.EqualsValueClause(expressionSyntax);
                    return equalsValueClause;
                }
            case SyntaxKind.FromClause:
                {
                    Debug.Assert(identifiers != null, "identifiers != null");
                    var identifier = (SyntaxToken)identifiers.First();
                    var expression = (ExpressionSyntax)children.First();
                    var fromClause = SyntaxFactory.FromClause(identifier, expression);
                    return fromClause;
                }
            case SyntaxKind.SelectClause:
                {
                    Debug.Assert(identifiers != null, "identifiers != null");
                    if (identifiers.Any())
                    {
                        var select = (SyntaxToken)identifiers.First();
                        var expression = (ExpressionSyntax)children.First();
                        var selectCLause = SyntaxFactory.SelectClause(select, expression);
                        return selectCLause;
                    }
                    else
                    {
                        var expression = (ExpressionSyntax)children.First();
                        var selectClause = SyntaxFactory.SelectClause(expression);
                        return selectClause;
                    }
                }
            case SyntaxKind.ConditionalExpression:
                {
                    var condition = (ExpressionSyntax)children[0];
                    var whenTrue = (ExpressionSyntax)children[1];
                    var whenFalse = (ExpressionSyntax)children[2];
                    var conditionalExpression = SyntaxFactory.ConditionalExpression(condition, whenTrue, whenFalse);
                    return conditionalExpression;
                }
            case SyntaxKind.QueryBody:
                {
                    var selectOrGroupClauseSyntax = (SelectOrGroupClauseSyntax)children.First();
                    var queryBody = SyntaxFactory.QueryBody(selectOrGroupClauseSyntax);
                    return queryBody;
                }
            case SyntaxKind.IfStatement:
                {
                    var condition = (ExpressionSyntax)children[0];
                    var statementSyntax = (StatementSyntax)children[1];
                    var ifStatement = SyntaxFactory.IfStatement(condition, statementSyntax);
                    return ifStatement;
                }
            case SyntaxKind.NullableType:
                {
                    Debug.Assert(identifiers != null, "identifiers != null");
                    if (identifiers.Any())
                    {
                        var questionToken = (SyntaxToken)identifiers.First();
                        var typeSyntax = (TypeSyntax)children.First();
                        var nullableType = SyntaxFactory.NullableType(typeSyntax, questionToken);
                        return nullableType;
                    }
                    {
                        var typeSyntax = (TypeSyntax)children.First();
                        var nullableType = SyntaxFactory.NullableType(typeSyntax);
                        return nullableType;
                    }
                }
            case SyntaxKind.QueryExpression:
                {
                    var fromClauseSyntax = (FromClauseSyntax)children.First();
                    var queryBody = (QueryBodySyntax)children[1];
                    var queryExpression = SyntaxFactory.QueryExpression(fromClauseSyntax, queryBody);
                    return queryExpression;
                }
            case SyntaxKind.PreIncrementExpression:
            case SyntaxKind.PreDecrementExpression:
            case SyntaxKind.LogicalNotExpression:
            case SyntaxKind.UnaryMinusExpression:
            case SyntaxKind.UnaryPlusExpression:
                {
                    ExpressionSyntax expression = (ExpressionSyntax)children[0];
                    var unary = SyntaxFactory.PrefixUnaryExpression(kind, expression);
                    return unary;
                }
            case SyntaxKind.PostIncrementExpression:
            case SyntaxKind.PostDecrementExpression:
                {
                    ExpressionSyntax expression = (ExpressionSyntax)children[0];
                    var unary = SyntaxFactory.PostfixUnaryExpression(kind, expression);
                    return unary;
                }
            case SyntaxKind.YieldReturnStatement:
                {
                    var expression = (ExpressionSyntax)children[0];
                    var yieldReturn = SyntaxFactory.YieldStatement(kind, expression);
                    return yieldReturn;
                }
            case SyntaxKind.ReturnStatement:
                {
                    ExpressionSyntax expression = (ExpressionSyntax)children[0];
                    var returnStatement = SyntaxFactory.ReturnStatement(expression);
                    return returnStatement;
                }
            case SyntaxKind.ElseClause:
                {
                    var statatementSyntax = (StatementSyntax)children[0];
                    var elseClause = SyntaxFactory.ElseClause(statatementSyntax);
                    return elseClause;
                }
            case SyntaxKind.IdentifierName:
                {
                    if (identifiers != null && identifiers.Any())
                    {
                        SyntaxToken stoken = (SyntaxToken)identifiers.First();
                        var identifierName = SyntaxFactory.IdentifierName(stoken);
                        return identifierName;
                    }
                    var identifier = children.First();
                    return identifier;
                }
            case SyntaxKind.TypeArgumentList:
                {
                    var listType = children.Select(child => (TypeSyntax)child).ToList();

                    var typespal = SyntaxFactory.SeparatedList(listType);
                    var typeArgument = SyntaxFactory.TypeArgumentList(typespal);
                    return typeArgument;
                }
            case SyntaxKind.GenericName:
                {
                    var gName = (GenericNameSyntax)node;
                    var typeArg = (TypeArgumentListSyntax)children[0];
                    var genericName = SyntaxFactory.GenericName(gName.Identifier, typeArg);
                    return genericName;
                }
            case SyntaxKind.IsExpression:
            case SyntaxKind.NotEqualsExpression:
                {
                    var leftExpression = (ExpressionSyntax)children[0];
                    var rightExpression = (ExpressionSyntax)children[1];
                    var notEqualsExpression = SyntaxFactory.BinaryExpression(kind, leftExpression, rightExpression);
                    return notEqualsExpression;
                }
            case SyntaxKind.SetAccessorDeclaration:
            case SyntaxKind.GetAccessorDeclaration:
                {
                    var blockSyntax = (BlockSyntax)children[0];
                    var getAcessor = SyntaxFactory.AccessorDeclaration(kind, blockSyntax);
                    return getAcessor;
                }
            case SyntaxKind.AccessorList:
                {
                    var list = children.Select(v => (AccessorDeclarationSyntax)v).ToList();
                    var syntaxList = new SyntaxList<AccessorDeclarationSyntax>();
                    syntaxList.AddRange(list);
                    var acessorList = SyntaxFactory.AccessorList();
                    acessorList = acessorList.AddAccessors(list.ToArray());
                    return acessorList;
                }
            case SyntaxKind.MemberBindingExpression:
                {
                    var syntaxName = (SimpleNameSyntax) children.First();
                    var memberBinding = SyntaxFactory.MemberBindingExpression(syntaxName);
                    return memberBinding;
                }
                /*case SyntaxKind.None:
                    break;
                case SyntaxKind.List:
                    break;
                case SyntaxKind.TildeToken:
                    break;
                case SyntaxKind.ExclamationToken:
                    break;
                case SyntaxKind.DollarToken:
                    break;
                case SyntaxKind.PercentToken:
                    break;
                case SyntaxKind.CaretToken:
                    break;
                case SyntaxKind.AmpersandToken:
                    break;
                case SyntaxKind.AsteriskToken:
                    break;
                case SyntaxKind.OpenParenToken:
                    break;
                case SyntaxKind.CloseParenToken:
                    break;
                case SyntaxKind.MinusToken:
                    break;
                case SyntaxKind.PlusToken:
                    break;
                case SyntaxKind.EqualsToken:
                    break;
                case SyntaxKind.OpenBraceToken:
                    break;
                case SyntaxKind.CloseBraceToken:
                    break;
                case SyntaxKind.OpenBracketToken:
                    break;
                case SyntaxKind.CloseBracketToken:
                    break;
                case SyntaxKind.BarToken:
                    break;
                case SyntaxKind.BackslashToken:
                    break;
                case SyntaxKind.ColonToken:
                    break;
                case SyntaxKind.SemicolonToken:
                    break;
                case SyntaxKind.DoubleQuoteToken:
                    break;
                case SyntaxKind.SingleQuoteToken:
                    break;
                case SyntaxKind.LessThanToken:
                    break;
                case SyntaxKind.CommaToken:
                    break;
                case SyntaxKind.GreaterThanToken:
                    break;
                case SyntaxKind.DotToken:
                    break;
                case SyntaxKind.QuestionToken:
                    break;
                case SyntaxKind.HashToken:
                    break;
                case SyntaxKind.SlashToken:
                    break;
                case SyntaxKind.SlashGreaterThanToken:
                    break;
                case SyntaxKind.LessThanSlashToken:
                    break;
                case SyntaxKind.XmlCommentStartToken:
                    break;
                case SyntaxKind.XmlCommentEndToken:
                    break;
                case SyntaxKind.XmlCDataStartToken:
                    break;
                case SyntaxKind.XmlCDataEndToken:
                    break;
                case SyntaxKind.XmlProcessingInstructionStartToken:
                    break;
                case SyntaxKind.XmlProcessingInstructionEndToken:
                    break;
                case SyntaxKind.BarBarToken:
                    break;
                case SyntaxKind.AmpersandAmpersandToken:
                    break;
                case SyntaxKind.MinusMinusToken:
                    break;
                case SyntaxKind.PlusPlusToken:
                    break;
                case SyntaxKind.ColonColonToken:
                    break;
                case SyntaxKind.QuestionQuestionToken:
                    break;
                case SyntaxKind.MinusGreaterThanToken:
                    break;
                case SyntaxKind.ExclamationEqualsToken:
                    break;
                case SyntaxKind.EqualsEqualsToken:
                    break;
                case SyntaxKind.EqualsGreaterThanToken:
                    break;
                case SyntaxKind.LessThanEqualsToken:
                    break;
                case SyntaxKind.LessThanLessThanToken:
                    break;
                case SyntaxKind.LessThanLessThanEqualsToken:
                    break;
                case SyntaxKind.GreaterThanEqualsToken:
                    break;
                case SyntaxKind.GreaterThanGreaterThanToken:
                    break;
                case SyntaxKind.GreaterThanGreaterThanEqualsToken:
                    break;
                case SyntaxKind.SlashEqualsToken:
                    break;
                case SyntaxKind.AsteriskEqualsToken:
                    break;
                case SyntaxKind.BarEqualsToken:
                    break;
                case SyntaxKind.AmpersandEqualsToken:
                    break;
                case SyntaxKind.PlusEqualsToken:
                    break;
                case SyntaxKind.MinusEqualsToken:
                    break;
                case SyntaxKind.CaretEqualsToken:
                    break;
                case SyntaxKind.PercentEqualsToken:
                    break;
                case SyntaxKind.BoolKeyword:
                    break;
                case SyntaxKind.ByteKeyword:
                    break;
                case SyntaxKind.SByteKeyword:
                    break;
                case SyntaxKind.ShortKeyword:
                    break;
                case SyntaxKind.UShortKeyword:
                    break;
                case SyntaxKind.IntKeyword:
                    break;
                case SyntaxKind.UIntKeyword:
                    break;
                case SyntaxKind.LongKeyword:
                    break;
                case SyntaxKind.ULongKeyword:
                    break;
                case SyntaxKind.DoubleKeyword:
                    break;
                case SyntaxKind.FloatKeyword:
                    break;
                case SyntaxKind.DecimalKeyword:
                    break;
                case SyntaxKind.StringKeyword:
                    break;
                case SyntaxKind.CharKeyword:
                    break;
                case SyntaxKind.VoidKeyword:
                    break;
                case SyntaxKind.ObjectKeyword:
                    break;
                case SyntaxKind.TypeOfKeyword:
                    break;
                case SyntaxKind.SizeOfKeyword:
                    break;
                case SyntaxKind.NullKeyword:
                    break;
                case SyntaxKind.TrueKeyword:
                    break;
                case SyntaxKind.FalseKeyword:
                    break;
                case SyntaxKind.IfKeyword:
                    break;
                case SyntaxKind.ElseKeyword:
                    break;
                case SyntaxKind.WhileKeyword:
                    break;
                case SyntaxKind.ForKeyword:
                    break;
                case SyntaxKind.ForEachKeyword:
                    break;
                case SyntaxKind.DoKeyword:
                    break;
                case SyntaxKind.SwitchKeyword:
                    break;
                case SyntaxKind.CaseKeyword:
                    break;
                case SyntaxKind.DefaultKeyword:
                    break;
                case SyntaxKind.TryKeyword:
                    break;
                case SyntaxKind.CatchKeyword:
                    break;
                case SyntaxKind.FinallyKeyword:
                    break;
                case SyntaxKind.LockKeyword:
                    break;
                case SyntaxKind.GotoKeyword:
                    break;
                case SyntaxKind.BreakKeyword:
                    break;
                case SyntaxKind.ContinueKeyword:
                    break;
                case SyntaxKind.ReturnKeyword:
                    break;
                case SyntaxKind.ThrowKeyword:
                    break;
                case SyntaxKind.PublicKeyword:
                    break;
                case SyntaxKind.PrivateKeyword:
                    break;
                case SyntaxKind.InternalKeyword:
                    break;
                case SyntaxKind.ProtectedKeyword:
                    break;
                case SyntaxKind.StaticKeyword:
                    break;
                case SyntaxKind.ReadOnlyKeyword:
                    break;
                case SyntaxKind.SealedKeyword:
                    break;
                case SyntaxKind.ConstKeyword:
                    break;
                case SyntaxKind.FixedKeyword:
                    break;
                case SyntaxKind.StackAllocKeyword:
                    break;
                case SyntaxKind.VolatileKeyword:
                    break;
                case SyntaxKind.NewKeyword:
                    break;
                case SyntaxKind.OverrideKeyword:
                    break;
                case SyntaxKind.AbstractKeyword:
                    break;
                case SyntaxKind.VirtualKeyword:
                    break;
                case SyntaxKind.EventKeyword:
                    break;
                case SyntaxKind.ExternKeyword:
                    break;
                case SyntaxKind.RefKeyword:
                    break;
                case SyntaxKind.OutKeyword:
                    break;
                case SyntaxKind.InKeyword:
                    break;
                case SyntaxKind.IsKeyword:
                    break;
                case SyntaxKind.AsKeyword:
                    break;
                case SyntaxKind.ParamsKeyword:
                    break;
                case SyntaxKind.ArgListKeyword:
                    break;
                case SyntaxKind.MakeRefKeyword:
                    break;
                case SyntaxKind.RefTypeKeyword:
                    break;
                case SyntaxKind.RefValueKeyword:
                    break;
                case SyntaxKind.ThisKeyword:
                    break;
                case SyntaxKind.BaseKeyword:
                    break;
                case SyntaxKind.NamespaceKeyword:
                    break;
                case SyntaxKind.UsingKeyword:
                    break;
                case SyntaxKind.ClassKeyword:
                    break;
                case SyntaxKind.StructKeyword:
                    break;
                case SyntaxKind.InterfaceKeyword:
                    break;
                case SyntaxKind.EnumKeyword:
                    break;
                case SyntaxKind.DelegateKeyword:
                    break;
                case SyntaxKind.CheckedKeyword:
                    break;
                case SyntaxKind.UncheckedKeyword:
                    break;
                case SyntaxKind.UnsafeKeyword:
                    break;
                case SyntaxKind.OperatorKeyword:
                    break;
                case SyntaxKind.ExplicitKeyword:
                    break;
                case SyntaxKind.ImplicitKeyword:
                    break;
                case SyntaxKind.YieldKeyword:
                    break;
                case SyntaxKind.PartialKeyword:
                    break;
                case SyntaxKind.AliasKeyword:
                    break;
                case SyntaxKind.GlobalKeyword:
                    break;
                case SyntaxKind.AssemblyKeyword:
                    break;
                case SyntaxKind.ModuleKeyword:
                    break;
                case SyntaxKind.TypeKeyword:
                    break;
                case SyntaxKind.FieldKeyword:
                    break;
                case SyntaxKind.MethodKeyword:
                    break;
                case SyntaxKind.ParamKeyword:
                    break;
                case SyntaxKind.PropertyKeyword:
                    break;
                case SyntaxKind.TypeVarKeyword:
                    break;
                case SyntaxKind.GetKeyword:
                    break;
                case SyntaxKind.SetKeyword:
                    break;
                case SyntaxKind.AddKeyword:
                    break;
                case SyntaxKind.RemoveKeyword:
                    break;
                case SyntaxKind.WhereKeyword:
                    break;
                case SyntaxKind.FromKeyword:
                    break;
                case SyntaxKind.GroupKeyword:
                    break;
                case SyntaxKind.JoinKeyword:
                    break;
                case SyntaxKind.IntoKeyword:
                    break;
                case SyntaxKind.LetKeyword:
                    break;
                case SyntaxKind.ByKeyword:
                    break;
                case SyntaxKind.SelectKeyword:
                    break;
                case SyntaxKind.OrderByKeyword:
                    break;
                case SyntaxKind.OnKeyword:
                    break;
                case SyntaxKind.EqualsKeyword:
                    break;
                case SyntaxKind.AscendingKeyword:
                    break;
                case SyntaxKind.DescendingKeyword:
                    break;
                case SyntaxKind.NameOfKeyword:
                    break;
                case SyntaxKind.AsyncKeyword:
                    break;
                case SyntaxKind.AwaitKeyword:
                    break;
                case SyntaxKind.WhenKeyword:
                    break;
                case SyntaxKind.ElifKeyword:
                    break;
                case SyntaxKind.EndIfKeyword:
                    break;
                case SyntaxKind.RegionKeyword:
                    break;
                case SyntaxKind.EndRegionKeyword:
                    break;
                case SyntaxKind.DefineKeyword:
                    break;
                case SyntaxKind.UndefKeyword:
                    break;
                case SyntaxKind.WarningKeyword:
                    break;
                case SyntaxKind.ErrorKeyword:
                    break;
                case SyntaxKind.LineKeyword:
                    break;
                case SyntaxKind.PragmaKeyword:
                    break;
                case SyntaxKind.HiddenKeyword:
                    break;
                case SyntaxKind.ChecksumKeyword:
                    break;
                case SyntaxKind.DisableKeyword:
                    break;
                case SyntaxKind.RestoreKeyword:
                    break;
                case SyntaxKind.ReferenceKeyword:
                    break;
                case SyntaxKind.LoadKeyword:
                    break;
                case SyntaxKind.InterpolatedStringStartToken:
                    break;
                case SyntaxKind.InterpolatedStringEndToken:
                    break;
                case SyntaxKind.InterpolatedVerbatimStringStartToken:
                    break;
                case SyntaxKind.OmittedTypeArgumentToken:
                    break;
                case SyntaxKind.OmittedArraySizeExpressionToken:
                    break;
                case SyntaxKind.EndOfDirectiveToken:
                    break;
                case SyntaxKind.EndOfDocumentationCommentToken:
                    break;
                case SyntaxKind.EndOfFileToken:
                    break;
                case SyntaxKind.BadToken:
                    break;
                case SyntaxKind.IdentifierToken:
                    break;
                case SyntaxKind.NumericLiteralToken:
                    break;
                case SyntaxKind.CharacterLiteralToken:
                    break;
                case SyntaxKind.StringLiteralToken:
                    break;
                case SyntaxKind.XmlEntityLiteralToken:
                    break;
                case SyntaxKind.XmlTextLiteralToken:
                    break;
                case SyntaxKind.XmlTextLiteralNewLineToken:
                    break;
                case SyntaxKind.InterpolatedStringToken:
                    break;
                case SyntaxKind.InterpolatedStringTextToken:
                    break;
                case SyntaxKind.EndOfLineTrivia:
                    break;
                case SyntaxKind.WhitespaceTrivia:
                    break;
                case SyntaxKind.SingleLineCommentTrivia:
                    break;
                case SyntaxKind.MultiLineCommentTrivia:
                    break;
                case SyntaxKind.DocumentationCommentExteriorTrivia:
                    break;
                case SyntaxKind.SingleLineDocumentationCommentTrivia:
                    break;
                case SyntaxKind.MultiLineDocumentationCommentTrivia:
                    break;
                case SyntaxKind.DisabledTextTrivia:
                    break;
                case SyntaxKind.PreprocessingMessageTrivia:
                    break;
                case SyntaxKind.IfDirectiveTrivia:
                    break;
                case SyntaxKind.ElifDirectiveTrivia:
                    break;
                case SyntaxKind.ElseDirectiveTrivia:
                    break;
                case SyntaxKind.EndIfDirectiveTrivia:
                    break;
                case SyntaxKind.RegionDirectiveTrivia:
                    break;
                case SyntaxKind.EndRegionDirectiveTrivia:
                    break;
                case SyntaxKind.DefineDirectiveTrivia:
                    break;
                case SyntaxKind.UndefDirectiveTrivia:
                    break;
                case SyntaxKind.ErrorDirectiveTrivia:
                    break;
                case SyntaxKind.WarningDirectiveTrivia:
                    break;
                case SyntaxKind.LineDirectiveTrivia:
                    break;
                case SyntaxKind.PragmaWarningDirectiveTrivia:
                    break;
                case SyntaxKind.PragmaChecksumDirectiveTrivia:
                    break;
                case SyntaxKind.ReferenceDirectiveTrivia:
                    break;
                case SyntaxKind.BadDirectiveTrivia:
                    break;
                case SyntaxKind.SkippedTokensTrivia:
                    break;
                case SyntaxKind.XmlElement:
                    break;
                case SyntaxKind.XmlElementStartTag:
                    break;
                case SyntaxKind.XmlElementEndTag:
                    break;
                case SyntaxKind.XmlEmptyElement:
                    break;
                case SyntaxKind.XmlTextAttribute:
                    break;
                case SyntaxKind.XmlCrefAttribute:
                    break;
                case SyntaxKind.XmlNameAttribute:
                    break;
                case SyntaxKind.XmlName:
                    break;
                case SyntaxKind.XmlPrefix:
                    break;
                case SyntaxKind.XmlText:
                    break;
                case SyntaxKind.XmlCDataSection:
                    break;
                case SyntaxKind.XmlComment:
                    break;
                case SyntaxKind.XmlProcessingInstruction:
                    break;
                case SyntaxKind.TypeCref:
                    break;
                case SyntaxKind.QualifiedCref:
                    break;
                case SyntaxKind.NameMemberCref:
                    break;
                case SyntaxKind.IndexerMemberCref:
                    break;
                case SyntaxKind.OperatorMemberCref:
                    break;
                case SyntaxKind.ConversionOperatorMemberCref:
                    break;
                case SyntaxKind.CrefParameterList:
                    break;
                case SyntaxKind.CrefBracketedParameterList:
                    break;
                case SyntaxKind.CrefParameter:
                    break;
                case SyntaxKind.AliasQualifiedName:
                    break;
                case SyntaxKind.PredefinedType:
                    break;
                case SyntaxKind.PointerType:
                    break;
                case SyntaxKind.OmittedTypeArgument:
                    break;
                case SyntaxKind.AnonymousMethodExpression:
                    break;
                case SyntaxKind.StackAllocArrayCreationExpression:
                    break;
                case SyntaxKind.OmittedArraySizeExpression:
                    break;
                case SyntaxKind.InterpolatedStringExpression:
                    break;
                case SyntaxKind.ImplicitElementAccess:
                    break;
                case SyntaxKind.ModuloExpression:
                    break;
                case SyntaxKind.LeftShiftExpression:
                    break;
                case SyntaxKind.RightShiftExpression:
                    break;
                case SyntaxKind.ExclusiveOrExpression:
                    break;
                case SyntaxKind.PointerMemberAccessExpression:
                    break;
                case SyntaxKind.ConditionalAccessExpression:
                    break;*/
                /*
                case SyntaxKind.ElementBindingExpression:
                    break;
                case SyntaxKind.ModuloAssignmentExpression:
                    break;
                case SyntaxKind.AndAssignmentExpression:
                    break;
                case SyntaxKind.ExclusiveOrAssignmentExpression:
                    break;
                case SyntaxKind.OrAssignmentExpression:
                    break;
                case SyntaxKind.LeftShiftAssignmentExpression:
                    break;
                case SyntaxKind.RightShiftAssignmentExpression:
                    break;
                case SyntaxKind.BitwiseNotExpression:
                    break;
                case SyntaxKind.PointerIndirectionExpression:
                    break;
                case SyntaxKind.AddressOfExpression:
                    break;
                case SyntaxKind.ThisExpression:
                    break;
                case SyntaxKind.BaseExpression:
                    break;
                case SyntaxKind.ArgListExpression:
                    break;
                case SyntaxKind.NumericLiteralExpression:
                    break;
                case SyntaxKind.StringLiteralExpression:
                    break;
                case SyntaxKind.CharacterLiteralExpression:
                    break;
                case SyntaxKind.TrueLiteralExpression:
                    break;
                case SyntaxKind.FalseLiteralExpression:
                    break;
                case SyntaxKind.NullLiteralExpression:
                    break;
                case SyntaxKind.SizeOfExpression:
                    break;
                case SyntaxKind.CheckedExpression:
                    break;
                case SyntaxKind.UncheckedExpression:
                    break;
                case SyntaxKind.MakeRefExpression:
                    break;
                case SyntaxKind.RefValueExpression:
                    break;
                case SyntaxKind.RefTypeExpression:
                    break;
                case SyntaxKind.LetClause:
                    break;
                case SyntaxKind.JoinClause:
                    break;
                case SyntaxKind.JoinIntoClause:
                    break;
                case SyntaxKind.WhereClause:
                    break;
                case SyntaxKind.OrderByClause:
                    break;
                case SyntaxKind.AscendingOrdering:
                    break;
                case SyntaxKind.DescendingOrdering:
                    break;
                case SyntaxKind.GroupClause:
                    break;
                case SyntaxKind.QueryContinuation:
                    break;
                case SyntaxKind.EmptyStatement:
                    break;
                case SyntaxKind.LabeledStatement:
                    break;
                case SyntaxKind.GotoStatement:
                    break;
                case SyntaxKind.GotoCaseStatement:
                    break;
                case SyntaxKind.GotoDefaultStatement:
                    break;
                case SyntaxKind.BreakStatement:
                    break;
                case SyntaxKind.ContinueStatement:
                    break;
                case SyntaxKind.YieldBreakStatement:
                    break;
                case SyntaxKind.WhileStatement:
                    break;
                case SyntaxKind.DoStatement:
                    break;
                case SyntaxKind.ForStatement:
                    break;
                case SyntaxKind.FixedStatement:
                    break;
                case SyntaxKind.CheckedStatement:
                    break;
                case SyntaxKind.UncheckedStatement:
                    break;
                case SyntaxKind.UnsafeStatement:
                    break;
                case SyntaxKind.LockStatement:
                    break;
                case SyntaxKind.DefaultSwitchLabel:
                    break;
                case SyntaxKind.CatchFilterClause:
                    break;
                case SyntaxKind.CompilationUnit:
                    break;
                case SyntaxKind.GlobalStatement:
                    break;
                case SyntaxKind.NamespaceDeclaration:
                    break;
                case SyntaxKind.UsingDirective:
                    break;
                case SyntaxKind.ExternAliasDirective:
                    break;
                case SyntaxKind.AttributeTargetSpecifier:
                    break;
                case SyntaxKind.ClassDeclaration:
                    break;
                case SyntaxKind.StructDeclaration:
                    break;
                case SyntaxKind.InterfaceDeclaration:
                    break;
                case SyntaxKind.EnumDeclaration:
                    break;
                case SyntaxKind.DelegateDeclaration:
                    break;
                case SyntaxKind.BaseList:
                    break;
                case SyntaxKind.SimpleBaseType:
                    break;
                case SyntaxKind.TypeParameterConstraintClause:
                    break;
                case SyntaxKind.ConstructorConstraint:
                    break;
                case SyntaxKind.ClassConstraint:
                    break;
                case SyntaxKind.StructConstraint:
                    break;
                case SyntaxKind.TypeConstraint:
                    break;
                case SyntaxKind.ExplicitInterfaceSpecifier:
                    break;
                case SyntaxKind.EnumMemberDeclaration:
                    break;
                case SyntaxKind.FieldDeclaration:
                    break;
                case SyntaxKind.EventFieldDeclaration:
                    break;
                case SyntaxKind.OperatorDeclaration:
                    break;
                case SyntaxKind.ConversionOperatorDeclaration:
                    break;
                case SyntaxKind.ConstructorDeclaration:
                    break;
                case SyntaxKind.BaseConstructorInitializer:
                    break;
                case SyntaxKind.ThisConstructorInitializer:
                    break;
                case SyntaxKind.DestructorDeclaration:
                    break;
                case SyntaxKind.EventDeclaration:
                    break;
                case SyntaxKind.IndexerDeclaration:
                    break;
                case SyntaxKind.AddAccessorDeclaration:
                    break;
                case SyntaxKind.RemoveAccessorDeclaration:
                    break;
                case SyntaxKind.UnknownAccessorDeclaration:
                    break;
                case SyntaxKind.BracketedParameterList:
                    break;
                case SyntaxKind.TypeParameterList:
                    break;
                case SyntaxKind.TypeParameter:
                    break;
                case SyntaxKind.IncompleteMember:
                    break;
                case SyntaxKind.ArrowExpressionClause:
                    break;
                case SyntaxKind.Interpolation:
                    break;
                case SyntaxKind.InterpolatedStringText:
                    break;
                case SyntaxKind.InterpolationAlignmentClause:
                    break;
                case SyntaxKind.InterpolationFormatClause:
                    break;
                case SyntaxKind.ShebangDirectiveTrivia:
                    break;
                case SyntaxKind.LoadDirectiveTrivia:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);*/
            }
            throw new Exception($"Unsupported kind support: {kind}");
        }
    }
}
