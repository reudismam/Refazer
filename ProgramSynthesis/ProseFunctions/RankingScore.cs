using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using ProseFunctions.Spg.Bean;

namespace ProseFunctions.Substrings
{
    public static class RankingScore
    {
        public const double VariableScore = 0;

        [FeatureCalculator("EditMap")]
        public static double Score_EditMap(double scriptScore, double editScore) => scriptScore + editScore;

        [FeatureCalculator("Traversal")]
        public static double Score_Traversal(double scriptScore, double editScore) => scriptScore + editScore;

        [FeatureCalculator("EditFilter")]
        public static double Score_EditFilter(double predScore, double splitScore) => predScore + splitScore;

        [FeatureCalculator("Match")]
        public static double Score_Match(double inSource, double matchScore) => matchScore;

        [FeatureCalculator("NodeMatch")]
        public static double Score_NodeMatch(double matchScore) => matchScore;

        [FeatureCalculator("SC")]
        public static double Score_CS(double childScore) => childScore;

        [FeatureCalculator("CList")]
        public static double Score_CList(double childScore, double childrenScore) => /*(childScore + childrenScore) > 0 ? -(childScore + childrenScore) :*/ (childScore + childrenScore);

        [FeatureCalculator("SP")]
        public static double Score_PS(double childScore) => childScore;

        [FeatureCalculator("PList")]
        public static double Score_PList(double childScore, double childrenScore) => /*(childScore + childrenScore) > 0 ? -(childScore + childrenScore) :*/ (childScore + childrenScore);

        [FeatureCalculator("SO")]
        public static double Score_SO(double childScore) => childScore;

        [FeatureCalculator("SL")]
        public static double Score_SL(double childScore, double childrenScore) => /*(childScore + childrenScore) > 0 ? -(childScore + childrenScore) :*/ (childScore + childrenScore);

        [FeatureCalculator("SN")]
        public static double Score_SN(double childScore) => childScore;

        [FeatureCalculator("NList")]
        public static double Score_NList(double childScore, double childrenScore) => /*(childScore + childrenScore) > 0 ? -(childScore + childrenScore) :*/ (childScore + childrenScore);

        [FeatureCalculator("SE")]
        public static double Score_SE(double childScore) => childScore;

        [FeatureCalculator("EList")]
        public static double Score_EList(double childScore, double childrenScore) => childScore + childrenScore;

        [FeatureCalculator("Transformation")]
        public static double Score_Script1(double inScore, double edit) => inScore + edit;

        [FeatureCalculator("Insert")]
        public static double Score_Insert(double inScore, double astScore, double kScore) => inScore + astScore + kScore;

        [FeatureCalculator("InsertBefore")]
        public static double Score_InsertBefore(double inScore, double astScore) => inScore + astScore;

        [FeatureCalculator("Move")]
        public static double Score_Move(double inScore, double toScore, double kScore) => inScore + kScore + toScore;

        [FeatureCalculator("Update")]
        public static double Score_Update(double inScore, double toScore) => inScore + toScore;

        [FeatureCalculator("Delete")]
        public static double Score_Delete(double inScore) => inScore;

        [FeatureCalculator("Node")]
        public static double Score_Node1(double kScore, double astScore) => kScore +  astScore;

        [FeatureCalculator("ConstNode")]
        public static double Score_Node1(double astScore) => astScore;

        [FeatureCalculator("Abstract")]
        public static double Score_Abstract(double kindScore) => kindScore;

        [FeatureCalculator("Context")]
        public static double Score_Parent(double matchScore, double kScore) => matchScore + 30;

        [FeatureCalculator("Concrete")]
        public static double Score_Concrete(double treeScore) => treeScore + 30;

        [FeatureCalculator("Pattern")]
        public static double Score_Pattern(double kindScore, double expression1Score) => kindScore + expression1Score;

        [FeatureCalculator("Reference")]
        public static double Score_Reference(double inScore, double patternScore, double kScore) => patternScore + 200;

        [FeatureCalculator(Method = CalculationMethod.FromLiteral)]
        public static double KScore(K k) => 1.1;

        [FeatureCalculator(Method = CalculationMethod.FromLiteral)]
        public static double KDScore(string kd) => 1.1;

        [FeatureCalculator(Method = CalculationMethod.FromLiteral)]
        public static double CScore(int c) => 1.1;

        [FeatureCalculator(Method = CalculationMethod.FromLiteral)]
        public static double KindScore(SyntaxKind kd) => 1.1;

        [FeatureCalculator(Method = CalculationMethod.FromLiteral)]
        public static double NodeScore(SyntaxNodeOrToken kd) => 1.1;
    }
}
