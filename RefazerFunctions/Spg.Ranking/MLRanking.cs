using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using System.Text.RegularExpressions;

namespace RefazerFunctions.Spg.Ranking
{
    public class MLRanking: RankingFunction
    {
        // Editing EditMap
        [FeatureCalculator("EditMap")]
        public double Score_EditMap(double scriptScore, double editScore) => -0.6406 + 23.7911 + scriptScore + editScore;

        [FeatureCalculator(nameof(Semantics.AllNodes))]
        public double Score_Traversal(double scriptScore, double editScore) => -0.6406 + scriptScore + editScore;

        [FeatureCalculator("EditFilter")]
        public double Score_EditFilter(double predScore, double splitScore) => -0.6406 + (predScore + splitScore);

        [FeatureCalculator(nameof(Semantics.Match))]
        public double Score_Match(double inSource, double matchScore) => -0.6406 + matchScore;

        [FeatureCalculator(nameof(Semantics.SC))]
        public double Score_CS(double childScore) => -0.6406 + childScore;

        [FeatureCalculator(nameof(Semantics.CList))]
        public double Score_CList(double childScore, double childrenScore) => -0.6406 + childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.SP))]
        public double Score_PS(double childScore) => -0.6406 + childScore;

        [FeatureCalculator(nameof(Semantics.PList))]
        public double Score_PList(double childScore, double childrenScore) => -0.6406 + childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.SN))]
        public double Score_SN(double childScore) => -0.6406 + childScore;

        [FeatureCalculator(nameof(Semantics.NList))]
        public double Score_NList(double childScore, double childrenScore) => -0.6406 + childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.SE))]
        public double Score_SE(double childScore) => -0.6406 + childScore;

        [FeatureCalculator(nameof(Semantics.EList))]
        public double Score_EList(double childScore, double childrenScore) => -0.6406 + childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.Transformation), Method = CalculationMethod.FromChildrenFeatureValues)]
        public double Score_Script1(double inScore, double edit) => -0.6406 - 21.0728 + edit;

        [FeatureCalculator(nameof(Semantics.Insert))]
        public double Score_Insert(double inScore, double astScore) => -0.6406 + inScore + astScore;

        [FeatureCalculator(nameof(Semantics.InsertBefore))]
        public double Score_InsertBefore(double inScore, double astScore) => -0.6406 + inScore + astScore;

        [FeatureCalculator(nameof(Semantics.Update))]
        public double Score_Update(double inScore, double toScore) => -0.6406 + inScore + toScore;

        [FeatureCalculator(nameof(Semantics.Delete))]
        public double Score_Delete(double inScore, double refscore) => -0.6406 + refscore;

        [FeatureCalculator(nameof(Semantics.Node))]
        public double Score_Node1(double kScore, double astScore) => -0.6406 + 1.1386 + astScore;

        [FeatureCalculator(nameof(Semantics.ConstNode))]
        public double Score_Node1(double astScore) => -0.6406 + 1.0916;

        // Editing Abstract
        [FeatureCalculator(nameof(Semantics.Abstract))]
        public double Score_Abstract(double kindScore) => -0.6406 + 0.9729;

        [FeatureCalculator(nameof(Semantics.Context))]
        public double Score_ParentP(double matchScore, double kScore) => -0.6406 + matchScore + kScore;

        [FeatureCalculator(nameof(Semantics.ContextPPP))]
        public double Score_ParentPPP(double matchScore, double kScore) => -0.6406 + matchScore;

        [FeatureCalculator(nameof(Semantics.Concrete))]
        public double Score_Concrete(double treeScore) => -0.6406 + 1.2855;

        [FeatureCalculator(nameof(Semantics.Variable))]
        public double Score_Variable(double idScore) => -0.6406 + idScore;

        [FeatureCalculator(nameof(Semantics.Pattern))]
        public double Score_Pattern(double kindScore, double expression1Score) => -0.6406 + 1.3718 + expression1Score;

        [FeatureCalculator(nameof(Semantics.Reference))]
        public double Score_Reference(double inScore, double patternScore, double kScore) => -0.6406 + 1.3481 + patternScore;

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
                return -0.3184;
            }
            else if (f8.Success)
            {
                return 0;
            }
            else if (f9.Success)
            {
                return 0;
            }
            else if (f10.Success)
            {
                return -1.1743;

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
