// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShelvesetComparerPage.cs" company="">
//
// </copyright>
// <summary>
//   The class creates the team explorer page for Shelveset Comparer extension.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tfs.ShelvesetComparer.TeamExplorer
{
    using Base;
    using Microsoft.TeamFoundation.Controls;

    /// <summary>
    ///     The class creates the team explorer page for Shelveset Comparer extension.
    /// </summary>
    [TeamExplorerPage(PageId)]
    public class ShelvesetComparerPage : TeamExplorerBasePage
    {
        /// <summary>
        ///     The unique id of the Team explorer page
        /// </summary>
        public const string PageId = "{9A984CF5-9D99-4C24-BDCB-E53A0D174343}";

        /// <summary>
        ///     Initializes a new instance of the ShelvesetComparerPage class
        /// </summary>
        public ShelvesetComparerPage()
        {
            this.Title = Resources.ToolWindowTitle;
        }
    }
}