using System;
using System.Collections.Generic;
using System.Linq;

namespace LongestCommonAncestor
{
    /// <summary>
    /// Find the difference between two list using longest common sequences
    /// </summary>
    public class LongestCommonSubsequenceManager<T>
    {
        /// <summary>
        /// Find the difference between two arrays
        /// </summary>
        /// <param name="baseline">Baseline</param>
        /// <param name="revision">Baseline</param>
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
        /// <param name="baselines">List to be compared</param>
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
            return p.Equals(q);
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