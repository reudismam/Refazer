using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationExtractor.Learn;
using Spg.LocationExtractor.Program;
using Spg.LocationExtractor.TextRegion;

namespace Spg.LocationExtractor.Location
{
    public class LocationExtractor
    {
        private Learner learn;

        public LocationExtractor()
        {
            learn = new Learner();
        }
        public List<Prog> Extract(Dictionary<Color, List<TRegion>> regions, Color color)
        { 
            List<Tuple<ListNode, ListNode>> examples = new List<Tuple<ListNode, ListNode>>();
            List<Prog> programs = null;

            if (regions[color].Count() == 1)
            {
                foreach (TRegion region in regions[color])
                {
                    Tuple<String, String> ex = Tuple.Create(region.Parent.Text, region.Text);
                    Tuple<ListNode, ListNode> lex = ASTProgram.Example(ex);
                    examples.Add(lex);
                }

                programs = learn.LearnRegion(examples);
            }
            else
            {
                examples = learn.Decompose(regions[color]);
                programs = learn.LearnSeqRegion(examples);
            }

            return programs;
        }


        /// <summary>
        /// Return a list of examples on the format: method, selection.
        /// </summary>
        /// <param name="list">List of regions selected by the user</param>
        /// <returns>list of examples</returns>
        private List<String> ParentRegion(List<TRegion> list, String tree)
        {
            List<String> examples = new List<String>();

            TRegion region = list[0];

            IEnumerable<SyntaxNode> methods = learn.map.SyntaxNodes(tree); //ASTManager.SyntaxElements(tree);

            foreach (SyntaxNode me in methods)
            {
                TextSpan span = me.Span;
                int start = span.Start;
                int length = span.Length;
                foreach (TRegion re in list)
                {
                    if (start <= re.Start && re.Start <= start + length)
                    {
                        String te = me.ToString();
                        examples.Add(te);
                    }
                }
            }
            return examples;
        }

        /// <summary>
        /// Return a list of examples on the format: method, selection.
        /// </summary>
        /// <param name="list">List of regions selected by the user</param>
        /// <returns>list of examples</returns>
        private List<TRegion> TRegionParent(List<TRegion> list, String tree)
        {
            List<TRegion> examples = new List<TRegion>();

            TRegion region = list[0];

            IEnumerable<SyntaxNode> methods = learn.map.SyntaxNodes(tree); //ASTManager.SyntaxElements(tree);

            foreach (SyntaxNode me in methods)
            {
                TextSpan span = me.Span;
                int start = span.Start;
                int length = span.Length;
                foreach (TRegion re in list)
                {
                    if (start <= re.Start && re.Start <= start + length)
                    {
                        String te = me.ToString();
                        TRegion tr = new TRegion();
                        tr.Start = start;
                        tr.Length = length;
                        tr.Text = te;
                        tr.Node = me;
                        examples.Add(tr);
                    }
                }
            }
            return examples;
        }

        /// <summary>
        /// Retrieve the list of region in input
        /// </summary>
        /// <param name="program">Program</param>
        /// <param name="input">Input</param>
        /// <returns>Regions list</returns>
        public List<TRegion> RetrieveString(Prog program, String input)
        {
            SyntaxTree tree1 = CSharpSyntaxTree.ParseText(input);

            List<TRegion> regions = program.RetrieveString(input);
            return regions;
        }

