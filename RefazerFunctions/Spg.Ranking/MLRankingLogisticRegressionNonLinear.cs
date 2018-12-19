﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RefazerFunctions.Bean;

namespace RefazerFunctions.Spg.Ranking
{
    public class MLRankingLogisticRegressionNonLinear: RankingFunction
    {
        private static Dictionary<Feature, double> weights;

        public MLRankingLogisticRegressionNonLinear()
        {
            // RegLog with feature "Concrete"
            //weights = new Dictionary<Feature, double>();
            //weights.Add(Feature.Intercept, 7.9675);
            //weights.Add(Feature.Constants, 0.7741);
            //weights.Add(Feature.References, -6.2142);
            //weights.Add(Feature.Concrete, 0.1887);
            //weights.Add(Feature.Abstract, -2.7572);
            //weights.Add(Feature.Nodes, -1.5912);
            //weights.Add(Feature.Patterns, -0.6199);
            //weights.Add(Feature.ParentOne, -1.2385);
            //weights.Add(Feature.ParentTwo, 0);
            //weights.Add(Feature.ParentThree, 0);
            //weights.Add(Feature.NodeItSelf, 0.8754);
            //weights.Add(Feature.Size, 0.0691);
            //weights.Add(Feature.Operations, 1.5204);
            //weights.Add(Feature.Constants_squared, 1.1304);
            //weights.Add(Feature.References_squared, 0);
            //weights.Add(Feature.Concrete_squared, -0.2242);
            //weights.Add(Feature.Abstract_squared, 0.0570);
            //weights.Add(Feature.Nodes_squared, -0.4708);
            //weights.Add(Feature.Patterns_squared, 0.0842);
            //weights.Add(Feature.ParentOne_squared, -2.5644);
            //weights.Add(Feature.ParentTwo_squared, 0);
            //weights.Add(Feature.ParentThree_squared, 0);
            //weights.Add(Feature.NodeItSelf_squared, -0.9989);
            //weights.Add(Feature.Size_squared, 0.0039);
            //weights.Add(Feature.Operations_squared, -0.0003);
            //weights.Add(Feature.Constants_cubic, 1.3519);
            //weights.Add(Feature.References_cubic, 2.1027);
            //weights.Add(Feature.Concrete_cubic, -0.0415);
            //weights.Add(Feature.Abstract_cubic, 0.1159);
            //weights.Add(Feature.Nodes_cubic, 0.0146);
            //weights.Add(Feature.Patterns_cubic, -0.0077);
            //weights.Add(Feature.ParentOne_cubic, 1.2704);
            //weights.Add(Feature.ParentTwo_cubic, 0);
            //weights.Add(Feature.ParentThree_cubic, 0);
            //weights.Add(Feature.NodeItSelf_cubic, 0.1183);
            //weights.Add(Feature.Size_cubic, 0.00005);
            //weights.Add(Feature.Operations_cubic, -1.2098);

            //... colocar as demais

            // RegLog without feature "Concrete"

            weights = new Dictionary<Feature, double>();
            weights.Add(Feature.Intercept, 4.4994);
            weights.Add(Feature.Constants, 0.0264);
            weights.Add(Feature.References, -3.7430);
            weights.Add(Feature.Concrete, 0); //Set to Zero
            weights.Add(Feature.Abstract, -0.3361);
            weights.Add(Feature.Nodes, -1.0702);
            weights.Add(Feature.Patterns, 0.4059);
            weights.Add(Feature.ParentOne, -1.9732);
            weights.Add(Feature.ParentTwo, 0);
            weights.Add(Feature.ParentThree, 0);
            weights.Add(Feature.NodeItSelf, 0.4731);
            weights.Add(Feature.Size, -0.0647);
            weights.Add(Feature.Operations, 2.3750);
            weights.Add(Feature.Constants_squared, -0.1472);
            weights.Add(Feature.References_squared, 0);
            weights.Add(Feature.Abstract_squared, -0.6482);
            weights.Add(Feature.Nodes_squared, -0.1073);
            weights.Add(Feature.Patterns_squared, 0.0446);
            weights.Add(Feature.ParentOne_squared, -0.0029);
            weights.Add(Feature.ParentTwo_squared, 0);
            weights.Add(Feature.ParentThree_squared, 0);
            weights.Add(Feature.NodeItSelf_squared, -0.3162);
            weights.Add(Feature.Size_squared, -0.0024);
            weights.Add(Feature.Operations_squared, 0.2977);
            weights.Add(Feature.Constants_cubic, 0.1184);
            weights.Add(Feature.References_cubic, 0.1660);
            weights.Add(Feature.Abstract_cubic, 0.2279);
            weights.Add(Feature.Nodes_cubic, 0.0224);
            weights.Add(Feature.Patterns_cubic, -0.0003);
            weights.Add(Feature.ParentOne_cubic, 0.3431);
            weights.Add(Feature.ParentTwo_cubic, 0);
            weights.Add(Feature.ParentThree_cubic, 0);
            weights.Add(Feature.NodeItSelf_cubic, -0.0266);
            weights.Add(Feature.Size_cubic, -0.00002);
            weights.Add(Feature.Operations_cubic, -0.1194);

        }

