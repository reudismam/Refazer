using System;
using System.Collections.Generic;
using System.Linq;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Synthesis;
using LocationCodeRefactoring.Spg.LocationRefactor.Location;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Util;
using Spg.LocationCodeRefactoring.Controller;
using Spg.LocationRefactor.Location;
using Spg.LocationRefactor.TextRegion;

namespace LocationCodeRefactoring.Spg.LocationRefactor.Transformation
{
    public class MappedLocationBasedTransformationManager : TransformationManager
    {

        /// <summary>
        /// Transform selection regions
        /// </summary>
        /// <returns>List of transformed locations</returns>
        public override List<Transformation> TransformProgram(bool compact)
        {
            RegionManager rManager = RegionManager.GetInstance();
            List<CodeLocation> locations = Controller.Locations; //previous locations

            List<Tuple<ListNode, ListNode>> mappingSelections = rManager.ElementsSelectionBeforeAndAfterEditing(locations);
            List<Tuple<ListNode, ListNode>> examples = EditedSelectionLocations(mappingSelections);

            SynthesizedProgram validated = LearnSynthesizerProgram(examples); //learn a synthesizer program
            EditorController.GetInstance().Program = validated;

            Dictionary<string, List<CodeLocation>> groupLocation = Groups(locations); //location for each file
            
            var transformations = new List<Transformation>();
            foreach (KeyValuePair<string, List<CodeLocation>> item in groupLocation)
            {
                string text = Transform(validated, item.Value, compact);
                Tuple<string, string> beforeAfter = Tuple.Create(item.Value[0].SourceCode, text);
                Transformation transformation = new Transformation(beforeAfter, item.Key);
                transformations.Add(transformation);
            }
            return transformations;
        }

        /// <summary>
        /// Transform a program
        /// </summary>
        /// <param name="program">Synthesized program</param>
        /// <param name="locations">Locations</param>
        /// <param name="compact">Define if input must to be compacted or not</param>
        /// <returns>Transformed program</returns>
        public override string Transform(SynthesizedProgram program, List<CodeLocation> locations, bool compact)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(locations[0].SourceCode); // all code location have the same source code
            List<Tuple<SyntaxNode, CodeLocation>> syntaxNodeCodeLocationPairs = new List<Tuple<SyntaxNode, CodeLocation>>();
            foreach (CodeLocation location in locations)
            {
                SyntaxNode selection = location.Region.Node;
                SyntaxNode node = tree.GetRoot().FindNode(selection.Span);
              
                Tuple<SyntaxNode, CodeLocation> tuple = Tuple.Create(node, location);
                syntaxNodeCodeLocationPairs.Add(tuple);
            }


            var text = FileUtil.ReadFile(locations[0].SourceClass);
            text = TransformEachLocation(text, syntaxNodeCodeLocationPairs, program, compact);
            
            SyntaxTree treeFormat = CSharpSyntaxTree.ParseText(text);
            SyntaxNode nodeFormat = treeFormat.GetRoot();//.NormalizeWhitespace();
            text = nodeFormat.GetText().ToString();
            return text;
        }

        private string TransformEachLocation(string text, List<Tuple<SyntaxNode, CodeLocation>> update, SynthesizedProgram program, bool compact)
        {
            string s = "";
            int i = 0;
            int nextStart = 0;
            foreach (var item in update)
            {
                try
                {
                    List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();
                    if (compact)
                    {
                        list = ASTManager.EnumerateSyntaxNodesAndTokens2(item.Item1, list);
                    }
                    else
                    {
                        list = ASTManager.EnumerateSyntaxNodesAndTokens(item.Item1, list);
                    }
                    ListNode lnode = new ListNode(list);

                    ASTTransformation treeNode = ASTProgram.TransformString(lnode, program);
                    string transformation = treeNode.Transformation;
                    s += ++i + "\n";
                    s += transformation + "\n";

                    int start = nextStart + item.Item2.Region.Start;
                    int end = start + item.Item2.Region.Length;
                    text = text.Substring(0, start) + transformation +
                    text.Substring(end);

                    nextStart += transformation.Length - item.Item2.Region.Length;
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            string classPath = update.First().Item2.SourceClass;
            string className = classPath.Substring(classPath.LastIndexOf(@"\") + 1, classPath.Length - (classPath.LastIndexOf(@"\") + 1));
            FileUtil.WriteToFile(@"C:\Users\SPG-04\Desktop\transformations\" + className, s);
            return text;
        }

        /// <summary>
        /// Learn a transformation program
        /// </summary>
        /// <param name="regionsToApplyTransformation">Location to apply transformations</param>
        /// <returns>Synthesized program</returns>
        public override SynthesizedProgram TransformationProgram(List<TRegion> regionsToApplyTransformation)
        {
            RegionManager rManager = RegionManager.GetInstance();
            List<CodeLocation> locations = Controller.Locations; //previous locations

            List<Tuple<ListNode, ListNode>> mappingSelections = rManager.ElementsSelectionBeforeAndAfterEditing(locations);
            List<Tuple<ListNode, ListNode>> examples = EditedSelectionLocations(mappingSelections);

            ASTProgram program = new ASTProgram();

            List<SynthesizedProgram> synthesizedProgs = program.GenerateStringProgram(examples);
            SynthesizedProgram validated = synthesizedProgs.Single();

            return validated;
        }

        /// <summary>
        /// Calculate the diference between location before, after edition
        /// </summary>
        /// <param name="allLocationsPairs">Locations before, after edition</param>
        /// <returns>Return a set of location before, after edition edited by the developer</returns>
        private List<Tuple<ListNode, ListNode>> EditedSelectionLocations(List<Tuple<ListNode, ListNode>> allLocationsPairs)
        {
            NodeComparer comparator = new NodeComparer();
            List < Tuple < ListNode, ListNode >> edited = new List<Tuple<ListNode, ListNode>>();
            foreach (Tuple<ListNode, ListNode> ln in allLocationsPairs)
            {
                bool isEqual = comparator.SequenceEqual(ln.Item1, ln.Item2);
                if (!isEqual)
                {
                    edited.Add(ln);
                }
            }
            
            return edited;
        }
    }
}


