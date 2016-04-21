// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShelvesetComparerToolWindow.cs" company="http://shelvesetcomparer.codeplex.com">
//     Copyright http://shelvesetcomparer.codeplex.com. All Rights Reserved.
//     This code released under the terms of the Microsoft Public License(MS-PL, http://opensource.org/licenses/ms-pl.html.)
//     This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   This class implements the tool window exposed by this package and hosts a user control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace Tfs.ShelvesetComparer.Views
{
    /// <summary>
    ///     This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    [Guid("EEB0458C-6D7E-4CC9-AC5F-FA64866B4B44")]
    public sealed class ShelvesetComparerToolWindow : ToolWindowPane
    {
        /// <summary>
        ///     Initializes a new instance of the ShelvesetComparerToolWindow class.
        /// </summary>
        public ShelvesetComparerToolWindow() : base(null)
        {
            this.Caption = Resources.ToolWindowTitle;
            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;
            this.Content = new MainView();
        }
    }
}