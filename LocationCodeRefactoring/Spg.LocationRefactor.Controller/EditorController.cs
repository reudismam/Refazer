using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Text.Projection;
using Spg.ExampleRefactoring.Bean;
using Spg.ExampleRefactoring.LCS;
using Spg.ExampleRefactoring.Projects;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Util;
using Spg.LocationRefactor.Location;
using Spg.LocationRefactor.Observer;
using Spg.LocationRefactor.Operator.Filter;
using Spg.LocationRefactor.Operator.Map;
using Spg.LocationRefactor.Program;
using Spg.LocationRefactor.TextRegion;
using Spg.LocationRefactor.Transform;

namespace Spg.LocationRefactor.Controller
{
    /// <summary>
    /// Controller for editor graphical interface
    /// </summary>
    public class EditorController
    {
        private List<IHilightObserver> _hilightObservers;
        private List<IProgramsGeneratedObserver> _programGeneratedObservers;
        private List<IProgramRefactoredObserver> _programsRefactoredObserver;
        private List<ILocationsTransformedObserver> _locationsTransformedObserver;
        private List<ILocationsObserver> _locationsObversers;

        /// <summary>
        /// Locations
        /// </summary>
        /// <returns>Locations</returns>
        public List<CodeLocation> Locations { get; set; }

        /// <summary>
        /// Transformation
        /// </summary>
        /// <returns>transformation</returns>
        public List<CodeTransformation> CodeTransformations { get; set; }

        /// <summary>
        /// Tuple of source code before and after for every class edited.
        /// </summary>
        /// <returns>Transformation</returns>
        public List<Transformation> SourceTransformations { get; set; }

        /// <summary>
        /// Regions before the transformation
        /// </summary>
        /// <returns></returns>
        public List<TRegion> SelectedLocations { get; set; }

        /// <summary>
        /// Learned programs
        /// </summary>
        public List<Prog> Progs { get; set; }

        /// <summary>
        /// Informatios about the project
        /// </summary>
        public ProjectInformation ProjectInformation { get; set; }

        /// <summary>
        /// Code before transformation
        /// </summary>
        /// <returns></returns>
        public string CurrentViewCodeBefore { get; set; }

        /// <summary>
        /// Code before transformation
        /// </summary>
        /// <returns>Code before transformation</returns>
        public string CurrentViewCodeAfter { get; set; }

        /// <summary>
        /// Current view source code path
        /// </summary>
        /// <returns></returns>
        public string CurrentViewCodePath { get; set; }

        /// <summary>
        /// Documents before after edition
        /// </summary>
        public List<Tuple<string, string>> DocumentsBeforeAndAfter { get; set; }

        /// <summary>
        /// Map of edited regions
        /// </summary>
        public Dictionary<string, IProjectionBuffer> ProjectionBuffers { get; set; }

        /// <summary>
        /// List of edited locations
        /// </summary>
        public Dictionary<string, List<Selection>> EditedLocations { get; set; }

        /// <summary>
        /// Files opened on window
        /// </summary>
        public Dictionary<string, bool> FilesOpened { get; set; }

        /// <summary>
        /// Locations computed so far
        /// </summary>
        public Tuple<List<CodeLocation>, List<CodeLocation>> LocationsFoundSoFar;

        /// <summary>
        /// Program computed for negative filtering
        /// </summary>
        /// <returns>Program learned for negative filtering</returns>
        public List<Prog> ProgramsWithNegatives { get; set; }

        /// <summary>
        /// Least Common ancestor of selected nodes
        /// </summary>
        public List<SyntaxNode> Lcas { get; set; }

        /// <summary>
        /// Synthesized program
        /// </summary>
        public SynthesizedProgram Program { get; set; }

        public List<TRegion> CodeTransformationsExceptions { get; set; }

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static EditorController _instance;

        /// <summary>
        /// Constructor
        /// </summary>
        private EditorController()
        {
            Init();
        }

