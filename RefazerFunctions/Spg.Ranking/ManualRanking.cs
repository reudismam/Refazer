﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RefazerFunctions.Spg.Ranking
{
    public class ManualRanking : RankingFunction
    {
        [FeatureCalculator("EditMap")]
        public double Score_EditMap(double scriptScore, double editScore) => scriptScore + editScore;

        [FeatureCalculator(nameof(Semantics.AllNodes))]
        public double Score_Traversal(double scriptScore, double editScore) => scriptScore + editScore;

        [FeatureCalculator("EditFilter")]
        public double Score_EditFilter(double predScore, double splitScore) => (predScore + splitScore) * 1.1;

        [FeatureCalculator(nameof(Semantics.Match))]
        public double Score_Match(double inSource, double matchScore) => matchScore;

        [FeatureCalculator(nameof(Semantics.SC))]
        public double Score_CS(double childScore) => childScore;

        [FeatureCalculator(nameof(Semantics.CList))]
        public double Score_CList(double childScore, double childrenScore) => childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.SP))]
        public double Score_PS(double childScore) => childScore;

        [FeatureCalculator(nameof(Semantics.PList))]
        public double Score_PList(double childScore, double childrenScore) => childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.SN))]
        public double Score_SN(double childScore) => childScore;

        [FeatureCalculator(nameof(Semantics.NList))]
        public double Score_NList(double childScore, double childrenScore) => childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.SE))]
        public double Score_SE(double childScore) => childScore;

        [FeatureCalculator(nameof(Semantics.EList))]
        public double Score_EList(double childScore, double childrenScore) => childScore + childrenScore;

        [FeatureCalculator(nameof(Semantics.Transformation), Method = CalculationMethod.FromChildrenFeatureValues)]

        public double Score_Script1(double inScore, double edit) => inScore + edit;

        [FeatureCalculator(nameof(Semantics.Insert))]
        public double Score_Insert(double inScore, double astScore) => inScore + astScore;

        [FeatureCalculator(nameof(Semantics.InsertBefore))]
        public double Score_InsertBefore(double inScore, double astScore) => inScore + astScore;

        [FeatureCalculator(nameof(Semantics.Update))]
        public double Score_Update(double inScore, double toScore) => inScore + toScore;

        public double Score_Delete(double inScore, double refscore) => inScore + refscore;

        [FeatureCalculator(nameof(Semantics.Node))]
        public double Score_Node1(double kScore, double astScore) => kScore + astScore;

        [FeatureCalculator(nameof(Semantics.ConstNode))]
        public double Score_Node1(double astScore) => astScore;

        [FeatureCalculator(nameof(Semantics.Abstract))]
        public double Score_Abstract(double kindScore) => kindScore;

        [FeatureCalculator(nameof(Semantics.Context))]
        public double Score_ParentP(double matchScore, double kScore) => matchScore;

        [FeatureCalculator(nameof(Semantics.ContextPPP))]
        public double Score_ParentPPP(double matchScore, double kScore) => matchScore;

        [FeatureCalculator(nameof(Semantics.Concrete))]
        public double Score_Concrete(double treeScore) => treeScore * 1000;

        [FeatureCalculator(nameof(Semantics.Variable))]
        public double Score_Variable(double idScore) => idScore;

        [FeatureCalculator(nameof(Semantics.Pattern))]
        public double Score_Pattern(double kindScore, double expression1Score) => kindScore + expression1Score;

        [FeatureCalculator(nameof(Semantics.Reference))]
        public double Score_Reference(double inScore, double patternScore, double kScore) => patternScore;

        [FeatureCalculator("id", Method = CalculationMethod.FromLiteral)]
        public double KDScore(string kd) => 1.1;

        [FeatureCalculator("c", Method = CalculationMethod.FromLiteral)]
        public double CScore(int c) => 1.1;

        [FeatureCalculator("kind", Method = CalculationMethod.FromLiteral)]
        public double KindScore(SyntaxKind kd) => 1.1;

        [FeatureCalculator("tree", Method = CalculationMethod.FromLiteral)]
        public double NodeScore(SyntaxNodeOrToken kd) => 1.1;
    }

    public class ExtractFeature
    {

        public int[] Extract(string program)
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
