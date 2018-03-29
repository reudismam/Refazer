////Begining of Manual Ranking Approach

//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.ProgramSynthesis;
//using Microsoft.ProgramSynthesis.AST;
//using System.Text.RegularExpressions;

//namespace RefazerFunctions
//{
//    public class RankingScore : Feature<double>
//    {
//        public RankingScore(Grammar grammar) : base(grammar, "Score") { }

//        public const double VariableScore = 0;

//        [FeatureCalculator("EditMap")]
//        public static double Score_EditMap(double scriptScore, double editScore) => scriptScore + editScore;

//        [FeatureCalculator(nameof(Semantics.AllNodes))]
//        public static double Score_Traversal(double scriptScore, double editScore) => scriptScore + editScore;

//        [FeatureCalculator("EditFilter")]
//        public static double Score_EditFilter(double predScore, double splitScore) => (predScore + splitScore) * 1.1;

//        [FeatureCalculator(nameof(Semantics.Match))]
//        public static double Score_Match(double inSource, double matchScore) => matchScore;

//        [FeatureCalculator(nameof(Semantics.SC))]
//        public static double Score_CS(double childScore) => childScore;

//        [FeatureCalculator(nameof(Semantics.CList))]
//        public static double Score_CList(double childScore, double childrenScore) => childScore + childrenScore;

//        [FeatureCalculator(nameof(Semantics.SP))]
//        public static double Score_PS(double childScore) => childScore;

//        [FeatureCalculator(nameof(Semantics.PList))]
//        public static double Score_PList(double childScore, double childrenScore) => childScore + childrenScore;

//        [FeatureCalculator(nameof(Semantics.SN))]
//        public static double Score_SN(double childScore) => childScore;

//        [FeatureCalculator(nameof(Semantics.NList))]
//        public static double Score_NList(double childScore, double childrenScore) => childScore + childrenScore;

//        [FeatureCalculator(nameof(Semantics.SE))]
//        public static double Score_SE(double childScore) => childScore;

//        [FeatureCalculator(nameof(Semantics.EList))]
//        public static double Score_EList(double childScore, double childrenScore) => childScore + childrenScore;

//        [FeatureCalculator(nameof(Semantics.Transformation), Method = CalculationMethod.FromChildrenFeatureValues)]

//        public static double Score_Script1(double inScore, double edit) => inScore + edit;

//        [FeatureCalculator(nameof(Semantics.Insert))]
//        public static double Score_Insert(double inScore, double astScore) => inScore + astScore;

//        [FeatureCalculator(nameof(Semantics.InsertBefore))]
//        public static double Score_InsertBefore(double inScore, double astScore) => inScore + astScore;

//        [FeatureCalculator(nameof(Semantics.Update))]
//        public static double Score_Update(double inScore, double toScore) => inScore + toScore;

//        [FeatureCalculator(nameof(Semantics.Delete))]
//        public static double Score_Delete(double inScore) => inScore;

//        [FeatureCalculator(nameof(Semantics.Node))]
//        public static double Score_Node1(double kScore, double astScore) => kScore + astScore;

//        [FeatureCalculator(nameof(Semantics.ConstNode))]
//        public static double Score_Node1(double astScore) => astScore;

//        [FeatureCalculator(nameof(Semantics.Abstract))]
//        public static double Score_Abstract(double kindScore) => kindScore;

//        [FeatureCalculator(nameof(Semantics.Context))]
//        public static double Score_ParentP(double matchScore, double kScore) => matchScore;

//        [FeatureCalculator(nameof(Semantics.ContextPPP))]
//        public static double Score_ParentPPP(double matchScore, double kScore) => matchScore;

//        [FeatureCalculator(nameof(Semantics.Concrete))]
//        public static double Score_Concrete(double treeScore) => treeScore * 1000;

//        [FeatureCalculator(nameof(Semantics.Variable))]
//        public static double Score_Variable(double idScore) => idScore;

//        [FeatureCalculator(nameof(Semantics.Pattern))]
//        public static double Score_Pattern(double kindScore, double expression1Score) => kindScore + expression1Score;

//        [FeatureCalculator(nameof(Semantics.Reference))]
//        public static double Score_Reference(double inScore, double patternScore, double kScore) => patternScore;

//        [FeatureCalculator("id", Method = CalculationMethod.FromLiteral)]
//        public static double KDScore(string kd) => 1.1;

//        [FeatureCalculator("c", Method = CalculationMethod.FromLiteral)]
//        public static double CScore(int c) => 1.1;

//        [FeatureCalculator("kind", Method = CalculationMethod.FromLiteral)]
//        public static double KindScore(SyntaxKind kd) => 1.1;

