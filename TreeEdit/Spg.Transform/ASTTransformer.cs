using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;
using RefazerObject.Transformation;
using TreeEdit.Spg.Match;
using TreeElement;

namespace TreeEdit.Spg.Transform
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ASTTransformer
    {
        public static List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>> Transform(List<TransformationInfo> transformations)
        {
            var groups = transformations.GroupBy(o => o.Before.SyntaxTree.FilePath);
            var dic = groups.ToDictionary(group => group.Key, group => group.ToList());
            var transformedDocuments = Transform(dic);
            return transformedDocuments;
        }

        private static List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>> Transform(Dictionary<string, List<TransformationInfo>> dic)
        {
            var transformationList = new List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>>();
            foreach (var item in dic)
            {
                var transformation = Transformation(item.Value);
                transformationList.Add(transformation.Result);
            }
            return transformationList;
        }

        /// <summary>
        /// Receives a list of modification in the source code in terms of 
        /// before and after edits in the source code and returns a new 
        /// version of the document where these modifications take place.
        /// </summary>
        /// <param name="transformationsList">List of modifications</param>
        [SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
        private static async Task<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>> Transformation(List<TransformationInfo> transformationsList)
        {
            var transformations = RemoveDuplicates(transformationsList);
            var workspace = new AdhocWorkspace();
            var projectId = ProjectId.CreateNewId();
            var versionStamp = VersionStamp.Create();
            var projectInfo = ProjectInfo.Create(projectId, versionStamp, "NewProject", "projName", LanguageNames.CSharp);
            var newProject = workspace.AddProject(projectInfo);
            var first = transformations.First().Before.SyntaxTree.FilePath;
            var sourceCode = FileUtil.ReadFile(first);
            var sourceAST = CSharpSyntaxTree.ParseText(sourceCode, path: transformations.First().Before.SyntaxTree.FilePath);
            var sourceText = SourceText.From(sourceCode);
            var document = workspace.AddDocument(newProject.Id, transformations.First().Before.SyntaxTree.FilePath, sourceText);
            var syntaxRoot = await document.GetSyntaxRootAsync();
            var nodes = new List<Tuple<SyntaxNode, SyntaxNode>>();

            var documentEditor = await DocumentEditor.CreateAsync(document);
            foreach (var node in transformations)
            {
                var nodeInEditor = MatchManager.FindNode(syntaxRoot, node.Before);
                nodes.Add(Tuple.Create(nodeInEditor.AsNode(), node.After.AsNode()));
            }
            foreach (var node in nodes)
            {
                documentEditor.ReplaceNode(node.Item1, node.Item2);
            }
            var beforeAST = (SyntaxNodeOrToken) sourceAST.GetRoot();
            var afterAST = (SyntaxNodeOrToken) documentEditor.GetChangedRoot();
            afterAST = Formatter.Format(afterAST.AsNode(), new AdhocWorkspace());
            var result = Tuple.Create(beforeAST, afterAST);
            return result;
        }

        public static List<TransformationInfo> RemoveDuplicates(List<TransformationInfo> transformations)
        {
            var nonDuplicates = transformations.Where(o => !transformations.Any(e => o.Before.Span.IntersectsWith(e.Before.Span) && e != o)).ToList();
            var duplicates = transformations.Where(o => transformations.Any(e => o.Before.Span.IntersectsWith(e.Before.Span) && e != o)).ToList();

            var hash = new HashSet<SyntaxNodeOrToken>();
            foreach (var transformation in  duplicates)
            {
                if (!hash.Contains(transformation.Before))
                {
                    var others = duplicates.Where(o => transformation.Before.Span.IntersectsWith(o.Before.Span) && transformation != o);
                    var ordered = others.OrderBy(o => o.Before.SpanStart).ThenByDescending(o => o.Before.Span.Length);
                    nonDuplicates.Add(ordered.First());
                    foreach (var item in ordered)
                    {
                        hash.Add(item.Before);
                    }
               }          
            }
            return nonDuplicates;
        }
    }
}
