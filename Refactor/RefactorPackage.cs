using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using LocateAdornment;
using Spg.LocationRefactor.Controller;
using Spg.LocationRefactor.Location;
using Spg.LocationRefactor.Transformation;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Spg.LocationRefactor.Observer;
using DefGuidList = Microsoft.VisualStudio.Editor.DefGuidList;

namespace SPG.Refactor
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
    [Guid(GuidList.guidRefactorPkgString)]
    public sealed class RefactorPackage : Package, IProgramRefactoredObserver/*, ILocationsTransformedObserver*/
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public RefactorPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
            EditorController controller = EditorController.GetInstance();
            controller.AddProgramRefactoredObserver(this);
            //controller.AddLocationsTransformedObvserver(this);
        }

        /// <summary>
        /// Update view when a refactor occur.
        /// </summary>
        /// <param name="pEvent">Event</param>
        public void NotifyProgramRefactored(ProgramRefactoredEvent pEvent)
        {
            List<Transformation> transformations = pEvent.transformations;
            IVsTextManager txtMgr = (IVsTextManager)GetService(typeof(SVsTextManager));
            IVsTextView vTextView = null;
            int mustHaveFocus = 1;
            txtMgr.GetActiveView(mustHaveFocus, null, out vTextView);

            IVsUserData userData = vTextView as IVsUserData;
            if (userData == null)
            {
                Console.WriteLine("No text view is currently open");
                return;
            }
            IWpfTextViewHost viewHost;
            object holder;
            Guid guidViewHost = DefGuidList.guidIWpfTextViewHost;
            userData.GetData(ref guidViewHost, out holder);
            viewHost = (IWpfTextViewHost)holder;
            Connector.Update(viewHost, transformations);
        }

        private List<Tuple<string, string>> DocumentsBeforeAndAfter()
        {
            EditorController controller = EditorController.GetInstance();
            var groupedLocation = RegionManager.GetInstance().GroupLocationsBySourceFile(controller.Locations);

            List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();
            foreach (var item in groupedLocation)
            {
                string documentContent = CurrrentDocumentContent(item.Key);
                if (!documentContent.Equals(item.Value.First().SourceCode))
                {
                    Tuple<string, string> tuple = Tuple.Create(item.Value.First().SourceCode, documentContent);
                    tuples.Add(tuple);
                }
            }
            return tuples;
        }

        private string CurrrentDocumentContent(string document)
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

                //var bufferData = (IVsEditorAdaptersFactoryService)GetService(typeof(IVsEditorAdaptersFactoryService));
                IComponentModel componentModel = Package.GetGlobalService(typeof(SComponentModel)) as IComponentModel;
                IVsEditorAdaptersFactoryService bufferData =
                    componentModel.GetService<IVsEditorAdaptersFactoryService>();
                Microsoft.VisualStudio.OLE.Interop.IServiceProvider sp = Package.GetGlobalService(
                    typeof(Microsoft.VisualStudio.OLE.Interop.IServiceProvider))
                    as Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

                var textBuffer = bufferData.GetDataBuffer(x);
                string text = textBuffer.CurrentSnapshot.GetText();
                return text;
            }
            catch (Exception e)
            {
                string text = File.ReadAllText(document);
                return text;
            }
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
                CommandID menuCommandID = new CommandID(GuidList.guidRefactorCmdSet, (int)PkgCmdIDList.cmdidRefactor);
                MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
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
            int mustHaveFocus = 1;
            txtMgr.GetActiveView(mustHaveFocus, null, out vTextView);

            IVsUserData userData = vTextView as IVsUserData;
            if (userData == null)
            {
                Console.WriteLine("No text view is currently open");
                return;
            }
            IWpfTextViewHost viewHost;
            object holder;
            Guid guidViewHost = DefGuidList.guidIWpfTextViewHost;
            userData.GetData(ref guidViewHost, out holder);
            viewHost = (IWpfTextViewHost)holder;

            EditorController controler = EditorController.GetInstance();
            controler.CurrentViewCodeAfter = Connector.GetText(viewHost);

            EditorController controller = EditorController.GetInstance();
            controller.DocumentsBeforeAndAfter = DocumentsBeforeAndAfter();

            controler.Refact();
        }

        //public void NotifyLocationsTransformed(LocationsTransformedEvent ltEvent)
        //{
        //    List<CodeTransformation> transformations = ltEvent.transformations;
        //    foreach (var transformation in transformations)
        //    {
        //        System.IO.StreamWriter file = new System.IO.StreamWriter(transformation.location.SourceClass);
        //        file.WriteLine(transformation.transformation.Item2);

        //        file.Close();
        //    }
        //}
    }
}