//        [FeatureCalculator("tree", Method = CalculationMethod.FromLiteral)]
//        public static double NodeScore(SyntaxNodeOrToken kd) => 1.1;
//    }

//    public class ExtractFeature
//    {

//        public static int[] Extract(string program)
//        {
//            //put here the R code.
//            string constNode = "constNode";
//            Match f1 = Regex.Match(program, constNode);
//            int v1 = int.Parse(f1.Value);

//            string reference = "Reference";
//            Match f2 = Regex.Match(program, reference);
//            int v2 = int.Parse(f1.Value);

//            string concretePattern = "Concrete";
//            Match f3 = Regex.Match(program, concretePattern);
//            int v3 = int.Parse(f1.Value);

//            string abstractPattern = "Abstract";
//            Match f4 = Regex.Match(program, abstractPattern);
//            int v4 = int.Parse(f1.Value);

//            string node = "Node";
//            Match f5 = Regex.Match(program, node);
//            int v5 = int.Parse(f1.Value);

//            string pattern = "Pattern";
//            Match f6 = Regex.Match(program, pattern);
//            int v6 = int.Parse(f1.Value);

//            string parentOne = "\"\\/\\[[0-9]\\]\"";
//            Match f7 = Regex.Match(program, parentOne);
//            int v7 = int.Parse(f1.Value);

//            string parentTwo = "\"\\/\\[[0-9]\\]\\/\\[[0-9]\\]\"";
//            Match f8 = Regex.Match(program, parentTwo);
//            int v8 = int.Parse(f1.Value);

//            string parentThree = "\"\\/\\[[0-9]\\]\\/\\[[0-9]\\]\\/\\[[0-9]\\]\"";
//            Match f9 = Regex.Match(program, parentThree);
//            int v9 = int.Parse(f1.Value);

//            string nodeItSelf = "\\.";
//            Match f10 = Regex.Match(program, nodeItSelf);
//            int v10 = int.Parse(f1.Value);

//            string sizeProg = "\\(";
//            Match f11 = Regex.Match(program, sizeProg);
//            int v11 = int.Parse(f1.Value);

//            string numberOp = "EditMap";
//            Match f12 = Regex.Match(program, numberOp);
//            int v12 = int.Parse(f1.Value);

//            //   int[,,,,,,,,,,,] vecFeature = new int[v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12];
//            int[] vecFeat = { v1, v2, v3, v4, v5, v6, v7, v8, v9, v10, v11, v12 };
//            return vecFeat;
//        }
//    }
//}

//// End of Manual Ranking Approach

//Ranking Using ML Approach
//-----------------------***----------------------------

//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.ProgramSynthesis;
//using Microsoft.ProgramSynthesis.AST;
//using System.Text.RegularExpressions;

//namespace RefazerFunctions
//{
//    public class RankingScore : Feature<double>
//    {
//        public RankingScore(Grammar grammar) : base(grammar, "Score") { }

//        public const double VariableScore = 0;

//        // Editing EditMap
//        [FeatureCalculator("EditMap")]
//        public static double Score_EditMap(double scriptScore, double editScore) => -3.9112 - 1.5992 + scriptScore + editScore;

//        [FeatureCalculator(nameof(Semantics.AllNodes))]
//        public static double Score_Traversal(double scriptScore, double editScore) => -3.9112 + scriptScore + editScore;

//        [FeatureCalculator("EditFilter")]
//        public static double Score_EditFilter(double predScore, double splitScore) => -3.9112 + (predScore + splitScore);

//        [FeatureCalculator(nameof(Semantics.Match))]
//        public static double Score_Match(double inSource, double matchScore) => -3.9112 + matchScore;

//        [FeatureCalculator(nameof(Semantics.SC))]
//        public static double Score_CS(double childScore) => -3.9112 + childScore;

//        [FeatureCalculator(nameof(Semantics.CList))]
//        public static double Score_CList(double childScore, double childrenScore) => -3.9112 + childScore + childrenScore;

//        [FeatureCalculator(nameof(Semantics.SP))]
//        public static double Score_PS(double childScore) => -3.9112 + childScore;

//        [FeatureCalculator(nameof(Semantics.PList))]
//        public static double Score_PList(double childScore, double childrenScore) => -3.9112 + childScore + childrenScore;

//        [FeatureCalculator(nameof(Semantics.SN))]
//        public static double Score_SN(double childScore) => -3.9112 + childScore;

//        [FeatureCalculator(nameof(Semantics.NList))]
//        public static double Score_NList(double childScore, double childrenScore) => -3.9112 + childScore + childrenScore;

//        [FeatureCalculator(nameof(Semantics.SE))]
//        public static double Score_SE(double childScore) => -3.9112 + childScore;

//        [FeatureCalculator(nameof(Semantics.EList))]
//        public static double Score_EList(double childScore, double childrenScore) => -3.9112 + childScore + childrenScore;

