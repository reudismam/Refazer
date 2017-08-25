/* Abordagem Refazer
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using RefazerFunctions.Spg.Bean;
using System.Text.RegularExpressions;

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

        [FeatureCalculator(nameof(Semantics.Transformation), Method = CalculationMethod.FromChildrenFeatureValues)]

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

        [FeatureCalculator(nameof(Semantics.Variable))]
        public static double Score_Variable(double idScore) => idScore;

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

    public class ExtractFeature {

        public static int[] Extract(string program)
        {    
            //put here the R code.
            string constNode = "constNode";
            Match f1 = Regex.Match(program, constNode);
            int v1 = int.Parse(f1.Value);
           
            string reference = "Reference";
            Match f2 = Regex.Match(program, reference);
            int v2 = int.Parse(f1.Value);

            string concretePattern = "Concrete";
            Match f3 = Regex.Match(program, concretePattern);
            int v3 = int.Parse(f1.Value);

            string abstractPattern = "Abstract";
            Match f4 = Regex.Match(program, abstractPattern);
            int v4 = int.Parse(f1.Value);

            string node = "Node";
            Match f5 = Regex.Match(program, node);
            int v5 = int.Parse(f1.Value);

            string pattern = "Pattern";
            Match f6 = Regex.Match(program, pattern);
            int v6 = int.Parse(f1.Value);

            string parentOne = "\"\\/\\[[0-9]\\]\"";
            Match f7 = Regex.Match(program, parentOne);
            int v7 = int.Parse(f1.Value);

            string parentTwo = "\"\\/\\[[0-9]\\]\\/\\[[0-9]\\]\"";
            Match f8 = Regex.Match(program, parentTwo);
            int v8 = int.Parse(f1.Value);

            string parentThree = "\"\\/\\[[0-9]\\]\\/\\[[0-9]\\]\\/\\[[0-9]\\]\"";
            Match f9 = Regex.Match(program, parentThree);
            int v9 = int.Parse(f1.Value);

            string nodeItSelf = "\\.";
            Match f10 = Regex.Match(program, nodeItSelf);
            int v10 = int.Parse(f1.Value);

            string sizeProg = "\\(";
            Match f11 = Regex.Match(program, sizeProg);
            int v11 = int.Parse(f1.Value);

            string numberOp = "EditMap";
            Match f12 = Regex.Match(program, numberOp);
            int v12 = int.Parse(f1.Value);

         //   int[,,,,,,,,,,,] vecFeature = new int[v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12];
            int[] vecFeat = { v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12 };
            return vecFeat;
        }
    }
}

*/

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using System.Text.RegularExpressions;

namespace RefazerFunctions
{
    public class RankingScore : Feature<double>
    {
        public RankingScore(Grammar grammar) : base(grammar, "Score") { }

        public const double VariableScore = 0;

        // Editing EditMap
        [FeatureCalculator("EditMap")]
        public static double Score_EditMap(double scriptScore, double editScore) => -1.5992 + editScore;

        [FeatureCalculator(nameof(Semantics.AllNodes))]
        public static double Score_Traversal(double scriptScore, double editScore) => scriptScore + editScore;

        [FeatureCalculator("EditFilter")]
        public static double Score_EditFilter(double predScore, double splitScore) => (predScore + splitScore) * 1.1;

        [FeatureCalculator(nameof(Semantics.Match))]
        public static double Score_Match(double inSource, double matchScore) => matchScore;

        [FeatureCalculator(nameof(Semantics.SC))]
        public static double Score_CS(double childScore) => -3.9112 + childScore;

        [FeatureCalculator(nameof(Semantics.CList))]
        public static double Score_CList(double childScore, double childrenScore) => -3.9112 + childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.SP))]
        public static double Score_PS(double childScore) => childScore;

        [FeatureCalculator(nameof(Semantics.PList))]
        public static double Score_PList(double childScore, double childrenScore) => childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.SN))]
        public static double Score_SN(double childScore) => -3.9112 + childScore;

        [FeatureCalculator(nameof(Semantics.NList))]
        public static double Score_NList(double childScore, double childrenScore) => -3.9112 + childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.SE))]
        public static double Score_SE(double childScore) => -3.9112 + childScore;

        [FeatureCalculator(nameof(Semantics.EList))]
        public static double Score_EList(double childScore, double childrenScore) => -3.9112 + childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.Transformation), Method = CalculationMethod.FromChildrenFeatureValues)]
        public static double Score_Script1(double inScore, double edit) => 0.12 + edit;
        
        [FeatureCalculator(nameof(Semantics.Insert))]
        public static double Score_Insert(double inScore, double astScore) => inScore + astScore;

        [FeatureCalculator(nameof(Semantics.InsertBefore))]
        public static double Score_InsertBefore(double inScore, double astScore) => inScore + astScore;

        [FeatureCalculator(nameof(Semantics.Update))]
        public static double Score_Update(double inScore, double toScore) => inScore + toScore;

        [FeatureCalculator(nameof(Semantics.Delete))]
        public static double Score_Delete(double inScore, double refscore) => refscore;

        [FeatureCalculator(nameof(Semantics.Node))]
        public static double Score_Node1(double kScore, double astScore) => -3.3629 + astScore;

        [FeatureCalculator(nameof(Semantics.ConstNode))]
        public static double Score_Node1(double astScore) => 4.9968;

        // Editing Asbtract
        [FeatureCalculator(nameof(Semantics.Abstract))]
        public static double Score_Abstract(double kindScore) => 2.7756;

        [FeatureCalculator(nameof(Semantics.Context))]
        public static double Score_ParentP(double matchScore, double kScore) => matchScore;

        [FeatureCalculator(nameof(Semantics.ContextPPP))]
        public static double Score_ParentPPP(double matchScore, double kScore) => matchScore;

        [FeatureCalculator(nameof(Semantics.Concrete))]
        public static double Score_Concrete(double treeScore) => 7.7285;

        [FeatureCalculator(nameof(Semantics.Variable))]
        public static double Score_Variable(double idScore) => idScore;

        [FeatureCalculator(nameof(Semantics.Pattern))]
        public static double Score_Pattern(double kindScore, double expression1Score) => -6.3057 + expression1Score;

        [FeatureCalculator(nameof(Semantics.Reference))]
        public static double Score_Reference(double inScore, double patternScore, double kScore) => -4.9876 + patternScore;

        [FeatureCalculator("id", Method = CalculationMethod.FromLiteral)]
        public static double KDScore(string kd)
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
                return 2.6713;
            }
            else if (f8.Success)
            {
                return 5.9474;
            }
            else if (f9.Success)
            {
                return 0;
            }
            else if (f10.Success) {
                return -2.2392;

            }

                return 0;

        }

        [FeatureCalculator("c", Method = CalculationMethod.FromLiteral)]
        public static double CScore(int c) => 0;

        [FeatureCalculator("kind", Method = CalculationMethod.FromLiteral)]
        public static double KindScore(SyntaxKind kd) => 0;

        [FeatureCalculator("tree", Method = CalculationMethod.FromLiteral)]
        public static double NodeScore(SyntaxNodeOrToken kd) => 0;
    }

}