﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using LocationCodeRefactoring.Spg.LocationCodeRefactoring.Controller;
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
        public override List<Transformation> TransformProgram()
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
                string text = Transform(validated, item.Value);
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
        public override string Transform(SynthesizedProgram program, List<CodeLocation> locations)
        {
            string text = "";
            SyntaxTree tree = CSharpSyntaxTree.ParseText(locations[0].SourceCode); // all code location have the same source code
            List<Tuple<SyntaxNode, CodeLocation>> update = new List<Tuple<SyntaxNode, CodeLocation>>();
            foreach (CodeLocation location in locations)
            {
                SyntaxNode selection = location.Region.Node;

                var decedents = from snode in tree.GetRoot().DescendantNodes()
                                where snode.Span.Start == selection.Span.Start && snode.Span.Length == selection.Span.Length
                                select snode;
                foreach (var item in decedents)
                {
                    Tuple<SyntaxNode, CodeLocation> tuple = Tuple.Create(item, location);
                    update.Add(tuple);
                }
            }

            text = FileUtil.ReadFile(locations[0].SourceClass);
            foreach (var item in update)
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
            }
            SyntaxTree treeFormat = CSharpSyntaxTree.ParseText(text);
            SyntaxNode nodeFormat = treeFormat.GetRoot();//.NormalizeWhitespace();
            text = nodeFormat.GetText().ToString();
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