        /// <summary>
        /// Set solution
        /// </summary>
        /// <param name="solution">Solution</param>
        public void SetSolution(string solution)
        {
            ProjectInformation.SolutionPath = solution;
        }

        /// <summary>
        /// Set project list
        /// </summary>
        /// <param name="project">Projects</param>
        public void SetProject(List<string> project)
        {
            ProjectInformation.ProjectPath = project;
        }

        /// <summary>
        /// Start controller
        /// </summary>

        public void Init()
        {
            SelectedLocations = new List<TRegion>();
            _hilightObservers = new List<IHilightObserver>();
            _programGeneratedObservers = new List<IProgramsGeneratedObserver>();
            _programsRefactoredObserver = new List<IProgramRefactoredObserver>();
            _locationsTransformedObserver = new List<ILocationsTransformedObserver>();
            _locationsObversers = new List<ILocationsObserver>();
            //CodeTransformations = new List<CodeTransformation>();
            FilesOpened = new Dictionary<string, bool>();
            ProjectInformation = ProjectInformation.GetInstance();
            Program = null;
            LCAManager.Init();
            RegionManager.Init();
            BoundaryManager.Init();
            //Decomposer.Init();
            //LCAManager.Init();
        }

        /// <summary>
        /// Reinit Editor controller
        /// </summary>
        public static void ReInit()
        {
            _instance = null;
        }

