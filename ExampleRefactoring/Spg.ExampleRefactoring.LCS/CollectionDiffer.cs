﻿using System;
using System.Collections.Generic;

namespace LCS2
{
    /// <summary>
    /// Find the difference between two list using longest common sequences
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListDiffer<T>
    {
        /// <summary>
        /// Find the difference between two arrays
        /// </summary>
        /// <param name="baseline"></param>
        /// <param name="revision"></param>
        /// <returns></returns>
        public virtual List<ComparisonResult<T>> FindDifference(
            List<T> baseline,
            List<T> revision)
        {
            int[,] differenceMatrix =
                GetLCSDifferenceMatrix<T>(baseline, revision);

            return FindDifference(
                differenceMatrix,
                baseline,
                revision,
                baseline.Count,
                revision.Count);
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
        private static List<ComparisonResult<T>> FindDifference(
            int[,] matrix,
            List<T> baseline,
            List<T> revision,
            int baselineIndex,
            int revisionIndex)
        {
            List<ComparisonResult<T>> results = new List<ComparisonResult<T>>();

            if (baselineIndex > 0 && revisionIndex > 0 &&
                baseline[baselineIndex - 1].Equals(revision[revisionIndex - 1]))
            {
                results.AddRange(
                    FindDifference(matrix, baseline, revision, baselineIndex - 1, revisionIndex - 1));

                results.Add(new ComparisonResult<T>
                {
                    DataCompared = baseline[baselineIndex - 1],
                    ModificationType = ModificationType.None
                });
            }
            else if (revisionIndex > 0 && (baselineIndex == 0 ||
                matrix[baselineIndex, revisionIndex - 1] >= matrix[baselineIndex - 1, revisionIndex]))
            {
                results.AddRange(
                    FindDifference(matrix, baseline, revision, baselineIndex, revisionIndex - 1));

                results.Add(new ComparisonResult<T>
                {
                    DataCompared = revision[revisionIndex - 1],
                    ModificationType = ModificationType.Inserted
                });
            }
            else if (baselineIndex > 0 && (revisionIndex == 0 ||
                matrix[baselineIndex, revisionIndex - 1] < matrix[baselineIndex - 1, revisionIndex]))
            {
                results.AddRange(
                    FindDifference(matrix, baseline, revision, baselineIndex - 1, revisionIndex));

                results.Add(new ComparisonResult<T>
                {
                    DataCompared = baseline[baselineIndex - 1],
                    ModificationType = ModificationType.Deleted
                });
            }

            return results;
        }

        /// <summary>
        /// Get Longest common difference matrix
        /// </summary>
        /// <typeparam name="T">Object in the list</typeparam>
        /// <param name="baseline">Baseline</param>
        /// <param name="revision">Revision</param>
        /// <returns>Longest common difference matrix</returns>
        private static int[,] GetLCSDifferenceMatrix<T>(
            List<T> baseline,
            List<T> revision)
        {
            int[,] matrix = new int[baseline.Count + 1, revision.Count + 1];

            for (int baselineIndex = 0; baselineIndex < baseline.Count; baselineIndex++)
            {
                for (int revisionIndex = 0; revisionIndex < revision.Count; revisionIndex++)
                {
                    if (baseline[baselineIndex].Equals(revision[revisionIndex]))
                    {
                        matrix[baselineIndex + 1, revisionIndex + 1] =
                            matrix[baselineIndex, revisionIndex] + 1;
                    }
                    else
                    {
                        int possibilityOne = matrix[baselineIndex + 1, revisionIndex];
                        int possibilityTwo = matrix[baselineIndex, revisionIndex + 1];

                        matrix[baselineIndex + 1, revisionIndex + 1] =
                            Math.Max(possibilityOne, possibilityTwo);
                    }
                }
            }

            return matrix;
        }
    }
}