//        [FeatureCalculator(nameof(Semantics.Transformation), Method = CalculationMethod.FromChildrenFeatureValues)]
//        public static double Score_Script1(double inScore, double edit) => -3.9112 + 0.12 + edit;

//        [FeatureCalculator(nameof(Semantics.Insert))]
//        public static double Score_Insert(double inScore, double astScore) => -3.9112 + inScore + astScore;

//        [FeatureCalculator(nameof(Semantics.InsertBefore))]
//        public static double Score_InsertBefore(double inScore, double astScore) => -3.9112 + inScore + astScore;

//        [FeatureCalculator(nameof(Semantics.Update))]
//        public static double Score_Update(double inScore, double toScore) => -3.9112 + inScore + toScore;

//        [FeatureCalculator(nameof(Semantics.Delete))]
//        public static double Score_Delete(double inScore, double refscore) => -3.9112 + refscore;

//        [FeatureCalculator(nameof(Semantics.Node))]
//        public static double Score_Node1(double kScore, double astScore) => -3.9112 - 3.3629 + astScore;

//        [FeatureCalculator(nameof(Semantics.ConstNode))]
//        public static double Score_Node1(double astScore) => -3.9112 + 4.9968;

//        // Editing Abstract
//        [FeatureCalculator(nameof(Semantics.Abstract))]
//        public static double Score_Abstract(double kindScore) => -3.9112 + 2.7756;

//        [FeatureCalculator(nameof(Semantics.Context))]
//        public static double Score_ParentP(double matchScore, double kScore) => -3.9112 + matchScore + kScore;

//        [FeatureCalculator(nameof(Semantics.ContextPPP))]
//        public static double Score_ParentPPP(double matchScore, double kScore) => -3.9112 + matchScore;

//        [FeatureCalculator(nameof(Semantics.Concrete))]
//        public static double Score_Concrete(double treeScore) => -3.9112 + 7.7285;

//        [FeatureCalculator(nameof(Semantics.Variable))]
//        public static double Score_Variable(double idScore) => -3.9112 + idScore;

//        [FeatureCalculator(nameof(Semantics.Pattern))]
//        public static double Score_Pattern(double kindScore, double expression1Score) => -3.9112 - 6.3057 + expression1Score;

//        [FeatureCalculator(nameof(Semantics.Reference))]
//        public static double Score_Reference(double inScore, double patternScore, double kScore) => -3.9112 + -4.9876 + patternScore;

//        [FeatureCalculator("id", Method = CalculationMethod.FromLiteral)]
//        public static double KDScore(string kd)
//        {
//            string parentOne = "\"\\/\\[[0-9]\\]\"";
//            Match f7 = Regex.Match(kd, parentOne);
//            string parentTwo = "\"\\/\\[[0-9]\\]\\/\\[[0-9]\\]\"";
//            Match f8 = Regex.Match(kd, parentTwo);

//            string parentThree = "\"\\/\\[[0-9]\\]\\/\\[[0-9]\\]\\/\\[[0-9]\\]\"";
//            Match f9 = Regex.Match(kd, parentThree);


//            string nodeItSelf = "\\.";
//            Match f10 = Regex.Match(kd, nodeItSelf);


//            if (f7.Success)
//            {
//                return 2.6713;
//            }
//            else if (f8.Success)
//            {
//                return 5.9474;
//            }
//            else if (f9.Success)
//            {
//                return 0;
//            }
//            else if (f10.Success)
//            {
//                return -2.2392;

//            }

//            return 0;

//        }

//        [FeatureCalculator("c", Method = CalculationMethod.FromLiteral)]
//        public static double CScore(int c) => 0;

//        [FeatureCalculator("kind", Method = CalculationMethod.FromLiteral)]
//        public static double KindScore(SyntaxKind kd) => 0;

//        [FeatureCalculator("tree", Method = CalculationMethod.FromLiteral)]
//        public static double NodeScore(SyntaxNodeOrToken kd) => 0;
//    }
//}

// End of ML Approach
// ---------------***------------


////Begining of New Training Data ML Approach

//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.ProgramSynthesis;
//using Microsoft.ProgramSynthesis.AST;
//using System.Text.RegularExpressions;

//namespace RefazerFunctions
//{
//    public class RankingScore : Feature<double>
//    {
//        public RankingScore(Grammar grammar) : base(grammar, "Score") { }

//        public const double VariableScore = 0;

//        // Editing EditMap
//        [FeatureCalculator("EditMap")]
//        public static double Score_EditMap(double scriptScore, double editScore) => -0.6406 + 23.7911 + scriptScore + editScore;