        // Editing EditMap
        [FeatureCalculator("EditMap")]
        public double Score_EditMap(double scriptScore, double editScore) => weights[Feature.Size] + weights[Feature.Operations] + scriptScore + editScore;

        [FeatureCalculator(nameof(Semantics.AllNodes))]
        public double Score_Traversal(double scriptScore, double editScore) => weights[Feature.Size] + scriptScore + editScore;

        [FeatureCalculator("EditFilter")]
        public double Score_EditFilter(double predScore, double splitScore) => weights[Feature.Size] + (predScore + splitScore);

        [FeatureCalculator(nameof(Semantics.Match))]
        public double Score_Match(double inSource, double matchScore) => weights[Feature.Size] + matchScore;

        [FeatureCalculator(nameof(Semantics.SC))]
        public double Score_CS(double childScore) => weights[Feature.Size] + childScore;

        [FeatureCalculator(nameof(Semantics.CList))]
        public double Score_CList(double childScore, double childrenScore) => weights[Feature.Size] + childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.SP))]
        public double Score_PS(double childScore) => weights[Feature.Size] + childScore;

        [FeatureCalculator(nameof(Semantics.PList))]
        public double Score_PList(double childScore, double childrenScore) => weights[Feature.Size] + childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.SN))]
        public double Score_SN(double childScore) => weights[Feature.Size] + childScore;

        [FeatureCalculator(nameof(Semantics.NList))]
        public double Score_NList(double childScore, double childrenScore) => weights[Feature.Size] + childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.SE))]
        public double Score_SE(double childScore) => weights[Feature.Size] + childScore;

        [FeatureCalculator(nameof(Semantics.EList))]
        public double Score_EList(double childScore, double childrenScore) => weights[Feature.Size] + childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.Transformation), Method = CalculationMethod.FromChildrenFeatureValues)]
        public double Score_Script1(double inScore, double edit)
        {
            // With feature "Concrete"
            //    Dictionary<Feature, int>  features = RankingScore.getFeatures();
            //    return weights[Feature.Intercept] + 
            //        weights[Feature.Constants] * features[Feature.Constants] + 
            //        weights[Feature.References] * features[Feature.References]+
            //        weights[Feature.Concrete] * features[Feature.Concrete]+
            //        weights[Feature.Abstract] * features[Feature.Abstract]+
            //        weights[Feature.Nodes] * features[Feature.Nodes]+
            //        weights[Feature.Patterns] * features[Feature.Patterns]+
            //        weights[Feature.ParentOne] * features[Feature.ParentOne]+
            //        weights[Feature.ParentTwo] * features[Feature.ParentTwo]+
            //        weights[Feature.ParentThree] * features[Feature.ParentThree]+
            //        weights[Feature.NodeItSelf] * features[Feature.NodeItSelf]+
            //        weights[Feature.Size] * features[Feature.Size]+
            //        weights[Feature.Operations] * features[Feature.Operations] +
            //        weights[Feature.Constants_squared] * features[Feature.Constants] * features[Feature.Constants] +
            //        weights[Feature.References_squared] * features[Feature.References] * features[Feature.References] +
            //        weights[Feature.Concrete_squared] * features[Feature.Concrete] * features[Feature.Concrete] +
            //        weights[Feature.Abstract_squared] * features[Feature.Abstract] * features[Feature.Abstract] +
            //        weights[Feature.Nodes_squared] * features[Feature.Nodes] * features[Feature.Nodes] +
            //        weights[Feature.Patterns_squared] * features[Feature.Patterns] * features[Feature.Patterns] +
            //        weights[Feature.ParentOne_squared] * features[Feature.ParentOne] * features[Feature.ParentOne] +
            //        weights[Feature.ParentTwo_squared] * features[Feature.ParentTwo] * features[Feature.ParentTwo] +
            //        weights[Feature.ParentThree_squared] * features[Feature.ParentThree] * features[Feature.ParentThree] +
            //        weights[Feature.NodeItSelf_squared] * features[Feature.NodeItSelf] * features[Feature.NodeItSelf] +
            //        weights[Feature.Size_squared] * features[Feature.Size] * features[Feature.Size] +
            //        weights[Feature.Operations_squared] * features[Feature.Operations] * features[Feature.Operations] +
            //        weights[Feature.Constants_cubic] * features[Feature.Constants] * features[Feature.Constants] * features[Feature.Constants] +
            //        weights[Feature.References_cubic] * features[Feature.References] * features[Feature.References] * features[Feature.References] +
            //        weights[Feature.Concrete_cubic] * features[Feature.Concrete] * features[Feature.Concrete] * features[Feature.Concrete] +
            //        weights[Feature.Abstract_cubic] * features[Feature.Abstract] * features[Feature.Abstract] * features[Feature.Abstract] +
            //        weights[Feature.Nodes_cubic] * features[Feature.Nodes] * features[Feature.Nodes] * features[Feature.Nodes] +
            //        weights[Feature.Patterns_cubic] * features[Feature.Patterns] * features[Feature.Patterns] * features[Feature.Patterns] +
            //        weights[Feature.ParentOne_cubic] * features[Feature.ParentOne] * features[Feature.ParentOne] * features[Feature.ParentOne] +
            //        weights[Feature.ParentTwo_cubic] * features[Feature.ParentTwo] * features[Feature.ParentTwo] * features[Feature.ParentTwo] +
            //        weights[Feature.ParentThree_cubic] * features[Feature.ParentThree] * features[Feature.ParentThree] * features[Feature.ParentThree] +
            //        weights[Feature.NodeItSelf_cubic] * features[Feature.NodeItSelf] * features[Feature.NodeItSelf] * features[Feature.NodeItSelf] +
            //        weights[Feature.Size_cubic] * features[Feature.Size] * features[Feature.Size] * features[Feature.Size] +
            //        weights[Feature.Operations_cubic] * features[Feature.Operations] * features[Feature.Operations] * features[Feature.Operations];
            //

            Dictionary<Feature, int> features = RankingScore.getFeatures();
            return weights[Feature.Intercept] +
                weights[Feature.Constants] * features[Feature.Constants] +
                weights[Feature.References] * features[Feature.References] +
                weights[Feature.Concrete] * features[Feature.Concrete] +
                weights[Feature.Abstract] * features[Feature.Abstract] +
                weights[Feature.Nodes] * features[Feature.Nodes] +
                weights[Feature.Patterns] * features[Feature.Patterns] +
                weights[Feature.ParentOne] * features[Feature.ParentOne] +
                weights[Feature.ParentTwo] * features[Feature.ParentTwo] +
                weights[Feature.ParentThree] * features[Feature.ParentThree] +
                weights[Feature.NodeItSelf] * features[Feature.NodeItSelf] +
                weights[Feature.Size] * features[Feature.Size] +
                weights[Feature.Operations] * features[Feature.Operations] +
                weights[Feature.Constants_squared] * features[Feature.Constants] * features[Feature.Constants] +
                weights[Feature.References_squared] * features[Feature.References] * features[Feature.References] +
                weights[Feature.Abstract_squared] * features[Feature.Abstract] * features[Feature.Abstract] +
                weights[Feature.Nodes_squared] * features[Feature.Nodes] * features[Feature.Nodes] +
                weights[Feature.Patterns_squared] * features[Feature.Patterns] * features[Feature.Patterns] +
                weights[Feature.ParentOne_squared] * features[Feature.ParentOne] * features[Feature.ParentOne] +
                weights[Feature.ParentTwo_squared] * features[Feature.ParentTwo] * features[Feature.ParentTwo] +
                weights[Feature.ParentThree_squared] * features[Feature.ParentThree] * features[Feature.ParentThree] +
                weights[Feature.NodeItSelf_squared] * features[Feature.NodeItSelf] * features[Feature.NodeItSelf] +
                weights[Feature.Size_squared] * features[Feature.Size] * features[Feature.Size] +
                weights[Feature.Operations_squared] * features[Feature.Operations] * features[Feature.Operations] +
                weights[Feature.Constants_cubic] * features[Feature.Constants] * features[Feature.Constants] * features[Feature.Constants] +
                weights[Feature.References_cubic] * features[Feature.References] * features[Feature.References] * features[Feature.References] +
                weights[Feature.Abstract_cubic] * features[Feature.Abstract] * features[Feature.Abstract] * features[Feature.Abstract] +
                weights[Feature.Nodes_cubic] * features[Feature.Nodes] * features[Feature.Nodes] * features[Feature.Nodes] +
                weights[Feature.Patterns_cubic] * features[Feature.Patterns] * features[Feature.Patterns] * features[Feature.Patterns] +
                weights[Feature.ParentOne_cubic] * features[Feature.ParentOne] * features[Feature.ParentOne] * features[Feature.ParentOne] +
                weights[Feature.ParentTwo_cubic] * features[Feature.ParentTwo] * features[Feature.ParentTwo] * features[Feature.ParentTwo] +
                weights[Feature.ParentThree_cubic] * features[Feature.ParentThree] * features[Feature.ParentThree] * features[Feature.ParentThree] +
                weights[Feature.NodeItSelf_cubic] * features[Feature.NodeItSelf] * features[Feature.NodeItSelf] * features[Feature.NodeItSelf] +
                weights[Feature.Size_cubic] * features[Feature.Size] * features[Feature.Size] * features[Feature.Size] +
                weights[Feature.Operations_cubic] * features[Feature.Operations] * features[Feature.Operations] * features[Feature.Operations];

        }

        [FeatureCalculator(nameof(Semantics.Insert))]
        public double Score_Insert(double inScore, double astScore) => weights[Feature.Size] + inScore + astScore;

        [FeatureCalculator(nameof(Semantics.InsertBefore))]
        public double Score_InsertBefore(double inScore, double astScore) => weights[Feature.Size] + inScore + astScore;

        [FeatureCalculator(nameof(Semantics.Update))]
        public double Score_Update(double inScore, double toScore) => weights[Feature.Size] + inScore + toScore;

        [FeatureCalculator(nameof(Semantics.Delete))]
        public double Score_Delete(double inScore, double refscore) => weights[Feature.Size] + refscore;

        [FeatureCalculator(nameof(Semantics.Node))]
        public double Score_Node1(double kScore, double astScore) => weights[Feature.Size] + weights[Feature.Nodes] + astScore;

        [FeatureCalculator(nameof(Semantics.ConstNode))]
        public double Score_Node1(double astScore) => weights[Feature.Size] + weights[Feature.Constants];

        // Editing Abstract
        [FeatureCalculator(nameof(Semantics.Abstract))]
        public double Score_Abstract(double kindScore) => weights[Feature.Size] + weights[Feature.Abstract];

        [FeatureCalculator(nameof(Semantics.Context))]
        public double Score_ParentP(double matchScore, double kScore) => weights[Feature.Size] + matchScore + kScore;

        [FeatureCalculator(nameof(Semantics.ContextPPP))]
        public double Score_ParentPPP(double matchScore, double kScore) => weights[Feature.Size] + matchScore;

        [FeatureCalculator(nameof(Semantics.Concrete))]
        public double Score_Concrete(double treeScore) => weights[Feature.Size] + weights[Feature.Concrete];

        [FeatureCalculator(nameof(Semantics.Variable))]
        public double Score_Variable(double idScore) => weights[Feature.Size] + idScore;

        [FeatureCalculator(nameof(Semantics.Pattern))]
        public double Score_Pattern(double kindScore, double expression1Score) => weights[Feature.Size] + weights[Feature.Patterns] + expression1Score;

        [FeatureCalculator(nameof(Semantics.Reference))]
        public double Score_Reference(double inScore, double patternScore, double kScore) => weights[Feature.Size] + weights[Feature.References] + patternScore;

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

            //var features = RankingScore.getFeatures();
            if (f7.Success)
            {
                //features[Feature.ParentOne] = features[Feature.ParentOne] + 1;
                return weights[Feature.ParentOne];
            }
            else if (f8.Success)
            {
                //features[Feature.ParentTwo] = features[Feature.ParentTwo] + 1;
                return weights[Feature.ParentTwo];
            }
            else if (f9.Success)
            {
                //features[Feature.ParentThree] = features[Feature.ParentThree] + 1;
                return weights[Feature.ParentThree];
            }
            else if (f10.Success)
            {
                //features[Feature.NodeItSelf] = features[Feature.NodeItSelf] + 1;
                return weights[Feature.NodeItSelf];
            }
            return 0;
        }

        [FeatureCalculator("c", Method = CalculationMethod.FromLiteral)]
        public double CScore(int c) => 0;

        [FeatureCalculator("kind", Method = CalculationMethod.FromLiteral)]
        public double KindScore(SyntaxKind kd) => 0;

        [FeatureCalculator("tree", Method = CalculationMethod.FromLiteral)]
        public double NodeScore(Node kd) => 0;
    }
}
