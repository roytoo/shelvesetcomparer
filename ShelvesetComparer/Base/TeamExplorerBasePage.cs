/*
    <copyright file="TeamExplorerBasePage.cs" company="http://shelvesetcomparer.codeplex.com">
        Copyright http://shelvesetcomparer.codeplex.com. All Rights Reserved.
        This code released under the terms of the Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
        This is sample code only, do not use in production environments.
    </copyright>
 */

namespace Tfs.ShelvesetComparer.Base
{
    using System;
    using Microsoft.TeamFoundation.Controls;

    /// <summary>
    ///     Team Explorer page base class.
    /// </summary>
    public class TeamExplorerBasePage : TeamExplorerBase, ITeamExplorerPage
    {
        #region [Fields]

        /// <summary>
        /// The title.
        /// </summary>
        private string _title;

        /// <summary>
        /// The is busy.
        /// </summary>
        private bool _isBusy;

        /// <summary>
        /// The page content.
        /// </summary>
        private object _pageContent;

        #endregion

        #region [Properties]

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get
            {
                return this._title;
            }

            set
            {
                this._title = value;
                this.RaisePropertyChanged(nameof(Title));
            }
        }

        /// <summary>
        /// Gets or sets the content of the page.
        /// </summary>
        public object PageContent
        {
            get
            {
                return this._pageContent;
            }

            set
            {
                this._pageContent = value;
                this.RaisePropertyChanged(nameof(PageContent));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is busy.
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return this._isBusy;
            }

            set
            {
                this._isBusy = value;
                this.RaisePropertyChanged(nameof(IsBusy));
            }
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Initializes the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PageInitializeEventArgs"/> instance containing the event data.</param>
        public virtual void Initialize(object sender, PageInitializeEventArgs e)
        {
            if (e != null)
            {
                this.ServiceProvider = e.ServiceProvider;
            }
        }

        /// <summary>
        /// Loaded event callback.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PageLoadedEventArgs"/> instance containing the event data.</param>
        public virtual void Loaded(object sender, PageLoadedEventArgs e)
        {
        }

        /// <summary>
        /// Saves the context.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PageSaveContextEventArgs"/> instance containing the event data.</param>
        public virtual void SaveContext(object sender, PageSaveContextEventArgs e)
        {
        }

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        public virtual void Refresh()
        {
        }

        /// <summary>
        /// Cancels this instance.
        /// </summary>
        public virtual void Cancel()
        {
        }

        /// <summary>
        /// Gets the extensibility service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        public virtual object GetExtensibilityService(Type serviceType)
        {
            return null;
        }

        #endregion
    }
}