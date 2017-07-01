using System;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Text.Editor;
using EnvDTE;
using Microsoft.VisualStudio.Text;
using Controller;

namespace RefazerUI
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class Transform: IEditStartedObserver
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 4129;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("cafc9643-b52b-4c58-85d9-fa70ad0b3678");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transform"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private Transform(Package package)
        {
            this.package = package;
            OleMenuCommandService commandService = this.Provider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }

            var controller = RefazerCC.GetInstance();
            controller.AddEditStartedObserver(this);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static Transform Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider Provider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new Transform(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            IVsTextManager txtMgr = (IVsTextManager)Provider.GetService(typeof(SVsTextManager));
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
            dte = (DTE)Provider.GetService(typeof(DTE)); // we have access to GetService here.
            string fullName = dte.Solution.FullName;
            var document = dte.ActiveDocument;
            string after = GetText(viewHost);

            var proj = dte.Solution.FindProjectItem(document.FullName);
            //throw new Exception("Error");
            var project = proj.ContainingProject;

            var controller = RefazerCC.GetInstance();
            controller.Transform(after);
            EnableTransformCommand(package, false);
        }

        public void EnableTransformCommand(Package package, bool flag)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }
            OleMenuCommandService commandService = Provider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                //var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                //commandService.AddCommand(menuItem);
                var menuItem = commandService.FindCommand(menuCommandID);
                menuItem.Enabled = flag;
            }
        }

        static public string GetText(IWpfTextViewHost host)
        {
            IWpfTextView view = host.TextView;

            ITextSnapshot document = view.TextSnapshot;
            return document.GetText();
        }

        public void EditStarted(EditStartedEvent @event)
        {
            EnableTransformCommand(package, true);
        }
    }
}
