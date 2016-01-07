using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Spg.ExampleRefactoring.Util;
using LocateAdornment;
using Spg.LocationRefactor.Location;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Projection;
using Microsoft.VisualStudio.TextManager.Interop;
using Spg.LocationRefactor.Controller;
using Spg.LocationRefactor.Observer;
using Spg.LocationRefactor.Location;
using Spg.LocationRefactor.TextRegion;
using DefGuidList = Microsoft.VisualStudio.Editor.DefGuidList;
using IServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using Spg.LocationRefactor.Program;

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
        //private List<Prog> _programs;

        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public IntelliExtractPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this));
            EditorController controler = EditorController.GetInstance();
            controler.AddProgramGeneratedObserver(this);
            controler.AddLocationsObserver(this);
            controler.AddHilightObserver(this);
        }

        public void NotifyHilightChanged(HighlightEvent hEvent)
        {
            IVsTextManager txtMgr = (IVsTextManager)GetService(typeof(SVsTextManager));
            IVsTextView vTextView;
            const int mustHaveFocus = 1;
            txtMgr.GetActiveView(mustHaveFocus, null, out vTextView);

            IVsUserData userData = vTextView as IVsUserData;
            if (userData == null)
            {
                Console.WriteLine(Resources.IntelliExtractPackage_NotifyHilightChanged_No_text_view_is_currently_open);
                return;
            }
            object holder;
            Guid guidViewHost = DefGuidList.guidIWpfTextViewHost;
            userData.GetData(ref guidViewHost, out holder);
            IWpfTextViewHost viewHost = (IWpfTextViewHost)holder;

            //ITextDocument docm = GetTextDocument(viewHost.TextView.TextBuffer);
            //string docName = docm.FilePath;

            EditorController controler = EditorController.GetInstance();
            foreach (CodeLocation r in hEvent.Regions)
            {
                //    if (r.SourceClass.ToUpperInvariant().Equals(docName.ToUpperInvariant()) || RegionManager.GetInstance().GroupRegionBySourceFile(controler.SelectedLocations).Count == 1)
                //    {
                Connector.Select(viewHost, r.Region);
                //    }
            }

            controler.CurrentViewCodeBefore = Connector.GetText(viewHost);
        }

        public static ITextDocument GetTextDocument(ITextBuffer textBuffer)
        {
            ITextDocument textDoc;
            var rc = textBuffer.Properties.TryGetProperty(
              typeof(ITextDocument), out textDoc);
            if (rc)
                return textDoc;

            return null;
        }

        public void NotifyLocationsSelected(LocationEvent lEvent)
        {
            var locations = lEvent.locations;
            List<TRegion> negatives = new List<TRegion>();
            List<int> indexNegatives = new List<int>();
            List<TRegion> positives = new List<TRegion>();
            EditorController controller = EditorController.GetInstance();

            DialogResult inforNeg = MessageBox.Show(Resources.IntelliExtractPackage_NotifyLocationsSelected_Do_you_want_to_inform_negative_locations_, Resources.IntelliExtractPackage_NotifyLocationsSelected_Do_you_want_to_inform_negative_locations_, MessageBoxButtons.YesNo);
            List<CodeLocation> previousLocations = null;

            for (int index = 0; index < locations.Count; index++)
            {
                var location = locations[index];
                bool isCorrectLocation = true;
                if (inforNeg == DialogResult.Yes)
                {
                    DialogResult dialogResult = MessageBox.Show(Resources.IntelliExtractPackage_NotifyLocationsSelected_class__ + location.SourceClass + "\n" + location.Region.Node.GetText() + "\n",
                        Resources.IntelliExtractPackage_NotifyLocationsSelected_Is_it_a_correct_location_, MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.No)
                    {
                        isCorrectLocation = false;
                    }
                }

                TRegion parent = new TRegion();
                parent.Text = location.SourceCode;
                location.Region.Parent = parent;

                if (!isCorrectLocation)
                {
                    negatives.Add(location.Region);
                    indexNegatives.Add(index);
                    //break;
                }
                else
                {
                    positives.Add(location.Region);
                }
                //}

                if (inforNeg == DialogResult.Yes)
                {
                    if (index % 10 == 0)
                    {
                        DialogResult contNeg = MessageBox.Show("Do you want to continue informing negative locations", Resources.IntelliExtractPackage_NotifyLocationsSelected_Do_you_want_to_inform_negative_locations_, MessageBoxButtons.YesNo);
                        if (contNeg == DialogResult.No)
                        {
                            previousLocations = locations;
                            break;
                        }
                    }
                }
            }

            if (negatives.Any())
            {
                //controller.Extract(controller.SelectedLocations, negatives);
                controller.Extract(controller.SelectedLocations, negatives);
                //JsonUtil<List<int>>.Write(indexNegatives, "negatives.json");
                SaveNegatives(negatives);
            }

            if (previousLocations != null)
            {
                negatives = new List<TRegion>();
                Prog prog = controller.Progs.First();
                foreach (var pLoc in previousLocations)
                {
                    bool isPresent = false;
                    foreach (var loc in controller.Locations)
                    {
                        if (pLoc.Region.Equals(loc.Region))
                        {
                            isPresent = true;
                            break;
                        }
                    }

                    if (!isPresent)
                    {
                        TRegion parent = new TRegion();
                        parent.Text = pLoc.SourceCode;
                        pLoc.Region.Parent = parent;
                        negatives.Add(pLoc.Region);
                    }
                }
                SaveNegatives(negatives);
            }

            MessageBox.Show(Resources.IntelliExtractPackage_NotifyLocationsSelected_Done_);

            controller.Done();
        }

        private void SaveNegatives(List<TRegion> negatives)
        {
            List<TRegion> tregions = new List<TRegion>();
            foreach (var item in negatives)
            {
                TRegion region = new TRegion();
                region.Start = item.Start;
                region.Length = item.Length;
                region.Path = item.Path;
                //region.Parent = item.Parent;
                region.Text = item.Text;
                tregions.Add(region);
            }

            JsonUtil<List<TRegion>>.Write(tregions, "negatives.json");

        }


        public void NotifyProgramGenerated(ProgramGeneratedEvent pEvent)
        {
            IVsTextManager txtMgr = (IVsTextManager)GetService(typeof(SVsTextManager));
            IVsTextView vTextView;
            const int mustHaveFocus = 1;
            txtMgr.GetActiveView(mustHaveFocus, null, out vTextView);

            IVsUserData userData = (IVsUserData)vTextView;

            IWpfTextViewHost viewHost;
            object holder;
            Guid guidViewHost = DefGuidList.guidIWpfTextViewHost;
            userData.GetData(ref guidViewHost, out holder);
            viewHost = (IWpfTextViewHost)holder;

            string text = Connector.GetText(viewHost);

            //this._programs = pEvent.programs;

            EditorController controler = EditorController.GetInstance();

            controler.RetrieveLocations();
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
            uint pwdReadLooks, pwdEditLocks, pgrfRdtFlags;
            rdt.GetDocumentInfo(pdwCookie, out pgrfRdtFlags, out pwdReadLooks, out pwdEditLocks,
                out pbstrMkDocument, out hierarchy, out pitemid, out ppunkDocData);

            try
            {
                IVsTextBuffer x = Marshal.GetObjectForIUnknown(ppunkDocData) as IVsTextBuffer;

                IComponentModel componentModel = GetGlobalService(typeof(SComponentModel)) as IComponentModel;
                IVsEditorAdaptersFactoryService bufferData =
                    componentModel.GetService<IVsEditorAdaptersFactoryService>();


                //IServiceProvider sp = GetGlobalService(
                //    typeof(IServiceProvider))
                //    as IServiceProvider;

                var textBuffer = bufferData.GetDataBuffer(x);
                var projectionBuffer = _CreateProjectionBuffer(textBuffer, locations);
                return projectionBuffer;
            }
            catch (Exception e)
            {
                Console.WriteLine(Resources.IntelliExtractPackage__CreateProjectionBuffer_Document_not_found_on_project__ + e.Message);
            }
            return null;
        }

        //public IProjectionBuffer CreateProjectionBuffer(IWpfTextViewHost host)
        //{
        //    //retrieve start and end position that we saved in MyToolWindow.CreateEditor()
        //    var textView = host.TextView;

        //    List<object> sourceSpans = new List<object>();
        //    foreach (var location in EditorController.GetInstance().Locations)
        //    {

        //        var startPosition = location.Region.Start;
        //        var length = location.Region.Length;
        //        if (startPosition != 0)
        //        {
        //            startPosition -= 1;
        //            length = Math.Min(length + 2, textView.TextBuffer.CurrentSnapshot.Length);
        //        }
        //        else
        //        {
        //            length = Math.Min(length + 1, textView.TextBuffer.CurrentSnapshot.Length);
        //        }

        //        //Take a snapshot of the text within these indices.
        //        var textSnapshot = textView.TextBuffer.CurrentSnapshot;
        //        var trackingSpan = textSnapshot.CreateTrackingSpan(startPosition, length, SpanTrackingMode.EdgeExclusive);
        //        sourceSpans.Add(trackingSpan);
        //    }

        //    //var ProjectionBufferFactory = (IProjectionBufferFactoryService) GetService(typeof(IProjectionBufferFactoryService));
        //    IComponentModel componentModel = GetGlobalService(typeof(SComponentModel)) as IComponentModel;
        //    IProjectionBufferFactoryService ProjectionBufferFactory = componentModel.GetService<IProjectionBufferFactoryService>();

        //    //Create the actual projection buffer
        //    var projectionBuffer = ProjectionBufferFactory.CreateProjectionBuffer(
        //        null
        //        , sourceSpans
        //        , ProjectionBufferOptions.None
        //        );
        //    return projectionBuffer;
        //}

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
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this));
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
                Console.WriteLine(Resources.IntelliExtractPackage_NotifyHilightChanged_No_text_view_is_currently_open);
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




