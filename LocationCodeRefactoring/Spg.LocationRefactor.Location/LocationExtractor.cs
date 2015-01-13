using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using LocationCodeRefactoring.Br.Spg.Location;
using LocationCodeRefactoring.Spg.LocationRefactor.Transformation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Setting;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Workspace;
using Spg.LocationCodeRefactoring.Controller;
using Spg.LocationRefactor.Learn;
using Spg.LocationRefactor.Program;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Location
{
    /// <summary>
    /// Location extractor
    /// </summary>
    public class LocationExtractor
    {
        private Learner learn;
        string solutionPath;
        List<SyntaxKind> syntaxKind = new List<SyntaxKind>();
        public LocationExtractor(SyntaxKind syntaxKind, string solutionPath)
        {
            this.solutionPath = solutionPath;
            learn = new Learner(syntaxKind);
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
        private List<TRegion> TRegionParent(List<TRegion> list, String tree)
        {
            List<TRegion> examples = new List<TRegion>();

            TRegion region = list[0];

            IEnumerable<SyntaxNode> methods = learn.map.SyntaxNodes(tree, list); //ASTManager.SyntaxElements(tree);

            foreach (SyntaxNode syntaxNode in methods)
            {
                TextSpan span = syntaxNode.Span;
                int start = span.Start;
                int length = span.Length;
                foreach (TRegion tRegion in list)
                {
                    if (start <= tRegion.Start && tRegion.Start <= start + length)
                    {
                        String te = syntaxNode.ToString();
                        TRegion tr = new TRegion();
                        tr.Start = start;
                        tr.Length = length;
                        tr.Text = te;
                        tr.Node = syntaxNode;
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

        /// <summary>
        /// Retrieve the list of region in input
        /// </summary>
        /// <param name="program">Program</param>
        /// <param name="input">Input</param>
        /// <returns>Regions list</returns>
        public List<Tuple<string, List<TRegion>>> Locations(Prog program, List<string> csfiles)
        {
            List<Tuple<string, List<TRegion>>> locations = new List<Tuple<string, List<TRegion>>>();
            foreach (string sourfile in csfiles)
            {
                SyntaxTree tree1 = CSharpSyntaxTree.ParseText(sourfile);

                List<TRegion> regions = program.RetrieveString(sourfile);
                Tuple<string, List<TRegion>> locationTuple = Tuple.Create(sourfile, regions);
            }
            return locations;
        }

        public List<Tuple<string, string>> SourceFiles(string solutionPath)
        {
            WorkspaceManager manager = new WorkspaceManager();
            List<Tuple<string, string>> sourceFiles = manager.GetSourcesFiles(solutionPath);
            return sourceFiles;
        }

        /*public String TransformProgram(Dictionary<Color, List<TRegion>> regionsBeforeEdit, Dictionary<Color, List<TRegion>> regionsAfterEdit, Color color, Prog prog)
        {
            SynthesizedProgram validated = null;
            EditorController controler = EditorController.GetInstance();

            List<Tuple<TRegion, TRegion>> exampleRegions = ExtractLocations(controler.CodeBefore, controler.CodeAfter, learn.syntaxKind);
            List<Tuple<ListNode, ListNode>> examples = Nodes(exampleRegions);

            SynthesizerSetting setting = new SynthesizerSetting();
            setting.dynamicTokens = true;
            setting.deviation = 2;
            setting.considerEmpty = true;
            setting.considerConstrStr = true;

            ASTProgram program = new ASTProgram(setting, examples);

            //String parenttext1 = exampleRegions[0].Item1.Text;
            //String parenttext2 = exampleRegions[0].Item2.Text;

            List<SynthesizedProgram> synthesizedProgs = program.GenerateStringProgram(examples);
            validated = synthesizedProgs.Single();

            //String text = regionsBeforeEdit[Color.White][0].Text;
            String text = controler.CodeBefore;
            List<TRegion> regions = prog.RetrieveString(text);

            List<TRegion> strRegions = TRegionParent(regions, text);
            foreach (TRegion region in strRegions)
            {
                try
                {
                    ASTTransformation tree = ASTProgram.TransformString(region.Node, validated);
                    String transformation = tree.transformation;

                    String escaped = Regex.Escape(region.Text);
                    String replacement = Regex.Replace(text, escaped, transformation);
                    text = replacement;
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return text;     
       }*/

        public String TransformProgram(Dictionary<Color, List<TRegion>> regionsBeforeEdit, Dictionary<Color, List<TRegion>> regionsAfterEdit, Color color, Prog prog)
        {
            SynthesizedProgram validated = null;
            EditorController controler = EditorController.GetInstance();

            List<Tuple<TRegion, TRegion>> exampleRegions = ExtractLocations(controler.CodeBefore, controler.CodeAfter, learn.syntaxKind);
            List<Tuple<ListNode, ListNode>> examples = Nodes(exampleRegions);

            SynthesizerSetting setting = new SynthesizerSetting();
            setting.dynamicTokens = true;
            setting.deviation = 2;
            setting.considerEmpty = true;
            setting.considerConstrStr = true;

            ASTProgram program = new ASTProgram(setting, examples);

            List<SynthesizedProgram> synthesizedProgs = program.GenerateStringProgram(examples);
            validated = synthesizedProgs.Single();

            String text = controler.CodeBefore;
            List<TRegion> regions = prog.RetrieveString(text);

            List<TRegion> strRegions = TRegionParent(regions, text);
            var locations = controler.locations;
            List<SyntaxNode> nodeLocation = Locations(locations);
            foreach (SyntaxNode node in nodeLocation)
            {
                try
                {
                    ASTTransformation tree = ASTProgram.TransformString(node.GetText().ToString(), validated);
                    String transformation = tree.transformation;

                    String escaped = Regex.Escape(node.GetText().ToString());
                    String replacement = Regex.Replace(text, escaped, transformation);
                    text = replacement;
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return text;
        }

        private List<SyntaxNode> Locations(List<CodeLocation> locations)
        {
            List<TRegion> regions = new List<TRegion>();
            foreach (CodeLocation location in locations)
            {
                bool distinct = true;
                foreach (TRegion region in regions)
                {
                    if (region.Start == location.Region.Start && region.Length == location.Region.Length)
                    {
                        distinct = false;
                        break;
                    }
                }
                if (distinct)
                {
                    regions.Add(location.Region);
                }
            }
            Strategy strategy = StatementStrategy.GetInstance(SyntaxKind.MethodDeclaration);
            var result = strategy.SyntaxNodesRegion(locations[0].SourceCode, regions);
            return result;
        }


        /// <summary>
        /// Return program able to transform locations.
        /// </summary>
        /// <param name="regionsBeforeEdit">Regions before edit list</param>
        /// <param name="regionsAfterEdit">Regions after edit list</param>
        /// <param name="color">Color of statement</param>
        /// <param name="prog">Locations program</param>
        /// <returns></returns>
        public SynthesizedProgram TransformationProgram(Dictionary<Color, List<TRegion>> regionsBeforeEdit, Dictionary<Color, List<TRegion>> regionsAfterEdit, Color color)
        {
            EditorController controller = EditorController.GetInstance();
            List<Tuple<TRegion, TRegion>> exampleRegions = ExtractLocations(controller.CodeBefore, controller.CodeAfter, learn.syntaxKind);
            List<Tuple<ListNode, ListNode>> examples = Nodes(exampleRegions);

            return TransformationManager.TransformationProgram(examples);
        }

        private List<Tuple<ListNode, ListNode>> Nodes(List<Tuple<TRegion, TRegion>> examples)
        {
            List<Tuple<ListNode, ListNode>> nodes = new List<Tuple<ListNode, ListNode>>();
            foreach (Tuple<TRegion, TRegion> tuple in examples)
            {
                Tuple<SyntaxNode, SyntaxNode> tnode = Tuple.Create(tuple.Item1.Node, tuple.Item2.Node);
                Tuple<ListNode, ListNode> tlnode = ASTProgram.Example(tnode);
                nodes.Add(tlnode);
            }
            return nodes;
        }

        public List<Tuple<TRegion, TRegion>> ExtractLocations(String codeBefore, String codeAfter, SyntaxKind kind)
        {
            Strategy strategy = StatementStrategy.GetInstance(kind);
            List<SyntaxNode> nodesBefore = strategy.SyntaxNodesRegion(codeBefore, EditorController.GetInstance().RegionsBeforeEdit[Color.LightGreen]);
            List<SyntaxNode> nodesAfter = strategy.SyntaxNodesRegion(codeAfter, EditorController.GetInstance().RegionsBeforeEdit[Color.LightGreen]);

            List<Tuple<TRegion, TRegion>> examples = new List<Tuple<TRegion, TRegion>>();
            for (int i = 0; i < nodesBefore.Count; i++)
            {
                String statementBefore = nodesBefore[i].GetText().ToString();
                SyntaxNode nodeBefore = nodesBefore[i];

                String statementAfter = nodesAfter[i].GetText().ToString();
                SyntaxNode nodeAfter = nodesAfter[i];

                if (!statementBefore.Equals(statementAfter))
                {
                    TRegion regionBefore = new TRegion();
                    regionBefore.Text = statementBefore;
                    regionBefore.Node = nodeBefore;

                    TRegion regionAfter = new TRegion();
                    regionAfter.Text = statementAfter;
                    regionAfter.Node = nodeAfter;

                    Tuple<TRegion, TRegion> example = Tuple.Create(regionBefore, regionAfter);
                    examples.Add(example);
                }
            }
            return examples;
        }
    }
}



//public List<Tuple<string, string>> TransformLocations(Dictionary<Color, List<TRegion>> regionsBeforeEdit, Dictionary<Color, List<TRegion>> regionsAfterEdit, Color color, Prog prog)
//{
//    SynthesizedProgram validated = null;

//    List<Tuple<TRegion, TRegion>> exampleRegions = ExtractLocations(regionsBeforeEdit[Color.White][0].Text, regionsAfterEdit[Color.White][0].Text, learn.syntaxKind);
//    List<Tuple<ListNode, ListNode>> examples = Nodes(exampleRegions);

//    ASTProgram program = new ASTProgram();

//    List<SynthesizedProgram> synthesizedProgs = program.GenerateStringProgram(examples);
//    validated = synthesizedProgs.Single();

//    String text = regionsBeforeEdit[Color.White][0].Text;
//    List<TRegion> regions = prog.RetrieveString(text);

//    List<TRegion> strRegions = TRegionParent(regions, text);
//    List<Tuple<string, string>> transformedLocations = new List<Tuple<string, string>>();
//    foreach (TRegion region in strRegions)
//    {
//        try
//        {
//            ASTTransformation tree = ASTProgram.TransformString(region.Node, validated);
//            String transformation = tree.transformation;

//            Tuple<string, string> transformedLocation = Tuple.Create(region.Text, transformation);
//            transformedLocations.Add(transformedLocation);
//        }
//        catch (ArgumentOutOfRangeException e)
//        {
//            Console.WriteLine(e.Message);
//        }
//    }
//    return transformedLocations;
//}

///// <summary>
///// Return a list of examples on the format: method, selection.
///// </summary>
///// <param name="list">List of regions selected by the user</param>
///// <returns>list of examples</returns>
//private List<String> ParentRegion(List<TRegion> list, String tree)
//{
//    List<String> examples = new List<String>();

//    TRegion region = list[0];

//    IEnumerable<SyntaxNode> methods = learn.map.SyntaxNodes(tree); //ASTManager.SyntaxElements(tree);

//    foreach (SyntaxNode me in methods)
//    {
//        TextSpan span = me.Span;
//        int start = span.Start;
//        int length = span.Length;
//        foreach (TRegion re in list)
//        {
//            if (start <= re.Start && re.Start <= start + length)
//            {
//                String te = me.ToString();
//                examples.Add(te);
//            }
//        }
//    }
//    return examples;
//}

//    public String TransformProgram(Dictionary<Color, List<TRegion>> regionsBeforeEdit, Dictionary<Color, List<TRegion>> regionsAfterEdit, Color color, Prog prog)
//        {
//            SynthesizedProgram validated = null;

//            List<Tuple<TRegion, TRegion>> exampleRegions = ExtractLocations(regionsBeforeEdit[Color.White][0].Text, regionsAfterEdit[Color.White][0].Text, learn.syntaxKind);
//            List<Tuple<ListNode, ListNode>> examples = Nodes(exampleRegions);

//            ASTProgram program = new ASTProgram();

//            String parenttext1 = exampleRegions[0].Item1.Text;
//            String parenttext2 = exampleRegions[0].Item2.Text;

//            //List<int> boundary = SynthesisManager.CreateBoundaryPoints2(parenttext1, parenttext2, regionsAfterEdit[Color.White][0].Text, examples[0]);
//            //BoundaryManager.GetInstance().boundary = boundary;
//            //program.boundary = boundary;

//            List<SynthesizedProgram> synthesizedProgs = program.GenerateStringProgram(examples);
//            validated = synthesizedProgs.Single();

//            String text = regionsBeforeEdit[Color.White][0].Text;
//            List<TRegion> regions = prog.RetrieveString(text);

//            List<TRegion> strRegions = TRegionParent(regions, text);
//            foreach (TRegion region in strRegions)
//            {
//                try
//                {
//                    /*SyntaxTree tree = ASTProgram.TransformString(region.Node, validated);
//                    String transformation = tree.GetText().ToString();

//                    SyntaxTree t = CSharpSyntaxTree.ParseText(transformation);
//                    SyntaxNode root = t.GetRoot().NormalizeWhitespace();
//                    t = t.WithChangedText(root.GetText());
//                    transformation = t.GetText().ToString();

//                    String escaped = Regex.Escape(region.Text);
//                    String replacement = Regex.Replace(text, escaped, transformation);
//                    text = replacement;*/

//ASTTransformation tree = ASTProgram.TransformString(region.Node, validated);
//String transformation = tree.transformation;

//String escaped = Regex.Escape(region.Text);
//String replacement = Regex.Replace(text, escaped, transformation);
//text = replacement;
//                }
//                catch (ArgumentOutOfRangeException e)
//                {
//                    Console.WriteLine(e.Message);
//                }
//            }
//            return text;
//            /*SynthesizedProgram validated = null;
//            foreach (Color c in regionsAfterEdit.Keys) {
//                if (!c.Equals(Color.White)) {
//                    List<Tuple<TRegion, TRegion>> exps =   ExtractLocations(regionsBeforeEdit[Color.White][0].Text, regionsAfterEdit[Color.White][0].Text, learn.syntaxKind);

//                    List<Tuple<ListNode, ListNode>> exBefore = Examples(regionsBeforeEdit, c);
//                    List<Tuple<ListNode, ListNode>> exAfter = Examples(regionsAfterEdit, c);

//                    ASTProgram program = new ASTProgram();

//                    List<Tuple<String, String>> tBefore = ExtractText(regionsBeforeEdit[c]);
//                    List<Tuple<String, String>> tAfter  = ExtractText(regionsAfterEdit[c]);

//                    String parenttext1 = tBefore[0].Item2;
//                    String parenttext2 = tAfter[0].Item2;

//                    Tuple<ListNode, ListNode> tup = Tuple.Create(exBefore[0].Item1, exAfter[0].Item1);

//                    List<int> boundary = SynthesisManager.CreateBoundaryPoints2(parenttext1, parenttext2, regionsAfterEdit[Color.White][0].Text, tup);
//                    BoundaryManager.GetInstance().boundary = boundary;
//                    program.boundary = boundary;

//                    List<Tuple<ListNode, ListNode>> examples = new List<Tuple<ListNode, ListNode>>();

//                    for (int i = 0; i < exBefore.Count(); i++)
//                    {
//                        Tuple<ListNode, ListNode> example = Tuple.Create(exBefore[i].Item1, exAfter[i].Item1);
//                        examples.Add(example);
//                    }

//                    List<SynthesizedProgram> synthesizedProgs = program.GenerateStringProgram(examples);
//                    validated = synthesizedProgs.Single();

//                    String text = regionsBeforeEdit[c][0].Parent.Text;
//                    List<TRegion> regions = prog.RetrieveString(text);

//                    List<TRegion> strRegions = TRegionParent(regions, text);
//                    foreach (TRegion region in strRegions) {
//                        try {
//                            SyntaxTree tree = program.TransformString(region.Node, validated);

//                            String transformation = tree.GetText().ToString();

//                            String escaped = Regex.Escape(region.Text);
//                            String replacement = Regex.Replace(text, escaped, transformation);
//                            text = replacement;
//                        }
//                        catch (ArgumentOutOfRangeException e)
//                        {
//                            Console.WriteLine("ArgumentOutOfRangeException ... refactor");
//                        }
//                    }
//                    SyntaxTree t = CSharpSyntaxTree.ParseText(text);
//                    SyntaxNode root = t.GetRoot().NormalizeWhitespace();
//                    t = t.WithChangedText(root.GetText());
//                    return t.GetText().ToString();
//                }
//            }
//            return null;*/

//            /*

//            SynthesizedProgram validated = null;
//            foreach (Color c in regionsAfterEdit.Keys) {
//                if (!c.Equals(Color.White)) {
//                    List<Tuple<ListNode, ListNode>> exBefore = Examples(regionsBeforeEdit, c);
//                    List<Tuple<ListNode, ListNode>> exAfter = Examples(regionsAfterEdit, c);

//                    ASTProgram program = new ASTProgram();
//                    List<Tuple<ListNode, ListNode>> examples = new List<Tuple<ListNode, ListNode>>();

//                    for (int i = 0; i < exBefore.Count(); i++)
//                    {
//                        Tuple<ListNode, ListNode> example = Tuple.Create(exBefore[i].Item1, exAfter[i].Item1);
//                        examples.Add(example);
//                    }

//                    List<SynthesizedProgram> synthesizedProgs = program.GenerateStringProgram(examples);
//                    validated = synthesizedProgs.Single();

//                    String text = regionsBeforeEdit[c][0].Parent.Text;
//                    List<TRegion> regions = prog.RetrieveString(text);

//                    List<String> strRegions = DecomposeStr(regions, text);
//                    foreach (String region in strRegions) {
//                        SyntaxTree tree = program.TransformString(region, validated);
//                        String transformation = tree.GetText().ToString();

//                        String escaped = Regex.Escape(region);
//                        String replacement = Regex.Replace(text, escaped, transformation);
//                        text = replacement; 
//                    }
//                    SyntaxTree t = CSharpSyntaxTree.ParseText(text);
//                    SyntaxNode root = t.GetRoot().NormalizeWhitespace();
//                    t = t.WithChangedText(root.GetText());
//                    return t.GetText().ToString();
//                }
//            }
//            return null;
//            */

//        }
