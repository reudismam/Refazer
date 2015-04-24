using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using EnvDTE;
using LocateAdornment;
using Spg.LocationCodeRefactoring.Controller;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace SPG.IntelliLocation
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
    [Guid(GuidList.guidIntelliLocationPkgString)]
    public sealed class IntelliLocationPackage : Package
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>


        public IntelliLocationPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
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
            Debug.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if ( null != mcs )
            {
                // Create the command for the menu item.
                CommandID menuCommandID = new CommandID(GuidList.guidIntelliLocationCmdSet, (int)PkgCmdIDList.cmdidLocate);
                MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandID );
                mcs.AddCommand( menuItem );
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
            //    // Show a Message Box to prove we were here
            //    IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
            //    Guid clsid = Guid.Empty;
            //    int result;
            //    Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(uiShell.ShowMessageBox(
            //               0,
            //               ref clsid,
            //               "MenuCommandTest",
            //               string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.ToString()),
            //               string.Empty,
            //               0,
            //               OLEMSGBUTTON.OLEMSGBUTTON_OK,
            //               OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
            //               OLEMSGICON.OLEMSGICON_INFO,
            //               0,        // false
            //               out result));
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
            object holder;
            Guid guidViewHost = DefGuidList.guidIWpfTextViewHost;
            userData.GetData(ref guidViewHost, out holder);
            var viewHost = (IWpfTextViewHost)holder;

            DTE dte; 
            dte = (DTE)GetService(typeof(DTE)); // we have access to GetService here.
            string fullName = dte.Solution.FullName;
            var document = dte.ActiveDocument;

            var proj = dte.Solution.FindProjectItem(document.FullName);

            var project = proj.ContainingProject;

            EditorController.GetInstance().ProjectInformation.ProjectPath = project.Name;
            EditorController.GetInstance().ProjectInformation.SolutionPath = fullName;
            EditorController.GetInstance().CurrentViewCodePath = document.FullName;
            EditorController.GetInstance().FilesOpened[document.FullName] = true;

            Connector.Execute(viewHost);
        }

        //public void Nothing(string document)
        //{
        //    var rdt = (IVsRunningDocumentTable)GetService(typeof(SVsRunningDocumentTable));
        //    IEnumRunningDocuments value;
        //    rdt.GetRunningDocumentsEnum(out value);

        //    IVsHierarchy hierarchy;
        //    uint pitemid;
        //    IntPtr ppunkDocData;
        //    uint pdwCookie;
        //    rdt.FindAndLockDocument((uint)_VSRDTFLAGS.RDT_CantSave, document, out hierarchy, out pitemid,
        //        out ppunkDocData, out pdwCookie);

        //    string pbstrMkDocument;
        //    uint pwdReadLooks, pwdEditLocks, pgrfRDTFlags;
        //    var y = rdt.GetDocumentInfo(pdwCookie, out pgrfRDTFlags, out pwdReadLooks, out pwdEditLocks,
        //        out pbstrMkDocument, out hierarchy, out pitemid, out ppunkDocData);

        //    IVsTextBuffer x = Marshal.GetObjectForIUnknown(ppunkDocData) as IVsTextBuffer;

        //    //var bufferData = (IVsEditorAdaptersFactoryService)GetService(typeof(IVsEditorAdaptersFactoryService));
        //    IComponentModel componentModel = Package.GetGlobalService(typeof(SComponentModel)) as IComponentModel;
        //    IVsEditorAdaptersFactoryService bufferData = componentModel.GetService<IVsEditorAdaptersFactoryService>();
        //    Microsoft.VisualStudio.OLE.Interop.IServiceProvider sp = Package.GetGlobalService(
        //        typeof(Microsoft.VisualStudio.OLE.Interop.IServiceProvider))
        //        as Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

        //    var textBuffer = bufferData.GetDataBuffer(x);
        //    string text = textBuffer.CurrentSnapshot.GetText();
        //}

    }
}