        /// <summary>
        /// Get singleton EditorController instance
        /// </summary>
        /// <returns>Editor controller instance</returns>
        public static EditorController GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EditorController();
            }
            return _instance;
        }

        /// <summary>
        /// Decompose locations
        /// </summary>
        public void Extract()
        {
            Lcas = RegionManager.LeastCommonAncestors(SelectedLocations);
            LocationExtractor extractor = new LocationExtractor();
            //remove
            JsonUtil<List<TRegion>>.Write(SelectedLocations, "input_selection.json");
            //remove
            Progs = extractor.Extract(SelectedLocations);

            var result = RegionManager.GetInstance().GroupRegionBySourceFile(SelectedLocations);
            if (result.Count == 1)
            {
                Progs = RecomputeWithNegativeLocations();
            }

            NotifyLocationProgramGeneratedObservers(Progs);
        }

        /// <summary>
        /// Extract applying positives and negatives examples
        /// </summary>
        /// <param name="positives"></param>
        /// <param name="negatives"></param>
        public void Extract(List<TRegion> positives, List<TRegion> negatives)
        {
            LocationExtractor extractor = new LocationExtractor();
            ProgramsWithNegatives = extractor.Extract(positives, negatives);
            NotifyLocationProgramGeneratedObservers(ProgramsWithNegatives);
        }

        /// <summary>
        /// Analize locations
        /// </summary>
        /// <returns>Location programs</returns>
        private List<Prog> RecomputeWithNegativeLocations()
        {
            if (SelectedLocations == null || !SelectedLocations.Any()) { throw new Exception("Selected regions cannot be null or empty."); }

            SyntaxNode lca = RegionManager.LeastCommonAncestor(CurrentViewCodeBefore, SelectedLocations);
            List<TRegion> regions = RetrieveLocations(lca, CurrentViewCodeBefore, Progs.First());
            regions = NonDuplicateLocations(regions);

            TRegion deepestRegion = SelectedLocations.First();
            for (int i = 1; i < SelectedLocations.Count; i++)
            {
                if (SelectedLocations[i].Start > deepestRegion.Start)
                {
                    deepestRegion = SelectedLocations[i];
                }
            }

            List<TRegion> negativesExamples = new List<TRegion>();
            foreach (TRegion region in regions)
            {
                if (region.Start > deepestRegion.Start) { break; } //after deepest region does not work as negative example.
                bool negative = true;
                foreach (TRegion selectedRegion in SelectedLocations)
                {
                    if (selectedRegion.Start <= region.Start && region.Start <= selectedRegion.Start + selectedRegion.Length)
                    {
                        negative = false; break;
                    }

                    if (region.Start <= selectedRegion.Start && selectedRegion.Start <= region.Start + region.Length)
                    {
                        negative = false; break;
                    }
                }

                if (negative)
                {
                    region.Parent = deepestRegion.Parent;
                    negativesExamples.Add(region);
                }
            }

            if (!negativesExamples.Any())
            {
                return Progs;
            }
            LocationExtractor extrator = new LocationExtractor();

            var withNegatives = extrator.Extract(SelectedLocations, negativesExamples);

            if (withNegatives.Any())
            {
                Progs = withNegatives;
            }

            return Progs;
        }

        /// <summary>
        /// Undo systematic editing
        /// </summary>
        public void Undo()
        {
            foreach (Transformation transformation in SourceTransformations)
            {
                string path = transformation.SourcePath;
                string sourceCode = transformation.transformation.Item1;
                FileUtil.WriteToFile(path, sourceCode);
            }
            CodeTransformations = null;
        }

        /// <summary>
        /// Edit locations
        /// </summary>
        /// <param name="start">Start position</param>
        /// <param name="length">Region length</param>
        /// <param name="span">Text selected by the span</param>
        /// <param name="sourceCode">Source code</param>
        public void Edit(int start, int length, string span, string sourceCode)
        {
            TRegion tregion = new TRegion();
            Color color = Color.LightGreen;
            tregion.Start = start;
            tregion.Length = length;
            tregion.Text = span;
            tregion.Color = color;
            tregion.Path = CurrentViewCodePath;

            SelectedLocations.Add(tregion);

            if (tregion.Parent == null)
            {
                TRegion parent = new TRegion
                {
                    Start = 0,
                    Length = sourceCode.Length,
                    Text = sourceCode,
                    Path = CurrentViewCodePath,
                    Color = Color.White
                };
                tregion.Parent = parent;
            }
        }

        /// <summary>
        /// Open files
        /// </summary>
        /// <returns>Open files</returns>
        public IEnumerable<string> OpenFiles()
        {
            List<string> files = new List<string>();
            foreach (KeyValuePair<string, bool> file in FilesOpened)
            {
                if (file.Value)
                {
                    files.Add(file.Key);
                }
            }
            return files;
        }

        /// <summary>
        /// Retrieve locations
        /// </summary>
        public void RetrieveLocations()
        {
            if (ProgramsWithNegatives != null)
            {
                RetrieveLocationsPosNegatives();
                return;
            }

            Prog prog = Progs.First();
            Dictionary<string, List<TRegion>> list = RegionManager.GetInstance().GroupRegionBySourceFile(SelectedLocations);

            List<CodeLocation> locationsList;
            if (list.Count() > 1)
            {
                locationsList = RetrieveLocationsMultiplesSourceClasses(prog);
            }
            else
            {
                locationsList = RetrieveLocationsSingleSourceClass(prog);
            }
            var sourceLocations = locationsList;        
            Locations = NonDuplicateLocations(sourceLocations);

            //remove
            List<Selection> selections = new List<Selection>();
            foreach (CodeLocation location in Locations)
            {
                Selection selection = new Selection(location.Region.Start, location.Region.Length, location.SourceClass, location.SourceCode, location.Region.Text);
                selections.Add(selection);
            }
            JsonUtil<List<Selection>>.Write(selections, "found_locations.json");
            //remove
            Dictionary<string, List<CodeLocation>> codeLocs = RegionManager.GetInstance().GroupLocationsBySourceFile(Locations);
            Tuple<List<CodeLocation>, List<CodeLocation>> tp = Tuple.Create(Locations, codeLocs[CurrentViewCodePath.ToUpperInvariant()]);

            LocationsFoundSoFar = tp;
            NotifyLocationsObservers(Locations);
        }

        /// <summary>
        /// Retrieve locations after developer indicate negative locations
        /// </summary>
        public void RetrieveLocationsPosNegatives()
        {
            Prog prog = ProgramsWithNegatives.First();
            Locations = RetrieveLocationsPosNegative(prog);

            //remove
            List<Selection> selections = new List<Selection>();
            foreach (CodeLocation location in Locations)
            {
                Selection selection = new Selection(location.Region.Start, location.Region.Length, location.SourceClass, location.SourceCode, location.Region.Text);
                selections.Add(selection);
            }
            JsonUtil<List<Selection>>.Write(selections, "found_locations.json");
            //remove
            Dictionary<string, List<CodeLocation>> codeLocs = RegionManager.GetInstance().GroupLocationsBySourceFile(Locations);
            Tuple<List<CodeLocation>, List<CodeLocation>> tp = Tuple.Create(Locations, codeLocs[CurrentViewCodePath.ToUpperInvariant()]);

            LocationsFoundSoFar = tp;
            NotifyLocationsObservers(Locations);
        }

        /// <summary>
        /// Indicate that the location process ended
        /// </summary>
        public void Done()
        {
            NotifyHilightObservers(LocationsFoundSoFar.Item2);
        }

        /// <summary>
        /// Retrieve location for a single class
        /// </summary>
        /// <param name="prog">Learned program</param>
        /// <returns>Code locations</returns>
        private List<CodeLocation> RetrieveLocationsSingleSourceClass(Prog prog)
        {
            List<CodeLocation> sourceLocations = new List<CodeLocation>();
            SyntaxNode lca = RegionManager.LeastCommonAncestor(CurrentViewCodeBefore, SelectedLocations);
            List<TRegion> regions = RetrieveLocations(lca, CurrentViewCodeBefore, prog);
            foreach (TRegion region in regions)
            {
                CodeLocation location = new CodeLocation { Region = region, SourceCode = CurrentViewCodeBefore, SourceClass = CurrentViewCodePath };
                sourceLocations.Add(location);
            }
               
            return sourceLocations;
        }

        /// <summary>
        /// Retrieve location single class after developer inform negative locations
        /// </summary>
        /// <param name="prog">Program</param>
        /// <returns>Locations for single class considering negative posistions</returns>
        private List<CodeLocation> RetrieveLocationsPosNegative(Prog prog)
        {
            List<CodeLocation> locations = new List<CodeLocation>();
            foreach (CodeLocation location in Locations)
            {
                MapBase m = (MapBase)prog.Ioperator;
                FilterBase filter = (FilterBase)m.SequenceExpression.Ioperator;
                bool isTruePositive = filter.IsMatch(location.Region.Node);
                if (isTruePositive)
                {
                    locations.Add(location);
                }
            }

            return locations;
        }

        /// <summary>
        /// Retrieve regions for multiple classes
        /// </summary>
        /// <param name="prog">Learned program</param>
        /// <returns>Code locations</returns>
        private List<CodeLocation> RetrieveLocationsMultiplesSourceClasses(Prog prog)
        {
            List<CodeLocation> sourceLocations = new List<CodeLocation>();

            List<TRegion> regions = RetrieveLocations(prog);
            foreach (TRegion region in regions)
            {
                CodeLocation location = new CodeLocation
                {
                    Region = region,
                    SourceCode = region.Parent.Text,
                    SourceClass = region.Path
                };

                sourceLocations.Add(location);
            }
            return sourceLocations;
        }

        /// <summary>
        /// Retrieve locations
        /// </summary>
        /// <param name="prog">Location program</param>
        public List<TRegion> RetrieveLocations(Prog prog)
        {
            LocationExtractor extractor = new LocationExtractor();
            List<TRegion> regions = extractor.RetrieveString(prog);
            return regions;
        }

        /// <summary>
        /// Retrieve location
        /// </summary>
        /// <param name="lca">Least common ancestor</param>
        /// <param name="sourceCode">Source code</param>
        /// <param name="prog">Program</param>
        /// <returns>Locations</returns>
        private List<TRegion> RetrieveLocations(SyntaxNode lca, string sourceCode, Prog prog)
        {
            LocationExtractor extractor = new LocationExtractor();
            List<TRegion> regions = extractor.RetrieveString(prog, sourceCode, lca);
            return regions;
        }

        /// <summary>
        /// Selected only non duplicate locations
        /// </summary>
        /// <param name="locations">All locations</param>
        /// <returns>Non duplicate locations</returns>
        private List<CodeLocation> NonDuplicateLocations(List<CodeLocation> locations)
        {
            locations = SegmentBySyntaxKind(locations);
            locations = EnclosedSyntaxKind(locations);
            return locations;
            //List<CodeLocation> clocations = new List<CodeLocation>();
            //var groupedLocations = RegionManager.GetInstance().GroupLocationsBySourceFile(locations);

            //foreach (KeyValuePair<string, List<CodeLocation>> item in groupedLocations)
            //{
            //    bool[] analized = Enumerable.Repeat(false, item.Value.Count).ToArray();
            //    for (int i = 0; i < item.Value.Count; i++)
            //    {
            //        CodeLocation location = item.Value[i];
            //        CodeLocation choosed = location;
            //        if (!analized[i])
            //        {
            //            for (int j = i + 1; j < item.Value.Count; j++)
            //            {
            //                var otherLocation = item.Value[j];
            //                bool intersect = location.Region.IntersectWith(otherLocation.Region);
            //                if (intersect && location.Region.Path.ToUpperInvariant().Equals(otherLocation.Region.Path.ToUpperInvariant()))
            //                {
            //                    analized[j] = true;
            //                    if (otherLocation.Region.Start > location.Region.Start)
            //                    {
            //                        choosed = otherLocation;
            //                    }
            //                }
            //            }
            //            clocations.Add(choosed);
            //        }
            //    }
            //}

            //if(locations.Count != clocations.Count) throw new Exception("Error in calculating non-duplicated locations."); 
            //return clocations;
        }

        private List<CodeLocation> EnclosedSyntaxKind(List<CodeLocation> locations)
        {
            Dictionary<CodeLocation, List<CodeLocation>> intersections = new Dictionary<CodeLocation, List<CodeLocation>>();

            var grouped = RegionManager.GetInstance().GroupLocationsBySourceFile(locations);
            foreach (KeyValuePair<string, List<CodeLocation>> item in grouped)
            {
                foreach (var location in item.Value)
                {
                    TRegion locRegion = new TRegion();
                    locRegion.Start = location.Region.Node.SpanStart;
                    locRegion.Length = location.Region.Node.Span.Length;
                    foreach (var otherLocation in item.Value)
                    {
                        TRegion locOtherRegion = new TRegion();
                        locOtherRegion.Start = otherLocation.Region.Node.SpanStart;
                        locOtherRegion.Length = otherLocation.Region.Node.Span.Length;
                        if (!(location.Equals(otherLocation)) && locOtherRegion.IsInside(locRegion) && !location.Region.Node.IsKind(otherLocation.Region.Node.CSharpKind()))
                        {
                            if (!intersections.ContainsKey(location))
                            {
                                intersections.Add(location, new List<CodeLocation>());
                            }
                            intersections[location].Add(otherLocation);
                        }
                    }
                }
            }

            List<CodeLocation> removes = new List<CodeLocation>();
            bool isLocationInsideAnother = ExistInsideAntoher(intersections);
            foreach (var codeLocation in locations)
            {
                if (intersections.ContainsKey(codeLocation))
                {
                    if (intersections[codeLocation].Count > 1)
                    {
                        foreach (var cdLocation in intersections[codeLocation])
                        {
                            removes.Add(cdLocation);
                        }
                    }else if (intersections[codeLocation].Count == 1)
                    {
                        if (codeLocation.Region.Start == intersections[codeLocation].First().Region.Start)
                        {
                            if (isLocationInsideAnother)
                            {
                                removes.Add(codeLocation);
                            }
                            else
                            {
                                removes.Add(intersections[codeLocation].First());
                            }
                        }
                        else
                        {
                            removes.Add(codeLocation);
                        }
                    }
                }   
            }

            locations = locations.Except(removes).ToList();
            return locations;
        }

        private bool ExistInsideAntoher(Dictionary<CodeLocation, List<CodeLocation>> intersections)
        {
            foreach (KeyValuePair<CodeLocation, List<CodeLocation>> item in intersections)
            {
                if (item.Value.Count() > 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Examine locations that are inside other locations
        /// </summary>
        /// <param name="locations">Locations</param>
        /// <returns>Remove locations that are inside other locations</returns>
        private List<CodeLocation> SegmentBySyntaxKind(List<CodeLocation> locations)
        {
            Dictionary<CodeLocation, List<CodeLocation>> intersections = new Dictionary<CodeLocation, List<CodeLocation>>();

            var grouped = RegionManager.GetInstance().GroupLocationsBySourceFile(locations);
            foreach (KeyValuePair<string, List<CodeLocation>> item in grouped)
            {
                foreach (var location in item.Value)
                {
                    TRegion locRegion = new TRegion();
                    locRegion.Start = location.Region.Node.SpanStart;
                    locRegion.Length = location.Region.Node.Span.Length;
                    foreach (var otherLocation in item.Value)
                    {
                        TRegion locOtherRegion = new TRegion();
                        locOtherRegion.Start = otherLocation.Region.Node.SpanStart;
                        locOtherRegion.Length = otherLocation.Region.Node.Span.Length;
                        if (!(location.Equals(otherLocation)) && locOtherRegion.IsInside(locRegion) && location.Region.Node.IsKind(otherLocation.Region.Node.CSharpKind()))
                        {
                            if (!intersections.ContainsKey(location))
                            {
                                intersections.Add(location, new List<CodeLocation>());
                            }
                            intersections[location].Add(otherLocation);
                        }
                    }
                }
            }

            List<CodeLocation> insides = new List<CodeLocation>();
            foreach (var codeLocation in locations)
            {
                if (!intersections.ContainsKey(codeLocation))
                {
                    insides.Add(codeLocation);
                }   
            }
            return insides;
        }

        /// <summary>
        /// Selected only non duplicate locations
        /// </summary>
        /// <param name="locations">All locations</param>
        /// <returns>Non duplicate locations</returns>
        private static List<TRegion> NonDuplicateLocations(List<TRegion> locations)
        {
            List<TRegion> nonDuplicateLocations = new List<TRegion>();
            foreach (TRegion location in locations)
            {
                bool distinct = true;
                foreach (TRegion region in nonDuplicateLocations)
                {
                    if (region.Start == location.Start && region.Length == location.Length)
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
        public void Refact()
        {
            CodeTransformations = new List<CodeTransformation>();
            long millBefore = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            FillEditedLocations();
            LocationExtractor extractor = new LocationExtractor();
            List<Transformation> transformations = extractor.TransformProgram(false);
            SourceTransformations = transformations;

            long millAfer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long totalTime = (millAfer - millBefore);
            FileUtil.WriteToFile(@"edit.t", totalTime.ToString());

            NotifyProgramRefactoredObservers(transformations);
            NotifyLocationsTransformedObservers();

//            ClearAfterRefact();
        }

        /// <summary>
        /// Fill edited locations. This method as created more for test purpose
        /// </summary>
        private void FillEditedLocations()
        {
            if (ProjectionBuffers == null) return;

            Dictionary<string, List<Selection>> dicSelections = new Dictionary<string, List<Selection>>();
            foreach (var item in ProjectionBuffers)
            {
                List<Selection> selections = new List<Selection>();
                foreach (var span in item.Value.CurrentSnapshot.GetSourceSpans())
                {
                    Selection selection = new Selection(span.Start, span.Length, item.Key, null, null);
                    selections.Add(selection);
                }
                dicSelections.Add(item.Key, selections);
            }

            EditedLocations = dicSelections;
            JsonUtil<Dictionary<string, List<Selection>>>.Write(dicSelections, "edited_selections.json");
        }


        /// <summary>
        /// Clear and start
        /// </summary>
        private void ClearAfterRefact()
        {
            //Init();
            SelectedLocations = new List<TRegion>();
            Progs = new List<Prog>();
            Locations = null;
            //CodeTransformations = new List<CodeTransformation>();
            CurrentViewCodeBefore = null;
            CurrentViewCodeAfter = null;
            CurrentViewCodePath = null;
            //ReInit();
        }

        /// <summary>
        /// Add highlight observer
        /// </summary>
        /// <param name="observer">Observer</param>
        public void AddHilightObserver(IHilightObserver observer)
        {
            _hilightObservers.Add(observer);
        }

        /// <summary>
        /// Notify highlight observers
        /// </summary>
        /// <param name="regions">Regions</param>
        private void NotifyHilightObservers(List<CodeLocation> regions)
        {
            HighlightEvent hEvent = new HighlightEvent(regions);
            foreach (IHilightObserver observer in _hilightObservers)
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
            _programGeneratedObservers.Add(observer);
        }

        /// <summary>
        /// Notify generated observers
        /// </summary>
        /// <param name="programs">Generated observers</param>
        private void NotifyLocationProgramGeneratedObservers(List<Prog> programs)
        {
            ProgramGeneratedEvent pEvent = new ProgramGeneratedEvent(programs);
            foreach (IProgramsGeneratedObserver observer in _programGeneratedObservers)
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
            _programsRefactoredObserver.Add(observer);
        }

        /// <summary>
        /// Notify program refactored observers
        /// </summary>
        /// <param name="transformations"></param>
        private void NotifyProgramRefactoredObservers(List<Transformation> transformations)
        {
            ProgramRefactoredEvent rEvent = new ProgramRefactoredEvent(transformations);

            foreach (IProgramRefactoredObserver observer in _programsRefactoredObserver)
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
            _locationsTransformedObserver.Add(observer);
        }

        /// <summary>
        /// Notify locations transformation observers
        /// </summary>
        private void NotifyLocationsTransformedObservers(/*SynthesizedProgram program, IEnumerable<CodeLocation> locations*/)
        {

            List<CodeTransformation> transformations = new List<CodeTransformation>();
            foreach (CodeTransformation codeTransformation in CodeTransformations)
            {
                CodeLocation location = new CodeLocation();
                location.SourceClass = codeTransformation.Location.SourceClass;
                location.SourceCode = codeTransformation.Location.SourceCode;
                TRegion region = new TRegion();
                region.Start = codeTransformation.Location.Region.Start;
                region.Length = codeTransformation.Location.Region.Length;
                region.Path = codeTransformation.Location.Region.Path;
                region.Parent = codeTransformation.Location.Region.Parent;
                region.Text = codeTransformation.Location.Region.Text;
                location.Region = region;

                CodeTransformation transformation = new CodeTransformation(location, codeTransformation.Trans, codeTransformation.Transformation);
                transformations.Add(transformation);
            }

            LocationsTransformedEvent ltEvent = new LocationsTransformedEvent(transformations);

            JsonUtil<List<CodeTransformation>>.Write(transformations, "transformed_locations.json");

            foreach (ILocationsTransformedObserver observer in _locationsTransformedObserver)
            {
                observer.NotifyLocationsTransformed(ltEvent);
            }
        }

        /// <summary>
        /// Add locations transformed observer
        /// </summary>
        /// <param name="observer">observer</param>
        public void AddLocationsObserver(ILocationsObserver observer)
        {
            _locationsObversers.Add(observer);
        }

        /// <summary>
        /// Notify locations observers
        /// </summary>
        /// <param name="locations">Code locations</param>
        private void NotifyLocationsObservers(List<CodeLocation> locations)
        {
            LocationEvent lEvent = new LocationEvent(locations);

            foreach (ILocationsObserver observer in _locationsObversers)
            {
                observer.NotifyLocationsSelected(lEvent);
            }
        }
    }
}





