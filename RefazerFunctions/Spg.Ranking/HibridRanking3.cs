using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;

namespace RefazerFunctions.Spg.Ranking
{
    public class HibridRanking3: RankingFunction
    {
        private RankingFunction ranking1;
        private RankingFunction ranking2;
        private int examples;
        private double THRESHOULD = 0.2;
        public HibridRanking3()
        {
            ranking1 = new MLRankingLogisticRegressionNonLinear();
            ranking2 = new ManualRanking();
        }

        public double Mix(double valueRanking1, double valueRanking2)
        {
            return THRESHOULD * valueRanking1 + (1 - THRESHOULD) * valueRanking2;
        }

        // Editing EditMap
        [FeatureCalculator("EditMap")]
        public double Score_EditMap(double scriptScore, double editScore)
        {
            return Mix(ranking1.Score_EditMap(scriptScore, editScore), ranking2.Score_EditMap(scriptScore, editScore));
        }

        [FeatureCalculator(nameof(Semantics.AllNodes))]
        public double Score_Traversal(double scriptScore, double editScore)
        {
            return Mix(ranking1.Score_Traversal(scriptScore, editScore), ranking2.Score_Traversal(scriptScore, editScore));
        }

        [FeatureCalculator("EditFilter")]
        public double Score_EditFilter(double predScore, double splitScore)
        {
            return Mix(ranking1.Score_EditFilter(predScore, splitScore), ranking2.Score_EditFilter(predScore, splitScore));
        }

        [FeatureCalculator(nameof(Semantics.Match))]
        public double Score_Match(double inSource, double matchScore)
        {
            return Mix(ranking1.Score_Match(inSource, matchScore), ranking2.Score_Match(inSource, matchScore));
        }

        [FeatureCalculator(nameof(Semantics.SC))]
        public double Score_CS(double childScore)
        {
            return Mix(ranking1.Score_CS(childScore), ranking2.Score_CS(childScore));
        }

        [FeatureCalculator(nameof(Semantics.CList))]
        public double Score_CList(double childScore, double childrenScore)
        {
            return Mix(ranking1.Score_CList(childScore, childrenScore), ranking2.Score_CList(childScore, childrenScore));
        }

        [FeatureCalculator(nameof(Semantics.SP))]
        public double Score_PS(double childScore)
        {
            return Mix(ranking1.Score_PS(childScore), ranking2.Score_PS(childScore));
        }

        [FeatureCalculator(nameof(Semantics.PList))]
        public double Score_PList(double childScore, double childrenScore)
        {
            return Mix(ranking1.Score_PList(childScore, childrenScore), ranking2.Score_PList(childScore, childrenScore));
        }

        [FeatureCalculator(nameof(Semantics.SN))]
        public double Score_SN(double childScore)
        {
            return Mix(ranking1.Score_SN(childScore), ranking2.Score_SN(childScore));
        }

        [FeatureCalculator(nameof(Semantics.NList))]
        public double Score_NList(double childScore, double childrenScore)
        { 
            return Mix(ranking1.Score_NList(childScore, childrenScore), ranking2.Score_NList(childScore, childrenScore));
        }

        [FeatureCalculator(nameof(Semantics.SE))]
        public double Score_SE(double childScore)
        {
            return Mix(ranking1.Score_SE(childScore), ranking2.Score_SE(childScore));
        }

        [FeatureCalculator(nameof(Semantics.EList))]
        public double Score_EList(double childScore, double childrenScore)
        {
            return Mix(ranking1.Score_EList(childScore, childrenScore), ranking2.Score_EList(childScore, childrenScore));
        }

        [FeatureCalculator(nameof(Semantics.Transformation), Method = CalculationMethod.FromChildrenFeatureValues)]
        public double Score_Script1(double inScore, double edit)
        {
            return Mix(ranking1.Score_Script1(inScore, edit), ranking2.Score_Script1(inScore, edit));
        }

        [FeatureCalculator(nameof(Semantics.Insert))]
        public double Score_Insert(double inScore, double astScore)
        {
            return Mix(ranking1.Score_Insert(inScore, astScore), ranking2.Score_Insert(inScore, astScore));
        }

