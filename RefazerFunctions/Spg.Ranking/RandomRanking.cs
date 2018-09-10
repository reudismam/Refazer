using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using System;
using System.Text.RegularExpressions;

namespace RefazerFunctions.Spg.Ranking
{
    public class RandomRanking : RankingFunction
    {
        public static int seed = 86028157;
        public const double VariableScore = 0;
        public static Random rnd = new Random(seed);


        public static double GetRandom(double min, double max)
        {
            return rnd.NextDouble() * (max - min) + min;
        }
        public double temp = GetRandom(-10, 10);

        // Editing EditMap
        [FeatureCalculator("EditMap")]
        public double Score_EditMap(double scriptScore, double editScore) => temp + GetRandom(-10, 10) + scriptScore + editScore;

        [FeatureCalculator(nameof(Semantics.AllNodes))]
        public double Score_Traversal(double scriptScore, double editScore) => temp + scriptScore + editScore;

        [FeatureCalculator("EditFilter")]
        public double Score_EditFilter(double predScore, double splitScore) => temp + (predScore + splitScore);

        [FeatureCalculator(nameof(Semantics.Match))]
        public double Score_Match(double inSource, double matchScore) => temp + matchScore;

        [FeatureCalculator(nameof(Semantics.SC))]
        public double Score_CS(double childScore) => temp + childScore;

        [FeatureCalculator(nameof(Semantics.CList))]
        public double Score_CList(double childScore, double childrenScore) => temp + childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.SP))]
        public double Score_PS(double childScore) => temp + childScore;

        [FeatureCalculator(nameof(Semantics.PList))]
        public double Score_PList(double childScore, double childrenScore) => temp + childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.SN))]
        public double Score_SN(double childScore) => temp + childScore;

        [FeatureCalculator(nameof(Semantics.NList))]
        public double Score_NList(double childScore, double childrenScore) => temp + childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.SE))]
        public double Score_SE(double childScore) => temp + childScore;

        [FeatureCalculator(nameof(Semantics.EList))]
        public double Score_EList(double childScore, double childrenScore) => temp + childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.Transformation), Method = CalculationMethod.FromChildrenFeatureValues)]
        public double Score_Script1(double inScore, double edit) => temp + GetRandom(-10, 10) + edit;

        [FeatureCalculator(nameof(Semantics.Insert))]
        public double Score_Insert(double inScore, double astScore) => temp + inScore + astScore;

        [FeatureCalculator(nameof(Semantics.InsertBefore))]
        public double Score_InsertBefore(double inScore, double astScore) => temp + inScore + astScore;

        [FeatureCalculator(nameof(Semantics.Update))]
        public double Score_Update(double inScore, double toScore) => temp + inScore + toScore;

        [FeatureCalculator(nameof(Semantics.Delete))]
        public double Score_Delete(double inScore, double refscore) => temp + refscore;

        [FeatureCalculator(nameof(Semantics.Node))]
        public double Score_Node1(double kScore, double astScore) => temp + GetRandom(-10, 10) + astScore;

        [FeatureCalculator(nameof(Semantics.ConstNode))]
        public double Score_Node1(double astScore) => temp + GetRandom(-10, 10);

        // Editing Abstract
        [FeatureCalculator(nameof(Semantics.Abstract))]
        public double Score_Abstract(double kindScore) => temp + GetRandom(-10, 10);

        [FeatureCalculator(nameof(Semantics.Context))]
        public double Score_ParentP(double matchScore, double kScore) => temp + matchScore + kScore;

        [FeatureCalculator(nameof(Semantics.ContextPPP))]
        public double Score_ParentPPP(double matchScore, double kScore) => temp + matchScore;

        [FeatureCalculator(nameof(Semantics.Concrete))]
        public double Score_Concrete(double treeScore) => temp + GetRandom(-10, 10);

        [FeatureCalculator(nameof(Semantics.Variable))]
        public double Score_Variable(double idScore) => temp + idScore;

        [FeatureCalculator(nameof(Semantics.Pattern))]
        public double Score_Pattern(double kindScore, double expression1Score) => temp + GetRandom(-10, 10) + expression1Score;


        [FeatureCalculator(nameof(Semantics.Reference))]
        public double Score_Reference(double inScore, double patternScore, double kScore) => temp + GetRandom(-10, 10) + patternScore;

        [FeatureCalculator("id", Method = CalculationMethod.FromLiteral)]
        public double KDScore(string kd)
        {
            string parentOne = "\"\\/\\[[0-9]\\]\"";
            Match f7 = Regex.Match(kd, parentOne);
            string parentTwo = "\"\\/\\[[0-9]\\]\\/\\[[0-9]\\]\"";
            Match f8 = Regex.Match(kd, parentTwo);
            string parentThree = "\"\\/\\[[0-9]\\]\\/\\[[0-9]\\]\\/\\[[0-9]\\]\"";
            Match f9 = Regex.Match(kd, parentThree);
            string nodeItSelf = "\\.";
            Match f10 = Regex.Match(kd, nodeItSelf);
            if (f7.Success)
            {
                return GetRandom(-10, 10);
            }
            else if (f8.Success)
            {
                return GetRandom(-10, 10);
            }
            else if (f9.Success)
            {
                return 0;
            }
            else if (f10.Success)
            {
                return GetRandom(-10, 10);

            }
            return 0;
        }

        [FeatureCalculator("c", Method = CalculationMethod.FromLiteral)]
        public double CScore(int c) => 0;

        [FeatureCalculator("kind", Method = CalculationMethod.FromLiteral)]
        public double KindScore(SyntaxKind kd) => 0;

        [FeatureCalculator("tree", Method = CalculationMethod.FromLiteral)]
        public double NodeScore(SyntaxNodeOrToken kd) => 0;
    }
}
