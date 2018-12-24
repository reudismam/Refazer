using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using RefazerManager;
using TreeElement;

namespace Clustering
{
    public class ClusteringAlgorithm
    {
        public static List<TransformationCluster> Clusters(List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>> submissions)
        {
            List<TransformationCluster> clusters = new List<TransformationCluster>();
            foreach (var tuple in submissions)
            {
                TransformationCluster cluster = new TransformationCluster();
                cluster.Examples.Add(tuple);
                var program = Refazer4CSharp.LearnTransformation(cluster.Examples);
                cluster.Program = program;
                clusters.Add(cluster);
            }
            foreach (var cluster in clusters)
            {
                foreach (var tuplej in submissions)
                {
                    if (!cluster.Examples.Contains(tuplej))
                    {
                        var examples = new List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>>(cluster.Examples);
                        examples.Add(tuplej);
                        var program = Refazer4CSharp.LearnTransformation(examples);
                        if (program != null)
                        {
                            cluster.Examples.Add(tuplej);
                            cluster.Program = program;
                        }
                    }
                }
            }
            return clusters;
        }

        public static List<TransformationCluster> Clusters(List<Tuple<string, string>> submissions)
        {
            var examples = GetExamples(submissions);
            return Clusters(examples);
        }

        public static List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>> GetExamples(List<Tuple<string, string>> examples)
        {
            var sotExamples = new List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>>();
            for (int i = 0; i < examples.Count; i++)
            {
                var example = examples[i];
                var inputText = FileUtil.ReadFile(example.Item1);
                var outputText = FileUtil.ReadFile(example.Item2);
                var inpTree = (SyntaxNodeOrToken)CSharpSyntaxTree.ParseText(inputText, path: example.Item1).GetRoot();
                var outTree = (SyntaxNodeOrToken)CSharpSyntaxTree.ParseText(outputText, path: example.Item2).GetRoot();
                var sotExample = new Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>(inpTree, outTree);
                sotExamples.Add(sotExample);
            }
            return sotExamples;
        }
    }
}
