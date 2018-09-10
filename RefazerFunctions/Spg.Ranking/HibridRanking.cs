using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;

namespace RefazerFunctions.Spg.Ranking
{
    public class HibridRanking: RankingFunction
    {
        private RankingFunction ranking;
        private int examples;
        private int THRESHOULD = 2;
        public HibridRanking()
        {
            examples = RankingScore.getExamplesCount();
            if (examples < THRESHOULD)
            {
                 ranking = new MLRankingLogisticRegressionNonLinear();
            }
            else
            {
                ranking = new ManualRanking();

            }
        }

        // Editing EditMap
        [FeatureCalculator("EditMap")]
        public double Score_EditMap(double scriptScore, double editScore)
        {
            return ranking.Score_EditMap(scriptScore, editScore);
        }

        [FeatureCalculator(nameof(Semantics.AllNodes))]
        public double Score_Traversal(double scriptScore, double editScore)
        {
            return ranking.Score_Traversal(scriptScore, editScore);
        }

        [FeatureCalculator("EditFilter")]
        public double Score_EditFilter(double predScore, double splitScore)
        {
            return ranking.Score_EditFilter(predScore, splitScore);
        }

        [FeatureCalculator(nameof(Semantics.Match))]
        public double Score_Match(double inSource, double matchScore)
        {
            return ranking.Score_Match(inSource, matchScore);
        }

        [FeatureCalculator(nameof(Semantics.SC))]
        public double Score_CS(double childScore)
        {
            return ranking.Score_CS(childScore);
        }

        [FeatureCalculator(nameof(Semantics.CList))]
        public double Score_CList(double childScore, double childrenScore)
        {
            return ranking.Score_CList(childScore, childrenScore);
        }

        [FeatureCalculator(nameof(Semantics.SP))]
        public double Score_PS(double childScore)
        {
            return ranking.Score_PS(childScore);
        }

        [FeatureCalculator(nameof(Semantics.PList))]
        public double Score_PList(double childScore, double childrenScore)
        {
            return ranking.Score_PList(childScore, childrenScore);
        }

        [FeatureCalculator(nameof(Semantics.SN))]
        public double Score_SN(double childScore)
        {
            return ranking.Score_SN(childScore);
        }

        [FeatureCalculator(nameof(Semantics.NList))]
        public double Score_NList(double childScore, double childrenScore)
        { 
            return ranking.Score_NList(childScore, childrenScore);
        }

        [FeatureCalculator(nameof(Semantics.SE))]
        public double Score_SE(double childScore)
        {
            return ranking.Score_SE(childScore);
        }

        [FeatureCalculator(nameof(Semantics.EList))]
        public double Score_EList(double childScore, double childrenScore)
        {
            return ranking.Score_EList(childScore, childrenScore);
        }

        [FeatureCalculator(nameof(Semantics.Transformation), Method = CalculationMethod.FromChildrenFeatureValues)]
        public double Score_Script1(double inScore, double edit)
        {
            return ranking.Score_Script1(inScore, edit);
        }

        [FeatureCalculator(nameof(Semantics.Insert))]
        public double Score_Insert(double inScore, double astScore)
        {
            return ranking.Score_Insert(inScore, astScore);
        }

        [FeatureCalculator(nameof(Semantics.InsertBefore))]
        public double Score_InsertBefore(double inScore, double astScore)
        {
            return ranking.Score_InsertBefore(inScore, astScore);
        }

        [FeatureCalculator(nameof(Semantics.Update))]
        public double Score_Update(double inScore, double toScore)
        {
            return ranking.Score_Update(inScore, toScore);
        }

        [FeatureCalculator(nameof(Semantics.Delete))]
        public double Score_Delete(double inScore, double refscore)
        {
            return ranking.Score_Delete(inScore, refscore);
        }

        [FeatureCalculator(nameof(Semantics.Node))]
        public double Score_Node1(double kScore, double astScore)
        {
            return ranking.Score_Node1(kScore, astScore);
        }

        [FeatureCalculator(nameof(Semantics.ConstNode))]
        public double Score_Node1(double astScore)
        {
            return ranking.Score_Node1(astScore);
        }

        // Editing Abstract
        [FeatureCalculator(nameof(Semantics.Abstract))]
        public double Score_Abstract(double kindScore)
        {
            return ranking.Score_Abstract(kindScore);
        }

        [FeatureCalculator(nameof(Semantics.Context))]
        public double Score_ParentP(double matchScore, double kScore)
        {
            return ranking.Score_ParentP(matchScore, kScore);
        }

        [FeatureCalculator(nameof(Semantics.ContextPPP))]
        public double Score_ParentPPP(double matchScore, double kScore)
        {
            return ranking.Score_ParentPPP(matchScore, kScore);
        }

        [FeatureCalculator(nameof(Semantics.Concrete))]
        public double Score_Concrete(double treeScore)
        {
            return ranking.Score_Concrete(treeScore);
        }

        [FeatureCalculator(nameof(Semantics.Variable))]
        public double Score_Variable(double idScore)
        {
            return ranking.Score_Variable(idScore);
        }

        [FeatureCalculator(nameof(Semantics.Pattern))]
        public double Score_Pattern(double kindScore, double expression1Score)
        {
            return ranking.Score_Pattern(kindScore, expression1Score);
        }

        [FeatureCalculator(nameof(Semantics.Reference))]
        public double Score_Reference(double inScore, double patternScore, double kScore)
        {
            return ranking.Score_Reference(inScore, patternScore, kScore);
        }

        [FeatureCalculator("id", Method = CalculationMethod.FromLiteral)]
        public double KDScore(string kd)
        {
            return ranking.KDScore(kd);
        }

        [FeatureCalculator("c", Method = CalculationMethod.FromLiteral)]
        public double CScore(int c) => ranking.CScore(c);

        [FeatureCalculator("kind", Method = CalculationMethod.FromLiteral)]
        public double KindScore(SyntaxKind kd) => ranking.KindScore(kd);

        [FeatureCalculator("tree", Method = CalculationMethod.FromLiteral)]
        public double NodeScore(SyntaxNodeOrToken kd) => ranking.NodeScore(kd);
    }
}
