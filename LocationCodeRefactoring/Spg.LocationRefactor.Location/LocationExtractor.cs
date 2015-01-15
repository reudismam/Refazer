using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using LocationCodeRefactoring.Br.Spg.Location;
using LocationCodeRefactoring.Spg.LocationRefactor.Transformation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Setting;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Util;
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
        /// <summary>
        /// Learner
        /// </summary>
        private Learner learn;

        /// <summary>
        /// Solution path
        /// </summary>
        string solutionPath;

        /// <summary>
        /// Controller
        /// </summary>
        public EditorController controller = EditorController.GetInstance();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="syntaxKind"></param>
        /// <param name="solutionPath"></param>
        public LocationExtractor(SyntaxKind syntaxKind, string solutionPath)
        {
            this.solutionPath = solutionPath;
            learn = new Learner(syntaxKind);
        }

        /// <summary>
        /// Extract location
        /// </summary>
        /// <param name="regions">List of selected regions</param>
        /// <param name="color">Color of the region</param>
        /// <returns>Extracted locations</returns>
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

        /// <summary>
        /// Learn a synthesizer program
        /// </summary>
        /// <param name="examples">Examples</param>
        /// <returns>Synthesizer program</returns>
        private SynthesizedProgram LearnSynthesizerProgram(List<Tuple<ListNode, ListNode>> examples)
        {
            SynthesizerSetting setting = new SynthesizerSetting(); //configure setting
            setting.dynamicTokens = true; setting.deviation = 2; setting.considerEmpty = true; setting.considerConstrStr = true;

            ASTProgram program = new ASTProgram(setting, examples); //create a new AST program and learn synthesizer
            List<SynthesizedProgram> synthesizedProgs = program.GenerateStringProgram(examples);
            SynthesizedProgram validated = synthesizedProgs.Single();
            return validated;
        }

        /// <summary>
        /// Transform selection regions
        /// </summary>
        /// <param name="regionsBeforeEdit">Regions before edition</param>
        /// <param name="regionsAfterEdit">Regions after edition</param>
        /// <param name="color">Color</param>
        /// <param name="prog">Program selected</param>
        /// <returns></returns>
        public List<Transformation> TransformProgram(Dictionary<Color, List<TRegion>> regionsBeforeEdit, Dictionary<Color, List<TRegion>> regionsAfterEdit, Color color, Prog prog)
        {
            List<Tuple<TRegion, TRegion>> exampleRegions = ExtractLocations(controller.CodeBefore, controller.CodeAfter);
            List<Tuple<ListNode, ListNode>> examples = ListNodes(exampleRegions);

            SynthesizedProgram validated = LearnSynthesizerProgram(examples); //learn a synthesizer program

            var locations = controller.locations; //previous locations

            Dictionary<string, List<CodeLocation>> groupLocation = Groups(locations); //location for each file
            StatementStrategy strategy = StatementStrategy.GetInstance();

            List<Transformation> transformations = new List<Transformation>();
            foreach (KeyValuePair<string, List<CodeLocation>> item in groupLocation)
            {
                string text = Transform(validated, item.Value);
                Tuple<string, string> beforeAfter = Tuple.Create(item.Value[0].SourceCode, text);
                Transformation transformation = new Transformation(beforeAfter, item.Key);
                transformations.Add(transformation);
            }
            return transformations;
        }

        public string Transform(SynthesizedProgram program, List<CodeLocation> locations)
        {
            string text = "";
            StatementStrategy strategy = StatementStrategy.GetInstance();
            SyntaxTree tree = CSharpSyntaxTree.ParseText(locations[0].SourceCode); // all code location have the same source code
            List<SyntaxNode> update = new List<SyntaxNode>();
            foreach (CodeLocation location in locations)
            {
                 SyntaxNode selection = strategy.SyntaxNodesRegion(location.SourceCode, location.Region);
                 var decedents = from snode in tree.GetRoot().DescendantNodes()
                                 where snode.Span.Start == selection.Span.Start && snode.Span.Length == selection.Span.Length
                                 select snode;
                 update.AddRange(decedents);
            }

            text = FileUtil.ReadFile(locations[0].SourceClass);
            foreach (SyntaxNode node in update)
            {
                try
                {
                    ASTTransformation treeNode = ASTProgram.TransformString(node, program);
                    String transformation = treeNode.transformation;
                    string nodeText = node.GetText().ToString();
                    String escaped = Regex.Escape(nodeText);
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

        /// <summary>
        /// Group location by file path
        /// </summary>
        /// <param name="locations">Location</param>
        /// <returns>Grouping</returns>
        private static Dictionary<string, List<CodeLocation>> Groups(List<CodeLocation> locations)
        {
            Dictionary<string, List<CodeLocation>> dic = new Dictionary<string, List<CodeLocation>>();
            foreach (CodeLocation location in locations)
            {
                List<CodeLocation> value;
                if(!dic.TryGetValue(location.SourceClass, out value)){
                    value = new List<CodeLocation>();
                    dic[location.SourceClass] = value;
                }
                value.Add(location);
            }
            return dic;
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
            List<Tuple<TRegion, TRegion>> exampleRegions = ExtractLocations(controller.CodeBefore, controller.CodeAfter);
            List<Tuple<ListNode, ListNode>> examples = ListNodes(exampleRegions);

            return TransformationManager.TransformationProgram(examples);
        }

        /// <summary>
        /// List node in the text regions
        /// </summary>
        /// <param name="examples">Text regions</param>
        /// <returns>Regions</returns>
        private List<Tuple<ListNode, ListNode>> ListNodes(List<Tuple<TRegion, TRegion>> examples)
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

        /// <summary>
        /// Extract location
        /// </summary>
        /// <param name="codeBefore">Code before transformation</param>
        /// <param name="codeAfter">Code after transformation</param>
        /// <returns>Locations</returns>
        public List<Tuple<TRegion, TRegion>> ExtractLocations(String codeBefore, String codeAfter)
        {
            Strategy strategy = StatementStrategy.GetInstance();
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


///// <summary>
///// Return a list of examples on the format: method, selection.
///// </summary>
///// <param name="list">List of regions selected by the user</param>
///// <returns>list of examples</returns>
//private List<TRegion> TRegionParent(List<TRegion> list, String tree)
//{
//    List<TRegion> examples = new List<TRegion>();

//    TRegion region = list[0];

//    IEnumerable<SyntaxNode> methods = learn.map.SyntaxNodes(tree, list); //ASTManager.SyntaxElements(tree);

//    foreach (SyntaxNode syntaxNode in methods)
//    {
//        TextSpan span = syntaxNode.Span;
//        int start = span.Start;
//        int length = span.Length;
//        foreach (TRegion tRegion in list)
//        {
//            if (start <= tRegion.Start && tRegion.Start <= start + length)
//            {
//                String te = syntaxNode.ToString();
//                TRegion tr = new TRegion();
//                tr.Start = start;
//                tr.Length = length;
//                tr.Text = te;
//                tr.Node = syntaxNode;
//                examples.Add(tr);
//            }
//        }
//    }
//    return examples;
//}


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

//public String TransformProgram(Dictionary<Color, List<TRegion>> regionsBeforeEdit, Dictionary<Color, List<TRegion>> regionsAfterEdit, Color color, Prog prog)
//{
//    SynthesizedProgram validated = null;
//    EditorController controler = EditorController.GetInstance();

//    List<Tuple<TRegion, TRegion>> exampleRegions = ExtractLocations(controler.CodeBefore, controler.CodeAfter, learn.syntaxKind);
//    List<Tuple<ListNode, ListNode>> examples = Nodes(exampleRegions);

//    SynthesizerSetting setting = new SynthesizerSetting();
//    setting.dynamicTokens = true;
//    setting.deviation = 2;
//    setting.considerEmpty = true;
//    setting.considerConstrStr = true;

//    ASTProgram program = new ASTProgram(setting, examples);

//    List<SynthesizedProgram> synthesizedProgs = program.GenerateStringProgram(examples);
//    validated = synthesizedProgs.Single();

//    String text = controler.CodeBefore;
//    List<TRegion> regions = prog.RetrieveString(text);

//    List<TRegion> strRegions = TRegionParent(regions, text);
//    var locations = controler.locations;
//    List<CodeLocation> nodeLocation = Locations(locations);
//    Dictionary<string, List<CodeLocation>> groupLocation = Groups(nodeLocation);
//    StatementStrategy strategy = StatementStrategy.GetInstance(SyntaxKind.MethodDeclaration);
//    //SyntaxTree tree = CSharpSyntaxTree.ParseText(text);
//    foreach (KeyValuePair<string, List<CodeLocation>> item in groupLocation)
//    {
//        SyntaxTree tree = CSharpSyntaxTree.ParseText(item.Value[0].SourceCode); // all code location have the same source code
//        List<SyntaxNode> update = new List<SyntaxNode>();
//        foreach (CodeLocation location in item.Value)
//        {
//            SyntaxNode selection = strategy.SyntaxNodesRegion(location.SourceCode, location.Region);
//            var descedents = from snode in tree.GetRoot().DescendantNodes()
//                             where snode.Span.Start == selection.Span.Start && snode.Span.Length == selection.Span.Length
//                             select snode;
//            update.AddRange(descedents);
//        }
//        text = FileUtil.ReadFile(item.Key);
//        foreach (SyntaxNode node in update)
//        {

//            try
//            {
//                ASTTransformation treeNode = ASTProgram.TransformString(node, validated);
//                String transformation = treeNode.transformation;

//                //var descedentsBegin = from snode in node.DescendantNodes()
//                //where snode.SpanStart == 
//                //                      select snode;

//                string nodeText = node.GetText().ToString();// .Replace("\r\n", "\n");
//                String escaped = Regex.Escape(nodeText);
//                //text = text.Replace("\r\n", "\n");
//                String replacement = Regex.Replace(text, escaped, transformation);
//                text = replacement;
//            }
//            catch (ArgumentOutOfRangeException e)
//            {
//                Console.WriteLine(e.Message);
//            }
//            ////try
//            ////{
//            ////SyntaxNode parent = node.Parent;
//            ////parent.ReplaceNode(node, node);
//            //ASTTransformation astTransformation = ASTProgram.TransformString(node, validated);
//            //string transformation = astTransformation.transformation;
//            //SyntaxNode transformationNode = astTransformation.tree.GetRoot();

//            //var track = node.TrackNodes<SyntaxNode>(transformationNode.DescendantNodes());
//            //String TreeText = track.GetText().ToString();

//            //ASTRewriter rewriter = new ASTRewriter(node, transformationNode);
//            //SyntaxNode newRoot = rewriter.Visit(tree.GetRoot());
//            ////SyntaxNode newRoot = tree.GetRoot().ReplaceNode(node, transformationNode);
//            //String trash = newRoot.GetText().ToString();
//            ////}
//            ////catch (Exception e)
//            ////{
//            ////    Console.WriteLine(e.Message);
//            ////}
//        }
//    }

//    return text;
//    /*foreach (SyntaxNode node in nodeLocation)
//    {
//        try
//        {
//            ASTTransformation tree = ASTProgram.TransformString(node.GetText().ToString(), validated);
//            String transformation = tree.transformation;

//            var descedentsBegin = from snode in node.DescendantNodes()
//                                  //where snode.SpanStart == 
//                                  select snode;

//            String escaped = Regex.Escape(node.GetText().ToString());
//            String replacement = Regex.Replace(text, escaped, transformation);
//            text = replacement;
//        }
//        catch (ArgumentOutOfRangeException e)
//        {
//            Console.WriteLine(e.Message);
//        }
//    }*/
//    //return text;
//}

///*public String TransformProgram(Dictionary<Color, List<TRegion>> regionsBeforeEdit, Dictionary<Color, List<TRegion>> regionsAfterEdit, Color color, Prog prog)
//    {
//        SynthesizedProgram validated = null;
//        EditorController controler = EditorController.GetInstance();

//        List<Tuple<TRegion, TRegion>> exampleRegions = ExtractLocations(controler.CodeBefore, controler.CodeAfter, learn.syntaxKind);
//        List<Tuple<ListNode, ListNode>> examples = Nodes(exampleRegions);

//        SynthesizerSetting setting = new SynthesizerSetting();
//        setting.dynamicTokens = true;
//        setting.deviation = 2;
//        setting.considerEmpty = true;
//        setting.considerConstrStr = true;

//        ASTProgram program = new ASTProgram(setting, examples);

//        //String parenttext1 = exampleRegions[0].Item1.Text;
//        //String parenttext2 = exampleRegions[0].Item2.Text;

//        List<SynthesizedProgram> synthesizedProgs = program.GenerateStringProgram(examples);
//        validated = synthesizedProgs.Single();

//        //String text = regionsBeforeEdit[Color.White][0].Text;
//        String text = controler.CodeBefore;
//        List<TRegion> regions = prog.RetrieveString(text);

//        List<TRegion> strRegions = TRegionParent(regions, text);
//        foreach (TRegion region in strRegions)
//        {
//            try
//            {
//                ASTTransformation tree = ASTProgram.TransformString(region.Node, validated);
//                String transformation = tree.transformation;

//                String escaped = Regex.Escape(region.Text);
//                String replacement = Regex.Replace(text, escaped, transformation);
//                text = replacement;
//            }
//            catch (ArgumentOutOfRangeException e)
//            {
//                Console.WriteLine(e.Message);
//            }
//        }
//        return text;     
//   }

///// <summary>
///// Source files in solution
///// </summary>
///// <param name="solutionPath">Solution path</param>
///// <returns>List of source file in the solution</returns>
//public List<Tuple<string, string>> SourceFiles(string solutionPath)
//{
//    WorkspaceManager manager = new WorkspaceManager();
//    List<Tuple<string, string>> sourceFiles = manager.GetSourcesFiles(solutionPath);
//    return sourceFiles;
//}