//        [FeatureCalculator(nameof(Semantics.AllNodes))]
//        public static double Score_Traversal(double scriptScore, double editScore) => -0.6406 + scriptScore + editScore;

//        [FeatureCalculator("EditFilter")]
//        public static double Score_EditFilter(double predScore, double splitScore) => -0.6406 + (predScore + splitScore);

//        [FeatureCalculator(nameof(Semantics.Match))]
//        public static double Score_Match(double inSource, double matchScore) => -0.6406 + matchScore;

//        [FeatureCalculator(nameof(Semantics.SC))]
//        public static double Score_CS(double childScore) => -0.6406 + childScore;

//        [FeatureCalculator(nameof(Semantics.CList))]
//        public static double Score_CList(double childScore, double childrenScore) => -0.6406 + childScore + childrenScore;

//        [FeatureCalculator(nameof(Semantics.SP))]
//        public static double Score_PS(double childScore) => -0.6406 + childScore;

//        [FeatureCalculator(nameof(Semantics.PList))]
//        public static double Score_PList(double childScore, double childrenScore) => -0.6406 + childScore + childrenScore;

//        [FeatureCalculator(nameof(Semantics.SN))]
//        public static double Score_SN(double childScore) => -0.6406 + childScore;

//        [FeatureCalculator(nameof(Semantics.NList))]
//        public static double Score_NList(double childScore, double childrenScore) => -0.6406 + childScore + childrenScore;

//        [FeatureCalculator(nameof(Semantics.SE))]
//        public static double Score_SE(double childScore) => -0.6406 + childScore;

//        [FeatureCalculator(nameof(Semantics.EList))]
//        public static double Score_EList(double childScore, double childrenScore) => -0.6406 + childScore + childrenScore;

//        [FeatureCalculator(nameof(Semantics.Transformation), Method = CalculationMethod.FromChildrenFeatureValues)]
//        public static double Score_Script1(double inScore, double edit) => -0.6406 - 21.0728 + edit;

//        [FeatureCalculator(nameof(Semantics.Insert))]
//        public static double Score_Insert(double inScore, double astScore) => -0.6406 + inScore + astScore;

//        [FeatureCalculator(nameof(Semantics.InsertBefore))]
//        public static double Score_InsertBefore(double inScore, double astScore) => -0.6406 + inScore + astScore;

//        [FeatureCalculator(nameof(Semantics.Update))]
//        public static double Score_Update(double inScore, double toScore) => -0.6406 + inScore + toScore;

//        [FeatureCalculator(nameof(Semantics.Delete))]
//        public static double Score_Delete(double inScore, double refscore) => -0.6406 + refscore;

//        [FeatureCalculator(nameof(Semantics.Node))]
//        public static double Score_Node1(double kScore, double astScore) => -0.6406 + 1.1386 + astScore;

//        [FeatureCalculator(nameof(Semantics.ConstNode))]
//        public static double Score_Node1(double astScore) => -0.6406 + 1.0916;

//        // Editing Abstract
//        [FeatureCalculator(nameof(Semantics.Abstract))]
//        public static double Score_Abstract(double kindScore) => -0.6406 + 0.9729;

//        [FeatureCalculator(nameof(Semantics.Context))]
//        public static double Score_ParentP(double matchScore, double kScore) => -0.6406 + matchScore + kScore;

//        [FeatureCalculator(nameof(Semantics.ContextPPP))]
//        public static double Score_ParentPPP(double matchScore, double kScore) => -0.6406 + matchScore;

//        [FeatureCalculator(nameof(Semantics.Concrete))]
//        public static double Score_Concrete(double treeScore) => -0.6406 + 1.2855;

//        [FeatureCalculator(nameof(Semantics.Variable))]
//        public static double Score_Variable(double idScore) => -0.6406 + idScore;

//        [FeatureCalculator(nameof(Semantics.Pattern))]
//        public static double Score_Pattern(double kindScore, double expression1Score) => -0.6406 + 1.3718 + expression1Score;

//        [FeatureCalculator(nameof(Semantics.Reference))]
//        public static double Score_Reference(double inScore, double patternScore, double kScore) => -0.6406 + 1.3481 + patternScore;

//        [FeatureCalculator("id", Method = CalculationMethod.FromLiteral)]
//        public static double KDScore(string kd)
//        {
//            string parentOne = "\"\\/\\[[0-9]\\]\"";
//            Match f7 = Regex.Match(kd, parentOne);
//            string parentTwo = "\"\\/\\[[0-9]\\]\\/\\[[0-9]\\]\"";
//            Match f8 = Regex.Match(kd, parentTwo);

//            string parentThree = "\"\\/\\[[0-9]\\]\\/\\[[0-9]\\]\\/\\[[0-9]\\]\"";
//            Match f9 = Regex.Match(kd, parentThree);


