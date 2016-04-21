// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileComparisonViewModel.cs" company="">
//
// </copyright>
// <summary>
//   The view model used for each file in the comparison grid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tfs.ShelvesetComparer.ViewModel
{
    using System.Globalization;
    using Microsoft.TeamFoundation.VersionControl.Client;

    /// <summary>
    ///     The view model used for each file in the comparison grid.
    /// </summary>
    public class FileComparisonViewModel
    {
        /// <summary>
        ///     Gets or sets the first pending change file
        /// </summary>
        public PendingChange FirstFile { get; set; }

        /// <summary>
        ///     Gets or sets the second pending change file
        /// </summary>
        public PendingChange SecondFile { get; set; }

        /// <summary>
        ///     Gets the display name of the first file
        /// </summary>
        public string FirstFileDisplayName
        {
            get { return GetFullFilePath(this.FirstFile); }
        }

        /// <summary>
        ///     Gets the display name of the second file
        /// </summary>
        public string SecondFileDisplayName
        {
            get { return GetFullFilePath(this.SecondFile); }
        }

        /// <summary>
        ///     Gets or sets the Color of text.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        ///     Returns the full file path of the pending change file.
        /// </summary>
        /// <param name="pendingChange">The pending change file</param>
        /// <returns>The full file path of the given pending change</returns>
        private static string GetFullFilePath(PendingChange pendingChange)
        {
            return (pendingChange != null)
                ? string.Format(CultureInfo.CurrentCulture, @"{0}/{1}", pendingChange.LocalOrServerFolder,
                    pendingChange.FileName)
                : string.Empty;
        }
    }
}