//------------------------------------------------------------------------------
// <copyright file="TransformPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using Microsoft.VisualStudio.TextManager.Interop;
using Spg.Controller;
using System.IO;
using Microsoft.VisualStudio.Text.Editor;
using EnvDTE;
using Microsoft.VisualStudio.Text;
using Controller;

namespace Transform
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(TransformPackage.PackageGuidString)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class TransformPackage : Package, IEditStartedObserver
    {

        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("1cca6c07-76d9-4f8b-a2a6-2e8115f24357");
        /// <summary>
        /// TransformPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "d160800c-ce11-4d86-bd1f-d1da407d1404";

        /// <summary>
        /// Initializes a new instance of the <see cref="Transform"/> class.
        /// </summary>
        public TransformPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
            EditorController controller = EditorController.GetInstance();
            controller.AddEditStartedObserver(this);
        }

        private void ConfigCallBackMethod(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }
            OleMenuCommandService commandService = this.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem); 
            }
        }

        private void EnableTransformCommand(Package package, bool flag)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }
            OleMenuCommandService commandService = this.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                //var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                //commandService.AddCommand(menuItem);
                var menuItem = commandService.FindCommand(menuCommandID);
                menuItem.Enabled = flag;
            }
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            ConfigCallBackMethod(this);
            EnableTransformCommand(this, false);
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            //Init.Initialize(this);
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
            Guid guidViewHost = Microsoft.VisualStudio.Editor.DefGuidList.guidIWpfTextViewHost;
            userData.GetData(ref guidViewHost, out holder);
            var viewHost = (IWpfTextViewHost)holder;

            DTE dte;
            dte = (DTE)GetService(typeof(DTE)); // we have access to GetService here.
            string fullName = dte.Solution.FullName;
            var document = dte.ActiveDocument;
            string after = GetText(viewHost);

            var proj = dte.Solution.FindProjectItem(document.FullName);
            //throw new Exception("Error");
            var project = proj.ContainingProject;

            EditorController controller = EditorController.GetInstance();
            controller.Transform(after);

            base.Initialize();
        }

        static public string GetText(IWpfTextViewHost host)
        {
            IWpfTextView view = host.TextView;

            ITextSnapshot document = view.TextSnapshot;
            return document.GetText();
        }

        public void EditStarted(EditStartedEvent @event)
        {
            EnableTransformCommand(this, true);
        }

        #endregion
    }
}