//            string nodeItSelf = "\\.";
//            Match f10 = Regex.Match(kd, nodeItSelf);


//            if (f7.Success)
//            {
//                return -0.3184;
//            }
//            else if (f8.Success)
//            {
//                return 0;
//            }
//            else if (f9.Success)
//            {
//                return 0;
//            }
//            else if (f10.Success)
//            {
//                return -1.1743;

//            }

//            return 0;

//        }

//        [FeatureCalculator("c", Method = CalculationMethod.FromLiteral)]
//        public static double CScore(int c) => 0;

//        [FeatureCalculator("kind", Method = CalculationMethod.FromLiteral)]
//        public static double KindScore(SyntaxKind kd) => 0;

//        [FeatureCalculator("tree", Method = CalculationMethod.FromLiteral)]
//        public static double NodeScore(SyntaxNodeOrToken kd) => 0;
//    }
//}

//// End of New Training Data ML Approach

//// Begining of Random Approach

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using System.Text.RegularExpressions;
using System;
using RefazerFunctions.Spg.Ranking;

namespace RefazerFunctions
{

    public class RankingScore : Feature<double>
    {
        public RankingScore(Grammar grammar) : base(grammar, "Score") { }

        private static RankingFunction ranking = new RandomRanking();
       // private static RankingFunction ranking = new ManualRanking();
        //private static RankingFunction ranking = new MLRanking();

        // Editing EditMap
        [FeatureCalculator("EditMap")]
        public static double Score_EditMap(double scriptScore, double editScore) => ranking.Score_EditMap(scriptScore, editScore);

        [FeatureCalculator(nameof(Semantics.AllNodes))]
        public static double Score_Traversal(double scriptScore, double editScore) => ranking.Score_Traversal(scriptScore, editScore);

        [FeatureCalculator("EditFilter")]
        public static double Score_EditFilter(double predScore, double splitScore) => ranking.Score_EditFilter(predScore, splitScore);

        [FeatureCalculator(nameof(Semantics.Match))]
        public static double Score_Match(double inSource, double matchScore) => ranking.Score_Match(inSource, matchScore);

        [FeatureCalculator(nameof(Semantics.SC))]
        public static double Score_CS(double childScore) => ranking.Score_CS(childScore);

        [FeatureCalculator(nameof(Semantics.CList))]
        public static double Score_CList(double childScore, double childrenScore) => ranking.Score_CList(childScore, childrenScore);

        [FeatureCalculator(nameof(Semantics.SP))]
        public static double Score_PS(double childScore) => ranking.Score_PS(childScore);

        [FeatureCalculator(nameof(Semantics.PList))]
        public static double Score_PList(double childScore, double childrenScore) => ranking.Score_PList(childScore, childrenScore);

        [FeatureCalculator(nameof(Semantics.SN))]
        public static double Score_SN(double childScore) => ranking.Score_SN(childScore);

        [FeatureCalculator(nameof(Semantics.NList))]
        public static double Score_NList(double childScore, double childrenScore) => ranking.Score_NList(childScore, childrenScore);

        [FeatureCalculator(nameof(Semantics.SE))]
        public static double Score_SE(double childScore) => ranking.Score_SE(childScore);

        [FeatureCalculator(nameof(Semantics.EList))]
        public static double Score_EList(double childScore, double childrenScore) => ranking.Score_EList(childScore, childrenScore);

        [FeatureCalculator(nameof(Semantics.Transformation), Method = CalculationMethod.FromChildrenFeatureValues)]
        public static double Score_Script1(double inScore, double edit) => ranking.Score_Script1(inScore, edit);

        [FeatureCalculator(nameof(Semantics.Insert))]
        public static double Score_Insert(double inScore, double astScore) => ranking.Score_Insert(inScore, astScore);

        [FeatureCalculator(nameof(Semantics.InsertBefore))]
        public static double Score_InsertBefore(double inScore, double astScore) => ranking.Score_InsertBefore(inScore, astScore);

        [FeatureCalculator(nameof(Semantics.Update))]
        public static double Score_Update(double inScore, double toScore) => ranking.Score_Update(inScore, toScore);

        [FeatureCalculator(nameof(Semantics.Delete))]
        public static double Score_Delete(double inScore, double refscore) => ranking.Score_Delete(inScore, refscore);

        [FeatureCalculator(nameof(Semantics.Node))]
        public static double Score_Node1(double kScore, double astScore) => ranking.Score_Node1(kScore, astScore);

        [FeatureCalculator(nameof(Semantics.ConstNode))]
        public static double Score_Node1(double astScore) => ranking.Score_Node1(astScore);

        // Editing Abstract
        [FeatureCalculator(nameof(Semantics.Abstract))]
        public static double Score_Abstract(double kindScore) => ranking.Score_Abstract(kindScore);

