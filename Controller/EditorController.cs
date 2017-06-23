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
using ProseManager;
using RefazerFunctions.Spg.Bean;
using Spg.Controller.Projects;
using TreeElement.Spg.Node;
using WorkSpaces.Spg.Workspace;

namespace Controller
{
    
    /// <summary>
    /// Controller for editor graphical interface
    /// </summary>
    public class EditorController
    {
        private readonly List<IEditStartedObserver> _editStartedOIbservers = new List<IEditStartedObserver>();
        private readonly List<ITransformationFinishedObserver> _transformationFinishedOIbservers = new List<ITransformationFinishedObserver>();

        /// <summary>
        /// Current Program
        /// </summary>
        public ProgramNode CurrentProgram
        {
            get;
            set;
        } 

        private Grammar grammar { get; set; }

        public List<object> Transformed { get; set; }

        /// <summary>
        /// Informations about the project
        /// </summary>
        public ProjectInformation ProjectInfo { get; set; }

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
       
        public string Before { get; set; }
        public string After { get; set; }
        /// <summary>
        /// Files opened on window
        /// </summary>
        public Dictionary<string, bool> FilesOpened { get; set; }  

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static EditorController _instance;

        /// <summary>
        /// Constructor
        /// </summary>
        private EditorController()
        {
            ProjectInfo = ProjectInformation.GetInstance();
            Transformed = new List<object>();
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
        [SuppressMessage("ReSharper", "ConvertIfStatementToNullCoalescingExpression")]
        public static EditorController GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EditorController();
            }
            return _instance;
        }       

        /// <summary>
        /// Open files
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

        public void Transform(string after)
        {
            this.After = after;
            var examples = Tuple.Create(Before, after);
            var refazer = new Refazer4CSharp();
            grammar = Refazer4CSharp.GetGrammar();
            CurrentProgram = refazer.LearnTransformations(grammar, examples);
            ExecuteProgram();
            NotifyTransformationFinishedObservers();
        }

        [SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
        private Dictionary<string, List<object>> ExecuteProgram()
        {         
            var asts = new List<SyntaxNodeOrToken>();
            if (ProjectInfo.SolutionPath == null)
            {
                //Run program
                return null;
            }
            var files = WorkspaceManager.GetInstance().GetSourcesFiles(null, ProjectInfo.SolutionPath);
            foreach (var v in files)
            {
                var tree = CSharpSyntaxTree.ParseText(v.Item1, path: v.Item2).GetRoot();
                asts.Add(tree);
            }

            var dicTrans = new Dictionary<string, List<object>>();
            foreach (var ast in asts)
            {
                var newInputState = State.Create(grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(ast)));
                object[] output = CurrentProgram.Invoke(newInputState).ToEnumerable().ToArray();

                Transformed.AddRange(output);
                if (output.Any())
                {
                    if (dicTrans.ContainsKey(ast.SyntaxTree.FilePath.ToUpperInvariant()))
                    {
                        dicTrans[ast.SyntaxTree.FilePath.ToUpperInvariant()].AddRange(output);
                    }
                    else
                    {
                        dicTrans[ast.SyntaxTree.FilePath.ToUpperInvariant()] = output.ToList();
                    }
                }
            }
            return dicTrans;
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
        /// Notifies highlight observers
        /// </summary>
        private void NotifyTransformationFinishedObservers()
        {
            TransformationFinishedEvent hEvent = new TransformationFinishedEvent();
            foreach (var observer in _transformationFinishedOIbservers)
            {
                observer.TransformationFinished(hEvent);
            }
        }

        /// <summary>
        /// Adds program generated observer
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





