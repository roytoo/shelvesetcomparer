// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TeamExplorerBaseNavigationItem.cs"company="http://shelvesetcomparer.codeplex.com">
//      Copyright http://shelvesetcomparer.codeplex.com. All Rights Reserved.
//      This code released under the terms of the Microsoft Public License(MS-PL, http://opensource.org/licenses/ms-pl.html.)
//      This is sample code only, do not use in production environments.
// </copyright>
// <summary>
//   Team Explorer base navigation item class.
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------

namespace Tfs.ShelvesetComparer.Base
{
    using System;
    using System.Drawing;
    using Microsoft.TeamFoundation.Controls;

    /// <summary>
    ///     Team Explorer base navigation item class.
    /// </summary>
    public class TeamExplorerBaseNavigationItem : TeamExplorerBase, ITeamExplorerNavigationItem
    {
        #region [Fields]

        /// <summary>
        /// The item visibility.
        /// </summary>
        private bool _isVisible = true;

        /// <summary>
        /// The item text.
        /// </summary>
        private string _text;

        /// <summary>
        /// The item glyph image.
        /// </summary>
        private Image _image;

        #endregion

        #region [Constructor]

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamExplorerBaseNavigationItem"/> class.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider.
        /// </param>
        public TeamExplorerBaseNavigationItem(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        #endregion

        #region [Properties]

        /// <summary>
        /// Gets or sets the item text.
        /// </summary>
        public string Text
        {
            get
            {
                return this._text;
            }

            set
            {
                this._text = value;
                this.RaisePropertyChanged("Text");
            }
        }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        public Image Image
        {
            get
            {
                return this._image;
            }

            set
            {
                this._image = value;
                this.RaisePropertyChanged("Image");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is visible.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return this._isVisible;
            }

            set
            {
                this._isVisible = value;
                this.RaisePropertyChanged("IsVisible");
            }
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Invalidates item.
        /// </summary>
        public virtual void Invalidate()
        {
        }

        /// <summary>
        /// Executes item action.
        /// </summary>
        public virtual void Execute()
        {
        }

        #endregion

    }
}