        [FeatureCalculator(nameof(Semantics.Context))]
        public static double Score_ParentP(double matchScore, double kScore) => ranking.Score_ParentP(matchScore, kScore);

        [FeatureCalculator(nameof(Semantics.ContextPPP))]
        public static double Score_ParentPPP(double matchScore, double kScore) => ranking.Score_ParentPPP(matchScore, kScore);

        [FeatureCalculator(nameof(Semantics.Concrete))]
        public static double Score_Concrete(double treeScore) => ranking.Score_Concrete(treeScore);

        [FeatureCalculator(nameof(Semantics.Variable))]
        public static double Score_Variable(double idScore) => ranking.Score_Variable(idScore);

        [FeatureCalculator(nameof(Semantics.Pattern))]
        public static double Score_Pattern(double kindScore, double expression1Score) => ranking.Score_Pattern(kindScore, expression1Score);


        [FeatureCalculator(nameof(Semantics.Reference))]
        public static double Score_Reference(double inScore, double patternScore, double kScore) => ranking.Score_Reference(inScore, patternScore, kScore);

        [FeatureCalculator("id", Method = CalculationMethod.FromLiteral)]
        public static double KDScore(string kd) => ranking.KDScore(kd);

        [FeatureCalculator("c", Method = CalculationMethod.FromLiteral)]
        public static double CScore(int c) => ranking.CScore(c);

        [FeatureCalculator("kind", Method = CalculationMethod.FromLiteral)]
        public static double KindScore(SyntaxKind kd) => ranking.KindScore(kd);

        [FeatureCalculator("tree", Method = CalculationMethod.FromLiteral)]
        public static double NodeScore(SyntaxNodeOrToken kd) => ranking.NodeScore(kd);
    }
}
////End of Random Ranking Approach

//// Begining of Null Approach

//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.ProgramSynthesis;
//using Microsoft.ProgramSynthesis.AST;
//using System.Text.RegularExpressions;
//using System;

//namespace RefazerFunctions
//{

//    public class RankingScore : Feature<double>
//    {
//        public RankingScore(Grammar grammar) : base(grammar, "Score") { }

//        public static int seed = 86028157;
//        public const double VariableScore = 0;
//     //   public static Random rnd = new Random(seed);

//        public static double temp = 0;

//        // Editing EditMap
//        [FeatureCalculator("EditMap")]
//        public static double Score_EditMap(double scriptScore, double editScore) => 0 + editScore;

//        [FeatureCalculator(nameof(Semantics.AllNodes))]
//        public static double Score_Traversal(double scriptScore, double editScore) => scriptScore + editScore;

//        [FeatureCalculator("EditFilter")]
//        public static double Score_EditFilter(double predScore, double splitScore) => (predScore + splitScore) * 1.1;

//        [FeatureCalculator(nameof(Semantics.Match))]
//        public static double Score_Match(double inSource, double matchScore) => matchScore;

//        [FeatureCalculator(nameof(Semantics.SC))]
//        public static double Score_CS(double childScore) => temp + childScore;

//        [FeatureCalculator(nameof(Semantics.CList))]
//        public static double Score_CList(double childScore, double childrenScore) => temp + childScore + childrenScore;

//        [FeatureCalculator(nameof(Semantics.SP))]
//        public static double Score_PS(double childScore) => childScore;

//        [FeatureCalculator(nameof(Semantics.PList))]
//        public static double Score_PList(double childScore, double childrenScore) => childScore + childrenScore;

//        [FeatureCalculator(nameof(Semantics.SN))]
//        public static double Score_SN(double childScore) => temp + childScore;

//        [FeatureCalculator(nameof(Semantics.NList))]
//        public static double Score_NList(double childScore, double childrenScore) => temp + childScore + childrenScore;

//        [FeatureCalculator(nameof(Semantics.SE))]
//        public static double Score_SE(double childScore) => temp + childScore;

//        [FeatureCalculator(nameof(Semantics.EList))]
//        public static double Score_EList(double childScore, double childrenScore) => temp + childScore + childrenScore;

//        [FeatureCalculator(nameof(Semantics.Transformation), Method = CalculationMethod.FromChildrenFeatureValues)]
//        public static double Score_Script1(double inScore, double edit) => 0 + edit;

//        [FeatureCalculator(nameof(Semantics.Insert))]
//        public static double Score_Insert(double inScore, double astScore) => inScore + astScore;

//        [FeatureCalculator(nameof(Semantics.InsertBefore))]
//        public static double Score_InsertBefore(double inScore, double astScore) => inScore + astScore;

//        [FeatureCalculator(nameof(Semantics.Update))]
//        public static double Score_Update(double inScore, double toScore) => inScore + toScore;

