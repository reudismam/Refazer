using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Extraction.Text.Semantics;

namespace ProseSample.Substrings
{
    public static class RankingScore
    {
        public const double VariableScore = 0;

        [FeatureCalculator("NodesMap")]
        public static double Score_NodesMap( double scriptScore, double editScore) =>  scriptScore + editScore;

        [FeatureCalculator("SplitNodes")]
        public static double Score_SplitNodes(double inScore) => inScore;

        [FeatureCalculator("Script1")]
        public static double Score_Script1(double inScore, double edit) => inScore + edit;

        [FeatureCalculator("Insert")]
        public static double Score_Insert(double inScore, double kScore, double expressionScore, double astScore) => inScore + kScore + expressionScore + astScore;

        [FeatureCalculator("Node1")]
        public static double Score_Node1(double kScore, double astScore) => kScore +  astScore;

        [FeatureCalculator("Node2")]
        public static double Score_Node2(double kScore, double astScore) => kScore + astScore;

        [FeatureCalculator("Const")]
        public static double Score_Node1(double astScore) => astScore;

        [FeatureCalculator("Literal")]
        public static double Score_Literal(double inScore, double treeScore) => treeScore;

        [FeatureCalculator("C1")]
        public static double Score_C1(double inScore, double kindScore, double expression1Score) => kindScore + expression1Score;

        [FeatureCalculator("C2")]
        public static double Score_C2(double inScore, double kindScore, double expression1Score, double expression2Score) => kindScore + expression1Score + expression2Score;

        [FeatureCalculator("Identifier")]
        public static double Score_Identifier(double inScore) => inScore;

        [FeatureCalculator("PredefinedType")]
        public static double Score_PredefinedType(double inScore) => inScore;

        [FeatureCalculator("NumericLiteralExpression")]
        public static double Score_NumericLiteralExpression(double inScore) => inScore;

        [FeatureCalculator("StringLiteralExpression")]
        public static double Score_StringLiteralExpression(double inScore) => inScore;

        [FeatureCalculator("Block")]
        public static double Block(double inScore) => inScore;

        [FeatureCalculator("SubStr")]
        public static double Score_SubStr(double x, double pp) => Math.Log(pp);

        [FeatureCalculator("PosPair")]
        public static double Score_PosPair(double pp1, double pp2) => pp1 * pp2;

        [FeatureCalculator("AbsPos")]
        public static double Score_AbsPos(double x, double k)
        {
            k = 1 / k - 1;
            // Prefer absolute positions to regex positions if k is small
            return Math.Max(10 * Token.MinScore - (k - 1) * 3 * Token.MinScore, 1 / k);
        }

        [FeatureCalculator(Method = CalculationMethod.FromLiteral)]
        public static double KScore(int k) => k >= 0 ? 1.0 / (k + 1.0) : 1.0 / (-k + 1.1);

        [FeatureCalculator(Method = CalculationMethod.FromLiteral)]
        public static double KDScore(string kd) => 1.1;

        [FeatureCalculator("BoundaryPair")]
        public static double Score_BoundaryPair(double r1, double r2) => r1 + r2;

        [FeatureCalculator("RegPos")]
        public static double Score_RegPos(double x, double rr, double k) => rr * k;

        [FeatureCalculator(Method = CalculationMethod.FromLiteral)]
        public static double RegexScore(RegularExpression r) => r.Score;
    }
}
