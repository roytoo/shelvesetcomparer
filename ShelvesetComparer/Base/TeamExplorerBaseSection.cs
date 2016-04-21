/*
    <copyright file="TeamExplorerBaseSection.cs" company="http://shelvesetcomparer.codeplex.com">
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
    ///     Team Explorer base section class.
    /// </summary>
    public class TeamExplorerBaseSection : TeamExplorerBase, ITeamExplorerSection
    {
        #region [Fields]
        /// <summary>
        /// The item title.
        /// </summary>
        private string title;

        /// <summary>
        /// The item is expanded.
        /// </summary>
        private bool isExpanded = true;

        /// <summary>
        /// The item is visible.
        /// </summary>
        private bool isVisible = true;

        /// <summary>
        /// The item is busy.
        /// </summary>
        private bool isBusy;

        /// <summary>
        /// The item section content.
        /// </summary>
        private object sectionContent;

        #endregion

        #region [Properties]

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.title = value;
                this.RaisePropertyChanged("Title");
            }
        }

        /// <summary>
        /// Gets or sets the content of the section.
        /// </summary>
        public object SectionContent
        {
            get
            {
                return this.sectionContent;
            }

            set
            {
                this.sectionContent = value;
                this.RaisePropertyChanged("SectionContent");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is visible.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }

            set
            {
                this.isVisible = value;
                this.RaisePropertyChanged("IsVisible");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return this.isExpanded;
            }

            set
            {
                this.isExpanded = value;
                this.RaisePropertyChanged("IsExpanded");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is busy.
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }

            set
            {
                this.isBusy = value;
                this.RaisePropertyChanged("IsBusy");
            }
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Initializes the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SectionInitializeEventArgs"/> instance containing the event data.</param>
        public virtual void Initialize(object sender, SectionInitializeEventArgs e)
        {
            if (e != null)
            {
                this.ServiceProvider = e.ServiceProvider;
            }
        }

        /// <summary>
        /// Saves the context.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SectionSaveContextEventArgs"/> instance containing the event data.</param>
        public virtual void SaveContext(object sender, SectionSaveContextEventArgs e)
        {
        }

        /// <summary>
        /// Loaded event handler callback.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SectionLoadedEventArgs"/> instance containing the event data.</param>
        public virtual void Loaded(object sender, SectionLoadedEventArgs e)
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
        /// <returns>just null</returns>
        public virtual object GetExtensibilityService(Type serviceType)
        {
            return null;
        }

        #endregion
    }
}