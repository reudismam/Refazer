using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using RefazerFunctions.Bean;

namespace RefazerFunctions.Spg.Ranking
{
    public interface RankingFunction
    {
        [FeatureCalculator("EditMap")]
        double Score_EditMap(double scriptScore, double editScore);

        [FeatureCalculator(nameof(Semantics.AllNodes))]
        double Score_Traversal(double scriptScore, double editScore);

        [FeatureCalculator("EditFilter")]
        double Score_EditFilter(double predScore, double splitScore);

        [FeatureCalculator(nameof(Semantics.Match))]
        double Score_Match(double inSource, double matchScore);

        [FeatureCalculator(nameof(Semantics.SC))]
        double Score_CS(double childScore);

        [FeatureCalculator(nameof(Semantics.CList))]
        double Score_CList(double childScore, double childrenScore);

        [FeatureCalculator(nameof(Semantics.SP))]
        double Score_PS(double childScore);

        [FeatureCalculator(nameof(Semantics.PList))]
        double Score_PList(double childScore, double childrenScore);

        [FeatureCalculator(nameof(Semantics.SN))]
        double Score_SN(double childScore);

        [FeatureCalculator(nameof(Semantics.NList))]
        double Score_NList(double childScore, double childrenScore);

        [FeatureCalculator(nameof(Semantics.SE))]
        double Score_SE(double childScore);

        [FeatureCalculator(nameof(Semantics.EList))]
        double Score_EList(double childScore, double childrenScore);

        [FeatureCalculator(nameof(Semantics.Transformation), Method = CalculationMethod.FromChildrenFeatureValues)]
        double Score_Script1(double inScore, double edit);

        [FeatureCalculator(nameof(Semantics.Insert))]
        double Score_Insert(double inScore, double astScore);

        [FeatureCalculator(nameof(Semantics.InsertBefore))]
        double Score_InsertBefore(double inScore, double astScore);

        [FeatureCalculator(nameof(Semantics.Update))]
        double Score_Update(double inScore, double toScore);

        [FeatureCalculator(nameof(Semantics.Delete))]
        double Score_Delete(double inScore, double refscore);

        [FeatureCalculator(nameof(Semantics.Node))]
        double Score_Node1(double kScore, double astScore);

        [FeatureCalculator(nameof(Semantics.ConstNode))]
        double Score_Node1(double astScore);

        // Editing Abstract
        [FeatureCalculator(nameof(Semantics.Abstract))]
        double Score_Abstract(double kindScore);

        [FeatureCalculator(nameof(Semantics.Context))]
        double Score_ParentP(double matchScore, double kScore);

        [FeatureCalculator(nameof(Semantics.ContextPPP))]
        double Score_ParentPPP(double matchScore, double kScore);

        [FeatureCalculator(nameof(Semantics.Concrete))]
        double Score_Concrete(double treeScore);

        [FeatureCalculator(nameof(Semantics.Variable))]
        double Score_Variable(double idScore);

        [FeatureCalculator(nameof(Semantics.Pattern))]
        double Score_Pattern(double kindScore, double expression1Score);


        [FeatureCalculator(nameof(Semantics.Reference))]
        double Score_Reference(double inScore, double patternScore, double kScore);

        [FeatureCalculator("id", Method = CalculationMethod.FromLiteral)]
        double KDScore(string kd);

        [FeatureCalculator("c", Method = CalculationMethod.FromLiteral)]
        double CScore(int c);

        [FeatureCalculator("kind", Method = CalculationMethod.FromLiteral)]
        double KindScore(SyntaxKind kd);

        [FeatureCalculator("tree", Method = CalculationMethod.FromLiteral)]
        double NodeScore(Node kd);
    }
}
