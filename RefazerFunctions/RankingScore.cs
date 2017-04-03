using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using RefazerFunctions.Spg.Bean;

namespace RefazerFunctions
{
    public class RankingScore: Feature<double>
    {
        public RankingScore(Grammar grammar) : base(grammar, "Score") { }

        public const double VariableScore = 0;

        [FeatureCalculator("EditMap")]
        public static double Score_EditMap(double scriptScore, double editScore) => scriptScore + editScore;

        [FeatureCalculator(nameof(Semantics.AllNodes))]
        public static double Score_Traversal(double scriptScore, double editScore) => scriptScore + editScore;

        [FeatureCalculator("EditFilter")]
        public static double Score_EditFilter(double predScore, double splitScore) => (predScore + splitScore) * 1.1;

        [FeatureCalculator(nameof(Semantics.Match))]
        public static double Score_Match(double inSource, double matchScore) => matchScore;

        [FeatureCalculator(nameof(Semantics.SC))]
        public static double Score_CS(double childScore) => childScore;

        [FeatureCalculator(nameof(Semantics.CList))]
        public static double Score_CList(double childScore, double childrenScore) => childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.SP))]
        public static double Score_PS(double childScore) => childScore;

        [FeatureCalculator(nameof(Semantics.PList))]
        public static double Score_PList(double childScore, double childrenScore) => childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.SN))]
        public static double Score_SN(double childScore) => childScore;

        [FeatureCalculator(nameof(Semantics.NList))]
        public static double Score_NList(double childScore, double childrenScore) => childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.SE))]
        public static double Score_SE(double childScore) => childScore;

        [FeatureCalculator(nameof(Semantics.EList))]
        public static double Score_EList(double childScore, double childrenScore) => childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.Transformation))]
        public static double Score_Script1(double inScore, double edit) => inScore + edit;

        [FeatureCalculator(nameof(Semantics.Insert))]
        public static double Score_Insert(double inScore, double astScore) => inScore + astScore;

        [FeatureCalculator(nameof(Semantics.InsertBefore))]
        public static double Score_InsertBefore(double inScore, double astScore) => inScore + astScore;

        [FeatureCalculator(nameof(Semantics.Update))]
        public static double Score_Update(double inScore, double toScore) => inScore + toScore;

        [FeatureCalculator(nameof(Semantics.Delete))]
        public static double Score_Delete(double inScore) => inScore;

        [FeatureCalculator(nameof(Semantics.Node))]
        public static double Score_Node1(double kScore, double astScore) => kScore +  astScore;

        [FeatureCalculator(nameof(Semantics.ConstNode))]
        public static double Score_Node1(double astScore) => astScore;

        [FeatureCalculator(nameof(Semantics.Abstract))]
        public static double Score_Abstract(double kindScore) => kindScore;

        [FeatureCalculator(nameof(Semantics.Context))]
        public static double Score_ParentP(double matchScore, double kScore) => matchScore;

        [FeatureCalculator(nameof(Semantics.ContextPPP))]
        public static double Score_ParentPPP(double matchScore, double kScore) => matchScore;

        [FeatureCalculator(nameof(Semantics.Concrete))]
        public static double Score_Concrete(double treeScore) => treeScore * 1000;

        [FeatureCalculator(nameof(Semantics.Pattern))]
        public static double Score_Pattern(double kindScore, double expression1Score) => kindScore + expression1Score;

        [FeatureCalculator(nameof(Semantics.Reference))]
        public static double Score_Reference(double inScore, double patternScore, double kScore) => patternScore;

        [FeatureCalculator("id", Method = CalculationMethod.FromLiteral)]
        public static double KDScore(string kd) => 1.1;

        [FeatureCalculator("c", Method = CalculationMethod.FromLiteral)]
        public static double CScore(int c) => 1.1;

        [FeatureCalculator("kind", Method = CalculationMethod.FromLiteral)]
        public static double KindScore(SyntaxKind kd) => 1.1;

        [FeatureCalculator("tree", Method = CalculationMethod.FromLiteral)]
        public static double NodeScore(SyntaxNodeOrToken kd) => 1.1;
    }
}
