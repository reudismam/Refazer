using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TreeEdit.Spg.Script;
using ProseSample.Substrings;
using TreeElement.Spg.Node;

namespace LongestCommonSubsequence
{
    /// <summary>
    /// Find the difference between two list using longest common sequences
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LongestCommonSubsequenceManager<T>
    {
        /// <summary>
        /// Find the difference between two arrays
        /// </summary>
        /// <param name="baseline">Baseline</param>
        /// <param name="revision">Baseline</param>
        /// <returns></returns>
        public virtual List<ComparisonResult<T>> FindDifference(List<T> baseline, List<T> revision)
        {
            int[,] differenceMatrix = Matrix(baseline, revision);

            return FindDifference(differenceMatrix, baseline, revision, baseline.Count, revision.Count);
        }

        /// <summary>
        /// Find the difference between two arrays
        /// </summary>
        /// <param name="baseline">Baseline</param>
        /// <param name="revision">Baseline</param>
        /// <returns></returns>
        public virtual List<T> FindCommon(List<T> baseline, List<T> revision)
        {
            int[,] differenceMatrix = Matrix(baseline, revision);

            var diffs = FindDifference(differenceMatrix, baseline, revision, baseline.Count, revision.Count);

            var lcs = new List<T>();

            foreach (var diff in diffs)
            {
                if (diff.EditionType.Equals(EditionType.None))
                {
                    lcs.Add(diff.DataCompared);
                }
            }

            return lcs;
        }

        /// <summary>
        /// Find the difference between two arrays
        /// </summary>
        /// <param name="baseline">Baseline</param>
        /// <param name="revision">Baseline</param>
        /// <returns></returns>
        public virtual List<T> FindDifference(List<List<T>> baselines)
        {
            if (baselines.Count < 2) throw new ArgumentException("baselines must contains at least two elements.");

            var baseline = baselines.First();
            foreach (var revision in baselines)
            {
                int[,] differenceMatrix = Matrix(baseline, revision);

                var diffs = FindDifference(differenceMatrix, baseline, revision, baseline.Count, revision.Count);

                var lcs = new List<T>();

                foreach (var diff in diffs)
                {
                    if (diff.EditionType.Equals(EditionType.None))
                    {
                        lcs.Add(diff.DataCompared);
                    }
                }

                baseline = lcs;
            }
            return baseline;
        }


        /// <summary>
        /// Find difference list
        /// </summary>
        /// <param name="matrix">Matrix</param>
        /// <param name="baseline">Baseline</param>
        /// <param name="revision">Revision</param>
        /// <param name="baselineIndex">Baseline index</param>
        /// <param name="revisionIndex">Revision Index</param>
        /// <returns></returns>
        private static List<ComparisonResult<T>> FindDifference(int[,] matrix, List<T> baseline, List<T> revision, int baselineIndex, int revisionIndex)
        {
            List<ComparisonResult<T>> results = new List<ComparisonResult<T>>();

            if (baselineIndex > 0 && revisionIndex > 0 && IsPartialyEquals(baseline[baselineIndex - 1], revision[revisionIndex - 1]))
            {
                results.AddRange(FindDifference(matrix, baseline, revision, baselineIndex - 1, revisionIndex - 1));

                results.Add(new ComparisonResult<T>
                {
                    DataCompared = baseline[baselineIndex - 1],
                    EditionType = EditionType.None
                });
            }
            else if (revisionIndex > 0 && (baselineIndex == 0 || matrix[baselineIndex, revisionIndex - 1] >= matrix[baselineIndex - 1, revisionIndex]))
            {
                results.AddRange(FindDifference(matrix, baseline, revision, baselineIndex, revisionIndex - 1));

                results.Add(new ComparisonResult<T>
                {
                    DataCompared = revision[revisionIndex - 1],
                    EditionType = EditionType.Inserted
                });
            }
            else if (baselineIndex > 0 && (revisionIndex == 0 || matrix[baselineIndex, revisionIndex - 1] < matrix[baselineIndex - 1, revisionIndex]))
            {
                results.AddRange(FindDifference(matrix, baseline, revision, baselineIndex - 1, revisionIndex));

                results.Add(new ComparisonResult<T>
                {
                    DataCompared = baseline[baselineIndex - 1],
                    EditionType = EditionType.Deleted
                });
            }

            return results;
        }

        private static bool IsPartialyEquals(T p, T q)
        {
            //var edita = (EditOperation<SyntaxNodeOrToken>)(object)p;
            //var editb = (EditOperation<SyntaxNodeOrToken>)(object)q;

            //var t1A = edita.T1Node;
            //var t1B = editb.T1Node;
            //var pa = edita.Parent;
            //var pb = editb.Parent;

            //if (p.GetType() == q.GetType() && CalcSimilarity(t1A, t1B) > 0.5 && CalcSimilarity(pa, pb) > 0.5 && edita.K == editb.K) return true;


            return p.Equals(q);
        }

        private static double CalcSimilarity(TreeNode<SyntaxNodeOrToken> a, TreeNode<SyntaxNodeOrToken> b)
        {
            if (a.Value.AsNode().DescendantNodesAndSelf().Count() < 20 &&
                b.Value.AsNode().DescendantNodesAndSelf().Count() < 20)
            {
                var t1aitree = ConverterHelper.ConvertCSharpToTreeNode(a.Value);
                var t1bitree = ConverterHelper.ConvertCSharpToTreeNode(b.Value);

                var t1atoken = ConverterHelper.ConvertITreeNodeToToken(t1aitree);
                var t1btoken = ConverterHelper.ConvertITreeNodeToToken(t1bitree);
                var pattern = ProseSample.Substrings.Spg.Witness.Match.BuildPattern(t1atoken, t1btoken, false);

                var common = pattern.Tree.DescendantNodesAndSelf().Where(o => !o.IsLabel(new TLabel(SyntaxKind.EmptyStatement)));
                var den = (double) a.Value.AsNode().DescendantNodesAndSelf().Count() +
                          b.Value.AsNode().DescendantNodesAndSelf().Count();
                var mult = (2.0*common.Count());

                var result = mult/den;

                return result;
            }
            return 0.0;
        }

        /// <summary>
        /// Longest common ancestor of two list
        /// </summary>
        /// <typeparam name="T">Object in the list</typeparam>
        /// <param name="baseline">Baseline</param>
        /// <param name="revision">Revision</param>
        /// <returns>Longest common difference matrix</returns>
        private static int[,] Matrix(List<T> baseline, List<T> revision)
        {
            int[,] matrix = new int[baseline.Count + 1, revision.Count + 1];

            for (int baselineIndex = 0; baselineIndex < baseline.Count; baselineIndex++)
            {
                for (int revisionIndex = 0; revisionIndex < revision.Count; revisionIndex++)
                {
                    if (baseline[baselineIndex].Equals(revision[revisionIndex]))
                    {
                        matrix[baselineIndex + 1, revisionIndex + 1] = matrix[baselineIndex, revisionIndex] + 1;
                    }
                    else
                    {
                        int possibilityOne = matrix[baselineIndex + 1, revisionIndex];
                        int possibilityTwo = matrix[baselineIndex, revisionIndex + 1];
                        matrix[baselineIndex + 1, revisionIndex + 1] = Math.Max(possibilityOne, possibilityTwo);
                    }
                }
            }

            return matrix;
        }
    }
}