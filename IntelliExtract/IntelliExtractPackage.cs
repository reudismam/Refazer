using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ExampleRefactoring.Spg.ExampleRefactoring.Util;
using LocateAdornment;
using LocationCodeRefactoring.Spg.LocationCodeRefactoring.Controller;
using LocationCodeRefactoring.Spg.LocationRefactor.Location;
using LocationCodeRefactoring.Spg.LocationRefactor.Program;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Projection;
using Microsoft.VisualStudio.TextManager.Interop;
using Spg.LocationCodeRefactoring.Observer;
using Spg.LocationRefactor.Location;
using Spg.LocationRefactor.TextRegion;
using DefGuidList = Microsoft.VisualStudio.Editor.DefGuidList;
using IServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace SPG.IntelliExtract
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidIntelliExtractPkgString)]
    public sealed class IntelliExtractPackage : Package, IProgramsGeneratedObserver, ILocationsObserver, IHilightObserver
    {
        private List<Prog> _programs;

        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>

        public IntelliExtractPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
            EditorController controler = EditorController.GetInstance();
            controler.AddProgramGeneratedObserver(this);
            controler.AddLocationsObserver(this);
            controler.AddHilightObserver(this);
        }

        public void NotifyHilightChanged(HighlightEvent hEvent)
        {
            IVsTextManager txtMgr = (IVsTextManager)GetService(typeof(SVsTextManager));
            IVsTextView vTextView = null;
            const int mustHaveFocus = 1;
            txtMgr.GetActiveView(mustHaveFocus, null, out vTextView);

            IVsUserData userData = vTextView as IVsUserData;
            if (userData == null)
            {
                Console.WriteLine("No text view is currently open");
                return;
            }
            object holder;
            Guid guidViewHost = DefGuidList.guidIWpfTextViewHost;
            userData.GetData(ref guidViewHost, out holder);
            var viewHost = (IWpfTextViewHost)holder;

            foreach (TRegion r in hEvent.regions)
            {
                Connector.Select(viewHost, r);
            }
            EditorController controler = EditorController.GetInstance();
            controler.CurrentViewCodeBefore = Connector.GetText(viewHost);
        }

        public void NotifyLocationsSelected(LocationEvent lEvent)
        {
            var locations = lEvent.locations;
            List<TRegion> negatives = new List<TRegion>();
            List<int> indexNegatives = new List<int>();
            List<TRegion> positives = new List<TRegion>();
            EditorController controller = EditorController.GetInstance();
            for (int index = 0; index < locations.Count; index++)
            {
                var location = locations[index];

                DialogResult dialogResult = MessageBox.Show(location.Region.Node.GetText() + "\n",
                    "Is it a correct location?", MessageBoxButtons.YesNo);
                TRegion parent = new TRegion();
                parent.Text = location.SourceCode;
                location.Region.Parent = parent;

                if (dialogResult == DialogResult.No)
                {
                    negatives.Add(location.Region);
                    indexNegatives.Add(index);
                    //break;
                }
                else
                {
                    positives.Add(location.Region);
                }
            }
            
            
            if (negatives.Any())
            {
                //controller.Extract(controller.SelectedLocations, negatives);
                controller.Extract(positives, negatives);
                JsonUtil<List<int>>.Write(indexNegatives, "negatives.json");
            }
            MessageBox.Show("Done!");

            controller.Done();
        }


        public void NotifyProgramGenerated(ProgramGeneratedEvent pEvent)
        {
            IVsTextManager txtMgr = (IVsTextManager)GetService(typeof(SVsTextManager));
            IVsTextView vTextView;
            const int mustHaveFocus = 1;
            txtMgr.GetActiveView(mustHaveFocus, null, out vTextView);

            IVsUserData userData = (IVsUserData) vTextView;
            
            IWpfTextViewHost viewHost;
            object holder;
            Guid guidViewHost = DefGuidList.guidIWpfTextViewHost;
            userData.GetData(ref guidViewHost, out holder);
            viewHost = (IWpfTextViewHost)holder;

            string text = Connector.GetText(viewHost);

            this._programs = pEvent.programs;

            EditorController controler = EditorController.GetInstance();

            controler.RetrieveLocations(text);
            controler.ProjectionBuffers = _CreateProjectionBuffers();
        }

        private Dictionary<string, IProjectionBuffer> _CreateProjectionBuffers()
        {
            EditorController controller = EditorController.GetInstance();
          var groupedLocation = RegionManager.GetInstance().GroupLocationsBySourceFile(controller.Locations);

            Dictionary<string, IProjectionBuffer> projectionBuffers = new Dictionary<string, IProjectionBuffer>();

            foreach (var item in groupedLocation)
            {
                var projectionBuffer = _CreateProjectionBuffer(item.Key, item.Value);
                if (projectionBuffer != null)
                {
                    projectionBuffers.Add(item.Key, projectionBuffer);
                }
            }
            return projectionBuffers;
        }

        private IProjectionBuffer _CreateProjectionBuffer(string document, List<CodeLocation> locations)
        {
            var rdt = (IVsRunningDocumentTable)GetService(typeof(SVsRunningDocumentTable));
            IEnumRunningDocuments value;
            rdt.GetRunningDocumentsEnum(out value);

            IVsHierarchy hierarchy;
            uint pitemid;
            IntPtr ppunkDocData;
            uint pdwCookie;
            rdt.FindAndLockDocument((uint)_VSRDTFLAGS.RDT_CantSave, document, out hierarchy, out pitemid,
                out ppunkDocData, out pdwCookie);

            string pbstrMkDocument;
            uint pwdReadLooks, pwdEditLocks, pgrfRDTFlags;
            var y = rdt.GetDocumentInfo(pdwCookie, out pgrfRDTFlags, out pwdReadLooks, out pwdEditLocks,
                out pbstrMkDocument, out hierarchy, out pitemid, out ppunkDocData);

            try
            {
                IVsTextBuffer x = Marshal.GetObjectForIUnknown(ppunkDocData) as IVsTextBuffer;

                IComponentModel componentModel = GetGlobalService(typeof (SComponentModel)) as IComponentModel;
                IVsEditorAdaptersFactoryService bufferData =
                    componentModel.GetService<IVsEditorAdaptersFactoryService>();
                IServiceProvider sp = GetGlobalService(
                    typeof (IServiceProvider))
                    as IServiceProvider;

                var textBuffer = bufferData.GetDataBuffer(x);
                var projectionBuffer = _CreateProjectionBuffer(textBuffer, locations);
                return projectionBuffer;
            }
            catch (Exception e)
            {
                Console.WriteLine("Document not found on project: " + e.Message);
            }
            return null;
        }

        public IProjectionBuffer CreateProjectionBuffer(IWpfTextViewHost host)
        {
            //retrieve start and end position that we saved in MyToolWindow.CreateEditor()
            var textView = host.TextView;

            List<object> sourceSpans = new List<object>();
            foreach (var location in EditorController.GetInstance().Locations)
            {

                var startPosition = location.Region.Start;
                var length = location.Region.Length;
                if (startPosition != 0)
                {
                    startPosition -= 1;
                    length = Math.Min(length + 2, textView.TextBuffer.CurrentSnapshot.Length);
                }
                else
                {
                    length = Math.Min(length + 1, textView.TextBuffer.CurrentSnapshot.Length);
                }

                //Take a snapshot of the text within these indices.
                var textSnapshot = textView.TextBuffer.CurrentSnapshot;
                var trackingSpan = textSnapshot.CreateTrackingSpan(startPosition, length, SpanTrackingMode.EdgeExclusive);
                sourceSpans.Add(trackingSpan);
            }

            //var ProjectionBufferFactory = (IProjectionBufferFactoryService) GetService(typeof(IProjectionBufferFactoryService));
            IComponentModel componentModel = GetGlobalService(typeof(SComponentModel)) as IComponentModel;
            IProjectionBufferFactoryService ProjectionBufferFactory = componentModel.GetService<IProjectionBufferFactoryService>();

            //Create the actual projection buffer
            var projectionBuffer = ProjectionBufferFactory.CreateProjectionBuffer(
                null
                , sourceSpans
                , ProjectionBufferOptions.None
                );
            return projectionBuffer;
        }

        private IProjectionBuffer _CreateProjectionBuffer(ITextBuffer textBuffer, List<CodeLocation> locations)
        {
            List<object> sourceSpans = new List<object>();
            foreach (var location in locations)
            {
                var startPosition = location.Region.Start;
                var length = location.Region.Length;
                if (startPosition != 0)
                {
                    startPosition -= 1;
                    length = Math.Min(length + 2, textBuffer.CurrentSnapshot.Length);
                }
                else
                {
                    length = Math.Min(length + 1, textBuffer.CurrentSnapshot.Length);
                }

                //Take a snapshot of the text within these indices.
                var textSnapshot = textBuffer.CurrentSnapshot;
                var trackingSpan = textSnapshot.CreateTrackingSpan(startPosition, length, SpanTrackingMode.EdgeExclusive);
                sourceSpans.Add(trackingSpan);
            }

            //var ProjectionBufferFactory = (IProjectionBufferFactoryService) GetService(typeof(IProjectionBufferFactoryService));
            IComponentModel componentModel = GetGlobalService(typeof(SComponentModel)) as IComponentModel;
            IProjectionBufferFactoryService ProjectionBufferFactory = componentModel.GetService<IProjectionBufferFactoryService>();

            //Create the actual projection buffer
            var projectionBuffer = ProjectionBufferFactory.CreateProjectionBuffer(
                null
                , sourceSpans
                , ProjectionBufferOptions.None
                );
            return projectionBuffer;
        }


        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                // Create the command for the menu item.
                CommandID menuCommandId = new CommandID(GuidList.guidIntelliExtractCmdSet, (int)PkgCmdIDList.cmdidExtract);
                MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandId);
                mcs.AddCommand(menuItem);
            }
        }
        #endregion

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            IVsTextManager txtMgr = (IVsTextManager)GetService(typeof(SVsTextManager));
            IVsTextView vTextView = null;
            const int mustHaveFocus = 1;
            txtMgr.GetActiveView(mustHaveFocus, null, out vTextView);

            IVsUserData userData = vTextView as IVsUserData;
            if (userData == null)
            {
                Console.WriteLine("No text view is currently open");
                return;
            }
            object holder;
            Guid guidViewHost = DefGuidList.guidIWpfTextViewHost;
            userData.GetData(ref guidViewHost, out holder);
            var viewHost = (IWpfTextViewHost)holder;

            EditorController controller = EditorController.GetInstance();
            controller.CurrentViewCodeBefore = Connector.GetText(viewHost);
            controller.Extract();
        }

    }
}
