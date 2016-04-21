namespace Tfs.ShelvesetComparer.TeamExplorer
{
    using System;
    using System.ComponentModel.Composition;
    using Microsoft.TeamFoundation.Controls;
    using Microsoft.VisualStudio.Shell;
    using Base;

    /// <summary>
    ///     The class creates the navigation link for Shelveset Comparer extension.
    /// </summary>
    [TeamExplorerNavigationLink(LinkId, TeamExplorerNavigationItemIds.MyWork, 1000)]
    public class ShelvesetComparerNavigationLink : TeamExplorerBaseNavigationLink
    {
        /// <summary>
        ///     The unique Id given to the navigation link
        /// </summary>
        public const string LinkId = "A1EC4AFD-FEBF-499B-82F7-E51F987D30D2";

        /// <summary>
        ///     Initializes a new instance of the ShelvesetComparerNavigationLink class.
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        [ImportingConstructor]
        public ShelvesetComparerNavigationLink([Import(typeof (SVsServiceProvider))] IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            this.Text = Resources.TeamExplorerLinkCaption;
            this.IsVisible = true;
            this.IsEnabled = true;
        }

        /// <summary>
        ///     Overridden method called when the navigation link is clicked.
        /// </summary>
        public override void Execute()
        {
            try
            {
                ITeamExplorer teamExplorer = this.GetService<ITeamExplorer>();
                teamExplorer?.NavigateToPage(new Guid(ShelvesetComparerPage.PageId), null);
            }
            catch (Exception ex)
            {
                this.ShowNotification(ex.Message, NotificationType.Error);
            }
        }

        /// <summary>
        ///     Overridden method called when the navigation link is refreshed.
        /// </summary>
        public override void Invalidate()
        {
            base.Invalidate();
            if (this.CurrentContext != null && this.CurrentContext.HasCollection && this.CurrentContext.HasTeamProject)
            {
                this.IsVisible = true;
            }
            else
            {
                this.IsVisible = false;
            }
        }
    }
}