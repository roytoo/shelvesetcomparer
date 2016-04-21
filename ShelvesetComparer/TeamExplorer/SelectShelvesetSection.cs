﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectShelvesetSection.cs" company="http://shelvesetcomparer.codeplex.com">
//     Copyright http://shelvesetcomparer.codeplex.com. All Rights Reserved.
//     This code released under the terms of the Microsoft Public License(MS-PL, http://opensource.org/licenses/ms-pl.html.)
//     This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   The class creates the team explorer section for the Shelveset Comparer extension.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Tfs.ShelvesetComparer.TeamExplorer
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.Controls;
    using Microsoft.TeamFoundation.VersionControl.Client;
    using Base;

    /// <summary>
    ///     The class creates the team explorer section for the Shelveset Comparer extension.
    /// </summary>
    [TeamExplorerSection("1555C86B-9D88-4AA6-9B85-99D97710BD74", ShelvesetComparerPage.PageId, 20)]
    public class SelectShelvesetSection : TeamExplorerBaseSection
    {
        /// <summary>
        ///     Contains the shelveset list
        /// </summary>
        private ObservableCollection<Shelveset> shelvesets;

        /// <summary>
        ///     Initializes a new instance of the SelectShelvesetSection class.
        /// </summary>
        public SelectShelvesetSection()
        {
            this.Title = Resources.TeamExplorerLinkCaption;
            this.FirstUserAccountName = string.Empty;
            this.SecondUserAccountName = string.Empty;
            this.IsVisible = true;
            this.IsExpanded = true;
            this.IsBusy = false;
            this.shelvesets = new ObservableCollection<Shelveset>();
            this.SectionContent = new Views.SelectShelvesetTeamExplorerView(this);
        }

        /// <summary>
        ///     Gets or sets the user account name for first shelveset.
        /// </summary>
        public string FirstUserAccountName { get; set; }

        /// <summary>
        ///     Gets or sets the user account name for second shelveset.
        /// </summary>
        public string SecondUserAccountName { get; set; }

        /// <summary>
        ///     Gets or sets the shelveset list
        /// </summary>
        public ObservableCollection<Shelveset> Shelvesets
        {
            get
            {
                return this.shelvesets;
            }

            protected set
            {
                this.shelvesets = value;
                this.RaisePropertyChanged("Shelvesets");
                this.View.ListShelvesets.ItemsSource = this.shelvesets;
            }
        }

        /// <summary>
        ///     Gets Team Foundation Context of the Team Explorer window.
        /// </summary>
        public ITeamFoundationContext Context => this.CurrentContext;

        /// <summary>
        ///     Gets the view of the current Team Explorer section
        /// </summary>
        protected Views.SelectShelvesetTeamExplorerView View => this.SectionContent as Views.SelectShelvesetTeamExplorerView;

        /// <summary>
        ///     Overridden method that initializes the team explorer section
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event arguments</param>
        public override async void Initialize(object sender, SectionInitializeEventArgs e)
        {
            base.Initialize(sender, e);
            var sectionContext = e.Context as ShelvesetsContext;
            if (sectionContext != null)
            {
                var context = sectionContext;
                this.Shelvesets = context.Shelvesets;
            }
            else
            {
                await this.RefreshAsync();
            }
        }

        /// <summary>
        ///     Refresh override.
        /// </summary>
        public override async void Refresh()
        {
            base.Refresh();
            await this.RefreshAsync();
        }

        /// <summary>
        ///     Save the current state of the section
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event arguments</param>
        public override void SaveContext(object sender, SectionSaveContextEventArgs e)
        {
            base.SaveContext(sender, e);
            if (e != null)
            {
                e.Context = new ShelvesetsContext
                {
                    Shelvesets = this.Shelvesets
                };
            }
        }

        /// <summary>
        ///     Refresh the list of shelveset shelveset asynchronously.
        /// </summary>
        /// <returns>The Task doing the refresh. Needed for Async methods</returns>
        public async Task RefreshShelvesets()
        {
            var shelveSets = new ObservableCollection<Shelveset>();

            // Make the server call asynchronously to avoid blocking the UI
            await
                Task.Run(
                    () =>
                    {
                        FetchShevlesets(this.FirstUserAccountName, this.SecondUserAccountName, this.CurrentContext,
                            out shelveSets);
                    });

            this.Shelvesets = shelveSets;
        }

        /// <summary>
        ///     Opens up the shelveset details page for the given shelveset
        /// </summary>
        /// <param name="shelveset">The shelveset to be displayed.</param>
        public void ViewShelvesetDetails(Shelveset shelveset)
        {
            ITeamExplorer teamExplorer = this.GetService<ITeamExplorer>();
            teamExplorer?.NavigateToPage(new Guid(TeamExplorerPageIds.ShelvesetDetails), shelveset);
        }

        /// <summary>
        ///     the method is invoked when the context of the current team explorer window has changed.
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event arguments</param>
        protected override async void ContextChanged(object sender, ContextChangedEventArgs e)
        {
            base.ContextChanged(sender, e);

            // If the team project collection or team project changed, refresh the data for this section
            if (e.TeamProjectCollectionChanged || e.TeamProjectChanged)
            {
                await this.RefreshAsync();
            }
        }

        /// <summary>
        ///     Retrieves the shelveset list for the current user
        /// </summary>
        /// <param name="userName">The user name </param>
        /// <param name="secondUsername">The second user name </param>
        /// <param name="context">The Team foundation server context</param>
        /// <param name="shelveSets">The shelveset list to be returned</param>
        private static void FetchShevlesets(string userName, string secondUsername, ITeamFoundationContext context,
            out ObservableCollection<Shelveset> shelveSets)
        {
            shelveSets = new ObservableCollection<Shelveset>();
            if (context != null && context.HasCollection && context.HasTeamProject)
            {
                var vcs = context.TeamProjectCollection.GetService<VersionControlServer>();
                if (vcs != null)
                {
                    string user = string.IsNullOrWhiteSpace(userName) ? vcs.AuthorizedUser : userName;
                    foreach (var shelveSet in vcs.QueryShelvesets(null, user).OrderByDescending(s => s.CreationDate))
                    {
                        shelveSets.Add(shelveSet);
                    }

                    if (!string.IsNullOrWhiteSpace(secondUsername) && secondUsername != userName)
                    {
                        user = string.IsNullOrWhiteSpace(secondUsername) ? vcs.AuthorizedUser : secondUsername;
                        foreach (var shelveSet in vcs.QueryShelvesets(null, user).OrderByDescending(s => s.CreationDate)
                            )
                        {
                            shelveSets.Add(shelveSet);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Refresh the list of shelveset and comparison shelveset asynchronously.
        /// </summary>
        /// <returns>The Task doing the refresh. Needed for Async methods</returns>
        private async Task RefreshAsync()
        {
            try
            {
                this.IsBusy = true;

                await this.RefreshShelvesets();
            }
            catch (Exception ex)
            {
                this.ShowNotification(ex.Message, NotificationType.Error);
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}