        public String TransformProgram(Dictionary<Color, List<TRegion>> regionsBeforeEdit, Dictionary<Color, List<TRegion>> regionsAfterEdit, Color color, Prog prog)
        {
            SynthesizedProgram validated = null;
            foreach (Color c in regionsAfterEdit.Keys) {
                if (!c.Equals(Color.White)) {
                    List<Tuple<ListNode, ListNode>> exBefore = Examples(regionsBeforeEdit, c);
                    List<Tuple<ListNode, ListNode>> exAfter = Examples(regionsAfterEdit, c);

                    ASTProgram program = new ASTProgram();
                    List<Tuple<ListNode, ListNode>> examples = new List<Tuple<ListNode, ListNode>>();

                    for (int i = 0; i < exBefore.Count(); i++)
                    {
                        Tuple<ListNode, ListNode> example = Tuple.Create(exBefore[i].Item1, exAfter[i].Item1);
                        examples.Add(example);
                    }

                    List<SynthesizedProgram> synthesizedProgs = program.GenerateStringProgram(examples);
                    validated = synthesizedProgs.Single();

                    String text = regionsBeforeEdit[c][0].Parent.Text;
                    List<TRegion> regions = prog.RetrieveString(text);

                    //List<String> strRegions = ParentRegion(regions, text);
                    List<TRegion> strRegions = TRegionParent(regions, text);
                    foreach (TRegion region in strRegions) {
                        try {
                            SyntaxTree tree = program.TransformString(region.Node, validated);
                        
              
                            String transformation = tree.GetText().ToString();

                            String escaped = Regex.Escape(region.Text);
                            String replacement = Regex.Replace(text, escaped, transformation);
                            text = replacement;
                        }
                        catch (ArgumentOutOfRangeException e)
                        {
                            Console.WriteLine("ArgumentOutOfRangeException ... refactor");
                        }
                    }
                    SyntaxTree t = CSharpSyntaxTree.ParseText(text);
                    SyntaxNode root = t.GetRoot().NormalizeWhitespace();
                    t = t.WithChangedText(root.GetText());
                    return t.GetText().ToString();
                }
            }
            return null;

            /*

            SynthesizedProgram validated = null;
            foreach (Color c in regionsAfterEdit.Keys) {
                if (!c.Equals(Color.White)) {
                    List<Tuple<ListNode, ListNode>> exBefore = Examples(regionsBeforeEdit, c);
                    List<Tuple<ListNode, ListNode>> exAfter = Examples(regionsAfterEdit, c);

                    ASTProgram program = new ASTProgram();
                    List<Tuple<ListNode, ListNode>> examples = new List<Tuple<ListNode, ListNode>>();

                    for (int i = 0; i < exBefore.Count(); i++)
                    {
                        Tuple<ListNode, ListNode> example = Tuple.Create(exBefore[i].Item1, exAfter[i].Item1);
                        examples.Add(example);
                    }

                    List<SynthesizedProgram> synthesizedProgs = program.GenerateStringProgram(examples);
                    validated = synthesizedProgs.Single();

                    String text = regionsBeforeEdit[c][0].Parent.Text;
                    List<TRegion> regions = prog.RetrieveString(text);

                    List<String> strRegions = DecomposeStr(regions, text);
                    foreach (String region in strRegions) {
                        SyntaxTree tree = program.TransformString(region, validated);
                        String transformation = tree.GetText().ToString();

                        String escaped = Regex.Escape(region);
                        String replacement = Regex.Replace(text, escaped, transformation);
                        text = replacement; 
                    }
                    SyntaxTree t = CSharpSyntaxTree.ParseText(text);
                    SyntaxNode root = t.GetRoot().NormalizeWhitespace();
                    t = t.WithChangedText(root.GetText());
                    return t.GetText().ToString();
                }
            }
            return null;
            */

        }

        public List<Tuple<ListNode, ListNode>> Examples(Dictionary<Color, List<TRegion>> regions,  Color color)
        {
            List<Tuple<ListNode, ListNode>> examples = new List<Tuple<ListNode, ListNode>>();
            if (regions[color].Count() == 1)
            {
                foreach (TRegion region in regions[color])
                {
                    Tuple<String, String> ex = Tuple.Create(region.Parent.Text, region.Text);
                    Tuple<ListNode, ListNode> lex = ASTProgram.Example(ex);
                    examples.Add(lex);
                }
            }
            else
            {
                examples = learn.Decompose(regions[color]);
            }

            return examples;
        }
    }
}
