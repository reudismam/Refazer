using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;

namespace ProseSample.Substrings
{
    public static class RankingScore
    {
        public const double VariableScore = 0;

        [FeatureCalculator("NodesMap")]
        public static double Score_NodesMap( double scriptScore, double editScore) =>  scriptScore + editScore;

        [FeatureCalculator("Stts")]
        public static double Score_Stts(double predScore, double splitScore) => predScore + splitScore;

        [FeatureCalculator("Breaks")]
        public static double Score_Breaks(double predScore, double splitScore) => predScore + splitScore;

        [FeatureCalculator("FTrue")]
        public static double Score_FTrue() => 1.1;

        [FeatureCalculator("SC")]
        public static double Score_CS(double childScore) => childScore;

        [FeatureCalculator("CList")]
        public static double Score_CList(double childScore, double childrenScore) => (childScore + childrenScore) > 0 ? -(childScore + childrenScore) : (childScore + childrenScore);

        [FeatureCalculator("SP")]
        public static double Score_PS(double childScore) => childScore;

        [FeatureCalculator("PList")]
        public static double Score_PList(double childScore, double childrenScore) => (childScore + childrenScore) > 0 ? -(childScore + childrenScore) : (childScore + childrenScore);

        [FeatureCalculator("SN")]
        public static double Score_SN(double childScore) => childScore;

        [FeatureCalculator("NList")]
        public static double Score_NList(double childScore, double childrenScore) => (childScore + childrenScore) > 0 ? -(childScore + childrenScore) : (childScore + childrenScore);

        [FeatureCalculator("SE")]
        public static double Score_SE(double childScore) => childScore;

        [FeatureCalculator("EList")]
        public static double Score_EList(double childScore, double childrenScore) => childScore + childrenScore;

        [FeatureCalculator("SplitNodes")]
        public static double Score_SplitNodes(double inScore) => inScore;

        [FeatureCalculator("Florest")]
        public static double Score_BreakByKind(double inScore, double kind) => inScore + kind;

        [FeatureCalculator("Script")]
        public static double Score_Script1(double inScore, double edit) => inScore + edit;

        [FeatureCalculator("OneTrans")]
        public static double Score_OneTrans(double inScore, double script) => inScore + script;

        [FeatureCalculator("ManyTrans")]
        public static double Score_ManyTrans(double inScore, double loop) => inScore + loop;

        [FeatureCalculator("Loop")]
        public static double Score_Loop(double inScore, double breaks) => inScore + breaks;

        [FeatureCalculator("Insert")]
        public static double Score_Insert(double inScore, double kScore, double expressionScore, double astScore) => inScore + kScore + expressionScore + astScore;

        [FeatureCalculator("Move")]
        public static double Score_Move(double inScore, double kScore, double fromScore, double toScore) => inScore + kScore + fromScore + toScore;

        [FeatureCalculator("Update")]
        public static double Score_Update(double inScore, double fromScore, double toScore) => inScore + fromScore + toScore;

        [FeatureCalculator("Delete")]
        public static double Score_Delete(double inScore, double fromScore) => inScore + fromScore;

        [FeatureCalculator("Node")]
        public static double Score_Node1(double kScore, double astScore) => kScore +  astScore;

        [FeatureCalculator("Const")]
        public static double Score_Node1(double astScore) => astScore;

        [FeatureCalculator("Tree")]
        public static double Score_Tree(double kindScore) => kindScore;

        [FeatureCalculator("Ref")]
        public static double Score_Ref(double inScore, double matchScore) => matchScore;

        [FeatureCalculator("KindRef")]
        public static double Score_KindRef(double inScore, double kindScore, double kScore) => kindScore;

        [FeatureCalculator("Abstract")]
        public static double Score_Abstract(double kindScore) => kindScore;

        [FeatureCalculator("Concrete")]
        public static double Score_Concrete(double treeScore) => -50;

        [FeatureCalculator("Parent")]
        public static double Score_Parent(double inScore, double matchScore, double kScore) => matchScore;

        [FeatureCalculator("P")]
        public static double Score_P(double inScore, double kindScore, double expression1Score) => kindScore + expression1Score;

        [FeatureCalculator("Literal")]
        public static double Score_Literal(double inScore, double treeScore) => treeScore;

        [FeatureCalculator("C")]
        public static double Score_C1(double inScore, double kindScore, double expression1Score) => kindScore + expression1Score;

        [FeatureCalculator(Method = CalculationMethod.FromLiteral)]
        public static double KScore(int k) => k >= 0 ? 1.0 / (k + 1.0) : 1.0 / (-k + 1.1);

        [FeatureCalculator(Method = CalculationMethod.FromLiteral)]

        public static double KDScore(string kd) => 1.1;

        [FeatureCalculator(Method = CalculationMethod.FromLiteral)]
        public static double KindScore(SyntaxKind kd) => 1.1;

        [FeatureCalculator(Method = CalculationMethod.FromLiteral)]
        public static double NodeScore(SyntaxNodeOrToken kd) => 1.1;
    }
}
