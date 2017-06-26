using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Text;
using TreeEdit.Spg.Match;
using TreeElement;

namespace TreeEdit.Spg.Transform
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ASTTransformer
    {
        public static List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>> Transform(List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>> transformations)
        {
            var groups = transformations.GroupBy(o => o.Item1.SyntaxTree.FilePath);
            var dic = groups.ToDictionary(group => group.Key, group => group.ToList());
            var transformedDocuments = Transform(dic);
            return transformedDocuments;
        }

        private static List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>> Transform(Dictionary<string, List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>>> dic)
        {
            var transformationList = new List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>>();
            foreach (var item in dic)
            {
                var transformation = Transformation(item.Value);
                transformationList.Add(transformation.Result);
            }
            return transformationList;
        }

        private static async Task<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>> Transformation(List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>> transformations)
        { 
            var workspace = new AdhocWorkspace();
            var projectId = ProjectId.CreateNewId();
            var versionStamp = VersionStamp.Create();
            var projectInfo = ProjectInfo.Create(projectId, versionStamp, "NewProject", "projName", LanguageNames.CSharp);
            var newProject = workspace.AddProject(projectInfo);
            var sourceCode = FileUtil.ReadFile(transformations.First().Item1.SyntaxTree.FilePath);
            var sourceAST = CSharpSyntaxTree.ParseText(sourceCode, path: transformations.First().Item1.SyntaxTree.FilePath);
            var sourceText = SourceText.From(sourceCode);
            var document = workspace.AddDocument(newProject.Id, transformations.First().Item1.SyntaxTree.FilePath, sourceText);
            var syntaxRoot = await document.GetSyntaxRootAsync();
            var nodes = new List<Tuple<SyntaxNode, SyntaxNode>>();

            //Finally... create the document editor
            var documentEditor = await DocumentEditor.CreateAsync(document);
            foreach (var node in transformations)
            {
                var nodeInEditor = MatchManager.FindNode(syntaxRoot, node.Item1);
                nodes.Add(Tuple.Create(nodeInEditor.AsNode(), node.Item2.AsNode()));
            }
            foreach (var node in nodes)
            {
                documentEditor.ReplaceNode(node.Item1, node.Item2);
            }
            var result = Tuple.Create((SyntaxNodeOrToken) sourceAST.GetRoot(), (SyntaxNodeOrToken) documentEditor.GetChangedRoot());
            return result;
        }
    }
}