//        [FeatureCalculator(nameof(Semantics.Delete))]
//        public static double Score_Delete(double inScore, double refscore) => refscore;

//        [FeatureCalculator(nameof(Semantics.Node))]
//        public static double Score_Node1(double kScore, double astScore) => 0 + astScore;

//        [FeatureCalculator(nameof(Semantics.ConstNode))]
//        public static double Score_Node1(double astScore) => 0;

//        // Editing Abstract
//        [FeatureCalculator(nameof(Semantics.Abstract))]
//        public static double Score_Abstract(double kindScore) => 0;

//        [FeatureCalculator(nameof(Semantics.Context))]
//        public static double Score_ParentP(double matchScore, double kScore) => matchScore;

//        [FeatureCalculator(nameof(Semantics.ContextPPP))]
//        public static double Score_ParentPPP(double matchScore, double kScore) => matchScore;

//        [FeatureCalculator(nameof(Semantics.Concrete))]
//        public static double Score_Concrete(double treeScore) => 0;

//        [FeatureCalculator(nameof(Semantics.Variable))]
//        public static double Score_Variable(double idScore) => idScore;

//        [FeatureCalculator(nameof(Semantics.Pattern))]
//        public static double Score_Pattern(double kindScore, double expression1Score) => 0 + expression1Score;


//        [FeatureCalculator(nameof(Semantics.Reference))]
//        public static double Score_Reference(double inScore, double patternScore, double kScore) => 0 + patternScore;

//        [FeatureCalculator("id", Method = CalculationMethod.FromLiteral)]
//        public static double KDScore(string kd)
//        {
//            string parentOne = "\"\\/\\[[0-9]\\]\"";
//            Match f7 = Regex.Match(kd, parentOne);
//            string parentTwo = "\"\\/\\[[0-9]\\]\\/\\[[0-9]\\]\"";
//            Match f8 = Regex.Match(kd, parentTwo);

//            string parentThree = "\"\\/\\[[0-9]\\]\\/\\[[0-9]\\]\\/\\[[0-9]\\]\"";
//            Match f9 = Regex.Match(kd, parentThree);


//            string nodeItSelf = "\\.";
//            Match f10 = Regex.Match(kd, nodeItSelf);


//            if (f7.Success)
//            {
//                return 0;
//            }
//            else if (f8.Success)
//            {
//                return 0;
//            }
//            else if (f9.Success)
//            {
//                return 0;
//            }
//            else if (f10.Success)
//            {
//                return 0;

//            }

//            return 0;

//        }

//        [FeatureCalculator("c", Method = CalculationMethod.FromLiteral)]
//        public static double CScore(int c) => 0;

//        [FeatureCalculator("kind", Method = CalculationMethod.FromLiteral)]
//        public static double KindScore(SyntaxKind kd) => 0;

//        [FeatureCalculator("tree", Method = CalculationMethod.FromLiteral)]
//        public static double NodeScore(SyntaxNodeOrToken kd) => 0;
//    }
//}
////End of Null Ranking Approach


//// Begining of Wide-Random Approach

//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.ProgramSynthesis;
//using Microsoft.ProgramSynthesis.AST;
//using System.Text.RegularExpressions;
//using System;

//namespace RefazerFunctions
//{

//    public class RankingScore : Feature<double>
//    {
//        public RankingScore(Grammar grammar) : base(grammar, "Score") { }

//        public static int seed = 86028157;
//        public const double VariableScore = 0;
//        public static Random rnd = new Random(seed);


//        public static double GetRandom(double min, double max)
//        {
//            return rnd.NextDouble() * (max - min) + min;
//        }
//        public static double temp = GetRandom(-1000, 1000);

//        // Editing EditMap
//        [FeatureCalculator("EditMap")]
//        public static double Score_EditMap(double scriptScore, double editScore) => GetRandom(-1000, 1000) + editScore;

//        [FeatureCalculator(nameof(Semantics.AllNodes))]
//        public static double Score_Traversal(double scriptScore, double editScore) => scriptScore + editScore;

//        [FeatureCalculator("EditFilter")]
//        public static double Score_EditFilter(double predScore, double splitScore) => (predScore + splitScore) * 1.1;

//        [FeatureCalculator(nameof(Semantics.Match))]
//        public static double Score_Match(double inSource, double matchScore) => matchScore;

//        [FeatureCalculator(nameof(Semantics.SC))]
//        public static double Score_CS(double childScore) => temp + childScore;

//        [FeatureCalculator(nameof(Semantics.CList))]
//        public static double Score_CList(double childScore, double childrenScore) => temp + childScore + childrenScore;

//        [FeatureCalculator(nameof(Semantics.SP))]
//        public static double Score_PS(double childScore) => childScore;

