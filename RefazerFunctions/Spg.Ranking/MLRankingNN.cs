using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RefazerFunctions.Spg.Ranking
{
    public class MLRankingNN: RankingFunction
    {
        private static Dictionary<Feature, double> weights;
        private static Dictionary<int, double[][]> layersWeights;
        private static double [] intercepts;
        private static int nLaywers;

        public MLRankingNN()
        {
            //Weight for Individual Features
            weights = new Dictionary<Feature, double>();
            weights.Add(Feature.Intercept, 0.3366);
            weights.Add(Feature.Constants, 1.9215);
            weights.Add(Feature.References, -9.6286);
            weights.Add(Feature.Concrete, 0.3366);
            weights.Add(Feature.Abstract, -0.2229);
            weights.Add(Feature.Nodes, -1.6885);
            weights.Add(Feature.Patterns, 0.0169);
            weights.Add(Feature.ParentOne, -1.6819);
            weights.Add(Feature.ParentTwo, 0);
            weights.Add(Feature.ParentThree, 0);
            weights.Add(Feature.NodeItSelf, 0.5745);
            weights.Add(Feature.Size, -0.0076);
            weights.Add(Feature.Operations, 1.1343);
            weights.Add(Feature.Constants_squared, 0.2373);
            weights.Add(Feature.References_squared, 1.9779);
            weights.Add(Feature.Concrete_squared, -0.0590);
            weights.Add(Feature.Abstract_squared, -0.2987);
            weights.Add(Feature.Nodes_squared, -0.2122);
            weights.Add(Feature.Patterns_squared, 0.0190);
            weights.Add(Feature.ParentOne_squared, -0.0099);
            weights.Add(Feature.ParentTwo_squared, 0);
            weights.Add(Feature.ParentThree_squared, 0);
            weights.Add(Feature.NodeItSelf_squared, 0);
            weights.Add(Feature.Size_squared, 0.0007);
            weights.Add(Feature.Operations_squared, 0.5960);
            weights.Add(Feature.Constants_cubic, 0.7458);
            weights.Add(Feature.References_cubic, 1.1460);
            weights.Add(Feature.Concrete_cubic, -0.0265);
            weights.Add(Feature.Abstract_cubic, 0.1108);
            weights.Add(Feature.Nodes_cubic, 0.0125);
            weights.Add(Feature.Patterns_cubic, -0.0011);
            weights.Add(Feature.ParentOne_cubic, 0.1603);
            weights.Add(Feature.ParentTwo_cubic, 0);
            weights.Add(Feature.ParentThree_cubic, 0);//
            weights.Add(Feature.NodeItSelf_cubic, -0.0428);
            weights.Add(Feature.Size_cubic, 0.000002);
            weights.Add(Feature.Operations_cubic, 1.4522);
            //initNetworkWeka();
            initNetworkPython();
        }

        private static void initNetworkPython()
        {
            //Neural Network (MultilayerPerception Weka)
            layersWeights = new Dictionary<int, double[][]>();
            string text = File.ReadAllText(@"C:\Users\SPG-09\Documents\Visual Studio 2017\Projects\Refazer\weights_python.txt");
            string [] lines = text.Split(new char[] {' ','\n', '\r', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            string interceptText = File.ReadAllText(@"C:\Users\SPG-09\Documents\Visual Studio 2017\Projects\Refazer\intercepts_python.txt");
            string [] interceptLines = interceptText.Split(new char[] { ' ', '\n', '\r', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            nLaywers = 4;
            int layer0 = 12;
            int layer1 = 30;
            int layer2 = 30;
            int layer3 = 30;
            int layer4 = 1;
            intercepts = new double[layer1 + layer2 + layer3 + layer4];
            int line = 0;
            var weights01 = WeightsPython(lines, line, layer0, layer1);
            layersWeights.Add(0, weights01);
            line += layer0 * layer1;
            var weights12 = WeightsPython(lines, line, layer1, layer2);
            layersWeights.Add(1, weights12);
            line += layer1 * layer2;
            var weights23 = WeightsPython(lines, line, layer2, layer3);
            layersWeights.Add(2, weights23);
            line += layer2 * layer3;
            var weights34 = WeightsPython(lines, line, layer3, layer4);
            layersWeights.Add(3, weights34);
            for (int i = 0; i < layer1 + layer2 + layer3 + layer4; i++)
            {
                intercepts[i] = double.Parse(interceptLines[i]);
            }
        }

        private static double [][] WeightsPython(string [] lines, int line, int currentLayer, int nextLayer)
        {
            double[][] weights = new double[nextLayer][];
            for (int i = 0; i < nextLayer; i++)
            {
                weights[i] = new double[currentLayer];
            }
            for (int i = 0; i < currentLayer; i++)
            {
                string str = lines[line];
                Console.WriteLine();
                for (int j = 0; j < nextLayer; j++)
                {
                    string strLine = lines[line++];
                    double value = double.Parse(strLine);
                    weights[j][i] = value;
                }
            }
            return weights;
        }

        private static void initNetworkWeka()
        {
            //Neural Network (MultilayerPerception Weka)
            nLaywers = 2;
            layersWeights = new Dictionary<int, double[][]>();
            string [] lines = File.ReadAllLines(@"C:\Users\SPG-09\Documents\Visual Studio 2017\Projects\Refazer\weights_weka");
            int layer2 = 2;
            int layer1 = 19;
            int layer0 = 36;
            int line = 0;
            int interceptIndex = layer1;
            intercepts = new double[nLaywers];
            var weights12 = WeightsWeka(lines, line, layer1, layer2, interceptIndex);
            layersWeights.Add(1, weights12);
            line += layer2 * layer1 + layer2 * 3;
            interceptIndex = 0;
            var weights01 = WeightsWeka(lines, line, layer0, layer1, interceptIndex);
            layersWeights.Add(0, weights01);
        }

        public static double [][] WeightsWeka (string [] lines, int line, int previousLayer, int nextLayer, int interceptIndex)
        {
            double[][] weights12 = new double[nextLayer][];
            for (int i = 0; i < nextLayer; i++)
            {
                var weightsLine = new double[previousLayer];
                weights12[i] = weightsLine;
                line = line +  2;
                string lineString = lines[line++];
                var strSplit = lineString.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                double intercept = double.Parse(strSplit[1]);
                intercepts[interceptIndex++] = intercept;
                for (int j = 0; j < previousLayer; j++)
                {
                    lineString = lines[line++];
                    strSplit = lineString.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    double weight = double.Parse(strSplit[2]);
                    weights12[i][j] = weight;
                }
            }
            return weights12;
        }

        private static double [] Propagation()
        {
            Dictionary<Feature, int> features = RankingScore.getFeatures();
            int[] layer0 = new int[] { features[Feature.Constants],
                                         features[Feature.References],
                                         features[Feature.Concrete],
                                         features[Feature.Abstract],
                                         features[Feature.Nodes],
                                         features[Feature.Patterns],
                                         features[Feature.ParentOne],
                                         features[Feature.ParentTwo],
                                         features[Feature.ParentThree],
                                         features[Feature.NodeItSelf],
                                         features[Feature.Size],
                                         features[Feature.Operations],
                                         (int) Math.Pow(features[Feature.Constants], 2),
                                         (int) Math.Pow(features[Feature.References], 2),
                                         (int) Math.Pow(features[Feature.Concrete], 2),
                                         (int) Math.Pow(features[Feature.Abstract], 2),
                                         (int) Math.Pow(features[Feature.Nodes], 2),
                                         (int) Math.Pow(features[Feature.Patterns], 2),
                                         (int) Math.Pow(features[Feature.ParentOne], 2),
                                         (int) Math.Pow(features[Feature.ParentTwo], 2),
                                         (int) Math.Pow(features[Feature.ParentThree], 2),
                                         (int) Math.Pow(features[Feature.NodeItSelf], 2),
                                         (int) Math.Pow(features[Feature.Size], 2),
                                         (int) Math.Pow(features[Feature.Operations], 2),
                                         (int) Math.Pow(features[Feature.Constants], 3),
                                         (int) Math.Pow(features[Feature.References], 3),
                                         (int) Math.Pow(features[Feature.Concrete], 3),
                                         (int) Math.Pow(features[Feature.Abstract], 3),
                                         (int) Math.Pow(features[Feature.Nodes], 3),
                                         (int) Math.Pow(features[Feature.Patterns], 3),
                                         (int) Math.Pow(features[Feature.ParentOne], 3),
                                         (int) Math.Pow(features[Feature.ParentTwo], 3),
                                         (int) Math.Pow(features[Feature.ParentThree], 3),
                                         (int) Math.Pow(features[Feature.NodeItSelf], 3),
                                         (int) Math.Pow(features[Feature.Size], 3),
                                         (int) Math.Pow(features[Feature.Operations], 3)
                                       };
            var activations = layer0.Select(x => (double)x).ToArray();
            int interceptIndex = 0;
            for (int i = 0; i < nLaywers - 1; i++)
            {
                activations = Activation(activations, layersWeights[i]);
                for (int j = 0; j < activations.Count(); j++)
                {
                    activations[j] = ReLU(intercepts[interceptIndex++] + activations[j]);
                }
            }
            activations = Activation(activations, layersWeights[nLaywers - 1]);
            for (int j = 0; j < activations.Count(); j++)
            {
                activations[j] = intercepts[interceptIndex++] + activations[j];
            }
            return activations;
        }

        private static double [] Activation(double [] player, double [][] llweights)
        {
            double [] activations = new double [llweights.Length];
            for (int i = 0; i < llweights.Length; i++)
            {
                double activation = 0;
                for (int j = 0; j < llweights[i].Length; j++)
                {
                    activation += llweights[i][j] * player[j];
                }
                activations[i] = activation;
            }
            return activations;
        }
        public static double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        //Rectified Linear Unit
        public static double ReLU(double x)

        {
            return Math.Max(0, x);// x < 0 ? 0 : x;
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
            double [] output = Propagation();
            if (output.Count() > 1)
            {
                return output[1];
            }
            return output.First();
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

            var features = RankingScore.getFeatures();
            if (f7.Success)
            {
                features[Feature.ParentOne] = features[Feature.ParentOne] + 1;
                return weights[Feature.ParentOne];
            }
            else if (f8.Success)
            {
                features[Feature.ParentTwo] = features[Feature.ParentTwo] + 1;
                return weights[Feature.ParentTwo];
            }
            else if (f9.Success)
            {
                features[Feature.ParentThree] = features[Feature.ParentThree] + 1;
                return weights[Feature.ParentThree];
            }
            else if (f10.Success)
            {
                features[Feature.NodeItSelf] = features[Feature.NodeItSelf] + 1;
                return weights[Feature.NodeItSelf];
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
