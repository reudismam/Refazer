//using FastColoredTextBoxNS;
using Microsoft.CodeAnalysis.CSharp;
using Spg.LocationCodeRefactoring.Observer;
using Spg.LocationRefactor.Program;
using Spg.LocationRefactor.TextRegion;
using System;
using System.Collections.Generic;
using System.Drawing;
using Spg.LocationRefactor.Location;
using Spg.ExampleRefactoring.Synthesis;
using LocationCodeRefactoring.Spg.LocationRefactor.Transformation;
using Spg.ExampleRefactoring.AST;

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

        private List<CodeLocation> locations;

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

        public string selec { get; set; }

        public string CodeBefore { get; set; }

        public string CodeAfter { get; set; }

        private static EditorController instance;

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

        public static EditorController GetInstance()
        {
            if(instance == null)
            {
                instance = new EditorController(SyntaxKind.MethodDeclaration, "");
            }
            return instance;
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

        /// <summary>
        /// Extract locations
        /// </summary>
        /// <param name="color"></param>
        public void Extract()
        {
            Color color = Color.LightGreen;
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
        }

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
            //tregion.Range = range;

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
                //tregion.Parent = RegionsBeforeEdit[Color.White][0];
                TRegion parent = new TRegion();
                parent.Start = 0;
                parent.Length = sourceCode.Length;
                parent.Text = sourceCode;
                parent.Color = Color.White;
                tregion.Parent = parent;
            }
        }

        /// <summary>
        /// Load from file
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>file content</returns>
        public string Load(String fileName)
        {
            System.IO.StreamReader sr = new
            System.IO.StreamReader(fileName);

            String text = sr.ReadToEnd();

            text = text.Replace("\r\n", "\n");
            TRegion tRegion = new TRegion();
            tRegion.Text = text;
            tRegion.Color = Color.White;

            List<TRegion> tRegions = new List<TRegion>();
            tRegions.Add(tRegion);

            RegionsBeforeEdit.Add(Color.White, tRegions);
            sr.Close();

            return text;
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
            LocationRefactor.Location.LocationExtractor extractor = new LocationRefactor.Location.LocationExtractor(syntaxKind, solutionPath);
            List<Tuple<string, string>> sourceFiles = extractor.SourceFiles(solutionPath);

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
                //Tuple<string, List<TRegion>> locationTuple = Tuple.Create(source, regions);
                //sourceLocations.Add(locationTuple);
            }

            List<TRegion> rs = extractor.RetrieveString(prog, program);

            this.locations = sourceLocations;
            NotifyHilightObservers(rs);
            NotifyLocationsObservers(sourceLocations);
        }

        /// <summary>
        /// Edit done
        /// </summary>
        /// <param name="code">Source code</param>
        /// <param name="start">Start position</param>
        /// <param name="length">Region length</param>
        public void EditDone(String code, int start, int length)
        {
            code = code.Replace("\r\n", "\n");
            TRegion tRegion = new TRegion();
            tRegion.Text = code;
            tRegion.Color = Color.White;

            List<TRegion> tRegions = new List<TRegion>();
            tRegions.Add(tRegion);

            RegionsAfterEdit[Color.White] = tRegions;

            String text = code.Substring(start, length);
            TRegion tregion = new TRegion();

            Color color = Color.LightGreen;

            tregion.Start = start;
            tregion.Length = length;
            tregion.Text = text;
            tregion.Color = color;

            List<TRegion> values;

            if (!RegionsAfterEdit.TryGetValue(color, out values))
            {
                values = new List<TRegion>();
                RegionsAfterEdit.Add(color, values);
            }

            values.Add(tregion);

            foreach (KeyValuePair<Color, List<TRegion>> dic in RegionsAfterEdit)
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
                tregion.Parent = RegionsAfterEdit[Color.White][0];
            }
        }

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

        /// <summary>
        /// Refact a region
        /// </summary>
        /// <param name="program">Source code</param>
        /// <param name="color">Selected color</param>
        public void Refact()
        {
            Prog prog = Progs[selec];
            Color color = Color.LightGreen;

            LocationRefactor.Location.LocationExtractor extractor = new LocationRefactor.Location.LocationExtractor(syntaxKind, solutionPath);
            String transformation = extractor.TransformProgram(RegionsBeforeEdit, RegionsAfterEdit, color, prog);

            //List<Tuple<string, string>> transformedLocations = extractor.TransformLocations(RegionsBeforeEdit, RegionsAfterEdit, color, prog);
            SynthesizedProgram synthesized = extractor.TransformationProgram(RegionsBeforeEdit, RegionsAfterEdit, color);

            NotifyProgramRefactoredObservers(transformation);
            NotifyLocationsTransformedObservers(synthesized, locations);
        }

        /// <summary>
        /// Add hilight observer
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
        /// <param name="observer">Pbserver</param>
        public void AddProgramRefactoredObserver(IProgramRefactoredObserver observer)
        {
            this.programsRefactoredObserver.Add(observer);
        }

        /// <summary>
        /// Notify program refactored observers
        /// </summary>
        /// <param name="transformation"></param>
        private void NotifyProgramRefactoredObservers(string transformation)
        {
            ProgramRefactoredEvent rEvent = new ProgramRefactoredEvent(transformation);

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

        /* private void NotifyLocationsTransformedObservers(List<Tuple<string, string>> transformedLocations)
         {
             LocationsTransformedEvent ltEvent = new LocationsTransformedEvent(transformedLocations);

             foreach (ILocationsTransformedObserver observer in locationsTransformedObserver) {
                 observer.NotifyLocationsTransformed(ltEvent);
             }
         }*/

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
