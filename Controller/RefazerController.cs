using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Controller.Event;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Utils;
using RefazerFunctions.Bean;
using RefazerManager;
using Spg.Controller.Projects;
using TreeEdit.Spg.Log;
using TreeEdit.Spg.LogInfo;
using TreeElement.Spg.Node;
using WorkSpaces.Spg.Workspace;

namespace Controller
{

    /// <summary>
    /// Controller for editor graphical interface
    /// </summary>
    public class RefazerController
    {
        private readonly List<IEditStartedObserver> _editStartedOIbservers = new List<IEditStartedObserver>();
        private readonly List<ITransformationFinishedObserver> _transformationFinishedOIbservers = new List<ITransformationFinishedObserver>();

        /// <summary>
        /// Gets the found locations
        /// </summary>
        /// <returns></returns>
        public List<SyntaxNodeOrToken> GetLocations()
        {
            return CodeFragmentsInfo.GetInstance().Locations;
        }

        /// <summary>
        /// Gets the transformations performed into the source code in terms of
        /// sourceCodeBefore and afterSourceCode code fragments
        /// </summary>
        public List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>> GetTransformations()
        {
            return TransformationsInfo.GetInstance().Transformations;
        }

        /// <summary>
        /// Current Program
        /// </summary>
        public ProgramNode CurrentProgram
        {
            get;
            set;
        }

        private Grammar Grammar { get; set; }

        /// <summary>
        /// Informations about the project
        /// </summary>
        public ProjectInformation ProjectInfo { get; set; }

        /// <summary>
        /// Code sourceCodeBefore transformation
        /// </summary>
        /// <returns></returns>
        public string CurrentViewCodeBefore { get; set; }

        /// <summary>
        /// Code sourceCodeBefore transformation
        /// </summary>
        /// <returns>Code sourceCodeBefore transformation</returns>
        public string CurrentViewCodeAfter { get; set; }

        /// <summary>
        /// Current view source code path
        /// </summary>
        /// <returns></returns>
        public string CurrentViewCodePath { get; set; }

        /// <summary>
        /// Documents sourceCodeBefore afterSourceCode edition
        /// </summary>
        public List<Tuple<string, string>> DocumentsBeforeAndAfter { get; set; }

        /// <summary>
        /// Defines a list of source code before the modifications of the developer
        /// </summary>
        public List<Tuple<string, string>> BeforeSourceCodeList { get; set; }

        /// <summary>
        /// Defines a list of source code after the modifications of the developer
        /// </summary>
        public List<Tuple<String, String>> AfterSourceCodeList { get; set; }
        /// <summary>
        /// Files opened on window
        /// </summary>
        public Dictionary<string, bool> FilesOpened { get; set; }

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static RefazerController _instance;

        /// <summary>
        /// Constructor
        /// </summary>
        private RefazerController()
        {
            ProjectInfo = ProjectInformation.GetInstance();
        }

        /// <summary>
        /// Set solution
        /// </summary>
        /// <param name="solution">Solution</param>
        public void SetSolution(string solution)
        {
            ProjectInfo.SolutionPath = solution;
        }

        /// <summary>
        /// Set project list
        /// </summary>
        /// <param name="project">Projects</param>
        public void SetProject(List<string> project)
        {
            ProjectInfo.ProjectPath = project;
        }

        /// <summary>
        /// Get singleton EditorController instance
        /// </summary>
        /// <returns>Editor controller instance</returns>
        public static RefazerController GetInstance()
        {
            if (_instance == null)
            {
                _instance = new RefazerController();
            }
            return _instance;
        }

        /// <summary>
        /// Gets opened files
        /// </summary>
        /// <returns>Open files</returns>
        [SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
        public IEnumerable<string> OpenFiles()
        {
            var files = new List<string>();
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
        /// Indicates the start of the transformation
        /// </summary>
        public void Init()
        {
            NotifyEditStartedObservers();
        }

        /// <summary>
        /// Specifies the start of the learning of transformations
        /// </summary>
        public void Transform()
        {
            var exampleTuples = Tuple.Create(BeforeSourceCodeList, AfterSourceCodeList);
            var examples = GetExamples(exampleTuples);
            var refazer = new Refazer4CSharp();
            Grammar = Refazer4CSharp.GetGrammar();
            CurrentProgram = refazer.LearnTransformation(Grammar, examples);
            ExecuteProgram();
            NotifyTransformationFinishedObservers();
        }

        private List<Tuple<string, string>> GetExamples(Tuple<List<Tuple<string, string>>, List<Tuple<string, string>>> exampleTuples)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executes the specified program. If not program exists, it uses the first program learned.
        /// </summary>
        /// <param name="program">Program to be executed</param>
        private void ExecuteProgram(ProgramNode program = null)
        {
            if (program == null) program = CurrentProgram;
            var asts = new List<SyntaxNodeOrToken>();
            var files = WorkspaceManager.GetInstance().GetSourcesFiles(null, ProjectInfo.SolutionPath);
            foreach (var v in files)
            {
                var tree = CSharpSyntaxTree.ParseText(v.Item1, path: v.Item2).GetRoot();
                asts.Add(tree);
            }
            foreach (var ast in asts)
            {
                var newInputState = State.Create(Grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(ast)));
                program.Invoke(newInputState);
            }
        }

        /// <summary>
        /// Notifies highlight observers
        /// </summary>
        private void NotifyEditStartedObservers()
        {
            EditStartedEvent hEvent = new EditStartedEvent();
            foreach (var observer in _editStartedOIbservers)
            {
                observer.EditStarted(hEvent);
            }
        }


        /// <summary>
        /// Notify highlight observers
        /// </summary>
        /// <param name="regions">Regions</param>
        private void NotifyTransformationFinishedObservers()
        {
            TransformationFinishedEvent hEvent = new TransformationFinishedEvent();
            foreach (var observer in _transformationFinishedOIbservers)
            {
                observer.TransformationFinished(hEvent);
            }
        }

        /// <summary>
        /// Add program generated observer
        /// </summary>
        /// <param name="observer">Observer</param>
        public void AddEditStartedObserver(IEditStartedObserver observer)
        {
            _editStartedOIbservers.Add(observer);
        }

        public void AddTransformationFinishedObserver(ITransformationFinishedObserver observer)
        {
            _transformationFinishedOIbservers.Add(observer);
        }
    }
}





