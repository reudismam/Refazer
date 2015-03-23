using System;
using System.Collections.Generic;
using System.Linq;
using ExampleRefactoring.Spg.ExampleRefactoring.AST;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using LocationCodeRefactoring.Spg.LocationRefactor.Location;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Util;
using Spg.LocationRefactor.Location;
using Spg.LocationRefactor.TextRegion;

namespace LocationCodeRefactoring.Spg.LocationRefactor.Transformation
{
    public class MapedLocationBasedTransformationManager : TransformationManager
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
        /// <returns>Transformed program</returns>
        public override string Transform(SynthesizedProgram program, List<CodeLocation> locations, bool compact)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(locations[0].SourceCode); // all code location have the same source code
            List<Tuple<SyntaxNode, CodeLocation>> update = new List<Tuple<SyntaxNode, CodeLocation>>();
            foreach (CodeLocation location in locations)
            {
                SyntaxNode selection = location.Region.Node;

                /*var decedents = from snode in tree.GetRoot().DescendantNodes()
                                where snode.Span.Start == selection.Span.Start && snode.Span.Length == selection.Span.Length
                                select snode;*/
                var decedents = ASTManager.NodesWithSameStartEndAndKind(tree, selection.Span.Start, selection.Span.End,
                    selection.CSharpKind());
                foreach (var item in decedents)
                {
                    Tuple<SyntaxNode, CodeLocation> tuple = Tuple.Create(item, location);
                    update.Add(tuple);
                }
            }

            var text = FileUtil.ReadFile(locations[0].SourceClass);
            text = TransformEachLocation(text, update, program, compact);
            /*foreach (var item in update)
            {
                try
                {
                    ASTTransformation treeNode = ASTProgram.TransformString(item.Item1, program);
                    String transformation = treeNode.transformation;
                    string nodeText = item.Item2.Region.Text;
                    String escaped = Regex.Escape(nodeText);
                    String replacement = Regex.Replace(text, escaped, transformation);
                    text = replacement;
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                }
            }*/
            SyntaxTree treeFormat = CSharpSyntaxTree.ParseText(text);
            SyntaxNode nodeFormat = treeFormat.GetRoot();//.NormalizeWhitespace();
            text = nodeFormat.GetText().ToString();
            return text;
        }

        private string TransformEachLocation(string text, List<Tuple<SyntaxNode, CodeLocation>> update, SynthesizedProgram program, bool compact)
        {
            //foreach (var item in update)
            //{
            //    try
            //    {
            //        ASTTransformation treeNode = ASTProgram.TransformString(item.Item1, program);
            //        string transformation = treeNode.transformation;


            //        string nodeText = item.Item2.Region.Text;
            //        string escaped = Regex.Escape(nodeText);
            //        string replacement = Regex.Replace(text, escaped, transformation);
            //        text = replacement;
            //    }
            //    catch (ArgumentOutOfRangeException e)
            //    {
            //        Console.WriteLine(e.Message);
            //    }
            //}
            //return text;
            //string replaced = text;
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
                    string transformation = treeNode.transformation;

                    int start = nextStart + item.Item2.Region.Start;
                    int end = start + item.Item2.Region.Length;
                    text = text.Substring(0, start) + transformation +
                    text.Substring(end);

                    nextStart += transformation.Length - item.Item2.Region.Length;

                    //string nodeText = item.Item2.Region.Text;
                    //string escaped = Regex.Escape(nodeText);
                    //string replacement = Regex.Replace(text, escaped, transformation);
                    //text = replacement;
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return text;
        }

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

        private List<Tuple<ListNode, ListNode>> EditedSelectionLocations(List<Tuple<ListNode, ListNode>> allLocationsPairs)
        {
            NodeComparer comparator = new NodeComparer();
            List < Tuple < ListNode, ListNode >> edited = new List<Tuple<ListNode, ListNode>>();
            foreach (var ln in allLocationsPairs)
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