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

        [FeatureCalculator("EditMap")]
        public static double Score_EditMap(double scriptScore, double editScore) => scriptScore + editScore;

        [FeatureCalculator("Traversal")]
        public static double Score_Traversal(double scriptScore, double editScore) => scriptScore + editScore;

        [FeatureCalculator("Stts")]
        public static double Score_Stts(double predScore, double splitScore) => predScore + splitScore;

        [FeatureCalculator("Breaks")]
        public static double Score_Breaks(double predScore, double splitScore) => predScore + splitScore;

        [FeatureCalculator("EditFilter")]
        public static double Score_EditFilter(double predScore, double splitScore) => predScore + splitScore;

        [FeatureCalculator("FTrue")]
        public static double Score_FTrue() => 1.1;

        [FeatureCalculator("Match")]
        public static double Score_Match(double inSource, double matchScore) => matchScore;

        [FeatureCalculator("NMatch")]
        public static double Score_NMatch(double matchScore) => matchScore;

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

        [FeatureCalculator("SplitNodes")]
        public static double Score_SplitNodes(double inScore) => inScore;

        [FeatureCalculator("Template")]
        public static double Score_Template(double inScore, double kind) => inScore + kind;

        [FeatureCalculator("Transformation")]
        public static double Score_Script1(double inScore, double edit) => inScore + edit;

        [FeatureCalculator("Script")]
        public static double Score_Edit(double edit) => edit;

        //[FeatureCalculator("Transformation")]
        //public static double Score_ManyTrans(double inScore, double loop) => inScore + loop;

        [FeatureCalculator("RegionMap")]
        public static double Score_Loop(double inScore, double breaks) => inScore + breaks;

        [FeatureCalculator("ParentNode")]
        public static double Score_ParentNode(double inScore, double expressionScore, double astScore) => inScore + expressionScore + astScore;

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

        [FeatureCalculator("Const")]
        public static double Score_Node1(double astScore) => astScore;

        [FeatureCalculator("Tree")]
        public static double Score_Tree(double kindScore) => kindScore;

        [FeatureCalculator("Ref")]
        public static double Score_Ref(double inScore, double matchScore) => matchScore;

        [FeatureCalculator("Variable")]
        public static double Score_Variable(double kindScore) => kindScore;

        [FeatureCalculator("Leaf")]
        public static double Score_Leaf(double kindScore) => kindScore;

        [FeatureCalculator("Abstract")]
        public static double Score_Abstract(double kindScore) => kindScore;

        [FeatureCalculator("Concrete")]
        public static double Score_Concrete(double treeScore) => treeScore;

        [FeatureCalculator("Parent")]
        public static double Score_Parent(double matchScore, double kScore) => matchScore + 30;

        [FeatureCalculator("RightChild")]
        public static double Score_RightChild(double inScore, double matchScore) => matchScore;

        [FeatureCalculator("Child")]
        public static double Score_Child(double inScore, double matchScore) => matchScore;

        [FeatureCalculator("P")]
        public static double Score_P(double kindScore, double expression1Score) => kindScore + expression1Score;

        [FeatureCalculator("Literal")]
        public static double Score_Literal(double treeScore) => treeScore + 30;

        [FeatureCalculator("C")]
        public static double Score_C1(double kindScore, double expression1Score) => kindScore + expression1Score;

        [FeatureCalculator("Reference")]
        public static double Score_Reference(double inScore, double patternScore, double kScore) => patternScore + 30;

        [FeatureCalculator(Method = CalculationMethod.FromLiteral)]
        public static double KScore(int k) => k >= 0 ? 1.0 : -30;

        [FeatureCalculator(Method = CalculationMethod.FromLiteral)]

        public static double KDScore(string kd) => 1.1;

        [FeatureCalculator(Method = CalculationMethod.FromLiteral)]
        public static double KindScore(SyntaxKind kd) => 1.1;

        [FeatureCalculator(Method = CalculationMethod.FromLiteral)]
        public static double NodeScore(SyntaxNodeOrToken kd) => 1.1;
    }
}
