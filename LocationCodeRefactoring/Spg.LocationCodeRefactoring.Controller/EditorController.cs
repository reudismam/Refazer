using System;
using System.Collections.Generic;
using System.Drawing;
using ExampleRefactoring.Spg.ExampleRefactoring.Bean;
using ExampleRefactoring.Spg.ExampleRefactoring.Util;
using LocationCodeRefactoring.Spg.LocationRefactor.Transformation;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Workspace;
using Spg.LocationCodeRefactoring.Observer;
using Spg.LocationRefactor.Location;
using Spg.LocationRefactor.Program;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationCodeRefactoring.Controller
{
    /// <summary>
    /// Controller for editor graphical interface
    /// </summary>
    public class EditorController
    {
        private List<IHilightObserver> hilightObservers;
        private List<IProgramsGeneratedObserver> programGeneratedObservers;
        private List<IProgramRefactoredObserver> programsRefactoredObserver;
        private List<ILocationsTransformedObserver> locationsTransformedObserver;
        private List<ILocationsObserver> locationsObversers;

        /// <summary>
        /// Locations
        /// </summary>
        /// <returns>Locations</returns>
        public List<CodeLocation> locations { get; set; }

        /// <summary>
        /// Transformation
        /// </summary>
        /// <returns>transformation</returns>
        public List<CodeTransformation> CodeTransformations { get; set; }

        public List<Transformation> SourceTransformations { get; set; }

        /// <summary>
        /// Regions before the transformation
        /// </summary>
        /// <returns></returns>
        public Dictionary<Color, List<TRegion>> RegionsBeforeEdit { get; set; }

        /// <summary>
        /// Regions after the transformation
        /// </summary>
        /// <returns></returns>
        public Dictionary<Color, List<TRegion>> RegionsAfterEdit { get; set; }

        /// <summary>
        /// Learned programs
        /// </summary>
        public Dictionary<String, Prog> Progs { get; set; }

        /// <summary>
        /// Syntax kind
        /// </summary>
        /// <returns>Syntax kind</returns>
        public SyntaxKind syntaxKind { get; set; }

        /// <summary>
        /// Solution path
        /// </summary>
        public string solutionPath { get; set; }

        /// <summary>
        /// Selected location program
        /// </summary>
        /// <returns>Selected program location</returns>
        public string selec { get; set; }

        public string CodeBefore { get; set; }

        /// <summary>
        /// Code before transformation
        /// </summary>
        /// <returns>Code before transformation</returns>
        public string CodeAfter { get; set; }

        private static EditorController instance;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="syntaxKind"></param>
        /// <param name="solutionPath"></param>
        private EditorController(SyntaxKind syntaxKind, string solutionPath)
        {
            this.syntaxKind = syntaxKind;
            this.solutionPath = solutionPath;
            this.RegionsBeforeEdit = new Dictionary<Color, List<TRegion>>();
            this.RegionsAfterEdit = new Dictionary<Color, List<TRegion>>();
            this.Progs = new Dictionary<String, Prog>();
            this.hilightObservers = new List<IHilightObserver>();
            this.programGeneratedObservers = new List<IProgramsGeneratedObserver>();
            this.programsRefactoredObserver = new List<IProgramRefactoredObserver>();
            this.locationsTransformedObserver = new List<ILocationsTransformedObserver>();
            this.locationsObversers = new List<ILocationsObserver>();
        }

        /// <summary>
        /// Get singleton EditorController instance
        /// </summary>
        /// <returns>Editor controller instance</returns>
        public static EditorController GetInstance()
        {
            if (instance == null)
            {
                instance = new EditorController(SyntaxKind.MethodDeclaration, "");
            }
            return instance;
        }

        /// <summary>
        /// Extract locations
        /// </summary>
        public void Extract()
        {
            Color color = Color.LightGreen;
            LocationExtractor extractor = new LocationExtractor(syntaxKind, solutionPath);
            //remove
            JsonUtil<List<TRegion>>.Write(RegionsBeforeEdit[color], "simple_api_change_input.json");
            //remove
            List<Prog> programs = extractor.Extract(RegionsBeforeEdit, (Color)color);

            List<Prog> filtereds = new List<Prog>();
            foreach (Prog program in programs)
            {
                Prog val;
                if (!Progs.TryGetValue(program.ToString(), out val))
                {
                    Progs.Add(program.ToString(), program);
                    filtereds.Add(program);
                }
            }

            NotifyProgramGeneratedObservers(filtereds);
        }

        /// <summary>
        /// Edit locations
        /// </summary>
        /// <param name="start">Start position</param>
        /// <param name="length">Region length</param>
        /// <param name="sourceCode">Source code</param>
        /// <param name="color">Color region</param>
        /// <param name="range">Region range</param>
        public void Edit(int start, int length, String span, String sourceCode)
        {
            TRegion tregion = new TRegion();
            Color color = Color.LightGreen;
            tregion.Start = start;
            tregion.Length = length;
            tregion.Text = span;
            tregion.Color = color;

            List<TRegion> values;
            if (!RegionsBeforeEdit.TryGetValue(color, out values))
            {
                values = new List<TRegion>();
                RegionsBeforeEdit.Add(color, values);
            }

            values.Add(tregion);

            foreach (KeyValuePair<Color, List<TRegion>> dic in RegionsBeforeEdit)
            {
                foreach (TRegion tr in dic.Value)
                {
                    if (tregion.IsParent(tr))
                    {
                        tregion.Parent = tr;
                    }
                }
            }

            if (tregion.Parent == null)
            {
                TRegion parent = new TRegion();
                parent.Start = 0;
                parent.Length = sourceCode.Length;
                parent.Text = sourceCode;
                parent.Color = Color.White;
                tregion.Parent = parent;
            }
        }

        /// <summary>
        /// Retrieve locations
        /// </summary>
        /// <param name="selected">Selected region</param>
        /// <param name="program">Selected program</param>
        public void RetrieveRegions(String selected, String program)
        {
            this.selec = selected;
            Prog prog = Progs[selected];
            LocationExtractor extractor = new LocationExtractor(syntaxKind, solutionPath);
            List<Tuple<string, string>> sourceFiles = (new WorkspaceManager()).SourceFiles(solutionPath);

            List<CodeLocation> sourceLocations = new List<CodeLocation>();

            foreach (Tuple<string, string> source in sourceFiles)
            {
                List<TRegion> regions = extractor.RetrieveString(prog, source.Item1);
                foreach (TRegion region in regions)
                {
                    CodeLocation location = new CodeLocation();
                    location.Region = region;
                    location.SourceCode = source.Item1;
                    location.SourceClass = source.Item2;
                    sourceLocations.Add(location);

                }
            }

            List<TRegion> rs = extractor.RetrieveString(prog, program);

            this.locations = NonDuplicateLocations(sourceLocations);

            //remove
            List<Selection> selections = new List<Selection>();
            foreach (CodeLocation location in locations)
            {
                Selection selection = new Selection(location.Region.Start, location.Region.Length, location.SourceClass, location.SourceCode);
                selections.Add(selection);
            }
            JsonUtil <List<Selection>>.Write(selections, "simple_api_change_output.json");
            //remove

            NotifyHilightObservers(rs);
            NotifyLocationsObservers(sourceLocations);
        }

        

        /// <summary>
        /// Selected only non duplicate locations
        /// </summary>
        /// <param name="locations">All locations</param>
        /// <returns>Non duplicate locations</returns>
        public List<CodeLocation> NonDuplicateLocations(List<CodeLocation> locations)
        {
            List<CodeLocation> nonDuplicateLocations = new List<CodeLocation>();
            foreach (CodeLocation location in locations)
            {
                bool distinct = true;
                foreach (CodeLocation region in nonDuplicateLocations)
                {
                    if (region.Region.Start == location.Region.Start && region.Region.Length == location.Region.Length)
                    {
                        distinct = false; break;
                    }
                }
                if (distinct)
                {
                    nonDuplicateLocations.Add(location);
                }
            }
            return nonDuplicateLocations;
        }

        /// <summary>
        /// Refactor a region
        /// </summary>
        /// <param name="program">Source code</param>
        /// <param name="color">Selected color</param>
        public void Refact()
        {
            Prog prog = Progs[selec];
            Color color = Color.LightGreen;

            LocationExtractor extractor = new LocationExtractor(syntaxKind, solutionPath);
            List<Transformation> transformations = extractor.TransformProgram(RegionsBeforeEdit, RegionsAfterEdit, color, prog);
            this.SourceTransformations = transformations;

            SynthesizedProgram synthesized = extractor.TransformationProgram(RegionsBeforeEdit, RegionsAfterEdit, color);

            NotifyProgramRefactoredObservers(transformations);
            NotifyLocationsTransformedObservers(synthesized, locations);
        }

        /// <summary>
        /// Add highlight observer
        /// </summary>
        /// <param name="observer">Observer</param>
        public void AddHilightObserver(IHilightObserver observer)
        {
            this.hilightObservers.Add(observer);
        }

        /// <summary>
        /// Notify highlight observers
        /// </summary>
        /// <param name="regions"></param>
        private void NotifyHilightObservers(List<TRegion> regions)
        {
            HighlightEvent hEvent = new HighlightEvent(regions);
            foreach (IHilightObserver observer in hilightObservers)
            {
                observer.NotifyHilightChanged(hEvent);
            }
        }

        /// <summary>
        /// Add program generated observer
        /// </summary>
        /// <param name="observer">Observer</param>
        public void AddProgramGeneratedObserver(IProgramsGeneratedObserver observer)
        {
            this.programGeneratedObservers.Add(observer);
        }

        /// <summary>
        /// Notify generated observers
        /// </summary>
        /// <param name="programs">Generated observers</param>
        private void NotifyProgramGeneratedObservers(List<Prog> programs)
        {
            ProgramGeneratedEvent pEvent = new ProgramGeneratedEvent(programs);
            foreach (IProgramsGeneratedObserver observer in programGeneratedObservers)
            {
                observer.NotifyProgramGenerated(pEvent);
            }
        }

        /// <summary>
        /// Add refactored observer
        /// </summary>
        /// <param name="observer">Observer</param>
        public void AddProgramRefactoredObserver(IProgramRefactoredObserver observer)
        {
            this.programsRefactoredObserver.Add(observer);
        }

        /// <summary>
        /// Notify program refactored observers
        /// </summary>
        /// <param name="transformations"></param>
        private void NotifyProgramRefactoredObservers(List<Transformation> transformations)
        {
            ProgramRefactoredEvent rEvent = new ProgramRefactoredEvent(transformations);

            foreach (IProgramRefactoredObserver observer in programsRefactoredObserver)
            {
                observer.NotifyProgramRefactored(rEvent);
            }
        }

        /// <summary>
        /// Add locations transformed observer
        /// </summary>
        /// <param name="observer">observer</param>
        public void AddLocationsTransformedObvserver(ILocationsTransformedObserver observer)
        {
            this.locationsTransformedObserver.Add(observer);
        }

        private void NotifyLocationsTransformedObservers(SynthesizedProgram program, List<CodeLocation> locations)
        {
            List<CodeTransformation> transformations = new List<CodeTransformation>();
            foreach (CodeLocation location in locations)
            {
                Tuple<string, string> transformedLocation = Transformation(location, program);

                if (transformedLocation == null) continue;

                CodeTransformation transformation = new CodeTransformation(location, transformedLocation);
                transformations.Add(transformation);
            }

            this.CodeTransformations = transformations;

            LocationsTransformedEvent ltEvent = new LocationsTransformedEvent(transformations);

            foreach (ILocationsTransformedObserver observer in locationsTransformedObserver)
            {
                observer.NotifyLocationsTransformed(ltEvent);
            }
        }

        /// <summary>
        /// Look for a place to put this. Refactor
        /// </summary>
        /// <returns></returns>
        public Tuple<string, string> Transformation(CodeLocation location, SynthesizedProgram program)
        {
            TRegion region = location.Region;
            try
            {
                ASTTransformation tree = ASTProgram.TransformString(region.Node, program);
                String transformation = tree.transformation;

                Tuple<string, string> transformedLocation = Tuple.Create(region.Node.GetText().ToString(), transformation);
                return transformedLocation;
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }


        /// <summary>
        /// Add locations transformed observer
        /// </summary>
        /// <param name="observer">observer</param>
        public void AddLocationsObserver(ILocationsObserver observer)
        {
            this.locationsObversers.Add(observer);
        }

        private void NotifyLocationsObservers(List<CodeLocation> transformedLocations)
        {
            LocationEvent lEvent = new LocationEvent(transformedLocations);

            foreach (ILocationsObserver observer in locationsObversers)
            {
                observer.NotifyLocationsSelected(lEvent);
            }
        }
    }
}

/*/// <summary>
       /// Extract locations
       /// </summary>
       /// <param name="color"></param>
       public void Extract(Color color)
       {
           LocationRefactor.Location.LocationExtractor extractor = new LocationRefactor.Location.LocationExtractor(syntaxKind, solutionPath);
           List<Prog> programs = extractor.Extract(RegionsBeforeEdit, (Color)color);

           List<Prog> filtereds = new List<Prog>();
           foreach (Prog program in programs)
           {
               Prog val;
               if (!Progs.TryGetValue(program.ToString(), out val))
               {
                   Progs.Add(program.ToString(), program);
                   filtereds.Add(program);
               }
           }

           NotifyProgramGeneratedObservers(filtereds);
       }*/

/* private void NotifyLocationsTransformedObservers(List<Tuple<string, string>> transformedLocations)
         {
             LocationsTransformedEvent ltEvent = new LocationsTransformedEvent(transformedLocations);

             foreach (ILocationsTransformedObserver observer in locationsTransformedObserver) {
                 observer.NotifyLocationsTransformed(ltEvent);
             }
         }*/


/* /// <summary>
         /// Edit locations
         /// </summary>
         /// <param name="start">Start position</param>
         /// <param name="length">Region length</param>
         /// <param name="sourceCode">Source code</param>
         /// <param name="color">Color region</param>
         /// <param name="range">Region range</param>
         public void Edit(int start, int length, String sourceCode, Color color, Range range)
         {
             TRegion tregion = new TRegion();

             tregion.Start = start;
             tregion.Length = length;
             tregion.Text = sourceCode;
             tregion.Color = color;
             tregion.Range = range;

             List<TRegion> values;

             if (!RegionsBeforeEdit.TryGetValue(color, out values))
             {
                 values = new List<TRegion>();
                 RegionsBeforeEdit.Add(color, values);
             }

             values.Add(tregion);

             foreach (KeyValuePair<Color, List<TRegion>> dic in RegionsBeforeEdit)
             {
                 foreach (TRegion tr in dic.Value)
                 {
                     if (tregion.IsParent(tr))
                     {
                         tregion.Parent = tr;
                     }
                 }
             }

             if (tregion.Parent == null)
             {
                 tregion.Parent = RegionsBeforeEdit[Color.White][0];
             }
         }*/

///// <summary>
///// Load from file
///// </summary>
///// <param name="fileName">File name</param>
///// <returns>file content</returns>
//public string Load(String fileName)
//{
//    System.IO.StreamReader sr = new
//    System.IO.StreamReader(fileName);

//    String text = sr.ReadToEnd();

//    text = text.Replace("\r\n", "\n");
//    TRegion tRegion = new TRegion();
//    tRegion.Text = text;
//    tRegion.Color = Color.White;

//    List<TRegion> tRegions = new List<TRegion>();
//    tRegions.Add(tRegion);

//    RegionsBeforeEdit.Add(Color.White, tRegions);
//    sr.Close();

//    return text;
//}

///// <summary>
///// Refact a region
///// </summary>
///// <param name="program">Source code</param>
///// <param name="color">Selected color</param>
//public void Refact(String program, Color color)
//{
//    Prog prog = Progs[program];

//    LocationRefactor.Location.LocationExtractor extractor = new LocationRefactor.Location.LocationExtractor(syntaxKind, solutionPath);
//    String transformation = extractor.TransformProgram(RegionsBeforeEdit, RegionsAfterEdit, color, prog);

//    //List<Tuple<string, string>> transformedLocations = extractor.TransformLocations(RegionsBeforeEdit, RegionsAfterEdit, color, prog);
//    SynthesizedProgram synthesized = extractor.TransformationProgram(RegionsBeforeEdit, RegionsAfterEdit, color);

//    NotifyProgramRefactoredObservers(transformation);
//    NotifyLocationsTransformedObservers(synthesized, locations);
//}


///// <summary>
///// Edit done
///// </summary>
///// <param name="code">Source code</param>
///// <param name="start">Start position</param>
///// <param name="length">Region length</param>
//public void EditDone(String code, int start, int length)
//{
//    code = code.Replace("\r\n", "\n");
//    TRegion tRegion = new TRegion();
//    tRegion.Text = code;
//    tRegion.Color = Color.White;

//    List<TRegion> tRegions = new List<TRegion>();
//    tRegions.Add(tRegion);

//    RegionsAfterEdit[Color.White] = tRegions;

//    String text = code.Substring(start, length);
//    TRegion tregion = new TRegion();

//    Color color = Color.LightGreen;

//    tregion.Start = start;
//    tregion.Length = length;
//    tregion.Text = text;
//    tregion.Color = color;

//    List<TRegion> values;

//    if (!RegionsAfterEdit.TryGetValue(color, out values))
//    {
//        values = new List<TRegion>();
//        RegionsAfterEdit.Add(color, values);
//    }

//    values.Add(tregion);

//    foreach (KeyValuePair<Color, List<TRegion>> dic in RegionsAfterEdit)
//    {
//        foreach (TRegion tr in dic.Value)
//        {
//            if (tregion.IsParent(tr))
//            {
//                tregion.Parent = tr;
//            }
//        }
//    }

//    if (tregion.Parent == null)
//    {
//        tregion.Parent = RegionsAfterEdit[Color.White][0];
//    }
//}