        [FeatureCalculator(nameof(Semantics.InsertBefore))]
        public double Score_InsertBefore(double inScore, double astScore)
        {
            return Mix(ranking1.Score_InsertBefore(inScore, astScore), ranking2.Score_InsertBefore(inScore, astScore));
        }

        [FeatureCalculator(nameof(Semantics.Update))]
        public double Score_Update(double inScore, double toScore)
        {
            return Mix(ranking1.Score_Update(inScore, toScore), ranking2.Score_Update(inScore, toScore));
        }

        [FeatureCalculator(nameof(Semantics.Delete))]
        public double Score_Delete(double inScore, double refscore)
        {
            return Mix(ranking1.Score_Delete(inScore, refscore), ranking2.Score_Delete(inScore, refscore));
        }

        [FeatureCalculator(nameof(Semantics.Node))]
        public double Score_Node1(double kScore, double astScore)
        {
            return Mix(ranking1.Score_Node1(kScore, astScore), ranking2.Score_Node1(kScore, astScore));
        }

        [FeatureCalculator(nameof(Semantics.ConstNode))]
        public double Score_Node1(double astScore)
        {
            return Mix(ranking1.Score_Node1(astScore), ranking2.Score_Node1(astScore));
        }

        // Editing Abstract
        [FeatureCalculator(nameof(Semantics.Abstract))]
        public double Score_Abstract(double kindScore)
        {
            return Mix(ranking1.Score_Abstract(kindScore), ranking2.Score_Abstract(kindScore));
        }

        [FeatureCalculator(nameof(Semantics.Context))]
        public double Score_ParentP(double matchScore, double kScore)
        {
            return Mix(ranking1.Score_ParentP(matchScore, kScore), ranking2.Score_ParentP(matchScore, kScore));
        }

        [FeatureCalculator(nameof(Semantics.ContextPPP))]
        public double Score_ParentPPP(double matchScore, double kScore)
        {
            return Mix(ranking1.Score_ParentPPP(matchScore, kScore), ranking2.Score_ParentPPP(matchScore, kScore));
        }

        [FeatureCalculator(nameof(Semantics.Concrete))]
        public double Score_Concrete(double treeScore)
        {
            return Mix(ranking1.Score_Concrete(treeScore), ranking2.Score_Concrete(treeScore));
        }

        [FeatureCalculator(nameof(Semantics.Variable))]
        public double Score_Variable(double idScore)
        {
            return Mix(ranking1.Score_Variable(idScore), ranking2.Score_Variable(idScore));
        }

        [FeatureCalculator(nameof(Semantics.Pattern))]
        public double Score_Pattern(double kindScore, double expression1Score)
        {
            return Mix(ranking1.Score_Pattern(kindScore, expression1Score), ranking2.Score_Pattern(kindScore, expression1Score));
        }

        [FeatureCalculator(nameof(Semantics.Reference))]
        public double Score_Reference(double inScore, double patternScore, double kScore)
        {
            return Mix(ranking1.Score_Reference(inScore, patternScore, kScore), ranking2.Score_Reference(inScore, patternScore, kScore));
        }

        [FeatureCalculator("id", Method = CalculationMethod.FromLiteral)]
        public double KDScore(string kd)
        {
            return Mix(ranking1.KDScore(kd), ranking2.KDScore(kd));
        }

        [FeatureCalculator("c", Method = CalculationMethod.FromLiteral)]
        public double CScore(int c) => Mix(ranking1.CScore(c), ranking2.CScore(c));

        [FeatureCalculator("kind", Method = CalculationMethod.FromLiteral)]
        public double KindScore(SyntaxKind kd) => Mix(ranking1.KindScore(kd), ranking2.KindScore(kd));

        [FeatureCalculator("tree", Method = CalculationMethod.FromLiteral)]
        public double NodeScore(SyntaxNodeOrToken kd) => Mix(ranking1.NodeScore(kd), ranking2.NodeScore(kd));
    }
}
