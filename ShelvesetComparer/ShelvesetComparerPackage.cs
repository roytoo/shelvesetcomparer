namespace Tfs.ShelvesetComparer
{
    using System;
    using System.ComponentModel.Design;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using ViewModel;
    using Views;

    /// <summary>
    ///     This is the class that implements the package exposed by this assembly.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [ProvideAutoLoad("64BBB0A3-9E71-4053-980E-C2AECBA66714")]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof (ShelvesetComparerToolWindow))]
    [Guid("274263AE-8D63-4BC0-B8E8-85CD44B5676B")]
    public sealed class ShelvesetComparerPackage : Package
    {
        /// <summary>
        ///     The unique id of the Tools menu item for extension
        /// </summary>
        private const uint CommandIdShelvesetComparerMenu = 0x100;

        /// <summary>
        ///     The unique id of the Tool window for the extension
        /// </summary>
        private const uint CommandIdShelvesetComparerToolWindow = 0x101;

        /// <summary>
        ///     Initialization of the package; this method is called right after the package is sited, so this is the place
        ///     where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            ExtensionSettings.CreateInstance(this);
            var mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null == mcs)
            {
                return;
            }

            var menuCommandId = new CommandID(new Guid("EBC07634-7668-403D-9B3B-D6B6B50E307C"), (int)CommandIdShelvesetComparerMenu);
            var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandId);
            mcs.AddCommand(menuItem);

            var toolwndCommandId = new CommandID(new Guid("EBC07634-7668-403D-9B3B-D6B6B50E307C"), (int)CommandIdShelvesetComparerToolWindow);
            var menuToolWin = new MenuCommand(this.ShowToolWindow, toolwndCommandId);
            mcs.AddCommand(menuToolWin);
        }

        /// <summary>
        ///     This function is called when the user clicks the menu item that shows the tool window.
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">Event arguments</param>
        private void ShowToolWindow(object sender, EventArgs e)
        {
            ToolWindowPane window = this.FindToolWindow(typeof (ShelvesetComparerToolWindow), 0, true);
            if (window?.Frame == null)
            {
                throw new NotSupportedException(Resources.CanNotCreateWindow);
            }

            var windowFrame = (IVsWindowFrame) window.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }

        /// <summary>
        ///     This function is the callback used to execute a command when the a menu item is clicked.
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">Event arguments</param>
        /// ///
        private void MenuItemCallback(object sender, EventArgs e)
        {
            ToolWindowPane window = this.FindToolWindow(typeof (ShelvesetComparerToolWindow), 0, true);
            if (window?.Frame == null)
            {
                throw new NotSupportedException(Resources.NoWindowFound);
            }

            var windowFrame = (IVsWindowFrame) window.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
    }
}