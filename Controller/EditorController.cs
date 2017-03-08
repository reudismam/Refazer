using Controller;
using Controller.Event;
using ProseManager;
using Spg.Controller.Projects;
using System;
using System.Collections.Generic;

namespace Spg.Controller
{
    /// <summary>
    /// Controller for editor graphical interface
    /// </summary>
    public class EditorController
    {
        private List<IEditStartedObserver> _editStartedOIbservers = new List<IEditStartedObserver>();
        private List<IEditFinishedObserver> _editFinishedOIbservers = new List<IEditFinishedObserver>();

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
       
        public string before { get; set; }
        public string after { get; set; }
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

        public void Init(string before)
        {
            this.before = before;
            NotifyEditStartedObservers();
        }

        public void Transform(string after)
        {
            this.after = after;
            var examples = Tuple.Create(before, after);
            var refazer = new Refazer4CSharp();
            var rename = refazer.LearnTransformations(examples);
            NotifyEditFinishedObservers();
        }

        /// <summary>
        /// Notify highlight observers
        /// </summary>
        /// <param name="regions">Regions</param>
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
        private void NotifyEditFinishedObservers()
        {
            EditFinishedEvent hEvent = new EditFinishedEvent();
            foreach (var observer in _editFinishedOIbservers)
            {
                observer.EditFinished(hEvent);
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
    }
}





