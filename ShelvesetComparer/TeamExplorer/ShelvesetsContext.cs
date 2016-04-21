// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShelvesetsContext.cs" company="">
//
// </copyright>
// <summary>
//   The class provides the place holder for storing shelveset information in the Shelveset Comparer team explorer
//   window.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tfs.ShelvesetComparer.TeamExplorer
{
    using System.Collections.ObjectModel;
    using Microsoft.TeamFoundation.VersionControl.Client;

    /// <summary>
    ///     The class provides the place holder for storing shelveset information in the Shelveset Comparer team explorer
    ///     window.
    /// </summary>
    internal class ShelvesetsContext
    {
        /// <summary>
        ///     Gets or sets the list of Shelveset.
        /// </summary>
        public ObservableCollection<Shelveset> Shelvesets { get; set; }
    }
}