//        [FeatureCalculator(nameof(Semantics.PList))]
//        public static double Score_PList(double childScore, double childrenScore) => childScore + childrenScore;

//        [FeatureCalculator(nameof(Semantics.SN))]
//        public static double Score_SN(double childScore) => temp + childScore;

//        [FeatureCalculator(nameof(Semantics.NList))]
//        public static double Score_NList(double childScore, double childrenScore) => temp + childScore + childrenScore;

//        [FeatureCalculator(nameof(Semantics.SE))]
//        public static double Score_SE(double childScore) => temp + childScore;

//        [FeatureCalculator(nameof(Semantics.EList))]
//        public static double Score_EList(double childScore, double childrenScore) => temp + childScore + childrenScore;

//        [FeatureCalculator(nameof(Semantics.Transformation), Method = CalculationMethod.FromChildrenFeatureValues)]
//        public static double Score_Script1(double inScore, double edit) => GetRandom(-1000, 1000) + edit;

//        [FeatureCalculator(nameof(Semantics.Insert))]
//        public static double Score_Insert(double inScore, double astScore) => inScore + astScore;

//        [FeatureCalculator(nameof(Semantics.InsertBefore))]
//        public static double Score_InsertBefore(double inScore, double astScore) => inScore + astScore;

//        [FeatureCalculator(nameof(Semantics.Update))]
//        public static double Score_Update(double inScore, double toScore) => inScore + toScore;

//        [FeatureCalculator(nameof(Semantics.Delete))]
//        public static double Score_Delete(double inScore, double refscore) => refscore;

//        [FeatureCalculator(nameof(Semantics.Node))]
//        public static double Score_Node1(double kScore, double astScore) => GetRandom(-1000, 1000) + astScore;

//        [FeatureCalculator(nameof(Semantics.ConstNode))]
//        public static double Score_Node1(double astScore) => GetRandom(-1000, 1000);

//        // Editing Abstract
//        [FeatureCalculator(nameof(Semantics.Abstract))]
//        public static double Score_Abstract(double kindScore) => GetRandom(-1000, 1000);

//        [FeatureCalculator(nameof(Semantics.Context))]
//        public static double Score_ParentP(double matchScore, double kScore) => matchScore;

//        [FeatureCalculator(nameof(Semantics.ContextPPP))]
//        public static double Score_ParentPPP(double matchScore, double kScore) => matchScore;

//        [FeatureCalculator(nameof(Semantics.Concrete))]
//        public static double Score_Concrete(double treeScore) => GetRandom(-1000, 1000);

//        [FeatureCalculator(nameof(Semantics.Variable))]
//        public static double Score_Variable(double idScore) => idScore;

//        [FeatureCalculator(nameof(Semantics.Pattern))]
//        public static double Score_Pattern(double kindScore, double expression1Score) => GetRandom(-1000, 1000) + expression1Score;


//        [FeatureCalculator(nameof(Semantics.Reference))]
//        public static double Score_Reference(double inScore, double patternScore, double kScore) => GetRandom(-1000, 1000) + patternScore;

//        [FeatureCalculator("id", Method = CalculationMethod.FromLiteral)]
//        public static double KDScore(string kd)
//        {
//            string parentOne = "\"\\/\\[[0-9]\\]\"";
//            Match f7 = Regex.Match(kd, parentOne);
//            string parentTwo = "\"\\/\\[[0-9]\\]\\/\\[[0-9]\\]\"";
//            Match f8 = Regex.Match(kd, parentTwo);

//            string parentThree = "\"\\/\\[[0-9]\\]\\/\\[[0-9]\\]\\/\\[[0-9]\\]\"";
//            Match f9 = Regex.Match(kd, parentThree);


//            string nodeItSelf = "\\.";
//            Match f10 = Regex.Match(kd, nodeItSelf);


//            if (f7.Success)
//            {
//                return GetRandom(-1000, 1000);
//            }
//            else if (f8.Success)
//            {
//                return GetRandom(-1000, 1000);
//            }
//            else if (f9.Success)
//            {
//                return 0;
//            }
//            else if (f10.Success)
//            {
//                return GetRandom(-1000, 1000);

//            }

//            return 0;

//        }

//        [FeatureCalculator("c", Method = CalculationMethod.FromLiteral)]
//        public static double CScore(int c) => 0;

//        [FeatureCalculator("kind", Method = CalculationMethod.FromLiteral)]
//        public static double KindScore(SyntaxKind kd) => 0;

//        [FeatureCalculator("tree", Method = CalculationMethod.FromLiteral)]
//        public static double NodeScore(SyntaxNodeOrToken kd) => 0;
//    }
//}
////End of Wide-Random Ranking Approach
