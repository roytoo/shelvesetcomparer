﻿namespace Tfs.ShelvesetComparer.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using EnvDTE;
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.VersionControl.Client;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    ///     The view model for Shelveset comparison view
    /// </summary>
    public class ShelvesetComparerViewModel : INotifyPropertyChanged
    {
        /// <summary>
        ///     The color used when the two files match
        /// </summary>
        private const string ColorMatchingFiles = "black";

        /// <summary>
        ///     The color used when the two files are different
        /// </summary>
        private const string ColorDifferentFiles = "red";

        /// <summary>
        ///     The color used when the two files do not have a corresponding match in the other shelveset.
        /// </summary>
        private const string ColorNoMatchingFile = "blue";

        /// <summary>
        ///     Static Instance Variable. A Singleton instance of view model is used to pass information between tool explorer
        ///     window and main view.
        /// </summary>
        private static ShelvesetComparerViewModel instance;

        /// <summary>
        ///     The summary text message for comparison
        /// </summary>
        private string summaryText;

        /// <summary>
        ///     The total number of files.
        /// </summary>
        private int totalNumberOfFiles;

        /// <summary>
        ///     The total number of matching files.
        /// </summary>
        private int numberOfMatchingFiles;

        /// <summary>
        ///     The total number of different files.
        /// </summary>
        private int numberOfDifferentFiles;

        /// <summary>
        ///     The service provider
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        ///     First Shelveset Name
        /// </summary>
        private string firstShelvesetName;

        /// <summary>
        ///     Second shelveset name
        /// </summary>
        private string secondShelvesetName;

        /// <summary>
        ///     The collection of files
        /// </summary>
        private readonly ObservableCollection<FileComparisonViewModel> files;

        /// <summary>
        ///     The filter of files
        /// </summary>
        private string filter;

        /// <summary>
        ///     Initializes a new instance of the ShelvesetComparerViewModel class
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        public ShelvesetComparerViewModel([Import(typeof (SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            this.files = new ObservableCollection<FileComparisonViewModel>();
            this.serviceProvider = serviceProvider;
            this.summaryText = string.Empty;
            this.totalNumberOfFiles = 0;
            this.numberOfMatchingFiles = 0;
            this.numberOfDifferentFiles = 0;
            this.firstShelvesetName = string.Empty;
            this.secondShelvesetName = string.Empty;
            this.filter = string.Empty;
        }

        /// <summary>
        ///     Notification event used by view to update itself when any property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Gets the single instance of the View Model
        /// </summary>
        public static ShelvesetComparerViewModel Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                var dte2 = Package.GetGlobalService(typeof (DTE)) as EnvDTE80.DTE2;

                var provider = new ServiceProvider(dte2.DTE as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
                instance = new ShelvesetComparerViewModel(provider);

                return instance;
            }
        }

        /// <summary>
        ///     Gets or sets the summary of the comparison.
        /// </summary>
        public string SummaryText
        {
            get
            {
                return this.summaryText;
            }

            set
            {
                this.summaryText = value;
                this.NotifyPropertyChanged("SummaryText");
            }
        }

        /// <summary>
        ///     Gets or sets the filter for files to be shown
        /// </summary>
        public string Filter
        {
            get
            {
                return this.filter;
            }

            set
            {
                this.filter = value;
                this.NotifyPropertyChanged("Files");
            }
        }

        /// <summary>
        ///     Gets or sets the total number of files
        /// </summary>
        public int TotalNumberOfFiles
        {
            get
            {
                return this.totalNumberOfFiles;
            }

            set
            {
                this.totalNumberOfFiles = value;
                this.NotifyPropertyChanged("TotalNumberOfFiles");
            }
        }

        /// <summary>
        ///     Gets or sets the number of matching files
        /// </summary>
        public int NumberOfMatchingFiles
        {
            get
            {
                return this.numberOfMatchingFiles;
            }

            set
            {
                this.numberOfMatchingFiles = value;
                this.NotifyPropertyChanged("NumberOfMatchingFiles");
            }
        }

        /// <summary>
        ///     Gets or sets the number of different files
        /// </summary>
        public int NumberOfDifferentFiles
        {
            get
            {
                return this.numberOfDifferentFiles;
            }

            set
            {
                this.numberOfDifferentFiles = value;
                this.NotifyPropertyChanged("NumberOfDifferentFiles");
            }
        }

        /// <summary>
        ///     Gets or sets the first shelveset name
        /// </summary>
        public string FirstShelvesetName
        {
            get
            {
                return this.firstShelvesetName;
            }

            set
            {
                this.firstShelvesetName = value;
                this.NotifyPropertyChanged("FirstShelvesetName");
            }
        }

        /// <summary>
        ///     Gets or sets the second shelveset name
        /// </summary>
        public string SecondShelvesetName
        {
            get
            {
                return this.secondShelvesetName;
            }

            set
            {
                this.secondShelvesetName = value;
                this.NotifyPropertyChanged("SecondShelvesetName");
            }
        }

        /// <summary>
        ///     Gets Files view model of all matching and different files.
        /// </summary>
        public IEnumerable<FileComparisonViewModel> Files
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.filter))
                {
                    return this.files;
                }

                return this.files.Where(s => HasMatchingFileName(s, this.filter));
            }
        }

        /// <summary>
        ///     Initializes the Shelveset Comparison View Model
        /// </summary>
        /// <param name="firstShelveset">The first shelveset.</param>
        /// <param name="secondShelveset">The second shelveset</param>
        public void Initialize(Shelveset firstShelveset, Shelveset secondShelveset)
        {
            if (firstShelveset == null)
            {
                throw new ArgumentNullException("firstShelveset");
            }

            if (secondShelveset == null)
            {
                throw new ArgumentNullException("secondShelveset");
            }

            var tfcontextManager = this.GetService<ITeamFoundationContextManager>();
            var vcs = tfcontextManager.CurrentContext.TeamProjectCollection.GetService<VersionControlServer>();
            if (vcs == null)
            {
                this.SummaryText = Resources.ConnectionErrorMessage;
                return;
            }

            this.FirstShelvesetName = firstShelveset.Name;
            this.SecondShelvesetName = secondShelveset.Name;

            this.files.Clear();
            var firstShelvesetChanges = vcs.QueryShelvedChanges(firstShelveset)[0].PendingChanges;
            var secondShelvesetChanges = vcs.QueryShelvedChanges(secondShelveset)[0].PendingChanges;
            var orderedCollection = new SortedList<string, FileComparisonViewModel>();

            var sameContentFileCount = 0;
            var commonFilesCount = 0;

            foreach (var pendingChange in firstShelvesetChanges)
            {
                var matchingFile = secondShelvesetChanges.FirstOrDefault(s => s.ItemId == pendingChange.ItemId) ??
                                   secondShelvesetChanges.FirstOrDefault(s => s.LocalOrServerItem == pendingChange.LocalOrServerItem);

                var sameContent = matchingFile != null && AreFilesInPendingChangesSame(pendingChange, matchingFile);
                var comparisonItem = new FileComparisonViewModel
                {
                    FirstFile = pendingChange,
                    SecondFile = matchingFile,
                    Color =
                        sameContent
                            ? ColorMatchingFiles
                            : (matchingFile != null) ? ColorDifferentFiles : ColorNoMatchingFile
                };

                orderedCollection.Add(pendingChange.LocalOrServerFolder + "/" + pendingChange.FileName, comparisonItem);
                if (sameContent)
                {
                    sameContentFileCount++;
                }

                if (matchingFile != null)
                {
                    commonFilesCount++;
                }
            }

            foreach (var pendingChange in secondShelvesetChanges)
            {
                if (orderedCollection.ContainsKey(pendingChange.LocalOrServerFolder + "/" + pendingChange.FileName))
                {
                    continue;
                }

                var isThereAreNamedFile = FindItemWithSameItemId(orderedCollection, pendingChange.ItemId);
                if (isThereAreNamedFile != null)
                {
                    continue;
                }

                var comparisonItem = new FileComparisonViewModel
                {
                    SecondFile = pendingChange,
                    Color = ColorNoMatchingFile
                };

                orderedCollection.Add(pendingChange.LocalOrServerFolder + "/" + pendingChange.FileName, comparisonItem);
            }

            foreach (var item in orderedCollection.Keys)
            {
                this.files.Add(orderedCollection[item]);
            }

            if (firstShelveset.Name == secondShelveset.Name && firstShelveset.OwnerName == secondShelveset.OwnerName)
            {
                this.SummaryText = Resources.SameShelvesetMessage;
                this.TotalNumberOfFiles = firstShelvesetChanges.Length;
                this.NumberOfDifferentFiles = 0;
                this.NumberOfMatchingFiles = firstShelvesetChanges.Length;
            }
            else
            {
                this.SummaryText = string.Format(CultureInfo.CurrentCulture, Resources.SummaryMessage, commonFilesCount,
                    sameContentFileCount, orderedCollection.Count - sameContentFileCount);
                this.TotalNumberOfFiles = commonFilesCount;
                this.NumberOfMatchingFiles = sameContentFileCount;
                this.NumberOfDifferentFiles = orderedCollection.Count - sameContentFileCount;
            }
        }

        /// <summary>
        ///     The method find a pending change item in the collection with the given item id.
        /// </summary>
        /// <param name="orderedCollection">The collection to find the pending change file in.</param>
        /// <param name="itemId">The item id</param>
        /// <returns>The pending change file if found. Null otherwise.</returns>
        private static FileComparisonViewModel FindItemWithSameItemId(
            SortedList<string, FileComparisonViewModel> orderedCollection, int itemId)
        {
            return orderedCollection.Keys.Select(key => orderedCollection[key])
                .FirstOrDefault(
                    item => (item.FirstFile?.ItemId == itemId) || (item.SecondFile?.ItemId == itemId));
        }

        /// <summary>
        ///     Compares two given files.
        /// </summary>
        /// <param name="firstFilePath">The first file path </param>
        /// <param name="secondFilePath">The second file path</param>
        /// <returns>True if the content of the files is the same. False otherwise</returns>
        private static bool FileCompare(string firstFilePath, string secondFilePath)
        {
            int file1Byte;
            int file2Byte;
            FileStream fs1 = null;
            FileStream fs2 = null;

            if (firstFilePath == secondFilePath)
            {
                return true;
            }

            try
            {
                fs1 = new FileStream(firstFilePath, FileMode.Open);
                fs2 = new FileStream(secondFilePath, FileMode.Open);

                do
                {
                    file1Byte = fs1.ReadByte();
                    file2Byte = fs2.ReadByte();
                } while ((file1Byte == file2Byte) && (file1Byte != -1));
            }
            finally
            {
                fs1?.Close();
                fs2?.Close();
            }

            return (file1Byte - file2Byte) == 0;
        }

        /// <summary>
        ///     Compares the contents of two given files.
        /// </summary>
        /// <param name="firstPendingChange">The first pending change file.</param>
        /// <param name="secondPendingChange">The second pending change file</param>
        /// <returns>True if the file contents are same. False otherwise.</returns>
        private static bool AreFilesInPendingChangesSame(PendingChange firstPendingChange,
            PendingChange secondPendingChange)
        {
            if (firstPendingChange != null && secondPendingChange != null &&
                firstPendingChange.ChangeType != ChangeType.Delete &&
                secondPendingChange.ChangeType != ChangeType.Delete)
            {
                string pendingChangeFileName = Path.GetTempFileName();
                firstPendingChange.DownloadShelvedFile(pendingChangeFileName);

                string matchingFileName = Path.GetTempFileName();
                secondPendingChange.DownloadShelvedFile(matchingFileName);
                return FileCompare(pendingChangeFileName, matchingFileName);
            }

            return false;
        }

        /// <summary>
        ///     Returns true or false depending upon whether the first or second file name starts with the given filter.
        /// </summary>
        /// <param name="fileComparisonViewModel">The file comparison object to looking into</param>
        /// <param name="filter">The filter</param>
        /// <returns>True if the name exists. False otherwise</returns>
        private static bool HasMatchingFileName(FileComparisonViewModel fileComparisonViewModel, string filter)
        {
            return
                fileComparisonViewModel.FirstFileDisplayName.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) >=
                0 ||
                fileComparisonViewModel.SecondFileDisplayName.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) >=
                0;
        }

        /// <summary>
        ///     Returns the service of the given type.
        /// </summary>
        /// <typeparam name="T">The type of service to get</typeparam>
        /// <returns>The service.</returns>
        private T GetService<T>()
        {
            if (this.serviceProvider != null)
            {
                return (T) this.serviceProvider.GetService(typeof (T));
            }

            return default(T);
        }

        /// <summary>
        ///     The method raise the Property Changed event for the given property
        /// </summary>
        /// <param name="propertyName">The property for which the event needs to be raised</param>
        private void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}